using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005DF RID: 1503
public class ServerTriggerKillAttachments : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x06001CB1 RID: 7345 RVA: 0x0008C3A0 File Offset: 0x0008A7A0
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerKillAttachments = (TriggerKillAttachments)synchronisedObject;
	}

	// Token: 0x06001CB2 RID: 7346 RVA: 0x0008C3B5 File Offset: 0x0008A7B5
	public void OnTrigger(string _trigger)
	{
		if (_trigger == this.m_triggerKillAttachments.m_trigger)
		{
			this.KillAttachments(base.gameObject);
		}
	}

	// Token: 0x06001CB3 RID: 7347 RVA: 0x0008C3DC File Offset: 0x0008A7DC
	private void KillAttachments(GameObject _root)
	{
		bool flag = MaskUtils.HasFlag<TriggerKillAttachments.KillMode>(this.m_triggerKillAttachments.m_killMode, TriggerKillAttachments.KillMode.Loose);
		if (flag)
		{
			GameObject gameObject = base.gameObject;
			IAttachment[] componentsInChildren = gameObject.GetComponentsInChildren<IAttachment>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != null && !(componentsInChildren[i].AccessGameObject() == null))
				{
					if (!componentsInChildren[i].IsAttached())
					{
						ServerPlayerRespawnManager.KillOrRespawn(componentsInChildren[i].AccessGameObject(), null);
					}
				}
			}
		}
		bool flag2 = MaskUtils.HasFlag<TriggerKillAttachments.KillMode>(this.m_triggerKillAttachments.m_killMode, TriggerKillAttachments.KillMode.Attached);
		if (flag2)
		{
			ServerAttachStation[] array = _root.RequestComponentsRecursive<ServerAttachStation>();
			for (int j = 0; j < array.Length; j++)
			{
				if (!(array[j] == null) && !(array[j].gameObject == null))
				{
					if (array[j].HasItem())
					{
						GameObject gameObject2 = array[j].TakeItem();
						if (gameObject2 != null)
						{
							ServerPlayerRespawnManager.KillOrRespawn(gameObject2, null);
						}
					}
				}
			}
		}
	}

	// Token: 0x0400165F RID: 5727
	private TriggerKillAttachments m_triggerKillAttachments;
}
