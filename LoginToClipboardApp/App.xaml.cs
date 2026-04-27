using System.Configuration;
using System.Data;
using System.Windows;

namespace LoginClipboardApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Window? view;
        private BaseViewModel? viewModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            viewModel = new AppViewModel() ;
            view = new MainWindow();
            view.DataContext = viewModel;
            view.ShowDialog();
        }
    }
}
