using Messerli.Lexer.Tokens;

namespace Messerli.Lexer;

/// <summary>
/// A lexeme represents a string token and it's associated position.
/// </summary>
public sealed record Lexeme(IToken Token, Position Position, bool IsLineBreak = false);