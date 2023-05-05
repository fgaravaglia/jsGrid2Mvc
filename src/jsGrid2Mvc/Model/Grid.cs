using jsGrid2Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace jsGrid2Mvc.Model
{
    /// <summary>
    /// Model for grid
    /// </summary>
    public class Grid : IModelObject
    {
        internal const string STATIC_DATASOURCE_NAME = "datasource";

        #region Properties
        /// <summary>
        /// Id of grid
        /// </summary>
        public string ControlId { get; private set; }
        /// <summary>
        /// Title of grid
        /// </summary>
        public string Label { get; private set; }
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
        public bool InsertingEnabled { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool EditingEnabled { get; private set; }
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
        /// TRUE if grid has to use static dataset
        /// </summary>
        public bool ReadFromStaticData { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? Autoload { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool ConfirmOnDelete { get; set; }
        /// <summary>
        /// Message to show to user before deleting a record
        /// </summary>
        public string ConfirmOnDeleteMessage { get; private set; }
        /// <summary>
        /// Base url of API that provides the data for grid
        /// </summary>
        public string? ControllerEndpoint { get; private set; }
        /// <summary>
        /// Function to be executed on Item Inserting event
        /// </summary>
        public string? OnItemInsertingCallBack { get; private set; }
		/// <summary>
		/// Function to be executed on Item Updating event
		/// </summary>
		public string? OnItemUpdatingCallBack { get; private set; }
		/// <summary>
		/// Function to be executed on Item Deleting event
		/// </summary>
		public string? OnItemDeletingCallBack { get; private set; }
		#endregion

		#region Fields
		internal readonly List<Column> _Columns = new();

        internal Dictionary<string, string> _StaticDataJsonArrays = new Dictionary<string, string>();
        #endregion

        /// <summary>
        /// Default Constr
        /// </summary>
        /// <param name="gridId"></param>
        public Grid(string gridId) : this(gridId, "")
        {

        }
        /// <summary>
        /// Constr
        /// </summary>
        /// <param name="gridId"></param>
        /// <param name="title"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Grid(string gridId, string title)
        {
            if (String.IsNullOrEmpty(gridId))
                throw new ArgumentNullException(nameof(gridId));
            this.ControlId = gridId;
            this.Width = "100%";
            this.Height = "400px";
            this.Label = String.IsNullOrEmpty(title) ? String.Empty : title;
            this._StaticDataJsonArrays = new Dictionary<string, string>();
            this.Pagination = new PaginationSettings();
            this.ConfirmOnDelete = false;
            this.ConfirmOnDeleteMessage = "";
        }
        /// <summary>
        /// Sets grid dimensions
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Grid WithDimensions(string width, string height)
        {
            if (String.IsNullOrEmpty(width))
                throw new ArgumentNullException(nameof(width));
            if (String.IsNullOrEmpty(height))
                throw new ArgumentNullException(nameof(height));

            this.Width = width;
            this.Height = height;
            return this;
        }
		/// <summary>
		/// Sets the grid caption
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public Grid WithCaption(string title)
        {
			if (String.IsNullOrEmpty(title))
				throw new ArgumentNullException(nameof(title));
			this.Label   = title;
			return this;
		}
        /// <summary>
        /// Set static data as datasource on page.
        /// </summary>
        /// <param name="jsonArray"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Grid UseStaticData(string jsonArray)
        {
            if (String.IsNullOrEmpty(jsonArray))
                throw new ArgumentNullException(nameof(jsonArray));

            this.ReadFromStaticData = true;
            if (this._StaticDataJsonArrays.ContainsKey(STATIC_DATASOURCE_NAME))
                this._StaticDataJsonArrays[STATIC_DATASOURCE_NAME] = jsonArray;
            else
                this._StaticDataJsonArrays.Add(STATIC_DATASOURCE_NAME, jsonArray);

            this.ControllerEndpoint = "";
            return this;
        }
        /// <summary>
        /// Sets the controlelr to be use to manage dataset
        /// </summary>
        /// <param name="controllerEndpoint"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Grid UseController(string controllerEndpoint)
        {
            if (String.IsNullOrEmpty(controllerEndpoint))
                throw new ArgumentNullException(nameof(controllerEndpoint));

            this.Autoload = true;
            this.ControllerEndpoint = controllerEndpoint;
            this.ReadFromStaticData = false;
            if (this._StaticDataJsonArrays.ContainsKey(STATIC_DATASOURCE_NAME))
                this._StaticDataJsonArrays.Remove(STATIC_DATASOURCE_NAME);

            return this;
        }
        /// <summary>
        /// Set referenced collection on page
        /// </summary>
        /// <param name="refCollectionname"></param>
        /// <param name="jsonArray"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Grid UseReferencedCollection(string refCollectionname, string jsonArray)
        {
            if (String.IsNullOrEmpty(refCollectionname))
                throw new ArgumentNullException(nameof(refCollectionname));
            if (String.IsNullOrEmpty(jsonArray))
                throw new ArgumentNullException(nameof(jsonArray));

            if (this._StaticDataJsonArrays.ContainsKey(refCollectionname))
                this._StaticDataJsonArrays[refCollectionname] = jsonArray;
            else
                this._StaticDataJsonArrays.Add(refCollectionname, jsonArray);
            return this;
        }
        /// <summary>
        /// Adds a new column
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public Grid AddColumn(Column column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));
            if (String.IsNullOrEmpty(column.ControlId))
                throw new ArgumentNullException(nameof(column), $"{nameof(column.ControlId)} cannot be null");

            if (this._Columns.Any(x => x.ControlId.Equals(column.ControlId, StringComparison.InvariantCultureIgnoreCase)))
                throw new InvalidOperationException($"Unable to add column {column.ControlId} item already there");

            this._Columns.Add(column);
            return this;
        }
        /// <summary>
        /// Enable sorting feature
        /// </summary>
        /// <returns></returns>
        public Grid Sortable()
        {
            this.SortingEnabled = true;
            return this;
        }
		/// <summary>
		/// Disable sorting feature
		/// </summary>
		/// <returns></returns>
		public Grid NotSortable()
		{
			this.SortingEnabled = false;
			return this;
		}
		/// <summary>
		/// Enables filtering feature
		/// </summary>
		/// <returns></returns>
		public Grid Filterable()
        {
            this.FilteringEnabled = true;
            return this;
        }
		/// <summary>
		/// Disable filtering feature
		/// </summary>
		/// <returns></returns>
		public Grid NotFilterable()
		{
			this.FilteringEnabled = false;
			return this;
		}
		/// <summary>
		/// Enable editing capabilities on datasource
		/// </summary>
		/// <param name="allowInsert"></param>
		/// <param name="allowDeleteOrUpdate"></param>
		/// <param name="confirmOnDelete"></param>
		/// <param name="confirmDeleteMessage"></param>
		/// <returns></returns>
		public Grid Editable(bool allowInsert, bool allowDeleteOrUpdate, bool confirmOnDelete = true, string confirmDeleteMessage = "Are you sure to delete current item?")
        {
            this.InsertingEnabled = allowInsert;
            this.EditingEnabled = allowDeleteOrUpdate;
            this.ConfirmOnDelete = confirmOnDelete;
            this.ConfirmOnDeleteMessage = confirmDeleteMessage;
            return this;
        }
		/// <summary>
		/// Disable filtering feature
		/// </summary>
		/// <returns></returns>
		public Grid ReadOnly()
		{
			this.InsertingEnabled = false;
			this.EditingEnabled = false;
			this.ConfirmOnDelete = false;
			return this;
		}
		/// <summary>
		/// Enables paging feature
		/// </summary>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public Grid Paginated(int pageSize = 20)
        {
            this.PagingEnabled = true;
            this.Pagination.PageSize = pageSize;
            return this;
        }
		/// <summary>
		/// Enables paging feature
		/// </summary>
		/// <param name="settings"></param>
		/// <returns></returns>
		public Grid Paginated(PaginationSettings settings)
		{
            this.Pagination = settings ?? throw new ArgumentNullException(nameof(settings));
			return this;
		}
		/// <summary>
		/// Sets callback for inserting new row
		/// </summary>
		/// <param name="jsFunction"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public Grid UsingThisFunctionOnItemInserting(string jsFunction)
        {
            if (String.IsNullOrEmpty(jsFunction))
                throw new ArgumentNullException(nameof(jsFunction));
            if(!jsFunction.ToLowerInvariant().Trim().StartsWith("function(args) {"))
                throw new ArgumentException("OnItemInserting callbak must be a function", nameof(jsFunction));

            this.OnItemInsertingCallBack = jsFunction;
            return this;
        }
        /// <summary>
        /// Sets callback for updating row
        /// </summary>
        /// <param name="jsFunction"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
		public Grid UsingThisFunctionOnItemUpdating(string jsFunction)
		{
			if (String.IsNullOrEmpty(jsFunction))
				throw new ArgumentNullException(nameof(jsFunction));
			if (!jsFunction.ToLowerInvariant().Trim().StartsWith("function(args) {"))
				throw new ArgumentException("OnItemUpdating callbak must be a function", nameof(jsFunction));

			this.OnItemUpdatingCallBack = jsFunction;
			return this;
		}
        /// <summary>
        /// Sets callback for deleting row
        /// </summary>
        /// <param name="jsFunction"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
		public Grid UsingThisFunctionOnItemDeleting(string jsFunction)
		{
			if (String.IsNullOrEmpty(jsFunction))
				throw new ArgumentNullException(nameof(jsFunction));
			if (!jsFunction.ToLowerInvariant().Trim().StartsWith("function(args) {"))
				throw new ArgumentException("onItemDeleting callbak must be a function", nameof(jsFunction));

			this.OnItemDeletingCallBack = jsFunction;
			return this;
		}
	}
}
