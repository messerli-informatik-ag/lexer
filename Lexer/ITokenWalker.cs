using System;
using System.Collections.Generic;

namespace Messerli.Lexer;

public interface ITokenWalker
{
    ITokenWalker Scan(string expression);

    ITokenWalker Scan(string expression, Func<IEnumerable<Lexeme>, IEnumerable<Lexeme>> postProcessTokens);

    Lexeme Pop();

    Lexeme Peek(int lookAhead = 0);

    LinePosition CalculateLinePosition(Lexeme lexeme);
}