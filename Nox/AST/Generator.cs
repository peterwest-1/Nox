
using System;
using System.IO;
using System.Text;

namespace Nox.AST
{
    internal class Generator
    {

        public static void Generate(string outputDir)
        {
            DefineAST(outputDir, "Expr", [
              "Binary   : Expr left, Token op, Expr right",
              "Grouping : Expr expression",
              "Literal  : Object value",
              "Unary    : Token op, Expr right"
            ]);
        }

        private static void DefineAST(string outputDir, string baseName, List<string> types)
        {
            string path = outputDir + "/" + baseName + ".cs";
            using var writer = new StreamWriter(path, false, Encoding.UTF8);

            writer.WriteLine($"using Nox;");
            writer.WriteLine();

            writer.WriteLine($"public abstract class {baseName}");
            writer.WriteLine("{");

            foreach (string type in types)
            {
                string className = type.Split(":")[0].Trim();
                string fields = type.Split(":")[1].Trim();
                DefineType(writer, baseName, className, fields);
            }

            writer.WriteLine("}");
        }

        private static void DefineType(StreamWriter writer, string baseName, string className, string fieldList)
        {
            writer.WriteLine("    class " + className + " : " + baseName);
            writer.WriteLine("    {");
            // Constructor.
            writer.WriteLine("        public " + className + "(" + fieldList + ")");
            writer.WriteLine("        {");

            // Store parameters in fields.
            string[] fields = fieldList.Split(", ");
            foreach (string field in fields)
            {
                string name = field.Split(" ")[1];
                writer.WriteLine("            this." + name + " = " + name + ";");
            }

            writer.WriteLine("        }");

            // Fields.
            writer.WriteLine();
            foreach (string field in fields)
            {
                writer.WriteLine("        public " + field + ";");
            }

            writer.WriteLine("    }");
        }
    }
}
