using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000BD1 RID: 3025
public class ClientTeleportalMapAvatarSender : ClientBaseTeleportalSender
{
	// Token: 0x06003DDA RID: 15834 RVA: 0x001270B7 File Offset: 0x001254B7
	protected override void Awake()
	{
		base.Awake();
		this.m_sender = base.gameObject.RequireComponent<TeleportalMapAvatarSender>();
	}

	// Token: 0x06003DDB RID: 15835 RVA: 0x001270D0 File Offset: 0x001254D0
	protected override IEnumerator TeleportRoutine(ClientTeleportal _entrancePortal, IClientTeleportalReceiver _receiver, IClientTeleportable _object)
	{
		GameObject root = ((MonoBehaviour)_object).gameObject;
		MapAvatarControls controls = root.RequireComponent<MapAvatarControls>();
		root = controls.gameObject;
		controls.enabled = false;
		Rigidbody rigidbody = root.RequireComponent<Rigidbody>();
		rigidbody.isKinematic = true;
		rigidbody.velocity = Vector3.zero;
		Collider collider = root.RequireComponent<Collider>();
		collider.enabled = false;
		MapAvatarGroundCast groundCast = root.RequestComponent<MapAvatarGroundCast>();
		if (groundCast != null)
		{
			groundCast.enabled = false;
		}
		MapAvatarDynamicLandscapeParenting dynamicParenting = root.RequestComponent<MapAvatarDynamicLandscapeParenting>();
		if (dynamicParenting != null)
		{
			dynamicParenting.enabled = false;
		}
		ClientWorldObjectSynchroniser synchroniser = root.RequestComponent<ClientWorldObjectSynchroniser>();
		if (synchroniser != null)
		{
			synchroniser.Pause();
		}
		Transform attachPoint = this.m_sender.GetAttachPoint(root);
		root.transform.SetParent(attachPoint, true);
		IEnumerator routine = this.m_sender.m_SenderAnimation.Run(root, attachPoint, default(Vector3));
		while (routine.MoveNext())
		{
			yield return null;
		}
		this.m_sender.OnAnimationFinished("Teleport");
		root.transform.SetParent(null, true);
		root.transform.rotation = Quaternion.identity;
		if (this.m_sender.m_useMotionBlur && this.m_sender.m_postProcessingBehaviour != null)
		{
			this.m_sender.m_postProcessingBehaviour.profile.motionBlur.enabled = true;
		}
		Animator mesh = root.RequestComponentRecursive<Animator>();
		if (mesh != null)
		{
			mesh.gameObject.SetActive(true);
		}
		ParticleSystem pfx = root.RequestComponentRecursive<ParticleSystem>();
		if (pfx != null)
		{
			pfx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		}
		IEnumerator transitionRoutine = null;
		TeleportalMapAvatarSender.TransitionTypes transitionType = this.m_sender.TransitionType;
		if (transitionType != TeleportalMapAvatarSender.TransitionTypes.ScreenTransition)
		{
			if (transitionType != TeleportalMapAvatarSender.TransitionTypes.Lerp)
			{
				if (transitionType == TeleportalMapAvatarSender.TransitionTypes.Instant)
				{
					MonoBehaviour monoBehaviour = _receiver as MonoBehaviour;
					if (monoBehaviour != null)
					{
						Camera main = Camera.main;
						WorldMapCamera worldMapCamera = main.gameObject.RequireComponent<WorldMapCamera>();
						Vector3 accessIdealOffset = worldMapCamera.AccessIdealOffset;
						main.transform.position = monoBehaviour.transform.position + accessIdealOffset;
					}
				}
			}
			else
			{
				MonoBehaviour monoBehaviour2 = _receiver as MonoBehaviour;
				if (monoBehaviour2 != null)
				{
					transitionRoutine = this.CameraLerpRoutine(monoBehaviour2.transform.position);
				}
			}
		}
		else
		{
			transitionRoutine = this.ScreenTransitionRoutine();
		}
		while (transitionRoutine != null && transitionRoutine.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003DDC RID: 15836 RVA: 0x001270FC File Offset: 0x001254FC
	private IEnumerator ScreenTransitionRoutine()
	{
		ScreenTransitionManager screenTransitionManager = GameUtils.RequireManager<ScreenTransitionManager>();
		bool transitionDone = false;
		screenTransitionManager.StartTransitionUp(delegate
		{
			transitionDone = true;
		});
		while (!transitionDone)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003DDD RID: 15837 RVA: 0x00127110 File Offset: 0x00125510
	private IEnumerator CameraLerpRoutine(Vector3 _endPos)
	{
		Camera mainCamera = Camera.main;
		WorldMapCamera worldMapCamera = mainCamera.gameObject.RequireComponent<WorldMapCamera>();
		worldMapCamera.enabled = false;
		Vector3 idealOffset = worldMapCamera.AccessIdealOffset;
		Vector3 idealLocation = _endPos + idealOffset;
		float distanceToIdeal = (idealLocation - mainCamera.transform.position).magnitude;
		float currentGradient = 0f;
		for (;;)
		{
			distanceToIdeal = (idealLocation - mainCamera.transform.position).magnitude;
			float deltaTime = TimeManager.GetDeltaTime(base.gameObject);
			MathUtils.AdvanceToTarget_Sinusoidal(ref distanceToIdeal, ref currentGradient, 0f, this.m_sender.m_lerpConfig.GradientLimit, this.m_sender.m_lerpConfig.TimeToMax, deltaTime);
			Vector3 offset = idealLocation - mainCamera.transform.position;
			Vector3 pos = idealLocation - offset.SafeNormalised(Vector3.zero) * distanceToIdeal;
			mainCamera.transform.position = pos;
			if (distanceToIdeal < 0.1f)
			{
				break;
			}
			yield return null;
		}
		worldMapCamera.enabled = true;
		yield break;
	}

	// Token: 0x040031A0 RID: 12704
	private TeleportalMapAvatarSender m_sender;

	// Token: 0x040031A1 RID: 12705
	private static readonly int m_iTeleport = Animator.StringToHash("Teleport");

	// Token: 0x040031A2 RID: 12706
	private static readonly int m_iIsFinished = Animator.StringToHash("IsFinished");

	// Token: 0x040031A3 RID: 12707
	private static readonly int m_iReset = Animator.StringToHash("Reset");
}
