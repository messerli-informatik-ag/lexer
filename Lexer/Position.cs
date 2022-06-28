namespace Messerli.Lexer;

/// <summary>
/// Represents the position of a Lexeme.
/// </summary>
public readonly struct Position
{
    public Position(int start, int length)
    {
        StartPosition = start;
        EndPosition = start + length;
        Length = length;
    }

    /// <summary>
    /// Represents the position of the first character of the lexeme, countent in number of characters.
    /// </summary>
    public int StartPosition { get; }

    /// <summary>
    /// Represents the position of the first character after the lexeme, countent in number of characters.
    /// </summary>
    public int EndPosition { get; }

    /// <summary>
    /// Represents the length of the lexeme.
    /// </summary>
    public int Length { get; }
}