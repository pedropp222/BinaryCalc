namespace binaries.Parsing
{
    internal class Token
    {
        public TokenType tokenType;
        public readonly string value;
        public readonly bool strict;

        public Token(TokenType type, string value, bool strict = false)
        {
            tokenType = type;
            this.value = value;
            this.strict = strict;
        }

        public Token(TokenType type)
        {
            tokenType = type;
            value = tokenType.ToString();
        }

        public override string ToString()
        {
            return tokenType + " "+value;
        }
    }
}
