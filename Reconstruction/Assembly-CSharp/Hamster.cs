using System;
using System.Collections.Generic;

// Token: 0x020000DA RID: 218
public class Hamster : Boss
{
	// Token: 0x1700025A RID: 602
	// (get) Token: 0x0600056E RID: 1390 RVA: 0x0000EDEC File Offset: 0x0000CFEC
	public override EnemyType EnemyType
	{
		get
		{
			return EnemyType.Hamster;
		}
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x0000EDF0 File Offset: 0x0000CFF0
	public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
	{
		base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
		if (!Hamster.isFirstHamster)
		{
			Hamster.isFirstHamster = true;
			Hamster.AllHamsters = new List<Hamster>();
		}
		base.DamageStrategy.ApplyBuffDmgIntensify(-0.75f);
		Hamster.AllHamsters.Add(this);
		base.ShowBossText(0.3f);
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x0000EE49 File Offset: 0x0000D049
	protected override void SetStrategy()
	{
		base.DamageStrategy = new HamsterStrategy(this);
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x0000EE57 File Offset: 0x0000D057
	public override void OnDie()
	{
		base.OnDie();
		this.HamsterReduce();
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x0000EE68 File Offset: 0x0000D068
	private void HamsterReduce()
	{
		Hamster.AllHamsters.Remove(this);
		foreach (Hamster hamster in Hamster.AllHamsters)
		{
			hamster.DamageStrategy.ApplyBuffDmgIntensify(0.25f);
		}
		if (Hamster.AllHamsters.Count <= 0)
		{
			Singleton<LevelManager>.Instance.SetAchievement("ACH_HAMSTER");
		}
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x0000EEEC File Offset: 0x0000D0EC
	public override void EnemyExit()
	{
		this.HamsterReduce();
		base.EnemyExit();
	}

	// Token: 0x04000245 RID: 581
	public static bool isFirstHamster;

	// Token: 0x04000246 RID: 582
	public static List<Hamster> AllHamsters;
}
