using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using Xunit;

namespace CodeAnalyser.NestedIfCounter.Tests
{
    public class NestedIfCounterAnalyzerTests
    {
        private static readonly MetadataReference[] References = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        };

        [Fact]
        public void AnalyzeNestedIfStatements0()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class TestClass
                    {
                        public void TestMethod()
                        {
                            
                        }
                    }
                }";

            VerifyCSharpDiagnostic(source, 0);
        }

        [Fact]
        public void AnalyzeNestedIfStatements1()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class TestClass
                    {
                        public void TestMethod()
                        {
                            if (true)
                            {
                                
                            }
                        }
                    }
                }";

            VerifyCSharpDiagnostic(source, 1);
        }

        [Fact]
        public void AnalyzeNestedIfStatements2()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class TestClass
                    {
                        public void TestMethod()
                        {
                            if (true)
                            {
                                if (true){}
                            }
                        }
                    }
                }";

            VerifyCSharpDiagnostic(source, 2);
        }

        [Fact]
        public void AnalyzeNestedIfStatements3()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class TestClass
                    {
                        public void TestMethod()
                        {
                            if (true)
                            {
                                if (true){ if (true){} }
                            }
                        }
                    }
                }";

            VerifyCSharpDiagnostic(source, 3);
        }

        [Fact]
        public void AnalyzeNestedIfStatements4()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class TestClass
                    {
                        public void TestMethod()
                        {
                            if (true)
                            {
                                if (true){ if (true){} }

                                if (true){ if (true){ if (true){} } }
                            }
                        }
                    }
                }";

            VerifyCSharpDiagnostic(source, 4);
        }

        [Fact]
        public void AnalyzeNestedIfStatements5()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class TestClass
                    {
                        public void TestMethod()
                        {
                            if (true)
                            {
                                if (true){ if (true){} }
                                if (true){ if (true){} }
                                if (true){ if (true){ if (true){} } }

                                if (true){ if (true){ if (true){ if (true){} } } }
                            }
                        }
                    }
                }";

            VerifyCSharpDiagnostic(source, 5);
        }

        private void VerifyCSharpDiagnostic(string source, int expectedCount)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(SourceText.From(source), CSharpParseOptions.Default);
            var compilation = CSharpCompilation.Create("TestCompilation",
                syntaxTrees: new[] { syntaxTree },
                references: References);

            var analyzers = new DiagnosticAnalyzer[] { new NestedIfCounterAnalyzer() };
            var compilationWithAnalyzers = compilation.WithAnalyzers(ImmutableArray.Create(analyzers));
            var diagnostics = compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync().GetAwaiter().GetResult();

            Assert.Equal(expectedCount, diagnostics.Length);
        }
    }
}