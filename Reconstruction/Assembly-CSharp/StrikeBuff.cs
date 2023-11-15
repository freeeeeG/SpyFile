using System;
using UnityEngine;

// Token: 0x02000034 RID: 52
public class StrikeBuff : TimeBuff
{
	// Token: 0x17000046 RID: 70
	// (get) Token: 0x0600012D RID: 301 RVA: 0x00006783 File Offset: 0x00004983
	public override EnemyBuffName BuffName
	{
		get
		{
			return EnemyBuffName.RuleStrikeBuff;
		}
	}

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x0600012E RID: 302 RVA: 0x00006787 File Offset: 0x00004987
	public override float BasicDuration
	{
		get
		{
			return 9999f;
		}
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x0600012F RID: 303 RVA: 0x0000678E File Offset: 0x0000498E
	public override float KeyValue
	{
		get
		{
			return 30f;
		}
	}

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x06000130 RID: 304 RVA: 0x00006795 File Offset: 0x00004995
	public override float KeyValue2
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x06000131 RID: 305 RVA: 0x0000679C File Offset: 0x0000499C
	public override bool IsTimeBase
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x06000132 RID: 306 RVA: 0x0000679F File Offset: 0x0000499F
	public override bool IsStackable
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000133 RID: 307 RVA: 0x000067A4 File Offset: 0x000049A4
	public override void Tick(float delta)
	{
		base.Tick(delta);
		this.timeCounter += Time.deltaTime;
		if (this.timeCounter > this.KeyValue)
		{
			this.timeCounter = 0f;
			this.Target.Flash(-(int)this.KeyValue2);
		}
	}

	// Token: 0x06000134 RID: 308 RVA: 0x000067F6 File Offset: 0x000049F6
	public override void Affect(int stacks)
	{
	}

	// Token: 0x06000135 RID: 309 RVA: 0x000067F8 File Offset: 0x000049F8
	public override void End()
	{
	}

	// Token: 0x04000116 RID: 278
	private float timeCounter;
}
