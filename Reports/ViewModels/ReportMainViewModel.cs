﻿using DBManager;
using Controls.Views;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Tokens;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.ViewModels
{
    class ReportMainViewModel : BindableBase
    {
        private bool _isActive;
        private DBEntities _entities;
        private DelegateCommand _newReport, _openReport;
        private EventAggregator _eventAggregator;
        private ObservableCollection<Report> _reportList;
        private Report _selectedReport;
        private UnityContainer _container;

        public ReportMainViewModel(DBEntities entities, 
                            EventAggregator eventAggregator,
                            UnityContainer container) : base()
        {
            _container = container;
            _entities = entities;
            _eventAggregator = eventAggregator;
            _isActive = false;
            _reportList = new ObservableCollection<Report>(entities.Reports);

            _openReport = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(ViewNames.ReportEditView, SelectedReport);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => SelectedReport != null);

            _newReport = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<ReportCreationRequested>().Publish(new NewReportToken());
                });

            _eventAggregator.GetEvent<ReportCreated>().Subscribe(
                rpt => 
                {
                    _reportList.Add(rpt);
                    SelectedReport = rpt;
                } ); 
        }

        public DelegateCommand NewReportCommand
        {
            get { return _newReport; }
        }

        public DelegateCommand OpenReportCommand
        {
            get { return _openReport; }
        }

        public ObservableCollection<Report> ReportList
        {
            get { return _reportList; }
        }

        public Report SelectedReport
        {
            get { return _selectedReport; }
            set 
            { 
                _selectedReport = value; 
                OpenReportCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("SelectedReport");                
            }
        }
    }
}