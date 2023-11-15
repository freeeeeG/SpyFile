using System;

// Token: 0x020008EC RID: 2284
public interface IMultiplayerTask
{
	// Token: 0x06002C67 RID: 11367
	void Start(object startData);

	// Token: 0x06002C68 RID: 11368
	void Stop();

	// Token: 0x06002C69 RID: 11369
	void Update();

	// Token: 0x06002C6A RID: 11370
	IConnectionModeSwitchStatus GetStatus();

	// Token: 0x06002C6B RID: 11371
	object GetData();
}
