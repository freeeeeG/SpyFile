using System;

// Token: 0x020000B0 RID: 176
public abstract class ABossStageScript : AStageScript
{
	// Token: 0x060003B2 RID: 946 RVA: 0x0000EAB1 File Offset: 0x0000CCB1
	protected virtual void Awake()
	{
		this.monster = base.GetComponent<AMonsterBase>();
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x0000EABF File Offset: 0x0000CCBF
	public bool IsBossDead()
	{
		return this.monster.IsDead();
	}

	// Token: 0x040003CB RID: 971
	private AMonsterBase monster;
}
