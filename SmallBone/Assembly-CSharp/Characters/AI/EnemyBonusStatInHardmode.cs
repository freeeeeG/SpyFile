using System;
using Data;
using Level;

namespace Characters.AI
{
	// Token: 0x020010F0 RID: 4336
	public sealed class EnemyBonusStatInHardmode
	{
		// Token: 0x0600543E RID: 21566 RVA: 0x000FC2C8 File Offset: 0x000FA4C8
		public void AttachTo(Character character)
		{
			if (!GameData.HardmodeProgress.hardmode)
			{
				return;
			}
			HardmodeLevelInfo.EnemyStatInfo enemyStatInfoByType = HardmodeLevelInfo.instance.GetEnemyStatInfoByType(character.type);
			Stat.Values values = new Stat.Values(new Stat.Value[]
			{
				new Stat.Value(Stat.Category.Percent, Stat.Kind.AttackDamage, (double)enemyStatInfoByType.attackMultiplier),
				new Stat.Value(Stat.Category.Percent, Stat.Kind.Health, (double)enemyStatInfoByType.healthMultiplier)
			});
			character.stat.AttachValues(values);
			character.stat.SetNeedUpdate();
		}
	}
}
