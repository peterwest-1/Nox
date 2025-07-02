
namespace Nox.ResolverImpl
{
    internal partial class Resolver : Expr.IVisitor<object>
    {
        public object VisitAssignExpr(Expr.Assign expr)
        {
            Resolve(expr.value);
            ResolveLocal(expr, expr.name);
            return null;
        }

        public object VisitBinaryExpr(Expr.Binary expr)
        {
            Resolve(expr.left);
            Resolve(expr.right);
            return null;
        }

        public object VisitCallExpr(Expr.Call expr)
        {
            Resolve(expr.callee);

            foreach (Expr argument in expr.arguments)
            {
                Resolve(argument);
            }

            return null;
        }
        public object VisitGetExpr(Expr.Get expr)
        {
            Resolve(expr.obj);
            return null!;
        }

        public object VisitGroupingExpr(Expr.Grouping expr)
        {
            Resolve(expr.expression);
            return null;
        }

        public object VisitLiteralExpr(Expr.Literal expr)
        {
            return null!;
        }

        public object VisitLogicalExpr(Expr.Logical expr)
        {
            Resolve(expr.left);
            Resolve(expr.right);
            return null!;
        }

        public object VisitSetExpr(Expr.Set expr)
        {
            Resolve(expr.value);
            Resolve(expr.obj);
            return null!;
        }

        public object VisitSuperExpr(Expr.Super expr)
        {
            if (currentClass == ClassType.NONE)
            {
                Nox.Error(expr.keyword, $"Can't use '{Keywords.SUPER}' outside of a class.");
            }
            else if (currentClass != ClassType.SUBCLASS)
            {
                Nox.Error(expr.keyword, $"Can't use '{Keywords.SUPER}' in a class with no superclass.");
            }
            ResolveLocal(expr, expr.keyword);
            return null;
        }

        public object VisitThisExpr(Expr.This expr)
        {
            if (currentClass == ClassType.NONE)
            {
                Nox.Error(expr.keyword, $"Can't use '{Keywords.THIS}' outside of a class.");
                return null!;
            }

            ResolveLocal(expr, expr.keyword);
            return null!;
        }

        public object VisitUnaryExpr(Expr.Unary expr)
        {
            Resolve(expr.right);
            return null!;
        }

        public object VisitVariableExpr(Expr.Variable expr)
        {
            if (scopes.Count != 0 && scopes.Peek().GetValueOrDefault(expr.name.lexeme) == false) // potential issue here boolean.FALSE
            {
                Nox.Error(expr.name, "Can't read local variable in its own initializer.");
            }

            ResolveLocal(expr, expr.name);
            return null!;
        }
    }
}
