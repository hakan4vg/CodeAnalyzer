using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Resources;
using CodeAnalyzerSpace;

namespace CodeAnalyzer.Analyzers.CodingConventions
{
    class PrivateMethodAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = DiagnosticRules.PrivateMethodRule;

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzePrivateMethods, SyntaxKind.MethodDeclaration);
        }

        private static void AnalyzePrivateMethods(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is MethodDeclarationSyntax methodDeclaration)
            {
                if (!methodDeclaration.Modifiers.Any(SyntaxKind.PrivateKeyword))
                {
                    var diagnostic = Diagnostic.Create(Rule
                                                      , methodDeclaration.Identifier.GetLocation()
                                                      , methodDeclaration.Identifier.Text);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
