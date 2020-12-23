using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace BrainfuckDotNet
{
    public static class Parser
    {
        private static readonly MethodInfo _consoleWrite = typeof(Console).GetMethod("Write", new Type[] { typeof(char) });
        private static readonly MethodInfo _consoleRead = typeof(Console).GetMethod("Read", new Type[0]);

        public static void Parse(IEnumerable<Token> tokens, ILGenerator ilGenerator)
        {
            Stack<Label> whileBegins = new Stack<Label>();
            Stack<Label> whileEnds = new Stack<Label>();

            ilGenerator.DeclareLocal(typeof(byte[]));
            ilGenerator.DeclareLocal(typeof(int));

            ilGenerator.Emit(OpCodes.Ldc_I4, 30000);
            ilGenerator.Emit(OpCodes.Newarr, typeof(byte));
            ilGenerator.Emit(OpCodes.Stloc_0);

            ilGenerator.Emit(OpCodes.Ldc_I4_0);
            ilGenerator.Emit(OpCodes.Stloc_1);

            foreach (var token in tokens)
            {
                switch (token.OpCode)
                {
                    case OpCode.Next:
                        ilGenerator.Emit(OpCodes.Ldloc_1);
                        ilGenerator.Emit(OpCodes.Ldc_I4, (int)token.Value);
                        ilGenerator.Emit(OpCodes.Add);
                        ilGenerator.Emit(OpCodes.Stloc_1);
                        break;
                    case OpCode.Prev:
                        ilGenerator.Emit(OpCodes.Ldloc_1);
                        ilGenerator.Emit(OpCodes.Ldc_I4, (int)token.Value);
                        ilGenerator.Emit(OpCodes.Sub);
                        ilGenerator.Emit(OpCodes.Stloc_1);
                        break;
                    case OpCode.Decr:
                        ilGenerator.Emit(OpCodes.Ldloc_0);
                        ilGenerator.Emit(OpCodes.Ldloc_1);
                        ilGenerator.Emit(OpCodes.Ldloc_0);
                        ilGenerator.Emit(OpCodes.Ldloc_1);
                        ilGenerator.Emit(OpCodes.Ldelem, typeof(byte));
                        ilGenerator.Emit(OpCodes.Ldc_I4, (int)token.Value);
                        ilGenerator.Emit(OpCodes.Sub);
                        ilGenerator.Emit(OpCodes.Stelem, typeof(byte));
                        break;
                    case OpCode.Incr:
                        ilGenerator.Emit(OpCodes.Ldloc_0);
                        ilGenerator.Emit(OpCodes.Ldloc_1);
                        ilGenerator.Emit(OpCodes.Ldloc_0);
                        ilGenerator.Emit(OpCodes.Ldloc_1);
                        ilGenerator.Emit(OpCodes.Ldelem, typeof(byte));
                        ilGenerator.Emit(OpCodes.Ldc_I4, (int)token.Value);
                        ilGenerator.Emit(OpCodes.Add);
                        ilGenerator.Emit(OpCodes.Stelem, typeof(byte));
                        break;
                    case OpCode.Prnt:
                        ilGenerator.Emit(OpCodes.Ldloc_0);
                        ilGenerator.Emit(OpCodes.Ldloc_1);
                        ilGenerator.Emit(OpCodes.Ldelem_U1);
                        ilGenerator.Emit(OpCodes.Call, _consoleWrite);
                        break;
                    case OpCode.Read:
                        ilGenerator.Emit(OpCodes.Ldloc_0);
                        ilGenerator.Emit(OpCodes.Ldloc_1);
                        ilGenerator.Emit(OpCodes.Call, _consoleRead);
                        ilGenerator.Emit(OpCodes.Stelem, typeof(byte));
                        break;
                    case OpCode.Whl_B:
                        var begin = ilGenerator.DefineLabel();
                        var end = ilGenerator.DefineLabel();
                        whileBegins.Push(begin);
                        whileEnds.Push(end);
                        ilGenerator.MarkLabel(begin);
                        ilGenerator.Emit(OpCodes.Ldloc_0);
                        ilGenerator.Emit(OpCodes.Ldloc_1);
                        ilGenerator.Emit(OpCodes.Ldelem_U1);
                        ilGenerator.Emit(OpCodes.Brfalse, end);
                        break;
                    case OpCode.Whl_E:
                        begin = whileBegins.Pop();
                        end = whileEnds.Pop();
                        ilGenerator.Emit(OpCodes.Br, begin);
                        ilGenerator.MarkLabel(end);
                        break;
                }
            }
            ilGenerator.Emit(OpCodes.Ret);
        }
    }
}
