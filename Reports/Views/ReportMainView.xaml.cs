﻿using DBManager;
using Prism.Events;
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
    /// Interaction logic for ReportMainView.xaml
    /// </summary>
    public partial class ReportMainView : UserControl
    {
        public ReportMainView(DBEntities entities, EventAggregator eventAggregator)
        {
            DataContext = new ViewModels.MainViewModel(entities, eventAggregator);
            InitializeComponent();
        }
    }
}
