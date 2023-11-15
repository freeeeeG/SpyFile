using System;
using System.Collections;
using Team17.Online;

// Token: 0x02000718 RID: 1816
public interface IPlayerManager
{
	// Token: 0x06002284 RID: 8836
	GamepadUser GetUser(EngagementSlot _slot);

	// Token: 0x06002285 RID: 8837
	ControlPadInput.PadNum GetEngagementPad(out EngagmentCircumstances o_circumstances);

	// Token: 0x06002286 RID: 8838
	bool HasPlayer();

	// Token: 0x06002287 RID: 8839
	bool HasSavablePlayer();

	// Token: 0x06002288 RID: 8840
	bool IsEngagingSlot(EngagementSlot slot);

	// Token: 0x06002289 RID: 8841
	bool HasFreeEngagementSlot();

	// Token: 0x0600228A RID: 8842
	bool IsBusy();

	// Token: 0x0600228B RID: 8843
	bool IsWarningActive(PlayerWarning warning);

	// Token: 0x0600228C RID: 8844
	IEnumerator RunGameownerEngagement(ControlPadInput.PadNum _engagingPadNum, EngagmentCircumstances _circumstances);

	// Token: 0x0600228D RID: 8845
	void StartGameownerEngagement(ControlPadInput.PadNum _engagingPadNum, EngagmentCircumstances _circumstances, VoidGeneric<GamepadUser> _finishedCall);

	// Token: 0x0600228E RID: 8846
	void StartPadEngagement(ControlPadInput.PadNum _engagingPadNum, EngagmentCircumstances _circumstances, VoidGeneric<GamepadUser> _finishedCall);

	// Token: 0x0600228F RID: 8847
	void DisengagePad(EngagementSlot _intendedSlot);

	// Token: 0x06002290 RID: 8848
	void ShowGamerCard(EngagementSlot slot);

	// Token: 0x06002291 RID: 8849
	void ShowGamerCard(GamepadUser localUser);

	// Token: 0x06002292 RID: 8850
	void ShowGamerCard(OnlineUserPlatformId onlineUser);

	// Token: 0x06002293 RID: 8851
	bool SupportsInvitesForAnyUser();

	// Token: 0x14000026 RID: 38
	// (add) Token: 0x06002294 RID: 8852
	// (remove) Token: 0x06002295 RID: 8853
	event VoidGeneric<EngagementSlot, GamepadUser, GamepadUser> EngagementChangeCallback;
}
