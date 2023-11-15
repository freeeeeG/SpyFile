using System;
using System.Collections.Generic;

// Token: 0x02000056 RID: 86
public abstract class ElementSkill : TurretSkill
{
	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x06000229 RID: 553 RVA: 0x00007E98 File Offset: 0x00006098
	public virtual List<int> InitElements { get; }

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x0600022A RID: 554 RVA: 0x00007EA0 File Offset: 0x000060A0
	// (set) Token: 0x0600022B RID: 555 RVA: 0x00007EA8 File Offset: 0x000060A8
	public List<int> Elements { get; set; }

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x0600022C RID: 556 RVA: 0x00007EB1 File Offset: 0x000060B1
	// (set) Token: 0x0600022D RID: 557 RVA: 0x00007EB9 File Offset: 0x000060B9
	public bool IsException { get; set; }

	// Token: 0x0600022E RID: 558 RVA: 0x00007EC4 File Offset: 0x000060C4
	public override void Build()
	{
		base.Build();
		using (List<int>.Enumerator enumerator = this.Elements.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				switch (enumerator.Current % 10)
				{
				case 0:
					this.strategy.BaseGoldCount++;
					break;
				case 1:
					this.strategy.BaseWoodCount++;
					break;
				case 2:
					this.strategy.BaseWaterCount++;
					break;
				case 3:
					this.strategy.BaseFireCount++;
					break;
				case 4:
					this.strategy.BaseDustCount++;
					break;
				}
			}
		}
	}
}
