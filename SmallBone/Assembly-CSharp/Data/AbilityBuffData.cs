using System;
using System.Collections.Generic;
using Platforms;
using Singletons;

namespace Data
{
	// Token: 0x0200029A RID: 666
	public class AbilityBuffData
	{
		// Token: 0x170002B6 RID: 694
		public string this[int index]
		{
			get
			{
				return this._stringDataBuffer[index];
			}
			set
			{
				this._stringDataBuffer[index] = value;
			}
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0002A278 File Offset: 0x00028478
		public AbilityBuffData(string key, int count, string defaultValue)
		{
			this.key = key;
			this.count = count;
			this._defaultValue = defaultValue;
			this._count = new IntData(key + "/count", false);
			this._stringDataBuffer = new List<string>(count);
			for (int i = 0; i < count; i++)
			{
				this._stringDataBuffer.Add(PersistentSingleton<PlatformManager>.Instance.platform.data.GetString(string.Format("{0}/{1}", key, i), ""));
			}
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0002A304 File Offset: 0x00028504
		public void Save()
		{
			for (int i = 0; i < this.count; i++)
			{
				PersistentSingleton<PlatformManager>.Instance.platform.data.SetString(string.Format("{0}/{1}", this.key, i), this._stringDataBuffer[i]);
			}
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x0002A358 File Offset: 0x00028558
		public void Reset()
		{
			for (int i = 0; i < this.count; i++)
			{
				this._stringDataBuffer[i] = this._defaultValue;
			}
		}

		// Token: 0x04000B1F RID: 2847
		public readonly string key;

		// Token: 0x04000B20 RID: 2848
		public readonly int count;

		// Token: 0x04000B21 RID: 2849
		private readonly string _defaultValue;

		// Token: 0x04000B22 RID: 2850
		private readonly IntData _count;

		// Token: 0x04000B23 RID: 2851
		private List<string> _stringDataBuffer;
	}
}
