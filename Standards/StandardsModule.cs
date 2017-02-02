﻿using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Standards
{
    public class StandardsModule : IModule
    {
        IUnityContainer _container;
        IRegionManager _regionManager;

        public StandardsModule(IRegionManager regionManager, IUnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<Object, Views.StandardMainView>
                (StandardsViewNames.StandardMainView);

            _regionManager.RegisterViewWithRegion
                (Navigation.RegionNames.MainNavigationRegion, typeof(Views.StandardNavigationItem));
        }
    }
}