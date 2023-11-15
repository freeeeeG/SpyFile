using System;
using UnityEngine;

// Token: 0x0200016C RID: 364
public class RuleMarkTrap : Rule
{
	// Token: 0x1700035C RID: 860
	// (get) Token: 0x0600095E RID: 2398 RVA: 0x00018CED File Offset: 0x00016EED
	public override bool Add
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700035D RID: 861
	// (get) Token: 0x0600095F RID: 2399 RVA: 0x00018CF0 File Offset: 0x00016EF0
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_MARKTRAP;
		}
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x00018CF4 File Offset: 0x00016EF4
	public override void OnGameInit()
	{
		GameTile specificTrap = ConstructHelper.GetSpecificTrap("MARKTRAP");
		specificTrap.transform.position = new Vector3Int(0, -1, 0);
		specificTrap.TileLanded();
		((TrapContent)specificTrap.Content).RevealTrap();
	}
}
