using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using KSerialization;

// Token: 0x02000B86 RID: 2950
[SerializationConfig(MemberSerialization.OptIn)]
public class SerializedList<ItemType>
{
	// Token: 0x17000689 RID: 1673
	// (get) Token: 0x06005B9C RID: 23452 RVA: 0x00219606 File Offset: 0x00217806
	public int Count
	{
		get
		{
			return this.items.Count;
		}
	}

	// Token: 0x06005B9D RID: 23453 RVA: 0x00219613 File Offset: 0x00217813
	public IEnumerator<ItemType> GetEnumerator()
	{
		return this.items.GetEnumerator();
	}

	// Token: 0x1700068A RID: 1674
	public ItemType this[int idx]
	{
		get
		{
			return this.items[idx];
		}
	}

	// Token: 0x06005B9F RID: 23455 RVA: 0x00219633 File Offset: 0x00217833
	public void Add(ItemType item)
	{
		this.items.Add(item);
	}

	// Token: 0x06005BA0 RID: 23456 RVA: 0x00219641 File Offset: 0x00217841
	public void Remove(ItemType item)
	{
		this.items.Remove(item);
	}

	// Token: 0x06005BA1 RID: 23457 RVA: 0x00219650 File Offset: 0x00217850
	public void RemoveAt(int idx)
	{
		this.items.RemoveAt(idx);
	}

	// Token: 0x06005BA2 RID: 23458 RVA: 0x0021965E File Offset: 0x0021785E
	public bool Contains(ItemType item)
	{
		return this.items.Contains(item);
	}

	// Token: 0x06005BA3 RID: 23459 RVA: 0x0021966C File Offset: 0x0021786C
	public void Clear()
	{
		this.items.Clear();
	}

	// Token: 0x06005BA4 RID: 23460 RVA: 0x0021967C File Offset: 0x0021787C
	[OnSerializing]
	private void OnSerializing()
	{
		MemoryStream memoryStream = new MemoryStream();
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(this.items.Count);
		foreach (ItemType itemType in this.items)
		{
			binaryWriter.WriteKleiString(itemType.GetType().FullName);
			long position = binaryWriter.BaseStream.Position;
			binaryWriter.Write(0);
			long position2 = binaryWriter.BaseStream.Position;
			Serializer.SerializeTypeless(itemType, binaryWriter);
			long position3 = binaryWriter.BaseStream.Position;
			long num = position3 - position2;
			binaryWriter.BaseStream.Position = position;
			binaryWriter.Write((int)num);
			binaryWriter.BaseStream.Position = position3;
		}
		memoryStream.Flush();
		this.serializationBuffer = memoryStream.ToArray();
	}

	// Token: 0x06005BA5 RID: 23461 RVA: 0x0021977C File Offset: 0x0021797C
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.serializationBuffer == null)
		{
			return;
		}
		FastReader fastReader = new FastReader(this.serializationBuffer);
		int num = fastReader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			string text = fastReader.ReadKleiString();
			int num2 = fastReader.ReadInt32();
			int position = fastReader.Position;
			Type type = Type.GetType(text);
			if (type == null)
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Type no longer exists: " + text
				});
				fastReader.SkipBytes(num2);
			}
			else
			{
				ItemType itemType;
				if (typeof(ItemType) != type)
				{
					itemType = (ItemType)((object)Activator.CreateInstance(type));
				}
				else
				{
					itemType = default(ItemType);
				}
				Deserializer.DeserializeTypeless(itemType, fastReader);
				if (fastReader.Position != position + num2)
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"Expected to be at offset",
						position + num2,
						"but was only at offset",
						fastReader.Position,
						". Skipping to catch up."
					});
					fastReader.SkipBytes(position + num2 - fastReader.Position);
				}
				this.items.Add(itemType);
			}
		}
	}

	// Token: 0x04003DBE RID: 15806
	[Serialize]
	private byte[] serializationBuffer;

	// Token: 0x04003DBF RID: 15807
	private List<ItemType> items = new List<ItemType>();
}
