using System;

// Token: 0x020004C8 RID: 1224
public class KnockKnock : Activatable
{
	// Token: 0x06001BEA RID: 7146 RVA: 0x00094D4F File Offset: 0x00092F4F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = false;
	}

	// Token: 0x06001BEB RID: 7147 RVA: 0x00094D5E File Offset: 0x00092F5E
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		if (!this.doorAnswered)
		{
			this.workTimeRemaining += dt;
		}
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06001BEC RID: 7148 RVA: 0x00094D7E File Offset: 0x00092F7E
	public void AnswerDoor()
	{
		this.doorAnswered = true;
		this.workTimeRemaining = 1f;
	}

	// Token: 0x04000F72 RID: 3954
	private bool doorAnswered;
}
