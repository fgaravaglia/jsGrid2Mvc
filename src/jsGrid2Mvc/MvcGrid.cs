using jsGrid2Mvc.Model;
using jsGrid2Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace jsGrid2Mvc
{
    /// <summary>
    /// MVC helper to render automatically the grid on CSHTML page
    /// </summary>
    public class MvcGrid : IHtmlContent
    {
        /// <summary>
        /// The grid to render
        /// </summary>
        public Grid Model { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MvcGrid(Grid model)
        { 
            this.Model = model ?? throw new ArgumentNullException(nameof(model));    
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="encoder"></param>
        public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            var renderer = new GridRenderer(this.Model);
            writer.Write(renderer.ToJavascripts());
        }
    }
}
