using Autodesk.Revit.UI;
using System.Windows;

namespace RevitAPI_SetPipeWallThickness
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {

        public MainView(ExternalCommandData commandData)
        {
            InitializeComponent();
            MainViewViewModel vm = new MainViewViewModel(commandData);
            vm.HideRequest += (s, e) => this.Hide();
            vm.ShowRequest += (s, e) => this.Show();
            vm.CloseRequest += (s, e) => this.Close();
            DataContext = vm;
        }
    }
}
