using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200018C RID: 396
public class ServerTriggerOnObject : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x060006CE RID: 1742 RVA: 0x0002DA6B File Offset: 0x0002BE6B
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerOnObject = (TriggerOnObject)synchronisedObject;
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x0002DA80 File Offset: 0x0002BE80
	public void OnTrigger(string _trigger)
	{
		if (this.m_triggerOnObject.m_trigger == _trigger)
		{
			if (this.m_triggerOnObject.m_targetObject != null)
			{
				this.m_triggerOnObject.m_targetObject.SendTrigger(this.m_triggerOnObject.m_triggerToFire);
			}
			for (int i = 0; i < this.m_triggerOnObject.m_targetObjects.Length; i++)
			{
				if (this.m_triggerOnObject.m_targetObjects[i] != null)
				{
					this.m_triggerOnObject.m_targetObjects[i].SendTrigger(this.m_triggerOnObject.m_triggerToFire);
				}
			}
		}
	}

	// Token: 0x040005A4 RID: 1444
	private TriggerOnObject m_triggerOnObject;
}
