using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200019E RID: 414
public class ServerTriggerWithCooldown : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x06000705 RID: 1797 RVA: 0x0002E33F File Offset: 0x0002C73F
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerWithCooldownBase = (TriggerWithCooldown)synchronisedObject;
		this.m_timeSinceLastTrigger = this.m_triggerWithCooldownBase.m_Cooldown;
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x0002E365 File Offset: 0x0002C765
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		this.m_timeSinceLastTrigger += TimeManager.GetDeltaTime(base.gameObject);
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x0002E388 File Offset: 0x0002C788
	public void OnTrigger(string _trigger)
	{
		if (_trigger == this.m_triggerWithCooldownBase.m_InTrigger && this.m_timeSinceLastTrigger >= this.m_triggerWithCooldownBase.m_Cooldown)
		{
			base.gameObject.SendTrigger(this.m_triggerWithCooldownBase.m_OutTrigger);
			this.m_timeSinceLastTrigger = 0f;
		}
	}

	// Token: 0x040005DA RID: 1498
	private TriggerWithCooldown m_triggerWithCooldownBase;

	// Token: 0x040005DB RID: 1499
	private float m_timeSinceLastTrigger;
}
