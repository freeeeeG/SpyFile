using System;
using System.Collections.Generic;
using System.IO;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000E01 RID: 3585
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Modifications<ModifierType, InstanceType> : ISaveLoadableDetails where ModifierType : Resource where InstanceType : ModifierInstance<ModifierType>
	{
		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06006DF7 RID: 28151 RVA: 0x002B5907 File Offset: 0x002B3B07
		public int Count
		{
			get
			{
				return this.ModifierList.Count;
			}
		}

		// Token: 0x06006DF8 RID: 28152 RVA: 0x002B5914 File Offset: 0x002B3B14
		public IEnumerator<InstanceType> GetEnumerator()
		{
			return this.ModifierList.GetEnumerator();
		}

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06006DF9 RID: 28153 RVA: 0x002B5926 File Offset: 0x002B3B26
		// (set) Token: 0x06006DFA RID: 28154 RVA: 0x002B592E File Offset: 0x002B3B2E
		public GameObject gameObject { get; private set; }

		// Token: 0x170007C7 RID: 1991
		public InstanceType this[int idx]
		{
			get
			{
				return this.ModifierList[idx];
			}
		}

		// Token: 0x06006DFC RID: 28156 RVA: 0x002B5945 File Offset: 0x002B3B45
		public ComponentType GetComponent<ComponentType>()
		{
			return this.gameObject.GetComponent<ComponentType>();
		}

		// Token: 0x06006DFD RID: 28157 RVA: 0x002B5952 File Offset: 0x002B3B52
		public void Trigger(GameHashes hash, object data = null)
		{
			this.gameObject.GetComponent<KPrefabID>().Trigger((int)hash, data);
		}

		// Token: 0x06006DFE RID: 28158 RVA: 0x002B5968 File Offset: 0x002B3B68
		public virtual InstanceType CreateInstance(ModifierType modifier)
		{
			return default(InstanceType);
		}

		// Token: 0x06006DFF RID: 28159 RVA: 0x002B597E File Offset: 0x002B3B7E
		public Modifications(GameObject go, ResourceSet<ModifierType> resources = null)
		{
			this.resources = resources;
			this.gameObject = go;
		}

		// Token: 0x06006E00 RID: 28160 RVA: 0x002B599F File Offset: 0x002B3B9F
		public virtual InstanceType Add(InstanceType instance)
		{
			this.ModifierList.Add(instance);
			return instance;
		}

		// Token: 0x06006E01 RID: 28161 RVA: 0x002B59B0 File Offset: 0x002B3BB0
		public virtual void Remove(InstanceType instance)
		{
			for (int i = 0; i < this.ModifierList.Count; i++)
			{
				if (this.ModifierList[i] == instance)
				{
					this.ModifierList.RemoveAt(i);
					instance.OnCleanUp();
					return;
				}
			}
		}

		// Token: 0x06006E02 RID: 28162 RVA: 0x002B5A04 File Offset: 0x002B3C04
		public bool Has(ModifierType modifier)
		{
			return this.Get(modifier) != null;
		}

		// Token: 0x06006E03 RID: 28163 RVA: 0x002B5A18 File Offset: 0x002B3C18
		public InstanceType Get(ModifierType modifier)
		{
			foreach (InstanceType instanceType in this.ModifierList)
			{
				if (instanceType.modifier == modifier)
				{
					return instanceType;
				}
			}
			return default(InstanceType);
		}

		// Token: 0x06006E04 RID: 28164 RVA: 0x002B5A8C File Offset: 0x002B3C8C
		public InstanceType Get(string id)
		{
			foreach (InstanceType instanceType in this.ModifierList)
			{
				if (instanceType.modifier.Id == id)
				{
					return instanceType;
				}
			}
			return default(InstanceType);
		}

		// Token: 0x06006E05 RID: 28165 RVA: 0x002B5B04 File Offset: 0x002B3D04
		public void Serialize(BinaryWriter writer)
		{
			writer.Write(this.ModifierList.Count);
			foreach (InstanceType instanceType in this.ModifierList)
			{
				writer.WriteKleiString(instanceType.modifier.Id);
				long position = writer.BaseStream.Position;
				writer.Write(0);
				long position2 = writer.BaseStream.Position;
				Serializer.SerializeTypeless(instanceType, writer);
				long position3 = writer.BaseStream.Position;
				long num = position3 - position2;
				writer.BaseStream.Position = position;
				writer.Write((int)num);
				writer.BaseStream.Position = position3;
			}
		}

		// Token: 0x06006E06 RID: 28166 RVA: 0x002B5BE4 File Offset: 0x002B3DE4
		public void Deserialize(IReader reader)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string text = reader.ReadKleiString();
				int num2 = reader.ReadInt32();
				int position = reader.Position;
				InstanceType instanceType = this.Get(text);
				if (instanceType == null && this.resources != null)
				{
					ModifierType modifierType = this.resources.TryGet(text);
					if (modifierType != null)
					{
						instanceType = this.CreateInstance(modifierType);
					}
				}
				if (instanceType == null)
				{
					if (text != "Condition")
					{
						DebugUtil.LogWarningArgs(new object[]
						{
							this.gameObject.name,
							"Missing modifier: " + text
						});
					}
					reader.SkipBytes(num2);
				}
				else if (!(instanceType is ISaveLoadable))
				{
					reader.SkipBytes(num2);
				}
				else
				{
					Deserializer.DeserializeTypeless(instanceType, reader);
					if (reader.Position != position + num2)
					{
						DebugUtil.LogWarningArgs(new object[]
						{
							"Expected to be at offset",
							position + num2,
							"but was only at offset",
							reader.Position,
							". Skipping to catch up."
						});
						reader.SkipBytes(position + num2 - reader.Position);
					}
				}
			}
		}

		// Token: 0x04005282 RID: 21122
		public List<InstanceType> ModifierList = new List<InstanceType>();

		// Token: 0x04005284 RID: 21124
		private ResourceSet<ModifierType> resources;
	}
}
