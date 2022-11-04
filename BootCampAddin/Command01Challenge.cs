#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

#endregion

namespace BootCampAddin
{
    [Transaction(TransactionMode.Manual)]
    public class Command01Challenge : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

         
            XYZ myPoint = new XYZ(0,0,0);
            string fizz = "FIZZ";
            string buzz = "BUZZ";

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(TextNoteType));

            Transaction t = new Transaction(doc);
            t.Start("Create text note");

            XYZ offset = new XYZ(0, 5, 0);
            XYZ newPoint = myPoint;

            for (int i =0; i<=100; i++)
            {
                newPoint = newPoint.Add(offset);

                if ((i % 5) == 0 && (i % 3) == 0)
                {
                    TextNote t1 = TextNote.Create(doc, doc.ActiveView.Id, newPoint, fizz + buzz, collector.FirstElementId());
                }
                else if ((i % 3) == 0)
                {
                    TextNote t1 = TextNote.Create(doc, doc.ActiveView.Id, newPoint, fizz, collector.FirstElementId());
                }
                else if ((i % 5) == 0)
                {
                    TextNote t1 = TextNote.Create(doc, doc.ActiveView.Id, newPoint, buzz, collector.FirstElementId());
                }
                else
                {
                    TextNote t1 = TextNote.Create(doc, doc.ActiveView.Id, newPoint, i.ToString(), collector.FirstElementId());
                }
            }

            t.Commit();

            return Result.Succeeded;
        }
    }
}
