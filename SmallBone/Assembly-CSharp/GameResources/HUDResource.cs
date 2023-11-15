using System;
using UnityEngine;

namespace GameResources
{
	// Token: 0x02000180 RID: 384
	public sealed class HUDResource : ScriptableObject
	{
		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x00018401 File Offset: 0x00016601
		// (set) Token: 0x0600083E RID: 2110 RVA: 0x00018408 File Offset: 0x00016608
		public static HUDResource.ResourceByMode playerFrame { get; private set; }

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x00018410 File Offset: 0x00016610
		// (set) Token: 0x06000840 RID: 2112 RVA: 0x00018417 File Offset: 0x00016617
		public static HUDResource.ResourceByMode playerDavyJonesFrame { get; private set; }

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000841 RID: 2113 RVA: 0x0001841F File Offset: 0x0001661F
		// (set) Token: 0x06000842 RID: 2114 RVA: 0x00018426 File Offset: 0x00016626
		public static HUDResource.ResourceByMode playerQuintessenceFrame { get; private set; }

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000843 RID: 2115 RVA: 0x0001842E File Offset: 0x0001662E
		// (set) Token: 0x06000844 RID: 2116 RVA: 0x00018435 File Offset: 0x00016635
		public static HUDResource.ResourceByMode playerSkill2Frame { get; private set; }

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000845 RID: 2117 RVA: 0x0001843D File Offset: 0x0001663D
		// (set) Token: 0x06000846 RID: 2118 RVA: 0x00018444 File Offset: 0x00016644
		public static HUDResource.ResourceByMode playerSubBarFrame { get; private set; }

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000847 RID: 2119 RVA: 0x0001844C File Offset: 0x0001664C
		// (set) Token: 0x06000848 RID: 2120 RVA: 0x00018453 File Offset: 0x00016653
		public static HUDResource.ResourceByMode playerSubSkill1Frame { get; private set; }

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000849 RID: 2121 RVA: 0x0001845B File Offset: 0x0001665B
		// (set) Token: 0x0600084A RID: 2122 RVA: 0x00018462 File Offset: 0x00016662
		public static HUDResource.ResourceByMode playerSubSkill2Frame { get; private set; }

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600084B RID: 2123 RVA: 0x0001846A File Offset: 0x0001666A
		// (set) Token: 0x0600084C RID: 2124 RVA: 0x00018471 File Offset: 0x00016671
		public static HUDResource.ResourceByMode playerSubSkullFrame { get; private set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600084D RID: 2125 RVA: 0x00018479 File Offset: 0x00016679
		// (set) Token: 0x0600084E RID: 2126 RVA: 0x00018480 File Offset: 0x00016680
		public static HUDResource.ResourceByMode timerFrame { get; private set; }

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x0600084F RID: 2127 RVA: 0x00018488 File Offset: 0x00016688
		// (set) Token: 0x06000850 RID: 2128 RVA: 0x0001848F File Offset: 0x0001668F
		public static HUDResource.ResourceByMode timerFrameWitchHardmodeLevel { get; private set; }

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000851 RID: 2129 RVA: 0x00018497 File Offset: 0x00016697
		// (set) Token: 0x06000852 RID: 2130 RVA: 0x0001849E File Offset: 0x0001669E
		public static HUDResource.ResourceByMode unlock { get; private set; }

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000853 RID: 2131 RVA: 0x000184A6 File Offset: 0x000166A6
		// (set) Token: 0x06000854 RID: 2132 RVA: 0x000184AD File Offset: 0x000166AD
		public static HUDResource.ResourceByMode minimap { get; private set; }

		// Token: 0x06000855 RID: 2133 RVA: 0x000184B8 File Offset: 0x000166B8
		public void Initialize()
		{
			base.hideFlags |= HideFlags.DontUnloadUnusedAsset;
			HUDResource.playerFrame = this._playerFrame;
			HUDResource.playerDavyJonesFrame = this._playerDavyJonesFrame;
			HUDResource.playerQuintessenceFrame = this._playerQuintessenceFrame;
			HUDResource.playerSkill2Frame = this._playerSkill2Frame;
			HUDResource.playerSubBarFrame = this._playerSubBarFrame;
			HUDResource.playerSubSkill1Frame = this._playerSubSkill1Frame;
			HUDResource.playerSubSkill2Frame = this._playerSubSkill2Frame;
			HUDResource.playerSubSkullFrame = this._playerSubSkullFrame;
			HUDResource.timerFrame = this._timerFrame;
			HUDResource.timerFrameWitchHardmodeLevel = this._timerFrameWitchHardmodeLevel;
			HUDResource.unlock = this._unlock;
			HUDResource.minimap = this._minimap;
		}

		// Token: 0x04000695 RID: 1685
		[SerializeField]
		private HUDResource.ResourceByMode _playerFrame;

		// Token: 0x04000696 RID: 1686
		[SerializeField]
		private HUDResource.ResourceByMode _playerDavyJonesFrame;

		// Token: 0x04000697 RID: 1687
		[SerializeField]
		private HUDResource.ResourceByMode _playerQuintessenceFrame;

		// Token: 0x04000698 RID: 1688
		[SerializeField]
		private HUDResource.ResourceByMode _playerSkill2Frame;

		// Token: 0x04000699 RID: 1689
		[SerializeField]
		private HUDResource.ResourceByMode _playerSubBarFrame;

		// Token: 0x0400069A RID: 1690
		[SerializeField]
		private HUDResource.ResourceByMode _playerSubSkill1Frame;

		// Token: 0x0400069B RID: 1691
		[SerializeField]
		private HUDResource.ResourceByMode _playerSubSkill2Frame;

		// Token: 0x0400069C RID: 1692
		[SerializeField]
		private HUDResource.ResourceByMode _playerSubSkullFrame;

		// Token: 0x0400069D RID: 1693
		[SerializeField]
		private HUDResource.ResourceByMode _timerFrame;

		// Token: 0x0400069E RID: 1694
		[SerializeField]
		private HUDResource.ResourceByMode _timerFrameWitchHardmodeLevel;

		// Token: 0x0400069F RID: 1695
		[SerializeField]
		private HUDResource.ResourceByMode _unlock;

		// Token: 0x040006A0 RID: 1696
		[SerializeField]
		private HUDResource.ResourceByMode _minimap;

		// Token: 0x02000181 RID: 385
		[Serializable]
		public sealed class ResourceByMode
		{
			// Token: 0x170001E0 RID: 480
			// (get) Token: 0x06000857 RID: 2135 RVA: 0x00018558 File Offset: 0x00016758
			public Sprite normal
			{
				get
				{
					return this._normal;
				}
			}

			// Token: 0x170001E1 RID: 481
			// (get) Token: 0x06000858 RID: 2136 RVA: 0x00018560 File Offset: 0x00016760
			public Sprite hard
			{
				get
				{
					return this._hard;
				}
			}

			// Token: 0x040006A1 RID: 1697
			[SerializeField]
			private Sprite _normal;

			// Token: 0x040006A2 RID: 1698
			[SerializeField]
			private Sprite _hard;
		}
	}
}
