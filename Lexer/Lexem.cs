using Messerli.Lexer.Tokens;

namespace Messerli.Lexer
{
    /// <summary>
    /// A lexem represents a string token and it's associated position.
    /// </summary>
    public record Lexem(IToken Token, Position Position, bool IsLineBreak = false);
}
