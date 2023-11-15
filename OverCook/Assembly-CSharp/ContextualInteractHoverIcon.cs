using System;
using UnityEngine;

// Token: 0x020003A5 RID: 933
public class ContextualInteractHoverIcon : ButtonHoverIcon, IAnticipateInteractionNotifications
{
	// Token: 0x0600117F RID: 4479 RVA: 0x000645F0 File Offset: 0x000629F0
	public void OnInteractionAnticipationStart(InteractionType _type, GameObject _player)
	{
		if (_type == InteractionType.Interact)
		{
			this.m_interactionCount++;
			base.SetVisibility(true);
		}
	}

	// Token: 0x06001180 RID: 4480 RVA: 0x0006460E File Offset: 0x00062A0E
	public void OnInteractionAnticipationEnded(InteractionType _type, GameObject _player)
	{
		if (_type == InteractionType.Interact)
		{
			this.m_interactionCount--;
			if (this.m_interactionCount == 0)
			{
				base.SetVisibility(false);
			}
		}
	}

	// Token: 0x04000D9F RID: 3487
	private int m_interactionCount;
}
