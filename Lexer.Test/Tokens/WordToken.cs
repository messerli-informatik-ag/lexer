using Messerli.Lexer.Tokens;

namespace Messerli.Lexer.Test.Tokens
{
    internal class WordToken : IToken
    {
        public WordToken(string word)
        {
            Word = word;
        }

        public string Word { get; }

        public override string ToString()
        {
            return Word;
        }
    }
}
