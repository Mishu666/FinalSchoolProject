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
    public static string FormatNumber(int number)
    {
        double final = number;
        if (final < 1000 && final > -1000) return number.ToString();
        if (final < 1000000 && final > -1000000)
        {
            return Math.Round((final / 1000), 1).ToString() + " K";
        }
        if (final < 1000000000 && final > -1000000000)
        {
            return Math.Round((final / 1000000), 1).ToString() + " M";
        }
        if (final < 1000000000000 && final > -1000000000000)
        {
            return Math.Round((final / 1000000000), 1).ToString() + " B";
        }
        else return "a heckuva lot";
    }

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

    public static void AddCssClass(HtmlControl control, string classname)
    {
        if (string.IsNullOrWhiteSpace(classname)) return;

        // Add a class
        control.Attributes.Add("class", String.Join(" ", control
                   .Attributes["class"]
                   .Split(' ')
                   .Except(new string[] { "", classname })
                   .Concat(new string[] { classname })
                   .ToArray()
           ));
        
    }

    public static void RemoveCssClass(HtmlControl control, string classname)
    {
        if (string.IsNullOrWhiteSpace(classname)) return;

        // Remove a class
        control.Attributes.Add("class", String.Join(" ", control
                  .Attributes["class"]
                  .Split(' ')
                  .Except(new string[] { "", classname })
                  .ToArray()
          ));
    }

}