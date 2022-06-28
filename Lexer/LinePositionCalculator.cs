using System.Collections.Generic;
using System.Linq;

namespace Messerli.Lexer
{
    public class LinePositionCalculator : ILinePositionCalculator
    {
        private readonly List<Position> _newLines;

        public LinePositionCalculator(List<Lexeme> lexemes) =>
            _newLines = lexemes
                .Where(l => l.IsLineBreak)
                .Select(l => l.Position)
                .ToList();

        public delegate ILinePositionCalculator Factory(List<Lexeme> lexemes);

        public LinePosition CalculateLinePosition(Lexeme lexeme)
            => CalculateRelativePosition(
                LineNumber(lexeme.Position.StartPosition),
                lexeme.Position.StartPosition,
                lexeme.Position.Length,
                FindClosestNewLineBefore(lexeme.Position.StartPosition));

        public LinePosition CalculateLinePosition(int absolutePosition)
            => CalculateRelativePosition(
                LineNumber(absolutePosition),
                absolutePosition,
                1,
                FindClosestNewLineBefore(absolutePosition));

        private int LineNumber(int absolutePosition)
            => _newLines
                .Count(l => l.StartPosition < absolutePosition);

        private LinePosition CalculateRelativePosition(int lineNumber, int absolutePosition, int length, Position newLinePosition)
            => new(
                ToHumanIndex(lineNumber),
                ToHumanIndex(absolutePosition - newLinePosition.EndPosition),
                length);

        private static int ToHumanIndex(int index)
            => index + 1;

        private Position FindClosestNewLineBefore(int position)
            => _newLines
                .LastOrDefault(l => l.StartPosition < position);
    }
}
