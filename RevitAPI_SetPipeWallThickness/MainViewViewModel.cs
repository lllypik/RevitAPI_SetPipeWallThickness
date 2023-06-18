using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Prism.Commands;
using RevitAPI2023Library;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RevitAPI_SetPipeWallThickness
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public List<PipeType> PipeTypes { get; } = new List<PipeType>();
        public PipeType SelectedPipeType { get; set; }

        public DelegateCommand ApplyCommand { get; set; }

        //Список объектов класса со свойствами DiameterDN, VolumesThicknessWall, SuggestedVolumesThicknessWall
        public List<MyPipes> Pipes { get; set; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;

            //Получаем список типов труб
            PipeTypes = SelectionUtils.GetPipeTypes(_commandData);

            //Делегат для обработчика нажатия кнопки "Применить"
            ApplyCommand = new DelegateCommand(OnApplyCommand);

            //Создаем базовый список с соотношением диаметр трубы/толщина стенки
            Pipes = CreateListPipes();
        }

        private void OnApplyCommand()
        {
            //Проверка на предмет выбора типа труб
            if (SelectedPipeType == null)
            {
                //RaiseHideRequest();
                TaskDialog.Show("Ошибка", "Не выбран тип трубопровода");
                //RaiseShowRequest();
                RaiseCloseRequest();
            }

            else
            {
                Document doc = _commandData.Application.ActiveUIDocument.Document;

                //Заполняем параметры через словарь значений диаметр трубы / толщина стенки
                Dictionary<string, string> diametrOfPipe = new Dictionary<string, string>();

                foreach (var p in Pipes)
                {
                    if (p.SuggestedVolumesThicknessWall == null || p.SuggestedVolumesThicknessWall == "")
                    {
                        diametrOfPipe.Add(p.DiameterDN, p.VolumesThicknessWall);
                    }

                    else
                    {
                        diametrOfPipe.Add(p.DiameterDN, p.SuggestedVolumesThicknessWall);
                        p.SuggestedVolumesThicknessWall = null;
                    }
                }

                //Получаем список труб выбранного типа        
                List<Pipe> pipes = SelectionUtils.GetPipesOfType(doc, SelectedPipeType.Name);

                //Транзакция для присвоения значения толщины стенки в параметр каждой трубы. Толщина стенки берется из словаря
                using (Transaction transaction = new Transaction(doc, "Set Thicknes wall of pipes"))
                {
                    transaction.Start();

                    string diameterDN;
                    string thicknessWall;

                    foreach (var pipe in pipes)
                    {
                        Parameter diametrDNpar = pipe.LookupParameter("Диаметр");
                        diameterDN = diametrDNpar.AsValueString();

                        if (diametrOfPipe.ContainsKey(diameterDN) == true)
                        {
                            thicknessWall = diametrOfPipe[diameterDN];
                            Parameter thicknessParameter = pipe.LookupParameter("MEP_t_стенки");
                            thicknessParameter.Set(diametrOfPipe[diameterDN]);
                        }

                        else
                        {
                            Parameter thicknessParameter = pipe.LookupParameter("MEP_t_стенки");
                            thicknessParameter.Set("000");
                        }
                    }

                    transaction.Commit();
                }

                TaskDialog.Show("Оповещение", "Операция выполнена");
 
                RaiseCloseRequest();
            }
        }

        public event EventHandler HideRequest;
        public void RaiseHideRequest()
        {
            HideRequest?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler ShowRequest;
        public void RaiseShowRequest()
        {
            ShowRequest?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CloseRequest;
        public void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }

        //Метод создания словаря (диаметры можно подгружать из Excell, но для данного приложения хватит и жестких значений)
        private List<MyPipes> CreateListPipes()
        {
            Pipes = new List<MyPipes>();
            Pipes.Add(new MyPipes() { DiameterDN = "10", VolumesThicknessWall = "2.5" });
            Pipes.Add(new MyPipes() { DiameterDN = "15", VolumesThicknessWall = "2.5" });
            Pipes.Add(new MyPipes() { DiameterDN = "20", VolumesThicknessWall = "2.5" });
            Pipes.Add(new MyPipes() { DiameterDN = "25", VolumesThicknessWall = "2.8" });
            Pipes.Add(new MyPipes() { DiameterDN = "32", VolumesThicknessWall = "2.8" });
            Pipes.Add(new MyPipes() { DiameterDN = "40", VolumesThicknessWall = "3.2" });
            Pipes.Add(new MyPipes() { DiameterDN = "50", VolumesThicknessWall = "3.5" });
            Pipes.Add(new MyPipes() { DiameterDN = "65", VolumesThicknessWall = "3.5" });
            Pipes.Add(new MyPipes() { DiameterDN = "80", VolumesThicknessWall = "3.5" });
            Pipes.Add(new MyPipes() { DiameterDN = "100", VolumesThicknessWall = "4.0" });
            Pipes.Add(new MyPipes() { DiameterDN = "125", VolumesThicknessWall = "4.0" });
            Pipes.Add(new MyPipes() { DiameterDN = "150", VolumesThicknessWall = "4.5" });
            Pipes.Add(new MyPipes() { DiameterDN = "200", VolumesThicknessWall = "4.5" });
            Pipes.Add(new MyPipes() { DiameterDN = "250", VolumesThicknessWall = "4.5" });
            return Pipes;
        }
    }

}
