using jsGrid2Mvc.Rendering;
using jsGrid2Mvc.Model;
using jsGrid2Mvc.Tests.JavascriptCompiler;
using System.Net;

namespace jqGrid2Mvc.Tests.Rendering
{
    public class GridRendererTests
    {
        Grid _TestGrid;
        GridRenderer _Renderer;
        bool JSvalidationEnabled;

        [SetUp]
        public void Setup()
        {
            this._TestGrid = new Grid("testGrid");
            this.JSvalidationEnabled = false;
        }

        [Test]
        public void Js_Contains_gridControlId()
        {
            //********** GIVEN
            this._Renderer = new GridRenderer(this._TestGrid);

            //********** WHEN
            var scripts = this._Renderer.ToJavascripts();

            //********** ASSERT
            StringAssert.Contains("$(\"#testGrid\").jsGrid", scripts);
            JavascriptVerifier.IsValid(scripts, this.JSvalidationEnabled);
            Assert.Pass();
        }

        [Test]
        public void Html_Contains_gridControlId()
        {
            //********** GIVEN
            this._Renderer = new GridRenderer(this._TestGrid);

            //********** WHEN
            var html = this._Renderer.ToHtml();

            //********** ASSERT
            StringAssert.Contains(@"<div id=""testGrid""></div>", html);
            Assert.Pass();
        }

        [Test]
        public void Render_IsValid_ForEmptyGrid()
        {
            //********** GIVEN
            this._Renderer = new GridRenderer(this._TestGrid);

            //********** WHEN
            var html = this._Renderer.ToJavascripts();

            //********** ASSERT
            JavascriptVerifier.IsValid(html, this.JSvalidationEnabled);
            Assert.Pass();
        }

        [Test]
        public void RenderIsValid_ForGridWithOneColumn()
        {
            //********** GIVEN
            this._TestGrid.AddColumn(new Column("testColumn1", "150").AsText().Required());
            this._Renderer = new GridRenderer(this._TestGrid);

           //********** WHEN
           var js = this._Renderer.ToJavascripts();

            //********** ASSERT
            StringAssert.Contains("{ name: \"testColumn1\", type: \"text\", width: 150,", js);
            JavascriptVerifier.IsValid(js, this.JSvalidationEnabled);
            Assert.Pass();
        }

        [Test]
        public void RenderIsValid_ForGridWithMultipleColumns()
        {
            //********** GIVEN
            this._TestGrid.AddColumn(new Column("Name", "150").AsText().Required());
            this._TestGrid.AddColumn(new Column("Age", "50").AsNumber());
            this._Renderer = new GridRenderer(this._TestGrid);

            //********** WHEN
            var js = this._Renderer.ToJavascripts();

            //********** ASSERT
            StringAssert.Contains("{ name: \"Name\", type: \"text\", width: 150,", js);
            StringAssert.Contains("{ name: \"Age\", type: \"number\", width: 50,", js);
            JavascriptVerifier.IsValid(js, this.JSvalidationEnabled);
            Assert.Pass();
        }


		[Test]
		public void RenderIsValid_ForGridWithAutoWidth()
		{
			//********** WHEN
			this._TestGrid.WithDimensions("auto", "auto");
			this._Renderer = new GridRenderer(this._TestGrid);

			//********** WHEN
			var js = this._Renderer.ToJavascripts();

			//********** ASSERT
			StringAssert.Contains("autowidth: true,", js);
			JavascriptVerifier.IsValid(js, this.JSvalidationEnabled);
			Assert.Pass();
		}

		[Test]
		public void RenderIsValid_ForGridWithCaption()
		{
			//********** GIVEN
			this._TestGrid.WithCaption("testCaption");
			this._Renderer = new GridRenderer(this._TestGrid);

			//********** WHEN
			var js = this._Renderer.ToJavascripts();
			var html = this._Renderer.ToHtml();

			//********** ASSERT
			StringAssert.Contains("<div class=\"d-flex justify-content-center fst-italic border border-info\">", html);
			JavascriptVerifier.IsValid(html + js, this.JSvalidationEnabled);
			Assert.Pass();
		}


		#region Tests on Examples

		[Test]
        public void RenderIsValid_ForExample_StaticData()
        {
            //********** GIVEN
            this._TestGrid.UseStaticData("[\r\n        { \"Name\": \"Otto Clay\", \"Age\": 25, \"Country\": 1, \"Address\": \"Ap #897-1459 Quam Avenue\", \"Married\": false },\r\n        { \"Name\": \"Connor Johnston\", \"Age\": 45, \"Country\": 2, \"Address\": \"Ap #370-4647 Dis Av.\", \"Married\": true },\r\n        { \"Name\": \"Lacey Hess\", \"Age\": 29, \"Country\": 3, \"Address\": \"Ap #365-8835 Integer St.\", \"Married\": false },\r\n        { \"Name\": \"Timothy Henson\", \"Age\": 56, \"Country\": 1, \"Address\": \"911-5143 Luctus Ave\", \"Married\": true },\r\n        { \"Name\": \"Ramona Benton\", \"Age\": 32, \"Country\": 3, \"Address\": \"Ap #614-689 Vehicula Street\", \"Married\": false }\r\n    ]")
                        .UseReferencedCollection("testGrid_countries", "[\r\n        { Name: \"\", Id: 0 },\r\n        { Name: \"United States\", Id: 1 },\r\n        { Name: \"Canada\", Id: 2 },\r\n        { Name: \"United Kingdom\", Id: 3 }\r\n    ]")
                        .AddColumn(new Column("Name", "150").AsText().Required().Sortable())
                        .AddColumn(new Column("Age", "50").AsNumber().Sortable())
                        .AddColumn(new Column("Address", "200").AsText().Sortable())
                        .AddColumn(new Column("Country", "200").AsSelect("testGrid_countries", "Id", "Name").Sortable())
                        .AddColumn(new Column("Married", "50").AsCheckBox());
            this._Renderer = new GridRenderer(this._TestGrid);

            //********** WHEN
            var js = this._Renderer.ToJavascripts();

            //********** ASSERT
            StringAssert.Contains("var datasource = [", js);
            StringAssert.Contains("var testGrid_countries = [", js);
            StringAssert.Contains("data: datasource,", js);
            StringAssert.Contains("{ name: \"Name\", type: \"text\", width: 150,", js);
            StringAssert.Contains("{ name: \"Age\", type: \"number\", width: 50,", js);
            StringAssert.Contains("{ name: \"Address\", type: \"text\", width: 200,", js);
            StringAssert.Contains("{ name: \"Country\", type: \"select\", width: 200,", js);
            StringAssert.Contains("{ name: \"Married\", type: \"checkbox\", width: 50,", js);
            JavascriptVerifier.IsValid(js, this.JSvalidationEnabled);
            Assert.Pass();
        }

        [Test]
        public void RenderIsValid_ForExample_ControllerData()
        {
            //********** GIVEN
            this._TestGrid.WithDimensions("100%", "auto")
                            .UseController("/api/data")
                            .UseReferencedCollection("testGrid_countries", "[\r\n        { Name: \"\", Id: 0 },\r\n        { Name: \"United States\", Id: 1 },\r\n        { Name: \"Canada\", Id: 2 },\r\n        { Name: \"United Kingdom\", Id: 3 }\r\n    ]")
                               .AddColumn(new Column("Name", "150").AsText().Required().Sortable().AlignOn(Column.ColumnAlignEnum.left))
                               .AddColumn(new Column("Age", "50").AsNumber().Sortable().AlignOn(Column.ColumnAlignEnum.center))
                               .AddColumn(new Column("Address", "200").AsText().Sortable().AlignOn(Column.ColumnAlignEnum.center))
                               .AddColumn(new Column("Country", "200").AsSelect("testGrid_countries", "Id", "Name").Sortable().AlignOn(Column.ColumnAlignEnum.center))
                               .AddColumn(new Column("Married", "50", title: "Is Married").AsCheckBox().AlignOn(Column.ColumnAlignEnum.center))
                               .AddColumn(new Column("control", "150", title: "Actions").AsControl().AlignOn(Column.ColumnAlignEnum.right))
                           .Sortable()
                           .Editable(true, true)
                           .Paginated()
                           .Filterable();
            this._Renderer = new GridRenderer(this._TestGrid);

            //********** WHEN
            var js = this._Renderer.ToJavascripts();

            //********** ASSERT
            StringAssert.DoesNotContain("var datasource = [", js);
            StringAssert.Contains("var testGrid_countries = [", js);
            StringAssert.DoesNotContain("data: datasource,", js);
            StringAssert.Contains("{ name: \"Name\", type: \"text\", width: 150,", js);
            StringAssert.Contains("{ name: \"Age\", type: \"number\", width: 50,", js);
            StringAssert.Contains("{ name: \"Address\", type: \"text\", width: 200,", js);
            StringAssert.Contains("{ name: \"Country\", type: \"select\", width: 200,", js);
            StringAssert.Contains("{ name: \"Married\", type: \"checkbox\", width: 50,", js);
            StringAssert.Contains("filtering: true,", js);
            StringAssert.Contains("inserting: true,", js);
            StringAssert.Contains("editing: true,", js);
            StringAssert.Contains(" sorting: true,", js);
            StringAssert.Contains("paging: true,", js);
            StringAssert.Contains("autoload: true,", js);
            StringAssert.Contains("pageSize: 20,", js);
            StringAssert.Contains("pageButtonCount: 15,", js);
            StringAssert.Contains("deleteConfirm: \"Are you sure to delete current item?\",", js);
            StringAssert.Contains("controller: {", js);
            StringAssert.Contains("loadData: function(filter) {", js);
            StringAssert.Contains("insertItem: function(item) {", js);
            StringAssert.Contains("updateItem: function(item) {", js);
            StringAssert.Contains("deleteItem: function(item) {", js);
            JavascriptVerifier.IsValid(js, this.JSvalidationEnabled);
            Assert.Pass();
        }

        #endregion



    }
}
