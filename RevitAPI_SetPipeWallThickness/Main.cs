using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitAPI_SetPipeWallThickness
{
    [Transaction(TransactionMode.Manual)]

    public class Main : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            MainView window = new MainView(commandData);
            window.ShowDialog();
            return Result.Succeeded;
        }
    }
}
