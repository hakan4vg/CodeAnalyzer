using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using Xunit;
using static System.Net.Mime.MediaTypeNames;

namespace CodeAnalyzer.Analyzers.Naming
{
    public class PascalCaseTest
    {
        private static readonly MetadataReference[] References = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        };

        [Fact]
        public void CheckPascalCase0()
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

            var analyzer = new PascalCaseAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(2, diagnostics.Length);

            // for (int i = 0; i < diagnostics.Length; i++)
            // {
            //     var diagnostic = diagnostics[i];
            //     var span = diagnostic.Location.SourceSpan;
            //     var actualMethodName = source.Substring(span.Start, span.Length);
            //     string expectedMethodName = char.ToUpper(actualMethodName[0]) + actualMethodName.Substring(1);
            // 
            //     Assert.Equal(expectedMethodName, actualMethodName);
            // }
        }

        [Fact]
        public void CheckPascalCase1()
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

            var analyzer = new PascalCaseAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(0, diagnostics.Length);
        }
    }
}
