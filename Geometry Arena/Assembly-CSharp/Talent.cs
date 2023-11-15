using System;
using UnityEngine;

// Token: 0x02000038 RID: 56
[Serializable]
public class Talent
{
	// Token: 0x170000BA RID: 186
	// (get) Token: 0x0600025E RID: 606 RVA: 0x0000DF23 File Offset: 0x0000C123
	// (set) Token: 0x0600025D RID: 605 RVA: 0x0000DF1A File Offset: 0x0000C11A
	public string Name
	{
		get
		{
			return LanguageText.Inst.GetFactorName(this.facs[1].type);
		}
		set
		{
			this.name = value;
		}
	}

	// Token: 0x0600025F RID: 607 RVA: 0x0000DF3C File Offset: 0x0000C13C
	public int CostWithCurLevel(int level)
	{
		float talent_BasicPrice = GameParameters.Inst.Talent_BasicPrice;
		float num = (float)this.price * talent_BasicPrice;
		if (this.facs[1].FactorTypeIsAbility())
		{
			num *= ((float)(level * level * level + 21 * level * level + 12 * level) / 80f + 1f) * 0.3f;
		}
		else
		{
			int type = this.facs[1].type;
			if (type == 70 || type == 80)
			{
				num *= (float)(level * 2 - 1);
				int jobId = TempData.inst.jobId;
				if (jobId % 3 == 0 && jobId <= 8)
				{
					num /= 3f;
				}
				else if (jobId % 3 == 1 && jobId <= 8)
				{
					num = num * 2f / 3f;
				}
			}
			else
			{
				Debug.LogError("Type??");
			}
		}
		return Mathf.Min(900000, num.RoundToInt());
	}

	// Token: 0x04000215 RID: 533
	private string name = "NAME_UNINITED";

	// Token: 0x04000216 RID: 534
	public int id = -1;

	// Token: 0x04000217 RID: 535
	public int job = -1;

	// Token: 0x04000218 RID: 536
	public int price = -1;

	// Token: 0x04000219 RID: 537
	public Talent.Fac[] facs = new Talent.Fac[4];

	// Token: 0x0400021A RID: 538
	public int originLevel = -1;

	// Token: 0x0400021B RID: 539
	public int maxLevel = -1;

	// Token: 0x02000146 RID: 326
	[Serializable]
	public class Fac
	{
		// Token: 0x060009BB RID: 2491 RVA: 0x00036B02 File Offset: 0x00034D02
		public bool FactorTypeIsAbility()
		{
			return this.type > 0 && this.type <= 15;
		}

		// Token: 0x0400099A RID: 2458
		public int type = -1;

		// Token: 0x0400099B RID: 2459
		public float num = -1f;
	}
}
