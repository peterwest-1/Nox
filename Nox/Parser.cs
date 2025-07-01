
using System;
using System.Security.Claims;
using static Expr;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Nox
{
    internal class Parser
    {

        private class ParseError : Exception { }


        private List<Token> tokens;
        private int current = 0;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public Expr Parse()
        {
            try
            {
                return Expression();
            }
            catch (ParseError)
            {
                return null;
            }
        }

        private Expr Expression()
        {
            return Equality();
        }

        private Expr Equality()
        {
            Expr expr = Comparison();

            while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
            {
                Token op = Previous();
                Expr right = Comparison();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Comparison()
        {
            Expr expr = Term();

            while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
            {
                Token op = Previous();
                Expr right = Term();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Term()
        {
            Expr expr = Factor();

            while (Match(TokenType.MINUS, TokenType.PLUS))
            {
                Token op = Previous();
                Expr right = Factor();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Factor()
        {
            Expr expr = Unary();

            while (Match(TokenType.SLASH, TokenType.STAR))
            {
                Token op = Previous();
                Expr right = Unary();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Unary()
        {
            if (Match(TokenType.BANG, TokenType.MINUS))
            {
                Token op = Previous();
                Expr right = Unary();
                return new Expr.Unary(op, right);
            }

            return Primary();
        }

        private Expr Primary()
        {
            if (Match(TokenType.FALSE)) return new Expr.Literal(false);
            if (Match(TokenType.TRUE)) return new Expr.Literal(true);
            if (Match(TokenType.NIL)) return new Expr.Literal(null);

            //if (Match(TokenType.NUMBER))
            //{
            //    var literal = Previous().literal;
            //    if (literal == null)
            //    {
            //        //var error = new NoxError(NoxErrorType.PARSE_ERROR_EXPECT_EXPRESSION, current, Previous().lexeme);
            //        //throw new NoxParseException(Previous(), "NUMBER token has null literal value.", error);
            //        //TODO handle this case properly
            //    }

            //    double numericValue;
            //    if (literal is double d)
            //        numericValue = d;
            //    else if (literal is int i)
            //        numericValue = (double)i;
            //    else if (literal is string s && double.TryParse(s, out double parsed))
            //        numericValue = parsed;
            //    else
            //    {
            //        //var error = new NoxError(NoxErrorType.PARSE_ERROR_EXPECT_EXPRESSION, current, Previous().lexeme);
            //        //throw new NoxParseException(Previous(), $"Unable to convert NUMBER literal to double: {literal}", error);
            //        //TODO handle this case properly
            //        throw new Exception();
            //    }

            //    return new Literal(numericValue);
            //}

            if (Match(TokenType.NUMBER, TokenType.STRING))
            {
                return new Literal(Previous().literal);
            }

            if (Match(TokenType.LEFT_PAREN))
            {
                Expr expr = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
                return new Grouping(expr);
            }

            throw Error(Peek(), "Expect expression.");
        }

        private Token Consume(TokenType type, string message)
        {
            if (Check(type)) return Advance();
            throw Error(Peek(), message);
        }

        private static ParseError Error(Token token, string message)
        {
            Nox.Error(token, message);
            return new ParseError();
        }


        private void Synchronize()
        {
            Advance();

            while (!IsAtEnd())
            {
                if (Previous().type == TokenType.SEMICOLON) return;

                switch (Peek().type)
                {
                    case TokenType.CLASS:
                    case TokenType.FUN:
                    case TokenType.VAR:
                    case TokenType.FOR:
                    case TokenType.IF:
                    case TokenType.WHILE:
                    case TokenType.PRINT:
                    case TokenType.RETURN:
                        return;
                }

                Advance();
            }
        }


        private bool Match(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }

            return false;
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;
            return Peek().type == type;
        }

        private Token Advance()
        {
            if (!IsAtEnd()) current++;
            return Previous();
        }

        private bool IsAtEnd()
        {
            return Peek().type == TokenType.EOF;
        }

        private Token Peek()
        {
            return tokens.ElementAt(current);
        }

        private Token Previous()
        {
            return tokens.ElementAt(current - 1);
        }
    }
}
