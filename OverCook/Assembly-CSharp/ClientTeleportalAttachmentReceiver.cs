using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005CB RID: 1483
public class ClientTeleportalAttachmentReceiver : ClientBaseTeleportalReceiver, IClientHandlePickup, IBaseHandlePickup
{
	// Token: 0x06001C45 RID: 7237 RVA: 0x00089FA4 File Offset: 0x000883A4
	protected override void Awake()
	{
		base.Awake();
		this.m_receiver = base.gameObject.RequireComponent<TeleportalAttachmentReceiver>();
	}

	// Token: 0x06001C46 RID: 7238 RVA: 0x00089FC0 File Offset: 0x000883C0
	protected override IEnumerator TeleportRoutine(ClientTeleportal _entrancePortal, IClientTeleportalSender _sender, IClientTeleportable _object)
	{
		GameObject obj = ((MonoBehaviour)_object).gameObject;
		ClientHandlePickupReferral pickupReferral = obj.RequireComponent<ClientHandlePickupReferral>();
		pickupReferral.SetHandlePickupReferree(this);
		Transform attachPoint = this.m_receiver.GetAttachPoint(obj);
		Transform prevParent = obj.transform.parent;
		obj.transform.SetParent(attachPoint, true);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.identity;
		obj.SetActive(true);
		IEnumerator routine = this.m_receiver.m_ReceiveAnimation.Run(obj, attachPoint, default(Vector3));
		while (routine.MoveNext())
		{
			yield return null;
		}
		this.m_receiver.OnAnimationFinished("Receive");
		ClientWorldObjectSynchroniser synchroniser = obj.RequestComponent<ClientWorldObjectSynchroniser>();
		if (synchroniser != null)
		{
			while (!synchroniser.IsReadyToResume())
			{
				yield return null;
			}
			synchroniser.Resume();
		}
		pickupReferral.SetHandlePickupReferree(null);
		obj.transform.localScale = Vector3.one;
		while (obj.transform.parent == this.m_receiver.GetAttachPoint(obj))
		{
			yield return null;
		}
		this.m_receiver.OnAnimationFinished("TeleportComplete");
		yield break;
	}

	// Token: 0x06001C47 RID: 7239 RVA: 0x00089FE2 File Offset: 0x000883E2
	public bool CanHandlePickup(ICarrier _carrier)
	{
		return false;
	}

	// Token: 0x06001C48 RID: 7240 RVA: 0x00089FE5 File Offset: 0x000883E5
	public int GetPickupPriority()
	{
		return int.MaxValue;
	}

	// Token: 0x0400161D RID: 5661
	private TeleportalAttachmentReceiver m_receiver;

	// Token: 0x0400161E RID: 5662
	private static int m_iIsFinished = Animator.StringToHash("IsFinished");

	// Token: 0x0400161F RID: 5663
	private static int m_iReceive = Animator.StringToHash("Receive");

	// Token: 0x04001620 RID: 5664
	private static int m_iReset = Animator.StringToHash("Reset");
}
