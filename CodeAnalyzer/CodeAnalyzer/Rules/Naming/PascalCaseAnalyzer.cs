using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Resources;

using CodeAnalyzerSpace;    


namespace CodeAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PascalCaseAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = CodeAnalyzerSpace.DiagnosticRules.PascalCaseRule;

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzePascalCase, SyntaxKind.ClassDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzePascalCase, SyntaxKind.MethodDeclaration);
        }

        private static void AnalyzePascalCase(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is ClassDeclarationSyntax classDeclaration)
            {
                var className = classDeclaration.Identifier.Text;

                if (CommonUtilities.IsPascalCase(className) == false)
                {
                    var diagnostic = Diagnostic.Create(Rule,
                                                       classDeclaration.Identifier.GetLocation(),
                                                       classDeclaration.Identifier.Text, className);

                    context.ReportDiagnostic(diagnostic);
                }
            }
            else if (context.Node is MethodDeclarationSyntax methodDeclaration)
            {
                var methodName = methodDeclaration.Identifier.Text;

                if (CommonUtilities.IsPascalCase(methodName) == false)
                {
                    var diagnostic = Diagnostic.Create(Rule,
                                                       methodDeclaration.Identifier.GetLocation(),
                                                       methodDeclaration.Identifier.Text, methodName);

                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
