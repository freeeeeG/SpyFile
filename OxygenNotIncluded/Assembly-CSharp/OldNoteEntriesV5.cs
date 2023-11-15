using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

// Token: 0x020004E6 RID: 1254
public class OldNoteEntriesV5
{
	// Token: 0x06001D0D RID: 7437 RVA: 0x0009A884 File Offset: 0x00098A84
	public void Deserialize(BinaryReader reader)
	{
		int num = reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			OldNoteEntriesV5.NoteStorageBlock item = default(OldNoteEntriesV5.NoteStorageBlock);
			item.Deserialize(reader);
			this.storageBlocks.Add(item);
		}
	}

	// Token: 0x0400101F RID: 4127
	public List<OldNoteEntriesV5.NoteStorageBlock> storageBlocks = new List<OldNoteEntriesV5.NoteStorageBlock>();

	// Token: 0x02001185 RID: 4485
	[StructLayout(LayoutKind.Explicit)]
	public struct NoteEntry
	{
		// Token: 0x04005C91 RID: 23697
		[FieldOffset(0)]
		public int reportEntryId;

		// Token: 0x04005C92 RID: 23698
		[FieldOffset(4)]
		public int noteHash;

		// Token: 0x04005C93 RID: 23699
		[FieldOffset(8)]
		public float value;
	}

	// Token: 0x02001186 RID: 4486
	[StructLayout(LayoutKind.Explicit)]
	public struct NoteEntryArray
	{
		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x060079E2 RID: 31202 RVA: 0x002DA6AB File Offset: 0x002D88AB
		public int StructSizeInBytes
		{
			get
			{
				return Marshal.SizeOf(typeof(OldNoteEntriesV5.NoteEntry));
			}
		}

		// Token: 0x04005C94 RID: 23700
		[FieldOffset(0)]
		public byte[] bytes;

		// Token: 0x04005C95 RID: 23701
		[FieldOffset(0)]
		public OldNoteEntriesV5.NoteEntry[] structs;
	}

	// Token: 0x02001187 RID: 4487
	public struct NoteStorageBlock
	{
		// Token: 0x060079E3 RID: 31203 RVA: 0x002DA6BC File Offset: 0x002D88BC
		public void Deserialize(BinaryReader reader)
		{
			this.entryCount = reader.ReadInt32();
			this.entries.bytes = reader.ReadBytes(this.entries.StructSizeInBytes * this.entryCount);
		}

		// Token: 0x04005C96 RID: 23702
		public int entryCount;

		// Token: 0x04005C97 RID: 23703
		public OldNoteEntriesV5.NoteEntryArray entries;
	}
}
