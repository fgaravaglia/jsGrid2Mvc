using jsGrid2Mvc.Configuration;
using jsGrid2Mvc.Model;
using jsGrid2Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace jsGrid2Mvc
{
    /// <summary>
    /// Uitlity to manage rendering of components
    /// </summary>
    public static class GridExtensions
    {
        /// <summary>
        /// Sets the grid in readonly mode, with sortable and firterable capability
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="apiEndpoint"></param>
        /// <returns></returns>
        public static Grid SetReadOnly(this Grid grid, string apiEndpoint) 
        {
            grid.WithDimensions("100%", "auto")
                .UseController(apiEndpoint)
                .Sortable()
                .Paginated()
                .Filterable();
			return grid;
        }
		/// <summary>
		/// Sets the grid in readonly mode, with sortable and firterable capability
		/// </summary>
		/// <param name="grid"></param>
		/// <returns></returns>
		public static Grid SetReadOnly<T>(this Grid grid, List<T> datasource)
		{
            grid.SetReadOnlyWithSerializedData(JsonSerializer.Serialize(datasource));
			return grid;
		}
		/// <summary>
		/// Sets the grid in readonly mode, with sortable and firterable capability
		/// </summary>
		/// <param name="grid"></param>
		/// <returns></returns>
		public static Grid SetReadOnlyWithSerializedData(this Grid grid, string serializedArray)
		{
			grid.WithDimensions("100%", "auto")
				.UseStaticData(serializedArray)
				.Sortable()
				.Paginated()
				.Filterable();
			return grid;
		}
        /// <summary>
        /// Sets referenced collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="grid"></param>
        /// <param name="collection"></param>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public static Grid UseReferencedCollection<T>(this Grid grid, List<T> collection, string collectionName)
        {
            return grid.UseReferencedCollection(collectionName, JsonSerializer.Serialize(collection));
		}
		/// <summary>
		/// Renders the HTML part fo the grid
		/// </summary>
		/// <param name="grid"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static HtmlString RenderHtml(this Grid grid)
        {
            if (grid == null) throw new ArgumentNullException(nameof(grid));

            return new HtmlString(new GridRenderer(grid).ToHtml());
        }
        /// <summary>
        /// Renders the js part fo the grid
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string RenderScripts(this Grid grid)
        {
            if (grid == null) throw new ArgumentNullException(nameof(grid));

            return new GridRenderer(grid).ToJavascripts();
        }
        /// <summary>
        /// Sets the grid starting fomr default configuration
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Grid FromConfiguration(this Grid grid, IConfiguration config)
        {
			if (grid == null) throw new ArgumentNullException(nameof(grid));
			if (config == null) throw new ArgumentNullException(nameof(config));

            var settings = config.GetGridSettings();
            grid.WithDimensions(settings.Width, settings.Height)
                .Editable(false, false, confirmOnDelete: settings.ConfirmOnDelete, confirmDeleteMessage: settings.ConfirmOnDeleteMessage);

            if (settings.SortingEnabled)
                grid.Sortable();
			if (settings.FilteringEnabled)
				grid.Filterable();
            if (settings.PagingEnabled)
                grid.Paginated(settings.Pagination);

            return grid;
		}
    }
}
