using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClosedXML.Excel;

namespace BotAgent.DataExporter
{
    public class Excel
    {
        public List<List<string>> Rows = new List<List<string>>();

        public void FileOpen(string path)
        {

            var workbook = new XLWorkbook(path);
            var ws1 = workbook.Worksheet(1);

            foreach (var xlRow in ws1.RangeUsed().Rows())
            {
                Rows.Add(new List<string>());

                foreach (var xlCell in xlRow.Cells())
                {
                    var formula = xlCell.FormulaA1;
                    var value = xlCell.Value.ToString();

                    string targetCellValue = (formula.Length == 0) ? value : "=" + formula;

                    Rows[Rows.Count - 1].Add(targetCellValue);
                }
            }
        }

        public void FileSave(string path)
        {
            CreateDirIfNotExist(path, true);

            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("Sample Sheet");

                for (int row = 0; row < Rows.Count; row++)
                {
                    for (int col = 0; col < Rows[row].Count; col++)
                    {
                        var cellAdress = GetExcelPos(row, col);

                        if (Rows[row][col].StartsWith("="))
                        {
                            workSheet.Cell(cellAdress).FormulaA1 = Rows[row][col];
                        }
                        else
                        {
                            workSheet.Cell(cellAdress).Value = Rows[row][col];
                        }
                    }
                }

                wb.SaveAs(path);
            }
        }

        public void AddRow(params string[] cells)
        {
            Rows.Add(cells.ToList());
        }

        public static string GetExcelPos(int row, int cell)
        {
            char[] alph = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            int count = cell / 26;
            string alphResult = string.Empty;

            if (count > 0)
            {
                alphResult = alph[count] + alph[count % 26].ToString();
            }
            else
            {
                alphResult = alph[cell].ToString();
            }

            return alphResult + (row + 1);
        }

        private void CreateDirIfNotExist(string dirPath, bool removeFilename = false)
        {
            if (removeFilename)
            {
                dirPath = Directory.GetParent(dirPath).FullName;
            }

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }
    }
}