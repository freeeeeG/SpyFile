using System;

// Token: 0x020001E0 RID: 480
public class ExcutionTrap : TrapContent
{
	// Token: 0x06000C6C RID: 3180 RVA: 0x00020594 File Offset: 0x0001E794
	public override void OnContentPass(Enemy enemy, GameTileContent content = null, int index = 0)
	{
		base.OnContentPass(enemy, content, index);
		float amount = (enemy.DamageStrategy.MaxHealth - enemy.DamageStrategy.CurrentHealth) * 0.12f * (1f + base.TrapIntensify + (float)enemy.DamageStrategy.TrapIntensify) / (1f - enemy.DamageStrategy.HiddenResist);
		float num;
		enemy.DamageStrategy.ApplyDamage(amount, out num, null, false);
		Singleton<StaticData>.Instance.ShowJumpDamage(enemy.model.position, (long)num, true);
		this.DamageAnalysis += (long)num;
		enemy.DamageStrategy.TrapIntensify = 0;
	}
}
