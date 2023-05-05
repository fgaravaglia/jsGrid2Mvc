# jsGrid2Mvc
jsGrid2Mvc is a library that provides to you a MVC wrapper for jsGrid, in order to generate html and Js from a model in a easy and consistent way.
The key component of the library are:
- Grid.cs: class tha contains the Grid model itself, that translate in Object-Oriented language the fileds and features provided by jsGrid library
- MvcGrid.cs: class that simplify the geenration of Javascript code on page
- GridExtensions.cs: class to extend and manage the model in a user friendly mode

you can install the library from nuget using the command:

```bat
dotnet add package jsGrid2Mvc
```

## Status

[![Build Status](https://garaproject.visualstudio.com/UmbrellaFramework/_apis/build/status%2Ffgaravaglia.jsGrid2Mvc?repoName=fgaravaglia%2FjsGrid2Mvc&branchName=main)](https://garaproject.visualstudio.com/UmbrellaFramework/_build/latest?definitionId=91&repoName=fgaravaglia%2FjsGrid2Mvc&branchName=main)

[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=jsGrid2Mvc&metric=bugs)](https://sonarcloud.io/summary/new_code?id=jsGrid2Mvc)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=jsGrid2Mvc&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=jsGrid2Mvc)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=jsGrid2Mvc&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=jsGrid2Mvc)


## Requirements
To use jsGrid2Mvc you have to install in you web application:
- Bootstap > 5.2: see XXXXXX
- jquert > : see XXXXX
- jsGrid: see XXXXX


# How to Configure
First of all you have to set the default configuration of the grid in your application. Add to _appSettings.json_ file this section:

```json
{
"UI": {
    "Grid": {
      "Width": "100%",
      "Height": "auto",
      "FilteringEnabled": true,
      "SearchEnabled": true,
      "SortingEnabled": true,
      "PagingEnabled": true,
      "Pagination": {
        "PageIndex": 1,
        "PageSize": 20,
        "ButtonCount": 15,
        "Pages": "{first} {prev} {pages} {next} {last}    {pageIndex} of {pageCount}",
        "PrevText": "Prev",
        "NextText": "Next",
        "FirstText": "First",
        "LastText": "Last",
        "NavigatorNextText": "...",
        "NavigatorPrevText": "..."
      },
      "ConfirmOnDelete": true,
      "ConfirmOnDeleteMessage": "Are you sure to delete current item?"
    }
  }
}
```

for the options, you can see GridSettings.cs file or see the Samples application.
Then, on your MVC model, you can create the grid:

```c#
var grid = new Grid("testGrid", "Grid to show how to interact with controller").FromConfiguration(config);
```

and you can customize it as you need:

```c#
grid.SetReadOnlyWithSerializedData("[\r\n        { \"Name\": \"Otto Clay\", \"Age\": 25, \"Country\": 1, \"Address\": \"Ap #897-1459 Quam Avenue\", \"Married\": false },\r\n        { \"Name\": \"Connor Johnston\", \"Age\": 45, \"Country\": 2, \"Address\": \"Ap #370-4647 Dis Av.\", \"Married\": true },\r\n        { \"Name\": \"Lacey Hess\", \"Age\": 29, \"Country\": 3, \"Address\": \"Ap #365-8835 Integer St.\", \"Married\": false },\r\n        { \"Name\": \"Timothy Henson\", \"Age\": 56, \"Country\": 1, \"Address\": \"911-5143 Luctus Ave\", \"Married\": true },\r\n        { \"Name\": \"Ramona Benton\", \"Age\": 32, \"Country\": 3, \"Address\": \"Ap #614-689 Vehicula Street\", \"Married\": false }\r\n    ]")
    .UseReferencedCollection("testGrid_countries", "[\r\n        { Name: \"\", Id: 0 },\r\n        { Name: \"United States\", Id: 1 },\r\n        { Name: \"Canada\", Id: 2 },\r\n        { Name: \"United Kingdom\", Id: 3 }\r\n    ]")
        .AddColumn(new Column("Name", "150").AsText().Required().Sortable())
        .AddColumn(new Column("Age", "50").AsNumber().Sortable().NotSearchable())
        .AddColumn(new Column("Address", "200").AsText().Sortable())
        .AddColumn(new Column("Country", "200").AsSelect("testGrid_countries", "Id", "Name").Sortable())
        .AddColumn(new Column("Married", "50", title: "Is Married").AsCheckBox())
        .AddColumn(new Column("control", "150", title: "Actions").AsControl());
```

# How to injects required component into DI
to register services and components to use this library, you have to use needed extensions method:

```c#
builder.Services.AddCommonGridViewSettings(builder.Configuration);
```