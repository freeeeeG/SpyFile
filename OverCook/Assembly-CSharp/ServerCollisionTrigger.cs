using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200012B RID: 299
public class ServerCollisionTrigger : ServerSynchroniserBase
{
	// Token: 0x06000579 RID: 1401 RVA: 0x0002A463 File Offset: 0x00028863
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_trigger = (CollisionTrigger)synchronisedObject;
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x0002A478 File Offset: 0x00028878
	private void OnTriggerEnter(Collider other)
	{
		if ((this.m_trigger.m_collisionFilter.value & 1 << other.gameObject.layer) != 0 && this.m_trigger.m_trigger != string.Empty)
		{
			base.gameObject.SendTrigger(this.m_trigger.m_trigger);
		}
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x0002A4DB File Offset: 0x000288DB
	private void OnCollisionEnter(Collision other)
	{
		this.OnTriggerEnter(other.collider);
	}

	// Token: 0x040004A9 RID: 1193
	private CollisionTrigger m_trigger;
}
