using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Resources;
using CodeAnalyzerSpace;

namespace CodeAnalyzer.Analyzers.CodingConventions
{
    class MethodLengthAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = DiagnosticRules.MethodLengthRule;

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeMethodLength, SyntaxKind.MethodDeclaration);
        }

        public static void AnalyzeMethodLength(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is MethodDeclarationSyntax methodDeclaration)
            {
                var startLine = methodDeclaration.GetLocation().GetLineSpan().StartLinePosition.Line;
                var endLine = methodDeclaration.GetLocation().GetLineSpan().EndLinePosition.Line;

                if (endLine - startLine > 30)
                {
                    var diagnostic = Diagnostic.Create(Rule,
                                                       methodDeclaration.Identifier.GetLocation(),
                                                       methodDeclaration.Identifier.Text);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
