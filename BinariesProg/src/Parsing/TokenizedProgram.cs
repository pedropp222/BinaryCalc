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
        public List<Token> tokens { get; private set; }
        public readonly LexerResult result;

        public TokenizedProgram(List<Token> tokens, LexerResult result) 
        {
            this.tokens = tokens;
            this.result = result;
        }


        public List<IdentifierToken> GetIdentifiers()
        {
            List<IdentifierToken> identifiers = new List<IdentifierToken>();

            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].tokenType == TokenType.IDENT)
                {
                    IdentifierToken? t = identifiers.Find((x) => x.name == tokens[i].value);
                    if (t == null)
                    {
                        identifiers.Add(new IdentifierToken(tokens[i].value,i));
                    }
                    else
                    {
                        t.locations.Add(i);
                    }
                }
            }
            return identifiers;
        }

        public void SetValueLocation(char val, int index)
        {
            tokens[index] = new Token(TokenType.DECIMAL_VALUE, val.ToString(),true);
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

    internal class IdentifierToken
    {
        public readonly string name;
        public List<int> locations;

        public IdentifierToken(string name, int firstLocation)
        {
            locations = new List<int>
            {
                firstLocation
            };
            this.name = name;
        }
    }

}
