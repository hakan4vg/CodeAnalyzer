using CodeAnalyzerSpace;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using Xunit;

namespace CodeAnalyzer.Analyzers.Encapsulation
{
    public class GetterSetterAnalyzerTests
    {
        private static readonly MetadataReference[] References = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        };

        [Fact]
        public void TestGetterSetterAnalyzer_NoDiagnostics()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class TestClass
                    {
                        private int _publicProperty;
                    
                        public int PublicProperty
                        {
                            get { return _publicProperty; }
                            set { _publicProperty = value; }
                        }
                    
                        private string _publicField;
                    
                        public string PublicField
                        {
                            get { return _publicField; }
                            set { _publicField = value; }
                        }
                    
                        public void TestMethod()
                        {
                            PublicProperty = 10;
                            PublicField = ""Test"";
                        }
                    }
                }";

            var analyzer = new GetterSetterAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Empty(diagnostics);
        }

        [Fact]
        public void TestGetterSetterAnalyzer_DiagnosticOnMissingAccessor()
        {
            string source = @"
                using System;

                namespace TestNamespace
                {
                    public class TestClass
                    {
                        public int MissingAccessorProperty { get; }

                        public void TestMethod()
                        {
                            var value = MissingAccessorProperty;
                        }
                    }
                }";

            var analyzer = new GetterSetterAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Single(diagnostics);
        }
        [Fact]
        public void CheckGetterSetter0()
        {
            string source = @"
        using System;

        namespace TestNamespace
        {
            public class testClass
            {
                public int i { get; };
            }
        }";

            var analyzer = new GetterSetterAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(1, diagnostics.Length);
        }

        [Fact]
        public void CheckGetterSetter1()
        {
            string source = @"
        using System;

        namespace TestNamespace
        {
            public class testClass
            {
                public int i { set; };
            }
        }";

            var analyzer = new GetterSetterAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(1, diagnostics.Length);
        }

        [Fact]
        public void CheckGetterSetter2()
        {
            string source = @"
                using System;
                
                namespace TestNamespace
                {
                    public class testClass
                    {
                        public int i { get; set;}
                    }
                }";

            var analyzer = new GetterSetterAnalyzer();
            var diagnostics = CodeAnalyzerSpace.CodeAnalyzer.GetSortedDiagnostics(analyzer, source);

            Assert.Equal(0, diagnostics.Length);
        }
    }
}
