#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Windows.Forms;
using System.CodeDom;
using Application = Autodesk.Revit.ApplicationServices.Application;
using System.Linq;

#endregion

namespace BootCampAddin
{
    [Transaction(TransactionMode.Manual)]
    public class CmdProjectSetup : IExternalCommand
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

            System.Windows.Forms.OpenFileDialog openDlg = new System.Windows.Forms.OpenFileDialog();

            openDlg.Title = "Select a file";
            openDlg.Filter = "csv (*.csv)|*csv|Excel Files (*.xlsx)|*xlsx|Text Files (*txt)|*.txt|All Files (*.*)|*.*";
            openDlg.RestoreDirectory = true;

            System.Windows.Forms.DialogResult result = openDlg.ShowDialog();

            string path = openDlg.FileName.ToString();


            

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string[] fileArray = System.IO.File.ReadAllLines(path);
                List<string> file = fileArray.ToList();
                file.RemoveAt(0);

                //FilteredElementCollector collector = new FilteredElementCollector(doc)
                //    .OfCategory(BuiltInCategory.OST_TitleBlocks);

                // ElementId tBlockId = collector.FirstElementId();
                Transaction t = new Transaction(doc);
                t.Start("Import Excel Data");

                foreach (string rowstring in file)
                {

                    string[] cellString = rowstring.Split(',');
                    string levelName = cellString[0];
                    string levelNumber = cellString[1];

                    

                    Level newLevel = Level.Create(doc, double.Parse(levelNumber));
                    newLevel.Name = levelName;


                    
                }
                t.Commit();
                t.Dispose();

            }
            else
            {
                TaskDialog.Show("Failed", "Incorrect File Selection");
            }



            


            

            return Result.Succeeded;
        }
    }
}
