﻿using KotorDotNET.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Extensions
{
    public static class BinaryWriterExtensions
    {
        public static void Write(this BinaryWriter writer, string value, int prefixSize)
        {
            if (prefixSize == 0)
            {
                writer.Write(Encoding.GetEncoding(1252).GetBytes(value));
            }
            else if (prefixSize == 1)
            {
                writer.Write((byte)value.Length);
            }
            else if (prefixSize == 2)
            {
                writer.Write((ushort)value.Length);
            }
            else if (prefixSize == 4)
            {
                writer.Write((uint)value.Length);
            }
            else if (prefixSize == 8)
            {
                writer.Write((ulong)value.Length);
            }
            else
            {
                throw new ArgumentException("Unsupported prefix size specified.");
            }
        }

        /// <summary>
        /// Write a ResRef to the stream.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value">The ResRef to write to the stream.</param>
        /// <param name="withPrefix">
        /// If true inserts a single byte containing the length of the ResRef,
        /// otherwise the ResRef is padded with null bytes to reach 16 bytes.
        /// </param>
        public static void Write(this BinaryWriter writer, ResRef value, bool withPrefix)
        {
            if (withPrefix)
            {
                Write(writer, value.Get(), 1);
            }
            else
            {
                Write(writer, value.Get().PadRight(16, '\0').Truncate(16), 0);
            }
        }
    }
}
