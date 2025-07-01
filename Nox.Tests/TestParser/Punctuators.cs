

//using Nox.AST;

//namespace Nox.Tests.TestParser
//{
//    public class Punctuators
//    {
//        [Fact]
//        public void TestParsePrintPunctuators()
//        {
//            //Arrange
//            string input = "(\"foo\")";
//            Tokenizer tokenizer = new(input);
//            List<Token> tokens = tokenizer.Tokenize();

//            //Act 
//            Parser parser = new(tokens);
//            Expr expression = parser.Parse();
//            string result = new Printer().Print(expression);

//            //Assert
//            Assert.Equal("(group foo)", result);

//        }

//        [Fact]
//        public void TestParsePrintUnaryOperators()
//        {
//            //Arrange
//            string input = "!true";
//            Tokenizer tokenizer = new(input);
//            List<Token> tokens = tokenizer.Tokenize();

//            //Act 
//            Parser parser = new(tokens);
//            Expr expression = parser.Parse();
//            string result = new Printer().Print(expression);

//            //Assert
//            Assert.Equal("(! true)", result);

//        }

//        [Fact]
//        public void TestParsePrintArithmeticOperatorsOne()
//        {
//            //Arrange
//            string input = "16 * 38 / 58";
//            Tokenizer tokenizer = new(input);
//            List<Token> tokens = tokenizer.Tokenize();

//            //Act 
//            Parser parser = new(tokens);
//            Expr expression = parser.Parse();
//            string result = new Printer().Print(expression);

//            //Assert
//            Assert.Equal("(/ (* 16.0 38.0) 58.0)", result);

//        }

//        [Fact]
//        public void TestParsePrintArithmeticOperatorsTwo()
//        {
//            //Arrange
//            string input = "52 + 80 - 94";
//            Tokenizer tokenizer = new(input);
//            List<Token> tokens = tokenizer.Tokenize();

//            //Act 
//            Parser parser = new(tokens);
//            Expr expression = parser.Parse();
//            string result = new Printer().Print(expression);

//            //Assert
//            Assert.Equal("(- (+ 52.0 80.0) 94.0)", result);

//        }

//        [Fact]
//        public void TestParsePrintComparisonOperatorsTwo()
//        {
//            //Arrange
//            string input = "83 < 99 < 115";
//            Tokenizer tokenizer = new(input);
//            List<Token> tokens = tokenizer.Tokenize();

//            //Act 
//            Parser parser = new(tokens);
//            Expr expression = parser.Parse();
//            string result = new Printer().Print(expression);

//            //Assert
//            Assert.Equal("(< (< 83.0 99.0) 115.0)", result);

//        }

//        [Fact]
//        public void TestParsePrintEqualityOperatorsTwo()
//        {
//            //Arrange
//            string input = "\"baz\" == \"baz\"";
//            Tokenizer tokenizer = new(input);
//            List<Token> tokens = tokenizer.Tokenize();

//            //Act 
//            Parser parser = new(tokens);
//            Expr expression = parser.Parse();
//            string result = new Printer().Print(expression);

//            //Assert
//            Assert.Equal("(== baz baz)", result);

//        }
//    }
//}
