using System;
using System.Collections.Generic;
using System.IO;

namespace InControl
{
	// Token: 0x0200029E RID: 670
	public struct KeyCombo
	{
		// Token: 0x06000C5F RID: 3167 RVA: 0x0003F4DC File Offset: 0x0003D8DC
		public KeyCombo(params Key[] keys)
		{
			this.data = 0UL;
			this.size = 0;
			for (int i = 0; i < keys.Length; i++)
			{
				this.Add(keys[i]);
			}
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x0003F515 File Offset: 0x0003D915
		private void AddInt(int key)
		{
			if (this.size == 8)
			{
				return;
			}
			this.data |= (ulong)((ulong)((long)key & 255L) << this.size * 8);
			this.size++;
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x0003F554 File Offset: 0x0003D954
		private int GetInt(int index)
		{
			return (int)(this.data >> index * 8 & 255UL);
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x0003F56B File Offset: 0x0003D96B
		public void Add(Key key)
		{
			this.AddInt((int)key);
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x0003F574 File Offset: 0x0003D974
		public Key Get(int index)
		{
			if (index < 0 || index >= this.size)
			{
				throw new IndexOutOfRangeException(string.Concat(new object[]
				{
					"Index ",
					index,
					" is out of the range 0..",
					this.size
				}));
			}
			return (Key)this.GetInt(index);
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x0003F5D3 File Offset: 0x0003D9D3
		public void Clear()
		{
			this.data = 0UL;
			this.size = 0;
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000C65 RID: 3173 RVA: 0x0003F5E4 File Offset: 0x0003D9E4
		public int Count
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000C66 RID: 3174 RVA: 0x0003F5EC File Offset: 0x0003D9EC
		public bool IsPressed
		{
			get
			{
				if (this.size == 0)
				{
					return false;
				}
				bool flag = true;
				for (int i = 0; i < this.size; i++)
				{
					int @int = this.GetInt(i);
					flag = (flag && KeyInfo.KeyList[@int].IsPressed);
					if (flag)
					{
					}
				}
				return flag;
			}
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x0003F64C File Offset: 0x0003DA4C
		public static KeyCombo Detect(bool modifiersAsKeys)
		{
			KeyCombo result = default(KeyCombo);
			if (modifiersAsKeys)
			{
				for (int i = 1; i < 5; i++)
				{
					if (KeyInfo.KeyList[i].IsPressed)
					{
						result.AddInt(i);
					}
				}
			}
			else
			{
				for (int j = 5; j < 13; j++)
				{
					if (KeyInfo.KeyList[j].IsPressed)
					{
						result.AddInt(j);
						return result;
					}
				}
			}
			for (int k = 13; k < KeyInfo.KeyList.Length; k++)
			{
				if (KeyInfo.KeyList[k].IsPressed)
				{
					result.AddInt(k);
					return result;
				}
			}
			result.Clear();
			return result;
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x0003F710 File Offset: 0x0003DB10
		public override string ToString()
		{
			string text;
			if (!KeyCombo.cachedStrings.TryGetValue(this.data, out text))
			{
				text = string.Empty;
				for (int i = 0; i < this.size; i++)
				{
					if (i != 0)
					{
						text += " ";
					}
					int @int = this.GetInt(i);
					text += KeyInfo.KeyList[@int].Name;
				}
			}
			return text;
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x0003F783 File Offset: 0x0003DB83
		public static bool operator ==(KeyCombo a, KeyCombo b)
		{
			return a.data == b.data;
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x0003F795 File Offset: 0x0003DB95
		public static bool operator !=(KeyCombo a, KeyCombo b)
		{
			return a.data != b.data;
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x0003F7AC File Offset: 0x0003DBAC
		public override bool Equals(object other)
		{
			if (other is KeyCombo)
			{
				KeyCombo keyCombo = (KeyCombo)other;
				return this.data == keyCombo.data;
			}
			return false;
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x0003F7DC File Offset: 0x0003DBDC
		public override int GetHashCode()
		{
			return this.data.GetHashCode();
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x0003F7EF File Offset: 0x0003DBEF
		internal void Load(BinaryReader reader)
		{
			this.size = reader.ReadInt32();
			this.data = reader.ReadUInt64();
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x0003F809 File Offset: 0x0003DC09
		internal void Save(BinaryWriter writer)
		{
			writer.Write(this.size);
			writer.Write(this.data);
		}

		// Token: 0x040009C2 RID: 2498
		private int size;

		// Token: 0x040009C3 RID: 2499
		private ulong data;

		// Token: 0x040009C4 RID: 2500
		private static Dictionary<ulong, string> cachedStrings = new Dictionary<ulong, string>();
	}
}
