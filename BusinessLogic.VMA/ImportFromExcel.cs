using BusinessLogic.Abstraction.VMA.Contract;
using ClosedXML.Excel;
using Database.Abstraction.VMA.Contract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.VMA
{
    public class ImportFromExcel : IImportFromExcel
    {
        private readonly IVendorRepository _vendorRepository;
        public ImportFromExcel(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        public async Task<int> ImportVendors(string filePath)
        {
            var dataTable = ReadExcelFile(filePath);
            return _vendorRepository.SaveImportedVendorsToDatabase(dataTable);
        }

        private DataTable ReadExcelFile(string filePath)
        {
            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1); // Read the first worksheet
                var dataTable = new DataTable();

                // Adding columns to DataTable
                foreach (var cell in worksheet.Row(1).CellsUsed())
                {
                    dataTable.Columns.Add(cell.Value.ToString());
                }

                // Adding rows to DataTable
                foreach (var row in worksheet.RowsUsed().Skip(1)) // Skipping header row
                {
                    var dataRow = dataTable.NewRow();
                    int i = 0;
                    foreach (var cell in row.Cells(1, dataTable.Columns.Count))
                    {
                        dataRow[i++] = cell.Value.ToString();
                    }
                    dataTable.Rows.Add(dataRow);
                }

                return dataTable;
            }
        }
    }
}
