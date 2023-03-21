using Autofac;
using CardGames.War;
using CardGames.War.Configuration;
using Prism.Events;
using CardGames.UI.Commands;
using CardGames.UI.Data;
using CardGames.UI.Factories;
using CardGames.UI.ViewModels;
using CardGames.UI.ViewModels.War;

namespace CardGames.UI.Startup
{
  public static class ContainerConfig
  {
    public static IContainer Configure()
    {
      ContainerBuilder builder = new();

      _ = builder.RegisterType<MainWindow>().AsSelf();
      _ = builder.RegisterType<MainViewModel>().As<IMainViewModel>();
      _ = builder.RegisterType<WarMainViewModel>().As<IWarMainViewModel>();

      _ = builder.RegisterType<StartPageViewModel>().As<IViewModelBaseWrapper>();
      _ = builder.RegisterType<UpdateViewCommand>().As<IUpdateViewCommandWrapper>();
      _ = builder.RegisterType<CardGameFactory>().As<ICardGameFactory>();
      _ = builder.RegisterType<WarHandViewModelFactory>().As<IWarHandViewModelFactory>();
      _ = builder.RegisterType<WarCardsOnTableViewModelFactory>().As<IWarCardsOnTableViewModelFactory>();
      _ = builder.RegisterType<WarPileOfWarCardsViewModelFactory>().As<IWarPileOfWarCardsViewModelFactory>();
      _ = builder.RegisterType<CardGameWar>().As<ICardGameWar>().SingleInstance();
      _ = builder.RegisterType<GameConfigurationWar>().As<IGameConfigurationWar>();
      _ = builder.RegisterType<UserDataService>().As<IUserDataService>();
      _ = builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

      return builder.Build();
    }
  }
}
