using jsGrid2Mvc.Model;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("jsGrid2Mvc.Tests")]

namespace jsGrid2Mvc.Rendering
{
    public abstract class ModelObjectRenderer<T> where T : IModelObject
    {
        #region Fields

        protected readonly T _ModelObject;
        protected readonly string _CtrlIdPlaceHolder;

        #endregion

        /// <summary>
        /// default constr
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="idPlaceHolder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected ModelObjectRenderer(T obj, string? idPlaceHolder)
        {
            _ModelObject = obj ?? throw new ArgumentNullException(nameof(obj));
            _CtrlIdPlaceHolder = idPlaceHolder ?? "##PLACEHOLDER##";
        }

        #region Protected methods

        protected abstract string OnGenerateJavascript();
        /// <summary>
        /// Renders the required Html Elements
        /// </summary>
        /// <returns></returns>
        protected virtual string OnGenerateHtml()
        {
            return "";
        }

        protected virtual void ReplacePlaceHolders(StringBuilder script)
        {
            // Insert grid id where needed (in columns)
            script.Replace(_CtrlIdPlaceHolder, _ModelObject.ControlId);
        }
        
        #endregion

        /// <summary>
        /// Renders only JS
        /// </summary>
        /// <returns></returns>
        public string ToJavascripts()
        {
            // Create javascript
            var script = new StringBuilder();

            // Start script
            // type=\"text/javascript\"
            script.AppendLine("<script>");
            script.Append(OnGenerateJavascript());
            script.AppendLine("</script>");
            return script.ToString();
        }
        /// <summary>
        /// Renders only the HTML
        /// </summary>
        /// <returns></returns>
        public string ToHtml()
        {
            return OnGenerateHtml();

        }
        /// <summary>
        ///     Creates and returns javascript + required html elements to render component
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // Create javascript
            var script = new StringBuilder();

            // Start script
            script.AppendLine(ToJavascripts());
            script.Append(ToHtml());

            ReplacePlaceHolders(script);

            // Return script + required elements
            return script.ToString();
        }
    }
}