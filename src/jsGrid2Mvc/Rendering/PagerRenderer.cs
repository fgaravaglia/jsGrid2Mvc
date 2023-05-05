using jsGrid2Mvc.Model;
using System.Text;

namespace jsGrid2Mvc.Rendering
{
    internal class PagerRenderer
    {
        readonly PaginationSettings _Settings;

        public PagerRenderer(PaginationSettings settings)
        { 
            this._Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <summary>
        /// Renders only JS
        /// </summary>
        /// <returns></returns>
        public string ToInnerJavascripts()
        {
            // Create javascript
            var script = new StringBuilder();

            script.AppendLine("\tpageIndex: " + this._Settings.PageIndex.ToString() + ", ");
            script.AppendLine("\tpageSize: " + this._Settings.PageSize.ToString() + ", ");
            if(this._Settings.ButtonCount.HasValue)
                script.AppendLine("\tpageButtonCount: " + this._Settings.ButtonCount.ToString() + ", ");
            script.Append('\t').Append(ConvertStringOptionToJs("pagerFormat", this._Settings.Format)).Append(",").AppendLine();
            script.Append('\t').Append(ConvertStringOptionToJs("pagePrevText", this._Settings.PrevText)).Append(",").AppendLine();
            script.Append('\t').Append(ConvertStringOptionToJs("pageNextText", this._Settings.NextText)).Append(",").AppendLine();
            script.Append('\t').Append(ConvertStringOptionToJs("pageFirstText", this._Settings.FirstText)).Append(",").AppendLine();
            script.Append('\t').Append(ConvertStringOptionToJs("pageLastText", this._Settings.LastText)).Append(",").AppendLine();
            script.Append('\t').Append(ConvertStringOptionToJs("pageNavigatorNextText", this._Settings.NavigatorNextText)).Append(',').AppendLine();
            script.Append('\t').Append(ConvertStringOptionToJs("pageNavigatorPrevText", this._Settings.NavigatorPrevText)).Append(',').AppendLine();

            return script.ToString();
        }

        static string ConvertStringOptionToJs(string key, string value)
        {
            return $"{key}: \"{value}\"";
        }
    }
}
