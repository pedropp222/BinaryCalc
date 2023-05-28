namespace binaries.Parsing
{
    internal enum TokenType
    {
        //NUMBERS
        BINARY_VALUE, HEX_VALUE, DECIMAL_VALUE,
        
        LEFT_PAREN, RIGHT_PAREN,
        PLUS, MINUS,

        //KEYWORDS
        AND, OR, NOT, 

        EOF
    }
}
