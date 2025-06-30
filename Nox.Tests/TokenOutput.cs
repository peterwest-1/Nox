using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Nox.Tests
{
    internal class TokenOutput
    {
        public static string Get(TokenType type)
        {
            return type switch
            {
                TokenType.LEFT_PAREN => "LEFT_PAREN ( null",
                TokenType.RIGHT_PAREN => "RIGHT_PAREN ) null",
                TokenType.LEFT_BRACE => "LEFT_BRACE { null",
                TokenType.RIGHT_BRACE => "RIGHT_BRACE } null",
                TokenType.COMMA => "COMMA , null",
                TokenType.DOT => "DOT . null",
                TokenType.MINUS => "MINUS - null",
                TokenType.PLUS => "PLUS + null",
                TokenType.SEMICOLON => "SEMICOLON ; null",
                TokenType.SLASH => "SLASH / null",
                TokenType.STAR => "STAR * null",
                TokenType.BANG => "BANG ! null",
                TokenType.BANG_EQUAL => "BANG_EQUAL != null",
                TokenType.EQUAL => "EQUAL = null",
                TokenType.EQUAL_EQUAL => "EQUAL_EQUAL == null",
                TokenType.GREATER => "GREATER > null",
                TokenType.GREATER_EQUAL => "GREATER_EQUAL >= null",
                TokenType.LESS => "LESS < null",
                TokenType.LESS_EQUAL => "LESS_EQUAL <= null",
                _ => throw new NotImplementedException()
            };

        }
    }
}

/*
        TokenType.LEFT_PAREN => "LEFT_PAREN ( null",
        TokenType.RIGHT_PAREN => "RIGHT_PAREN ) null",
        TokenType.LEFT_BRACE => "LEFT_BRACE { null",
        TokenType.RIGHT_BRACE => "RIGHT_BRACE } null",
        TokenType.COMMA => "COMMA , null",
        TokenType.DOT => "DOT . null",
        TokenType.MINUS => "MINUS - null",
        TokenType.PLUS => "PLUS + null",
        TokenType.SEMICOLON => "SEMICOLON ; null",
        TokenType.SLASH => "SLASH / null",
        TokenType.STAR => "STAR * null",
        TokenType.BANG => "BANG ! null",
        TokenType.BANG_EQUAL => "BANG_EQUAL != null",
        TokenType.EQUAL => "EQUAL = null",
        TokenType.EQUAL_EQUAL => "EQUAL_EQUAL == null",
        TokenType.GREATER => "GREATER > null",
        TokenType.GREATER_EQUAL => "GREATER_EQUAL >= null",
        TokenType.LESS => "LESS < null",
        TokenType.LESS_EQUAL => "LESS_EQUAL <= null"
 */