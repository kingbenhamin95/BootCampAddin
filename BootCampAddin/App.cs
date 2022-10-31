#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

#endregion

namespace BootCampAddin
{
    internal class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication a)
        {
            TaskDialog.Show("Test", "Myaddin is starting up!");

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            TaskDialog.Show("Test", "Myaddin is closing!");
            return Result.Succeeded;
        }
    }
}
