using jsGrid2Mvc.Model;
using Microsoft.AspNetCore.Html;

namespace jsGrid2Mvc.Samples.Models.jsGridSamples
{
    public class StaticDataModel
    {
        public Grid BuildGrid()
        {
            var grid = new Grid("testGrid", "")
                           .SetReadOnlyWithSerializedData("[\r\n        { \"Name\": \"Otto Clay\", \"Age\": 25, \"Country\": 1, \"Address\": \"Ap #897-1459 Quam Avenue\", \"Married\": false },\r\n        { \"Name\": \"Connor Johnston\", \"Age\": 45, \"Country\": 2, \"Address\": \"Ap #370-4647 Dis Av.\", \"Married\": true },\r\n        { \"Name\": \"Lacey Hess\", \"Age\": 29, \"Country\": 3, \"Address\": \"Ap #365-8835 Integer St.\", \"Married\": false },\r\n        { \"Name\": \"Timothy Henson\", \"Age\": 56, \"Country\": 1, \"Address\": \"911-5143 Luctus Ave\", \"Married\": true },\r\n        { \"Name\": \"Ramona Benton\", \"Age\": 32, \"Country\": 3, \"Address\": \"Ap #614-689 Vehicula Street\", \"Married\": false }\r\n    ]")
                           .UseReferencedCollection("testGrid_countries", "[\r\n        { Name: \"\", Id: 0 },\r\n        { Name: \"United States\", Id: 1 },\r\n        { Name: \"Canada\", Id: 2 },\r\n        { Name: \"United Kingdom\", Id: 3 }\r\n    ]")
                               .AddColumn(new Column("Name", "150").AsText().Required().Sortable())
                               .AddColumn(new Column("Age", "50").AsNumber().Sortable().NotSearchable())
                               .AddColumn(new Column("Address", "200").AsText().Sortable())
                               .AddColumn(new Column("Country", "200").AsSelect("testGrid_countries", "Id", "Name").Sortable())
                               .AddColumn(new Column("Married", "50", title: "Is Married").AsCheckBox())
                               .AddColumn(new Column("control", "150", title: "Actions").AsControl());
            return grid;
        }

        public MvcGrid Grid { get { return new MvcGrid(this.BuildGrid());    } }
    }
}
