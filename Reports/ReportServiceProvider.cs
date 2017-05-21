﻿using Controls.Views;
using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Wrappers;
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

                    if (token.TargetBatch != null)
                        creationDialog.Batch = token.TargetBatch;

                    if (creationDialog.ShowDialog() == true)
                        _eventAggregator.GetEvent<ReportListUpdateRequested>().Publish();
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
                    output.OrderDate = poDialog.Date;
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

        public void ApplyControlPlan(IEnumerable<ISelectableRequirement> reqList, ControlPlan conPlan)
        {
            if (conPlan.IsDefault)
                foreach (ReportItemWrapper riw in reqList)
                    riw.IsSelected = true;
            else
            {
                foreach (ReportItemWrapper riw in reqList)
                    riw.IsSelected = false;

                foreach (ControlPlanItem cpi in conPlan.ControlPlanItems)
                {
                    ISelectableRequirement tempRIW = reqList.FirstOrDefault(riw => riw.RequirementInstance.ID == cpi.Requirement.ID || 
                                                    ( riw.RequirementInstance.IsOverride && riw.RequirementInstance.Overridden.ID == cpi.Requirement.ID));
                    if (tempRIW != null)
                        tempRIW.IsSelected = true;
                    
                    else
                        _eventAggregator.GetEvent<StatusNotificationIssued>().Publish("Alcuni requisiti richiesti non sono stati trovati");
                }
            }
        }

        public List<Test> GenerateTestList(List<ISelectableRequirement> reqList)
        {
            List<Test> output = new List<Test>();

            foreach (ISelectableRequirement req in reqList.Where(isr => isr.IsSelected))
            {
                Test tempTest = new Test();
                tempTest.IsComplete = false;
                tempTest.Method = req.RequirementInstance.Method;
                tempTest.MethodIssue = tempTest.Method.Standard.CurrentIssue;
                tempTest.Notes = req.RequirementInstance.Description;

                foreach (SubRequirement subReq in req.RequirementInstance.SubRequirements)
                {
                    SubTest tempSubTest = new SubTest();
                    tempSubTest.Name = subReq.SubMethod.Name;
                    tempSubTest.Requirement = subReq.RequiredValue;
                    tempSubTest.UM = subReq.SubMethod.UM;
                    tempTest.SubTests.Add(tempSubTest);
                }
                output.Add(tempTest);
            }

            return output;
        }

        public IEnumerable<Test> GenerateTestList(List<TaskItemWrapper> reqList)
        {
            List<Test> output = new List<Test>();

            foreach (TaskItemWrapper req in reqList.Where(isr => isr.IsSelected))
            {
                Test tempTest = new Test();
                tempTest.IsComplete = false;
                tempTest.Method = req.RequirementInstance.Method;
                tempTest.MethodIssue = tempTest.Method.Standard.CurrentIssue;
                tempTest.Notes = req.RequirementInstance.Description;
                tempTest.TaskItems.Add(req.TaskItemInstance);
                
                foreach (SubRequirement subReq in req.RequirementInstance.SubRequirements)
                {
                    SubTest tempSubTest = new SubTest();
                    tempSubTest.Name = subReq.SubMethod.Name;
                    tempSubTest.Requirement = subReq.RequiredValue;
                    tempSubTest.UM = subReq.SubMethod.UM;
                    tempTest.SubTests.Add(tempSubTest);
                }
                output.Add(tempTest);
            }

            return output;
        }
    }
}
