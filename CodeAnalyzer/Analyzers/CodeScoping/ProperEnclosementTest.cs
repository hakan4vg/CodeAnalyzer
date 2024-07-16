using CodeAnalyzerSpace;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using Xunit;

namespace CodeAnalyzer.Analyzers.CodeScoping
{
    public class ProperEnclosementAnalyzerTests
    {
        private static readonly MetadataReference[] References = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        };

        [Fact]
        public void TestProperEnclosement_NoDiagnostics()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class TestClass
                    {
                        public void TestMethod()
                        {
                            int a = (1 + 2) * (3 + 4);
                        }
                    }
                }";

            var analyzer = new ProperEnclosementAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Empty(diagnostics);
        }

        [Fact]
        public void TestProperEnclosement_MismatchedParentheses()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class TestClass
                    {
                        public void TestMethod()
                        {
                            int a = (1 + 2) * (3 + 4;
                        }
                    }
                }";

            var analyzer = new ProperEnclosementAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Single(diagnostics);
        }

        [Fact]
        public void TestProperEnclosement_UnclosedParentheses()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class TestClass
                    {
                        public void TestMethod()
                        {
                            int a = (1 + 2 * (3 + 4);
                        }
                    }
                }";

            var analyzer = new ProperEnclosementAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Single(diagnostics);
        }

        [Fact]
        public void TestProperEnclosement_UnopenedParentheses()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class TestClass
                    {
                        public void TestMethod()
                        {
                            int a = (1 + 2) * (3 + 4;
                        }
                    }
                }";

            var analyzer = new ProperEnclosementAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Single(diagnostics);
        }
    }
}
