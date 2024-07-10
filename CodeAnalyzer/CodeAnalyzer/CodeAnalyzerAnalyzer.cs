using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace CodeAnalyzer
{

    public static class DiagnosticRules
    {
        public static DiagnosticDescriptor PascalCaseRule = new DiagnosticDescriptor(
            id: "CA001",
            title: "PascalCase Naming Rule",
            messageFormat: "The name '{0}' should be in PascalCase",
            category: "Naming",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true
        );

        public static DiagnosticDescriptor CamelCaseRule = new DiagnosticDescriptor(
            id: "CA002",
            title: "CamelCase Naming Rule",
            messageFormat: "The name '{0}' should be in camelCase",
            category: "Naming",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true
        );

        public static DiagnosticDescriptor ConstantsRule = new DiagnosticDescriptor(
            id: "CA003",
            title: "ConstantsRule",
            messageFormat: "The constant '{0}' is not properly named",
            category: "Naming",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true
        );

        public static DiagnosticDescriptor IndentationRule = new DiagnosticDescriptor(
            id: "CA004",
            title: "Indentation Rule",
            messageFormat: "The line '{0}' is not indented properly (4 spaces)",
            category: "Whitespaces and Indents",
            defaultSeverity: DiagnosticSeverity.Info,
            isEnabledByDefault: true
        );

        public static DiagnosticDescriptor WhitespaceRule = new DiagnosticDescriptor(
            id: "CA005",
            title: "Whitespace Rule",
            messageFormat: "Operators should have whitespaces around them",
            category: "Whitespaces and Indents",
            defaultSeverity: DiagnosticSeverity.Info,
            isEnabledByDefault: true
        );

        public static DiagnosticDescriptor LineLengthRule = new DiagnosticDescriptor(
            id: "CA006",
            title: "Line Length Rule",
            messageFormat: "A line should not exceed 120 characters.",
            category: "Code Conventions",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true
        );

        public static DiagnosticDescriptor NewLineRule = new DiagnosticDescriptor(
            id: "CA007",
            title: "New Line Rule",
            messageFormat: "You should add a new line between methods",
            category: "Code Conventions",
            defaultSeverity: DiagnosticSeverity.Info,
            isEnabledByDefault: true
        );

        public static DiagnosticDescriptor ParameterCountRule = new DiagnosticDescriptor(
            id: "CA008",
            title: "Parameter Count Rule",
            messageFormat: "You shouldn't use more than five parameters per method",
            category: "Class and Method Structure",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true
        );

        public static DiagnosticDescriptor MethodLengthRule = new DiagnosticDescriptor(
            id: "CA009",
            title: "Method Length Rule",
            messageFormat: "Method {0} should not exceed ~70 lines",
            category: "Code Conventions",
            defaultSeverity: DiagnosticSeverity.Info,
            isEnabledByDefault: true
        );

        public static DiagnosticDescriptor UnnecessaryCurlyBraces = new DiagnosticDescriptor(
            id:"CA010",
            title:"Unnecessary Curly Braces Rule",
            messageFormat:"You've entered unnecessary curly braces",
            category:"Class and Method Structure",
            defaultSeverity:DiagnosticSeverity.Error,
            isEnabledByDefault:true
        );
    }

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CodeAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
            DiagnosticRules.PascalCaseRule,
            DiagnosticRules.CamelCaseRule,
            DiagnosticRules.IndentationRule
        );

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeMethodDeclaration, SyntaxKind.MethodDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeVariableDeclaration, SyntaxKind.VariableDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeIndent, SyntaxKind.WhitespaceTrivia);
            // Diğer kuralları buraya ekleyin
        }

        private static void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;
            var className = classDeclaration.Identifier.Text;
            if (!char.IsUpper(className[0]) || className.Any(char.IsLower))
            {
                var diagnostic = Diagnostic.Create(DiagnosticRules.PascalCaseRule, classDeclaration.Identifier.GetLocation(), className);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private static void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
        {
            var methodDeclaration = (MethodDeclarationSyntax)context.Node;
            var methodName = methodDeclaration.Identifier.Text;
            if (!char.IsUpper(methodName[0]) || methodName.Any(char.IsLower))
            {
                var diagnostic = Diagnostic.Create(DiagnosticRules.PascalCaseRule, methodDeclaration.Identifier.GetLocation(), methodName);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private static void AnalyzeVariableDeclaration(SyntaxNodeAnalysisContext context)
        {
            var variableDeclaration = (VariableDeclarationSyntax)context.Node;
            foreach (var variable in variableDeclaration.Variables)
            {
                var variableName = variable.Identifier.Text;
                if (!char.IsLower(variableName[0]))
                {
                    var diagnostic = Diagnostic.Create(DiagnosticRules.CamelCaseRule, variable.Identifier.GetLocation(), variableName);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        private static void AnalyzeIndent(SyntaxTreeAnalysisContext context)
        {
            var root = context.Tree.GetRoot(context.CancellationToken);
            var lines = root.ToFullString().Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (line.StartsWith("\t") || (line.Length - line.TrimStart().Length) % 4 != 0)
                {
                    var diagnostic = Diagnostic.Create(DiagnosticRules.IndentationRule, Location.Create(context.Tree, new TextSpan(root.GetLocation().SourceSpan.Start, 0)), $"Line {i + 1}: Use four spaces.");
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        // Diğer analiz metodlarını burada tanımlayın
    }

}
