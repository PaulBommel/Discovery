using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace Discovery.Delivery
{
    public readonly record struct TemporaryWordsFile : IDisposable
    {
        public TemporaryWordsFile(IEnumerable<string> words, string fileName, TesseractEngine engine)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName));
            FileName = fileName;
            File.WriteAllLines(FileName, words);
            engine.SetVariable("user_words_suffix", FileName);
        }

        public string FileName { get; }

        public void Dispose()
        {
            if(File.Exists(FileName))
                File.Delete(FileName);
        }
    }
}
