

namespace Nox.Tests.TestInterpreter
{
    public class PrintTests : IDisposable
    {

        private readonly StringWriter _stringWriter;
        private readonly TextWriter _originalOutput;

        public PrintTests()
        {
            _stringWriter = new StringWriter();
            _originalOutput = Console.Out;
            Console.SetOut(_stringWriter);
        }

        private string InterpretExpression(string input)
        {
            //try
            //{
            var tokenizer = new Tokenizer(input);
            var tokens = tokenizer.Tokenize();

            var parser = new Parser(tokens);
            var expression = parser.Parse();

            var interpreter = new Interpreter();
            interpreter.Interpret(expression);

            return _stringWriter.ToString();
            //}
            //catch (Exception ex)
            //{
            //    // Re-throw with more context if needed
            //    throw new InvalidOperationException($"Failed to interpret expression: '{input}'", ex);
            //}
        }

        public void Dispose()
        {
            Console.SetOut(_originalOutput);
            _stringWriter?.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public void TestParsePrintEvaluations()
        {
            //Arrange
            string input = "2 + 3";

            //Act
            var result = InterpretExpression(input);

            //Assert
            Assert.Contains("5", result);

        }

        [Fact]
        public void TestParsePrintBooleans()
        {
            //Arrange
            string input = "true";

            //Act
            var result = InterpretExpression(input);

            //Assert
            Assert.Contains("true", result);
        }

        [Fact]
        public void TestParsePrintStrings()
        {
            //Arrange
            string input = "\"hello world!\"";

            //Act 
            var result = InterpretExpression(input);

            //Assert
            Assert.Contains("hello world!", result);
        }

        [Fact]
        public void TestParsePrintParensEvaluated()
        {
            //Arrange
            string input = "(\"hello world!\")";

            //Act 
            var result = InterpretExpression(input);

            //Assert
            Assert.Contains("hello world!", result);
        }

        [Fact]
        public void TestParsePrintNegationEvaluated()
        {
            //Arrange
            string input = "-(73)";

            //Act 
            var result = InterpretExpression(input);

            //Assert
            Assert.Contains("-73", result);
        }

        [Fact]
        public void TestParsePrintArithmeticOperatorsEvaluatedOne()
        {
            //Arrange
            string input = "(18 * 3 / (3 * 6))";

            //Act 
            var result = InterpretExpression(input);

            //Assert
            Assert.Contains("3", result);
        }

        [Fact]
        public void TestParsePrintArithmeticOperatorsEvaluatedTwo()
        {
            //Arrange
            string input = "20 + 74 - (-(14 - 33))";

            //Act 
            var result = InterpretExpression(input);

            //Assert
            Assert.Contains("75", result);
        }

        [Fact]
        public void TestParsePrintStringConcatEvaluated()
        {
            //Arrange
            string input = "\"hello\" + \" world!\"";

            //Act 
            var result = InterpretExpression(input);

            //Assert
            Assert.Contains("hello world!", result);
        }

        [Fact]
        public void TestParsePrintRelationalOperatorsEvaluated()
        {
            //Arrange
            string input = "10 > 5";

            //Act 
            var result = InterpretExpression(input);

            //Assert
            Assert.Contains("true", result);
        }


        [Fact]
        public void TestParsePrintEqualityOperatorsEvaluated()
        {
            //Arrange
            string input = "156 == (89 + 67)";

            //Act 
            var result = InterpretExpression(input);

            //Assert
            Assert.Contains("true", result);
        }

    }
}
