using System;
using UnityEngine;

// Token: 0x020009A6 RID: 2470
public interface ILaunchableRocket
{
	// Token: 0x17000560 RID: 1376
	// (get) Token: 0x06004984 RID: 18820
	LaunchableRocketRegisterType registerType { get; }

	// Token: 0x17000561 RID: 1377
	// (get) Token: 0x06004985 RID: 18821
	GameObject LaunchableGameObject { get; }

	// Token: 0x17000562 RID: 1378
	// (get) Token: 0x06004986 RID: 18822
	float rocketSpeed { get; }

	// Token: 0x17000563 RID: 1379
	// (get) Token: 0x06004987 RID: 18823
	bool isLanding { get; }
}
