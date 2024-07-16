using CodeAnalyzerSpace;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Resources;

namespace CodeAnalyzer.Analyzers.Naming
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CamelCaseAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = DiagnosticRules.CamelCaseRule;

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeCamelCase, SyntaxKind.VariableDeclaration);
            //context.RegisterSyntaxNodeAction(AnalyzeCamelCase, SyntaxKind.FieldDeclaration);
        }

        public static void AnalyzeCamelCase(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is VariableDeclarationSyntax variableDeclaration)
            {
                foreach (var variable in variableDeclaration.Variables)
                {
                    var variableName = variable.Identifier.Text;

                    if (CommonUtilities.IsCamelCase(variableName) == false)
                    {
                        var diagnostic = Diagnostic.Create(Rule, variable.Identifier.GetLocation(), variable.Identifier.Text, variableName);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
            else if (context.Node is FieldDeclarationSyntax fieldDeclaration)
            {
                foreach (var field in fieldDeclaration.Declaration.Variables)
                {
                    var fieldName = field.Identifier.Text;

                    if (CommonUtilities.IsCamelCase(fieldName) == false)
                    {
                        var diagnostic = Diagnostic.Create(Rule, field.Identifier.GetLocation(), field.Identifier.Text, fieldName);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }
    }
}
