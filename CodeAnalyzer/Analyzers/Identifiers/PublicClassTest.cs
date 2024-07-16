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
    public class PublicClassTest
    {
        private static readonly MetadataReference[] References = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        };

        [Fact]
        public void CheckPublicClass0()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    class testClass
                    {

                    }
                }";

            var analyzer = new PublicClassAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(1, diagnostics.Length);
        }
    }
}
