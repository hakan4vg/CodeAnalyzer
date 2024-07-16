
using CodeAnalyzerSpace;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using Xunit;
using static System.Net.Mime.MediaTypeNames;

namespace CodeAnalyzer.Analyzers.WhitespacesAndIndents
{
    public class IndentationsTest
    {
        private static readonly MetadataReference[] References = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        };

        [Fact]
        public void CheckIntendationRule0()
        {
            string source = @"using System;
                namespace TestNamespace
                    {
                  public class TestClass
                    {
                        private const int MAX_VALUE = 100;
                    }
                }";

            var analyzer = new IndentationsAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Single(diagnostics);
            var diagnostic = diagnostics.Single();
            var expectedMessage = string.Format("Line {0}: Use four spaces", 4); // Adjust the line number as needed
            Assert.Equal(expectedMessage, diagnostic.GetMessage());
        }

        [Fact]
        public void CheckIndentationRule1()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class TestClass
                    {
                        public void TestMethod()
                        {
                            int i = 0;
                            if (i == 0)
                            {
                                private const int maxValue = 100;
                            }
                        }
                    }
                }";

            var analyzer = new IndentationsAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            //Assert.Equal(1, diagnostics.Length);
        }
    }
}
