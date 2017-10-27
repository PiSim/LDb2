﻿using Microsoft.Practices.Unity;
using Infrastructure;
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
            DBPrincipal principal = _container.Resolve<DBPrincipal>();

            _container.RegisterType<SpecificationServiceProvider>(new ContainerControlledLifetimeManager());
            _container.Resolve<SpecificationServiceProvider>();

            _container.RegisterType<Object, Views.MethodEdit>(SpecificationViewNames.MethodEdit);
            _container.RegisterType<Object, Views.MethodMain>(SpecificationViewNames.MethodMain);
            _container.RegisterType<Object, Views.SpecificationEdit>(SpecificationViewNames.SpecificationEdit);
            _container.RegisterType<Object, Views.SpecificationMain>(SpecificationViewNames.SpecificationMain);
            _container.RegisterType<Object, Views.SpecificationTestListEdit>(SpecificationViewNames.SpecificationTestListEdit);
            _container.RegisterType<Object, Views.SpecificationVersionEdit>(SpecificationViewNames.SpecificationVersionEdit);
            _container.RegisterType<Object, Views.SpecificationVersionList>(SpecificationViewNames.SpecificationVersionList);
            _container.RegisterType<Object, Views.StandardIssueEdit>(SpecificationViewNames.StandardIssueEdit);

            _container.RegisterType<Views.MethodCreationDialog>();
            _container.RegisterType<Views.SpecificationCreationDialog>();

            _container.RegisterType<ViewModels.MethodCreationDialogViewModel>();
            _container.RegisterType<ViewModels.MethodEditViewModel>();
            _container.RegisterType<ViewModels.MethodMainViewModel>();
            _container.RegisterType<ViewModels.SpecificationCreationDialogViewModel>();
            _container.RegisterType<ViewModels.SpecificationMainViewModel>();
            _container.RegisterType<ViewModels.SpecificationEditViewModel>();
            _container.RegisterType<ViewModels.SpecificationVersionEditViewModel>();
            _container.RegisterType<ViewModels.StandardIssueEditViewModel>();

            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, 
                                                typeof(Views.MethodNavigationItem));


            // ADD Specification tag to navigation Menu if user is authorized

            if (principal.IsInRole(UserRoleNames.SpecificationView))
                _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, 
                                                    typeof(Views.SpecificationNavigationItem));
        }
    }
}