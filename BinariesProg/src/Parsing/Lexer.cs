using binaries.src.Parsing;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binaries.Parsing
{
    internal class Lexer
    {
        private Dictionary<string, TokenType> keywords;

        private List<Token> tokens;

        private int current;
        private string input;

        private bool error;

        public Lexer() 
        { 
            keywords = new Dictionary<string, TokenType>();
            tokens = new List<Token>();

            keywords.Add("+", TokenType.PLUS);
            keywords.Add("-", TokenType.MINUS);
            keywords.Add("(", TokenType.LEFT_PAREN);
            keywords.Add(")", TokenType.RIGHT_PAREN);

            keywords.Add("AND", TokenType.AND);
            keywords.Add("OR", TokenType.OR);

            keywords.Add("NOT", TokenType.NOT);
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
                case 'O':
                    c = NextChar();
                    if (c == 'R')
                    {
                        tokens.Add(new Token(TokenType.OR));
                    }
                    else
                    {
                        Console.WriteLine("EXPECTED 'R' AFTER O");
                        error = true;
                        return;
                    }
                break;
                case 'A':
                    c = NextChar();
                    if (c == 'N')
                    {
                        c = NextChar();
                        if (c == 'D')
                        {
                            tokens.Add(new Token(TokenType.AND));
                        }
                        else
                        {
                            Console.WriteLine("EXPECTED 'D' AFTER 'AN'");
                            error = true;
                        }
                    }
                    else if (IsNumber(c))
                    {
                        current--;
                        ParseNumber(NumberType.HEX);
                    }
                    else
                    {
                        Console.WriteLine("UNKNOWN CHARACTER '"+c+"' AFTER 'A'. EXPECTED 'AND' OR HEX NUMBER");
                        error = true;
                    }
                break;
                case 'N':
                    c = NextChar();
                    if (c == 'O')
                    {
                        c = NextChar();
                        if (c == 'T')
                        {
                            tokens.Add(new Token(TokenType.NOT));
                        }
                        else
                        {
                            Console.WriteLine("EXPECTED 'T' AFTER 'NO'");
                            error = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("EXPECTED 'O' AFTER 'N'");
                        error = true;
                    }
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
                default:
                    if (IsNumber(c))
                    {
                        ParseNumber(NumberType.UNKNOWN);
                    }
                    else if (c != '\0')
                    {
                        Console.WriteLine("UNKNOWN SYMBOL '" + c + "'");
                        error = true;
                    }
                    break;
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
                //we consumed a token while searching for a number and must go back
                current--;
            }
            
            tokens.Add(new Token(currentType == NumberType.BINARY 
                ? TokenType.BINARY_VALUE : 
                currentType == NumberType.DECIMAL 
                ? TokenType.DECIMAL_VALUE : TokenType.HEX_VALUE,
                number.ToString()));
        }

        private NumberType DetermineNumber(char c, NumberType current)
        {
            if ((c == '0' || c == '1') && (current == NumberType.UNKNOWN))
            {
                return NumberType.BINARY;
            }
            else if (c >= '2' && c <= '9' && (int)current <= 1)
            {
                return NumberType.DECIMAL;
            }
            else if (c >= 'A' && c <= 'F' && (int)current <= 2)
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
        DECIMAL,
        HEX
    }

}
