using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for WarningList
/// </summary>
public class Warning
{

    public List<string> WarnControls;
    public string Text;

    public Warning()
    {
        WarnControls = new List<string>();
        Text = "";
    }

    public Warning(List<string> warnContorls, string text)
    {
        WarnControls = warnContorls;
        Text = text;
    }

    public Warning(string warnContorl, string text)
    {
        WarnControls = new List<string>();
        WarnControls.Add(warnContorl);
        Text = text;
    }



}