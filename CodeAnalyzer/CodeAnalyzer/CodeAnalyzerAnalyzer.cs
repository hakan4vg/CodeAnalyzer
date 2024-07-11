﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Linq;
using System.Threading;

namespace CodeAnalyzer
{

    public static class DiagnosticRules
    {


        /*public abstract class DescriptorBase
        {
            public abstract string Id { get; }
            public abstract string Title { get; }
            public abstract string MessageFormat { get; }
            public abstract string Category { get; }
            public abstract DiagnosticSeverity DefaultSeverity { get; }
            public abstract bool IsEnabledByDefault { get; }

            public DiagnosticDescriptor Rule => new DiagnosticDescriptor(
                id: Id,
                title: Title,
                messageFormat: MessageFormat,
                category: Category,
                defaultSeverity: DefaultSeverity,
                isEnabledByDefault: IsEnabledByDefault
            );
        }*/
        public static DiagnosticDescriptor PascalCaseRule = new DiagnosticDescriptor(
            id: "CA001",
            title: "PascalCase Naming Rule",
            messageFormat: "The name '{0}' should be in PascalCase",
            category: "Naming",
            defaultSeverity: DiagnosticSeverity.Info,
            isEnabledByDefault: true
        );

        public static DiagnosticDescriptor CamelCaseRule = new DiagnosticDescriptor(
            id: "CA002",
            title: "CamelCase Naming Rule",
            messageFormat: "The name '{0}' should be in camelCase",
            category: "Naming",
            defaultSeverity: DiagnosticSeverity.Info,
            isEnabledByDefault: true
        );
        
        public static DiagnosticDescriptor ConstantsRule = new DiagnosticDescriptor(
            id: "CA003",
            title: "ConstantsRule",
            messageFormat: "The constant '{0}' is not properly named",
            category: "Naming",
            defaultSeverity: DiagnosticSeverity.Info,
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
            defaultSeverity: DiagnosticSeverity.Info,
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
            messageFormat: "Method '{0}' should not exceed ~70 lines",
            category: "Code Conventions",
            defaultSeverity: DiagnosticSeverity.Info,
            isEnabledByDefault: true
        );

        public static DiagnosticDescriptor UnnecessaryCurlyBracesRule = new DiagnosticDescriptor(
            id:"CA010",
            title:"Unnecessary Curly Braces Rule",
            messageFormat:"You've entered unnecessary curly braces",
            category:"Class and Method Structure",
            defaultSeverity:DiagnosticSeverity.Error,
            isEnabledByDefault:true
        );

        public static DiagnosticDescriptor PublicClassRule = new DiagnosticDescriptor(
            id: "CA011",
            title: "Public Class Access Modifier Rule",
            messageFormat: "Class '{0}' should be public",
            category: "Identifiers",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public static DiagnosticDescriptor PrivateMethodRule = new DiagnosticDescriptor(
            id: "CA012",
            title: "Private Method Modifier Rule",
            messageFormat: "Method '{0}' should be private",
            category: "Identifiers",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public static DiagnosticDescriptor PublicPropertyRule = new DiagnosticDescriptor(
            id: "CA013",
            title: "Public Property Modifier Rule",
            messageFormat: "Property '{0}' should be public",
            category: "Identifiers",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public static DiagnosticDescriptor CodeBlockScopingRule = new DiagnosticDescriptor(
            id: "CA014",
            title: "Code Block Framing Rule",
            messageFormat: "This block should've been framed in curly braces",
            category: "Code Scoping",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public static DiagnosticDescriptor NullReferenceRule = new DiagnosticDescriptor(
            id: "CA015",
            title: "Null Reference Rule",
            messageFormat: "You are trying to use something that is null",
            category: "Code Scoping",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public static DiagnosticDescriptor GetterRule = new DiagnosticDescriptor(
            id: "CA016",
            title: "Getter Rule",
            messageFormat: "You're missing getter method",
            category: "Encapsulation",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true
        );
       
        public static DiagnosticDescriptor SetterRule = new DiagnosticDescriptor(
            id: "CA017",
            title: "Setter Rule",
            messageFormat: "You're missing setter method",
            category: "Encapsulation",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true
        );

        public static DiagnosticDescriptor GotoBanRule= new DiagnosticDescriptor(
            id: "CA018",
            title: "Goto Ban Rule",
            messageFormat: "Goto usage is considered a bad practice",
            category: "Code Conventions",
            defaultSeverity: DiagnosticSeverity.Info,
            isEnabledByDefault: true
        );

        public static DiagnosticDescriptor VarBanRule = new DiagnosticDescriptor(
            id: "CA019",
            title: "Var Ban Rule",
            messageFormat: "Using var instead of type is considered a bad practice",
            category: "Code Conventions",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true
        );
    }

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CodeAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
            DiagnosticRules.PascalCaseRule,
            DiagnosticRules.CamelCaseRule,
            DiagnosticRules.IndentationRule,
            DiagnosticRules.WhitespaceRule,
            DiagnosticRules.ConstantsRule
           
        );

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
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

        




        private static void AnalyzePascalCase(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is ClassDeclarationSyntax classDeclaration)
            {
                var className = classDeclaration.Identifier.Text;
                if (CommonUtilities.IsPascalCase(className) == false)
                {
                    var diagnostic = Diagnostic.Create(DiagnosticRules.PascalCaseRule, classDeclaration.Identifier.GetLocation(), className);
                }
            }
            else if (context.Node is MethodDeclarationSyntax methodDeclaration)
            {
                var methodName = methodDeclaration.Identifier.Text;
                if (CommonUtilities.IsPascalCase(methodName) == false)
                {
                    var diagnostic = Diagnostic.Create(DiagnosticRules.PascalCaseRule, methodDeclaration.Identifier.GetLocation(), methodName);
                }
            }
        }

        private static void AnalyzeCamelCase(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is VariableDeclarationSyntax variableDeclaration)
            {
                foreach (var variable in variableDeclaration.Variables)
                {
                    var variableName = variable.Identifier.Text;

          
                    if (!CommonUtilities.IsCamelCase(variableName))
                    {
                        var diagnostic = Diagnostic.Create(DiagnosticRules.CamelCaseRule, variable.Identifier.GetLocation(), variableName);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }

            else if (context.Node is FieldDeclarationSyntax fieldDeclaration)
            {
                foreach(var variable in fieldDeclaration.Declaration.Variables)
                {
                    var variableName = variable.Identifier.Text;

                    if (!CommonUtilities.IsCamelCase(variableName))
                    {
                        var diagnostic = Diagnostic.Create(DiagnosticRules.CamelCaseRule, variable.Identifier.GetLocation(), variableName);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }


        private static void AnalyzeConstantNaming(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is FieldDeclarationSyntax fieldDeclaration)
            {
                if (fieldDeclaration.Modifiers.Any(SyntaxKind.ConstKeyword))
                {
                    foreach (var variable in fieldDeclaration.Declaration.Variables)
                    {
                        var constantName = variable.Identifier.Text;
                        if (!constantName.All(char.IsUpper) || !constantName.Contains('_')){
                            var diagnostic = Diagnostic.Create(DiagnosticRules.ConstantsRule, variable.Identifier.GetLocation(), constantName);
                            context.ReportDiagnostic(diagnostic);

                        }
                    }
                }
            }
        }


        private static void AnlyzeIndentations(SyntaxNodeAnalysisContext context)
        {
            var root = context.Node.SyntaxTree.GetRoot(context.CancellationToken);
            var lines = root.ToFullString().Split('\n');
            int linenumber = 1;
            foreach (var line in lines)
            {
                if (line.StartsWith("\t") || line.Length - line.TrimStart().Length % 4 != 0)
                {
                    var diagnostic = Diagnostic.Create(DiagnosticRules.IndentationRule, Location.Create(context.Node.SyntaxTree, new TextSpan(root.GetLocation().SourceSpan.Start, 0)),
                                                       $"Line {linenumber}: Use four spaces.");
                    context.ReportDiagnostic(diagnostic);

                }
                linenumber++;

            }

        }

        private static void AnalyzeWhiteSpace(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is BinaryExpressionSyntax binaryExpression)
            {
                var leftToken = binaryExpression.Left.GetLastToken();
                var rightToken = binaryExpression.Right.GetLastToken();
                var operatorToken = binaryExpression.OperatorToken;

                if(!leftToken.TrailingTrivia.Any(SyntaxKind.WhitespaceTrivia) || !rightToken.LeadingTrivia.Any(SyntaxKind.WhitespaceTrivia)){
                    var diagnostic = Diagnostic.Create(DiagnosticRules.WhitespaceRule, operatorToken.GetLocation(), operatorToken.Text);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        // Diğer analiz metodlarını burada tanımlayın
    }

}
