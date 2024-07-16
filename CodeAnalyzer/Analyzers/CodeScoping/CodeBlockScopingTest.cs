using CodeAnalyzerSpace;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using Xunit;

namespace CodeAnalyzer.Analyzers.CodeScoping
{
    public class CodeBlockScopingTests
    {
        private static readonly MetadataReference[] References = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        };

        [Fact]
        public void TestCodeBlockScoping_NoDiagnostics()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class TestClass
                    {
                        public void TestMethod()
                        {
                            {
                                Console.WriteLine(""Inside inner block."");
                            }
                        }
                    }
                }";

            var analyzer = new CodeBlockScopingAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Single(diagnostics);
        }

        [Fact]
        public void TestCodeBlockScoping_NestedBlockDiagnostic()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class TestClass
                    {
                        public void TestMethod()
                        {
                            {
                                {
                                    Console.WriteLine(""Nested inner blocks."");
                                }
                            }
                        }
                    }
                }";

            var analyzer = new CodeBlockScopingAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(2, diagnostics.Length);
        }
    }
}
