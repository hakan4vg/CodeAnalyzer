using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;


public static class CommonUtilities
{
    public static bool IsPascalCase(string identifier)
    {
        if (string.IsNullOrEmpty(identifier))
            return false;

        string pattern = @"^[A-Z][a-z0-9]*([A-Z][a-z0-9]*)*$";
        return Regex.IsMatch(identifier, pattern);
    }

    public static bool IsCamelCase(string identifier){
        if (string.IsNullOrEmpty(identifier))
            return false;

        string pattern = @"^[a-z]+(?:[A-Z][a-z0-9]*)*$";
        return Regex.IsMatch(identifier, pattern);
    }

    public static bool ContainsOnlyWhitespace(SyntaxNode Node){
        return Node.DescendantTrivia().All(trivia => trivia.IsKind(SyntaxKind.WhitespaceTrivia));
    }

    public static bool IsValidIndentation(SyntaxNode Node, int spacesPerIndentation = 4)
    {
        var leadingTrivia = Node.GetLeadingTrivia();
        var spaceCount = leadingTrivia.Count(t => t.IsKind(SyntaxKind.WhitespaceTrivia));
        return spaceCount % spacesPerIndentation == 0;
    }

    public static bool IsIdentifierUsedInCode(SyntaxNode root, string identifier)
    {
        return root.DescendantNodes().OfType<IdentifierNameSyntax>()
                   .Any(id => id.Identifier.Text == identifier);
    }
}
