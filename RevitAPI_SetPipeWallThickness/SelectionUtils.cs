using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;


namespace RevitAPI2023Library
{
    public static class SelectionUtils
    {

        public static List<Element> PickPipes(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedObjects = uidoc.Selection.PickObjects(ObjectType.Element, "Выбери элемент");
            List<Element> elementList = selectedObjects.Select(selectedObject => doc.GetElement(selectedObject)).ToList();
            return elementList;
        }

        public static List<PipeType> GetPipeTypes(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<PipeType> pipeTypes = new FilteredElementCollector(doc)
                                            .OfClass(typeof(PipeType))
                                            .Cast<PipeType>()
                                            .ToList();

            return pipeTypes;
        }

        public static List<Pipe> GetAllPipes(Document doc)
        {
            List<Pipe> pipes = new FilteredElementCollector(doc)
                                            .OfClass(typeof(Pipe))
                                            .Cast<Pipe>()
                                            .ToList();
            return pipes;
        }

        public static List<Pipe> GetPipesOfType(Document doc, string nameSelectedPipeType)
        {

            List<Pipe> pipes = new FilteredElementCollector(doc)
                                .OfClass(typeof(Pipe))
                                .Cast<Pipe>()
                                .Where(x => x.Name == nameSelectedPipeType)
                                .ToList();

            return pipes;
        }


    }
}
