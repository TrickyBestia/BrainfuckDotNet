using System.Collections.Generic;
using System.IO;

namespace BrainfuckDotNet
{
    public static class Lexer
    {
        public static IEnumerable<Token> Tokenize(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                while (true)
                {
                    switch (reader.Read())
                    {
                        case -1:
                            yield break;
                        case '>':
                            yield return new Token(OpCode.Next);
                            break;
                        case '<':
                            yield return new Token(OpCode.Prev);
                            break;
                        case '+':
                            yield return new Token(OpCode.Incr);
                            break;
                        case '-':
                            yield return new Token(OpCode.Decr);
                            break;
                        case '.':
                            yield return new Token(OpCode.Prnt);
                            break;
                        case ',':
                            yield return new Token(OpCode.Read);
                            break;
                        case '[':
                            yield return new Token(OpCode.Whl_B);
                            break;
                        case ']':
                            yield return new Token(OpCode.Whl_E);
                            break;
                    }
                }
            }
        }
    }
}