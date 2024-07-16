using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Resources;
using CodeAnalyzerSpace;

namespace CodeAnalyzer.Analyzers.CodingConventions
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class GotoBanAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = DiagnosticRules.GotoBanRule;

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeGoto, SyntaxKind.GotoStatement);
        }

        public static void AnalyzeGoto(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is GotoStatementSyntax gotoStatement)
            {
                var targetLabel = gotoStatement.Expression.ToString();

                var diagnostic = Diagnostic.Create(Rule, context.Node.GetLocation(), targetLabel);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
