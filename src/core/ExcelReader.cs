
// ExcelReader.cs
// This static class provides methods to read user data from an Excel file using NPOI.
// It parses the file and returns a list of UserData objects for use in data-driven tests.

using NPOI.SS.UserModel; // NPOI library for Excel file manipulation
using NPOI.XSSF.UserModel; // For working with .xlsx files
using System.Collections.Generic; // For List and Dictionary
using System.IO; // For file operations

// Represents a user record read from the Excel file
public class UserData
{
    public string? UserId { get; set; } // User ID for login
    public string? Password { get; set; } // Password for login
    public string? AccountNumber { get; set; } // Account number for verification
}

// ExcelReader provides static methods to read user data from Excel
public static class ExcelReader
{
    // Reads users from the specified Excel file and sheet
    // Parameters:
    //   filePath - path to the Excel file
    //   sheetName - name of the sheet to read (default: "Users")
    // Returns a list of UserData objects
    public static List<UserData> ReadUsers(string filePath, string sheetName = "Users")
    {
        var list = new List<UserData>(); // List to hold user records

        // Open the Excel file for reading
        using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            // Load the workbook and sheet
            IWorkbook workbook = new XSSFWorkbook(file);
            ISheet sheet = workbook.GetSheet(sheetName) ?? workbook.GetSheetAt(0);

            // Read the header row to map column names to indices
            var headerRow = sheet.GetRow(0);
            var ColIndex = new Dictionary<string, int>();
            for (int colIndex = 0; colIndex < headerRow.LastCellNum; colIndex++)
            {
                var cellValue = headerRow.GetCell(colIndex);
                if (!string.IsNullOrEmpty(cellValue.ToString().Trim()))
                {
                    ColIndex[cellValue.StringCellValue.Trim()] = colIndex;
                }
                else
                {
                    continue;
                }
            }
            // Iterate through each row to extract user data
            for (int rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
            {
                var row = sheet.GetRow(rowIndex);
                if (row == null) continue;

                // Create a UserData object for each row
                var user = new UserData
                {
                    UserId = GetCellString(row, ColIndex, "UserId"),
                    Password = GetCellString(row, ColIndex, "Password"),
                    AccountNumber = GetCellString(row, ColIndex, "AccountNumber")
                };
                // Skip rows with empty UserId
                if(string.IsNullOrWhiteSpace(user.UserId)) continue;
                list.Add(user);
            }
        }

        return list;
    }

    // Helper method to get the string value of a cell by column name
    private static string GetCellString(IRow row, Dictionary<string, int> colIndex, string columnName)
    {
        if (!colIndex.ContainsKey(columnName)) return string.Empty;
        var cell = row.GetCell(colIndex[columnName]);
        if (cell == null) return string.Empty;

        return cell.ToString().Trim();
    }
}
