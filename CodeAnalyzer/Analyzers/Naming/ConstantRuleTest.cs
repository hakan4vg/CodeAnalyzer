using CodeAnalyzerSpace;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using Xunit;
using static System.Net.Mime.MediaTypeNames;

namespace CodeAnalyzer.Analyzers.Naming
{
    public class ConstantRuleTest
    {
        private static readonly MetadataReference[] References = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        };

        [Fact]
        public void CheckConstantRule0()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class TestClass
                        {
                            private const int MAX_VALUE = 100;
                        }
                }";

            var analyzer = new ConstantRuleAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(1, diagnostics.Length);
        }

        [Fact]
        public void CheckConstantRule1()
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

            var analyzer = new ConstantRuleAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(1, diagnostics.Length);
        }
    }
}
