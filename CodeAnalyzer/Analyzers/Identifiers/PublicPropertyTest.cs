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
    public class PublicPropertyTest
    {
        private static readonly MetadataReference[] References = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        };

        [Fact]
        public void CheckPublicProperty0()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    class testClass
                    {

                    }
                }";

            var analyzer = new PublicPropertyAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(0, diagnostics.Length);
        }

        [Fact]
        public void CheckPublicProperty1()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    class testClass
                    {
                        int PublicProperty { get; set; }
                    }
                }";

            var analyzer = new PublicPropertyAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(1, diagnostics.Length);
        }
    }
}
