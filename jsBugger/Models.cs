using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Parse
{
    public enum WORD
    {
        NULL,
        ASTERISK, // *
        LPARENT, // (
        RPARENT, // )
        LBRACE, // {
        RBRACE, // }
        LBRACKET, // [
        RBRACKET, // ]
        PIPE, // |
        AND, // &
        PERCENT, // %
        CARET, // ^
        DASH, // -
        UNDERSCORE, // _
        EQUAL, // =
        LESSTHAN, // <
        GREATERTHAN, // >
        COMMA, // ,
        QUOTE, // "
        END, // ;
        DOTT, // :
        NEWLINE, // \n
        COMMENTSTART, // /*
        COMMENTEND, // */
        FUNCTION,
        ATTR,
        DQUOTE,
        PROP,
        PARSEFLOAT,
        PARSEINT,
        EACH,
        TEXTFUNC,
        TRIMFUNC,
        RETURN,
        IF,
        ELSE,
        VAR,
        FALSE,
        TRUE,
        TYPEOF,
        PROGRAMSTATEMENT,
        USERINPUT,
        EMPTY
    }

    public class Token
    {
        public WORD TokenType { get; set; }
        public int Index { get; set; }
        public string TokenString { get; set; }
        public int LinesIndex { get; set; }

        public int TokenNextIndex { get; set; }
        public WORD NextTokenType { get; set; }
        public WORD EqualToken { get; set; }
        public int Connective { get; set; }
    }

    public class Variable
    {
        public int usedCounter { get; set; }
        public string variableName { get; set; }
    }
}
