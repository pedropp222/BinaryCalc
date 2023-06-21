using binaries.Conversion;
using binaries.Parsing;
using binaries.Representation;
using binaries.src.AST;
using binaries.src.Parsing;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace binaries
{
    internal class Program
    {
        private static string version = "1.4.0";

        static AstPrinter astPrinter = new AstPrinter();

        static void Main(string[] args)
        {
            Console.Title = "BinaryCalc " + version;

            string? program = "";

            Lexer l = new Lexer();

            TokenizedProgram tokenized;

            while (true)
            {
                Console.Write("> ");
                program = Console.ReadLine();

                if (program == null || program == "quit" || program.Trim().Length == 0)
                {
                    break;
                }

                tokenized = l.ParseTokens(program);

                Console.WriteLine("-------------------");

                if (tokenized.result == LexerResult.SUCCESS)
                {
                    List<IdentifierToken> identifiers = tokenized.GetIdentifiers();
                    if (identifiers.Count > 0)
                    {
                        Console.WriteLine("Truth table for expression:");

                        foreach(IdentifierToken a in identifiers)
                        {
                            Console.Write(a.name+"\t");
                        }
                        Console.WriteLine("| Out");

                        int iterations = (int)Math.Pow(2, identifiers.Count);

                        for (int i = 0; i < iterations; i++)
                        {
                            string value = LeftPad(Convert.ToString(i, 2), identifiers.Count);
                            
                            foreach(char c in value)
                            {
                                Console.Write(c+"\t");
                            }

                            for (int k = 0; k < value.Length; k++)
                            {
                                for(int j = 0; j < identifiers[k].locations.Count;j++)
                                {
                                    tokenized.SetValueLocation(value[k], identifiers[k].locations[j]);
                                }
                            }

                            BinaryValue bv = ProcessExpression(tokenized,false);

                            Console.WriteLine("| " + BinaryConverter.BinaryToDecimal(bv)+(i==iterations-1?"\n":""));
                        }

                    }
                    else
                    {
                        BinaryValue bv = ProcessExpression(tokenized,true);

                        int dec = BinaryConverter.BinaryToDecimal(bv);
                        Console.WriteLine("===================");
                        Console.WriteLine("Expression Result: " +
                            bv.ToFancyString() + "(d" + dec +
                            "|" + BinaryConverter.BinaryToDecimal2Complement(bv) + ")" +
                            "(x" + BinaryConverter.BinaryToHexadecimal(bv) + ")" +
                            "(o" + BinaryConverter.DecimalToBase(dec, 8) + ")" +
                            "(q" + BinaryConverter.DecimalToBase(dec, 4) + ")\n");
                        if (bv.overflow)
                        {
                            Console.WriteLine("Overflow.\n");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Error while tokenizing program.");
                }
            }
        }

        private static BinaryValue ProcessExpression(TokenizedProgram tokenized, bool printExpr)
        {
            Parser p = new Parser(tokenized.tokens);

            Expression e;
            try
            {
                e = p.ParseExpr();
                if (printExpr) Console.WriteLine(astPrinter.Print(e));
            }
            catch (ParseErrorException exp)
            {
                Console.WriteLine("PARSE EXCEPTION: " + exp.Message);
                Console.WriteLine("===================");
                Console.WriteLine(tokenized.PrintProgram(exp.errorLocation));
                return new BinaryValue("0");
            }

            BinaryValue bv = e.Evaluate();

            return bv;
        }

        private static string LeftPad(string value, int minNum)
        {
            while (value.Length < minNum)
            {
                value = "0" + value;
            }

            return value;
        }
    }
}