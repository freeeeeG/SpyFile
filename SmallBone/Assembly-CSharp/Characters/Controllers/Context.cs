using System;
using System.Collections.Generic;

namespace Characters.Controllers
{
	// Token: 0x02000915 RID: 2325
	public class Context
	{
		// Token: 0x060031CF RID: 12751 RVA: 0x0009420B File Offset: 0x0009240B
		public T Get<T>(string name)
		{
			return (T)((object)this._dictionary[name]);
		}

		// Token: 0x060031D0 RID: 12752 RVA: 0x0009421E File Offset: 0x0009241E
		public void Set<T>(string name, T value)
		{
			if (this._dictionary.ContainsKey(name))
			{
				this._dictionary[name] = value;
				return;
			}
			this._dictionary.Add(name, value);
		}

		// Token: 0x060031D1 RID: 12753 RVA: 0x00094253 File Offset: 0x00092453
		public ContextVariable<T> GetVariable<T>(string name)
		{
			return new ContextVariable<T>(this, name);
		}

		// Token: 0x040028CF RID: 10447
		private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();
	}
}
