using System;
using UnityEngine;

// Token: 0x020003A6 RID: 934
public class ContextualPickupHoverIcon : ButtonHoverIcon, IAnticipateInteractionNotifications
{
	// Token: 0x06001182 RID: 4482 RVA: 0x0006463F File Offset: 0x00062A3F
	public void OnInteractionAnticipationStart(InteractionType _type, GameObject _player)
	{
		if (_type == InteractionType.Pickup)
		{
			this.m_interactionCount++;
			base.SetVisibility(true);
		}
	}

	// Token: 0x06001183 RID: 4483 RVA: 0x0006465C File Offset: 0x00062A5C
	public void OnInteractionAnticipationEnded(InteractionType _type, GameObject _player)
	{
		if (_type == InteractionType.Pickup)
		{
			this.m_interactionCount--;
			if (this.m_interactionCount == 0)
			{
				base.SetVisibility(false);
			}
		}
	}

	// Token: 0x04000DA0 RID: 3488
	private int m_interactionCount;
}
