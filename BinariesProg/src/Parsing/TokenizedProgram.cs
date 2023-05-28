using binaries.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binaries.src.Parsing
{
    internal class TokenizedProgram
    {
        public readonly List<Token> tokens;
        public readonly LexerResult result;

        public TokenizedProgram(List<Token> tokens, LexerResult result) 
        {
            this.tokens = tokens;
            this.result = result;
        }

        public string PrintProgram(int errorLocation = -1)
        {
            if (tokens.Count == 0)
            {
                return "<empty>";
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < tokens.Count; i++)
            {
                sb.Append(tokens[i].value);
                if (i == errorLocation)
                {
                    sb.Append(" <-- here");
                    break;
                }
                if (i != tokens.Count - 1)
                {
                    sb.Append(',');
                }
            }

            return sb.ToString();
        }
    }
}
