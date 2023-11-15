using System;

// Token: 0x020001ED RID: 493
public interface ILogicalButton : ILogicalElement
{
	// Token: 0x0600082C RID: 2092
	bool JustPressed();

	// Token: 0x0600082D RID: 2093
	bool JustReleased();

	// Token: 0x0600082E RID: 2094
	bool HasUnclaimedPressEvent();

	// Token: 0x0600082F RID: 2095
	void ClaimPressEvent();

	// Token: 0x06000830 RID: 2096
	bool HasUnclaimedReleaseEvent();

	// Token: 0x06000831 RID: 2097
	void ClaimReleaseEvent();

	// Token: 0x06000832 RID: 2098
	float GetHeldTimeLength();

	// Token: 0x06000833 RID: 2099
	bool IsDown();
}
