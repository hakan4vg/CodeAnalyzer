using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace CodeAnalyser.NestedIfCounter
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NestedIfCounterAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "NIF001";
        private const string Title = "Nested If Analyzer";
        private const string MessageFormat = "Number of nested if statements: {0}";
        private const string Category = "Nested If Analysis";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeIfStatement, SyntaxKind.IfStatement);
        }

        private static void AnalyzeIfStatement(SyntaxNodeAnalysisContext context)
        {
            var ifStatement = (IfStatementSyntax)context.Node;
            int nestedIfCount = CountNestedIfStatements(ifStatement.Statement);
            var diagnostic = Diagnostic.Create(Rule, ifStatement.GetLocation(), nestedIfCount);
            context.ReportDiagnostic(diagnostic);
        }

        private static int CountNestedIfStatements(SyntaxNode node)
        {
            int count = 0;
            if (node is BlockSyntax block)
            {
                foreach (var statement in block.Statements)
                {
                    if (statement is IfStatementSyntax ifStatement)
                    {
                        count++;
                        count += CountNestedIfStatements(ifStatement.Statement);
                    }
                }
            }
            else if (node is IfStatementSyntax ifStmt)
            {
                count++;
                count += CountNestedIfStatements(ifStmt.Statement);
            }
            return count;
        }
    }
}