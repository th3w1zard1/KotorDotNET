﻿using KotorDotNET.FileFormats.Kotor2DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Patching.Modifiers.For2DA.Values
{
    /// <summary>
    /// Gets a value under a specific column.
    /// </summary>
    public class CellValue : IValue
    {
        /// <summary>
        /// The column that will be looked at.
        /// </summary>
        public string ColumnHeader { get; set; }

        public CellValue(string columnHeader)
        {
            ColumnHeader = columnHeader;
        }

        public string GetValue(Memory memory, TwoDA twoda, TwoDARow? row, string? columnHeader)
        {
            return row.GetCell(ColumnHeader);
        }
    }
}
