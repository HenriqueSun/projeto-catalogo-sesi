using SesiCatalogo.Web.Catalog;

namespace SesiCatalogo.Tests;

public sealed class TextNormalizerTests
{
    [Theory]
    [InlineData("Matemática", "matematica")]
    [InlineData("  CIÊNCIAS da Natureza ", "ciencias da natureza")]
    [InlineData("Inglês Técnico", "ingles tecnico")]
    [InlineData("ação", "acao")]
    public void Normalize_removes_accents_case_and_surrounding_spaces(string input, string expected)
    {
        Assert.Equal(expected, TextNormalizer.Normalize(input));
    }
}
