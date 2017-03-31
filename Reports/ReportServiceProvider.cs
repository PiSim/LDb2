﻿using Controls.Views;
using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Tokens;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports
{
    public class ReportServiceProvider : IReportServiceProvider
    {
        private DBEntities _entities;
        private EventAggregator _eventAggregator;
        private IUnityContainer _container;

        public ReportServiceProvider(DBEntities entities,
                                    EventAggregator aggregator,
                                    IUnityContainer container)
        {
            _container = container;
            _entities = entities;
            _eventAggregator = aggregator;

            _eventAggregator.GetEvent<ReportCreationRequested>().Subscribe(
                token =>
                {
                    Views.ReportCreationDialog creationDialog =
                        _container.Resolve<Views.ReportCreationDialog>();

                    if (creationDialog.ShowDialog() == true)
                    {
                        _eventAggregator.GetEvent<ReportCreated>().Publish(creationDialog.ReportInstance);
                    }
                });
        }

        public PurchaseOrder AddPOToExternalReport(ExternalReport target)
        {
            ExternalReport targetReport = _entities.ExternalReports.First(xtr => xtr.ID == target.ID);

            NewPODialog poDialog = _container.Resolve<NewPODialog>();
            poDialog.SetSupplier(targetReport.ExternalLab);

            if (poDialog.ShowDialog() == true)
            {
                PurchaseOrder output = _entities.PurchaseOrders.FirstOrDefault(po => po.Number == poDialog.Number);

                if (output == null)
                {
                    output = new PurchaseOrder();
                    output.Currency = _entities.Currencies
                         .First(crn => crn.ID == poDialog.Currency.ID);
                    output.Number = poDialog.Number;
                    output.Organization = _entities.Organizations
                        .First(sup => sup.ID == poDialog.Supplier.ID);
                    output.Total = poDialog.Total;
                }

                targetReport.PO = output;
                _entities.SaveChanges();

                return output;
            }

            else
                return null;
        } 
    }
}
