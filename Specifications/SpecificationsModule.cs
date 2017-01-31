﻿using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Specifications
{
    public class SpecificationsModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public SpecificationsModule(IRegionManager regionManager, IUnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<Object, Views.SpecificationEditView>(ViewNames.SpecificationsEditView);
            _container.RegisterType<Object, Views.SpecificationMainView>(ViewNames.SpecificationsMainView);

            _regionManager.RegisterViewWithRegion("MainNavigationRegion", typeof(Views.SpecificationNavigationItem));
        }
    }
}