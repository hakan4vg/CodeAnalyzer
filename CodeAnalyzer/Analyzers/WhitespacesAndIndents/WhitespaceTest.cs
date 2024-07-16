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
    public class WhitespaceTest
    {
        private static readonly MetadataReference[] References = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        };

        [Fact]
        public void CheckWhitespace0()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class TestClass
                        {

                            private const int MAX_VALUE= 100; int i =0, i = i-1;
                        }
                }";

            var analyzer = new WhitespaceAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(4, diagnostics.Length);
        }

        [Fact]
        public void CheckWhitespace1()
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
                            if (i ==0)
                            {
                                i = i +31;
                            }
                        }
                    }
                }";

            var analyzer = new WhitespaceAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(2, diagnostics.Length);
        }
    }
}
