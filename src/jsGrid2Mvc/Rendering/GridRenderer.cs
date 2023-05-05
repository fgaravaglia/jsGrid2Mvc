using jsGrid2Mvc.Model;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;


namespace jsGrid2Mvc.Rendering
{
    internal class GridRenderer : ModelObjectRenderer<Grid>
    {
        /// <summary>
        /// Default 
        /// </summary>
        /// <param name="obj"></param>
        public GridRenderer(Grid obj) : base(obj, "##grid_id##")
        {
        }

        protected override string OnGenerateJavascript()
        {
            // Create javascript
            var script = new StringBuilder();

            // start jquery function
            script.AppendLine("$(function() {");
            script.AppendLine();

            // stati data if required
            foreach (var collection in this._ModelObject._StaticDataJsonArrays)
                script.AppendLine("var " + collection.Key + " = " + collection.Value + ";");

            // start refresh function
            script.AppendLine("function updateGrid(){ $(\"#" + this._CtrlIdPlaceHolder + "\").jsGrid(\"render\"); }");
            // end refresh function

            // Start script for grid
            script.AppendLine();
            script.AppendLine("$(\"#" + this._CtrlIdPlaceHolder + "\").jsGrid({");
            script.AppendLine("\twidth: \"" + this._ModelObject.Width + "\",");
            script.AppendLine("\theight: \"" + this._ModelObject.Height + "\",").AppendLine();
            if (this._ModelObject.Width.ToLowerInvariant() == "auto")
                script.AppendLine("\tautowidth: true,");

			// data
			if (this._ModelObject.ReadFromStaticData)
                script.AppendLine("\tdata: datasource,").AppendLine();
            
            if(this._ModelObject.Autoload.HasValue)
                script.AppendLine("\t" + ConvertBooleanOptionToJs("autoload", this._ModelObject.Autoload.Value) + ",");
            script.AppendLine("\t" + ConvertBooleanOptionToJs("filtering", this._ModelObject.FilteringEnabled) + ",");
            script.AppendLine("\t" + ConvertBooleanOptionToJs("inserting", this._ModelObject.InsertingEnabled) + ",");
            script.AppendLine("\t" + ConvertBooleanOptionToJs("editing", this._ModelObject.EditingEnabled) + ",");
            script.AppendLine("\t" + ConvertBooleanOptionToJs("sorting", this._ModelObject.SortingEnabled) + ",");
            script.AppendLine("\t" + ConvertBooleanOptionToJs("paging", this._ModelObject.PagingEnabled) + ",");
            if (this._ModelObject.PagingEnabled)
            {
                var renderer = new PagerRenderer(this._ModelObject.Pagination);
                script.AppendLine(renderer.ToInnerJavascripts());
            }

			if (this._ModelObject.ConfirmOnDelete)
            {
                script.AppendLine("\tconfirmDeleting: true,");
                if (String.IsNullOrEmpty(this._ModelObject.ConfirmOnDeleteMessage))
                    script.AppendLine("\tdeleteConfirm: \"Are you sure to delete current item?\",");
                else
                    script.AppendLine("\tdeleteConfirm: \"" + this._ModelObject.ConfirmOnDeleteMessage + "\",");
            }

            script.AppendLine("\tnoDataContent: \"No data found\",");

            if (!String.IsNullOrEmpty(this._ModelObject.ControllerEndpoint))
                AddController(script);

            if (!String.IsNullOrEmpty(this._ModelObject.OnItemInsertingCallBack))
                script.Append("\tonItemInserting: " + this._ModelObject.OnItemInsertingCallBack).AppendLine(",");

			if (!String.IsNullOrEmpty(this._ModelObject.OnItemUpdatingCallBack))
				script.Append("\tonItemUpdating: " + this._ModelObject.OnItemUpdatingCallBack).AppendLine(",");

			if (!String.IsNullOrEmpty(this._ModelObject.OnItemDeletingCallBack))
				script.Append("\tonItemDeleting: " + this._ModelObject.OnItemDeletingCallBack).AppendLine(",");

			if (this._ModelObject._Columns.Any())
            {
                // start fields
                script.AppendLine("\tfields: [");
                foreach (var col in this._ModelObject._Columns)
                {
                    var colIndex = this._ModelObject._Columns.IndexOf(col);
                    AddColumn(script, col);
                    if (colIndex < this._ModelObject._Columns.Count - 1)
                        script.Append(',');
                    script.AppendLine();
                }
                // end fields
                script.AppendLine("\t],").AppendLine();
            }

            script.AppendLine("\tloadIndication: true,");
            script.AppendLine("\tloadIndicationDelay: 500,");
            script.AppendLine("\tloadMessage: \"Please, wait...\",");
            script.AppendLine("\tloadShading: true,");
            script.AppendLine("\tupdateOnResize: true");

            // End script for grid
            script.AppendLine("});");

            // End jquery function
            script.AppendLine();
            script.AppendLine("});");


            // Insert grid id where needed (in columns)
            script.Replace(_CtrlIdPlaceHolder, this._ModelObject.ControlId);

            return script.ToString();
        }
        /// <summary>
        /// Renders the required Html Elements
        /// </summary>
        /// <returns></returns>
        protected override string OnGenerateHtml()
        {
            // Create table which is used to render grid
            var table = new StringBuilder();

			// add html for table caption
			if (!String.IsNullOrEmpty(this._ModelObject.Label))
            {
                // start caption Row
                table.AppendLine("<div class=\"row\">");
                // start column for entire row
                table.AppendLine("\t<div class=\"col-md-12\">");
                // set caption content
                table.Append("\t\t<div class=\"d-flex justify-content-center fst-italic border border-info\">").Append(this._ModelObject.Label).AppendLine("</div>");
                // end column for entire row
                table.AppendLine("\t</div>");
				// end caption row
				table.AppendLine("</div>");
			}

            //add table
            table.AppendFormat("<div id=\"{0}\"></div>", this._ModelObject.ControlId);
            return table.ToString();
        }

        #region Private Methods

        static string ConvertBooleanOptionToJs(string key, bool value)
        {
            return $"{key}: {value.ToString().ToLowerInvariant()}";
        }

        static string ConvertStringOptionToJs(string key, string value)
        {
            return $"{key}: \"{value}\"";
        }

        static void AddColumn(StringBuilder script, Column col)
        {
            script.Append("\t\t{ ")
                    .Append(ConvertStringOptionToJs("name", col.ControlId))
                    .Append(", ").Append(ConvertStringOptionToJs("type", col.ColumnType.ToString()))
                    .Append(", width: " + col.Width);

            script.Append(", " + ConvertBooleanOptionToJs("sorting", col.IsSortingEnabled));
			script.Append(", " + ConvertBooleanOptionToJs("autosearch", col.IsSearchingEnabled));

			// TO sort number filed in right way, I need to specify the sorter type. see http://js-grid.com/docs/#grid-fields
			if (col.IsSortingEnabled && col.ColumnType == Column.EnumColumnTypes.number)
                script.Append(", sorter: \"number\"");

            
			if (col.Align.HasValue)
                script.Append(", " + ConvertStringOptionToJs("align", col.Align.Value.ToString()));

            if (!String.IsNullOrEmpty(col.Title))
                script.Append(", " + ConvertStringOptionToJs("title", col.Title));

            if (!String.IsNullOrEmpty(col._Validate))
                script.Append(", ").Append(ConvertStringOptionToJs("validate", col._Validate));

            if (col.ColumnType == Column.EnumColumnTypes.select)
            {
                script.Append(", ").Append("items: " + col.RefCollectionName)
                    .Append(", ").Append(ConvertStringOptionToJs("valueField", col.RefCollectionValueId))
                    .Append(", ").Append(ConvertStringOptionToJs("textField", col.RefCollectionTextField));
            }
            script.Append(" }");
        }

        void AddController(StringBuilder script)
        {
            // start controller
            script.AppendLine("\tcontroller: {");

            script.AppendLine("\t\tloadData: function(filter) {");
            script.AppendLine("\t\t\treturn $.ajax({");
            script.AppendLine("\t\t\t\ttype: \"GET\", url: \"" + this._ModelObject.ControllerEndpoint + "\", data: filter, headers: { 'Content-Type': 'application/json' }, dataType: \"json\"");
            script.AppendLine("\t\t\t});");
            script.AppendLine("\t\t},");

            script.AppendLine("\t\tinsertItem: function(item) {");
            script.AppendLine("\t\t\t$.ajax({");
            script.AppendLine("\t\t\t\ttype: \"POST\", url: \"" + this._ModelObject.ControllerEndpoint + "\", data: JSON.stringify(item), headers: { 'Content-Type': 'application/json' }, dataType: \"json\"");
            script.AppendLine("\t\t\t})");
            script.AppendLine("\t\t\t.done(function(item) { $(\"#" + this._ModelObject.ControlId + "\").jsGrid(\"loadData\"); });");
            script.AppendLine("\t\t\tupdateGrid();");
            script.AppendLine("\t\t\treturn item;");
            script.AppendLine("\t\t},");

            script.AppendLine("\t\tupdateItem: function(item) {");
            script.AppendLine("\t\t\t$.ajax({");
            script.AppendLine("\t\t\t\ttype: \"PUT\", url: \"" + this._ModelObject.ControllerEndpoint + "/\" + item.id, data: JSON.stringify(item), headers: { 'Content-Type': 'application/json' }, dataType: \"json\"");
            script.AppendLine("\t\t\t});");
            script.AppendLine("\t\t\treturn item;");
            script.AppendLine("\t\t},");

            script.AppendLine("\t\tdeleteItem: function(item) {");
            script.AppendLine("\t\t\t$.ajax({");
            script.AppendLine("\t\t\t\ttype: \"DELETE\", url: \"" + this._ModelObject.ControllerEndpoint + "/\" + item.id, headers: { 'Content-Type': 'application/json' }, dataType: \"json\"");
            script.AppendLine("\t\t\t})");
            script.AppendLine("\t\t\t.done(function(item) { alert(\"done!\"); $(\"#\" + this._ModelObject.ControlId + \"\").jsGrid(\"loadData\"); });");
            script.AppendLine("\t\t\tupdateGrid();");
            script.AppendLine("\t\t\treturn item;");
            script.AppendLine("\t\t}");

            //end controler
            script.AppendLine("\t},");
        }
        #endregion
    }
}
