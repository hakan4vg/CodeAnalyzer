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

namespace CodeAnalyzer.Tests
{
    [TestClass]
    public class CodeAnalyzerTests : CSharpAnalyzerTest<CodeAnalyzer, MSTestVerifier>
    {
        [TestMethod]
        public async Task Test_PascalCaseRule_Violation()
        {
            var testCode = @"
            public class className
            {
            }
            ";

            var expectedDiagnostic = new DiagnosticResult(DiagnosticRules.PascalCaseRule)
                                                          .WithLocation(2, 26).WithArguments("className");
            await VerifyAnalyzerAsync(testCode, expectedDiagnostic);
        }

        private async Task VerifyAnalyzerAsync(string testCode, DiagnosticResult expectedDiagnostic)
        {
            throw new NotImplementedException();
        }
    }
}
