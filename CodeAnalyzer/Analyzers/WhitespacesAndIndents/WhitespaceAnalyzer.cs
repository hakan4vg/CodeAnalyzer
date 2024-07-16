using CodeAnalyzerSpace;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Resources;


namespace CodeAnalyzer.Analyzers.WhitespacesAndIndents
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class WhitespaceAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = DiagnosticRules.WhitespaceRule;

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxTreeAction(AnalyzeSyntaxTree);
        }

        private static void ReportDiagnostic(SyntaxTreeAnalysisContext context, SyntaxToken token, SyntaxKind op)
        {
            var diagnostic = Diagnostic.Create(Rule,
                                               token.GetLocation(),
                                               token.ToString());
            context.ReportDiagnostic(diagnostic);
        }

        private static void AnalyzeSyntaxTree(SyntaxTreeAnalysisContext context)
        {
            var root = context.Tree.GetRoot();

            foreach (var binaryExpression in root.DescendantNodes().OfType<BinaryExpressionSyntax>())
            {
                // List of operators to check for spacing
                var operatorsToCheck = new[]
                {
                    SyntaxKind.PlusToken,
                    SyntaxKind.MinusToken,
                    SyntaxKind.AsteriskToken,
                    SyntaxKind.SlashToken,
                    SyntaxKind.PercentToken,
                    SyntaxKind.EqualsEqualsToken,
                    SyntaxKind.ExclamationEqualsToken,
                    SyntaxKind.LessThanToken,
                    SyntaxKind.LessThanEqualsToken,
                    SyntaxKind.GreaterThanToken,
                    SyntaxKind.GreaterThanEqualsToken,
                    SyntaxKind.AmpersandToken,
                    SyntaxKind.BarToken,
                    SyntaxKind.CaretToken,
                    SyntaxKind.LessThanLessThanToken,
                    SyntaxKind.GreaterThanGreaterThanToken
                };

                foreach (var op in operatorsToCheck)
                {
                    var token = binaryExpression.OperatorToken;

                    if (token.IsKind(op))
                    {
                        // Check for spaces around the operator
                        if (!token.HasLeadingTrivia && !token.LeadingTrivia.Any(SyntaxKind.WhitespaceTrivia))
                        {
                            ReportDiagnostic(context, token, op);
                        }

                        if (!token.HasTrailingTrivia && !token.TrailingTrivia.Any(SyntaxKind.WhitespaceTrivia))
                        {
                            ReportDiagnostic(context, token, op);
                        }
                    }
                }
            }
        }
    }
}
