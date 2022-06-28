using System;
using System.Collections.Generic;

namespace Messerli.Lexer;

public interface ITokenWalker
{
    void Scan(string expression);

    void Scan(string expression, Func<IEnumerable<Lexeme>, IEnumerable<Lexeme>> postProcessTokens);

    Lexeme Pop();

    Lexeme Peek(int lookAhead = 0);

    LinePosition CalculateLinePosition(Lexeme lexeme);
}