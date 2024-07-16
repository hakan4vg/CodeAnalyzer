using CodeAnalyzer.Analyzers.CodingConventions;
using CodeAnalyzer.Analyzers.Naming;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalyzer.Analyzers.Identifiers
{
    public class PrivateMethodTest
    {
        private static readonly MetadataReference[] References = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        };

        [Fact]
        public void CheckPrivateMethod0()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class TestClass
                    {
                        private void TestMethod()
                        {

                        }
                    }
                }";

            var analyzer = new PrivateMethodAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(0, diagnostics.Length);
        }
        [Fact]
        public void CheckPrivateMethod1()
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

            var analyzer = new PrivateMethodAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(1, diagnostics.Length);
        }
    }
}
