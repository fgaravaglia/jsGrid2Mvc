using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jsGrid2Mvc.Model
{
    /// <summary>
    /// Settings for pagination of the grid
    /// </summary>
    public  class PaginationSettings
    {
        public int PageIndex { get;  set; }

        public int PageSize { get;  set; }

        public int? ButtonCount { get;  set; }

        public string Format { get;  set; }

        public string PrevText { get;  set; }

        public string NextText { get;  set; }

        public string FirstText { get;  set; }

        public string LastText { get;  set; }

        public string NavigatorNextText { get;  set; }

        public string NavigatorPrevText { get;  set; }

        public PaginationSettings()
        {
            this.PageIndex = 1;
            this.PageSize = 20;
            this.ButtonCount = 15;
            this.Format = "Pages: {first} {prev} {pages} {next} {last}    {pageIndex} of {pageCount}";
            this.PrevText = "Prev";
            this.NextText = "Next";
            this.FirstText = "First";
            this.LastText = "Last";
            this.NavigatorNextText = "...";
            this.NavigatorPrevText = "...";
        }

    }
}
