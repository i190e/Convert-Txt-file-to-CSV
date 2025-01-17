using System.Text;
using System;
using System.IO;
using System.Data;
using System.Linq.Expressions;
using System.Diagnostics.Metrics;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Specialized;

namespace WordsCombinations
{
    public class Functions
    {
        private string TxtPath;
        private string CsvPath;
        private string CompareCsvPath;
        private char[] splitChars;
        private string[] russianAlphabet;
        private string[] englishAlphabet;
        private string[] latinAlphabet;
        private string[] LatinNotOffical;
        private string[] QwertyAlphabetEn;
        private string[] QwertyAlphabetRu;

        //проверку на заполненость пути файла в функциях (без диалога actionSelection()) чтобы работало без этой фйнкции как .dll

        protected string ReadLinePath(string? msg) //Console Function ReadLine (PathToFileName) With Dialog And Exeption
        {
            string? tmp;
            try
            {
                if (msg == "" || msg == null)
                {
                    msg = "Enter Path to Filename";
                }
                Console.WriteLine(msg);

                tmp = Console.ReadLine();
                if (tmp != null)
                {
                    return tmp;
                }
                else Console.WriteLine("Error Path to Filename");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Processing failed: {e.Message}");
                throw;
            }

            return "";
        }

        void SetTxtPatrh(string filePath)
        {

            foreach (char a in filePath)
            {
                if (a == '\\')
                {
                    filePath += "\\" + "\\";
                }
                else
                    filePath += a;
            }
            if (filePath.Length > 260)
            {
                //Console.WriteLine($"Длина строки пути файла {InternalPath}, максимальная длина строки пути файла в Windows 260");
                throw new Exception($"Длина строки пути файла {filePath}, максимальная длина строки пути файла в Windows 260");
            }
            this.TxtPath = filePath;
        }
        void SetCsvPatrh(string filePath)
        {
            foreach (char a in filePath)
            {
                if (a == '\\')
                {
                    filePath += "\\" + "\\";
                }
                else
                    filePath += a;
            }


            if (filePath.Length > 260)
            {
                //Console.WriteLine($"Длина строки пути файла {InternalPath}, максимальная длина строки пути файла в Windows 260");
                throw new Exception($"Длина строки пути файла {filePath}, максимальная длина строки пути файла в Windows 260");
            }

            this.CsvPath = filePath;
        }
        void TXTToCSV()
        {

            try
            {

                if (this.TxtPath == "")
                {
                    throw new Exception("TXT Source not define");
                }


                List<string> temp = new List<string>();
                List<string> distinct = new List<string>();
                List<string> distinctSort = new List<string>();
                String line = "";
                int countAll = 0;
                string CurrentDirectory = Directory.GetCurrentDirectory();
                //string pattern = @"\b(\p{IsGreek}+(\s)?)+\p{Pd}\s(\p{IsBasicLatin}+(\s)?)+";
                //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                //System.Console.InputEncoding = Encoding.GetEncoding(1251);
                //txt in UTF_16BE
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Console.WriteLine("Current Path");
                Console.WriteLine(CurrentDirectory);
                //string[] textfiles= Directory.GetFiles(SelectFileDialog());            
                StreamWriter targetTmp = new StreamWriter(this.CsvPath);
                //textfiles[0] = SelectFileDialog();
                string[] tempWords;
                int startAlf = Convert.ToInt32("0400", 16);
                int endtAlf = Convert.ToInt32("04FF", 16);
                StreamReader source = new StreamReader($"{this.TxtPath}", Encoding.GetEncoding(866));

                //foreach (string name in textfiles)
                {
                    try
                    {
                        //Pass the file path and file name to the StreamReader constructor

                        line = source.ReadLine();
                        //Continue to read until you reach end of file
                        while (line != null)
                        {
                            line += source.ReadLine() + ";";
                        }

                        tempWords = line.Split(this.splitChars);

                        foreach (string name in tempWords)
                        {
                            targetTmp.Write(name + ";");
                        }
                        //close the file
                        source.Close();
                        targetTmp.Close();
                        Console.WriteLine(countAll);
                    }

                    catch (FileNotFoundException ex)
                    {
                        // Write error.
                        Console.WriteLine(ex);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: " + e.Message);
                    }
                    finally
                    {
                        Console.WriteLine("Executing finally block.");
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
        string flipCharArray(string word, char[] alphabet, char[] flipedAlphabet)
        {
            string tempWord = "";
            int alphabetLenght = alphabet.Length;
            foreach (char ch in word)
            {
                for (int i = 0; i < alphabetLenght; i++)
                { 
                    if (alphabet[i] == ch)
                        tempWord += flipedAlphabet[i];
                    break;
                }                       
            }           
            return tempWord; 
        }
        string flipStringArray(string word, string[] alphabet, string[] flipedAlphabet)
        {
            string tempWord = "";
            string[] tempChars = word.Split('\'');
            int alphabetLenght = alphabet.Length;
            foreach (string wordChar in tempChars)
            {
                for (int i = 0; i < alphabetLenght; i++)
                {
                    if (alphabet[i] == wordChar)
                        tempWord += flipedAlphabet[i] + "'";
                    break;
                }
            }
            return tempWord.Substring(tempWord.Length - 1); ;
        }
        string[] stringArrayFromCSV()
        {

            if (this.CsvPath != "")
            {

                try
                {
                    // Read in nonexistent file.
                    StreamReader source = new StreamReader($"{this.CsvPath}");
                    string[] arrayWord = source.ReadLine().Split(this.splitChars);
                    return arrayWord;
                }
                catch (FileNotFoundException ex)
                {
                    // Write error.
                    Console.WriteLine(ex);
                }
            }
            else
            {
                Console.WriteLine("PathTo CSV File not exist");
            }

            return new string[] { };
        }
        string[] stringArrayFromTXT()
        {
            if (this.TxtPath == "")
            {
                Console.WriteLine("TXT Source not define");
                return new string[] { };
            }
            string CurrentDirectory = Directory.GetCurrentDirectory();
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //System.Console.InputEncoding = Encoding.GetEncoding(1251);
            //txt in UTF_16BE
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string[] tempWords = new string[] { }; ;
            string? line = "";
            StreamReader source = new StreamReader($"{this.TxtPath}", Encoding.GetEncoding(866));
            {
                try
                {
                    //Pass the file path and file name to the StreamReader constructor
                    line = source.ReadLine(); //Continue to read until you reach end of file
                    while (line != null)
                    {
                        line += source.ReadLine() + ";";
                    }
                    if (line != null)
                    {
                        tempWords = line.Split(this.splitChars);
                    }
                    return tempWords;
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine(ex);
                    return new string[] { };
                }
            }
        }
        void arraryStringToCSVFile(string[] words)
        {
            if (this.CsvPath != "")
            {
                StreamWriter targetFile = new StreamWriter(this.CsvPath);
                foreach (string word in words)
                {
                    targetFile.Write(word + ";");
                }
                targetFile.Close();
            }
            else
            {
                throw new Exception("PathTo CSV File is empty");
            }
        }
        string[] russianToLatin(string[] words)
        {
            int arraylenght = words.Length;
            string[] wordsOnLatinica = new string[arraylenght];
            string tempWord = "";
            for (int i = 0; i < arraylenght; i++)
            {
                wordsOnLatinica[i] = flipStringArray(words[i], russianAlphabet, latinAlphabet);
                    wordsOnLatinica[i] = tempWord;
                tempWord = "";
            }
            return wordsOnLatinica;
        }     
        string[] russianToLatinNotOffical(string[] words)
        {
            int arraylenght = words.Length;
            string[] wordsOnLatinica = new string[arraylenght];
            string tempWord = "";
            for (int i = 0; i < arraylenght; i++)
            {
                wordsOnLatinica[i] = flipStringArray(words[i], russianAlphabet, LatinNotOffical);
                wordsOnLatinica[i] = tempWord;
                tempWord = "" ;
            }
            return wordsOnLatinica;
        }
        string[] LatinToRussin(string[] latinWords)
        {
            int arraylenght = latinWords.Length;
            string[] wordsOnRussian = new string[arraylenght];
            string tempWord = "";
            for (int i = 0; i < arraylenght; i++)
            {
                wordsOnRussian[i] = flipStringArray(latinWords[i], latinAlphabet,russianAlphabet);
                wordsOnRussian[i] = tempWord;
                tempWord = "";
            }
            return wordsOnRussian;
        }
        string[] LatinNotOfficalToRussin(string[] latinWords)
        {
            int arraylenght = latinWords.Length;
            string[] wordsOnRussian = new string[arraylenght];
            string tempWord = "";
            for (int i = 0; i < arraylenght; i++)
            {
                wordsOnRussian[i] = flipStringArray(latinWords[i], LatinNotOffical, russianAlphabet);
                wordsOnRussian[i] = tempWord;
                tempWord = "";
            }
            return wordsOnRussian;
        }
        string[] interselectWords(string[] wordsA, string[] wordsB)
        {
            int arrayAlenght = wordsA.Length;
            int arrayBlenght = wordsB.Length;
            List<string> interselectWords = new List<string>();
            string[] interselectWordArray;
            string word = "";
            for (int i = 0; i < arrayAlenght; i++)
            {
                for (int j = 0; j < arrayBlenght; j++)
                {
                    if (wordsA[i] == wordsB[j])
                    {
                        interselectWords.Add(wordsA[i]);
                    }
                }
            }
            interselectWordArray = interselectWords.ToArray();
            return interselectWordArray;
        }
        string[] distinctCVS(string[] wordsA)
        {
            List<string> words = new List<string>();
            foreach (string word in wordsA)
            {
                if (word != "")
                {
                    words.Add(word);
                }
            }
            return words.ToArray();
        }
        string? palidromWord(string word)
        {
            char[] chars = word.ToCharArray();
            chars = chars.Reverse().ToArray();
            if (word == new string(chars))
                return word;
            return null;
        }
        string[] palidromWordFromCSV()
        {
            List<string> wordsPalidroms = new List<string>();
            foreach (string word in stringArrayFromCSV())
            {
                string? tempPalidromWord = palidromWord(word);
                if (tempPalidromWord != null)
                {
                    wordsPalidroms.Add(tempPalidromWord);
                }
            }

            return wordsPalidroms.ToArray();
        }
        int nuberWorldsInCSV()
        {
            return stringArrayFromCSV().Count();
        }

        /*
         * 'ConvertTxt' to enter filepath to file.txt
         * 'OpenCSV' to enter filepath to file.csv
         * 'CSV' to convert TXT file to CSV
         * 'Latin' to conver CSV file to Latin alphabet
         * 'LatinNotOfficial' to conver CSV file to Latin Not Official alphabet
         * 'LatinToRussian'  to conver CSV file from Latin alphabet
         * 'LatinNotOfficialToRussian'  to conver CSV file from LatinNotOfficial alphabet
         * 'Palidrom' to search palidroms in CSV file
         * 'NumberWordsInCSV' to show numbers words
         * 'NumberWordsInTwoCSV' to show numbers words
         * 'Distinct' to remove words dublicats from CSV file
         * 'CSVSearch' to search in CSV file
         * 'CSVtoXls' to convert CSV to Xls file
         *  
         * 'flipRuEn' to flip word in file on russian to english keyboard layout
         * 'flipEnRu' to flip word in file on english to russian keyboard layout
         * 'flipRuEn' to flip words in file on russian to english keyboard layout
         * 'flipEnRu' to flip words in file on english to russian keyboard layout
         * 'flipEnRu' to search interselect word in en CSV file end flip ru alphabet worlds from ru dictionary worlds
         * 'flipEnRu' to flip word in file on english to russian keyboard layout
         * 'flipEnRu' to search interselect words in en CSV file end flip ru alphabet worlds from ru dictionary worlds
         * 'flipEnRu' to flip words in file on english to russian keyboard layout
         * interselect to search interselect word in CSV file
         * interselect to search interselect words in CSV file
         * 
         * 
         * 
         */
        
        Functions(string csvPath, string txtPath = "")
        {

            this.splitChars = new char[] { ' ', ',', '.', '!', '?', ';', '/', '|', '<', '>', '"', ':', '~', '`', '#', '%', '^', '&', '*', '(', ')', '{', '}', '-', '+', '=', '_', '\\', '\t', '\n', '\t', '\r', };

            this.russianAlphabet = new string[] { "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я" };
            this.englishAlphabet = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            this.latinAlphabet = new string[] { "a", "b", "v", "g", "d", "e", "e", "zh", "z", "i", "y", "k", "l", "m", "n", "o", "p", "r", "c", "t", "u", "f", "kh", "ts", "ch", "sh", "shch", "", "y", "", "e", "yu", "ya" };
            this.LatinNotOffical = new string[] { "a", "b", "v", "g", "d", "e", "e", "zh", "z", "i", "y", "k", "l", "m", "n", "o", "p", "r", "c", "t", "u", "f", "kh", "ts", "ch", "sh", "shch", "", "y", "", "e", "yu", "ya" };
            this.QwertyAlphabetEn = new string[] { "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "[", "]", "\\", "a", "s", "d", "f", "g", "h", "j", "k", "l", ";", "\'", "z", "x", "c", "v", "b", "n", "m", ",", ".", "/" };
            this.QwertyAlphabetRu = new string[] { "й", "ц", "у", "к", "е", "н", "г", "ш", "щ", "з", "х", "ъ", "\\", "ф", "ы", "в", "а", "п", "р", "о", "л", "д", "ж", "э", "я", "ч", "с", "м", "и", "т", "ь", "б", "ю", "." };
            this.CsvPath = csvPath;
            this.TxtPath = txtPath;
            this.CompareCsvPath = "";
        }
    }


}

