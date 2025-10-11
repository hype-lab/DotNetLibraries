using HypeLab.IO.Core.Data.Attributes.Excel;
using HypeLab.IO.Core.Data.Models.Excel;
using HypeLab.IO.Core.Data.Options.Impl.Excel;
using HypeLab.IO.Core.Exceptions;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HypeLab.IO.Core.Helpers.Excel
{
    public static class ExcelRowReaderHelper
    {
        private static class MapperCache<T> where T : class
        {
            public static readonly ConditionalWeakTable<IExcelRowReader, ExcelRowReaderInternal<T>> Table = new();
        }

        public static T? MapCurrentTo<T>(this IExcelRowReader reader, bool hasHeader, ExcelParserOptions options, ILogger? logger = null)
            where T : class
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            // prende (o crea) il mapper per questo reader
            if (!MapperCache<T>.Table.TryGetValue(reader, out ExcelRowReaderInternal<T>? wrapper))
            {
                wrapper = new ExcelRowReaderInternal<T>(reader, hasHeader, options, logger);
                MapperCache<T>.Table.Add(reader, wrapper);
            }

            // Mappa la riga corrente (gestisce lui il primo giro con header)
            return wrapper.MapCurrent();
        }

        // questa classe privata incapsula il mapping
        private sealed class ExcelRowReaderInternal<T>
            where T : class
        {
            private readonly IExcelRowReader _reader;
            private readonly ILogger? _logger;
            private readonly ExcelParserOptions _options;
            private readonly List<(PropertyInfo Prop, int Index)> _propertyMap = [];
            private bool _initialized;

            public ExcelRowReaderInternal(IExcelRowReader reader, bool hasHeader, ExcelParserOptions options, ILogger? logger)
            {
                _reader = reader;
                _logger = logger;
                _options = options;

                if (hasHeader)
                {
                    // Current è l'header: costruisci mappa e avanza alla prima riga dati
                    BuildMapFromHeader(_reader.Current);
                    _initialized = true;

                    // AVANZA: evita di mappare l'header come dato
                    if (!_reader.ReadNext())
                    {
                        // file con solo header
                        _initialized = true; // segnato, ma non ci sono righe
                    }
                }
                else
                {
                    // Senza header: mappa per index o per ordine proprietà
                    BuildMapWithoutHeader();
                    _initialized = true;
                }
            }

            public T? MapCurrent()
            {
                // Se non inizializzato per qualche motivo, fail safe
                if (!_initialized)
                {
                    BuildMapWithoutHeader();
                    _initialized = true;
                }

                string?[] row = _reader.Current;
                Type type = typeof(T);
                T? instance;
                try
                {
                    instance = (T?)Activator.CreateInstance(type, nonPublic: true);
                }
                catch (Exception ex)
                {
                    throw new ObjectCreationException($"Unable to create instance of type {type.FullName}.", ex);
                }

                if (instance == null)
                    throw new NullObjectException($"Failed to create instance of type {type.FullName}.");

                foreach ((PropertyInfo prop, int idx) in _propertyMap)
                {
                    if (idx < 0 || idx >= row.Length) continue;

                    string? cellValue = row[idx];
                    try
                    {
                        Type targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                        if (CommonFunctions.TryConvertCellValue(cellValue, targetType, prop.Name, _options, out object? converted))
                        {
                            prop.SetValue(instance, converted);
                        }
                        else
                        {
                            // a seconda delle policy puoi loggare o tirare
                            _logger?.LogWarning("Failed conversion for property {Prop} at col {Col} (row {Row}) value '{Val}'",
                                prop.Name, idx, _reader.RowIndex, cellValue);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "Error mapping col {Col} to property {Prop} (row {Row})", idx, prop.Name, _reader.RowIndex);
                        if (_options.ThrowOnParseError) throw;
                    }
                }

                return instance;
            }

            private void BuildMapFromHeader(string?[] header)
            {
                Dictionary<string, int> headerMap = new(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < header.Length; i++)
                {
                    string? name = header[i];
                    if (!string.IsNullOrWhiteSpace(name) && !headerMap.ContainsKey(name!))
                        headerMap[name!] = i;
                }

                PropertyInfo[] props = GetBindableProps();
                _propertyMap.Clear();

                foreach (PropertyInfo prop in props)
                {
                    // se la proprietà ha un attributo ExcelIgnore con OnRead=true, la ignoro
                    if (prop.GetCustomAttribute<ExcelIgnoreAttribute>()?.OnRead == true)
                        continue;

                    // index diretto vince sempre
                    int? ix = prop.GetCustomAttribute<ExcelColumnIndexAttribute>()?.Index;
                    if (ix is int fixedIndex && fixedIndex >= 0)
                    {
                        _propertyMap.Add((prop, fixedIndex));
                        continue;
                    }

                    // nome colonna
                    ExcelColumnAttribute? colAttr = prop.GetCustomAttribute<ExcelColumnAttribute>();
                    if (colAttr?.OnRead == true)
                    {
                        if (headerMap.TryGetValue(colAttr.Name, out int idx))
                            _propertyMap.Add((prop, idx));
                        else if (colAttr.ThrowExceptionIfNotFound)
                            throw new InvalidOperationException($"Column '{colAttr.Name}' not found in header.");

                        // else: ignora la prop
                        continue;
                    }

                    // fallback: prova il nome della property
                    if (headerMap.TryGetValue(prop.Name, out int idxByPropName))
                        _propertyMap.Add((prop, idxByPropName));
                }
            }

            private void BuildMapWithoutHeader()
            {
                PropertyInfo[] props = GetBindableProps();
                _propertyMap.Clear();

                bool hasIndexAttribute = props.Any(p => p.IsDefined(typeof(ExcelColumnIndexAttribute)));
                if (hasIndexAttribute)
                {
                    bool atLeastOnePropWithoutIndexAttribute = false;
                    foreach (PropertyInfo prop in props)
                    {
                        int? ix = prop.GetCustomAttribute<ExcelColumnIndexAttribute>()?.Index;
                        if (ix is int fixedIndex && fixedIndex >= 0)
                        {
                            _propertyMap.Add((prop, fixedIndex));
                        }
                        else
                        {
                            if (!atLeastOnePropWithoutIndexAttribute)
                                atLeastOnePropWithoutIndexAttribute = true;
                        }
                    }

                    if (atLeastOnePropWithoutIndexAttribute && _propertyMap.Count > 0)
                        throw new InvalidOperationException("Some properties do not have an ExcelColumnIndexAttribute. Consider adding indices.");
                }

                if (_propertyMap.Count == 0)
                {
                    throw new InvalidOperationException("No properties found with ExcelColumnIndexAttribute or ExcelColumnAttribute.");
                }
                else
                {
                    // fallback: mappa per ordine proprietà
                    int ordinal = 0;
                    foreach (PropertyInfo? prop in props.Where(p => !p.IsDefined(typeof(ExcelColumnIndexAttribute))))
                        _propertyMap.Add((prop, ordinal++));
                }
            }

            private static PropertyInfo[] GetBindableProps()
            {
                return [.. typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanWrite && !(p.GetCustomAttribute<ExcelIgnoreAttribute>()?.OnRead ?? false))];
            }
        }
    }
}
