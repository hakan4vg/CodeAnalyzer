using CodeAnalyzer.Analyzers.CodingConventions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalyzer.Analyzers.CodeStructure
{
    public class ParameterCountTest
    {
        private static readonly MetadataReference[] References = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        };

        [Fact]
        public void CheckParameterCount0()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class testClass
                    {
                        public void testMethod(int x, int y, int z)
                        {

                        }
                    }
                }";

            var analyzer = new ParameterCountAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(0, diagnostics.Length);
        }

        [Fact]
        public void CheckParameterCount1()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class testClass
                    {
                        public void testMethod(int x, int y, int z, int u, int v)
                        {

                        }
                    }
                }";

            var analyzer = new ParameterCountAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(0, diagnostics.Length);
        }

        [Fact]
        public void CheckParameterCount2()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class testClass
                    {
                        public void testMethod(int x, int y, int z, int u, int v, int w)
                        {

                        }
                    }
                }";

            var analyzer = new ParameterCountAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(1, diagnostics.Length);
        }
    }
}
