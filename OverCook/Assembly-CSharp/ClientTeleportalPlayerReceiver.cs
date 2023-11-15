using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005D1 RID: 1489
public class ClientTeleportalPlayerReceiver : ClientBaseTeleportalReceiver
{
	// Token: 0x06001C69 RID: 7273 RVA: 0x0008AAC0 File Offset: 0x00088EC0
	protected override void Awake()
	{
		base.Awake();
		if (ClientTeleportalPlayerReceiver.c_groundCastMask == 0)
		{
			ClientTeleportalPlayerReceiver.c_groundCastMask = LayerMask.GetMask(new string[]
			{
				"Ground",
				"SlopedGround"
			});
		}
		this.m_receiver = base.gameObject.RequireComponent<TeleportalPlayerReceiver>();
	}

	// Token: 0x06001C6A RID: 7274 RVA: 0x0008AB18 File Offset: 0x00088F18
	protected override IEnumerator TeleportRoutine(ClientTeleportal _entrancePortal, IClientTeleportalSender _sender, IClientTeleportable _object)
	{
		GameObject root = ((MonoBehaviour)_object).gameObject;
		PlayerControls controls = root.RequestComponentUpwardsRecursive<PlayerControls>();
		root = controls.gameObject;
		Transform attachPoint = this.m_receiver.GetAttachPoint(root);
		root.transform.SetParent(attachPoint);
		root.transform.rotation = Quaternion.identity;
		root.transform.localScale = Vector3.one;
		Collider collider = root.RequestComponent<Collider>();
		Vector3 offset = default(Vector3);
		RaycastHit raycastInfo;
		if (this.m_receiver.m_groundPlayer && Physics.Raycast(new Ray(attachPoint.position, -Vector3.up), out raycastInfo, 2f, ClientTeleportalPlayerReceiver.c_groundCastMask))
		{
			offset = raycastInfo.point - attachPoint.position;
			root.transform.SetPositionAndRotation(raycastInfo.point, attachPoint.rotation);
		}
		else
		{
			Vector3 position = root.transform.position + (attachPoint.position - collider.bounds.center);
			root.transform.SetPositionAndRotation(position, attachPoint.rotation);
			offset = collider.bounds.extents;
		}
		Animator mesh = root.RequestComponentRecursive<Animator>();
		if (mesh != null)
		{
			mesh.gameObject.SetActive(true);
		}
		IEnumerator routine = this.m_receiver.m_ReceiverAnimation.Run(root, attachPoint, offset);
		while (routine.MoveNext())
		{
			yield return null;
		}
		root.transform.SetParent(null, true);
		DynamicLandscapeParenting dynamicParenting = root.RequestComponent<DynamicLandscapeParenting>();
		if (dynamicParenting != null)
		{
			dynamicParenting.enabled = true;
		}
		collider.enabled = true;
		this.m_receiver.OnAnimationFinished("Receive");
		ClientWorldObjectSynchroniser synchroniser = root.RequestComponent<ClientWorldObjectSynchroniser>();
		if (synchroniser != null)
		{
			yield return null;
			while (!synchroniser.IsReadyToResume())
			{
				yield return null;
			}
			synchroniser.Resume();
		}
		controls.enabled = true;
		controls.Motion.SetKinematic(false);
		ParticleSystem pfx = root.RequestComponentRecursive<ParticleSystem>();
		if (pfx != null)
		{
			pfx.Play();
		}
		yield break;
	}

	// Token: 0x04001632 RID: 5682
	private TeleportalPlayerReceiver m_receiver;

	// Token: 0x04001633 RID: 5683
	private const float c_maxGroundCastDistance = 2f;

	// Token: 0x04001634 RID: 5684
	private static LayerMask c_groundCastMask = 0;

	// Token: 0x04001635 RID: 5685
	private Generic<bool> m_canTeleportCallback = () => true;

	// Token: 0x04001636 RID: 5686
	private ClientTeleportCallback m_teleportStartedCallback = delegate(IClientTeleportable _object)
	{
	};

	// Token: 0x04001637 RID: 5687
	private ClientTeleportCallback m_teleportFinishedCallback = delegate(IClientTeleportable _object)
	{
	};

	// Token: 0x04001638 RID: 5688
	private static readonly int m_iReceive = Animator.StringToHash("Receive");

	// Token: 0x04001639 RID: 5689
	private static readonly int m_iReset = Animator.StringToHash("Reset");

	// Token: 0x0400163A RID: 5690
	private static readonly int m_iIsFinished = Animator.StringToHash("IsFinished");
}
