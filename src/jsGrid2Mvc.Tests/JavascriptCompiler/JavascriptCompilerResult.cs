using System.Collections.ObjectModel;
using NUglify;

namespace jsGrid2Mvc.Tests.JavascriptCompiler
{
    public class JavascriptCompilerResult
    {
        public JavascriptCompilerResult()
        {
            Errors = new Collection<UglifyError>();
        }

        public bool IsValid 
        { 
            get { return !HasErrors; }
        }

        public bool HasErrors
        {
            get { return Errors.Count > 0; }
        }

        public ICollection<UglifyError> Errors { get; set; } 
    }
}
