
namespace Nox.ResolverImpl
{
    internal partial class Resolver : Stmt.IVisitor<object>
    {
        public object VisitBlockStmt(Stmt.Block stmt)
        {
            BeginScope();
            Resolve(stmt.statements);
            EndScope();
            return null;
        }

        public object VisitClassStmt(Stmt.Class stmt)
        {
            ClassType enclosingClass = currentClass;
            currentClass = ClassType.CLASS;
            Declare(stmt.name);
            Define(stmt.name);

            if (stmt.superclass != null &&
                stmt.name.lexeme.Equals(stmt.superclass.name.lexeme))
            {
                Nox.Error(stmt.superclass.name,
                    "A class can't inherit from itself.");
            }


            if (stmt.superclass != null)
            {
                currentClass = ClassType.SUBCLASS;
                Resolve(stmt.superclass);
            }

            if (stmt.superclass != null)
            {
                BeginScope();
                scopes.Peek()[Keywords.SUPER] = true;
            }

            BeginScope();
            scopes.Peek()[Keywords.THIS] = true;

            foreach (Stmt.Function method in stmt.methods)
            {
                FunctionType declaration = FunctionType.METHOD;
                if (method.name.lexeme.Equals(Keywords.INIT))
                {
                    declaration = FunctionType.INITIALIZER;
                }

                ResolveFunction(method, declaration);
            }

            EndScope();

            if (stmt.superclass != null) EndScope();

            currentClass = enclosingClass;
            return null!;
        }

        public object VisitExpressionStmt(Stmt.Expression stmt)
        {
            Resolve(stmt.expression);
            return null;
        }

        public object VisitFunctionStmt(Stmt.Function stmt)
        {
            Declare(stmt.name);
            Define(stmt.name);

            ResolveFunction(stmt, FunctionType.FUNCTION);
            return null;
        }

        public object VisitIfStmt(Stmt.If stmt)
        {
            Resolve(stmt.condition);
            Resolve(stmt.thenBranch);
            if (stmt.elseBranch != null) Resolve(stmt.elseBranch);
            return null;
        }

        public object VisitPrintStmt(Stmt.Print stmt)
        {
            Resolve(stmt.expression);
            return null!;
        }

        public object VisitReturnStmt(Stmt.Return stmt)
        {
            if (currentFunction == FunctionType.NONE)
            {
                Nox.Error(stmt.keyword, "Can't return from top-level code.");
            }

            if (stmt.value != null)
            {
                if (currentFunction == FunctionType.INITIALIZER)
                {
                    Nox.Error(stmt.keyword, "Can't return a value from an initializer.");
                }
                Resolve(stmt.value);
            }

            return null;
        }
        public object VisitVarStmt(Stmt.Var stmt)
        {
            Declare(stmt.name);
            if (stmt.initializer != null)
            {
                Resolve(stmt.initializer);
            }
            Define(stmt.name);
            return null;
        }
        public object VisitWhileStmt(Stmt.While stmt)
        {
            Resolve(stmt.condition);
            Resolve(stmt.body);
            return null;
        }
    }
}
