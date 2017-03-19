﻿using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instruments.ViewModels
{
    public class NewCalibrationDialogViewModel : BindableBase
    {
        private DelegateCommand _cancel, _confirm;
        private Views.NewCalibrationDialog _parentDialog;

        public NewCalibrationDialogViewModel(Views.NewCalibrationDialog parentDialog) : base()
        {

        }

        public DelegateCommand CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand ConfirmCommand
        {
            get { return _confirm; }
        }
    }
}
