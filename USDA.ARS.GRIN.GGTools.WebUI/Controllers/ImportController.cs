using System.Web.Mvc;
using System;
using System.Collections.Generic;
using ExcelDataReader;
using System.Data;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;
using System.Reflection;
using USDA.ARS.GRIN.GGTools.DataLayer;
using System.Linq.Expressions;
using System.Linq;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class ImportController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ActionResult Index(string eventAction = "", int folderId = 0)
        {
            try
            {
                ImportViewModel viewModel = new ImportViewModel();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult LoadFileDefinition(ImportViewModel viewModel)
        {
            DataTable sourceTable = new DataTable();
            DataTable destinationTable = new DataTable();
            SysTableViewModel sysTableViewModel = new SysTableViewModel();
            SysTableField sysTableField = new SysTableField();
            SpeciesImport speciesImport = new SpeciesImport();
           

            if (!viewModel.Validate())
            {
                if (viewModel.ValidationMessages.Count > 0) return View("~/Views/Import/Index.cshtml", viewModel);
            }

            viewModel.ImportFileName = viewModel.DocumentUpload.FileName;

            using (var stream = viewModel.DocumentUpload.InputStream)
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();

                    foreach (DataTable table in result.Tables)
                    {
                        Console.WriteLine($"Sheet: {table.TableName}");

                        // Iterate through each row in the DataTable
                        for (int rowIndex = 0; rowIndex < table.Rows.Count; rowIndex++)
                        {
                            if (rowIndex == 0)
                            {
                                DataRow row = table.Rows[rowIndex];

                                // Iterate through each column in the row
                                for (int colIndex = 0; colIndex < table.Columns.Count; colIndex++)
                                {
                                    object cellValue = row[colIndex];

                                    // Check if the cell value is null or empty
                                    if (cellValue == DBNull.Value || string.IsNullOrEmpty(cellValue.ToString()))
                                    {
                                        Console.WriteLine($"Null or empty value found at Row {rowIndex + 1}, Column {colIndex + 1}");
                                    }
                                    else
                                    {
                                        viewModel.DataCollectionFields.Add(new CodeValue { Value = cellValue.ToString(), Title = cellValue.ToString() });
                                    }

                                    //bool isHeaderRow = true;
                                    //do
                                    //{
                                    //    // Iterate through each row in the current sheet
                                    //    while (reader.Read())
                                    //    {
                                    //        // Check if the current row is a header row
                                    //        //if (isHeaderRow)
                                    //        //{
                                    //        //    // Process header row (you can store or validate headers here)
                                    //        //    for (int i = 0; i < reader.FieldCount; i++)
                                    //        //    {
                                    //        //        //Console.Write(reader.GetValue(i) + "\t");
                                    //        //        viewModel.DataCollectionFields.Add(new CodeValue { ID=i, Value = reader.GetValue(i).ToString(), Title = reader.GetValue(i).ToString() });
                                    //        //    }
                                    //        //    Console.WriteLine(); // Move to the next line for the next row

                                    //        //    // Set the flag to false since we've processed the header row
                                    //        //    isHeaderRow = false;
                                    //        //}
                                    //        //else
                                    //        //{
                                    //        // Process data rows
                                    //        for (int i = 0; i < reader.FieldCount; i++)
                                    //        {
                                    //            var DEBUG = reader.GetValue(i);
                                    //            if (DEBUG != null)
                                    //            {
                                    //                viewModel.DataCollectionFields.Add(new CodeValue { ID = i, Value = reader.GetValue(i).ToString(), Title = reader.GetValue(i).ToString() });
                                    //            }
                                    //        }
                                    //        Console.WriteLine(); // Move to the next line for the next row
                                    //        //}
                                    //    }
                                    //} while (reader.NextResult()); // Move to next sheet, if available
                                }
                            }
                        }
                        return View("~/Views/Import/Index.cshtml", viewModel);
                    }


                    
                }
            }
            return View();
        }

        [HttpPost]
        public PartialViewResult ImportFile(ImportViewModel viewModel)
        {

            DataTable sourceTable = new DataTable();
            DataTable destinationTable = new DataTable();
            SysTableViewModel sysTableViewModel = new SysTableViewModel();
            SysTableField sysTableField = new SysTableField();
            SpeciesImport speciesImport = new SpeciesImport();
            bool genusMatch;
            bool speciesMatch;

            viewModel.ImportFileName = viewModel.DocumentUpload.FileName;

            using (var stream = viewModel.DocumentUpload.InputStream)
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true,
                        }
                    });
                    sourceTable = result.Tables[0];

                    List<string> columnsToCopy = new List<string>();

                    foreach (DataColumn col in sourceTable.Columns)
                    {
                        columnsToCopy.Add(col.ColumnName);
                    }


                    // Define the subset of columns to copy
                    //string[] columnsToCopy = {"ID", "TAXONOMY_GENUS", "TAXONOMY_SPECIES", "AUTHOR" };

                    // Add columns to the destination DataTable based on the subset
                    foreach (string column in columnsToCopy)
                    {
                        destinationTable.Columns.Add(column, sourceTable.Columns[column].DataType);
                    }

                    // Add columns that will show matching species data (if any).
                    DataColumn taxonomySpeciesResultColumn = new DataColumn();
                    taxonomySpeciesResultColumn.ColumnName = "MATCH_GENUS";
                    destinationTable.Columns.Add(taxonomySpeciesResultColumn.ColumnName, sourceTable.Columns["TAXONOMY_GENUS"].DataType);

                    DataColumn taxonomySpeciesResultColumn2 = new DataColumn();
                    taxonomySpeciesResultColumn.ColumnName = "MATCH_SPECIES";
                    destinationTable.Columns.Add(taxonomySpeciesResultColumn.ColumnName, sourceTable.Columns["TAXONOMY_SPECIES"].DataType);
                                       
                    DataColumn matchNoteColumn = new DataColumn();
                    matchNoteColumn.ColumnName = "MATCH_NOTE";
                    destinationTable.Columns.Add(matchNoteColumn.ColumnName);

                    // Copy data from source to destination
                    foreach (DataRow sourceRow in sourceTable.Rows)
                    {
                        DataRow destRow = destinationTable.NewRow();
                        foreach (string column in columnsToCopy)
                        {
                            destRow[column] = sourceRow[column];

                            if (column == "TAXONOMY_SPECIES")
                            {
                                string sourceGenusName = sourceRow["TAXONOMY_GENUS"].ToString();
                                string sourceSpeciesName = sourceRow[column].ToString();

                                // Check genus
                                GenusViewModel genusViewModel = new GenusViewModel();
                                genusViewModel.SearchEntity.Name = sourceGenusName;
                                genusViewModel.Search();

                                if (genusViewModel.DataCollection.Count > 0)
                                {
                                    destRow["MATCH_GENUS"] = "Y";
                                    genusMatch = true;
                                }
                                else
                                {
                                    destRow["MATCH_GENUS"] = "NO";
                                    genusMatch = false;
                                }

                                if (!String.IsNullOrEmpty(sourceSpeciesName))
                                {
                                    SpeciesViewModel speciesViewModel = new SpeciesViewModel();
                                    speciesViewModel.SearchEntity.Name = sourceGenusName + " " + sourceSpeciesName;
                                    speciesViewModel.Search();

                                    if (speciesViewModel.DataCollection.Count > 0)
                                    {
                                        destRow["MATCH_SPECIES"] = "YES";
                                        speciesMatch = true;
                                    }
                                    else
                                    {
                                        destRow["MATCH_SPECIES"] = "NO";
                                        speciesMatch = false;
                                        List<Species> speciesList = speciesViewModel.SearchNames(genusViewModel.Entity.ID, sourceSpeciesName);
                                        if (speciesList.Count > 0)
                                        {
                                            List<string> speciesNameList = new List<string>();
                                            string speciesNameString = String.Empty;
                                            foreach (var species in speciesList)
                                            {
                                                speciesNameString += species.SpeciesName + ",";
                                            }
                                            destRow["MATCH_NOTE"] = speciesNameString;
                                        }

                                        // If genus matches but species does not, retrieve a list of all species linked
                                        // to the genus, ranked in order of closeness of match.



                                        //if (genusMatch == true)
                                        //{
                                        //    SpeciesViewModel speciesViewModel1 = new SpeciesViewModel();
                                        //    speciesViewModel1.SearchEntity.GenusName = sourceGenusName;
                                        //    speciesViewModel1.Search();
                                        //    if (speciesViewModel1.DataCollection.Count > 0)
                                        //    {
                                        //        destRow["MATCH_FOUND"] = speciesViewModel1.DataCollection.Count + " similar";
                                        //    }
                                        //}
                                        //speciesMatch = false;
                                    }
                                }
                            }

                        }
                        destinationTable.Rows.Add(destRow);
                    }

                    viewModel.DataCollectionDataTable = destinationTable.Copy();

                    
                }
            }
            return PartialView("~/Views/Import/Index.cshtml", viewModel);
        }
    }
}
