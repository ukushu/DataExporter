# DataExporter

Most easy way to work with ```*.csv```, ```*.xlsx```, ```*.ini``` files

This is minimalistic classes. You will be able only to work with ```*.csv```, ```*.xlsx``` -- as with 2-dimensions arrays. What can be more simple than this?

# Excel Worker

**Example usage:**


```
Excel xl = new Excel();

xl.FileOpen("c:\\file1.xlsx");

var row1Cell6Value = xl.Rows[0][5];

xl.AddRow("asdf","asdffffff","5")

xl.FileSave("c:\\file2.xlsx");
```

Work with formulas:

```
var widthAdress = Excel.GetExcelPos(0, 1);
var heightAdress = Excel.GetExcelPos(0, 2);

xl.Rows[0][0] = String.Format("={0}*{1}", widthAdress , heightAdress);
```

# Csv Worker

**Description:**

With this simple class you will be able to work with ```csv```, ```tsv``` and other ```*sv``` files

**Example usage:**

```
Csv csv = new Csv();

csv.FileOpen("c:\\file1.csv");

var row1Cell6Value = csv.Rows[0][5];

csv.AddRow("asdf","asdffffff","5")

csv.FileSave("c:\\file2.csv");
```

Work with ```tsv``` files (exaple of usage another separator):

```
Csv csv = new Csv('\t');
```

# Ini Worker

**Description:**

Simple class to work with ```ini``` files. Ini files data saved with the following dara: "Section", "key" and "value"

**Example usage:**

```
Ini ini = new Ini(filePath);

ini.Write("SomeKey", "SomeValue");// Will be saved to "Default" section
ini.Write("SomeSection", "SomeKey", "SomeValue");

var a = ini.Read("SomeKey"); // will read SomeKey value from "Default" section
var b = ini.Read("SomeSection", "SomeKey", "SomeValue");
```
