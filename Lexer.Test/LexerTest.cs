﻿using System.Linq;
using Lexer.Test.LexerRules;
using Lexer.Test.Tokens;
using Messerli.Lexer;
using Messerli.Lexer.Exceptions;
using Xunit;

namespace Lexer.Test
{
    /// <summary>
    /// Test to verify the functionality of the lexer.
    /// </summary>
    public class LexerTest
    {
        [Fact]
        public void GivenSymbolsWithOverlappingPrefixesTheLexerGetsTheLongerOne()
        {
            var tokenizer = CreateTestTokenizer();

            Assert.IsType<EqualToken>(tokenizer.Scan("=").Single().Token);
            Assert.IsType<DoubleEqualToken>(tokenizer.Scan("==").Single().Token);
            Assert.IsType<GreaterToken>(tokenizer.Scan("<").Single().Token);
            Assert.IsType<GreaterEqualToken>(tokenizer.Scan("<=").Single().Token);
            Assert.IsType<GreaterEqualToken>(tokenizer.Scan("<===").First().Token);
            Assert.IsType<DoubleEqualToken>(tokenizer.Scan("<===").Last().Token);
        }

        [Fact]
        public void GivenALexerRuleForIdentifiersDoNotReturKeyTokenInTheMiddle()
        {
            var tokenizer = CreateTestTokenizer();

            Assert.IsType<IdentifierToken>(tokenizer.Scan("sand").Single().Token);
            Assert.IsType<IdentifierToken>(tokenizer.Scan("andor").Single().Token);
            Assert.IsType<AndToken>(tokenizer.Scan("and").Single().Token);
        }

        [Fact]
        public void GivenLexerRulesTheLexemsHaveTheRightPositions()
        {
            var tokenizer = CreateTestTokenizer();

            var lexems = tokenizer.Scan("love and sand and testing").ToList();
            Assert.Equal(9, lexems.Count);

            Assert.Equal(0, lexems[0].Position.StartPosition);
            Assert.Equal(4, lexems[0].Position.Length);
            Assert.Equal(4, lexems[0].Position.EndPosition);

            Assert.IsType<SpaceToken>(lexems[3].Token);

            Assert.Equal(8, lexems[3].Position.StartPosition);
            Assert.Equal(1, lexems[3].Position.Length);
            Assert.Equal(9, lexems[3].Position.EndPosition);

            Assert.IsType<AndToken>(lexems[6].Token);

            Assert.Equal(14, lexems[6].Position.StartPosition);
            Assert.Equal(3, lexems[6].Position.Length);
            Assert.Equal(17, lexems[6].Position.EndPosition);
        }

        [Fact]
        public void GivenALexerMissingAProductionForAGivenStringItShouldThrowAnException()
        {
            var tokenizer = new Tokenizer(new EmptyRules(), s => new LexerReader(s), lexems => new LinePositionCalculator(lexems));

            Assert.Throws<UnknownTokenException>(() => tokenizer.Scan("You can't tokenize this!"));
        }

        [Fact]
        public void GivenAStringAndAMissingTokenizerThrows()
        {
            var x = new ContextedRules();
            var tokenizer = new Tokenizer(x, s => new LexerReader(s), lexems => new LinePositionCalculator(lexems));

            var exception = Assert.Throws<UnknownTokenException>(() => tokenizer.Scan("aa aa cc aa UU cc aa"));

            Assert.Equal("Unknown Token 'U' at Line 1 Column 13", exception.Message);
        }

        [Fact]
        public void GivenALexerAndAContextedLexerRuleGenerateTokenContexted()
        {
            var x = new ContextedRules();
            var tokenizer = new Tokenizer(x, s => new LexerReader(s), lexems => new LinePositionCalculator(lexems));

            var lexems = tokenizer.Scan("aa aa cc aa bb cc aa").ToList();

            Assert.Equal(13, lexems.Count);

            Assert.IsType<AaToken>(lexems[0].Token);
            Assert.IsType<SpaceToken>(lexems[1].Token);
            Assert.IsType<AaToken>(lexems[2].Token);
            Assert.IsType<SpaceToken>(lexems[3].Token);

            // The first cc produces a CcToken
            Assert.IsType<CcToken>(lexems[4].Token);
            Assert.IsType<SpaceToken>(lexems[5].Token);
            Assert.IsType<AaToken>(lexems[6].Token);
            Assert.IsType<SpaceToken>(lexems[7].Token);
            Assert.IsType<BbToken>(lexems[8].Token);
            Assert.IsType<SpaceToken>(lexems[9].Token);

            // The second cc produces a CcAfterBbToken because there is a BbToken already produced
            Assert.IsType<CcAfterBbToken>(lexems[10].Token);
            Assert.IsType<SpaceToken>(lexems[11].Token);
            Assert.IsType<AaToken>(lexems[12].Token);
        }

        private Tokenizer CreateTestTokenizer()
        {
            return new Tokenizer(new ExampleRules(), s => new LexerReader(s), lexems => new LinePositionCalculator(lexems));
        }
    }
}
