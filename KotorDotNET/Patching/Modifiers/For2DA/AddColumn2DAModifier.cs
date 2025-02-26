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
    /// Used to insert a new column into a TwoDA instance.
    /// </summary>
    public class AddColumn2DAModifier : IModifier<TwoDA>
    {
        /// <summary>
        /// The header for the column that is to be inserted.
        /// </summary>
        public string ColumnHeader { get; }
        /// <summary>
        /// The default value for all the cells under the new column.
        /// </summary>
        public string DefaultValue { get; }

        public Dictionary<ITarget, IValue> Values { get; set; }

        public Dictionary<int, IValue> ToStoreInMemory { get; set; }


        public AddColumn2DAModifier(string header, string defaultValue, Dictionary<ITarget, IValue> values,
            Dictionary<int, IValue> toStoreInMemory)
        {
            ColumnHeader = header;
            DefaultValue = defaultValue;
            Values = values;
            ToStoreInMemory = toStoreInMemory;
        }

        public void Apply(TwoDA twoda, Memory memory, ILogger logger)
        {
            twoda.AddColumn(ColumnHeader);

            foreach (var row in twoda.Rows())
            {
                row.SetCell(ColumnHeader, DefaultValue);
            }
        }
    }
}
