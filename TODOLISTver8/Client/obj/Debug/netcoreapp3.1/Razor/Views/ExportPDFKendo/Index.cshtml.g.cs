#pragma checksum "D:\Visual Studio 2019\JWT\TodoList-ver-8\TODOLISTver8\Client\Views\ExportPDFKendo\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6c6f97ba6f8f6d10ed03af6920f326fa35d4a8fd"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_ExportPDFKendo_Index), @"mvc.1.0.view", @"/Views/ExportPDFKendo/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\Visual Studio 2019\JWT\TodoList-ver-8\TODOLISTver8\Client\Views\_ViewImports.cshtml"
using Client;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Visual Studio 2019\JWT\TodoList-ver-8\TODOLISTver8\Client\Views\_ViewImports.cshtml"
using Client.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6c6f97ba6f8f6d10ed03af6920f326fa35d4a8fd", @"/Views/ExportPDFKendo/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3332004e6f18ccbec22253d7e177fe1fd5f40969", @"/Views/_ViewImports.cshtml")]
    public class Views_ExportPDFKendo_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "D:\Visual Studio 2019\JWT\TodoList-ver-8\TODOLISTver8\Client\Views\ExportPDFKendo\Index.cshtml"
  
    ViewData["Title"] = "Index";
    Layout = "~/Views/Layout/Layout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("<button id=\"export\">Export to PDF</button>\r\n<div id=\"grid\"></div>\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral(@"
    <link rel=""stylesheet"" href=""https://kendo.cdn.telerik.com/2017.2.621/styles/kendo.common-material.min.css"" />
    <link rel=""stylesheet"" href=""https://kendo.cdn.telerik.com/2017.2.621/styles/kendo.material.min.css"" />
    <link rel=""stylesheet"" href=""https://kendo.cdn.telerik.com/2017.2.621/styles/kendo.material.mobile.min.css"" />

    <script src=""https://kendo.cdn.telerik.com/2017.2.621/js/jquery.min.js""></script>
    <script src=""https://kendo.cdn.telerik.com/2017.2.621/js/angular.min.js""></script>
    <script src=""https://kendo.cdn.telerik.com/2017.2.621/js/kendo.all.min.js""></script>

");
                WriteLiteral("    <script>\r\n        $(\"#grid\").kendoGrid({\r\n            toolbar: [\"pdf\"],\r\n            pdf: {\r\n                forceProxy: true,\r\n                //proxyURL: \"/proxy\"\r\n                //proxyURL: \"/ExportPDFKendo/save\"\r\n                proxyURL: \"");
#nullable restore
#line 29 "D:\Visual Studio 2019\JWT\TodoList-ver-8\TODOLISTver8\Client\Views\ExportPDFKendo\Index.cshtml"
                      Write(Url.Action("Save", "Users"));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"""
            },
            dataSource: {
                type: ""odata"",
                transport: {
                    read: ""https://demos.telerik.com/kendo-ui/service/Northwind.svc/Products""
                },
                pageSize: 7
            },
            pageable: true,
            columns: [
                { width: 300, field: ""ProductName"", title: ""Product Name"" },
                { field: ""UnitsOnOrder"", title: ""Units On Order"" },
                { field: ""UnitsInStock"", title: ""Units In Stock"" }
            ]
        });
        $(""#export"").click(function(e) {
    var grid = $(""#grid"").data(""kendoGrid"");
    grid.saveAsPDF();
});
    </script>
");
            }
            );
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
