using jsGrid2Mvc.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jsGrid2Mvc.Configuration
{
    /// <summary>
    /// Class to map settings from appSettings.json file, in order to share common behaviour to grids
    /// </summary>
    public class GridSettings
    {
		/// <summary>
		/// width of grid
		/// </summary>
		public string Width { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public string Height { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public bool FilteringEnabled { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public bool SearchEnabled { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public bool SortingEnabled { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public bool PagingEnabled { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public PaginationSettings Pagination { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public bool ConfirmOnDelete { get; set; }
		/// <summary>
		/// Message to show to user before deleting a record
		/// </summary>
		public string ConfirmOnDeleteMessage { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public GridSettings() 
		{
			this.Width = "100%";
			this.Height = "100%";
			this.Pagination = new PaginationSettings();
			this.ConfirmOnDelete = true;
			this.ConfirmOnDeleteMessage = "Are you sure to delete current item?";
		}
	}
}
