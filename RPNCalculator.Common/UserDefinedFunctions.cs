using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RPNCalculator.Common
{
    internal class UserDefinedFunctions
    {
        public double? CreateFunction(string function, ref Stack<string> stack)
        {
            string baseFunction = @"
                
                using System;
                using System.Collections.Generic;

                namespace UserDefinedFunctions
                {
                    public class UDF
                    {
                        replace
                    }
                }";

            string completeFunction = baseFunction.Replace("replace", function);
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(completeFunction);
            string assemblyName = Path.GetRandomFileName();
            MetadataReference[] references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Collections.Generic.Stack<string>).Assembly.Location),
            };

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> diagnostics = result.Diagnostics;

                    //foreach (Diagnostic diagnostic in diagnostics)
                    //{
                    //    MessageBox.Show(diagnostic.Id + ":" + diagnostic.GetMessage() + "\r\n");
                    //}
                    return null;
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());
                    Type type = assembly.GetType("UserDefinedFunctions.UDF");
                    object obj = Activator.CreateInstance(type);
                    object returnVal = type.InvokeMember("entryFunction",
                        BindingFlags.Default | BindingFlags.InvokeMethod,
                        null,
                        obj,
                        new object[] { stack });
                    return (double)returnVal;
                }


            }
        }
    }
}
