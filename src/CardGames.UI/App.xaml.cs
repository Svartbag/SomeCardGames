using Autofac;
using System.Windows;
using CardGames.UI.Startup;

namespace CardGames.UI
{
  public partial class App : Application
  {
    private void Application_Startup(object sender, StartupEventArgs e)
    {
      //var mainWindow = new MainWindow(
      //  new MainViewModel(
      //    new UserDataService()));
      IContainer container = ContainerConfig.Configure();

      MainWindow mainWindow = container.Resolve<MainWindow>();

      mainWindow.Show();
    }
  }
}
