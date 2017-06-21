﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Services
{
    public static class ReportService
    {        
        public static IEnumerable<ExternalReport> GetExternalReports()
        {
            // Returns all external report instances

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.ExternalReports.Include(exrep => exrep.ExternalLab)
                                                .OrderBy(exrep => exrep.InternalNumber)
                                                .ToList();
            }
        }

        public static Report GetReport(int ID)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Reports.First(entry => entry.ID == ID);
            }
        }

        public static IEnumerable<Report> GetReports()
        {
            // Returns all Report entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Reports.AsNoTracking()
                                        .Include(rep => rep.Author)
                                        .Include(rep => rep.Batch.Material.Aspect)
                                        .Include(rep => rep.Batch.Material.MaterialLine)
                                        .Include(rep => rep.Batch.Material.MaterialType)
                                        .Include(rep => rep.Batch.Material.Project.Oem)
                                        .Include(rep => rep.Batch.Material.Recipe.Colour)
                                        .Include(rep => rep.SpecificationVersion.Specification.Standard.CurrentIssue)
                                        .Include(rep => rep.SpecificationVersion.Specification.Standard.Organization)
                                        .Where(rep => true)
                                        .OrderByDescending(rep => rep.Number)
                                        .ToList();
            }
        }
    }
}
