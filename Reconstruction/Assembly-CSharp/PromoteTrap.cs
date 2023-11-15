using System;
using UnityEngine;

// Token: 0x020001E6 RID: 486
public class PromoteTrap : TrapContent
{
	// Token: 0x06000C7B RID: 3195 RVA: 0x000208BB File Offset: 0x0001EABB
	public override void OnContentPass(Enemy enemy, GameTileContent content = null, int index = 0)
	{
		base.OnContentPass(enemy, content, index);
		enemy.DamageStrategy.TrapIntensify += Mathf.RoundToInt(1f * (1f + base.TrapIntensify));
	}
}
