using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Parse
{
    public class Parser
    {
        List<Token> TokenList = new List<Token>();
        int currentLine = 0;
        static bool inDQuote = false;
        
        static bool inObject = false;
        static bool lastNewLine = false;
        static bool inStatement = false;
        static bool inAttributeStatements = false;

        static int returnCounter = 0;

        public static int lastAppenedFunctionCounter = 0;
        static bool inComment = false;

        public static string currentFunctionName = "";
        public static bool inFunction = false;
        public static bool inDoubleFunction = false;

        public static int TabCount = 0;

        static List<Variable> Variables = new List<Variable>();

        public void StartParse(List<Token> TokenList, int currentLine)
        {
            this.currentLine = currentLine;
            this.TokenList = TokenList;

            for (int i = 0; i < TokenList.Count; i++)
            {
                Token CurrentToken = TokenList[i];

                #region Comment
                if (EqualTokenString(i, "/*") && !inComment)
                {
                    inComment = true;
                }
                #endregion

                if (!inComment)
                {
                    #region Function
                    if (EqualTokenType(i, WORD.FUNCTION) && EqualTokenType(i - 1, WORD.DOTT) && EqualTokenType(TokenList.Count - 1, WORD.LBRACE) && !inFunction)
                    {
                        inFunction = true;
                        currentFunctionName = getPrevToken(i - 1).TokenString;
                        TabCount++;
                    }
                    #endregion

                    #region Double Function
                    if (EqualTokenType(i, WORD.FUNCTION) && EqualTokenType(i + 2, WORD.LPARENT) && EqualTokenType(TokenList.Count - 1, WORD.LBRACE))
                    {
                        inDoubleFunction = true;
                        TabCount++;
                    }

                    if (inFunction)
                    {
                        if (EqualTokenType(i, WORD.RETURN) && !inDoubleFunction && currentFunctionName != "sliderSettings" && currentFunctionName != "categorySettings")
                        {
                            returnCounter++;
                        }

                        if (returnCounter > 1)
                        {
                            WriteError(i, "Birden fazla return kullanımı yanlış");
                            returnCounter = 0;
                        }
                    }
                    else
                    {
                        returnCounter = 0;
                    }

                    if (EqualTokenType(i, WORD.RBRACE) && inDoubleFunction && TokenList.Count == 1 && TabCount == 2)
                    {
                        TabCount--;
                        inDoubleFunction = false;
                    }
                    #endregion

                    #region Function
                    if (EqualTokenType(i + 1, WORD.COMMA) && EqualTokenType(i, WORD.RBRACE) && inFunction)
                    {
                        inFunction = false;
                        currentFunctionName = "";
                        TabCount--;
                    }
                    #endregion
                }

                #region Comment
                if (EqualTokenString(i, "*/") && inComment)
                {
                    inComment = false;
                }
                #endregion
            }
        }

        public Token getPrevToken(int i)
        {
            if (i < 0)
            {
                return new Token();
            } 
            else
            {
                return TokenList[i - 1];
            }
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
