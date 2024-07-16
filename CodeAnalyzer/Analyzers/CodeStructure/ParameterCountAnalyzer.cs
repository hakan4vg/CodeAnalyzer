using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Resources;
using CodeAnalyzerSpace;

namespace CodeAnalyzer.Analyzers.CodeStructure
{
    class ParameterCountAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = DiagnosticRules.ParameterCountRule;

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeParameterCount, SyntaxKind.MethodDeclaration);
        }

        public static void AnalyzeParameterCount(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is MethodDeclarationSyntax methodDeclaration)
            {
                if (methodDeclaration.ParameterList.Parameters.Count > 5)
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
