﻿// Add a reference to System.Data.SqlClient
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;

// test comment
public class ExcelController : ControllerBase
{
    [HttpPost("api/excel/upload")]
    public IActionResult UploadExcelFile(IFormFile file)
    {
        // Parse the Excel file---note
        var dataTable = ParseExcelFile(file);

        // Insert the data into the database
        InsertDataIntoDatabase(dataTable);
        return Ok();
    }

    private DataTable ParseExcelFile(IFormFile file)
    {
        var dataTable = new DataTable();

        using (var stream = new MemoryStream())
        {
            file.CopyTo(stream);
            var excelReader = ExcelReaderFactory.CreateReader(stream);

            // Read the first sheet from the Excel file
            var dataSet = excelReader.AsDataSet(new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = true
                }
            });
            dataTable = dataSet.Tables[0];
        }

        return dataTable;
    }

    private void InsertDataIntoDatabase(DataTable dataTable)
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
                    command.Parameters.Add("@groupId", SqlDbType.Int).Value = row["GroupId"];

                 

                    command.ExecuteNonQuery();
                }
            }
        }

        //using (var connection = new SqlConnection(connectionString))
        //{
        //    connection.Open();


        // Create a SqlCommand object with parameterized SQL to insert the data
        //var commandText = "INSERT INTO Students ([Id], [password],[firstName], [lastName], [phone], [email], [parentPhone], [pictureUrl]) VALUES (@Id,  @password,  @firstName, @lastName, @phone, @email, @parentPhone,@pictureUrl) ";

        //using (var command = new SqlCommand(commandText, connection))
        //{
        //    // Add parameters for each column in the table
        //    command.Parameters.Add("@Id", SqlDbType.Int);
        //    command.Parameters.Add("@password", SqlDbType.NVarChar);
        //    command.Parameters.Add("@firstName", SqlDbType.NVarChar);
        //    command.Parameters.Add("@lastName", SqlDbType.NVarChar);
        //    command.Parameters.Add("@phone", SqlDbType.NVarChar);
        //    command.Parameters.Add("@email", SqlDbType.VarChar);
        //    command.Parameters.Add("@parentPhone", SqlDbType.VarChar);
        //    command.Parameters.Add("@pictureUrl", SqlDbType.VarChar);


        //    // Add more parameters for each column in your table

        //    // Loop through the rows of the DataTable and insert each row into the database
        //    foreach (DataRow row in dataTable.Rows)
        //    {
        //        command.Parameters["@Id"].Value = row["Id"];
        //        command.Parameters["@password"].Value = row["Password"];
        //        command.Parameters["@firstName"].Value = row["FirstName"];
        //        command.Parameters["@lastName"].Value = row["LastName"];
        //        command.Parameters["@phone"].Value = row["Phone"];
        //        command.Parameters["@email"].Value = row["Email"];
        //        command.Parameters["@parentPhone"].Value = row["ParentPhone"];
        //        command.Parameters["@pictureUrl"].Value = row["PictureUrl"];


        //        // Set the value of each parameter for each column in your table
        //        // ...


        //        command.ExecuteNonQuery();
        //    }
    }
}
    