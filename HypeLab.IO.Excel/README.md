[![NuGet](https://img.shields.io/nuget/v/HypeLab.IO.Excel.svg?style=flat-square)](https://www.nuget.org/packages/HypeLab.IO.Excel)
![Target Framework](https://img.shields.io/badge/target-.NET%20Standard%202.0-blue?style=flat-square)

# HypeLab.IO.Excel

**HypeLab.IO.Excel** is a powerful .NET library for reading, writing, and parsing Excel files using only the OpenXML standard, without external dependencies.

## Key Features

- ✅ Strongly-typed parsing with attributes support (`ExcelColumn`,`ExcelColumnIndex`,`ExcelIgnore`)
- 📥 Excel reading from file, stream or URL
- 🌍 Support for cultures, styles, custom dates, custom true/false etc
- 🛠 Supports both file and in-memory operations
- 🔍 Built-in validation and error logging
- 🧩 Compatible with .NET Standard 2.0

# 📚 Documentation
Full documentation, examples, and API reference available at:
👉 https://hype-lab.it/strumenti-per-sviluppatori/excel

## Installation

```bash
dotnet add package HypeLab.IO.Excel
```

## Getting Started

Read and write Excel files with just a few lines of code:

```csharp
// Reading a list of objects from Excel
ExcelSheetData sheetData = ExcelReader.ExtractSheetData(path, options, logger: logger); // and other methods

// Parsing the sheet data into strongly-typed objects
ExcelParseResult<MyModel> result = await ExcelParser.ParseToAsync<MyModel>(sheetData, options: options, logger: logger).ConfigureAwait(false); // and other methods

// Writing to Excel with default options
ExcelWorkbookWriter.WriteFile(pathOutput, worksheets, logger); // and other methods

```

# 🧩 Attributes
`[ExcelColumn("ColumnName")]` — Map property to a column by name

`[ExcelColumnIndex(2)]` — Map by the column index - Works on read only

`[ExcelIgnore(OnRead = true)]` — Ignore properties

You can also customize behavior using `ExcelReaderOptions` or `ExcelWriterOptions` for culture, sheet name, validation, and styling.

# 💬 Feedback & contribution
This library is intended to be useful to those who work frequently with Excel in a .NET environment.
Bugs, suggestions or ideas are welcome!