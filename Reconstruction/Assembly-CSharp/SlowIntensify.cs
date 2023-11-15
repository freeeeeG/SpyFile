using System;

// Token: 0x02000032 RID: 50
public class SlowIntensify : TimeBuff
{
	// Token: 0x1700003A RID: 58
	// (get) Token: 0x0600011B RID: 283 RVA: 0x00006694 File Offset: 0x00004894
	public override EnemyBuffName BuffName
	{
		get
		{
			return EnemyBuffName.SlowIntensify;
		}
	}

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x0600011C RID: 284 RVA: 0x00006698 File Offset: 0x00004898
	public override bool IsStackable
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x0600011D RID: 285 RVA: 0x0000669B File Offset: 0x0000489B
	public override bool IsTimeBase
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x0600011E RID: 286 RVA: 0x0000669E File Offset: 0x0000489E
	public override int MaxStacks
	{
		get
		{
			return 100;
		}
	}

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x0600011F RID: 287 RVA: 0x000066A2 File Offset: 0x000048A2
	public override float KeyValue
	{
		get
		{
			return 0.01f;
		}
	}

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x06000120 RID: 288 RVA: 0x000066A9 File Offset: 0x000048A9
	public override float BasicDuration
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x06000121 RID: 289 RVA: 0x000066B0 File Offset: 0x000048B0
	public override void Affect(int stacks)
	{
		this.Target.DamageStrategy.FrostIntensify += this.KeyValue * (float)stacks;
	}

	// Token: 0x06000122 RID: 290 RVA: 0x000066D2 File Offset: 0x000048D2
	public override void End()
	{
		this.Target.DamageStrategy.FrostIntensify -= this.KeyValue * (float)this.CurrentStack;
	}
}
