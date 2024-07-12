using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalyser.NestedIfCounter
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NestedIfCounterAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "NestedIfCounter";
        private const string Title = "Nested if Count";
        private const string MessageFormat = "Number of nested if statements inside this block: {0}";
        private const string Category = "Usage";

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

            if (ifStatement.Statement is BlockSyntax block)
            {
                int nestedIfCount = CountNestedIfStatements(block);
                if (nestedIfCount > 0)
                {
                    var diagnostic = Diagnostic.Create(Rule, ifStatement.GetLocation(), nestedIfCount.ToString());
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        private static int CountNestedIfStatements(BlockSyntax block)
        {
            int count = 0;

            foreach (var statement in block.Statements)
            {
                if (statement is IfStatementSyntax nestedIf)
                {
                    ++count;

                    if (nestedIf.Statement is BlockSyntax nestedBlock)
                    {
                        count += CountNestedIfStatements(nestedBlock);
                    }
                }
            }

            return count;
        }
    }
}