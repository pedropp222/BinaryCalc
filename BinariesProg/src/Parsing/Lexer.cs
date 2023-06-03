using binaries.src.Parsing;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace binaries.Parsing
{
    internal class Lexer
    {
        private Dictionary<string, TokenType> keywords;
        private List<char> neutralChars;

        private List<Token> tokens;

        private int current;
        private string input;

        private bool error;

        public Lexer() 
        { 
            keywords = new Dictionary<string, TokenType>();
            neutralChars = new List<char>();
            tokens = new List<Token>();

            neutralChars.AddRange(new[] { ' ', '+', '-', '(', ')', '\0' });

            keywords.Add("AND", TokenType.AND);
            keywords.Add("OR", TokenType.OR);
            keywords.Add("NOT", TokenType.NOT);
            keywords.Add("XOR", TokenType.XOR);
            keywords.Add("NAND", TokenType.NAND);
        }


        public TokenizedProgram ParseTokens(string program)
        {
            input = program;
            tokens.Clear();
            current = 0;
            error = false;

            while(!Parsed() && !error)
            {
                ParseToken();
            }

            tokens.Add(new Token(TokenType.EOF, ""));

            if (!error)
            {
                //Normalize all automatically set numeric values to biggest numeric type
                int maxType = 0;
                foreach(Token t in tokens)
                {
                    if (t.tokenType <= TokenType.HEX_VALUE)
                    {
                        maxType = Math.Max(maxType, (int)t.tokenType);
                    }
                }
                foreach (Token t in tokens)
                {
                    if (t.tokenType <= TokenType.HEX_VALUE && !t.strict)
                    {
                        t.tokenType = (TokenType)maxType;
                    }
                }
            }

            return new TokenizedProgram(tokens,error?LexerResult.ERROR:LexerResult.SUCCESS);
        }

        private void ParseToken()
        {
            char c = NextChar();

            switch (c)         
            {                                  
                case ' ':
                break;

                case '(':
                    tokens.Add(new Token(TokenType.LEFT_PAREN,"("));
                break;
                case ')':
                    tokens.Add(new Token(TokenType.RIGHT_PAREN,")"));
                break;
                case '+':
                    tokens.Add(new Token(TokenType.PLUS,"+"));
                break;
                case '-':
                    tokens.Add(new Token(TokenType.MINUS,"-"));
                break;
                case 'x':
                    c = NextChar();
                    if (IsNumber(c))
                    {
                        ParseNumber(NumberType.HEX);
                    }
                    else
                    {
                        Console.WriteLine("EXPECTED HEX NUMBER AFTER 'x'");
                        error = true;
                    }
                break;
                case 'b':
                    c = NextChar();
                    if (IsNumber(c))
                    {
                        ParseNumber(NumberType.BINARY);
                    }
                    else
                    {
                        Console.WriteLine("EXPECTED BINARY NUMBER AFTER 'b'");
                        error = true;
                    }
                    break;
                case 'd':
                    c = NextChar();
                    if (IsNumber(c))
                    {
                        ParseNumber(NumberType.DECIMAL);
                    }
                    else
                    {
                        Console.WriteLine("EXPECTED DECIMAL NUMBER AFTER 'd'");
                        error = true;
                    }
                break;
                case 'q':
                    c = NextChar();
                    if (IsNumber(c))
                    {
                        ParseNumber(NumberType.QUAD);
                    }
                    else
                    {
                        Console.WriteLine("EXPECTED QUAD NUMBER AFTER 'q'");
                        error = true;
                    }
                break;
                case 'o':
                    c = NextChar();
                    if (IsNumber(c))
                    {
                        ParseNumber(NumberType.OCT);
                    }
                    else
                    {
                        Console.WriteLine("EXPECTED OCTAL NUMBER AFTER 'o'");
                        error = true;
                    }
                    break;
                case '\0':
                break;
                default:
                    if (IsDecimal(c))
                    {
                        ParseNumber(NumberType.UNKNOWN);
                        break;
                    }
                    ParseKeywordOrHexNumber();
                    break;
            }

        }

        private void ParseKeywordOrHexNumber()
        {
            int charCount = 0;

            string word = "";

            char c = input[current - 1];

            while (c!=' '&&!Parsed())
            {
                charCount++;
                word += c;
                if (keywords.ContainsKey(word))
                {
                    c = Peek();
                    if (IsNeutralChar(c))
                    {
                        tokens.Add(new Token(keywords.GetValueOrDefault(word)));
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Keyword '" + word +"' must be followed by space or parenthesis");
                        error = true;
                        return;
                    }
                }

                c = NextChar();
            }

            current -= charCount;
            c = input[current - 1];

            if (IsNumber(c))
            {
                ParseNumber(NumberType.UNKNOWN);
            }
            else
            {
                Console.WriteLine("Unknown Token: '" + word + "'");
                error = true;
            }
        }

        private void ParseNumber(NumberType type)
        {
            StringBuilder number = new StringBuilder();

            NumberType currentType = type;

            bool enforceType = false;

            char c = input[current - 1];
            if (currentType == NumberType.UNKNOWN)
            {
                
                currentType = DetermineNumber(c, currentType);       
            }
            else
            {
                enforceType = true;
            }

            while(IsNumber(c))
            {
                number.Append(c);
                if (Parsed()) break;
                c = NextChar();

                NumberType checkType = DetermineNumber(c, currentType);
                if (enforceType && checkType != currentType)
                {
                    Console.WriteLine("ERROR: NUMBER WAS SET TO " + currentType + " BUT GOT '"+c+"' WHICH IS " + checkType);
                    error = true;
                    break;
                }
                currentType = checkType;
            }

            if (!Parsed())
            {
                if (!IsNeutralChar(c))
                {
                    Console.WriteLine("Expected space or parenthesis after numeric value '" + number.ToString() + "'");
                    error = true;
                    return;
                }
                //we consumed a character while searching for a number and must go back
                current--;
            }

            TokenType nType;

            switch (currentType)
            {
                case NumberType.BINARY:
                    nType = TokenType.BINARY_VALUE; break;
                case NumberType.QUAD: 
                    nType = TokenType.QUAD_VALUE; break;
                case NumberType.DECIMAL:
                    nType = TokenType.DECIMAL_VALUE; break;
                case NumberType.OCT:
                    nType = TokenType.OCTAL_VALUE; break;
                default:
                    nType = TokenType.HEX_VALUE; break;
            }

            tokens.Add(new Token(nType,number.ToString(),enforceType));
        }

        private bool IsNeutralChar(char c)
        {
            return neutralChars.Contains(c);
        }

        private NumberType DetermineNumber(char c, NumberType current)
        {
            if ((c == '0' || c == '1') && (current == NumberType.UNKNOWN))
            {
                return NumberType.BINARY;
            }
            else if (c >= '2' && c <= '3' && (int) current <= 1)
            {
                return NumberType.QUAD;
            }
            else if (c >= '4' && c <= '9' && (int) current <= 2)
            {
                return NumberType.DECIMAL;
            }
            else if (c >= 'A' && c <= 'F' && (int) current <= 3)
            {
                return NumberType.HEX;
            }
            else
            {
                return current;
            }
        }

        private bool IsNumber(char c)
        {
            return c >= '0' && c <= '9' || c >= 'A' && c <= 'F';
        }

        private bool IsDecimal(char c)
        {
            return c >= '0' && c <= '9';
        }

        private char Peek()
        {
            if (current >= input.Length)
            {
                return '\0';
            }
            return input[current];
        }

        private char NextChar()
        {
            current++;
            if (Parsed())
            {
                return '\0';
            }
            return input[current-1];
        }

        private bool Parsed()
        {
            bool p = current > input.Length;
            return p;
        }
    }

    internal enum NumberType
    {
        UNKNOWN,
        BINARY,
        QUAD,
        DECIMAL,
        HEX,
        OCT
    }

}
