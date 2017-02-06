﻿using Infrastructure;
using Infrastructure.Events;
using Navigation;
using Prism;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls.ViewModels
{
    class ToolbarViewModel : Prism.Mvvm.BindableBase
    {
        private DelegateCommand _navigateBack, _navigateForward, _save;
        private DelegateCommand<IModuleNavigationTag> _requestNavigation;
        private IEventAggregator _eventAggregator;

        public ToolbarViewModel(IEventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;

            _navigateBack = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<NavigateBackRequested>().Publish();
                });

            _navigateForward = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<NavigateForwardRequested>().Publish();
                });

            _requestNavigation = new DelegateCommand<IModuleNavigationTag>(
                view => 
                {
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(view.ViewName);
                });
                
           _save = new DelegateCommand(
               () => 
               {
                  _eventAggregator.GetEvent<CommitRequested>().Publish(); 
               });

        }

        public DelegateCommand NavigateBackCommand
        {
            get { return _navigateBack; }
        }

        public DelegateCommand NavigateForwardCommand
        {
            get { return _navigateForward; }
        }

        public DelegateCommand<IModuleNavigationTag> RequestNavigationCommand
        {
            get { return _requestNavigation; }
        }
        
        public DelegateCommand SaveCommand
        {
            get { return _save; }
        }


    }
}
