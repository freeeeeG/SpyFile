using System;
using System.Collections.Generic;
using BT.SharedValues;

namespace BT
{
	// Token: 0x02001405 RID: 5125
	public class Context : Dictionary<string, SharedValue>
	{
		// Token: 0x1700141F RID: 5151
		// (get) Token: 0x060064E4 RID: 25828 RVA: 0x0012455B File Offset: 0x0012275B
		// (set) Token: 0x060064E5 RID: 25829 RVA: 0x00124563 File Offset: 0x00122763
		public float deltaTime { get; set; }

		// Token: 0x060064E6 RID: 25830 RVA: 0x0012456C File Offset: 0x0012276C
		public T Get<T>(string name)
		{
			SharedValue sharedValue;
			if (base.TryGetValue(name, out sharedValue))
			{
				return (sharedValue as SharedValue<T>).GetValue();
			}
			return default(T);
		}

		// Token: 0x060064E7 RID: 25831 RVA: 0x00124599 File Offset: 0x00122799
		public void Set<T>(string name, SharedValue<T> value)
		{
			if (!base.ContainsKey(name))
			{
				base.Add(name, value);
				return;
			}
			base[name] = value;
		}

		// Token: 0x060064E8 RID: 25832 RVA: 0x001245B5 File Offset: 0x001227B5
		public static Context Create()
		{
			return new Context();
		}
	}
}
