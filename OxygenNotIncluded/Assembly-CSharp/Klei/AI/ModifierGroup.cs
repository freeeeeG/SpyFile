using System;
using System.Collections.Generic;

namespace Klei.AI
{
	// Token: 0x02000E03 RID: 3587
	public class ModifierGroup<T> : Resource
	{
		// Token: 0x06006E0B RID: 28171 RVA: 0x002B5E04 File Offset: 0x002B4004
		public IEnumerator<T> GetEnumerator()
		{
			return this.modifiers.GetEnumerator();
		}

		// Token: 0x170007C8 RID: 1992
		public T this[int idx]
		{
			get
			{
				return this.modifiers[idx];
			}
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06006E0D RID: 28173 RVA: 0x002B5E24 File Offset: 0x002B4024
		public int Count
		{
			get
			{
				return this.modifiers.Count;
			}
		}

		// Token: 0x06006E0E RID: 28174 RVA: 0x002B5E31 File Offset: 0x002B4031
		public ModifierGroup(string id, string name) : base(id, name)
		{
		}

		// Token: 0x06006E0F RID: 28175 RVA: 0x002B5E46 File Offset: 0x002B4046
		public void Add(T modifier)
		{
			this.modifiers.Add(modifier);
		}

		// Token: 0x04005287 RID: 21127
		public List<T> modifiers = new List<T>();
	}
}
