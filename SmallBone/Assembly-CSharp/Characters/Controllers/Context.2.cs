using System;
using System.Collections.Generic;

namespace Characters.Controllers
{
	// Token: 0x02000916 RID: 2326
	public class Context<TKey>
	{
		// Token: 0x060031D3 RID: 12755 RVA: 0x0009426F File Offset: 0x0009246F
		public TVal Get<TVal>(TKey key)
		{
			return (TVal)((object)this._dictionary[key]);
		}

		// Token: 0x060031D4 RID: 12756 RVA: 0x00094282 File Offset: 0x00092482
		public void Set<TVal>(TKey key, TVal value)
		{
			if (this._dictionary.ContainsKey(key))
			{
				this._dictionary[key] = value;
				return;
			}
			this._dictionary.Add(key, value);
		}

		// Token: 0x060031D5 RID: 12757 RVA: 0x000942B7 File Offset: 0x000924B7
		public ContextVariable<TKey, TVal> GetVariable<TVal>(TKey key)
		{
			return new ContextVariable<TKey, TVal>(this, key);
		}

		// Token: 0x040028D0 RID: 10448
		private readonly Dictionary<TKey, object> _dictionary = new Dictionary<TKey, object>();
	}
}
