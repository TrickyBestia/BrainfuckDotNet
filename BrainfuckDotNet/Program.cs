using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace BrainfuckDotNet
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 1 || !File.Exists(args[0]))
            {
                Console.WriteLine("You must specify correct source code file.");
                Environment.Exit(1);
            }

            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(Path.GetFileNameWithoutExtension(args[0])), AssemblyBuilderAccess.RunAndSave);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule($"{Path.GetFileNameWithoutExtension(args[0])}.exe");
            var classBuilder = moduleBuilder.DefineType("Program", TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.Class);
            var methodBuilder = classBuilder.DefineMethod("Main", MethodAttributes.Static | MethodAttributes.Private);
            assemblyBuilder.SetEntryPoint(methodBuilder);

            IEnumerable<Token> source = null;

            Console.WriteLine("Tokenizing...");
            using (FileStream fs = new FileStream(args[0], FileMode.Open))
                source = Lexer.Tokenize(fs).ToList();

            Console.WriteLine("Optimizing...");
            var optimizedSource = Optimizer.Optimize(source).ToList();
            Console.WriteLine("Parsing...");
            Parser.Parse(optimizedSource, methodBuilder.GetILGenerator());

            Console.WriteLine("Compiling...");
            classBuilder.CreateType();
            assemblyBuilder.Save($"{Path.GetFileNameWithoutExtension(args[0])}.exe");
            File.Move($"{Path.GetFileNameWithoutExtension(args[0])}.exe", Path.Combine(Path.GetDirectoryName(args[0]), $"{Path.GetFileNameWithoutExtension(args[0])}.exe"));
            Console.WriteLine("Done!");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}
