using System;
using UnityEngine;

// Token: 0x0200016B RID: 363
public class RuleBonusTrap : Rule
{
	// Token: 0x1700035B RID: 859
	// (get) Token: 0x0600095B RID: 2395 RVA: 0x00018CA8 File Offset: 0x00016EA8
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_BONUSTRAP;
		}
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x00018CAC File Offset: 0x00016EAC
	public override void OnGameInit()
	{
		GameTile specificTrap = ConstructHelper.GetSpecificTrap("BONUSTRAP");
		specificTrap.transform.position = new Vector3Int(0, 1, 0);
		specificTrap.TileLanded();
		((TrapContent)specificTrap.Content).RevealTrap();
	}
}
