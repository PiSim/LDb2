﻿using DBManager;
using Infrastructure;
using Infrastructure.Events;
using iText;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting
{
    public class ReportingEngine
    {
        private DBEntities _entities;
        private EventAggregator _eventAggregator;

        public ReportingEngine(DBEntities entities,
                                EventAggregator eventAggregator)
        {
            _entities = entities;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<GenerateReportDataSheetRequested>().Subscribe(
                targetReport =>
                {
                    string sheetPath = GenerateReportDataSheet(targetReport);

                    System.Diagnostics.Process.Start(sheetPath);
                });
        }

        private void AddTestToDataSheet(Table table, Test test)
        {
            int rowHeight = test.SubTests.Count;

            int jj = 0;

            foreach (SubTest sTest in test.SubTests)
            {
                Cell testCell = new Cell();
                if (jj == 0)
                    testCell.Add(new Paragraph(test.Method.Property.Name));

                string testLabel;

                if (rowHeight == 1)
                    testLabel = "";
                else
                    testLabel = sTest.Name;

                testCell.Add(new Paragraph(testLabel).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)
                                                    .SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.TOP));

                if (sTest.Requirement != "")
                    testCell.Add(new Paragraph("[" + sTest.Requirement + "]").SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)
                                                                            .SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.BOTTOM));

                table.AddCell(testCell);
                table.AddCell(new Cell().Add(new Paragraph(sTest.UM))
                                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                                        .SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE));               

                for (int ii = 0; ii < 4; ii++)
                {
                    table.AddCell(new Cell());
                }

                if (jj == 0)
                    table.AddCell(new Cell(rowHeight, 1).Add(new Paragraph(test.Method.Standard.Name))
                                                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                                                        .SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE));

                jj++;
            }

        }

        public string GenerateReportDataSheet(Report target)
        {
            string filename = target.Category + "_"
                            + target.Number + "_FRD_" 
                            + target.Batch.Number + ".pdf";
            string fullPath = System.IO.Path.GetTempPath() + filename;
            PdfWriter writer = new PdfWriter(fullPath);
            PdfDocument pdfDoc = new PdfDocument(writer);
            Document dataSheet = new Document(pdfDoc);
            
            // Adds company logo and title at the top of the page

            Table titletable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 2}));
            
            titletable.AddCell(new Cell().Add(new Paragraph("LOGO_VUL"))
                                        .SetBorder(iText.Layout.Borders.Border.NO_BORDER));
            titletable.AddCell(new Cell().Add(new Paragraph("CONTROLLO PRODOTTO FINITO"))
                                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                                        .Add(new Paragraph("Modulo raccolta dati")
                                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER))
                                        .SetBorder(iText.Layout.Borders.Border.NO_BORDER));

            dataSheet.Add(titletable);

            // Attempts to retrieve relevant material info

            string colourName, materialCode, prjName, recipeCode;
            
            try 
            {
                colourName = target.Batch.Material.Recipe.Colour.Name;
            }
            catch(NullReferenceException)
            {
                colourName = "";
            }

            try
            {
                materialCode = target.Batch.Material.Construction.Type.Code
                                + target.Batch.Material.Construction.Line
                                + target.Batch.Material.Construction.Aspect.Code;
            }
            catch(NullReferenceException)
            {
               materialCode = "";
            }
            
            try 
            {
                prjName = target.Batch.Material.Construction.Project.Name;
            }
            catch(NullReferenceException)
            {
                prjName = "";
            }
            
            try 
            {
                recipeCode = target.Batch.Material.Recipe.Code;
            }
            catch(NullReferenceException)
            {
                recipeCode = "";
            }

            // Composes the header table with the report and material info            

            Table headerTable = new Table(new float[] {1, 2, 1, 2 });
            headerTable.SetWidthPercent(100);
            headerTable.AddCell(new Cell().Add(new Paragraph("Report N. :")));
            headerTable.AddCell(new Cell().Add(new Paragraph(target.Category + target.Number)));
            headerTable.AddCell(new Cell().Add(new Paragraph("Progetto:")));
            headerTable.AddCell(new Cell().Add(new Paragraph(prjName)));
            headerTable.AddCell(new Cell().Add(new Paragraph("Batch:")));
            headerTable.AddCell(new Cell().Add(new Paragraph(target.Batch.Number)));
            headerTable.AddCell(new Cell().Add(new Paragraph("Specifica:")));
            headerTable.AddCell(new Cell().Add(new Paragraph(target.SpecificationVersion.Specification.Standard.Name
                                                            + " : " + target.SpecificationIssues.Issue
                                                            + " - " + target.SpecificationVersion.Name)));
            headerTable.AddCell(new Cell().Add(new Paragraph("Materiale:")));
            headerTable.AddCell(new Cell().Add(new Paragraph(materialCode)));
            headerTable.AddCell(new Cell().Add(new Paragraph("Colore:")));
            headerTable.AddCell(new Cell().Add(new Paragraph( recipeCode + " "
                                                            + colourName)));

            dataSheet.Add(headerTable);

            Table testTable = new Table(UnitValue.CreatePercentArray(new float[] { 5, 1, 1, 1, 1, 1, 1 }));
            testTable.SetWidthPercent(100);

            testTable.AddCell(new Cell().Add(new Paragraph("Prova"))
                                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            testTable.AddCell(new Cell().Add(new Paragraph("UM"))
                                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            testTable.AddCell(new Cell(1, 3).Add(new Paragraph("Valori misurati"))
                                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            testTable.AddCell(new Cell().Add(new Paragraph("Media"))
                                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            testTable.AddCell(new Cell().Add(new Paragraph("Metodo"))
                                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

            foreach (Test tst in target.Tests)
            {
                AddTestToDataSheet(testTable, tst);
            }

            dataSheet.Add(testTable);

            Table footerTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 2, 1, 2}));
            footerTable.SetWidthPercent(100);

            footerTable.AddCell(new Cell(2, 1).Add(new Paragraph("Note:"))
                                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                                            .SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE));
            footerTable.AddCell(new Cell(2, 3));
            footerTable.AddCell(new Cell().Add(new Paragraph("TL:")));
            footerTable.AddCell(new Cell().Add(new Paragraph(target.Author.Name)));
            footerTable.AddCell(new Cell().Add(new Paragraph("Firma TL:")));
            footerTable.AddCell(new Cell()); 
            footerTable.AddCell(new Cell().Add(new Paragraph("RL:")));

            string rlName;

            try
            {
                rlName = _entities.PersonRoles.First(pr => pr.Name == PersonRoleNames.LabManager)
                                            .RoleMappings.FirstOrDefault(prm => prm.IsSelected)
                                            .Person
                                            .Name;
            }
            catch (NullReferenceException)
            {
                rlName = "";
            }

            footerTable.AddCell(new Cell().Add(new Paragraph(rlName)));
            footerTable.AddCell(new Cell().Add(new Paragraph("Firma RL:")));
            footerTable.AddCell(new Cell());
            footerTable.AddCell(new Cell().Add(new Paragraph("Approvato:")));
            footerTable.AddCell(new Cell().Add(new Paragraph("\u25A1  SI  \u25A1  NO")));
            footerTable.AddCell(new Cell().Add(new Paragraph("Data:")));
            footerTable.AddCell(new Cell().Add(new Paragraph(DateTime.Now.ToShortDateString())));

            dataSheet.Add(footerTable);

            dataSheet.Close();
            return fullPath;
        }

    }
}
