﻿using Controls.Views;
using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal class ServiceProvider
    {

        internal static Person AddPerson()
        {
            StringInputDialog addPersonDialog = new StringInputDialog
            {
                Title = "Creazione nuova Persona",
                Message = "Nome:"
            };

            if (addPersonDialog.ShowDialog() != true)
                return null;

            Person newPerson = new Person
            {
                Name = addPersonDialog.InputString
            };

            foreach (PersonRole prr in PeopleService.GetPersonRoles())
            {
                PersonRoleMapping tempPRM = new PersonRoleMapping();
                tempPRM.roleID = prr.ID;
                tempPRM.IsSelected = false;
                newPerson.RoleMappings.Add(tempPRM);
            }

            newPerson.Create();

            return newPerson;
        }

        internal static void AddUserRole(string name)
        {
            UserRole newRole = new UserRole
            {
                Name = name,
                Description = ""
            };

            newRole.Create();

            foreach (User usr in DataService.GetUsers())
            {
                UserRoleMapping newMap = new UserRoleMapping
                {
                    IsSelected = false,
                    RoleID = newRole.ID,
                    UserID = usr.ID
                };

                newMap.Create();
            }
        }

        internal static Batch CreateBatch()
        {
            Views.BatchCreationDialog batchCreator = new Views.BatchCreationDialog();

            if (batchCreator.ShowDialog() == true)
            {
                return batchCreator.BatchInstance;
            }

            else
                return null;
        }

        public static InstrumentType CreateNewInstrumentType()
        {
            StringInputDialog creationDialog = new StringInputDialog
            {
                Title = "Crea nuovo Tipo strumenti"
            };

            if (creationDialog.ShowDialog() == true)
            {
                InstrumentType output = new InstrumentType()
                {
                    Name = creationDialog.InputString
                };

                output.Create();
                return output;
            }

            return null;
        }

        public static MeasurableQuantity CreateNewMeasurableQuantity()
        {
            StringInputDialog creationDialog = new StringInputDialog
            {
                Title = "Crea nuova quantità misurabile"
            };

            if (creationDialog.ShowDialog() == true)
            {
                MeasurableQuantity output = new MeasurableQuantity()
                {
                    Name = creationDialog.InputString
                };

                output.Create();
                return output;
            }

            return null;
        }

        public static Organization CreateNewOrganization()
        {
            StringInputDialog creationDialog = new StringInputDialog
            {
                Title = "Crea nuovo Ente"
            };

            if (creationDialog.ShowDialog() == true)
            {
                Organization output = new Organization
                {
                    Category = "",
                    Name = creationDialog.InputString
                };
                foreach (OrganizationRole orr in DataService.GetOrganizationRoles())
                {
                    OrganizationRoleMapping tempORM = new OrganizationRoleMapping
                    {
                        IsSelected = false,
                        RoleID = orr.ID
                    };

                    output.RoleMapping.Add(tempORM);
                }

                output.Create();

                return output;
            }
            else return null;
        }

        public static void CreateNewOrganizationRole()
        {
            StringInputDialog creationDialog = new StringInputDialog();
            creationDialog.Title = "Crea nuovo Ruolo Organizzazione";

            if (creationDialog.ShowDialog() == true)
            {
                OrganizationRole output = new OrganizationRole();
                output.Description = "";
                output.Name = creationDialog.InputString;
                output.Create();

                OrganizationService.CreateMappingsForNewRole(output);
            }

        }

        public static CalibrationReport RegisterNewCalibration(Instrument target)
        {
            Views.NewCalibrationDialog calibrationDialog = new Views.NewCalibrationDialog();
            calibrationDialog.InstrumentInstance = target;
            if (calibrationDialog.ShowDialog() == true)
            {
                CalibrationReport output = calibrationDialog.ReportInstance;

                output.Instrument.UpdateCalibrationDueDate();
                output.Instrument.Update();

                return output;
            }
            else return null;
        }

        internal static ExternalReport StartExternalReportCreation()
        {
            Views.ExternalReportCreationDialog creationDialog = new Views.ExternalReportCreationDialog();

            if (creationDialog.ShowDialog() == true)
                return creationDialog.ExternalReportInstance;
            else
                return null;
        }

        internal static void UpdateProjectCosts()
        {
            IEnumerable<Project> _prjList = ProjectService.GetProjects();

            foreach (Project prj in _prjList)
            {
                double oldvalue = prj.TotalExternalCost;

                prj.UpdateExternalReportCost();

                if (prj.TotalExternalCost != oldvalue)
                    prj.Update();
            }
        }
    }
}
