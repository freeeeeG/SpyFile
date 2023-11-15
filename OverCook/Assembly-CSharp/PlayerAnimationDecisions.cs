using System;
using GameModes.Horde;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000A00 RID: 2560
[RequireComponent(typeof(PlayerControls))]
[RequireComponent(typeof(PlayerIDProvider))]
[RequireComponent(typeof(HeldItemsMeshVisibility))]
[ExecutionDependency(typeof(ChefMeshReplacer))]
[AddComponentMenu("Scripts/Game/Player/PlayerAnimationDecisions")]
public class PlayerAnimationDecisions : AnimationInspectionBase
{
	// Token: 0x0600320F RID: 12815 RVA: 0x000EAE8D File Offset: 0x000E928D
	private void Start()
	{
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06003210 RID: 12816 RVA: 0x000EAEA7 File Offset: 0x000E92A7
	private void OnDestroy()
	{
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06003211 RID: 12817 RVA: 0x000EAEC4 File Offset: 0x000E92C4
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.StartEntities)
		{
			this.m_transform = base.gameObject.transform;
			this.m_animator = base.gameObject.RequireComponentInImmediateChildren<Animator>();
			this.m_playerIDProvider = base.gameObject.GetComponent<PlayerIDProvider>();
			this.m_controls = base.gameObject.GetComponent<PlayerControls>();
			this.m_carrier = base.gameObject.GetComponent<ClientPlayerAttachmentCarrier>();
			this.m_heldItemsMeshVisibility = base.gameObject.GetComponent<ClientHeldItemsMeshVisibility>();
			this.m_heldItemsMeshVisibility.SetVisState(this.m_visState);
			this.m_tailMeshVisibility = base.gameObject.GetComponent<ClientTailMeshVisibility>();
			this.m_tailMeshVisibility.SetVisState(this.m_tailVisState);
			this.m_bodyMeshVisibility = base.gameObject.GetComponent<ClientBodyMeshVisibility>();
			this.m_bodyMeshVisibility.SetVisState(this.m_bodyVisState);
			this.m_controls.RegisterForInteractTrigger(new VoidGeneric<ClientInteractable>(this.OnInteract));
			this.m_controls.RegisterForThrowTrigger(new VoidGeneric<GameObject>(this.OnThrow));
			this.m_controls.RegisterForFallingTrigger(new VoidGeneric<bool>(this.OnFall));
			GameObject obj = base.gameObject.RequestChild("PFX_RunningPuff");
			ParticleSystem particleSystem = obj.RequestComponent<ParticleSystem>();
			this.m_psEmission = particleSystem.emission;
			this.AddDatastreams();
			this.m_bStarted = true;
		}
	}

	// Token: 0x06003212 RID: 12818 RVA: 0x000EB016 File Offset: 0x000E9416
	private void Update()
	{
		if (this.m_bStarted)
		{
			this.UpdateVariables();
			this.UpdateVisStates();
		}
	}

	// Token: 0x06003213 RID: 12819 RVA: 0x000EB030 File Offset: 0x000E9430
	private void OnInteract(ClientInteractable _interactable)
	{
		if (_interactable && _interactable.gameObject.GetComponent<BellowsSpray>() != null && this.m_animator != null && this.m_animator.isInitialized)
		{
			this.m_animator.SetTrigger(PlayerAnimationDecisions.m_bellowsUse);
		}
	}

	// Token: 0x06003214 RID: 12820 RVA: 0x000EB08F File Offset: 0x000E948F
	private void OnThrow(GameObject _throwable)
	{
		if (_throwable != null && this.m_animator != null && this.m_animator.isInitialized)
		{
			this.m_animator.SetTrigger(PlayerAnimationDecisions.m_iThrow);
		}
	}

	// Token: 0x06003215 RID: 12821 RVA: 0x000EB0CE File Offset: 0x000E94CE
	private void OnFall(bool _isFalling)
	{
		if (this.m_animator != null && this.m_animator.isInitialized)
		{
			this.m_animator.SetBool(PlayerAnimationDecisions.m_iFalling, _isFalling);
		}
	}

	// Token: 0x06003216 RID: 12822 RVA: 0x000EB102 File Offset: 0x000E9502
	public bool IsHoldingSomething()
	{
		return this.m_carrier != null && this.m_carrier.InspectCarriedItem() != null;
	}

	// Token: 0x06003217 RID: 12823 RVA: 0x000EB128 File Offset: 0x000E9528
	protected bool IsHolding<T>()
	{
		if (this.m_carrier != null)
		{
			GameObject gameObject = this.m_carrier.InspectCarriedItem();
			if (gameObject != null && gameObject.GetComponent<T>() != null)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003218 RID: 12824 RVA: 0x000EB171 File Offset: 0x000E9571
	public bool IsHoldingWaterGun()
	{
		return this.IsHolding<WaterGunSpray>();
	}

	// Token: 0x06003219 RID: 12825 RVA: 0x000EB179 File Offset: 0x000E9579
	public bool IsHoldingBellows()
	{
		return this.IsHolding<BellowsSpray>();
	}

	// Token: 0x0600321A RID: 12826 RVA: 0x000EB184 File Offset: 0x000E9584
	protected bool IsInteracting<T>()
	{
		if (this.m_controls != null)
		{
			ClientInteractable currentlyInteracting = this.m_controls.GetCurrentlyInteracting();
			if (currentlyInteracting != null && currentlyInteracting.GetComponent<T>() != null)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600321B RID: 12827 RVA: 0x000EB1CD File Offset: 0x000E95CD
	public bool IsChopping()
	{
		return this.IsInteracting<Workstation>();
	}

	// Token: 0x0600321C RID: 12828 RVA: 0x000EB1D5 File Offset: 0x000E95D5
	public bool IsWashing()
	{
		return this.IsInteracting<WashingStation>();
	}

	// Token: 0x0600321D RID: 12829 RVA: 0x000EB1DD File Offset: 0x000E95DD
	public bool IsPettingKevin()
	{
		return this.IsInteracting<StoryKevinCosmeticDecisions>();
	}

	// Token: 0x0600321E RID: 12830 RVA: 0x000EB1E5 File Offset: 0x000E95E5
	public bool IsPushingPushable()
	{
		return this.IsInteracting<PushableObject>();
	}

	// Token: 0x0600321F RID: 12831 RVA: 0x000EB1ED File Offset: 0x000E95ED
	public bool IsWearingBackpack()
	{
		return this.m_carrier != null && this.m_carrier.InspectCarriedItem(PlayerAttachTarget.Back) != null;
	}

	// Token: 0x06003220 RID: 12832 RVA: 0x000EB214 File Offset: 0x000E9614
	public bool IsRepairing()
	{
		return this.IsInteracting<HordeTarget>();
	}

	// Token: 0x06003221 RID: 12833 RVA: 0x000EB21C File Offset: 0x000E961C
	public void SetInCannon(bool _inCannon)
	{
		this.m_inCannon = _inCannon;
	}

	// Token: 0x06003222 RID: 12834 RVA: 0x000EB225 File Offset: 0x000E9625
	public bool IsInCannon()
	{
		return this.m_inCannon;
	}

	// Token: 0x06003223 RID: 12835 RVA: 0x000EB22D File Offset: 0x000E962D
	public void FireCannon()
	{
		this.m_animator.SetTrigger(PlayerAnimationDecisions.m_iCannonFired);
	}

	// Token: 0x06003224 RID: 12836 RVA: 0x000EB23F File Offset: 0x000E963F
	public void SetCannonSpeed(float speed)
	{
		this.m_animator.SetFloat(PlayerAnimationDecisions.m_iCannonDuration, speed);
	}

	// Token: 0x06003225 RID: 12837 RVA: 0x000EB254 File Offset: 0x000E9654
	private void UpdateVariables()
	{
		if (this.m_animator == null || !this.m_animator.isInitialized)
		{
			return;
		}
		this.m_animator.SetFloat(PlayerAnimationDecisions.m_Speed, this.m_controls.GetMovementSpeed());
		this.m_animator.SetBool(PlayerAnimationDecisions.m_Holding, this.IsHoldingSomething());
		this.m_chopping = this.IsChopping();
		this.m_washingUp = this.IsWashing();
		this.m_animator.SetBool(PlayerAnimationDecisions.m_Chopping, this.m_chopping);
		this.m_animator.SetBool(PlayerAnimationDecisions.m_iWashingUp, this.m_washingUp);
		this.m_animator.SetBool(PlayerAnimationDecisions.m_iPettingKevin, this.IsPettingKevin());
		this.m_animator.SetBool(PlayerAnimationDecisions.m_waterGun, this.IsHoldingWaterGun());
		this.m_animator.SetBool(PlayerAnimationDecisions.m_bellows, this.IsHoldingBellows());
		bool flag = this.IsPushingPushable() && this.m_controls.GetDirectlyUnderPlayerControl();
		Vector3 vector = Vector3.zero;
		if (flag)
		{
			ClientInteractable currentlyInteracting = this.m_controls.GetCurrentlyInteracting();
			if (currentlyInteracting != null && currentlyInteracting.gameObject != null)
			{
				ClientPushableObject clientPushableObject = currentlyInteracting.gameObject.RequireComponent<ClientPushableObject>();
				Vector2 cosmeticMovementDirection = clientPushableObject.CosmeticMovementDirection;
				vector = new Vector3(cosmeticMovementDirection.x, 0f, cosmeticMovementDirection.y);
			}
			if (vector.sqrMagnitude > 0.001f)
			{
				vector = this.m_transform.InverseTransformDirection(vector);
				vector.Normalize();
			}
			vector.y = vector.z;
			vector.z = 0f;
		}
		float deltaTime = TimeManager.GetDeltaTime(base.gameObject.layer);
		vector = Vector2.MoveTowards(this.m_prevPushMovement, vector, this.m_pushLerpSpeed * deltaTime);
		this.m_animator.SetFloat(PlayerAnimationDecisions.m_iPushX, vector.x);
		this.m_animator.SetFloat(PlayerAnimationDecisions.m_iPushY, vector.y);
		this.m_prevPushMovement = vector;
		this.m_animator.SetBool(PlayerAnimationDecisions.m_iPushing, flag);
		this.m_repairing = this.IsRepairing();
		this.m_animator.SetBool(PlayerAnimationDecisions.m_iRepairing, this.m_repairing);
		this.m_animator.SetBool(PlayerAnimationDecisions.m_iInCannon, this.m_inCannon);
		bool flag2 = this.m_controls.Motion.GetVelocity().sqrMagnitude > 0.001f;
		if (!flag2 && this.m_psEmission.enabled)
		{
			this.m_psEmission.enabled = false;
		}
		else if (flag2 && !this.m_psEmission.enabled && !this.m_controls.m_bRespawning)
		{
			this.m_psEmission.enabled = true;
		}
	}

	// Token: 0x06003226 RID: 12838 RVA: 0x000EB528 File Offset: 0x000E9928
	private void UpdateVisStates()
	{
		HeldItemsMeshVisibility.VisState visState = this.m_visState;
		if (this.m_chopping || (this.m_animator.isInitialized && this.m_animator.GetFloat(PlayerAnimationDecisions.m_iShowKnife) > 0f))
		{
			visState = HeldItemsMeshVisibility.VisState.Chopping;
		}
		else if (this.m_washingUp || (this.m_animator.isInitialized && this.m_animator.GetBool(PlayerAnimationDecisions.m_iWashingUp)))
		{
			visState = HeldItemsMeshVisibility.VisState.Washing;
		}
		else if (this.m_repairing || (this.m_animator.isInitialized && this.m_animator.GetBool(PlayerAnimationDecisions.m_iRepairing)))
		{
			visState = HeldItemsMeshVisibility.VisState.Repairing;
		}
		else if (this.IsHoldingSomething())
		{
			visState = HeldItemsMeshVisibility.VisState.Carrying;
		}
		else
		{
			visState = HeldItemsMeshVisibility.VisState.Idle;
		}
		if (visState != this.m_visState)
		{
			this.m_heldItemsMeshVisibility.SetVisState(visState);
			this.m_visState = visState;
		}
		TailMeshVisibility.VisState visState2 = this.m_tailVisState;
		if (this.IsWearingBackpack())
		{
			visState2 = TailMeshVisibility.VisState.Hidden;
		}
		else
		{
			visState2 = TailMeshVisibility.VisState.Visible;
		}
		if (visState2 != this.m_tailVisState)
		{
			this.m_tailMeshVisibility.SetVisState(visState2);
			this.m_tailVisState = visState2;
		}
		BodyMeshVisibility.VisState visState3 = this.m_bodyVisState;
		if (this.IsInCannon())
		{
			visState3 = BodyMeshVisibility.VisState.Hidden;
		}
		else
		{
			visState3 = BodyMeshVisibility.VisState.Visible;
		}
		if (visState3 != this.m_bodyVisState)
		{
			this.m_bodyMeshVisibility.SetVisState(visState3);
			this.m_bodyVisState = visState3;
		}
	}

	// Token: 0x06003227 RID: 12839 RVA: 0x000EB68D File Offset: 0x000E9A8D
	private void AddDatastreams()
	{
	}

	// Token: 0x04002842 RID: 10306
	private PlayerIDProvider m_playerIDProvider;

	// Token: 0x04002843 RID: 10307
	private PlayerControls m_controls;

	// Token: 0x04002844 RID: 10308
	private ClientPlayerAttachmentCarrier m_carrier;

	// Token: 0x04002845 RID: 10309
	private ClientHeldItemsMeshVisibility m_heldItemsMeshVisibility;

	// Token: 0x04002846 RID: 10310
	private Transform m_transform;

	// Token: 0x04002847 RID: 10311
	private HeldItemsMeshVisibility.VisState m_visState = HeldItemsMeshVisibility.VisState.Idle;

	// Token: 0x04002848 RID: 10312
	private bool m_chopping;

	// Token: 0x04002849 RID: 10313
	private bool m_washingUp;

	// Token: 0x0400284A RID: 10314
	private bool m_bStarted;

	// Token: 0x0400284B RID: 10315
	private ParticleSystem.EmissionModule m_psEmission;

	// Token: 0x0400284C RID: 10316
	private ClientTailMeshVisibility m_tailMeshVisibility;

	// Token: 0x0400284D RID: 10317
	private TailMeshVisibility.VisState m_tailVisState = TailMeshVisibility.VisState.Visible;

	// Token: 0x0400284E RID: 10318
	private ClientBodyMeshVisibility m_bodyMeshVisibility;

	// Token: 0x0400284F RID: 10319
	private BodyMeshVisibility.VisState m_bodyVisState = BodyMeshVisibility.VisState.Visible;

	// Token: 0x04002850 RID: 10320
	private static int m_iShowKnife = Animator.StringToHash("ShowKnife");

	// Token: 0x04002851 RID: 10321
	private static int m_iWashingUp = Animator.StringToHash("WashingUp");

	// Token: 0x04002852 RID: 10322
	private static int m_iThrow = Animator.StringToHash("Throw");

	// Token: 0x04002853 RID: 10323
	private static int m_iChop = Animator.StringToHash("Chop");

	// Token: 0x04002854 RID: 10324
	private static int m_iFalling = Animator.StringToHash("Falling");

	// Token: 0x04002855 RID: 10325
	private static int m_Speed = Animator.StringToHash("Speed");

	// Token: 0x04002856 RID: 10326
	private static int m_Holding = Animator.StringToHash("Holding");

	// Token: 0x04002857 RID: 10327
	private static int m_Chopping = Animator.StringToHash("Chopping");

	// Token: 0x04002858 RID: 10328
	private static int m_iPettingKevin = Animator.StringToHash("Pet");

	// Token: 0x04002859 RID: 10329
	private static int m_bellows = Animator.StringToHash("Bellows");

	// Token: 0x0400285A RID: 10330
	private static int m_bellowsUse = Animator.StringToHash("BellowsUse");

	// Token: 0x0400285B RID: 10331
	private static int m_waterGun = Animator.StringToHash("WaterGun");

	// Token: 0x0400285C RID: 10332
	[SerializeField]
	public float m_pushLerpSpeed = 15f;

	// Token: 0x0400285D RID: 10333
	private static int m_iPushing = Animator.StringToHash("IsPushPull");

	// Token: 0x0400285E RID: 10334
	private static int m_iPushX = Animator.StringToHash("PushPull_X");

	// Token: 0x0400285F RID: 10335
	private static int m_iPushY = Animator.StringToHash("PushPull_Y");

	// Token: 0x04002860 RID: 10336
	private Vector2 m_prevPushMovement = new Vector2(0f, 0f);

	// Token: 0x04002861 RID: 10337
	private bool m_repairing;

	// Token: 0x04002862 RID: 10338
	private static int m_iRepairing = Animator.StringToHash("Hammering");

	// Token: 0x04002863 RID: 10339
	private bool m_inCannon;

	// Token: 0x04002864 RID: 10340
	private static int m_iInCannon = Animator.StringToHash("isInCannon");

	// Token: 0x04002865 RID: 10341
	private static int m_iCannonFired = Animator.StringToHash("CannonFired");

	// Token: 0x04002866 RID: 10342
	private static int m_iCannonDuration = Animator.StringToHash("CannonDuration");
}
