using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameModes
{
	// Token: 0x020006B3 RID: 1715
	[Serializable]
	public abstract class KeyValueData<K, V> : ScriptableObject
	{
		// Token: 0x06002077 RID: 8311 RVA: 0x0009CFC0 File Offset: 0x0009B3C0
		public virtual bool Contains(K key)
		{
			return this.IndexOf(key) != -1;
		}

		// Token: 0x06002078 RID: 8312 RVA: 0x0009CFCF File Offset: 0x0009B3CF
		protected virtual int IndexOf(K key)
		{
			return this.m_keys.IndexOf(key);
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x0009CFE0 File Offset: 0x0009B3E0
		public virtual V Get(K key)
		{
			int num = this.IndexOf(key);
			return (num == -1) ? this.m_defaultValue : this.m_values[num];
		}

		// Token: 0x040018EA RID: 6378
		[SerializeField]
		protected V m_defaultValue = default(V);

		// Token: 0x040018EB RID: 6379
		[SerializeField]
		protected List<K> m_keys = new List<K>();

		// Token: 0x040018EC RID: 6380
		[SerializeField]
		protected List<V> m_values = new List<V>();
	}
}
