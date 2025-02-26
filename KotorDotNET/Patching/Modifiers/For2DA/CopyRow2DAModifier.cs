﻿using KotorDotNET.FileFormats.Kotor2DA;
using KotorDotNET.Patching.Modifiers.For2DA.Targets;
using KotorDotNET.Patching.Modifiers.For2DA.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Patching.Modifiers.For2DA
{
    /// <summary>
    /// Used to copy a row in a TwoDA instance.
    /// </summary>
    public class CopyRow2DAModifier
    {
        /// <summary>
        /// If assigned a non-null value, a new row will only be inserted if there
        /// is no existing row containing the the same value as this column. If a
        /// match is found then the existing row is to be modified instead. 
        /// </summary>
        public string? ExclusiveColumn { get; set; }
        /// <summary>
        /// The value generator to be used to get the row header. If this value is
        /// null then the row index will be used instead.
        /// </summary>
        public IValue? RowLabel { get; set; }
        /// <summary>
        /// The target row to copy.
        /// </summary>
        public ITarget ITarget { get; set; }
        /// <summary>
        /// Maps column headers (Keys) to cell values.
        /// </summary>
        public Dictionary<string, IValue> Data { get; set; }
        /// <summary>
        /// Maps token IDs (Keys) to values.
        /// </summary>
        public Dictionary<int, IValue> ToStoreInMemory { get; set; }

        public CopyRow2DAModifier(ITarget target, string? exclusiveColumn, IValue? rowLabel, Dictionary<string, IValue> data, Dictionary<int, IValue> toStore)
        {
            ITarget = target;
            ExclusiveColumn = exclusiveColumn;
            RowLabel = rowLabel;
            Data = data;
            ToStoreInMemory = toStore;
        }

        public void Apply(TwoDA twoda, Memory memory, Logger logger)
        {
            TwoDARow source = ITarget.Search(twoda);

            TwoDARow target;
            string RowLabelIfNew = twoda.Rows().Count.ToString();

            if (ExclusiveColumn == null)
            {
                target = twoda.AddRow(RowLabelIfNew);
            }
            else
            {
                if (twoda._headers.Contains(ExclusiveColumn) && Data.Keys.Contains(ExclusiveColumn))
                {
                    var cells = twoda.GetCellsUnderColumn(ExclusiveColumn).ToList();
                    var exclusiveValue = Data[ExclusiveColumn].GetValue(memory, twoda, null, null);

                    if (cells.Contains(exclusiveValue))
                    {
                        target = twoda.Rows()[cells.IndexOf(exclusiveValue)];
                    }
                    else
                    {
                        target = twoda.AddRow(RowLabelIfNew);
                    }
                }
                else
                {
                    target = twoda.AddRow(RowLabelIfNew);
                }
            }

            if (RowLabel != null)
            {
                target.Header = RowLabel.GetValue(memory, twoda, target, null);
            }

            foreach (var header in twoda.ColumnHeaders())
            {
                var value = source.GetCell(header);
                target.SetCell(header, value);
            }

            foreach (var pair in Data)
            {
                var columnHeader = pair.Key;
                var valuer = pair.Value;

                if (twoda.ColumnHeaders().Contains(columnHeader))
                {
                    var value = valuer.GetValue(memory, twoda, target, columnHeader);
                    target.SetCell(columnHeader, value);
                }
            }

            foreach (var toStore in ToStoreInMemory)
            {
                var value = toStore.Value.GetValue(memory, twoda, target, null);
                memory.Set2DAToken(toStore.Key, value);
            }
        }
    }
}
