using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.CSharp.Testing.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace CodeAnalyzerTest
{
    [TestClass]
    public class CodeAnalyzerTest : CSharpAnalyzerTest<CodeAnalyzer.CodeAnalyzer, MSTestVerifier>
    {
        private async Task VerifyAnalyzerAsync(string testCode, params DiagnosticResult[] expectedDiagnostics)
        {
            var test = new CSharpAnalyzerTest<CodeAnalyzer.CodeAnalyzer, MSTestVerifier>
            {
                TestCode = testCode,
            };

            test.ExpectedDiagnostics.AddRange(expectedDiagnostics);
            await test.RunAsync();
        }

        [TestMethod]
        public async Task Test_PascalCaseRule_Violation()
        {
            var testCode = @"
            public class className
            {
            }
            ";

            var expectedDiagnostic = new DiagnosticResult(CodeAnalyzer.DiagnosticRules.PascalCaseRule)
                                      .WithLocation(2, 26).WithArguments("className");

            await VerifyAnalyzerAsync(testCode, expectedDiagnostic);
        }

        // Add other tests here
    }
}
