using System.Collections.Generic;

namespace BrainfuckDotNet
{
    public static class Optimizer
    {
        public static IEnumerable<Token> Optimize(IEnumerable<Token> tokens)
        {
            Token? previousToken = null;
            foreach (var token in tokens)
            {
                if (token.OpCode != OpCode.Decr
                    && token.OpCode != OpCode.Incr
                    && token.OpCode != OpCode.Next
                    && token.OpCode != OpCode.Prev)
                {
                    if (previousToken.HasValue)
                    {
                        yield return previousToken.Value;
                        previousToken = null;
                    }
                    yield return token;
                }
                else if (previousToken.HasValue)
                {
                    if (token.OpCode == previousToken.Value.OpCode)
                    {
                        previousToken = new Token(token.OpCode, (int)previousToken.Value.Value + 1);
                    }
                    else
                    {
                        yield return previousToken.Value;
                        previousToken = new Token(token.OpCode, 1);
                    }
                }
                else previousToken = new Token(token.OpCode, 1);
            }
            if (previousToken.HasValue)
                yield return previousToken.Value;
        }
    }
}
