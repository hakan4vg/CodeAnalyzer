using CodeAnalyzerSpace;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Linq;

namespace CodeAnalyzer.Analyzers.WhitespacesAndIndents
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class IndentationsAnalyzer : DiagnosticAnalyzer
    {

        private static readonly DiagnosticDescriptor Rule = DiagnosticRules.IndentationRule;
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(Rule);



        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxTreeAction(AnalyzeIndentations);
        }

        private static void AnalyzeIndentations(SyntaxTreeAnalysisContext context)
        {
            var root = context.Tree.GetRoot(context.CancellationToken);
            var lines = root.GetText().Lines;

            foreach (var line in lines)
            {
                var leadingSpaces = line.ToString().TakeWhile(c => c == ' ').Count();
                if (leadingSpaces % 4 != 0)
                {
                    var startPosition = line.Span.Start; // Get the start position of the line
                    var location = Location.Create(context.Tree, new TextSpan(startPosition, line.Span.Length));
                    var diagnostic = Diagnostic.Create(Rule,
                                                       location,
                                                       line.LineNumber + 1); // Adjust line number to 1-based index
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

    }
}