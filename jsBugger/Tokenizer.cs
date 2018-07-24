using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Parse
{
    public class Tokenizer
    {
        static String FULL_STRING = "";
        public static List<Token> Tokens = new List<Token>();

        public static List<Token> GetTokenFromString(String FULL_STRING, int emtyLine)
        {
            if (emtyLine == 1)
            {
                Token t = new Token();

                t.TokenType = WORD.NEWLINE;
                t.TokenString = "";

                Tokens.Add(t);
                return Tokens;
            }

            Tokenizer.FULL_STRING = FULL_STRING;
            String[] StringList = SplitString();
            int Index = 0;

            foreach (var item in StringList)
            {
                Token t = new Token();
                bool NextToken = false;
                switch (item)
                {
                    case "function":
                        t.TokenType = WORD.FUNCTION;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "if":
                        t.TokenType = WORD.IF;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "else":
                        t.TokenType = WORD.ELSE;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "return":
                        t.TokenType = WORD.RETURN;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "var":
                        t.TokenType = WORD.VAR;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "true":
                        t.TokenType = WORD.TRUE;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "false":
                        t.TokenType = WORD.FALSE;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "typeof":
                        t.TokenType = WORD.TYPEOF;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "attr":
                        t.TokenType = WORD.ATTR;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "prop":
                        t.TokenType = WORD.PROP;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "parseFloat":
                        t.TokenType = WORD.PARSEFLOAT;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "parseInt":
                        t.TokenType = WORD.PARSEINT;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "each":
                        t.TokenType = WORD.EACH;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "text":
                        t.TokenType = WORD.TEXTFUNC;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "trim":
                        t.TokenType = WORD.TRIMFUNC;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case ":":
                        t.TokenType = WORD.DOTT;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "/*":
                        t.TokenType = WORD.COMMENTSTART;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "*/":
                        t.TokenType = WORD.COMMENTEND;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case ";":
                        t.TokenType = WORD.END;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "*":
                        t.TokenType = WORD.ASTERISK;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "(":
                        t.TokenType = WORD.LPARENT;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case ")":
                        t.TokenType = WORD.RPARENT;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "{":
                        t.TokenType = WORD.LBRACE;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "}":
                        t.TokenType = WORD.RBRACE;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "[":
                        t.TokenType = WORD.LBRACKET;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "]":
                        t.TokenType = WORD.RBRACKET;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "|":
                        t.TokenType = WORD.PIPE;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "&":
                        t.TokenType = WORD.PIPE;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "%":
                        t.TokenType = WORD.PERCENT;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "^":
                        t.TokenType = WORD.CARET;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "-":
                        t.TokenType = WORD.DASH;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "_":
                        t.TokenType = WORD.UNDERSCORE;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "=":
                        t.TokenType = WORD.EQUAL;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case ",":
                        t.TokenType = WORD.COMMA;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "\"":
                        t.TokenType = WORD.QUOTE;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case "'":
                        t.TokenType = WORD.DQUOTE;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    case " ":
                        t.TokenType = WORD.EMPTY;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                    default:
                        t.TokenType = WORD.USERINPUT;
                        t.Index = Index;
                        t.TokenString = item;
                        break;
                }
                Tokens.Add(t);
                Index++;
            }
            return Tokens;
        }

        static String[] SplitString()
        {
            List<String> StringList = new List<String>();
            ReplaceForSplit();
            foreach (var item in FULL_STRING.Split(' '))
            {
                if (!string.IsNullOrEmpty(item))
                {
                    StringList.Add(item.Trim());
                }
            }
            return StringList.ToArray();
        }

        static void ReplaceForSplit()
        {
            FULL_STRING = FULL_STRING.Replace("["," [ ");
            FULL_STRING = FULL_STRING.Replace("]"," ] ");
            FULL_STRING = FULL_STRING.Replace("{"," { ");
            FULL_STRING = FULL_STRING.Replace("}"," } ");
            FULL_STRING = FULL_STRING.Replace("."," . ");
            FULL_STRING = FULL_STRING.Replace("("," ( ");
            FULL_STRING = FULL_STRING.Replace(")"," ) ");
            FULL_STRING = FULL_STRING.Replace(";"," ; ");
            FULL_STRING = FULL_STRING.Replace("|"," | ");
            FULL_STRING = FULL_STRING.Replace("&"," & ");
            FULL_STRING = FULL_STRING.Replace("-"," - ");
            FULL_STRING = FULL_STRING.Replace(">"," > ");
            FULL_STRING = FULL_STRING.Replace("<"," < ");
            FULL_STRING = FULL_STRING.Replace(","," , ");
            FULL_STRING = FULL_STRING.Replace("\""," \" ");
            FULL_STRING = FULL_STRING.Replace("%"," % ");
            FULL_STRING = FULL_STRING.Replace(":"," : ");
            FULL_STRING = FULL_STRING.Replace("/*"," /* ");
            FULL_STRING = FULL_STRING.Replace("*/"," */ ");
            FULL_STRING = FULL_STRING.Replace("'"," ' ");

            FULL_STRING = FULL_STRING.Replace("+"," + ");
            FULL_STRING = FULL_STRING.Replace("="," = ");
            while (FULL_STRING.Contains("  "))
            {
                FULL_STRING = FULL_STRING.Replace("  ", " ");
            }
        }
    }
}
