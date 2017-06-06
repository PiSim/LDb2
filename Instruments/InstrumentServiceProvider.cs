﻿using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instruments
{
    public class InstrumentServiceProvider
    {
        private DBEntities _entities;
        private EventAggregator _eventAggregator;
        private IUnityContainer _container;

        public InstrumentServiceProvider(DBEntities entities,
                                        EventAggregator aggregator,
                                        IUnityContainer container)
        {
            _entities = entities;
            _eventAggregator = aggregator;
            _container = container;

            _eventAggregator.GetEvent<InstrumentCreationRequested>()
                            .Subscribe(
                () =>
                {
                    RegisterNewInstrument();
                });
        }

        public CalibrationReport RegisterNewCalibration(Instrument target)
        {
            Views.NewCalibrationDialog calibrationDialog = _container.Resolve<Views.NewCalibrationDialog>();
            calibrationDialog.InstrumentInstance = target;
            if (calibrationDialog.ShowDialog() == true)
            {
                CalibrationReport output = calibrationDialog.ReportInstance;
                Instrument tempTarget = _entities.Instruments.First(ins => ins.ID == target.ID);

                DateTime tempNewDate = output.Date.AddMonths(target.ControlPeriod);
                if (tempNewDate > tempTarget.CalibrationDueDate)
                {
                    tempTarget.CalibrationDueDate = tempNewDate;
                    _entities.SaveChanges();
                }
                _eventAggregator.GetEvent<CalibrationIssued>().Publish(output);
                return output;
            }
            else return null;
        }

        public Instrument RegisterNewInstrument()
        {
            Views.InstrumentCreationDialog creationDialog = _container.Resolve<Views.InstrumentCreationDialog>();
            if (creationDialog.ShowDialog() == true)
            {
                _eventAggregator.GetEvent<InstrumentListUpdateRequested>().Publish();
                return creationDialog.InstrumentInstance;
            }
            else
                return null;
        }
    }
}
