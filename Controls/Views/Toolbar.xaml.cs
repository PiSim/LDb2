﻿using Infrastructure;
using Infrastructure.Events;
using Navigation;
using Microsoft.Practices.Unity;
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

namespace Controls.Views
{
    /// <summary>
    /// Interaction logic for ToolbarView.xaml
    /// </summary>
    public partial class Toolbar : UserControl
    {
        private EventAggregator _eventAggregator;

        public Toolbar(EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            InitializeComponent();
        }


        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavigationToken token = new NavigationToken((MainNavigationComboBox.SelectedItem as IModuleNavigationTag).ViewName);
            _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                (DataContext as ViewModels.ToolbarViewModel).RunSearchCommand.Execute(SearchBox.Text);
        }
    }
}