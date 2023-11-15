using System;
using Characters.Gear.Upgrades;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Data.Hardmode.Upgrades
{
	// Token: 0x020002B3 RID: 691
	[CreateAssetMenu(fileName = "UpgradeShopSettings", menuName = "ScriptableObjects/UpgradeShopSettings")]
	public sealed class UpgradeShopSettings : ScriptableObject
	{
		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000E1F RID: 3615 RVA: 0x0002CB7B File Offset: 0x0002AD7B
		public static UpgradeShopSettings instance
		{
			get
			{
				if (UpgradeShopSettings._instance == null)
				{
					UpgradeShopSettings._instance = Resources.Load<UpgradeShopSettings>("HardmodeSetting/UpgradeShopSettings");
				}
				return UpgradeShopSettings._instance;
			}
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x0002CBA0 File Offset: 0x0002ADA0
		public int GetRemoveCost(UpgradeObject.Type type)
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			int result = 0;
			if (type == UpgradeObject.Type.Cursed)
			{
				UpgradeShopSettings.CostByChapter[] array = this._removeRiskyCosts;
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].TryTakeCost(currentChapter.type, ref result))
					{
						return result;
					}
				}
			}
			else
			{
				UpgradeShopSettings.CostByChapter[] array = this._removeCosts;
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].TryTakeCost(currentChapter.type, ref result))
					{
						return result;
					}
				}
			}
			return result;
		}

		// Token: 0x04000BBF RID: 3007
		private const string harmode = "HardmodeSetting";

		// Token: 0x04000BC0 RID: 3008
		[SerializeField]
		private UpgradeShopSettings.CostByChapter[] _removeCosts;

		// Token: 0x04000BC1 RID: 3009
		[SerializeField]
		private UpgradeShopSettings.CostByChapter[] _removeRiskyCosts;

		// Token: 0x04000BC2 RID: 3010
		private static UpgradeShopSettings _instance;

		// Token: 0x020002B4 RID: 692
		[Serializable]
		private class CostByChapter
		{
			// Token: 0x06000E22 RID: 3618 RVA: 0x0002CC18 File Offset: 0x0002AE18
			public bool TryTakeCost(Chapter.Type type, ref int cost)
			{
				if (type != this._chapter)
				{
					return false;
				}
				cost = this._cost;
				return true;
			}

			// Token: 0x04000BC3 RID: 3011
			[SerializeField]
			private Chapter.Type _chapter;

			// Token: 0x04000BC4 RID: 3012
			[SerializeField]
			private int _cost;
		}
	}
}
