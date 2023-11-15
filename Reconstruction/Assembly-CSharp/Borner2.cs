using System;

// Token: 0x020000D1 RID: 209
public class Borner2 : Borner
{
	// Token: 0x17000251 RID: 593
	// (get) Token: 0x0600054A RID: 1354 RVA: 0x0000E959 File Offset: 0x0000CB59
	public override EnemyType EnemyType
	{
		get
		{
			return EnemyType.Borner2;
		}
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x0000E95D File Offset: 0x0000CB5D
	public override void OnDie()
	{
		base.OnDie();
		if (!Singleton<LevelManager>.Instance.LevelEnd)
		{
			Singleton<LevelManager>.Instance.SetAchievement("ACH_NEST");
		}
	}
}
