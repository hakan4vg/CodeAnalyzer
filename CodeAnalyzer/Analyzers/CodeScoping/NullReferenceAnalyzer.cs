using CodeAnalyzerSpace;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalyzer.Analyzers.CodeScoping
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NullReferenceAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = DiagnosticRules.NullReferenceRule;
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeNullReference, SyntaxKind.IfStatement);
        }


        private static void AnalyzeNullReference(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is IfStatementSyntax ifStatement)
            {
                var condition = ifStatement.Condition;

                // Check if the condition is a null comparison
                if (IsNullOrDefaultComparison(condition))
                {
                    var diagnostic = Diagnostic.Create(Rule, ifStatement.GetLocation());
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        private static bool IsNullOrDefaultComparison(ExpressionSyntax condition)
        {
            if (condition is BinaryExpressionSyntax binaryExpression &&
                binaryExpression.IsKind(SyntaxKind.EqualsExpression))
            {
                // Check if either side of the binary expression is a null literal
                if (binaryExpression.Left.IsKind(SyntaxKind.NullLiteralExpression) ||
                    binaryExpression.Right.IsKind(SyntaxKind.NullLiteralExpression))
                {
                    return true;
                }
            }
            return false;
        }


    }
}
