﻿using Nox;

public abstract class Stmt
{
    public interface IVisitor<T>
    {
        T VisitBlockStmt(Block stmt);
        T VisitClassStmt(Class stmt);
        T VisitExpressionStmt(Expression stmt);
        T VisitFunctionStmt(Function stmt);
        T VisitIfStmt(If stmt);
        T VisitPrintStmt(Print stmt);
        T VisitReturnStmt(Return stmt);
        T VisitVarStmt(Var stmt);
        T VisitWhileStmt(While stmt);
    }
    public class Block : Stmt
    {
        public Block(List<Stmt> statements)
        {
            this.statements = statements;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitBlockStmt(this);
        }

        public List<Stmt> statements;
    }
    public class Class : Stmt
    {
        public Class(Token name, Expr.Variable superclass, List<Stmt.Function> methods)
        {
            this.name = name;
            this.superclass = superclass;
            this.methods = methods;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitClassStmt(this);
        }

        public Token name;
        public Expr.Variable superclass;
        public List<Stmt.Function> methods;
    }
    public class Expression : Stmt
    {
        public Expression(Expr expression)
        {
            this.expression = expression;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitExpressionStmt(this);
        }

        public Expr expression;
    }
    public class Function : Stmt
    {
        public Function(Token name, List<Token> paras, List<Stmt> body)
        {
            this.name = name;
            this.paras = paras;
            this.body = body;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitFunctionStmt(this);
        }

        public Token name;
        public List<Token> paras;
        public List<Stmt> body;
    }
    public class If : Stmt
    {
        public If(Expr condition, Stmt thenBranch, Stmt elseBranch)
        {
            this.condition = condition;
            this.thenBranch = thenBranch;
            this.elseBranch = elseBranch;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitIfStmt(this);
        }

        public Expr condition;
        public Stmt thenBranch;
        public Stmt elseBranch;
    }
    public class Print : Stmt
    {
        public Print(Expr expression)
        {
            this.expression = expression;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitPrintStmt(this);
        }

        public Expr expression;
    }
    public class Return : Stmt
    {
        public Return(Token keyword, Expr value)
        {
            this.keyword = keyword;
            this.value = value;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitReturnStmt(this);
        }

        public Token keyword;
        public Expr value;
    }
    public class Var : Stmt
    {
        public Var(Token name, Expr initializer)
        {
            this.name = name;
            this.initializer = initializer;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitVarStmt(this);
        }

        public Token name;
        public Expr initializer;
    }
    public class While : Stmt
    {
        public While(Expr condition, Stmt body)
        {
            this.condition = condition;
            this.body = body;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitWhileStmt(this);
        }

        public Expr condition;
        public Stmt body;
    }

    public abstract T Accept<T>(IVisitor<T> visitor);
}
