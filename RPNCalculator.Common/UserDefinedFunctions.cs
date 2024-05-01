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
        /*
         * Author: Craig Price
         * Description: This method creates and compiles a user defined function from the string and stack reference provided.
         * Responsibility: Backend work for the Calculator
         * Certification: I certify that I wrote this code myself.
         */
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
            string assemblyLocation = Path.GetRandomFileName();
            MetadataReference[] references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Collections.Generic.Stack<string>).Assembly.Location),
            };

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyLocation,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    return null;
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());
                    Type type = assembly.GetType("UserDefinedFunctions.UDF");
                    object instance = Activator.CreateInstance(type);
                    object returnVal = type.InvokeMember("entryFunction",
                        BindingFlags.Default | BindingFlags.InvokeMethod,
                        null,
                        instance,
                        new object[] { stack });
                    return (double)returnVal;
                }


            }
        }
    }
}
