﻿

namespace Nox.Tests.TestTokenizer
{
    public class Numbers
    {
        [Fact]
        public void TestTokenizeNumbersAll()
        {
            //Arrange
            string input = "123\r\n123.456\r\n.456\r\n123.";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(7, tokens.Count);

            Assert.Equal("NUMBER 123 123.0", queue.Dequeue().ToString());
            Assert.Equal("NUMBER 123.456 123.456", queue.Dequeue().ToString());
            Assert.Equal("DOT . null", queue.Dequeue().ToString());
            Assert.Equal("NUMBER 456 456.0", queue.Dequeue().ToString());
            Assert.Equal("NUMBER 123 123.0", queue.Dequeue().ToString());
            Assert.Equal("DOT . null", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());

        }

        [Fact]
        public void TestTokenizeNumbersIntegers()
        {
            //Arrange
            string input = "123";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(2, tokens.Count);

            Assert.Equal("NUMBER 123 123.0", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());

        }

        [Fact]
        public void TestTokenizeNumbersWithDot()
        {
            //Arrange
            string input = "123.456";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(2, tokens.Count);


            Assert.Equal("NUMBER 123.456 123.456", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());

        }

        [Fact]
        public void TestTokenizeNumbersEndingOnDot()
        {
            //Arrange
            string input = "123.";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(3, tokens.Count);


            Assert.Equal("NUMBER 123 123.0", queue.Dequeue().ToString());
            Assert.Equal("DOT . null", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());

        }

        [Fact]
        public void TestTokenizeNumbersLiteralsOne()
        {
            //Arrange
            string input = "42";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(2, tokens.Count);

            Assert.Equal("NUMBER 42 42.0", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());

        }

        [Fact]
        public void TestTokenizeNumbersLiteralsTwo()
        {
            //Arrange
            string input = "1234.1234";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(2, tokens.Count);

            Assert.Equal("NUMBER 1234.1234 1234.1234", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());

        }
    }
}
