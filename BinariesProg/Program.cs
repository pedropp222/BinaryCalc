using binaries.Conversion;
using binaries.Parsing;
using binaries.Representation;
using binaries.src.AST;
using binaries.src.Parsing;
using System;

namespace binaries
{
    internal class Program
    {
        private static string version = "1.3.0";

        static void Main(string[] args)
        {
            Console.Title = "BinaryCalc " + version;

            AstPrinter astPrinter = new AstPrinter();

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
                    Parser p = new Parser(tokenized.tokens);

                    Expression e = null;
                    try
                    {
                        e = p.ParseExpr();
                        Console.WriteLine(astPrinter.Print(e));
                    }
                    catch (ParseErrorException exp)
                    {
                        Console.WriteLine("PARSE EXCEPTION: " + exp.Message);
                        Console.WriteLine("===================");
                        Console.WriteLine(tokenized.PrintProgram(exp.errorLocation));
                        continue;
                    }

                    BinaryValue bv = e.Evaluate();
                    int dec = BinaryConverter.BinaryToDecimal(bv);
                    Console.WriteLine("===================");
                    Console.WriteLine("Expression Result: " + 
                        bv.ToFancyString()+"(d"+dec+
                        "|"+BinaryConverter.BinaryToDecimal2Complement(bv)+")" +
                        "(x"+BinaryConverter.BinaryToHexadecimal(bv)+")" +
                        "(o"+BinaryConverter.DecimalToBase(dec,8)+")"+
                        "(q"+ BinaryConverter.DecimalToBase(dec, 4) + ")\n");
                    if (bv.overflow)
                    {
                        Console.WriteLine("Overflow.\n");
                    }
                }
                else
                {
                    Console.WriteLine("Error while tokenizing program.");
                }
            }
        }
    }
}