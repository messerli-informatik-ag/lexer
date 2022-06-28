namespace Messerli.Lexer;

public interface ILinePositionCalculator
{
    LinePosition CalculateLinePosition(Lexem lexem);

    LinePosition CalculateLinePosition(int absolutePosition);
}