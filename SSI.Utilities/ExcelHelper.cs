using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;

namespace SSI.Utilities
{
    public static class ExcelHelper
    {
        private static string tempPath = (ConfigHelper.AppSettings("UploadRoot") ?? "~/Upload/") + "Temp/";
        public static DataSet ImportToDataSet(string excelPath, bool firstRowAsHeader = true)
        {
            FileStream stream = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
            return ImportToDataSet(stream, firstRowAsHeader);
        }

        public static DataSet ImportToDataSet(Stream stream, bool firstRowAsHeader = true)
        {
            DataSet ds = new DataSet();
            IWorkbook workbook = WorkbookFactory.Create(stream);
            IFormulaEvaluator evaluator = WorkbookFactory.CreateFormulaEvaluator(workbook);
            int sheetCount = workbook.NumberOfSheets;

            for (int i = 0; i < sheetCount; i++)
            {
                ISheet sheet = workbook.GetSheetAt(i);
                DataTable dt = ExcelToDataTable(sheet, evaluator, firstRowAsHeader);
                ds.Tables.Add(dt);
            }
            return ds;
        }

        public static DataTable ImportToDataTable(string excelPath, bool firstRowAsHeader = true, string sheetName = "Sheet1")
        {
            FileStream stream = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
            return ImportToDataTable(stream, firstRowAsHeader, sheetName);
        }

        public static DataTable ImportToDataTable(Stream stream, bool firstRowAsHeader = true, string sheetName = "Sheet1")
        {
            IWorkbook workbook = WorkbookFactory.Create(stream);
            IFormulaEvaluator evaluator = WorkbookFactory.CreateFormulaEvaluator(workbook);
            ISheet sheet = workbook.GetSheet(sheetName);
            return ExcelToDataTable(sheet, evaluator, firstRowAsHeader);
        }

        private static DataTable ExcelToDataTable(ISheet sheet, IFormulaEvaluator evaluator, bool firstRowAsHeader)
        {
            if (firstRowAsHeader)
            {
                return ExcelToDataTableFirstRowAsHeader(sheet, evaluator);
            }
            else
            {
                return ExcelToDataTable(sheet, evaluator);
            }
        }

        private static DataTable ExcelToDataTableFirstRowAsHeader(ISheet sheet, IFormulaEvaluator evaluator)
        {
            using (DataTable dt = new DataTable())
            {
                IRow firstRow = sheet.GetRow(0);
                int cellCount = GetCellCount(sheet);
                for (int i = 0; i < cellCount; i++)
                {
                    if (firstRow.GetCell(i) != null)
                    {
                        dt.Columns.Add(firstRow.GetCell(i).StringCellValue ?? string.Format("F{0}", i + 1), typeof(string));
                    }
                    else
                    {
                        dt.Columns.Add(string.Format("F{0}", i + 1), typeof(string));
                    }
                }

                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dr = dt.NewRow();
                    FillDataRowByRow(row, evaluator, ref dr);
                    dt.Rows.Add(dr);
                }
                dt.TableName = sheet.SheetName;
                return dt;
            }
        }

        private static DataTable ExcelToDataTable(ISheet sheet, IFormulaEvaluator evaluator)
        {
            using (DataTable dt = new DataTable())
            {
                if (sheet.LastRowNum > 0)
                {
                    int cellCount = GetCellCount(sheet);
                    for (int i = 0; i < cellCount; i++)
                    {
                        dt.Columns.Add(string.Format("F{0}", i), typeof(string));
                    }

                    for (int i = 0; i < sheet.FirstRowNum; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dt.Rows.Add(dr);
                    }

                    for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        DataRow dr = dt.NewRow();
                        FillDataRowByRow(row, evaluator, ref dr);
                        dt.Rows.Add(dr);
                    }
                }
                dt.TableName = sheet.SheetName;
                return dt;
            }
        }

        private static void FillDataRowByRow(IRow row, IFormulaEvaluator evaluator, ref DataRow dr)
        {
            if (row != null)
            {
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    ICell cell = row.GetCell(i);
                    if (cell != null)
                    {
                        dr[i] = GetCellObject(evaluator, cell);
                    }
                }
            }
        }

        private static object GetCellObject(IFormulaEvaluator evaluator, ICell cell)
        {
            object obj = null;
            switch (cell.CellType)
            {
                case CellType.Blank:
                    {
                        obj = DBNull.Value;
                        break;
                    }
                case CellType.Boolean:
                    {
                        obj = cell.BooleanCellValue;
                        break;
                    }
                case CellType.Numeric:
                    {
                        if (DateUtil.IsCellDateFormatted(cell))
                        {
                            obj = cell.DateCellValue;
                        }
                        else
                        {
                            obj = cell.NumericCellValue;
                        }
                        break;
                    }
                case CellType.String:
                    {
                        obj = cell.StringCellValue;
                        break;
                    }
                case CellType.Error:
                    {
                        obj = cell.ErrorCellValue;
                        break;
                    }
                case CellType.Formula:
                    {
                        obj = GetCellObject(evaluator, evaluator.EvaluateInCell(cell));
                        break;
                    }
                default:
                    throw new NotSupportedException(string.Format("类型错误:{0}", cell.CellType));
            }
            return obj;
        }

        private static int GetCellCount(ISheet sheet)
        {
            int cellCount = 0;
            for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row != null && row.LastCellNum > cellCount)
                {
                    cellCount = row.LastCellNum;
                }
            }
            return cellCount;
        }


        public static byte[] ExportToContent(DataTable data, bool isXLSX = false, string sheetName = "Sheet1", string sheetTitle = "")
        {
            using (MemoryStream stream = new MemoryStream())
            {
                IWorkbook workbook = GetWorkbook(data, isXLSX, sheetName, sheetTitle);
                workbook.Write(stream);
                return stream.ToArray();
            }
        }

        public static string ExportToFile(DataTable data, bool isXLSX = false, string sheetName = "Sheet1", string sheetTitle = "")
        {
            string fileName = Guid.NewGuid().ToString("N") + (isXLSX ? ".xlsx" : ".xls");
            string excelPath = HttpContext.Current.Server.MapPath(tempPath + fileName);
            using (FileStream stream = File.OpenWrite(excelPath))
            {
                IWorkbook workbook = GetWorkbook(data, isXLSX, sheetName, sheetTitle);
                workbook.Write(stream);
                return excelPath;
            }
        }

        private static IWorkbook GetWorkbook(DataTable data, bool isXLSX, string sheetName, string sheetTitle = "")
        {
            IWorkbook workbook = isXLSX ? new XSSFWorkbook() : new HSSFWorkbook() as IWorkbook;
            ISheet sheet = workbook.CreateSheet(sheetName);
            int indexRow = 0;

            if (!string.IsNullOrEmpty(sheetTitle))
            {
                IRow titleRow = sheet.CreateRow(indexRow);
                titleRow.HeightInPoints = 20;
                IFont titleFont = workbook.CreateFont();
                titleFont.FontName = "黑体";
                titleFont.FontHeightInPoints = 16;
                titleFont.IsBold = true;
                ICellStyle titleStyle = workbook.CreateCellStyle();
                titleStyle.SetFont(titleFont);
                titleStyle.Alignment = HorizontalAlignment.Center;
                titleStyle.VerticalAlignment = VerticalAlignment.Center;
                ICell titleCell = titleRow.CreateCell(0, CellType.String);
                titleCell.SetCellValue(sheetTitle);
                titleCell.CellStyle = titleStyle;
                sheet.AddMergedRegion(new CellRangeAddress(indexRow, indexRow, 0, data.Columns.Count - 1));
                indexRow++;
            }

            IRow headRow = sheet.CreateRow(indexRow);
            IFont headFont = workbook.CreateFont();
            headFont.FontName = "宋体";
            headFont.FontHeightInPoints = 10;
            headFont.IsBold = true;
            ICellStyle headStyle = workbook.CreateCellStyle();
            headStyle.SetFont(headFont);
            headStyle.BorderTop = BorderStyle.Thin;
            headStyle.BorderRight = BorderStyle.Thin;
            headStyle.BorderBottom = BorderStyle.Thin;
            headStyle.BorderLeft = BorderStyle.Thin;
            headStyle.FillForegroundColor = 22;
            headStyle.FillPattern = FillPattern.SolidForeground;

            for (int i = 0; i < data.Columns.Count; i++)
            {
                ICell headCell = headRow.CreateCell(i, CellType.String);
                headCell.SetCellValue(data.Columns[i].ColumnName);
                headCell.CellStyle = headStyle;
            }

            indexRow++;

            IFont dataFont = workbook.CreateFont();
            dataFont.FontName = "宋体";
            dataFont.FontHeightInPoints = 9;
            ICellStyle dataStyle = workbook.CreateCellStyle();
            dataStyle.SetFont(dataFont);
            dataStyle.BorderTop = BorderStyle.Thin;
            dataStyle.BorderRight = BorderStyle.Thin;
            dataStyle.BorderBottom = BorderStyle.Thin;
            dataStyle.BorderLeft = BorderStyle.Thin;

            for (int i = 0; i < data.Rows.Count; i++)
            {
                IRow dataRow = sheet.CreateRow(indexRow + i);
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    ICell dataCell = dataRow.CreateCell(j, CellType.String);
                    dataCell.SetCellValue(data.Rows[i][j].ToString());
                    dataCell.CellStyle = dataStyle;
                }
            }

            indexRow--;

            autoColumnWidth(sheet, new CellRangeAddress(indexRow, sheet.LastRowNum, sheet.GetRow(indexRow).FirstCellNum, sheet.GetRow(indexRow).LastCellNum));

            return workbook;
        }

        private static void autoColumnWidth(ISheet sheet, CellRangeAddress cellRangeAddress)
        {
            for (int c = cellRangeAddress.FirstColumn; c <= cellRangeAddress.LastColumn; c++)
            {
                int columnWidth = sheet.GetColumnWidth(c) / 256;
                for (int r = cellRangeAddress.FirstRow; r <= cellRangeAddress.LastRow; r++)
                {
                    ICell cell = sheet.GetRow(r).GetCell(c);
                    if (cell != null)
                    {
                        int stringLength = Encoding.Default.GetBytes(cell.StringCellValue).Length;
                        if (columnWidth < stringLength)
                        {
                            columnWidth = stringLength;
                        }
                    }
                }
                if (columnWidth > 255)
                {
                    sheet.SetColumnWidth(c, 255 * 256);
                }
                else
                {
                    sheet.SetColumnWidth(c, columnWidth * 256 + 200);
                }
            }
        }
    }
}


