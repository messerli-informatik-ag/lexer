using System.Collections.Generic;

namespace Messerli.Lexer;

public interface ITokenizer
{
    List<Lexeme> Scan(string expression);
}