﻿using DBManager;
using Microsoft.Prism.Mvvm;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Reports.Views
{
    /// <summary>
    /// Interaction logic for ReportEditView.xaml
    /// </summary>
    public partial class ExternalReportEdit : UserControl, INavigationAware, IView
    {
        public ExternalReportEdit()
        {
            InitializeComponent();
        }


        public bool IsNavigationTarget(NavigationContext ncontext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext ncontext)
        {

        }

        public void OnNavigatedTo(NavigationContext ncontext)
        {
            (DataContext as ViewModels.ExternalReportEditViewModel).ExternalReportInstance =
                 ncontext.Parameters["ObjectInstance"] as ExternalReport;
        }
    }
}