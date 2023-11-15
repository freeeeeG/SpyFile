using System;
using UnityEngine;

// Token: 0x02000BA6 RID: 2982
public interface ITileFlipAnimatorProvider
{
	// Token: 0x06003D0C RID: 15628
	Animator Begin(FlipDirection _direction);

	// Token: 0x06003D0D RID: 15629
	void End(FlipDirection _direction);

	// Token: 0x06003D0E RID: 15630
	bool IsComplete();
}
