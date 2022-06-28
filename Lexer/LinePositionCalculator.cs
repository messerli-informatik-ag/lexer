using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Messerli.Lexer;

public class LinePositionCalculator : ILinePositionCalculator
{
    private readonly ImmutableList<Position> _newLines;

    public LinePositionCalculator(IEnumerable<Lexeme> lexemes) =>
        _newLines = lexemes
            .Where(l => l.IsLineBreak)
            .Select(l => l.Position)
            .ToImmutableList();

    public delegate ILinePositionCalculator Factory(List<Lexeme> lexemes);

    public static LinePositionCalculator Create(List<Lexeme> lexemes)
        => new(lexemes);

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

    private static LinePosition CalculateRelativePosition(int lineNumber, int absolutePosition, int length, Position newLinePosition)
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