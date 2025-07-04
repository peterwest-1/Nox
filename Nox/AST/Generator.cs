﻿
using System.Text;

namespace Nox.AST
{
    internal class Generator
    {

        public static void Generate(string outputDirectory)
        {
            DefineAST(outputDirectory, "Expr", [
              "Assign   : Token name, Expr value",
              "Binary   : Expr left, Token op, Expr right",
              "Call     : Expr callee, Token paren, List<Expr> arguments",
              "Get      : Expr obj, Token name",
              "Grouping : Expr expression",
              "Literal  : object value",
              "Logical  : Expr left, Token op, Expr right",
              "Set      : Expr obj, Token name, Expr value",
              "Super    : Token keyword, Token method",
              "This     : Token keyword",
              "Unary    : Token op, Expr right",
              "Variable : Token name"
            ]);

            DefineAST(outputDirectory, "Stmt", [
              "Block      : List<Stmt> statements",
              "Class      : Token name, Expr.Variable superclass, List<Stmt.Function> methods",
              "Expression : Expr expression",
              "Function   : Token name, List<Token> paras, List<Stmt> body",
              "If         : Expr condition, Stmt thenBranch, Stmt elseBranch",
              "Print      : Expr expression",
              "Return     : Token keyword, Expr value",
              "Var        : Token name, Expr initializer",
              "While      : Expr condition, Stmt body"
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

            DefineVisitor(writer, baseName, types);

            foreach (string type in types)
            {
                string className = type.Split(":")[0].Trim();
                string fields = type.Split(":")[1].Trim();
                DefineType(writer, baseName, className, fields);
            }

            writer.WriteLine();
            writer.WriteLine("    public abstract T Accept<T>(IVisitor<T> visitor);");


            writer.WriteLine("}");
        }

        private static void DefineType(StreamWriter writer, string baseName, string className, string fieldList)
        {
            writer.WriteLine("    public class " + className + " : " + baseName);
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

            writer.WriteLine("        public override T Accept<T>(IVisitor<T> visitor)");
            writer.WriteLine("        {");
            writer.WriteLine("            return visitor.Visit" + className + baseName + "(this);");
            writer.WriteLine("        }");

            // Fields.
            writer.WriteLine();
            foreach (string field in fields)
            {
                writer.WriteLine("        public " + field + ";");
            }

            writer.WriteLine("    }");
        }

        private static void DefineVisitor(StreamWriter writer, string baseName, List<string> types)
        {
            writer.WriteLine("    public interface IVisitor<T>");
            writer.WriteLine("    {");

            foreach (string type in types)
            {
                string typeName = type.Split(":")[0].Trim();
                writer.WriteLine("        T Visit" + typeName + baseName + "(" +
                    typeName + " " + baseName.ToLower() + ");");
            }

            writer.WriteLine("    }");
        }
    }
}
