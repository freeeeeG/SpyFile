using System;
using UnityEngine;

namespace FX
{
	// Token: 0x0200023E RID: 574
	public class GlobalSoundSettings : ScriptableObject
	{
		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000B4D RID: 2893 RVA: 0x0001F513 File Offset: 0x0001D713
		public SoundInfo gearDestroying
		{
			get
			{
				return this._gearDestroyed;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000B4E RID: 2894 RVA: 0x0001F51B File Offset: 0x0001D71B
		public SoundInfo endFreeze
		{
			get
			{
				return this._endFreeze;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000B4F RID: 2895 RVA: 0x0001F523 File Offset: 0x0001D723
		public static GlobalSoundSettings instance
		{
			get
			{
				if (GlobalSoundSettings._instance == null)
				{
					GlobalSoundSettings._instance = Resources.Load<GlobalSoundSettings>("GlobalSoundSettings");
				}
				return GlobalSoundSettings._instance;
			}
		}

		// Token: 0x04000970 RID: 2416
		[SerializeField]
		private SoundInfo _gearDestroyed;

		// Token: 0x04000971 RID: 2417
		[SerializeField]
		private SoundInfo _endFreeze;

		// Token: 0x04000972 RID: 2418
		private static GlobalSoundSettings _instance;
	}
}
