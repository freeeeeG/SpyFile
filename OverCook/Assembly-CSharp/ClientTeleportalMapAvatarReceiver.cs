using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000BCE RID: 3022
public class ClientTeleportalMapAvatarReceiver : ClientBaseTeleportalReceiver
{
	// Token: 0x06003DC8 RID: 15816 RVA: 0x001268E0 File Offset: 0x00124CE0
	protected override void Awake()
	{
		base.Awake();
		if (ClientTeleportalMapAvatarReceiver.c_groundCastMask == 0)
		{
			ClientTeleportalMapAvatarReceiver.c_groundCastMask = LayerMask.GetMask(new string[]
			{
				"Ground",
				"SlopedGround"
			});
		}
		this.m_receiver = base.gameObject.RequireComponent<TeleportalMapAvatarReceiver>();
	}

	// Token: 0x06003DC9 RID: 15817 RVA: 0x00126938 File Offset: 0x00124D38
	protected override IEnumerator TeleportRoutine(ClientTeleportal _entrancePortal, IClientTeleportalSender _sender, IClientTeleportable _object)
	{
		GameObject root = ((MonoBehaviour)_object).gameObject;
		MapAvatarControls controls = root.RequestComponentUpwardsRecursive<MapAvatarControls>();
		root = controls.gameObject;
		Transform attachPoint = this.m_receiver.GetAttachPoint(root);
		root.transform.SetParent(attachPoint);
		root.transform.rotation = Quaternion.identity;
		root.transform.localScale = Vector3.one;
		Collider collider = root.RequestComponent<Collider>();
		Vector3 offset = default(Vector3);
		RaycastHit raycastInfo;
		if (this.m_receiver.m_groundPlayer && Physics.Raycast(new Ray(attachPoint.position, -Vector3.up), out raycastInfo, 2f, ClientTeleportalMapAvatarReceiver.c_groundCastMask))
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
		ClientTeleportalMapAvatarSender clientMapAvatarSender = _sender as ClientTeleportalMapAvatarSender;
		if (clientMapAvatarSender != null)
		{
			TeleportalMapAvatarSender mapAvatarSender = clientMapAvatarSender.gameObject.RequestComponent<TeleportalMapAvatarSender>();
			if (mapAvatarSender != null)
			{
				if (mapAvatarSender.TransitionType == TeleportalMapAvatarSender.TransitionTypes.ScreenTransition)
				{
					ScreenTransitionManager screenTransitionManager = GameUtils.RequireManager<ScreenTransitionManager>();
					this.MoveCameraToTarget(root.transform);
					bool transitionDone = false;
					screenTransitionManager.StartTransitionDown(delegate
					{
						transitionDone = true;
					});
					while (!transitionDone)
					{
						yield return null;
					}
				}
				if (mapAvatarSender.m_useMotionBlur && mapAvatarSender.m_postProcessingBehaviour != null)
				{
					mapAvatarSender.m_postProcessingBehaviour.profile.motionBlur.enabled = false;
				}
			}
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
		MapAvatarDynamicLandscapeParenting dynamicParenting = root.RequestComponent<MapAvatarDynamicLandscapeParenting>();
		if (dynamicParenting != null)
		{
			dynamicParenting.enabled = true;
		}
		MapAvatarGroundCast groundCast = root.RequestComponent<MapAvatarGroundCast>();
		if (groundCast != null)
		{
			groundCast.enabled = true;
		}
		collider.enabled = true;
		this.m_receiver.OnAnimationFinished("Receive");
		ClientWorldObjectSynchroniser synchroniser = root.RequestComponent<ClientWorldObjectSynchroniser>();
		if (synchroniser != null)
		{
			while (!synchroniser.IsReadyToResume())
			{
				yield return null;
			}
			synchroniser.Resume();
		}
		Rigidbody rigidbody = root.RequireComponent<Rigidbody>();
		rigidbody.isKinematic = false;
		controls.enabled = true;
		ParticleSystem pfx = root.RequestComponentRecursive<ParticleSystem>();
		if (pfx != null)
		{
			pfx.Play();
		}
		yield break;
	}

	// Token: 0x06003DCA RID: 15818 RVA: 0x00126964 File Offset: 0x00124D64
	private void MoveCameraToTarget(Transform _target)
	{
		Camera main = Camera.main;
		WorldMapCamera worldMapCamera = main.gameObject.RequireComponent<WorldMapCamera>();
		Vector3 accessIdealOffset = worldMapCamera.AccessIdealOffset;
		worldMapCamera.transform.position = _target.position + accessIdealOffset;
	}

	// Token: 0x0400318E RID: 12686
	private TeleportalMapAvatarReceiver m_receiver;

	// Token: 0x0400318F RID: 12687
	private const float c_maxGroundCastDistance = 2f;

	// Token: 0x04003190 RID: 12688
	private static LayerMask c_groundCastMask = 0;

	// Token: 0x04003191 RID: 12689
	private Generic<bool> m_canTeleportCallback = () => true;

	// Token: 0x04003192 RID: 12690
	private ClientTeleportCallback m_teleportStartedCallback = delegate(IClientTeleportable _object)
	{
	};

	// Token: 0x04003193 RID: 12691
	private ClientTeleportCallback m_teleportFinishedCallback = delegate(IClientTeleportable _object)
	{
	};

	// Token: 0x04003194 RID: 12692
	private static readonly int m_iReceive = Animator.StringToHash("Receive");

	// Token: 0x04003195 RID: 12693
	private static readonly int m_iReset = Animator.StringToHash("Reset");

	// Token: 0x04003196 RID: 12694
	private static readonly int m_iIsFinished = Animator.StringToHash("IsFinished");
}
