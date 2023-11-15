using System;
using System.IO;
using UnityEngine;

namespace Pathfinding.Serialization
{
	// Token: 0x020000B4 RID: 180
	public class GraphSerializationContext
	{
		// Token: 0x060007E4 RID: 2020 RVA: 0x000348EF File Offset: 0x00032AEF
		public GraphSerializationContext(BinaryReader reader, GraphNode[] id2NodeMapping, uint graphIndex, GraphMeta meta)
		{
			this.reader = reader;
			this.id2NodeMapping = id2NodeMapping;
			this.graphIndex = graphIndex;
			this.meta = meta;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00034914 File Offset: 0x00032B14
		public GraphSerializationContext(BinaryWriter writer)
		{
			this.writer = writer;
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00034923 File Offset: 0x00032B23
		public void SerializeNodeReference(GraphNode node)
		{
			this.writer.Write((node == null) ? -1 : node.NodeIndex);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0003493C File Offset: 0x00032B3C
		public GraphNode DeserializeNodeReference()
		{
			int num = this.reader.ReadInt32();
			if (this.id2NodeMapping == null)
			{
				throw new Exception("Calling DeserializeNodeReference when not deserializing node references");
			}
			if (num == -1)
			{
				return null;
			}
			GraphNode graphNode = this.id2NodeMapping[num];
			if (graphNode == null)
			{
				throw new Exception("Invalid id (" + num.ToString() + ")");
			}
			return graphNode;
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x00034995 File Offset: 0x00032B95
		public void SerializeVector3(Vector3 v)
		{
			this.writer.Write(v.x);
			this.writer.Write(v.y);
			this.writer.Write(v.z);
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x000349CA File Offset: 0x00032BCA
		public Vector3 DeserializeVector3()
		{
			return new Vector3(this.reader.ReadSingle(), this.reader.ReadSingle(), this.reader.ReadSingle());
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x000349F2 File Offset: 0x00032BF2
		public void SerializeInt3(Int3 v)
		{
			this.writer.Write(v.x);
			this.writer.Write(v.y);
			this.writer.Write(v.z);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x00034A27 File Offset: 0x00032C27
		public Int3 DeserializeInt3()
		{
			return new Int3(this.reader.ReadInt32(), this.reader.ReadInt32(), this.reader.ReadInt32());
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x00034A4F File Offset: 0x00032C4F
		public int DeserializeInt(int defaultValue)
		{
			if (this.reader.BaseStream.Position <= this.reader.BaseStream.Length - 4L)
			{
				return this.reader.ReadInt32();
			}
			return defaultValue;
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x00034A83 File Offset: 0x00032C83
		public float DeserializeFloat(float defaultValue)
		{
			if (this.reader.BaseStream.Position <= this.reader.BaseStream.Length - 4L)
			{
				return this.reader.ReadSingle();
			}
			return defaultValue;
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x00034AB8 File Offset: 0x00032CB8
		public Object DeserializeUnityObject()
		{
			if (this.reader.ReadInt32() == 2147483647)
			{
				return null;
			}
			string text = this.reader.ReadString();
			string text2 = this.reader.ReadString();
			string text3 = this.reader.ReadString();
			Type type = Type.GetType(text2);
			if (type == null)
			{
				Debug.LogError("Could not find type '" + text2 + "'. Cannot deserialize Unity reference");
				return null;
			}
			if (!string.IsNullOrEmpty(text3))
			{
				UnityReferenceHelper[] array = Object.FindObjectsOfType(typeof(UnityReferenceHelper)) as UnityReferenceHelper[];
				int i = 0;
				while (i < array.Length)
				{
					if (array[i].GetGUID() == text3)
					{
						if (type == typeof(GameObject))
						{
							return array[i].gameObject;
						}
						return array[i].GetComponent(type);
					}
					else
					{
						i++;
					}
				}
			}
			Object[] array2 = Resources.LoadAll(text, type);
			for (int j = 0; j < array2.Length; j++)
			{
				if (array2[j].name == text || array2.Length == 1)
				{
					return array2[j];
				}
			}
			return null;
		}

		// Token: 0x040004AE RID: 1198
		private readonly GraphNode[] id2NodeMapping;

		// Token: 0x040004AF RID: 1199
		public readonly BinaryReader reader;

		// Token: 0x040004B0 RID: 1200
		public readonly BinaryWriter writer;

		// Token: 0x040004B1 RID: 1201
		public readonly uint graphIndex;

		// Token: 0x040004B2 RID: 1202
		public readonly GraphMeta meta;
	}
}
