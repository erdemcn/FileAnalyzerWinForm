using System.Text;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using DocumentFormat.OpenXml.Packaging;
using Body = DocumentFormat.OpenXml.Wordprocessing.Body;


namespace FileAnalyzer
{
    public partial class FileAnalyzer : Form
    {
        private OpenFileDialog openFileDialog;

        public FileAnalyzer()
        {
            InitializeComponent();
            openFileDialog = new OpenFileDialog();
        }

        private void FileAnalyzer_Load(object sender, EventArgs e)
        {
            btnUpload.Enabled = false;
            comboBoxFileType.Items.AddRange(new object[] { "txt", "docx", "pdf" });
        }

        private void comboBoxFileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFileType.SelectedItem != null)
            {
                string selectedExtension = comboBoxFileType.SelectedItem.ToString().ToLower();

                if (selectedExtension == "txt")
                    openFileDialog.Filter = "Text Files (*.txt)|*.txt";
                else if (selectedExtension == "docx")
                    openFileDialog.Filter = "Word Files (*.docx)|*.docx";
                else if (selectedExtension == "pdf")
                    openFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";

                btnUpload.Enabled = true;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedFile = OpenFileDialogMethod();
                if (string.IsNullOrEmpty(selectedFile)) return;

                if (!IsValidFileType(selectedFile))
                {
                    MessageBox.Show("Geçersiz dosya türü seçildi.");
                    return;
                }

                progressBar.Value = 0;
                var analysisResult = AnalyzeFile(selectedFile);
                progressBar.Value = 100;

                TabPage newTab = new TabPage($"Analiz {tabControlResults.TabCount + 1}");
                RichTextBox newRichTextBox = new RichTextBox
                {
                    Dock = DockStyle.Fill,
                    ReadOnly = true
                };

                newRichTextBox.AppendText($"Dosya Adı: {selectedFile}\n");
                newRichTextBox.AppendText($"Toplam Kelime Sayısı: {analysisResult.TotalWordCount}\n");
                newRichTextBox.AppendText($"Toplam Farklı Kelime Sayısı: {analysisResult.DistinctWordCount}\n");
                newRichTextBox.AppendText($"Toplam Noktalama Sayısı: {analysisResult.PunctuationCount}\n\n");

                newRichTextBox.AppendText("En Çok Tekrar Eden Kelimeler:\n");
                foreach (var word in analysisResult.TopWords)
                {
                    newRichTextBox.AppendText($"{word.Key}: {word.Value} kez\n");
                }

                newTab.Controls.Add(newRichTextBox);
                tabControlResults.TabPages.Add(newTab);
                tabControlResults.SelectedTab = newTab;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }

        private string OpenFileDialogMethod()
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
            return string.Empty;
        }

        private bool IsValidFileType(string filePath)
        {
            string extension = System.IO.Path.GetExtension(filePath).ToLower();
            return extension == ".txt" || extension == ".docx" || extension == ".pdf";
        }

        private AnalysisResult AnalyzeFile(string filePath)
        {
            string content = string.Empty;
            string extension = System.IO.Path.GetExtension(filePath).ToLower();

            if (extension == ".txt")
            {
                content = File.ReadAllText(filePath);
            }
            else if (extension == ".docx")
            {
                content = ReadDocxContent(filePath);
            }
            else if (extension == ".pdf")
            {
                content = ReadPdfContent(filePath);
            }

            if (string.IsNullOrEmpty(content))
            {
                MessageBox.Show("Dosya içeriği okunamadı!");
                return null;
            }

            var wordCounts = AnalyzeWords(content);
            var punctuationCount = AnalyzePunctuation(content);

            return new AnalysisResult
            {
                TotalWordCount = wordCounts.Sum(w => w.Value),
                DistinctWordCount = wordCounts.Count,
                PunctuationCount = punctuationCount,
                TopWords = wordCounts.OrderByDescending(w => w.Value).Take(10).ToDictionary(x => x.Key, x => x.Value)
            };
        }

        private string ReadPdfContent(string filePath)
        {
            try
            {
                using (PdfReader reader = new PdfReader(filePath))
                {
                    StringBuilder text = new StringBuilder();

                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                        text.Append(" ");
                    }

                    return text.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"PDF dosyası okunurken hata oluştu: {ex.Message}");
                return string.Empty;
            }
        }

        private string ReadDocxContent(string filePath)
        {
            try
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
                {
                    Body body = wordDoc.MainDocumentPart.Document.Body;
                    return body.InnerText;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DOCX dosyası okunurken hata oluştu: {ex.Message}");
                return string.Empty;
            }
        }

        private Dictionary<string, int> AnalyzeWords(string content)
        {
            var words = Regex.Matches(content, @"\b\w+\b")
                             .Cast<Match>()
                             .Select(m => m.Value.ToLower())
                             .Where(w => !IsStopWord(w))
                             .ToList();

            var wordCount = words.GroupBy(w => w)
                                 .ToDictionary(g => g.Key, g => g.Count());

            return wordCount;
        }

        private bool IsStopWord(string word)
        {
            var stopWords = new HashSet<string> { "ve", "ama", "de", "da", "bir", "bu", "ile", "ya", "ya da" };
            return stopWords.Contains(word);
        }

        private int AnalyzePunctuation(string content)
        {
            var punctuationMarks = new[] { '.', ',', '!', '?', ';', ':', '-', '(', ')', '[', ']', '{', '}' };
            return content.Count(c => punctuationMarks.Contains(c));
        }

        public class AnalysisResult
        {
            public int TotalWordCount { get; set; }
            public int DistinctWordCount { get; set; }
            public int PunctuationCount { get; set; }
            public Dictionary<string, int> TopWords { get; set; }
        }
    }
}
