using CodeAnalyzer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CodeAnalyzer.Analyzers.Naming
{
    public class CamelCaseTest
    {
        private static readonly MetadataReference[] References = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        };

        [Fact]
        public void CheckCamelCase0()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class TestClass
                    {
                        int Sayi = 0;

                        public void TestMethod()
                        {
                            int Intejer = 0;
                        }
                    }
                }";

            var analyzer = new CamelCaseAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(2, diagnostics.Length);

            // for (int i = 0; i < diagnostics.Length; i++)
            // {
            //     var diagnostic = diagnostics[i];
            //     var span = diagnostic.Location.SourceSpan;
            //     var actualMethodName = source.Substring(span.Start, span.Length);
            //     string expectedMethodName = char.ToLower(actualMethodName[0]) + actualMethodName.Substring(1);
            // 
            //     Assert.Equal(expectedMethodName, actualMethodName);
            // }
        }

        [Fact]
        public void CheckCamelCase1()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class testClass
                    {
                        public void testMethod()
                        {

                        }
                    }
                }";

            var analyzer = new CamelCaseAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(0, diagnostics.Length);
        }
    }
}
