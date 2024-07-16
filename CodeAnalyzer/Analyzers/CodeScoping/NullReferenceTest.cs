using CodeAnalyzerSpace;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using Xunit;

namespace CodeAnalyzer.Analyzers.CodeScoping
{
    public class NullReferenceAnalyzerTests
    {
        private static readonly MetadataReference[] References = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        };

        [Fact]
        public void TestNullReferenceAnalyzer_NoDiagnostics()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class TestClass
                    {
                        public void TestMethod()
                        {
                            int a = 10;
                            if (a == 10)
                            {
                                Console.WriteLine(""a is 10"");
                            }
                        }
                    }
                }";

            var analyzer = new NullReferenceAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Empty(diagnostics);
        }

        [Fact]
        public void TestNullReferenceAnalyzer_DiagnosticOnNullComparison()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class TestClass
                    {
                        public void TestMethod()
                        {
                            object obj = null;
                            if (obj == null)
                            {
                                Console.WriteLine(""obj is null"");
                            }
                        }
                    }
                }";

            var analyzer = new NullReferenceAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Single(diagnostics);
        }
    }
}
