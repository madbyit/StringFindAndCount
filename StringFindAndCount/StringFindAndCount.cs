using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


namespace StringFindAndCount
{
    public class StringFindAndCountProgram
    {
        private readonly Dictionary<string,int> strcountdict = new();

        /* Takes one argument, a path to a file. */
        public StringFindAndCountProgram(string[] filepath_in)
        {
            /* Execute stringfinder / counter */
            RunStringFindAndCount(filepath_in);
        }

        /* Find and count string in file */
        private void RunStringFindAndCount(string[] filepath_arr)
        {
            var counter = 0;

            /* Get file name without extension */
            foreach (var (filepath, filename) in from string filepath in filepath_arr
                                                 let filename = Path.GetFileName(filepath).Split('.')[0]
                                                 select (filepath, filename))
            {
                
                try
                {
                    var file = File.Open(filepath, FileMode.Open); /* Open the file */

                    using (StreamReader sr = new StreamReader(file))
                    {
                        /* This will count every string named as filename,
                         * but NOT if the word appear in- or beside another word */
                        while (!sr.EndOfStream)
                        {
                            var inner_counter = sr.ReadLine()
                                                .Split('.', '?', '!', ',', ' ') /* Most common word separators in a textfile. */
                                                .GroupBy(s => s)
                                                .Select(g => new { Word = g.Key, Count = g.Count() });

                            var wordcount = inner_counter.SingleOrDefault(c => c.Word == filename);
                            counter += (wordcount == null) ? 0 : wordcount.Count; /* Adds 0 if null, otherwise add counted. */
                        }
                    }

                    /* Store filename and number of times it occured into a dictionary */
                    strcountdict.Add(filename, counter);

                    counter = 0; /* Reset counter */
                    file.Close(); /* Close file */
                }
                catch (Exception ex)
                {
                    //TODO: Improve exception handling
                    /*
                     FileNotFoundException
                     ArgumentException
                     PathTooLongException
                     UnauthorizedAccessException
                     */
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        /* Get number of times a word occured from the Dictionary */
        public int GetKeyValue(string key)
        {
            var keyvalue = 0;
            try
            {
                bool iskey = strcountdict.TryGetValue(key, out var value);
                if (iskey) keyvalue = strcountdict[key];
                else
                {
                    Console.WriteLine($"Can't find a file with this name: {key}.");
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message); /* Log purpose */
            }

            return keyvalue;
        }

        static void Main(string[] args)
        {
            string filePath = Environment.CurrentDirectory + "/App_Data";
            try
            {
                string[] files = Directory.GetFiles(@filePath);
                StringFindAndCountProgram str_find_prog = new(files);
                
                foreach (KeyValuePair<string, int> keyValue in str_find_prog.strcountdict)
                {
                    Console.WriteLine($"{keyValue.Key} has {keyValue.Value} of equal string names");
                }

                Console.ReadLine();
            }
            catch(DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }

        }
    }
}