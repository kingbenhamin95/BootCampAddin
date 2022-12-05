#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

#endregion

namespace BootCampAddin
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
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


            FilteredElementCollector modelCurves = new FilteredElementCollector(doc)
                .OfClass(typeof(CurveElement))
                .WhereElementIsNotElementType();

            IList<CurveElement> aGlaz = new List<CurveElement>();
            IList<CurveElement> aWall = new List<CurveElement>();
            IList<CurveElement> mDuct = new List<CurveElement>();
            IList<CurveElement> pPipe = new List<CurveElement>();

            foreach (CurveElement l in modelCurves)
            {
                string name = l.LineStyle.Name;

                switch (name)
                {
                    case "A-GLAZ":
                        aGlaz.Add(l);
                        break;
                    case "A-WALL":
                        aWall.Add(l);
                        break;
                    case "M-DUCT":
                        mDuct.Add(l);
                        break;
                    case "P-PIPE":
                        pPipe.Add(l);
                        break;
                    default:
                        break;
                }
                    
            }

            WallType currentWT = GetWallTypeByName(doc, "Storefront");
            WallType genericWT = GetWallTypeByName(doc, "Generic - 8\"");
            DuctType ductType = GetDuctTypeByName(doc, "Default");
            PipeType pipeType = GetPipeTypeByName(doc, "Default");            

            MEPSystemType ductSystemType = GetMEPSystemTypeByName(doc, "Supply Air");
            MEPSystemType pipeSystemType = GetMEPSystemTypeByName(doc, "Domestic Hot Water");

            ElementId level = Level.GetNearestLevelId(doc, 0);

            Transaction t = new Transaction(doc);
            t.Start("RAB Session04");

            foreach (CurveElement a in aGlaz)
            {
                Curve curve = a.GeometryCurve;
                Wall.Create(doc, curve, currentWT.Id, level, 20, 0, false, false);
            }

            foreach (CurveElement a in aWall)
            {
                Curve curve = a.GeometryCurve;                
                Wall.Create(doc, curve, genericWT.Id, level, 20, 0, false, false);
            }

            foreach (CurveElement a in pPipe)
            {
                Curve curve = a.GeometryCurve;
                XYZ start = curve.GetEndPoint(0);
                XYZ end = curve.GetEndPoint(1);
                
                Duct.Create(doc, ductSystemType.Id, ductType.Id, level, start, end);
            }

            foreach (CurveElement a in mDuct)
            {
                Curve curve = a.GeometryCurve;
                XYZ start = curve.GetEndPoint(0);
                XYZ end = curve.GetEndPoint(1);

                Pipe.Create(doc, pipeSystemType.Id, pipeType.Id, level, start,end);

            }


            t.Commit();
            t.Dispose();

            return Result.Succeeded;


        }
        private WallType GetWallTypeByName(Document doc, string wallType)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(WallType));            

            foreach (WallType currentWT in collector)
            {
                if (currentWT.Name == wallType)
                    return currentWT;
            }

            return null;

        }
        private PipeType GetPipeTypeByName(Document doc, string pipeType)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(PipeType));
            //collector.OfCategory(BuiltInCategory.OST_Walls).WhereElementIsElementType();

            foreach (PipeType currentPT in collector)
            {
                if (currentPT.Name == pipeType)
                    return currentPT;
            }

            return null;

        }
        private DuctType GetDuctTypeByName(Document doc, string ductType)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(DuctType));
            //collector.OfCategory(BuiltInCategory.OST_Walls).WhereElementIsElementType();

            foreach (DuctType currentPT in collector)
            {
                if (currentPT.Name == ductType)
                    return currentPT;
            }

            return null;

        }
        private MEPSystemType GetMEPSystemTypeByName(Document doc, string wallType)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(MEPSystemType));
            //collector.OfCategory(BuiltInCategory.OST_Walls).WhereElementIsElementType();

            foreach (MEPSystemType currentST in collector)
            {
                if (currentST.Name == wallType)
                    return currentST;
            }

            return null;

        }

    }
}
