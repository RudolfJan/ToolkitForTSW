using Caliburn.Micro;
using Logging.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Logging.Library.Wpf.Views;
using ToolkitForTSW.ViewModels;
using System.Reflection;
using System.IO;
using ToolkitForTSW.DialogServices;
using Logging.Library.Wpf.ViewModels;
using Utilities.Library.Wpf.ViewModels;
using Utilities.Library.Wpf.Models;
using System.ComponentModel;
using TreeBuilders.Library.Wpf.ViewModels;
using System.Diagnostics;

namespace ToolkitForTSW
  {
  public class Bootstrapper : BootstrapperBase
    {
    private readonly SimpleContainer _container = new SimpleContainer();

    public Bootstrapper()
      {
      Initialize();
      StarDebugLogger();
      }

    [Conditional("DEBUG")]
    public static void StarDebugLogger()
      {
      LogManager.GetLog = type => new DebugLogger(type);
      }

    protected override void Configure()
      {
      // This makes the container return a SimpleContainer if you need one
      _container.Instance(_container);

      // https://stackoverflow.com/questions/11058507/caliburn-micro-view-viewmodel-name-resolution-issue
   
      // Instantiate some singletons
      _container
        .Singleton<IWindowManager, WindowManager>()
        .Singleton<IDialogService, DialogService>()
        .Singleton<IEventAggregator, EventAggregator>()
        .Singleton<IAboutModel,AboutModel>();

      foreach(var assembly in SelectAssemblies())
        {
        assembly.GetTypes()
          .Where(type => type.IsClass)
          .Where(type => type.Name.EndsWith("ViewModel"))
          .ToList()
          .ForEach(viewModelType => _container.RegisterPerRequest(viewModelType, viewModelType.ToString(), viewModelType));
        }
      }

     protected override IEnumerable<Assembly> SelectAssemblies()
      {
      // https://www.jerriepelser.com/blog/split-views-and-viewmodels-in-caliburn-micro/

      var assemblies = base.SelectAssemblies().ToList();
      assemblies.Add(typeof(LoggingViewModel).GetTypeInfo().Assembly);
      assemblies.Add(typeof(AboutViewModel).GetTypeInfo().Assembly);
      assemblies.Add(typeof(FileTreeViewModel).GetTypeInfo().Assembly);
      return assemblies;
      }

    protected override async void OnStartup(object sender, StartupEventArgs e)
      {
      await DisplayRootViewForAsync(typeof(ShellViewModel));
      }
  
    protected override object GetInstance(Type service, string key)
      {
      return _container.GetInstance(service, key);
      }

    protected override IEnumerable<object> GetAllInstances(Type service)
      {
      return _container.GetAllInstances(service);
      }

    protected override void BuildUp(object instance)
      {
      _container.BuildUp(instance);
      }
    }
  }
  
