using CodeAnalyzerSpace;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalyzer.Analyzers.Encapsulation
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class GetterSetterAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = DiagnosticRules.GetterSetterRule;
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeGetterSetter, SyntaxKind.PropertyDeclaration);
        }


        private static void AnalyzeGetterSetter(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is PropertyDeclarationSyntax propertyDeclaration)
            {
                if (propertyDeclaration.AccessorList != null)
                {
                    bool hasGetAccessor = false;
                    bool hasSetAccessor = false;
                    bool hasInvalidGetAccessor = false;
                    bool hasInvalidSetAccessor = false;

                    foreach (var accessor in propertyDeclaration.AccessorList.Accessors)
                    {
                        if (accessor.Kind() == SyntaxKind.GetAccessorDeclaration)
                        {
                            hasGetAccessor = true;
                            if (accessor.Body == null && accessor.ExpressionBody == null && accessor.SemicolonToken.IsKind(SyntaxKind.None))
                            {
                                hasInvalidGetAccessor = true;
                            }
                        }
                        if (accessor.Kind() == SyntaxKind.SetAccessorDeclaration)
                        {
                            hasSetAccessor = true;
                            if (accessor.Body == null && accessor.ExpressionBody == null && accessor.SemicolonToken.IsKind(SyntaxKind.None))
                            {
                                hasInvalidSetAccessor = true;
                            }
                        }
                    }

                    if (hasGetAccessor && hasInvalidGetAccessor || hasSetAccessor && hasInvalidSetAccessor || !hasGetAccessor || !hasSetAccessor)
                    {
                        var diagnostic = Diagnostic.Create(Rule, propertyDeclaration.GetLocation());
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }


    }
}
