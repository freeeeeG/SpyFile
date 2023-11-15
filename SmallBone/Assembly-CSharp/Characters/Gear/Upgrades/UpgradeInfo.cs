using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Gear.Upgrades
{
	// Token: 0x02000853 RID: 2131
	[Serializable]
	public class UpgradeInfo
	{
		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x06002C52 RID: 11346 RVA: 0x000879F1 File Offset: 0x00085BF1
		public string name
		{
			get
			{
				return this.upgradeObject.displayName;
			}
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x06002C53 RID: 11347 RVA: 0x000879FE File Offset: 0x00085BFE
		public int maxLevel
		{
			get
			{
				return this.upgradeObject.maxLevel;
			}
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x06002C54 RID: 11348 RVA: 0x00087A0B File Offset: 0x00085C0B
		public UpgradeObject upgradeObject
		{
			get
			{
				return this.upgradeObject;
			}
		}

		// Token: 0x0400256C RID: 9580
		[SerializeField]
		private List<UpgradeObject> _upgradeByLevels;

		// Token: 0x02000854 RID: 2132
		[Serializable]
		private class UpgradeByLevel
		{
			// Token: 0x0400256D RID: 9581
			[SerializeField]
			internal UpgradeObject upgradeObject;
		}
	}
}
