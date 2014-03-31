using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using Ninject;

namespace YoutrackBoard
{
    class AppBootstrapper : Caliburn.Micro.BootstrapperBase
    {
        private IKernel _kernel;

        public AppBootstrapper() : base(true)
        {
            Start();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            
            DisplayRootViewFor<IShell>();
        }

        protected override void Configure()
        {
            base.Configure();
            _kernel = CreateContainer();
        }

        private IKernel CreateContainer()
        {
            Castle.Core.ProxyServices.IsDynamicProxy(typeof(AppBootstrapper));
            var kernel =  new StandardKernel();

            kernel.Bind<IShell>().To<ShellViewModel>().InSingletonScope(); ;
            kernel.Bind<IWindowManager>().To<WindowManager>();
            kernel.Bind<YouTrackClientFactory>().ToSelf().InSingletonScope();
            return kernel;
        }

        protected override void BuildUp(object instance)
        {
            _kernel.Inject(instance);
        }
        
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _kernel.GetAll(service);
        }

        protected override object GetInstance(Type service, string key)
        {
            if (string.IsNullOrEmpty(key))
                _kernel.Get(service);
            return _kernel.Get(service, key);
        }
    }
}