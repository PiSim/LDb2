﻿using DBManager;
using DBManager.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Controls.ViewModels
{
    public class ProjectPickerDialogViewModel : BindableBase
    {
        private DelegateCommand<Window> _cancel, _confirm;
        private Project _selectedProject;
        
        public ProjectPickerDialogViewModel() : base()
        {
            
            _cancel = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = false;
                });

            _confirm = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = true;
                });
        }

        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand<Window> ConfirmCommand
        {
            get { return _confirm; }
        }

        public IEnumerable<Project> ProjectList
        {
            get { return ProjectService.GetProjects(); }
        }

        public Project SelectedProject
        {
            get { return _selectedProject; }
            set { _selectedProject = value; }
        }

    }
}