using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// for IO operations
using System.IO;

namespace PerceptionLib
{
    public class CSV
    {
        
        /// Class to store one row in CSV 
        public class CsvRow : List<string>
        {
            public string LineText { get; set; }
        }

        
        /// Class to write data to a CSV file
        
        public class CsvFileWriter : StreamWriter
        {
            public CsvFileWriter(Stream stream): base(stream)
            {
            }

            public CsvFileWriter(string filename)
                : base(filename)

            {
            }

            
            /// Writes a single row to a CSV file.
            public void WriteRow(CsvRow row)
            {
                    // builds the whole line or row as a string
                    StringBuilder builder = new StringBuilder();
                    bool firstColumn = true;
                    foreach (string value in row)
                    {
                        // Add separator if this isn't the first value
                        if (!firstColumn)
                            builder.Append(',');
                    
                        // Appeneds the data
                        builder.Append(value);
                        firstColumn = false;
                    }
                    row.LineText = builder.ToString();
                    WriteLine(row.LineText);
                }
            }

        
        /// Class to read data from a CSV file
        public class CsvFileReader : StreamReader
        {
            public CsvFileReader(Stream stream)
                : base(stream)
            {
            }

            public CsvFileReader(string filename)
                : base(filename)
            {
            }

            
            /// Reads a row of data from a CSV file
            public bool ReadRow(CsvRow row)
            {
                row.LineText = ReadLine();
                if (String.IsNullOrEmpty(row.LineText))
                    return false;

                int pos = 0;
                int rows = 0;

                while (pos < row.LineText.Length)
                {
                    string value;

                    // Special handling for quoted field
                    if (row.LineText[pos] == '"')
                    {
                        // Skip initial quote
                        pos++;

                        // Parse quoted value
                        int start = pos;
                        while (pos < row.LineText.Length)
                        {
                            // Test for quote character
                            if (row.LineText[pos] == '"')
                            {
                                // Found one
                                pos++;

                                // If two quotes together, keep one
                                // Otherwise, indicates end of value
                                if (pos >= row.LineText.Length || row.LineText[pos] != '"')
                                {
                                    pos--;
                                    break;
                                }
                            }
                            pos++;
                        }
                        value = row.LineText.Substring(start, pos - start);
                        value = value.Replace("\"\"", "\"");
                    }
                    else
                    {
                        // Parse unquoted value
                        int start = pos;
                        while (pos < row.LineText.Length && row.LineText[pos] != ',')
                            pos++;
                        value = row.LineText.Substring(start, pos - start);
                    }

                    // Add field to list
                    if (rows < row.Count)
                        row[rows] = value;
                    else
                        row.Add(value);
                    rows++;

                    // Eat up to and including next comma
                    while (pos < row.LineText.Length && row.LineText[pos] != ',')
                        pos++;
                    if (pos < row.LineText.Length)
                        pos++;
                }
                // Delete any unused items
                while (row.Count > rows)
                    row.RemoveAt(rows);

                // Return true if any columns read
                return (row.Count > 0);
            }
        }
    }
}
