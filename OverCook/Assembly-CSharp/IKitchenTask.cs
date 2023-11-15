using System;

// Token: 0x02000AA4 RID: 2724
public interface IKitchenTask
{
	// Token: 0x170003BF RID: 959
	// (get) Token: 0x060035DC RID: 13788
	bool isRunning { get; }

	// Token: 0x060035DD RID: 13789
	void Start();

	// Token: 0x060035DE RID: 13790
	void Update();

	// Token: 0x060035DF RID: 13791
	KitchenTaskStatus GetStatus();
}
