using System;
using Data;
using Singletons;

namespace Hardmode
{
	// Token: 0x0200014F RID: 335
	public sealed class HardmodeManager : Singleton<HardmodeManager>
	{
		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x000134C9 File Offset: 0x000116C9
		public int _maxLevel
		{
			get
			{
				return GameData.HardmodeProgress.maxLevel;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x000134D0 File Offset: 0x000116D0
		public int currentLevel
		{
			get
			{
				return GameData.HardmodeProgress.hardmodeLevel;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x000134D7 File Offset: 0x000116D7
		public int clearedLevel
		{
			get
			{
				return GameData.HardmodeProgress.clearedLevel;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x000134DE File Offset: 0x000116DE
		public bool hardmode
		{
			get
			{
				return GameData.HardmodeProgress.hardmode;
			}
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x000134E5 File Offset: 0x000116E5
		protected override void Awake()
		{
			base.Awake();
			base.name = "HardmodeManager";
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x000134F8 File Offset: 0x000116F8
		public void SetLevel(int level)
		{
			GameData.HardmodeProgress.hardmodeLevel = level;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00013500 File Offset: 0x00011700
		public HardmodeManager.EnemyStep GetEnemyStep()
		{
			if (!this.hardmode)
			{
				return HardmodeManager.EnemyStep.Normal;
			}
			if (this.currentLevel < 4)
			{
				return HardmodeManager.EnemyStep.A;
			}
			if (this.currentLevel < 8)
			{
				return HardmodeManager.EnemyStep.B;
			}
			return HardmodeManager.EnemyStep.C;
		}

		// Token: 0x02000150 RID: 336
		public enum EnemyStep
		{
			// Token: 0x040004E3 RID: 1251
			Normal,
			// Token: 0x040004E4 RID: 1252
			A,
			// Token: 0x040004E5 RID: 1253
			B,
			// Token: 0x040004E6 RID: 1254
			C
		}
	}
}
