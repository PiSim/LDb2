﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public partial class Test
    {
        public string MethodName => Method?.Standard?.Name;

        public string PropertyName => Method?.Property?.Name;

    }

    public static class TestExtension
    {
        public static void CreateTests(this IEnumerable<Test> testList)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Tests.AddRange(testList);
                entities.SaveChanges();
            }
        }

        public static void Delete(this Test entry)
        {
            // Deletes a Test entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Entry(entities.Tests
                        .First(tst => tst.ID == entry.ID))
                        .State = System.Data.Entity.EntityState.Deleted;

                entities.SaveChanges();

                entry.ID = 0;
            }
        }

        public static TaskItem GetTaskItem(this Test entry)
        {
            // Returns the task item that originated a Test entry, or null if one doesn't exist

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.TaskItems.FirstOrDefault(tski => tski.TestID == entry.ID);
            }
        }

        public static void Load(this Test entry)
        {
            if (entry == null)
                return;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                Test tempEntry = entities.Tests.Include(tst => tst.Instrument)
                                                .Include(tst => tst.Method.Property)
                                                .Include(tst => tst.Method.Standard.Organization)
                                                .Include(tst => tst.Person)
                                                .Include(tst => tst.Report)
                                                .Include(tst => tst.SubTests)
                                                .First(tst => tst.ID == entry.ID);

                entry.Date = tempEntry.Date;
                entry.Instrument = tempEntry.Instrument;
                entry.instrumentID = tempEntry.instrumentID;
                entry.IsComplete = tempEntry.IsComplete;
                entry.Method = tempEntry.Method;
                entry.MethodID = tempEntry.MethodID;
                entry.Notes = tempEntry.Notes;
                entry.operatorID = tempEntry.operatorID;
                entry.Person = tempEntry.Person;
                entry.Report = tempEntry.Report;
                entry.ReportID = tempEntry.ReportID;
                entry.SubTests = tempEntry.SubTests;
            }
        }

        public static void SetMethod(this Test entry,
                                    Method methodEntity)
        {
            entry.Method = methodEntity;
            entry.MethodID = (methodEntity == null) ? 0 : methodEntity.ID;
        }

        public static void Update(this IEnumerable<Test> entryList)
        {
            // Updates all related Test and Subtest instances in a report

            using (DBEntities entities = new DBEntities())
            {
                foreach (Test tst in entryList)
                {
                    entities.Tests.AddOrUpdate(tst);
                    foreach (SubTest sts in tst.SubTests)
                        entities.SubTests.AddOrUpdate(sts);
                }
                entities.SaveChanges();
            }
        }
    }
}
