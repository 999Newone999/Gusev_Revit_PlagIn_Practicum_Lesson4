using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gusev_Revit_PlagIn_Practicum_Lesson4
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class CreationModel : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            List<String> levelNames = new List<String>();
            levelNames.Add("Уровень 1");
            levelNames.Add("Уровень 2");

            List<Level> levels = GetLevels(doc, levelNames);

            CreateWalls(doc, 10000, 5000, 0, 0, levels.ElementAt(0), levels.ElementAt(1));
            /*
            double width = UnitUtils.ConvertToInternalUnits(10000, UnitTypeId.Millimeters);
            double depth = UnitUtils.ConvertToInternalUnits(5000, UnitTypeId.Millimeters);
            double dx = width / 2;
            double dy = depth / 2;

            List<XYZ> points = new List<XYZ>();
            points.Add(new XYZ(-dx, -dy, 0));
            points.Add(new XYZ(dx, -dy, 0));
            points.Add(new XYZ(dx, dy, 0));
            points.Add(new XYZ(-dx, dy, 0));
            points.Add(new XYZ(-dx, -dy, 0));

            List<Wall> walls = new List<Wall>();

            Transaction transaction = new Transaction(doc, "Построение стен");
            transaction.Start();
            for (int i = 0; i < 4; i++)
            {
                Line line = Line.CreateBound(points[i], points[i + 1]);
                Wall wall = Wall.Create(doc, line, levels.ElementAt(0).Id, false);
                wall.get_Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).Set(levels.ElementAt(1).Id);
                walls.Add(wall);
            }

            transaction.Commit();
            */

            /* var res1 = new FilteredElementCollector(doc)
                            .OfClass(typeof(Wall))
                            //.Cast<Wall>()
                            .OfType<Wall>()
                            .ToList();
             var res2 = new FilteredElementCollector(doc)
                            .OfClass(typeof(WallType))
                            //.Cast<Wall>()
                            .OfType<WallType>()
                            .ToList();
             var res3 = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilyInstance))
                //.Cast<Wall>()
                .OfType<FamilyInstance>()
                .ToList();

             var res4 = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilyInstance))
                .OfCategory(BuiltInCategory.OST_Doors)
                //.Cast<Wall>()
                .OfType<FamilyInstance>()
                .Where(x=>x.Name.Equals("36\" x 84\""))
                .ToList();

             var res5 = new FilteredElementCollector(doc)
                             .WhereElementIsNotElementType()
                             .ToList();
            */


            return Result.Succeeded;
        }

        // Метод получающий существующие уровни из документа, имена которых перечислены в входном списке
 
        public List<Level> GetLevels(Document doc, List<String> levelNames)
        {
            List<Level> listNamedlevel = new List<Level>();
            List<Level> listlevel = new FilteredElementCollector(doc)
                            .OfClass(typeof(Level))
                            .OfType<Level>()
                            .ToList();
            foreach (String levelName in levelNames)
            {
                try
                {
                    listNamedlevel.Add(listlevel.FirstOrDefault(x => x.Name.Equals(levelName)));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return listNamedlevel;
        }

        public List<Wall> CreateWalls(Document doc, double _width, double _depth, double x, double y,
                                    Level baseLevel, Level upperLevel)
        {
            double width = UnitUtils.ConvertToInternalUnits(_width, UnitTypeId.Millimeters);
            double depth = UnitUtils.ConvertToInternalUnits(_depth, UnitTypeId.Millimeters);
            double dx = width / 2;
            double dy = depth / 2;

            List<XYZ> points = new List<XYZ>();
            points.Add(new XYZ(x-dx, y-dy, 0));
            points.Add(new XYZ(x+dx, y-dy, 0));
            points.Add(new XYZ(x+dx, y+dy, 0));
            points.Add(new XYZ(x-dx, y+dy, 0));
            points.Add(new XYZ(x-dx, y-dy, 0));

            List<Wall> walls = new List<Wall>();

            Transaction transaction = new Transaction(doc, "Построение стен");
            transaction.Start();
            for (int i = 0; i < 4; i++)
            {
                Line line = Line.CreateBound(points[i], points[i + 1]);
                Wall wall = Wall.Create(doc, line, baseLevel.Id, false);
                wall.get_Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).Set(upperLevel.Id);
                walls.Add(wall);
            }

            transaction.Commit();

            return walls;
        }
    }
}
