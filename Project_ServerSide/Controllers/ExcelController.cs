using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Versioning;


namespace Project_ServerSide.Controllers
{
    [ApiController]
    public class ExcelController : ControllerBase
    {
        [HttpPost("api/excel/upload/groupId/{groupId}")]
        public IActionResult UploadExcelFile(IFormFile file, int groupId)
        {
            // Set the license context for EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Parse the Excel file
            var dataTable = ParseExcelFile(file);

            // Insert the data into the database
            InsertDataIntoDatabase(dataTable, groupId);

            return Ok();
        }

        private DataTable ParseExcelFile(IFormFile file)
        {
            var dataTable = new DataTable();

            using (var package = new ExcelPackage(file.OpenReadStream()))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var cellCount = worksheet.Dimension.End.Column;
                var rowCount = worksheet.Dimension.End.Row;

                // Set up the columns in the data table
                for (var cell = 1; cell <= cellCount; cell++)
                {
                    dataTable.Columns.Add(worksheet.Cells[1, cell].Value.ToString());
                }

                // Add the rows to the data table
                for (var row = 2; row <= rowCount; row++)
                {
                    var dataRow = dataTable.NewRow();
                    for (var cell = 1; cell <= cellCount; cell++)
                    {
                        dataRow[cell - 1] = worksheet.Cells[row, cell].Value;
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }

        private void InsertDataIntoDatabase(DataTable dataTable, int groupId)
        {
            // Connect to the database
            var connectionString = "Data Source=Media.ruppin.ac.il;Initial Catalog=igroup127_prod; User ID=igroup127; Password=igroup127_29833";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("spInsertStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    foreach (DataRow row in dataTable.Rows)
                    {
                        command.Parameters.Clear();

                        command.Parameters.Add("@Id", SqlDbType.Int).Value = row["StudentId"];
                        command.Parameters.Add("@password", SqlDbType.NVarChar).Value = row["Password"];
                        command.Parameters.Add("@firstName", SqlDbType.NVarChar).Value = row["FirstName"];
                        command.Parameters.Add("@lastName", SqlDbType.NVarChar).Value = row["LastName"];
                        command.Parameters.Add("@phone", SqlDbType.NVarChar).Value = row["Phone"];
                        command.Parameters.Add("@email", SqlDbType.VarChar).Value = row["Email"];
                        command.Parameters.Add("@parentPhone", SqlDbType.VarChar).Value = row["ParentPhone"];
                        command.Parameters.Add("@pictureUrl", SqlDbType.VarChar).Value = row["PictureUrl"];
                        command.Parameters.Add("@groupId", SqlDbType.Int).Value = groupId;

                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
