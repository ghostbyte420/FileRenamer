using System.Net.Http.Json;
using System.Text;                    
using System.Text.RegularExpressions; 
using System.Text.Json;   

namespace FileRenamer
{
    public partial class Main : Form
    {
        // One shared "phone line" to the internet, used for all translations.
        private static readonly HttpClient httpClient = new();

        // Remembers words we've already translated (so we don't re-ask).
        private readonly System.Collections.Concurrent.ConcurrentDictionary<string, string> wordCache = new();

        // Temporary spy: remembers the last error so we can see it.
        private string? lastError;

        // =====================================================================
        //  THE LANGUAGE LIST
        //  Left = name shown in dropdown.  Right = official 2-letter code.
        //  TO ADD A NEW LANGUAGE: add ONE line below. 👇👇👇
        //  Codes: https://en.wikipedia.org/wiki/List_of_ISO_639_language_codes
        // =====================================================================

        private static readonly Dictionary<string, string> LanguageCodes = new()
        {
            ["English"] = "en",
            ["German"] = "de",
            ["Spanish"] = "es",
            ["French"]  = "fr",
            ["Italian"] = "it",
            ["Portuguese"] = "pt",
            ["Russian"] = "ru",
            ["Mandarin"] = "zh-CN",
            ["Japanese"] = "ja",
            ["Korean"] = "ko",
            ["Arabic"] = "ar",
            ["Hindi"] = "hi",
            ["Dutch"] = "nl",
            ["Swedish"] = "sv",
            ["Turkish"] = "tr",
            ["Polish"] = "pl",
            ["Czech"] = "cs",
            ["Danish"] = "da",
            ["Finnish"] = "fi",
            ["Norwegian"] = "no"        
        };

        public Main()
        {
            InitializeComponent();

            // 🏷️ Tell the translator we're a normal browser (so it doesn't block us).
            if (!httpClient.DefaultRequestHeaders.Contains("User-Agent"))
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

            // 🔌 Connect the buttons to their code.
            main_button_searchDirectory.Click += main_button_searchDirectory_Click;
            main_button_translate.Click += main_button_translate_Click;

            // Fill both dropdowns with our language NAMES.
            foreach (string languageName in LanguageCodes.Keys)
            {
                main_comboBox_selectLang_from.Items.Add(languageName);
                main_comboBox_selectLang_to.Items.Add(languageName);
            }

            // Starting choices: FROM English -> TO German.
            main_comboBox_selectLang_to.SelectedItem = "German";
            main_comboBox_selectLang_from.SelectedItem = "English";      
        }

        // =====================================================================
        //  STEP 1: The "Select Directory" button
        // =====================================================================
        private void main_button_searchDirectory_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog folderPicker = new();
            if (folderPicker.ShowDialog() == DialogResult.OK)
            {
                main_textBox_directoryLocation.Text = folderPicker.SelectedPath;
            }
        }

        // =====================================================================
        //  STEP 3: The "Translate" button
        // =====================================================================
        private async void main_button_translate_Click(object sender, EventArgs e)
        {
            string folderPath = main_textBox_directoryLocation.Text;
            string fromLanguage = main_comboBox_selectLang_from.SelectedItem?.ToString() ?? "";
            string toLanguage = main_comboBox_selectLang_to.SelectedItem?.ToString() ?? "";

            if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath))
            {
                MessageBox.Show("Please pick a real folder first! 📁");
                return;
            }
            if (fromLanguage == toLanguage)
            {
                MessageBox.Show("The two languages are the same. Nothing to do! 🙂");
                return;
            }

            try
            {
                main_button_translate.Enabled = false;

                wordCache.Clear();   // 🧠 fresh memory each time (in case languages changed)

                // Make a list of EVERYTHING (deepest items first).
                List<(string path, bool isFolder)> items = GatherItems(folderPath);

                main_progressBar.Minimum = 0;
                main_progressBar.Maximum = Math.Max(items.Count, 1);
                main_progressBar.Value = 0;

                // A thread-safe box to store each item's new name.
                var newNames = new System.Collections.Concurrent.ConcurrentDictionary<string, string>();

                // 📊 A safe way to update the bar/label from background work.
                //    (Progress<T> always runs this on the UI thread for us.)
                IProgress<string> reporter = new Progress<string>(currentName =>
                {
                    if (main_progressBar.Value < main_progressBar.Maximum)
                        main_progressBar.Value++;
                    main_label_status.Text = "Translating: " + currentName;
                });

                // ===== PHASE 1: translate ALL names at once (5 at a time) =====
                var options = new ParallelOptions { MaxDegreeOfParallelism = 2 };
                await Parallel.ForEachAsync(items, options, async (item, cancelToken) =>
                {
                    string name = Path.GetFileName(item.path);
                    string extension = item.isFolder ? "" : Path.GetExtension(name);
                    string nameNoExtension = item.isFolder ? name : Path.GetFileNameWithoutExtension(name);

                    string translated = await TranslateTextAsync(nameNoExtension, fromLanguage, toLanguage);

                    newNames[item.path] = translated + extension;   // remember the new name
                    reporter.Report(name);                          // move the bar + label
                });

                #region Deug: tells us what's really happening...
                /*
                 
                var firstItem = items[0];
                MessageBox.Show(
                    $"FROM: {fromLanguage}   TO: {toLanguage}\n\n" +
                    $"Old name: {Path.GetFileName(firstItem.path)}\n" +
                    $"New name: {newNames[firstItem.path]}\n\n" +
                    $"Last error: {(lastError ?? "none")}");

                */
                #endregion

                // ===== PHASE 2: rename on disk, deepest-first (safe, one at a time) =====
                main_label_status.Text = "Renaming...";
                foreach (var (path, isFolder) in items)
                {
                    string parentFolder = Path.GetDirectoryName(path)!;
                    string oldName = Path.GetFileName(path);
                    string newName = newNames[path];

                    if (newName == oldName) continue;   // nothing changed

                    string newFullPath = Path.Combine(parentFolder, newName);

                    if (isFolder)
                        Directory.Move(path, newFullPath);
                    else
                        File.Move(path, newFullPath);
                }

                main_label_status.Text = "All done! ✅";
                MessageBox.Show("All done! ✅");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Oops, something went wrong: " + ex.Message);
            }
            finally
            {
                main_button_translate.Enabled = true;
            }
        } 

        // ---------------------------------------------------------------------
        //  Builds a list of every file & folder, DEEPEST FIRST, so renaming
        //  a folder never breaks the paths of things still waiting inside it.
        // ---------------------------------------------------------------------
        private List<(string path, bool isFolder)> GatherItems(string folderPath)
        {
            List<(string, bool)> items = new();

            // Files in this folder.
            foreach (string file in Directory.GetFiles(folderPath))
                items.Add((file, false));

            // Each subfolder: add ITS contents first, then the subfolder itself.
            foreach (string subFolder in Directory.GetDirectories(folderPath))
            {
                items.AddRange(GatherItems(subFolder));   // deeper first
                items.Add((subFolder, true));
            }

            return items;
        }

        // ---------------------------------------------------------------------
        //  Renames ONE file or folder.
        // ---------------------------------------------------------------------
        private async Task RenameOneItemAsync(string fullPath, string fromLanguage, string toLanguage, bool isFolder)
        {
            string parentFolder = Path.GetDirectoryName(fullPath)!;
            string name = Path.GetFileName(fullPath);

            // For files, keep the extension (.txt, .jpg) UNCHANGED.
            string extension = isFolder ? "" : Path.GetExtension(name);
            string nameNoExtension = isFolder ? name : Path.GetFileNameWithoutExtension(name);

            string translatedName = await TranslateTextAsync(nameNoExtension, fromLanguage, toLanguage);

            if (translatedName == nameNoExtension) return;   // nothing changed

            string newFullPath = Path.Combine(parentFolder, translatedName + extension);

            if (isFolder)
                Directory.Move(fullPath, newFullPath);
            else
                File.Move(fullPath, newFullPath);
        }

        // ---------------------------------------------------------------------
        //  🌐 Asks the free online translator. If it fails, keep the original.
        // ---------------------------------------------------------------------

        // ---------------------------------------------------------------------
        //  Splits a name into pieces and translates ONLY the real-word pieces,
        //  keeping numbers, underscores, dashes, and short codes in place.
        //  Example: "7w_axt13"  ->  "7w_axe13"
        // ---------------------------------------------------------------------
        private async Task<string> TranslateTextAsync(string text, string fromLanguage, string toLanguage)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;

            // Split into pieces but KEEP the separators (the "(...)" keeps them).
            string[] pieces = Regex.Split(text, "([^A-Za-z]+)");
            StringBuilder builder = new();

            foreach (string piece in pieces)
            {
                // Only translate pieces that are 2+ letters (real words).
                if (piece.Length >= 2 && piece.All(char.IsLetter))
                    builder.Append(await TranslateWordAsync(piece, fromLanguage, toLanguage));
                else
                    builder.Append(piece);   // keep numbers/underscores/codes as-is
            }

            return MakeSafeFileName(builder.ToString());
        }

        // ---------------------------------------------------------------------
        //  Translates ONE word, using our 🧠 memory box so we never ask twice.
        // ---------------------------------------------------------------------

        private async Task<string> TranslateWordAsync(string word, string fromLanguage, string toLanguage)
        {
            string key = word.ToLowerInvariant();
            if (wordCache.TryGetValue(key, out string? cached))
                return cached;

            string fromCode = LanguageCodes[fromLanguage];
            string toCode = LanguageCodes[toLanguage];

            // 📧 Put YOUR real email here — this raises the free limit to ~50,000/day.
            string myEmail = "aasr.opc@proton.me";

            string url = $"https://api.mymemory.translated.net/get" +
                         $"?q={Uri.EscapeDataString(word)}&langpair={fromCode}|{toCode}" +
                         $"&de={Uri.EscapeDataString(myEmail)}";

            string result = word;   // default: keep original if anything fails
            try
            {
                MyMemoryResponse? answer = await httpClient.GetFromJsonAsync<MyMemoryResponse>(url);
                string? translated = answer?.responseData?.translatedText;

                // Accept only a REAL translation (not a warning/limit message).
                bool isWarning =
                    string.IsNullOrWhiteSpace(translated) ||
                    translated!.Contains("MYMEMORY WARNING", StringComparison.OrdinalIgnoreCase) ||
                    translated.Contains("QUOTA", StringComparison.OrdinalIgnoreCase) ||
                    translated.Contains("INVALID", StringComparison.OrdinalIgnoreCase);

                if (!isWarning)
                    result = translated;
                else
                    lastError = translated;   // 🔦 remember what it said
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
            }

            await Task.Delay(150);   // 😴 tiny pause so we stay friendly

            wordCache[key] = result;
            return result;
        }

        // ---------------------------------------------------------------------
        //  Removes characters not allowed in file/folder names.
        // ---------------------------------------------------------------------
        private static string MakeSafeFileName(string name)
        {
            foreach (char badChar in Path.GetInvalidFileNameChars())
                name = name.Replace(badChar, '_');
            return name.Trim();
        }

        // ---------------------------------------------------------------------
        //  Match the shape of the translator's JSON answer.
        // ---------------------------------------------------------------------
        private class MyMemoryResponse
        {
            public ResponseData? responseData { get; set; }
        }

        private class ResponseData
        {
            public string? translatedText { get; set; }
        }
    }
}