using Funcky.Monads;

namespace Messerli.Lexer;

public interface ILexerReader
{
    int Position { get; }

    Option<char> Peek(int lookAhead = 0);

    Option<char> Read();
}