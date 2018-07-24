using Core.Parse;
using System;
using System.IO;

namespace jsBugger
{
    class Program
    {
        static string[] fileText;
        static void Main(string[] args)
        {
            try
            {
                var fileText = File.ReadAllLines(args[0]);

                for (int i = 0; i < fileText.Length; i++)
                {
                    int emptyLine = fileText[i] == "" ? 1 : 0;
                    Tokenizer.GetTokenFromString(fileText[i], emptyLine);
                    Parser p = new Parser();
                    p.StartParse(Tokenizer.Tokens, i + 1);
                    if (Parser.inFunction)
                    {
                        Parser.lastAppenedFunctionCounter++;
                    }
                    Tokenizer.Tokens = new System.Collections.Generic.List<Token>();
                }
                Console.WriteLine("");
                Console.WriteLine("Finished.");
                Console.ReadKey();
            }
            catch (Exception)
            {
                Console.WriteLine("Dosya okuması başarısız.");
            }
        }
    }
}
