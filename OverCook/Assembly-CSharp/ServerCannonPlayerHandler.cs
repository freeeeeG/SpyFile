using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200044E RID: 1102
public class ServerCannonPlayerHandler : ServerSynchroniserBase, IServerCannonHandler
{
	// Token: 0x06001458 RID: 5208 RVA: 0x0006EFC0 File Offset: 0x0006D3C0
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_cannon = synchronisedObject.gameObject.RequireComponent<ServerCannon>();
	}

	// Token: 0x06001459 RID: 5209 RVA: 0x0006EFDA File Offset: 0x0006D3DA
	public bool CanHandle(GameObject _obj)
	{
		return !(_obj == null) && _obj.GetComponent<PlayerControls>() != null;
	}

	// Token: 0x0600145A RID: 5210 RVA: 0x0006EFF8 File Offset: 0x0006D3F8
	public void ExitCannonRoutine(GameObject _obj)
	{
		_obj.GetComponent<Rigidbody>().isKinematic = false;
		GroundCast groundCast = _obj.RequestComponent<GroundCast>();
		if (groundCast != null)
		{
			groundCast.ForceUpdateNow();
		}
		ServerWorldObjectSynchroniser serverWorldObjectSynchroniser = _obj.RequestComponent<ServerWorldObjectSynchroniser>();
		if (serverWorldObjectSynchroniser != null)
		{
			serverWorldObjectSynchroniser.ResumeAllClients(false);
		}
	}

	// Token: 0x0600145B RID: 5211 RVA: 0x0006F044 File Offset: 0x0006D444
	public void Load(GameObject _obj)
	{
	}

	// Token: 0x0600145C RID: 5212 RVA: 0x0006F046 File Offset: 0x0006D446
	public void Unload(GameObject _obj)
	{
	}

	// Token: 0x0600145D RID: 5213 RVA: 0x0006F048 File Offset: 0x0006D448
	public bool IsFlying()
	{
		return this.m_cannon.IsFlying();
	}

	// Token: 0x04000FC5 RID: 4037
	private ServerCannon m_cannon;
}
