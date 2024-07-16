using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Resources;
using CodeAnalyzerSpace;

namespace CodeAnalyzer.Analyzers.CodeStructure
{
    class CurlyBracesAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = DiagnosticRules.UnnecessaryCurlyBracesRule;

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeIfStatement, SyntaxKind.IfStatement);
            context.RegisterSyntaxNodeAction(AnalyzeIfStatement, SyntaxKind.ElseClause);
            context.RegisterSyntaxNodeAction(AnalyzeSingleStatement, SyntaxKind.ForStatement);
            context.RegisterSyntaxNodeAction(AnalyzeSingleStatement, SyntaxKind.WhileStatement);
            ;
        }

        private void AnalyzeIfStatement(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is IfStatementSyntax ifStatement && ifStatement.Statement is not BlockSyntax)
            {
                var diagnostic = Diagnostic.Create(Rule, ifStatement.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
            else if (context.Node is ElseClauseSyntax elseClause && elseClause.Statement is not BlockSyntax)
            {
                var diagnostic = Diagnostic.Create(Rule, elseClause.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }

        private void AnalyzeSingleStatement(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is StatementSyntax statement && !(statement is BlockSyntax))
            {
                var diagnostic = Diagnostic.Create(Rule, statement.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }

    }
}
