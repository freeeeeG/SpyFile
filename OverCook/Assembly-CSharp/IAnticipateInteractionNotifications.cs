using System;
using UnityEngine;

// Token: 0x0200058B RID: 1419
public interface IAnticipateInteractionNotifications
{
	// Token: 0x06001AEF RID: 6895
	void OnInteractionAnticipationStart(InteractionType _type, GameObject _player);

	// Token: 0x06001AF0 RID: 6896
	void OnInteractionAnticipationEnded(InteractionType _type, GameObject _player);
}
