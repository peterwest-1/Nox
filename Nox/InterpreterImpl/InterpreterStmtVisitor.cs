
namespace Nox.InterpreterImpl
{
    internal partial class Interpreter : Stmt.IVisitor<object>
    {
        public object VisitBlockStmt(Stmt.Block stmt)
        {
            ExecuteBlock(stmt.statements, new Environment(environment));
            return null;
        }

        public object VisitClassStmt(Stmt.Class stmt)
        {
            object superclass = null;
            if (stmt.superclass != null)
            {
                superclass = Evaluate(stmt.superclass);
                if ((superclass is not NoxClass))
                {
                    throw new NoxRuntimeException(stmt.superclass.name,
                        "Superclass must be a class.");
                }
            }

            environment.Define(stmt.name.lexeme, null);

            if (stmt.superclass != null)
            {
                environment = new Environment(environment);
                environment.Define(Keywords.SUPER, superclass);
            }



            Dictionary<string, NoxFunction> methods = [];
            foreach (Stmt.Function method in stmt.methods)
            {
                NoxFunction function = new(method, environment, method.name.lexeme.Equals(Keywords.INIT));
                methods[method.name.lexeme] = function;
            }

            NoxClass klass = new(stmt.name.lexeme, (NoxClass)superclass, methods);

            if (superclass != null)
            {
                environment = environment.enclosing;
            }

            environment.Assign(stmt.name, klass);
            return null;
        }

        public object VisitExpressionStmt(Stmt.Expression stmt)
        {
            Evaluate(stmt.expression);
            return null;
        }

        public object VisitFunctionStmt(Stmt.Function stmt)
        {
            NoxFunction function = new(stmt, environment, false);
            environment.Define(stmt.name.lexeme, function);
            return null;
        }

        public object VisitIfStmt(Stmt.If stmt)
        {
            if (IsTruthy(Evaluate(stmt.condition)))
            {
                Execute(stmt.thenBranch);
            }
            else if (stmt.elseBranch != null)
            {
                Execute(stmt.elseBranch);
            }
            return null;
        }

        public object VisitPrintStmt(Stmt.Print stmt)
        {
            object value = Evaluate(stmt.expression);
            Console.WriteLine(Stringify(value));
            return null;
        }

        public object VisitReturnStmt(Stmt.Return stmt)
        {
            object value = null;
            if (stmt.value != null) value = Evaluate(stmt.value);

            throw new NoxReturnException(value);
        }

        public object VisitVarStmt(Stmt.Var stmt)
        {
            object value = null;
            if (stmt.initializer != null)
            {
                value = Evaluate(stmt.initializer);
            }

            environment.Define(stmt.name.lexeme, value);
            return null;
        }

        public object VisitWhileStmt(Stmt.While stmt)
        {
            while (IsTruthy(Evaluate(stmt.condition)))
            {
                Execute(stmt.body);
            }
            return null;
        }
    }
}
