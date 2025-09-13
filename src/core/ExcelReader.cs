using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;  
using System.Collections.Generic;
using System.IO;


public class UserData
{
    public string? UserId { get; set; }
    public string? Password { get; set; }
    public string? AccountNumber { get; set; }
}
public static class ExcelReader
{
    public static List<UserData> ReadUsers(string filePath, string sheetName = "Users")
    {
        var list = new List<UserData>();

        using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            IWorkbook workbook = new XSSFWorkbook(file);
            ISheet sheet = workbook.GetSheet(sheetName) ?? workbook.GetSheetAt(0);

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
            for (int rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
            {
                var row = sheet.GetRow(rowIndex);
                if (row == null) continue;

                var user = new UserData
                {
                    UserId = GetCellString(row, ColIndex, "UserId"),
                    Password = GetCellString(row, ColIndex, "Password"),
                    AccountNumber = GetCellString(row, ColIndex, "AccountNumber")
                };
                if(string.IsNullOrWhiteSpace(user.UserId)) continue;
                list.Add(user);
            }
        }

        return list;
    }

    private static string GetCellString(IRow row, Dictionary<string, int> colIndex, string columnName)
    {
        if (!colIndex.ContainsKey(columnName)) return string.Empty;
        var cell = row.GetCell(colIndex[columnName]);
        if (cell == null) return string.Empty;

        return cell.ToString().Trim();
    }
}
