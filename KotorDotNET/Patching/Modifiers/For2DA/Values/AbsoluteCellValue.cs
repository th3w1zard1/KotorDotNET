﻿using KotorDotNET.FileFormats.Kotor2DA;
using KotorDotNET.Patching.Modifiers.For2DA.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Patching.Modifiers.For2DA.Values
{
    public class AbsoluteCellValue : IValue
    {
        public string ColumnHeader { get; set; }
        public ITarget Target { get; set; }

        public AbsoluteCellValue(ITarget target, string columnHeader)
        {
            Target = target;
            ColumnHeader = columnHeader;
        }

        public string GetValue(Memory memory, TwoDA twoda, TwoDARow? row, string? columnHeader)
        {
            var target = Target.Search(twoda);
            return target.GetCell(ColumnHeader);
        }
    }
}
