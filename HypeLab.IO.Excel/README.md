[![NuGet](https://img.shields.io/nuget/v/HypeLab.IO.Excel.svg?style=flat-square)](https://www.nuget.org/packages/HypeLab.IO.Excel)
![Target Framework](https://img.shields.io/badge/target-.NET%20Standard%202.0-blue?style=flat-square)

# [v1.0.7] Added New API in reading / parsing
- New API for reading Excel files from a Stream (`ExcelSheetData ExtractSheetData(Stream stream, ExcelReaderOptions? options = null, ILogger? logger = null);`)

- New APIs for parsing Excel files into strongly-typed objects directyl from the input file path, a byte array or a stream:
```csharp
// Synchronous
ExcelParseResult<T> ParseTo<T>(string filePath, ExcelImportOptions? options = null, ILogger? logger = null);
ExcelParseResult<T> ParseTo<T>(byte[] fileBytes, ExcelImportOptions? options = null, ILogger? logger = null);
ExcelParseResult<T> ParseTo<T>(Stream stream, ExcelImportOptions? options = null, ILogger? logger = null);

// Asynchronous
Task<ExcelParseResult<T>> ParseToAsync<T>(string filePath, ExcelImportOptions? options = null, ILogger? logger = null, CancellationToken cancellationToken = default);
Task<ExcelParseResult<T>> ParseToAsync<T>(byte[] fileBytes, ExcelImportOptions? options = null, ILogger? logger = null, CancellationToken cancellationToken = default);
Task<ExcelParseResult<T>> ParseToAsync<T>(Stream stream, ExcelImportOptions? options = null, ILogger? logger = null, CancellationToken cancellationToken = default);
```

## Other improvements
- Added (deactivatable) rows length normalization to `ExcelSheetData` with respect to header length;
- Added exception throwing management in ExcelReader;
- Converted sheet data rows and columns arrays to nullable string from non-nullable string;
- Added logger support in ExcelValidator methods;
- Updated NuGets.

# ---

## History of benchmark results:
New benchmark made by me using the same 65,000+ rows Excel file, made with `BenchmarkDotNet`:

### Optimizations to parsing methods
Test made using the `ExcelSheetData` extracted from the `HypeLabXlsx_ExtractSheetData` method used previously

Starting point:
```
| Method              | Mean     | Error   | StdDev  | Gen0     | Gen1     | Gen2   | Allocated |
|---------------------|----------|---------|---------|----------|----------|--------|-----------|
| HypeLabXlsx_BindT   | 140.2 ms | 2.79 ms | 5.95 ms | 20250.0  | 5500.0   | 250.0  | 120.54 MB |
```

07/06/2025:
```
| Method              | Mean     | Error   | StdDev  | Gen0     | Gen1     | Gen2   | Allocated |
|---------------------|----------|---------|---------|----------|----------|--------|-----------|
| HypeLabXlsx_BindT   | 112.5 ms | 1.92 ms | 1.80 ms | 4800.0   | 2000.0   | 600.0  | 26.54 MB  |

| Method                   | Mean     | Error   | StdDev  | Gen0     | Gen1     | Gen2   | Allocated |
|--------------------------|----------|---------|---------|----------|----------|--------|-----------|
| HypeLabXlsx_BindTAsync   | 106.2 ms | 2.00 ms | 1.87 ms | 4600.0   | 1600.0   | 200.0  | 28.2 MB   |
```

### Optimizations to reading methods
`ExcelReader.ExtractSheetData` improvements from the reddit discussion [here](https://www.reddit.com/r/dotnet/comments/1lput9r/i_have_released_a_nuget_package_to_readwrite/).

Starting benchmark with `HypeLabXlsx_ExtractSheetData` method made by `u/@MarkPflug`
```
| Method                | Mean       | Error    | Ratio | Allocated    | Alloc Ratio |
|-----------------------|------------|----------|-------|--------------|-------------|
| Baseline              | 190.0 ms   | 5.45 ms  | 1.00  | 243.9 KB     | 1.00        |
| SylvanXlsx            | 292.7 ms   | 3.59 ms  | 1.54  | 659.98 KB    | 2.71        |
| ExcelDataReaderXlsx   | 941.7 ms   | 17.66 ms | 4.96  | 353883.76 KB | 1,450.95    |
| HypeLabXlsx_SheetData | 1,193.8 ms | 20.94 ms | 6.28  | 459799.31 KB | 1,885.21    |
| OpenXmlXlsx           | 2,669.5 ms | 44.03 ms | 14.05 | 502498.45 KB | 2,060.28    |
```

07/04/2025 (Second optimization):
```
| Method                           | Mean     | Error   | StdDev   | Gen0      | Gen1     | Gen2     | Allocated  |	
|----------------------------------|----------|---------|----------|-----------|----------|----------|------------|
| HypeLabXlsx_ExtractSheetData     | 306.8 ms | 5.82 ms | 6.47  ms | 16000.000 | 6500.000 | 2000.000 | *88.23 MB* |
| HypeLabXlsx_ExtractSheetData     | 309.6 ms | 6.10 ms | 8.14  ms | 16000.000 | 6500.000 | 2000.000 | *88.23 MB* |
| HypeLabXlsx_ExtractSheetData     | 298.6 ms | 5.64 ms | 5.27  ms | 16000.000 | 6500.000 | 2000.000 | *88.23 MB* |
```
In this last benchmark, Mean dropped further, also the mean of Error and StdDev slightly decreased. Starting from 1 second circa, now the mean is around 300 ms, which is a great improvement.
But the most important thing is that the allocated memory is now around 88 MB, which is almost half of the previous benchmark (around 228 MB).
So happy for these results, i see i can still improve it further, but for now this is a great result, considering the data is not streamed, but still materialized in memory as `List<string[]>`, which is a simple and easy to use API for the users of this library.

07/04/2025 (First optimization):
```
| Method                           | Mean     | Error   | StdDev   | Gen0      | Gen1     | Gen2     | Allocated |
|----------------------------------|----------|---------|----------|-----------|----------|----------|-----------|
| HypeLabXlsx_ExtractSheetData     | 370.8 ms | 6.14 ms | 12.67 ms | 41000.000 | 12000.000| 4000.000 | 228.24 MB |
| HypeLabXlsx_ExtractSheetData     | 358.9 ms | 7.07 ms | 6.94  ms | 41000.000 | 12000.000| 4000.000 | 228.24 MB |
| HypeLabXlsx_ExtractSheetData     | 367.6 ms | 7.21 ms | 9.37  ms | 41000.000 | 12000.000| 4000.000 | 228.24 MB |
```

07/03/2025:
```
| Method                           | Mean     | Error  | StdDev | Gen0     | Gen1     | Gen2    | Allocated	|
|----------------------------------|----------|--------|--------|----------|----------|---------|-----------|
| HypeLabXlsx_ExtractSheetData     | 542.5 ms | 8.39 ms| 7.44 ms| 53000.000| 17000.000| 9000.000| 275.25 MB |
| HypeLabXlsx_ExtractSheetData     | 538.4 ms | 1.63 ms| 1.52 ms| 53000.000| 17000.000| 9000.000| 275.25 MB |
| HypeLabXlsx_ExtractSheetData     | 549.6 ms | 6.78 ms| 6.34 ms| 54000.000| 18000.000| 9000.000| 275.25 MB |

```

Some improvements were made to the `HypeLabXlsx_ExtractSheetData` method, which is now faster than before.
Allocated memory almost halved, but it's still higher. I'm trying to keep `ExcelSheetData` with the raw rows as `List<string[]>`, working with a stream of course could improve it even more, but I wanted to also keep the API simple and easy to use.
Btw i'm still trying to reduce memory allocation.


# HypeLab.IO.Excel

**HypeLab.IO.Excel** is a powerful .NET library for reading, writing, and parsing Excel files using only the OpenXML standard, without external dependencies.

## Important note
Version 1.0.4 **fixed a styling bug** that caused the library to not work correctly with Excel files that had styles applied on multiple sheets. **Older versions are going to be deprecated soon**.

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