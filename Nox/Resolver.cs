
using System;

namespace Nox
{
    internal class Resolver : Expr.IVisitor<object>, Stmt.IVisitor<object>
    {
        enum FunctionType
        {
            NONE,
            FUNCTION,
            METHOD,
            INITIALIZER,
        }

        enum ClassType
        {
            NONE,
            CLASS
        }

        private readonly Interpreter interpreter;

        private readonly Stack<Dictionary<string, bool>> scopes = [];
        private FunctionType currentFunction = FunctionType.NONE;
        private ClassType currentClass = ClassType.NONE;


        public Resolver(Interpreter interpreter)
        {
            this.interpreter = interpreter;
        }

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

        public object VisitBlockStmt(Stmt.Block stmt)
        {
            BeginScope();
            Resolve(stmt.statements);
            EndScope();
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

        public object VisitGroupingExpr(Expr.Grouping expr)
        {
            Resolve(expr.expression);
            return null;
        }

        public object VisitIfStmt(Stmt.If stmt)
        {
            Resolve(stmt.condition);
            Resolve(stmt.thenBranch);
            if (stmt.elseBranch != null) Resolve(stmt.elseBranch);
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
                    Nox.Error(stmt.keyword,
                        "Can't return a value from an initializer.");
                }
                Resolve(stmt.value);
            }

            return null;
        }

        public object VisitUnaryExpr(Expr.Unary expr)
        {
            Resolve(expr.right);
            return null!;
        }

        public object VisitVariableExpr(Expr.Variable expr)
        {
            if (scopes.Count != 0 &&
                        scopes.Peek().GetValueOrDefault(expr.name.lexeme) == false) // potential issue here boolean.FALSE
            {
                Nox.Error(expr.name,
                    "Can't read local variable in its own initializer.");
            }

            ResolveLocal(expr, expr.name);
            return null!;
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

        private void Resolve(Expr expr) => expr.Accept(this);

        private void Resolve(Stmt stmt) => stmt.Accept(this);

        public void Resolve(List<Stmt> statements)
        {
            foreach (Stmt statement in statements)
            {
                Resolve(statement);
            }
        }

        private void BeginScope() => scopes.Push([]);

        private void EndScope() => scopes.Pop();

        private void Declare(Token name)
        {
            if (scopes.Count == 0) return;
            Dictionary<string, bool> scope = scopes.Peek();
            if (scope.ContainsKey(name.lexeme))
            {
                Nox.Error(name,
                    "Already a variable with this name in this scope.");
            }
            scope.Add(name.lexeme, false);
        }

        private void Define(Token name)
        {
            if (scopes.Count == 0) return;
            scopes.Peek().Add(name.lexeme, true);
        }

        private void ResolveLocal(Expr expr, Token name)
        {
            for (int i = scopes.Count - 1; i >= 0; i--)
            {
                if (scopes.ElementAt(i).ContainsKey(name.lexeme))
                {
                    interpreter.Resolve(expr, scopes.Count - 1 - i);
                    return;
                }
            }
        }

        private void ResolveFunction(Stmt.Function function, FunctionType type)
        {
            FunctionType enclosingFunction = currentFunction;
            currentFunction = type;

            BeginScope();
            foreach (Token param in function.paras)
            {
                Declare(param);
                Define(param);
            }
            Resolve(function.body);
            EndScope();
            currentFunction = enclosingFunction;
        }

        public object VisitClassStmt(Stmt.Class stmt)
        {
            ClassType enclosingClass = currentClass;
            currentClass = ClassType.CLASS;

            Declare(stmt.name);
            Define(stmt.name);

            BeginScope();
            scopes.Peek().Add("this", true);

            foreach (Stmt.Function method in stmt.methods)
            {
                FunctionType declaration = FunctionType.METHOD;
                if (method.name.lexeme.Equals("init"))
                {
                    declaration = FunctionType.INITIALIZER;
                }

                ResolveFunction(method, declaration);
            }

            EndScope();

            currentClass = enclosingClass;
            return null!;
        }

        public object VisitGetExpr(Expr.Get expr)
        {
            Resolve(expr.obj);
            return null!;
        }

        public object VisitSetExpr(Expr.Set expr)
        {
            Resolve(expr.value);
            Resolve(expr.obj);
            return null!;
        }

        public object VisitThisExpr(Expr.This expr)
        {
            if (currentClass == ClassType.NONE)
            {
                Nox.Error(expr.keyword,
                    "Can't use 'this' outside of a class.");
                return null!;
            }

            ResolveLocal(expr, expr.keyword);
            return null!;
        }
    }
}
