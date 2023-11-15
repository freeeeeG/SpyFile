using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005CE RID: 1486
public class ClientTeleportalAttachmentSender : ClientBaseTeleportalSender, IClientHandlePickup, IBaseHandlePickup
{
	// Token: 0x06001C59 RID: 7257 RVA: 0x0008A60E File Offset: 0x00088A0E
	protected override void Awake()
	{
		base.Awake();
		this.m_sender = base.gameObject.RequireComponent<TeleportalAttachmentSender>();
	}

	// Token: 0x06001C5A RID: 7258 RVA: 0x0008A628 File Offset: 0x00088A28
	protected override IEnumerator TeleportRoutine(ClientTeleportal _entrancePortal, IClientTeleportalReceiver _receiver, IClientTeleportable _object)
	{
		GameObject obj = ((MonoBehaviour)_object).gameObject;
		Transform attachPoint = this.m_sender.GetAttachPoint(obj);
		while (obj.transform.parent != attachPoint)
		{
			yield return null;
		}
		ClientWorldObjectSynchroniser synchroniser = obj.RequestComponent<ClientWorldObjectSynchroniser>();
		if (synchroniser != null)
		{
			synchroniser.Pause();
		}
		ClientHandlePickupReferral pickupReferral = obj.RequireComponent<ClientHandlePickupReferral>();
		pickupReferral.SetHandlePickupReferree(this);
		IEnumerator routine = this.m_sender.m_TeleportAnimation.Run(obj, attachPoint, default(Vector3));
		while (routine.MoveNext())
		{
			yield return null;
		}
		this.m_sender.OnAnimationFinished("Teleport");
		pickupReferral.SetHandlePickupReferree(null);
		obj.transform.SetParent(null, true);
		obj.SetActive(false);
		yield break;
	}

	// Token: 0x06001C5B RID: 7259 RVA: 0x0008A64A File Offset: 0x00088A4A
	public bool CanHandlePickup(ICarrier _carrier)
	{
		return false;
	}

	// Token: 0x06001C5C RID: 7260 RVA: 0x0008A64D File Offset: 0x00088A4D
	public int GetPickupPriority()
	{
		return int.MaxValue;
	}

	// Token: 0x04001628 RID: 5672
	private TeleportalAttachmentSender m_sender;

	// Token: 0x04001629 RID: 5673
	private static int m_iTeleport = Animator.StringToHash("Teleport");

	// Token: 0x0400162A RID: 5674
	private static int m_iIsFinished = Animator.StringToHash("IsFinished");

	// Token: 0x0400162B RID: 5675
	private static int m_iReset = Animator.StringToHash("Reset");
}
