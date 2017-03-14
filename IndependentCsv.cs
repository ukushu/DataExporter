using System.Linq;

namespace BotAgent.DataExporter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// My old slow, buggy class... Works indepentently, don't need external references.
    /// Perfect for use in Unity for some simple needs.
    /// 
    /// In any case, better not use this class without some exact reason. 
    /// Please, use Csv class instead!
    /// 
    /// Possibly will be rewriten in future... Possibly... :)
    /// </summary>
    public class IndependentCsv
    {
        public List<List<string>> Rows = new List<List<string>>();

        public char SeparatorChar;

        /// <summary>
        /// Init Csv file editor
        /// </summary>
        /// <param name="separatorChar">Char separator on FileRead and FileSave</param>
        public IndependentCsv(char separatorChar=',')
        {
            SeparatorChar = separatorChar;
        }

        public void FileOpen(string path)
        {
            try
            {
                StreamReader csvRdr = new StreamReader(path);
                List<string> tmpRow;

                Rows.Clear();

                string text = csvRdr.ReadToEnd();

                var tmpCsv = new List<string>(
                    text.Split(new string[] { "\r\n" },
                        StringSplitOptions.None));

                for (int i = 0; i < tmpCsv.Count; i++)
                {
                    RecursivelyRowFix(tmpCsv, i);
                }

                //DO NOT COMBINE WITH PREVIUS LOOP!
                for (int i = 0; i < tmpCsv.Count; i++)
                {
                    tmpRow = ReadListFromScvRow(tmpCsv[i]);

                    if (!(tmpRow.Count == 1 && tmpRow[0].Length == 0))
                    {
                        Rows.Add(tmpRow);
                    }
                }

                csvRdr.Close();
            }
            catch (Exception)
            {
                //// File not exist or this is non CSV
            }
        }

        private void FileSave(string path, bool appendFile = false)
        {
            var correctPath = Directory.GetParent(path).ToString();

            Directory.CreateDirectory(correctPath);

            using (StreamWriter writer = new StreamWriter(path, appendFile, Encoding.UTF8))
            {
                for (int i = 0; i < Rows.Count; i++)
                {
                    writer.WriteLine(BuildCsvRow(Rows[i].ToArray()));
                }
            }
        }

        public void AddRow(params string[] cells)
        {
            Rows.Add(new List<string>(cells));
        }

        private List<string> ReadListFromScvRow(string row)
        {
            if (row == string.Empty)
            {
                return new List<string>();
            }

            var charToSearch = '"';
            var foundIndexes = new List<int>();
            var indexesToRemove = new List<int>();
            List<string> rezult = new List<string>();

            //// Search for quotes char indexes
            for (int i = row.IndexOf(charToSearch); i > -1; i = row.IndexOf(charToSearch, i + 1))
            {
                foundIndexes.Add(i);
            }

            //// Check for /"/"
            var indexCounter = foundIndexes.Count - 1;
            for (int i = 0; i < indexCounter; i++)
            {
                if (foundIndexes[i + 1] - foundIndexes[i] == 1)
                {
                    indexesToRemove.Add(i);
                    indexesToRemove.Add(i + 1);
                }
            }

            ////  and miss them
            for (int i = indexesToRemove.Count - 1; i >= 0; i--)
            {
                foundIndexes.RemoveAt(indexesToRemove[i]);
            }

            //// Add first and last symbols indexes if needed
            if (foundIndexes.Count > 0)
            {
                if (foundIndexes[0] != 0)
                {
                    foundIndexes.Insert(0, 0);
                }
                if (foundIndexes[foundIndexes.Count - 1] != row.Length - 1)
                {
                    foundIndexes.Add(row.Length);
                }
            }
            else
            {
                foundIndexes.Insert(0, 0);
                foundIndexes.Add(row.Length);
            }

            //// Divide to subLines
            var subLinesToAnalize = foundIndexes.Count - 1;
            for (int i = 0; i < subLinesToAnalize; i++)
            {
                var offset = 0;

                if (row[0] == '"')
                {
                    if (i % 2 == 0)
                        offset = 1;
                    else
                        offset = 0;
                }
                else
                {
                    if (i % 2 == 0)
                        offset = 0;
                    else
                        offset = 1;
                }

                var subStrStart = foundIndexes[i];
                var subStrLength = foundIndexes[i + 1] + offset - subStrStart;

                var subStr = row.Substring(subStrStart, subStrLength);

                if (subStr.Count(x => x == '"') == 1 && subStr[0] == '\"')
                {
                    subStr = subStr.TrimStart('\"');
                }

                //// If subline contains \"\" 
                if (subStr.Contains("\""))
                {
                    subStr = subStr.Replace("\"\"", "\"").TrimStart('\"').TrimEnd('\"').TrimStart(SeparatorChar).TrimEnd(SeparatorChar);

                    rezult.Add(subStr);
                }
                else
                {
                    subStr = subStr.TrimStart(SeparatorChar).TrimEnd(SeparatorChar);

                    string[] tmp = subStr.Split(SeparatorChar);

                    foreach (string cell in tmp)
                    {
                        rezult.Add(cell);
                    }
                }
            }

            return rezult;
        }

        private string BuildCsvRow(params string[] rowCells)
        {
            StringBuilder builder = new StringBuilder();

            bool firstColumn = true;

            foreach (string value in rowCells)
            {
                if (value != null)
                {
                    // Add separator if this isn't the first value
                    if (!firstColumn)
                        builder.Append(SeparatorChar);

                    // Implement special handling for values that contain comma or quote
                    // Enclose in quotes and double up any double quotes
                    if (value.IndexOfAny(new char[] { '"', SeparatorChar }) != -1)
                    {


                        builder.AppendFormat("\"{0}\"", value.Replace("\"", "\"\""));
                    }
                    else
                    {
                        builder.Append(value);
                    }

                    firstColumn = false;
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// Some Cells can consist of lot of strings (there will be lot of "\r\n"). 
        /// Here we combine those strings into one text line;
        /// </summary>
        private void RecursivelyRowFix(List<string> csvFileRows, int index)
        {
            const char charToSearch = '"';

            var quotesCounter = csvFileRows[index].Split(charToSearch).Length - 1;

            if (quotesCounter % 2 != 0)
            {
                csvFileRows[index] += "\r\n" + csvFileRows[index + 1];

                csvFileRows.RemoveAt(index + 1);

                RecursivelyRowFix(csvFileRows, index);
            }
        }
    }
}