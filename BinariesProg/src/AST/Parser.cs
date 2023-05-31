using binaries.Parsing;
using binaries.src.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace binaries.src.AST
{
    internal class Parser
    {
        private readonly List<Token> tokens;
        private int current = 0;
        private int exprStack = 0;

        public Parser(List<Token> tokens) 
        { 
            this.tokens = tokens;
        }

        public Expression ParseExpr()
        {
            Expression exp = ParseTerm();
            if (exp == null)
            {
                throw new ParseErrorException("The expression is empty.",current);
            }
            else if (current != tokens.Count-1 && exprStack == 0)
            {
                StringBuilder sb = new StringBuilder();
                for(int i = current; i < tokens.Count; i++)
                {
                    sb.Append(tokens[i].tokenType.ToString()).Append(' ');
                    if (i != tokens.Count-1)
                    {
                        sb.Append(',');
                    }
                    
                }

                throw new ParseErrorException("The expression is malformed. Expected end of file, got "+sb.ToString(),current);
            }

            return exp;
        }

        private Expression ParseTerm()
        {
            Expression term = ParseBitwise();

            while (Match(TokenType.PLUS, TokenType.MINUS))
            {
                term = new Binary(term, Previous(), ParseBitwise(), current);
            }

            return term;
        }

        private Expression ParseBitwise()
        {
            Expression exp = ParseUnary();

            while (Match(TokenType.AND,TokenType.OR,TokenType.XOR,TokenType.NAND))
            {
                exp = new Binary(exp, Previous(), ParseUnary(),current);
            }

            return exp;
        }

        private Expression ParseUnary()
        {
            if (Match(TokenType.NOT))
            {
                return new Unary(Previous(), ParseUnary(), current);
            }

            return ParseNumber();
        }

        private Expression ParseNumber()
        {
            if (Match(TokenType.DECIMAL_VALUE,TokenType.HEX_VALUE,TokenType.BINARY_VALUE))
            {
                return new Literal(Previous().value, Previous().tokenType);
            }
            else if (Match(TokenType.LEFT_PAREN))
            {
                exprStack++;
                Expression exp = ParseExpr();

                if (!Match(TokenType.RIGHT_PAREN))
                {
                    Console.WriteLine("Expected Closing Parenthesis");
                    return null;
                }

                exprStack--;
                return new Grouping(exp);
            }

            Token previous = Previous();
            if (previous != null)
            {
                Console.WriteLine("Error parsing: Unexpected token " + Previous().tokenType.ToString());
            }
            else
            {
                Console.WriteLine("I don't know what to tell ya");
            }
            return null;
        }

        private bool Match(params TokenType[] types)
        {
            foreach(TokenType type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }

            return false;
        }

        private bool Check(TokenType type)
        {
            if (IsEnd()) return false;

            return Peek().tokenType == type;
        }

        private Token Advance()
        {
            if (!IsEnd()) current++;
            return Previous();
        }

        private bool IsEnd()
        {
            return Peek().tokenType == TokenType.EOF;
        }

        private Token Peek()
        {
            return tokens[current];
        }

        private Token Previous()
        {
            if (current == 0) return null;
            return tokens[current - 1];
        }
    }
}
