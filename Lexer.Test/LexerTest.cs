using System.Linq;
using Messerli.Lexer.Exceptions;
using Messerli.Lexer.Test.LexerRules;
using Messerli.Lexer.Test.Tokens;
using Xunit;

namespace Messerli.Lexer.Test;

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
    public void GivenLexerRulesTheLexemesHaveTheRightPositions()
    {
        var tokenizer = CreateTestTokenizer();

        var lexemes = tokenizer.Scan("love and sand and testing").ToList();
        Assert.Equal(9, lexemes.Count);

        Assert.Equal(0, lexemes[0].Position.StartPosition);
        Assert.Equal(4, lexemes[0].Position.Length);
        Assert.Equal(4, lexemes[0].Position.EndPosition);

        Assert.IsType<SpaceToken>(lexemes[3].Token);

        Assert.Equal(8, lexemes[3].Position.StartPosition);
        Assert.Equal(1, lexemes[3].Position.Length);
        Assert.Equal(9, lexemes[3].Position.EndPosition);

        Assert.IsType<AndToken>(lexemes[6].Token);

        Assert.Equal(14, lexemes[6].Position.StartPosition);
        Assert.Equal(3, lexemes[6].Position.Length);
        Assert.Equal(17, lexemes[6].Position.EndPosition);
    }

    [Fact]
    public void GivenALexerMissingAProductionForAGivenStringItShouldThrowAnException()
    {
        var tokenizer = new Tokenizer(EmptyRules.GetRules(), LexerReader.Create, LinePositionCalculator.Create);

        Assert.Throws<UnknownTokenException>(() => tokenizer.Scan("You can't tokenize this!"));
    }

    [Fact]
    public void GivenAStringAndAMissingTokenizerThrows()
    {
        var tokenizer = new Tokenizer(RulesWithContext.GetRules(), LexerReader.Create, LinePositionCalculator.Create);

        var exception = Assert.Throws<UnknownTokenException>(() => tokenizer.Scan("aa aa cc aa UU cc aa"));

        Assert.Equal("Unknown Token 'U' at Line 1 Column 13", exception.Message);
    }

    [Fact]
    public void GivenALexerAndAContextedLexerRuleGenerateTokenContexted()
    {
        var tokenizer = new Tokenizer(RulesWithContext.GetRules(), LexerReader.Create, LinePositionCalculator.Create);

        var lexemes = tokenizer.Scan("aa aa cc aa bb cc aa").ToList();

        Assert.Equal(13, lexemes.Count);

        Assert.IsType<AaToken>(lexemes[0].Token);
        Assert.IsType<SpaceToken>(lexemes[1].Token);
        Assert.IsType<AaToken>(lexemes[2].Token);
        Assert.IsType<SpaceToken>(lexemes[3].Token);

        // The first cc produces a CcToken
        Assert.IsType<CcToken>(lexemes[4].Token);
        Assert.IsType<SpaceToken>(lexemes[5].Token);
        Assert.IsType<AaToken>(lexemes[6].Token);
        Assert.IsType<SpaceToken>(lexemes[7].Token);
        Assert.IsType<BbToken>(lexemes[8].Token);
        Assert.IsType<SpaceToken>(lexemes[9].Token);

        // The second cc produces a CcAfterBbToken because there is a BbToken already produced
        Assert.IsType<CcAfterBbToken>(lexemes[10].Token);
        Assert.IsType<SpaceToken>(lexemes[11].Token);
        Assert.IsType<AaToken>(lexemes[12].Token);
    }

    private static Tokenizer CreateTestTokenizer()
        => new(ExampleRules.GetRules(), LexerReader.Create, LinePositionCalculator.Create);
}