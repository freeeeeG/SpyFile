using System;

// Token: 0x02000173 RID: 371
public class GainCoinAnim : ReusableObject
{
	// Token: 0x0600097B RID: 2427 RVA: 0x000191DE File Offset: 0x000173DE
	public void ReclaimSelf()
	{
		Singleton<ObjectPool>.Instance.UnSpawn(this);
	}
}
