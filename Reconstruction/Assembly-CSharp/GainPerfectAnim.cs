using System;

// Token: 0x02000174 RID: 372
public class GainPerfectAnim : ReusableObject
{
	// Token: 0x0600097D RID: 2429 RVA: 0x000191F3 File Offset: 0x000173F3
	public void ReclaimSelf()
	{
		Singleton<ObjectPool>.Instance.UnSpawn(this);
	}
}
