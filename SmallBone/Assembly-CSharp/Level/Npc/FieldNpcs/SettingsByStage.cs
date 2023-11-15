using System;
using UnityEngine;

namespace Level.Npc.FieldNpcs
{
	// Token: 0x020005FB RID: 1531
	[Serializable]
	public class SettingsByStage
	{
		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06001EB2 RID: 7858 RVA: 0x0005D401 File Offset: 0x0005B601
		public int[] darkPriestGoldCosts
		{
			get
			{
				return this._darkPriestGoldCosts;
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06001EB3 RID: 7859 RVA: 0x0005D409 File Offset: 0x0005B609
		public int sleepySekeletonHealthPercentCost
		{
			get
			{
				return this._sleepySekeletonHealthPercentCost;
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06001EB4 RID: 7860 RVA: 0x0005D411 File Offset: 0x0005B611
		public RarityPossibilities sleepySekeletonHeadPossibilities
		{
			get
			{
				return this._sleepySekeletonHeadPossibilities;
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06001EB5 RID: 7861 RVA: 0x0005D419 File Offset: 0x0005B619
		public int plebbyGoldCost
		{
			get
			{
				return this._plebbyGoldCost;
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06001EB6 RID: 7862 RVA: 0x0005D421 File Offset: 0x0005B621
		public RarityPossibilities plebbyItemPossibilities
		{
			get
			{
				return this._plebbyItemPossibilities;
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06001EB7 RID: 7863 RVA: 0x0005D429 File Offset: 0x0005B629
		public int harpyWarriorBones
		{
			get
			{
				return this._harpyWarriorBones;
			}
		}

		// Token: 0x040019EA RID: 6634
		[Header("DarkPriest")]
		[SerializeField]
		private int[] _darkPriestGoldCosts;

		// Token: 0x040019EB RID: 6635
		[SerializeField]
		[Range(0f, 100f)]
		[Header("SleepySkeleton")]
		private int _sleepySekeletonHealthPercentCost;

		// Token: 0x040019EC RID: 6636
		[SerializeField]
		private RarityPossibilities _sleepySekeletonHeadPossibilities;

		// Token: 0x040019ED RID: 6637
		[Header("Plebby")]
		[SerializeField]
		private int _plebbyGoldCost;

		// Token: 0x040019EE RID: 6638
		[SerializeField]
		private RarityPossibilities _plebbyItemPossibilities;

		// Token: 0x040019EF RID: 6639
		[SerializeField]
		[Header("Harpy Warrior")]
		private int _harpyWarriorBones;
	}
}
