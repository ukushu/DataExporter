# DataExporter

Most easy way to work with ```*.csv```, ```*.xlsx```, ```*.ini``` files

# Excel Worker

**Description:**

With this simple class you will be able to work with ```*.xlsx``` files as with 2-dimension array; What can be more simple than this?

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

**Why I had chose exactly this way to interact with ```xlsx``` files?**

This info can be useful in case of you need sth MORE than my class functionality. There is exist few ways to interact with Excel files. Let's check my short experience with them:

* Microsoft.Office.Interop.Excel

  * Excel must be installed on the machine
  * Extremally slow (11 250 cells will be saved in 22 sec)
  * Lot of memory leaks
  * Hard to write code. In case of some code mistakes you will leave background runned Excel process.

* OleDB
  * No ability to work with formulas (you will be able only to read value from cell, but not a formula)
  * Fast
  * Easy in use

* OpenXml
  * Hard in use/ Code is not easy readeble
  * Fast

* ClosedXml ( wrapper around OpenXml ) [used in my lib]
  * Easy in use
  * Fast. In 57 times faster than Interop  (20 000 cells will be saved in 00:00:00.6787608 sec)

# Csv Worker

**Description:**

With this simple class you will be able to work with ```csv```, ```tsv``` and other ```*sv``` files as with 2-dimension array; What can be more simple than this?

This class uses standard ```Microsoft.VisualBasic.FileIO``` VS library. This library have 2 weak point's that you need to know:

* It's works only in standard VS projects (as example you cannot use it in Unity project)
* It doesnt work with large files (I have lame hack for this -- separate csv to few files). But most of users will not see such behavior :)

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

Simple class to work with ```ini``` files. Ini files data saved with the following dara: "Section", "key" and "value".

It's slow because of it's based on 'kernel32.dll' methods. If you need to read/write lot of data better to use some another library. But in another case its a good way.


**Example usage:**

```
Ini ini = new Ini(filePath);

ini.Write("SomeKey", "SomeValue");// Will be saved to "Default" section
ini.Write("SomeSection", "SomeKey", "SomeValue");

var a = ini.Read("SomeKey"); // will read SomeKey value from "Default" section
var b = ini.Read("SomeSection", "SomeKey", "SomeValue");
```
