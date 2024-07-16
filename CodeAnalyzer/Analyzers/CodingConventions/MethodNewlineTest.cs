using CodeAnalyzer.Analyzers.Naming;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalyzer.Analyzers.CodingConventions
{
    public class MethodNewlineTest
    {
        private static readonly MetadataReference[] References = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        };

        [Fact]
        public void CheckMethodNewline0()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class testClass
                    {
                        public void testMethod0()
                        {

                        }

                        public void testMethod1()
                        {

                        }
                    }
                }";

            var analyzer = new MethodNewlineAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(0, diagnostics.Length);
        }

        [Fact]
        public void CheckMethodNewline1()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class testClass
                    {
                        public void testMethod0()
                        {

                        }
                        public void testMethod1()
                        {

                        }
                        public void testMethod2()
                        {

                        }
                    }
                }";

            var analyzer = new MethodNewlineAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(2, diagnostics.Length);
        }

        [Fact]
        public void CheckMethodNewline2()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class testClass
                    {
                        public void testMethod0()
                        {

                        }
                        public void testMethod1()
                        {

                        }
                        public void testMethod2()
                        {

                        }
                        public void testMethod2()
                        {

                        }
                    }
                }";

            var analyzer = new MethodNewlineAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(3, diagnostics.Length);
        }

        [Fact]
        public void CheckMethodNewline3()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class testClass
                    {
                        public void testMethod0()
                        {

                        }
                        public void testMethod1()
                        {

                        }
                        public void testMethod2()
                        {

                        }

                        public void testMethod2()
                        {

                        }
                    }
                }";

            var analyzer = new MethodNewlineAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(2, diagnostics.Length);
        }

        [Fact]
        public void CheckMethodNewline4()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class testClass
                    {
                        public void testMethod0()
                        {

                        }
                        public void testMethod1()
                        {

                        }

                        public void testMethod2()
                        {

                        }
                        public void testMethod2()
                        {

                        }
                    }
                }";

            var analyzer = new MethodNewlineAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(2, diagnostics.Length);
        }
    }
}
