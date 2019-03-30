using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for GlobalFunctions
/// </summary>
public static class GlobalFunctions
{
    public static void AddCssClass(WebControl control, string classname)
    {
        if (string.IsNullOrWhiteSpace(classname)) return;

        // Add a class
        control.CssClass = string.Join(" ", control
               .CssClass
               .Split(' ')
               .Except(new string[] { "", classname })
               .Concat(new string[] { classname })
               .ToArray()
        );
    }

    public static void RemoveCssClass(WebControl control, string classname)
    {
        if (string.IsNullOrWhiteSpace(classname)) return;

        // Remove a class
        control.CssClass = string.Join(" ", control
               .CssClass
               .Split(' ')
               .Except(new string[] { "", classname })
               .ToArray()
       );
    }
}