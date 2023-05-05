using jsGrid2Mvc.Configuration;
using jsGrid2Mvc.Model;
using Microsoft.AspNetCore.Html;

namespace jsGrid2Mvc.Samples.Models.jsGridSamples
{
    public class ControllerModel
    {
		readonly Grid _Grid;


		public ControllerModel(IConfiguration config)
		{ 
			String deleteConfirmFunction = @"function (item) { return 'The item ' + item.name + ' will be removed. Are you sure?'; }";
			//var grid = new Grid("testGrid", "Grid to show how to interact with controller")
			//                .WithDimensions("auto", "auto")
			//                .UseController("/api/data")
			//                .UseReferencedCollection("testGrid_countries", "[\r\n        { Name: \"\", Id: 0 },\r\n        { Name: \"United States\", Id: 1 },\r\n        { Name: \"Canada\", Id: 2 },\r\n        { Name: \"United Kingdom\", Id: 3 }\r\n    ]")
			//                   .AddColumn(new Column("name", "150", title: "Client Name").AsText().Required().Sortable().AlignOn(Column.ColumnAlignEnum.left))
			//                   .AddColumn(new Column("age", "50").AsNumber().Sortable().AlignOn(Column.ColumnAlignEnum.center))
			//                   .AddColumn(new Column("address", "200").AsText().Sortable().AlignOn(Column.ColumnAlignEnum.center))
			//                   .AddColumn(new Column("country", "200").AsSelect("testGrid_countries", "Id", "Name").Sortable().AlignOn(Column.ColumnAlignEnum.center))
			//                   .AddColumn(new Column("married", "50", title: "Is Married").AsCheckBox().AlignOn(Column.ColumnAlignEnum.center))
			//                   .AddColumn(new Column("control", "150", title: "Actions").AsControl().AlignOn(Column.ColumnAlignEnum.right))
			//               .Sortable()
			//               .Editable(true, true,  confirmOnDelete: true, confirmDeleteMessage: deleteConfirmFunction)
			//               .Paginated()
			//               .Filterable();
			var grid = new Grid("testGrid", "Grid to show how to interact with controller")
				.FromConfiguration(config)
				.UseController("/api/data")
				.UseReferencedCollection("testGrid_countries", "[\r\n        { Name: \"\", Id: 0 },\r\n        { Name: \"United States\", Id: 1 },\r\n        { Name: \"Canada\", Id: 2 },\r\n        { Name: \"United Kingdom\", Id: 3 }\r\n    ]")
				   .AddColumn(new Column("name", "150", title: "Client Name").AsText().Required().Sortable().AlignOn(Column.EnumColumnAlign.left))
				   .AddColumn(new Column("age", "50").AsNumber().Sortable().AlignOn(Column.EnumColumnAlign.center))
				   .AddColumn(new Column("address", "200").AsText().Sortable().AlignOn(Column.EnumColumnAlign.center))
				   .AddColumn(new Column("country", "200").AsSelect("testGrid_countries", "Id", "Name").Sortable().AlignOn(Column.EnumColumnAlign.center))
				   .AddColumn(new Column("married", "50", title: "Is Married").AsCheckBox().AlignOn(Column.EnumColumnAlign.center))
				   .AddColumn(new Column("control", "150", title: "Actions").AsControl().AlignOn(Column.EnumColumnAlign.right))
			   .Editable(true, true, confirmOnDelete: true, confirmDeleteMessage: deleteConfirmFunction);
			this._Grid = grid;
		}

		public HtmlString HtmlGrid { get { return this._Grid.RenderHtml(); } }
        public MvcGrid JsGrid { get { return new MvcGrid(this._Grid); } }
    }
}
