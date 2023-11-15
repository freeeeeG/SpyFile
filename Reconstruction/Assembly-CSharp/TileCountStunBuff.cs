using System;

// Token: 0x02000030 RID: 48
public class TileCountStunBuff : TileBuff
{
	// Token: 0x17000030 RID: 48
	// (get) Token: 0x0600010A RID: 266 RVA: 0x0000657F File Offset: 0x0000477F
	public override EnemyBuffName BuffName
	{
		get
		{
			return EnemyBuffName.TileCountStun;
		}
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x0600010B RID: 267 RVA: 0x00006582 File Offset: 0x00004782
	public override bool IsStackable
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x0600010C RID: 268 RVA: 0x00006585 File Offset: 0x00004785
	public override bool IsTimeBase
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x0600010D RID: 269 RVA: 0x00006588 File Offset: 0x00004788
	public override float BasicDuration
	{
		get
		{
			return 999f;
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x0600010E RID: 270 RVA: 0x0000658F File Offset: 0x0000478F
	public override float KeyValue
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x0600010F RID: 271 RVA: 0x00006596 File Offset: 0x00004796
	public override void Tick(float delta)
	{
		if (this.tileCount > 0 && this.Target.CurrentTile.Content.ContentType == GameTileContentType.Trap)
		{
			this.End();
			base.IsFinished = true;
			return;
		}
		this.tileCount++;
	}

	// Token: 0x06000110 RID: 272 RVA: 0x000065D5 File Offset: 0x000047D5
	public override void Affect(int stacks)
	{
		this.tileCount = 0;
	}

	// Token: 0x06000111 RID: 273 RVA: 0x000065DE File Offset: 0x000047DE
	public override void End()
	{
		this.Target.DamageStrategy.StunTime += (float)this.PrivateStacks * this.KeyValue * (float)this.tileCount;
	}

	// Token: 0x04000115 RID: 277
	private int tileCount;
}
