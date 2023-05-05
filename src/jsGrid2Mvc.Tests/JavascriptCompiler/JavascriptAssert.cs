using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace jsGrid2Mvc.Tests.JavascriptCompiler
{
    public class JavascriptVerifier
    {
        readonly JavaScriptCompiler JavascriptCompiler;

        public JavascriptVerifier()
        { 
            this.JavascriptCompiler = new JavaScriptCompiler();
        }

        public void IsValid(string javascript, List<string> ignoreRuleCodes)
        {
            JavascriptCompiler.AddIgnoreRules(ignoreRuleCodes);

            var result = JavascriptCompiler.Compile(javascript);

            JavascriptCompiler.RemoveIgnoreRules(ignoreRuleCodes);

            var errors = string.Join(", ", result.Errors.Select(x => $"line: {x.StartLine}:{x.EndLine}, column: {x.StartColumn}:{x.EndColumn}, error: {x.Message}, code: {x.ErrorCode}"));
            Assert.That(result.IsValid, Is.True,  $"Validated Script:\n{javascript}\nError(s) while compiling: {errors}");          
        }

        public static void IsValid(string javascript, bool enabled)
        {
            if (!enabled)
                return;

            JavascriptVerifier verifier = new JavascriptVerifier();
            verifier.IsValid(javascript, new List<string>() { "JS1195" });    
        }
    }
}
