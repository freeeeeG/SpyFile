using System;

// Token: 0x02000028 RID: 40
public abstract class EnemyBuff
{
	// Token: 0x1700000A RID: 10
	// (get) Token: 0x060000C4 RID: 196
	public abstract EnemyBuffName BuffName { get; }

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x060000C5 RID: 197 RVA: 0x000061E6 File Offset: 0x000043E6
	// (set) Token: 0x060000C6 RID: 198 RVA: 0x000061EE File Offset: 0x000043EE
	public bool IsFinished { get; set; }

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x060000C7 RID: 199
	public abstract bool IsTimeBase { get; }

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x060000C8 RID: 200
	public abstract bool IsStackable { get; }

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x060000C9 RID: 201 RVA: 0x000061F7 File Offset: 0x000043F7
	public virtual int MaxStacks
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x060000CA RID: 202 RVA: 0x000061FA File Offset: 0x000043FA
	public virtual float KeyValue { get; }

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x060000CB RID: 203 RVA: 0x00006202 File Offset: 0x00004402
	public virtual float KeyValue2 { get; }

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x060000CC RID: 204 RVA: 0x0000620A File Offset: 0x0000440A
	public virtual float BasicDuration { get; }

	// Token: 0x060000CD RID: 205 RVA: 0x00006212 File Offset: 0x00004412
	public virtual void ApplyBuff(Enemy target, int stacks, bool isAbnormal = false)
	{
		this.Target = target;
		this.IsAbnormal = isAbnormal;
	}

	// Token: 0x060000CE RID: 206
	public abstract void Affect(int stacks);

	// Token: 0x060000CF RID: 207 RVA: 0x00006222 File Offset: 0x00004422
	public virtual void Tick(float delta)
	{
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x00006224 File Offset: 0x00004424
	public virtual void OnHit()
	{
	}

	// Token: 0x060000D1 RID: 209
	public abstract void End();

	// Token: 0x04000108 RID: 264
	public int CurrentStack;

	// Token: 0x0400010B RID: 267
	public Enemy Target;

	// Token: 0x0400010C RID: 268
	public float Duration;

	// Token: 0x0400010D RID: 269
	public bool IsAbnormal;
}
