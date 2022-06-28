namespace Messerli.Lexer;

public interface ILinePositionCalculator
{
    LinePosition CalculateLinePosition(Lexeme lexeme);

    LinePosition CalculateLinePosition(int absolutePosition);
}