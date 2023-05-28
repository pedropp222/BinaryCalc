namespace binaries.Parsing
{
    internal class Token
    {
        public readonly TokenType tokenType;
        public readonly string value;

        public Token(TokenType type, string value)
        {
            tokenType = type;
            this.value = value;
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
