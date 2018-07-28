using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Parse
{
    public class Parser
    {
        List<Token> TokenList = new List<Token>();
        public static string currentFunctionName = "";
        int currentLine = 0;
        static bool inDQuote = false;
        static bool inComment = false;
        public static bool inFunction = false;
        public static bool inDoubleFunction = false;
        static bool inObject = false;
        static bool lastNewLine = false;
        static bool inStatement = false;
        static bool inAttributeStatements = false;

        static int returnCounter = 0;
        public static int lastAppenedFunctionCounter = 0;

        static List<Variable> Variables = new List<Variable>();

        public void StartParse(List<Token> TokenList, int currentLine)
        {
            this.currentLine = currentLine;
            this.TokenList = TokenList;

            for (int i = 0; i < TokenList.Count; i++)
            {
                Token CurrentToken = TokenList[i];

                // -Start- Kullanılmayan değişken kontrolü
                foreach (var item in Variables)
                {
                    if (EqualTokenString(i, item.variableName))
                    {
                        item.usedCounter++;
                    }
                }

                if (EqualTokenType(i, WORD.RBRACE) && EqualTokenType(i + 1, WORD.COMMA) && inFunction)
                {
                    foreach (var item in Variables)
                    {
                        if (item.usedCounter < 1 && item.variableName != "data")
                        {
                            WriteError(i, "Kullanılmayan değişken -> " + item.variableName);
                        }
                    }

                    Variables = new List<Variable>();
                    if (inDoubleFunction)
                    {
                        inDoubleFunction = false;
                    }
                    else
                    {
                        inFunction = false;
                    }
                    returnCounter = 0;
                    lastAppenedFunctionCounter = 0;
                }
                // -End- Kullanılmayan değişken kontrolü

                // -Start- Yorum satırlarını es geç
                if (EqualTokenType(i, WORD.COMMENTEND))
                {
                    inComment = false;
                }

                if (inComment)
                {
                    continue;
                }

                if (EqualTokenType(i, WORD.COMMENTSTART) && (EqualTokenType(i + 1, WORD.ASTERISK) || EqualTokenType(i + 2, WORD.ASTERISK)))
                {
                    inComment = true;
                }
                // -End- Yorum satırlarını es geç

                // -Start- Fonksiyon ismi al
                if (EqualTokenType(i, WORD.FUNCTION) && EqualTokenType(i - 1, WORD.DOTT))
                {
                    if (!inFunction)
                    {
                        inDoubleFunction = true;
                    }

                    currentFunctionName = TokenList[i - 2].TokenString;
                    if (char.IsUpper(currentFunctionName[0]))
                    {
                        WriteError(i, "Fonksiyon isimleri küçük harf ile başlamalıdır.");
                    }
                    inFunction = true;
                    lastAppenedFunctionCounter = 0;
                } 
                else if (EqualTokenType(i, WORD.FUNCTION) && EqualTokenType(i + 1, WORD.USERINPUT))
                {
                    inDoubleFunction = true;

                    currentFunctionName = TokenList[i + 1].TokenString;
                    if (char.IsUpper(currentFunctionName[0]))
                    {
                        WriteError(i, "Fonksiyon isimleri küçük harf ile başlamalıdır.");
                    }
                    inFunction = true;
                    lastAppenedFunctionCounter = 0;
                }
                // -End- Fonksiyon ismi al

                // -Start- Tek tırnak kontrolü
                if (EqualTokenType(i, WORD.QUOTE) && !inDQuote)
                {
                    WriteError(i, "Stringler tek tırnak ile oluşturulmalıdır.");
                }

                if (EqualTokenType(i, WORD.DQUOTE) && inDQuote)
                {
                    inDQuote = false;
                }
                else if (EqualTokenType(i, WORD.DQUOTE) && !inDQuote)
                {
                    inDQuote = true;
                }
                // -End- Tek tırnak kontrolü

                // -Start- Tek satırda birden fazla değişken kontrolü
                if (EqualTokenType(i, WORD.VAR) && EqualTokenType(i + 2, WORD.COMMA))
                {
                    WriteError(i, "Tek satırda birden fazla değişken tanımlaması yapılmamalıdır.");
                }
                // -End- Tek satırda birden fazla değişken kontrolü

                // -Start- Değişken tanımlamalarını tut.
                if (EqualTokenType(i, WORD.VAR) && EqualTokenType(i + 1, WORD.USERINPUT) && inFunction)
                {
                    Variables.Add(new Variable() { usedCounter = 0, variableName = getNextToken(i).TokenString });
                    i += 1;
                }
                // -End- Değişken tanımlamalarını tut.

                // -Start- Return den önce boşlukları kontrol et
                if (EqualTokenType(i, WORD.RETURN))
                {
                    if (lastAppenedFunctionCounter > 4 && inFunction && !inStatement)
                    {
                        if (!lastNewLine && currentFunctionName != "_testPageConditions")
                        {
                            WriteError(i, " 'return' ifadelerinden önce boşluk bırakılmalı.");
                        }
                    }

                    if (!inDoubleFunction && currentFunctionName != "isElementInSlider" &&
                        currentFunctionName != "getCategoryList" && currentFunctionName != "_testPageConditions")
                    {
                        returnCounter++;
                    }

                    if (returnCounter > 1)
                    {
                        WriteError(i, "Birden fazla return kullanımı yanlış.");
                    }
                }
                // -End- Return den önce boşlukları kontrol et

                // -Start- Attr - Prop fallback kontrol et
                if (inAttributeStatements && EqualTokenType(i, WORD.COMMA))
                {
                    inAttributeStatements = false;
                }

                if (inAttributeStatements && EqualTokenType(i, WORD.RPARENT) && (EqualTokenType(i + 1, WORD.END) || EqualTokenType(i + 1, WORD.COMMA)
                    || EqualTokenType(i + 1, WORD.RPARENT)))
                {
                    WriteError(i + 1, "Fallback gerekiyor.");
                    inAttributeStatements = false;
                }

                if ((EqualTokenType(i, WORD.ATTR)) && EqualTokenType(i + 1, WORD.LPARENT))
                {
                    inAttributeStatements = true;
                }
                // -End- Attr - Prop fallback kontrol et

                // -Start- Blokların bitişlerini takip et.
                if (EqualTokenType(i, WORD.RBRACE))
                {
                    if (EqualTokenType(i + 1, WORD.END) && inObject)
                    {
                        inObject = false;
                    }
                    else if (inStatement)
                    {
                        inStatement = false;
                    }
                }
                // -End- Blokların bitişlerini takip et.

                if (EqualTokenType(i, WORD.NEWLINE))
                {
                    lastNewLine = true;
                }
                else
                {
                    lastNewLine = false;
                }

                if (EqualTokenType(i, WORD.IF) || EqualTokenType(i, WORD.ELSE))
                {
                    inStatement = true;
                }
            }
        }

        public Token getPrevToken(int i)
        {
            return TokenList[i - 1];
        }

        public Token getNextToken(int i)
        {
            return TokenList[i + 1];
        }

        public bool EqualTokenType(int index, WORD tokenType)
        {
            try
            {
                if (TokenList[index].TokenType == tokenType)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool EqualTokenString(int index, string tokenString)
        {
            try
            {
                if (TokenList[index].TokenString == tokenString)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void WriteError(int i, string errorMessage = "")
        {
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("                               Error                                      ");
            Console.ForegroundColor = ConsoleColor.Gray;

            string writeLine = "";
            try
            {
                for (int j = i - 10; j < i + 10; j++)
                {
                    writeLine += TokenList[j].TokenString;
                }
            }
            catch (Exception)
            {
                try
                {
                    for (int j = i - 2; j < i + 2; j++)
                    {
                        writeLine += TokenList[j].TokenString;
                    }
                }
                catch (Exception)
                {
                    writeLine = "";
                }
            }
            Console.WriteLine("| " + writeLine.PadRight(72,' '));
            Console.WriteLine("| Error Message : " + errorMessage.PadRight(56, ' '));
            Console.WriteLine("| Satır : " + currentLine.ToString().PadRight(64, ' '));
            Console.WriteLine("| Function : " + currentFunctionName.PadRight(61, ' '));
        }
    }
}
