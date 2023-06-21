namespace binaries.Parsing
{
    internal enum TokenType
    {
        //NUMBERS
        BINARY_VALUE, QUAD_VALUE, OCTAL_VALUE, DECIMAL_VALUE, HEX_VALUE,
        
        LEFT_PAREN, RIGHT_PAREN,
        PLUS, MINUS, SHIFT_LEFT, SHIFT_RIGHT,

        //KEYWORDS
        AND, OR, NOT, XOR, NAND,

        IDENT,

        EOF
    }
}
