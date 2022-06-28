using Messerli.Lexer.Test.LexerRules;
using Xunit;

namespace Messerli.Lexer.Test
{
    public class PositionWithLinesTest
    {
        private const string ExampleTextWihtNewLines = "Hello\r\n\r\nThis is a test\nWe are on line four\nLine five\r\nthe end";

        [Fact]
        public void GiveALexerAndALineSeparatorThePositionsAreGivenInLineAndColumn()
        {
            var tokenizer = new Tokenizer(new WordTokenizerWithLines(), s => new LexerReader(s), lexemes => new LinePositionCalculator(lexemes));

            var lexemes = tokenizer.Scan(ExampleTextWihtNewLines);

            var positions = new LinePositionCalculator(lexemes);

            // hello on line 1
            Assert.Equal(1, positions.CalculateLinePosition(lexemes[0]).Line);
            Assert.Equal(1, positions.CalculateLinePosition(lexemes[0]).Column);
            Assert.Equal(5, positions.CalculateLinePosition(lexemes[0]).Length);

            // This on line 3
            Assert.Equal(3, positions.CalculateLinePosition(lexemes[3]).Line);
            Assert.Equal(1, positions.CalculateLinePosition(lexemes[3]).Column);
            Assert.Equal(4, positions.CalculateLinePosition(lexemes[3]).Length);

            // is on line 3
            Assert.Equal(3, positions.CalculateLinePosition(lexemes[5]).Line);
            Assert.Equal(6, positions.CalculateLinePosition(lexemes[5]).Column);
            Assert.Equal(2, positions.CalculateLinePosition(lexemes[5]).Length);

            // end at the last line of the file
            Assert.Equal(6, positions.CalculateLinePosition(lexemes[27]).Line);
            Assert.Equal(5, positions.CalculateLinePosition(lexemes[27]).Column);
            Assert.Equal(3, positions.CalculateLinePosition(lexemes[27]).Length);
        }
    }
}
