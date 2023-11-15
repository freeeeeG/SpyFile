using System;
using UnityEngine;

// Token: 0x020001DD RID: 477
public class BonusTrap : TrapContent
{
	// Token: 0x06000C67 RID: 3175 RVA: 0x0002049C File Offset: 0x0001E69C
	public override void OnContentPass(Enemy enemy, GameTileContent content = null, int index = 0)
	{
		base.OnContentPass(enemy, content, index);
		if (content == null)
		{
			content = this;
		}
		int num = Mathf.RoundToInt(2f * (1f + base.TrapIntensify + (float)enemy.DamageStrategy.TrapIntensify));
		((TrapContent)content).CoinAnalysis += num;
		((TrapContent)content).CoinGainThisTurn += num;
		Singleton<StaticData>.Instance.GainMoneyEffect(enemy.model.position, num);
		enemy.DamageStrategy.TrapIntensify = 0;
	}
}
