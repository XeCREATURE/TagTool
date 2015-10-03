﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloOnlineTagTool
{
	/// <summary>
	/// A file containing a cache of all of the stringID strings.
	/// </summary>
	public class StringIdCache
	{
		/// <summary>
		/// Loads a stringID cache from a string_ids.dat file.
		/// </summary>
		/// <param name="stream">The stream to read from.</param>
		/// <param name="resolver">The stringID resolver to use.</param>
		public StringIdCache(Stream stream, StringIdResolverBase resolver)
		{
			Resolver = resolver;
			Strings = new List<string>();
			Load(stream);
		}

		/// <summary>
		/// Gets the strings in the file.
		/// Note that strings can be <c>null</c>.
		/// </summary>
		public List<string> Strings { get; private set; }

		/// <summary>
		/// Gets the stringID resolver that the cache is using.
		/// </summary>
		public StringIdResolverBase Resolver { get; private set; }

		/// <summary>
		/// Gets the string corresponding to a stringID.
		/// </summary>
		/// <param name="stringId">The stringID.</param>
		/// <returns>The string corresponding to the stringID, or <c>null</c> if not found.</returns>
		public string GetString(StringId stringId)
		{
			var strIndex = Resolver.StringIdToIndex(stringId);
			if (strIndex < 0 || strIndex >= Strings.Count)
				return null;
			return Strings[strIndex];
		}

		/// <summary>
		/// Gets the stringID corresponding to a string list index.
		/// </summary>
		/// <param name="strIndex">The string list index to convert.</param>
		/// <returns>The corresponding stringID.</returns>
		public StringId GetStringId(int strIndex)
		{
			if (strIndex < 0 || strIndex >= Strings.Count)
				return StringId.Null;
			return Resolver.IndexToStringId(strIndex);
		}

		/// <summary>
		/// Gets the stringID corresponding to a string in the list.
		/// </summary>
		/// <param name="str">The string to search for.</param>
		/// <returns>The corresponding stringID, or <see cref="StringId.Null"/> if not found.</returns>
		public StringId GetStringId(string str)
		{
			return GetStringId(Strings.IndexOf(str));
		}

		/// <summary>
		/// Saves the string data back to the file.
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		public void Save(Stream stream)
		{
			var writer = new BinaryWriter(stream);

			// Write the string count and then skip over the offset table, because it will be filled in last
			writer.Write(Strings.Count);
			writer.BaseStream.Position += 4 + Strings.Count * 4; // 4 byte data size + 4 bytes per string offset
			
			// Write string data and keep track of offsets
			var stringOffsets = new int[Strings.Count];
			var dataOffset = (int)writer.BaseStream.Position;
			for (var i = 0; i < Strings.Count; i++)
			{
				var str = Strings[i];
				if (str == null)
				{
					// Null string - set offset to -1
					stringOffsets[i] = -1;
					continue;
				}

				// Write the string as null-terminated ASCII
				stringOffsets[i] = (int)writer.BaseStream.Position;
				var data = Encoding.ASCII.GetBytes(str);
				writer.Write(data, 0, data.Length);
				writer.Write((byte)0);
			}
			var dataEndOffset = (int)writer.BaseStream.Position;

			// Now go back and write the string offsets
			writer.BaseStream.Position = 0x4;
			writer.Write(dataEndOffset - dataOffset); // Data size
			foreach (var offset in stringOffsets)
				writer.Write(offset);
			writer.BaseStream.SetLength(writer.BaseStream.Position);
		}

		/// <summary>
		/// Loads the cache from a stream.
		/// </summary>
		/// <param name="stream">The stream to read from.</param>
		private void Load(Stream stream)
		{
			var reader = new BinaryReader(stream);

			// Read the header
			var stringCount = reader.ReadInt32();  // int32 string count
			var dataSize = reader.ReadInt32();     // int32 string data size

			// Read string offsets
			var stringOffsets = new int[stringCount];
			for (var i = 0; i < stringCount; i++)
				stringOffsets[i] = reader.ReadInt32();

			// Seek to each offset and read each string
			var dataOffset = reader.BaseStream.Position;
			foreach (var offset in stringOffsets)
			{
				if (offset == -1 || offset >= dataSize)
				{
					Strings.Add(null);
					continue;
				}
				reader.BaseStream.Position = dataOffset + offset;
				Strings.Add(ReadString(reader));
			}
		}

		/// <summary>
		/// Reads a null-terminated ASCII string from a stream.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns>The string.</returns>
		private static string ReadString(BinaryReader reader)
		{
			var result = new StringBuilder();
			while (true)
			{
				var ch = reader.ReadByte();
				if (ch == 0)
					break;
				result.Append((char)ch);
			}
			return result.ToString();
		}
	}
}
