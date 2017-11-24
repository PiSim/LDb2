﻿using Controls.Views;
using DBManager;
using DBManager.Services;
using DBManager.EntityExtensions;
using Infrastructure;
using Infrastructure.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Views;

namespace Services
{
    public static class CommonProcedures
    {
        public static bool AddTestsToReport(Report entry)
        {
            AddTestDialog testDialog = new AddTestDialog();
            testDialog.ReportInstance = entry ?? throw new NullReferenceException();

            if (testDialog.ShowDialog() == true)
            {
                if (testDialog.TestList.Count() == 0)
                    return false;

                IEnumerable<Test> testList = GenerateTestList(testDialog.TestList);

                foreach (Test tst in testList)
                    tst.ReportID = entry.ID;
                testList.CreateTests();

                return true;
            }

            else
                return false;
        }

        public static void ApplyControlPlan(IEnumerable<ISelectableRequirement> reqList, ControlPlan conPlan)
        {
            IEnumerable<ControlPlanItem> itemList = conPlan.GetControlPlanItems();

            foreach (ISelectableRequirement isr in reqList)
            {
                isr.IsSelected = itemList.First(cpi => cpi.RequirementID == isr.RequirementInstance.ID
                                                        || cpi.RequirementID == isr.RequirementInstance.OverriddenID)
                                        .IsSelected;
            }
        }

        public static void DeleteSample(Sample smp)
        {
            smp.Delete();
            SampleLogChoiceWrapper tempChoice = SampleLogActions.ActionList.First(scc => scc.Code == smp.Code);
            Batch tempBatch = MaterialService.GetBatch(smp.BatchID);

            tempBatch.ArchiveStock -= tempChoice.ArchiveModifier;
            tempBatch.LongTermStock -= tempChoice.LongTermModifier;
            tempBatch.Update();
        }

        public static void CheckMaterialData(Material target)
        {
            target.Load();

            if (target.Project == null)
            {
                ProjectPickerDialog prjDialog = new ProjectPickerDialog();
                if (prjDialog.ShowDialog() == true)
                    target.Project = prjDialog.ProjectInstance;
            }

            if (target.Recipe.Colour == null)
            {
                ColorPickerDialog colourPicker = new ColorPickerDialog();
                if (colourPicker.ShowDialog() == true)
                    target.Recipe.Colour = colourPicker.ColourInstance;
            }
        }

        public static Requirement GenerateRequirement(Method method)
        {
            method.LoadSubMethods();

            Requirement tempReq = new Requirement();
            tempReq.MethodID = method.ID;
            tempReq.IsOverride = false;
            tempReq.Name = "";
            tempReq.Description = "";
            tempReq.Position = 0;

            foreach (SubMethod measure in method.SubMethods)
            {
                SubRequirement tempSub = new SubRequirement();
                tempSub.SubMethodID = measure.ID;
                tempReq.SubRequirements.Add(tempSub);
            }

            return tempReq;
        }
        
        public static IEnumerable<TaskItem> GenerateTaskItemList(IEnumerable<Requirement> reqList)
        {
            List<TaskItem> output = new List<TaskItem>();

            foreach(Requirement req in reqList)
            {
                TaskItem tempItem = new TaskItem();

                tempItem.Description = req.Description;
                tempItem.MethodID = req.MethodID;
                tempItem.Name = req.Name;
                tempItem.Position = 0;
                tempItem.RequirementID = req.ID;
                tempItem.SpecificationVersionID = req.SpecificationVersionID;
                    
                foreach (SubRequirement sreq in req.SubRequirements)
                {
                    SubTaskItem tempSubItem = new SubTaskItem();

                    tempSubItem.Name = sreq.SubMethod.Name;
                    tempSubItem.RequiredValue = sreq.RequiredValue;
                    tempSubItem.SubMethodID = sreq.SubMethodID;
                    tempSubItem.SubRequirementID = sreq.ID;
                    tempSubItem.UM = sreq.SubMethod.UM;

                    tempItem.SubTaskItems.Add(tempSubItem);
                }

                output.Add(tempItem);
            }

            return output;
        }

        public static ICollection<Test> GenerateTestList(IEnumerable<ISelectableRequirement> reqList)
        {
            List<Test> output = new List<Test>();

            foreach (ISelectableRequirement req in reqList.Where(isr => isr.IsSelected))
            {
                req.RequirementInstance.Load();

                Test tempTest = new Test();
                tempTest.Duration = req.RequirementInstance.Method.Duration;
                tempTest.IsComplete = false;
                tempTest.MethodID = req.RequirementInstance.Method.ID;
                tempTest.RequirementID = req.RequirementInstance.ID;
                tempTest.Notes = req.RequirementInstance.Description;

                foreach (SubRequirement subReq in req.RequirementInstance.SubRequirements)
                {
                    SubTest tempSubTest = new SubTest();
                    tempSubTest.Name = subReq.SubMethod.Name;
                    tempSubTest.RequiredValue = subReq.RequiredValue;
                    tempSubTest.SubRequiremntID = subReq.ID;
                    tempSubTest.UM = subReq.SubMethod.UM;
                    tempTest.SubTests.Add(tempSubTest);
                }
                output.Add(tempTest);
            }

            return output;
        }

        public static Batch GetBatch(string batchNumber)
        {
            Batch temp = MaterialService.GetBatch(batchNumber);

            if (temp == null)
            {
                temp = new Batch()
                {
                    Number = batchNumber
                };

                temp.Create();
            }

            return temp;
        }

        public static void SetTaskItemsAssignment(IEnumerable<Test> testList, DBManager.Task taskEntry)
        {
            foreach (TaskItem tski in taskEntry.TaskItems)
            {
                Test tempTest = testList.First(tst => tst.RequirementID == tski.RequirementID
                                                && !tst.TaskItems.Any());
                
                tski.TestID = tempTest.ID;

            }

            taskEntry.TaskItems.Update();
        }

        public static Material StartMaterialSelection()
        {
            MaterialCreationDialog materialPicker = new MaterialCreationDialog();
            if (materialPicker.ShowDialog() == true)
            {
                Material output = MaterialService.GetMaterial(materialPicker.MaterialType,
                                                            materialPicker.MaterialLine,
                                                            materialPicker.MaterialAspect,
                                                            materialPicker.MaterialRecipe);
                return output;
            }

            else
                return null;
        }

        public static Batch StartBatchSelection()
        {
            BatchPickerDialog batchPicker = new BatchPickerDialog();
            if (batchPicker.ShowDialog() == true)
            {
                Batch output = GetBatch(batchPicker.BatchNumber);
                return output;
            }

            else
                return null;
        }

        public static void StartSampleLog()
        {
            SampleLogDialog logger = new SampleLogDialog();

            logger.Show();
        }
    }
}
