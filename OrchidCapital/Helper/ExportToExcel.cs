using ClosedXML.Excel;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace OrchidCapital.Helper
{
    public class ExportDataModel<T> where T : class
    {
        public required string Title { get; set; }
        public DataTable data { get; set; }
        public IEnumerable<T> Tdata { get; set; }
        public string ExcludeColumnList { get; set; }
        public Dictionary<string, string> filters { get; set; } = null;
    }

    public static class ExportToExcel
    {
        public static byte[] GenerateExcel<T>(ExportDataModel<T> exportDataModel, ref string errorMsg) where T : class
        {
            try
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add();

                #region "Add Title"

                int totalcolumns = 0;
                // Exclude columns from model

                PropertyInfo[] propInfo = null;

                if (exportDataModel.data != null)
                {
                    if (!string.IsNullOrEmpty(exportDataModel.ExcludeColumnList))
                    {
                        string exCols = "," + exportDataModel.ExcludeColumnList + ",";
                        for (int i = exportDataModel.data.Columns.Count - 1; i >= 0; i--)
                        {
                            if (exCols.Contains("," + exportDataModel.data.Columns[i].ColumnName + ","))
                            {
                                exportDataModel.data.Columns.Remove(exportDataModel.data.Columns[i].ColumnName);
                                exportDataModel.data.AcceptChanges();
                            }
                        }
                    }
                    totalcolumns = exportDataModel.data.Columns.Count;
                }
                else
                {
                    // Get properties of T
                    propInfo = typeof(T).GetProperties();
                    string exCols = "," + exportDataModel.ExcludeColumnList + ",";

                    propInfo = propInfo.Where(p => !exCols.Contains("," + p.Name + ",")).ToArray();

                    totalcolumns = propInfo.Length;
                }

                worksheet.Cell(1, 1).Value = exportDataModel.Title;
                worksheet.Range(1, 1, 1, totalcolumns).Merge();
                worksheet.Range(1, 1, 1, totalcolumns).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Range(1, 1, 1, totalcolumns).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range(1, 1, 1, totalcolumns).Style.Font.SetBold(true);

                worksheet.Cell(2, 1).Value = "";
                #endregion

                int rowindex = 3;


                #region "Add Filters"

                if (exportDataModel.filters != null && exportDataModel.filters.Count > 0)
                {
                    worksheet.Cell(rowindex, 1).Value = "Filter Applied:";
                    foreach (var filter in exportDataModel.filters)
                    {
                        rowindex++;

                        worksheet.Cell(rowindex, 1).Value = filter.Key;
                        worksheet.Cell(rowindex, 2).Value = filter.Value;
                    }
                    rowindex++;
                    worksheet.Cell(rowindex, 1).Value = "";
                    rowindex++;
                }

                #endregion

                if (exportDataModel.data != null && exportDataModel.data.Rows.Count > 0)
                {
                    // Add DataTable content
                    worksheet.Cell(rowindex, 1).InsertTable(exportDataModel.data);

                    // Apply formatting based on data type
                    for (int i = 0; i < exportDataModel.data.Columns.Count; i++)
                    {
                        var column = exportDataModel.data.Columns[i];
                        var dataType = column.DataType;

                        if (dataType == typeof(DateTime))
                        {
                            worksheet.Column(i + 1).Style.DateFormat.Format = "yyyy-mm-dd";
                        }
                        else if (dataType == typeof(int) || dataType == typeof(long))
                        {
                            worksheet.Column(i + 1).Style.NumberFormat.Format = "#,##0";
                        }
                        else if (dataType == typeof(double) || dataType == typeof(float) || dataType == typeof(decimal))
                        {
                            worksheet.Column(i + 1).Style.NumberFormat.Format = "#,##0.00";
                        }
                        else if (dataType == typeof(TimeSpan))
                        {
                            worksheet.Column(i + 1).Style.NumberFormat.Format = "[h]:mm";
                        }
                    }
                }
                else
                {
                    // Add headers
                    for (int col = 0; col < propInfo.Length; col++)
                    {
                        var displayNameAttribute = propInfo[col].GetCustomAttribute<DisplayNameAttribute>();

                        // Get the display name if the attribute exists, otherwise use the property name
                        var displayName = displayNameAttribute != null ? displayNameAttribute.DisplayName : propInfo[col].Name;


                        worksheet.Cell(rowindex, col + 1).Value = displayName;
                        worksheet.Cell(rowindex, col + 1).Style.Font.Bold = true;
                    }

                    // Add data
                    int row = rowindex + 1;
                    foreach (var item in exportDataModel.Tdata)
                    {
                        for (int col = 0; col < propInfo.Length; col++)
                        {
                            var value = propInfo[col].GetValue(item);
                            worksheet.Cell(row, col + 1).Value = Convert.ToString(value);

                            // Apply formatting based on type
                            if (value is DateTime)
                            {
                                worksheet.Cell(row, col + 1).Style.DateFormat.Format = "yyyy-mm-dd";
                            }
                            else if (value is int || value is long || value is decimal)
                            {
                                worksheet.Cell(row, col + 1).Style.NumberFormat.Format = "#,##0";
                            }
                            else if (value is double || value is float || value is decimal)
                            {
                                worksheet.Cell(row, col + 1).Style.NumberFormat.Format = "#,##0.00";
                            }
                        }
                        row++;
                    }
                }

                // Adjust column widths
                //worksheet.Columns().AdjustToContents();

                // Save the workbook to a memory stream
                using var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);

                // Reset the memory stream position to the beginning
                memoryStream.Position = 0;

                errorMsg = string.Empty;
                return memoryStream.ToArray();

            }
            catch (Exception ex)
            {
                errorMsg = ex.ToString();
                return null;
            }
        }
    }
}
