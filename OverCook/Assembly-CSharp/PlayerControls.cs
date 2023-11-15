using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000A08 RID: 2568
[ExecutionDependency(typeof(PlayerInputLookup))]
[AddComponentMenu("Scripts/Game/Player/PlayerControls")]
[RequireComponent(typeof(PlayerIDProvider))]
[RequireComponent(typeof(PlayerAttachmentCarrier))]
[RequireComponent(typeof(AttachmentThrower))]
[RequireComponent(typeof(GroundCast))]
[RequireComponent(typeof(RigidbodyMotion))]
public class PlayerControls : MonoBehaviour
{
	// Token: 0x06003260 RID: 12896 RVA: 0x000EC180 File Offset: 0x000EA580
	private InteractWithItemHelper.ScanCondition<ClientInteractable> BuildCanInteractCondition(GameObject _object)
	{
		return (ClientInteractable _interact) => _interact.CanInteract(_object);
	}

	// Token: 0x06003261 RID: 12897 RVA: 0x000EC1A8 File Offset: 0x000EA5A8
	private InteractWithItemHelper.ScanCondition BuildPickupOrPlaceCondition(ICarrier _carrier)
	{
		return delegate(GameObject _object)
		{
			if (_carrier != null)
			{
				GameObject gameObject = _carrier.InspectCarriedItem();
				if (gameObject == null || !_object.IsInHierarchyOf(gameObject))
				{
					IClientHandlePickup controllingPickupHandler_Client = PlayerControlsHelper.GetControllingPickupHandler_Client(_object);
					IClientHandlePlacement controllingPlacementHandler_Client = PlayerControlsHelper.GetControllingPlacementHandler_Client(_object);
					bool flag = controllingPickupHandler_Client != null && controllingPickupHandler_Client.CanHandlePickup(_carrier);
					bool flag2 = gameObject != null && controllingPlacementHandler_Client != null;
					return flag || flag2;
				}
			}
			return false;
		};
	}

	// Token: 0x1700037B RID: 891
	// (get) Token: 0x06003262 RID: 12898 RVA: 0x000EC1D0 File Offset: 0x000EA5D0
	public PlayerControls.ControlSchemeData ControlScheme
	{
		get
		{
			return this.m_controlScheme;
		}
	}

	// Token: 0x1700037C RID: 892
	// (get) Token: 0x06003263 RID: 12899 RVA: 0x000EC1D8 File Offset: 0x000EA5D8
	public PlayerIDProvider PlayerIDProvider
	{
		get
		{
			return this.m_playerIDProvider;
		}
	}

	// Token: 0x1700037D RID: 893
	// (get) Token: 0x06003264 RID: 12900 RVA: 0x000EC1E0 File Offset: 0x000EA5E0
	public PlayerControls.MovementData Movement
	{
		get
		{
			return this.m_movement;
		}
	}

	// Token: 0x1700037E RID: 894
	// (get) Token: 0x06003265 RID: 12901 RVA: 0x000EC1E8 File Offset: 0x000EA5E8
	public float MovementScale
	{
		get
		{
			return this.m_movementScale;
		}
	}

	// Token: 0x1700037F RID: 895
	// (get) Token: 0x06003266 RID: 12902 RVA: 0x000EC1F0 File Offset: 0x000EA5F0
	public RigidbodyMotion Motion
	{
		get
		{
			return this.m_motion;
		}
	}

	// Token: 0x17000380 RID: 896
	// (get) Token: 0x06003267 RID: 12903 RVA: 0x000EC1F8 File Offset: 0x000EA5F8
	public PlayerControls.InteractionObjects CurrentInteractionObjects
	{
		get
		{
			return this.m_interactionObjects;
		}
	}

	// Token: 0x17000381 RID: 897
	// (get) Token: 0x06003268 RID: 12904 RVA: 0x000EC200 File Offset: 0x000EA600
	public LevelConfigBase LevelConfig
	{
		get
		{
			return this.m_levelConfigBase;
		}
	}

	// Token: 0x17000382 RID: 898
	// (get) Token: 0x06003269 RID: 12905 RVA: 0x000EC208 File Offset: 0x000EA608
	public LayerMask GroundLayer
	{
		get
		{
			return (!this.m_groundCast.HasGroundContact()) ? default(LayerMask) : this.m_groundCast.GetGroundLayer();
		}
	}

	// Token: 0x17000383 RID: 899
	// (get) Token: 0x0600326A RID: 12906 RVA: 0x000EC23E File Offset: 0x000EA63E
	public Collider GroundCollider
	{
		get
		{
			return (!this.m_groundCast.HasGroundContact()) ? null : this.m_groundCast.GetGroundCollider();
		}
	}

	// Token: 0x17000384 RID: 900
	// (get) Token: 0x0600326B RID: 12907 RVA: 0x000EC261 File Offset: 0x000EA661
	public Vector3 GroundNormal
	{
		get
		{
			return (!this.m_groundCast.HasGroundContact()) ? Vector3.up : this.m_groundCast.GetGroundNormal();
		}
	}

	// Token: 0x17000385 RID: 901
	// (get) Token: 0x0600326C RID: 12908 RVA: 0x000EC288 File Offset: 0x000EA688
	public float GroundDistance
	{
		get
		{
			return this.m_groundCast.GetGroundDistance();
		}
	}

	// Token: 0x17000386 RID: 902
	// (get) Token: 0x0600326D RID: 12909 RVA: 0x000EC295 File Offset: 0x000EA695
	public SurfaceMovable SurfaceMovable
	{
		get
		{
			return this.m_surfaceMovable;
		}
	}

	// Token: 0x17000387 RID: 903
	// (get) Token: 0x0600326E RID: 12910 RVA: 0x000EC29D File Offset: 0x000EA69D
	public WindAccumulator WindReceiver
	{
		get
		{
			return this.m_windReceiver;
		}
	}

	// Token: 0x17000388 RID: 904
	// (get) Token: 0x0600326F RID: 12911 RVA: 0x000EC2A5 File Offset: 0x000EA6A5
	public PlayerPhysicsSurface PhysicsSurface
	{
		get
		{
			return this.m_currentPhysicsSurface;
		}
	}

	// Token: 0x17000389 RID: 905
	// (get) Token: 0x06003270 RID: 12912 RVA: 0x000EC2AD File Offset: 0x000EA6AD
	public PlayerControls.ThrowIndicators ThrowIndicator
	{
		get
		{
			return this.m_throwIndicator;
		}
	}

	// Token: 0x1700038A RID: 906
	// (get) Token: 0x06003271 RID: 12913 RVA: 0x000EC2B5 File Offset: 0x000EA6B5
	public bool ServerControlled
	{
		get
		{
			return this.m_bServerControlled;
		}
	}

	// Token: 0x06003272 RID: 12914 RVA: 0x000EC2C0 File Offset: 0x000EA6C0
	public bool CanButtonBePressed()
	{
		return Application.isFocused && this.GetDirectlyUnderPlayerControl() && !T17DialogBoxManager.HasAnyOpenDialogs() && (!(T17InGameFlow.Instance != null) || !(T17InGameFlow.Instance.m_Rootmenu.GetCurrentOpenMenu() != null));
	}

	// Token: 0x06003273 RID: 12915 RVA: 0x000EC31D File Offset: 0x000EA71D
	public bool GetDirectlyUnderPlayerControl()
	{
		return this.m_directlyUnderPlayerControl && !this.m_directControlSuppression.IsSuppressed();
	}

	// Token: 0x06003274 RID: 12916 RVA: 0x000EC33B File Offset: 0x000EA73B
	public void SetDirectlyUnderPlayerControl(bool _underControl)
	{
		if (this.m_directlyUnderPlayerControl != _underControl)
		{
			this.m_directlyUnderPlayerControl = _underControl;
		}
	}

	// Token: 0x06003275 RID: 12917 RVA: 0x000EC350 File Offset: 0x000EA750
	public Suppressor Suppress(UnityEngine.Object _suppressor)
	{
		return this.m_directControlSuppression.AddSuppressor(_suppressor);
	}

	// Token: 0x06003276 RID: 12918 RVA: 0x000EC35E File Offset: 0x000EA75E
	public void ReleaseSuppressor(Suppressor _suppressor)
	{
		_suppressor.Release();
		this.m_directControlSuppression.UpdateSuppressors();
	}

	// Token: 0x06003277 RID: 12919 RVA: 0x000EC371 File Offset: 0x000EA771
	public bool IsSuppressed()
	{
		return this.m_directControlSuppression.IsSuppressed();
	}

	// Token: 0x06003278 RID: 12920 RVA: 0x000EC37E File Offset: 0x000EA77E
	public void SetMovementScale(float _scale)
	{
		this.m_movementScale = _scale;
	}

	// Token: 0x06003279 RID: 12921 RVA: 0x000EC387 File Offset: 0x000EA787
	public float GetUnclampedMovementSpeed()
	{
		return this.m_xzSpeed / this.m_movement.RunSpeed;
	}

	// Token: 0x0600327A RID: 12922 RVA: 0x000EC39B File Offset: 0x000EA79B
	public float GetMovementSpeed()
	{
		return Mathf.Clamp01(this.GetUnclampedMovementSpeed());
	}

	// Token: 0x0600327B RID: 12923 RVA: 0x000EC3A8 File Offset: 0x000EA7A8
	public ClientInteractable GetCurrentlyInteracting()
	{
		return this.m_impl_default.GetCurrentlyInteracting();
	}

	// Token: 0x0600327C RID: 12924 RVA: 0x000EC3B5 File Offset: 0x000EA7B5
	public bool IsDashing()
	{
		return this.m_impl_default.m_clientImpl.IsDashing();
	}

	// Token: 0x1700038B RID: 907
	// (get) Token: 0x0600327E RID: 12926 RVA: 0x000EC3D0 File Offset: 0x000EA7D0
	// (set) Token: 0x0600327D RID: 12925 RVA: 0x000EC3C7 File Offset: 0x000EA7C7
	public bool AllowSwitchingWhenDisabled
	{
		get
		{
			return this.m_allowSwitchWhenDisabled;
		}
		set
		{
			this.m_allowSwitchWhenDisabled = value;
		}
	}

	// Token: 0x0600327F RID: 12927 RVA: 0x000EC3D8 File Offset: 0x000EA7D8
	public void RegisterForInteractTrigger(VoidGeneric<ClientInteractable> _callback)
	{
		this.m_impl_default.RegisterForInteractTrigger(_callback);
	}

	// Token: 0x06003280 RID: 12928 RVA: 0x000EC3E6 File Offset: 0x000EA7E6
	public void UnregisterForInteractTrigger(VoidGeneric<ClientInteractable> _callback)
	{
		this.m_impl_default.UnregisterForInteractTrigger(_callback);
	}

	// Token: 0x06003281 RID: 12929 RVA: 0x000EC3F4 File Offset: 0x000EA7F4
	public void RegisterForThrowTrigger(VoidGeneric<GameObject> _callback)
	{
		this.m_impl_default.RegisterForThrowTrigger(_callback);
	}

	// Token: 0x06003282 RID: 12930 RVA: 0x000EC402 File Offset: 0x000EA802
	public void UnregisterForThrowTrigger(VoidGeneric<GameObject> _callback)
	{
		this.m_impl_default.UnregisterForThrowTrigger(_callback);
	}

	// Token: 0x06003283 RID: 12931 RVA: 0x000EC410 File Offset: 0x000EA810
	public void RegisterForFallingTrigger(VoidGeneric<bool> _callback)
	{
		this.m_impl_default.RegisterForFallingTrigger(_callback);
	}

	// Token: 0x06003284 RID: 12932 RVA: 0x000EC41E File Offset: 0x000EA81E
	public void UnregisterForFallingTrigger(VoidGeneric<bool> _callback)
	{
		this.m_impl_default.UnregisterForFallingTrigger(_callback);
	}

	// Token: 0x06003285 RID: 12933 RVA: 0x000EC42C File Offset: 0x000EA82C
	public void NotifySessionInteractionStarted(ClientSessionInteractable _interaction)
	{
		this.m_impl_default.NotifySessionInteractionStarted(_interaction);
	}

	// Token: 0x06003286 RID: 12934 RVA: 0x000EC43A File Offset: 0x000EA83A
	public void NotifySessionInteractionEnded(ClientSessionInteractable _interaction)
	{
		this.m_impl_default.NotifySessionInteractionEnded(_interaction);
	}

	// Token: 0x06003287 RID: 12935 RVA: 0x000EC448 File Offset: 0x000EA848
	public void OnCollisionEnter(Collision _collision)
	{
		this.m_impl_default.OnCollisionEnter(_collision);
	}

	// Token: 0x06003288 RID: 12936 RVA: 0x000EC458 File Offset: 0x000EA858
	private void Awake()
	{
		this.m_playerIDProvider = base.gameObject.GetComponent<PlayerIDProvider>();
		this.m_groundCast = base.gameObject.RequireComponent<GroundCast>();
		this.m_groundCast.RegisterGroundChangedCallback(new VoidGeneric<Collider>(this.OnGroundChanged));
		this.m_motion = base.gameObject.RequireComponent<RigidbodyMotion>();
		this.m_surfaceMovable = base.gameObject.GetComponent<SurfaceMovable>();
		this.m_windReceiver = base.gameObject.GetComponent<WindAccumulator>();
		this.m_impl_default = base.gameObject.AddComponent<PlayerControlsImpl_Default>();
		this.m_CanInteractCondition = this.BuildCanInteractCondition(base.gameObject);
		this.m_Transform = base.transform;
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06003289 RID: 12937 RVA: 0x000EC518 File Offset: 0x000EA918
	private void Init()
	{
		if (!this.m_bInit)
		{
			this.m_impl_default.Init(this);
			this.m_bInit = true;
		}
	}

	// Token: 0x0600328A RID: 12938 RVA: 0x000EC538 File Offset: 0x000EA938
	private void Start()
	{
		this.SetActiveControlsImpl(this.m_impl_default);
		this.m_previousPosition = this.m_Transform.localPosition;
		this.m_previousParent = this.m_Transform.parent;
	}

	// Token: 0x0600328B RID: 12939 RVA: 0x000EC568 File Offset: 0x000EA968
	private void OnEnable()
	{
		if (this.m_activeControlsImpl != null)
		{
			this.m_activeControlsImpl.Enable();
		}
		this.m_previousPosition = this.m_Transform.localPosition;
		this.m_previousParent = this.m_Transform.parent;
	}

	// Token: 0x0600328C RID: 12940 RVA: 0x000EC5A2 File Offset: 0x000EA9A2
	private void OnDisable()
	{
		if (this.m_activeControlsImpl != null)
		{
			this.m_activeControlsImpl.Disable();
		}
		this.SetInteractionObjects(new PlayerControls.InteractionObjects());
		this.m_xzSpeed = 0f;
	}

	// Token: 0x0600328D RID: 12941 RVA: 0x000EC5D0 File Offset: 0x000EA9D0
	private void OnDestroy()
	{
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		if (this.m_activeControlsImpl != null)
		{
			this.m_activeControlsImpl.Disable();
			this.m_activeControlsImpl = null;
		}
	}

	// Token: 0x0600328E RID: 12942 RVA: 0x000EC608 File Offset: 0x000EAA08
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.StartEntities)
		{
			this.m_clientCarrier = base.gameObject.RequestComponent<ClientPlayerAttachmentCarrier>();
			this.m_serverCarrier = base.gameObject.RequestComponent<ServerPlayerAttachmentCarrier>();
			if (this.m_clientCarrier != null)
			{
				this.m_PickupOrPlaceCondition = this.BuildPickupOrPlaceCondition(this.m_clientCarrier);
			}
			this.m_levelConfigBase = ((!(this.m_debugLevelConfig != null)) ? GameUtils.GetLevelConfig() : this.m_debugLevelConfig);
			this.m_bGridSelection = this.m_levelConfigBase.m_gridSelection;
			this.Init();
		}
	}

	// Token: 0x0600328F RID: 12943 RVA: 0x000EC6AC File Offset: 0x000EAAAC
	private void Update()
	{
		if (!MultiplayerController.IsSynchronisationActive())
		{
			return;
		}
		if (!TimeManager.IsPaused(base.gameObject) && this.m_directControlSuppression.IsSuppressed())
		{
			this.m_directControlSuppression.UpdateSuppressors();
			if (this.m_controlScheme != null && !this.m_directControlSuppression.IsSuppressed())
			{
				this.m_controlScheme.ClearEvents();
			}
		}
		this.m_activeControlsImpl.Update_Impl();
	}

	// Token: 0x06003290 RID: 12944 RVA: 0x000EC720 File Offset: 0x000EAB20
	public void FixedUpdate()
	{
		float fixedDeltaTime = TimeManager.GetFixedDeltaTime(base.gameObject);
		if (fixedDeltaTime > 0f)
		{
			if (this.m_previousParent != this.m_Transform.parent)
			{
				Matrix4x4 rhs = Matrix4x4.identity;
				Matrix4x4 lhs = Matrix4x4.identity;
				if (this.m_Transform.parent != null)
				{
					lhs = this.m_Transform.parent.worldToLocalMatrix;
				}
				if (this.m_previousParent != null)
				{
					rhs = this.m_previousParent.worldToLocalMatrix.inverse;
				}
				this.m_previousPosition = lhs * rhs * new Vector4(this.m_previousPosition.x, this.m_previousPosition.y, this.m_previousPosition.z, 1f);
				this.m_previousParent = this.m_Transform.parent;
			}
			Vector3 localPosition = this.m_Transform.localPosition;
			this.m_localVelocity = localPosition - this.m_previousPosition;
			this.m_localVelocity /= fixedDeltaTime;
			this.m_previousPosition = localPosition;
			this.m_xzSpeed = this.m_localVelocity.XZ().magnitude;
		}
	}

	// Token: 0x06003291 RID: 12945 RVA: 0x000EC860 File Offset: 0x000EAC60
	public void UpdateNearbyObjects()
	{
		PlayerControls.InteractionObjects interactionObjects = this.FindNearbyObjects();
		this.SetInteractionObjects(interactionObjects);
	}

	// Token: 0x06003292 RID: 12946 RVA: 0x000EC87C File Offset: 0x000EAC7C
	private void SetInteractionObjects(PlayerControls.InteractionObjects _newInteractionObjects)
	{
		this.UpdateInteractAnticipation(InteractionType.Interact, this.m_interactionObjects.m_interactable, _newInteractionObjects.m_interactable);
		this.UpdateInteractAnticipation(InteractionType.Pickup, this.m_interactionObjects.m_iHandlePickup, _newInteractionObjects.m_iHandlePickup);
		this.UpdateInteractAnticipation(InteractionType.Placement, this.m_interactionObjects.m_iHandlePlacement, _newInteractionObjects.m_iHandlePlacement);
		this.UpdateInteractAnticipation(InteractionType.Catch, this.m_interactionObjects.m_iHandleCatch, _newInteractionObjects.m_iHandleCatch);
		this.UpdateInteractAnticipation(InteractionType.GridOccupant, this.m_interactionObjects.m_gridLocation, _newInteractionObjects.m_gridLocation);
		this.m_interactionObjects.Copy(_newInteractionObjects);
	}

	// Token: 0x06003293 RID: 12947 RVA: 0x000EC910 File Offset: 0x000EAD10
	private IAnticipateInteractionNotifications[] GetAncipatorsFromComponent(object _participantCmpt)
	{
		if (_participantCmpt != null && _participantCmpt as MonoBehaviour != null)
		{
			GameObject gameObject = (_participantCmpt as MonoBehaviour).gameObject;
			return gameObject.RequestInterfaces<IAnticipateInteractionNotifications>();
		}
		return null;
	}

	// Token: 0x06003294 RID: 12948 RVA: 0x000EC948 File Offset: 0x000EAD48
	private void UpdateInteractAnticipation(InteractionType _type, object _participantCmptBefore, object _participantCmptAfter)
	{
		if (_participantCmptBefore != _participantCmptAfter)
		{
			IAnticipateInteractionNotifications[] ancipatorsFromComponent = this.GetAncipatorsFromComponent(_participantCmptBefore);
			IAnticipateInteractionNotifications[] ancipatorsFromComponent2 = this.GetAncipatorsFromComponent(_participantCmptAfter);
			if (ancipatorsFromComponent != null)
			{
				foreach (IAnticipateInteractionNotifications anticipateInteractionNotifications in ancipatorsFromComponent)
				{
					anticipateInteractionNotifications.OnInteractionAnticipationEnded(_type, base.gameObject);
				}
			}
			if (ancipatorsFromComponent2 != null)
			{
				foreach (IAnticipateInteractionNotifications anticipateInteractionNotifications2 in ancipatorsFromComponent2)
				{
					anticipateInteractionNotifications2.OnInteractionAnticipationStart(_type, base.gameObject);
				}
			}
		}
	}

	// Token: 0x06003295 RID: 12949 RVA: 0x000EC9CC File Offset: 0x000EADCC
	private PlayerControls.InteractionObjects FindNearbyObjects()
	{
		this.m_tmpInteractionObjects.Reset();
		Vector3 collidersInArc = InteractWithItemHelper.GetCollidersInArc(1f, 3.1415927f, this.m_Transform, this.m_colliders, this.m_interactMask, this.m_bGridSelection);
		GameObject gameObject = (!(this.m_clientCarrier != null)) ? null : this.m_clientCarrier.InspectCarriedItem();
		if (gameObject)
		{
			ClientUsableItem component = gameObject.GetComponent<ClientUsableItem>();
			if (component != null && component.CanInteract(base.gameObject))
			{
				this.m_tmpInteractionObjects.m_interactable = component;
			}
			else
			{
				this.m_tmpInteractionObjects.m_interactable = null;
			}
		}
		else
		{
			this.m_tmpInteractionObjects.m_interactable = InteractWithItemHelper.ScanForComponent<ClientInteractable>(this.m_colliders, collidersInArc, this.m_CanInteractCondition);
		}
		GameObject gameObject2 = InteractWithItemHelper.ScanForObject(this.m_colliders, collidersInArc, this.m_PickupOrPlaceCondition);
		if (gameObject2 != null)
		{
			this.m_tmpInteractionObjects.m_TheOriginalHandlePickup = gameObject2;
			this.m_tmpInteractionObjects.m_iHandlePickup = PlayerControlsHelper.GetControllingPickupHandler_Client(gameObject2);
			this.m_tmpInteractionObjects.m_iHandlePlacement = PlayerControlsHelper.GetControllingPlacementHandler_Client(gameObject2);
		}
		else
		{
			this.m_tmpInteractionObjects.m_TheOriginalHandlePickup = null;
			this.m_tmpInteractionObjects.m_iHandlePickup = null;
			this.m_tmpInteractionObjects.m_iHandlePlacement = null;
		}
		this.m_tmpInteractionObjects.m_gridLocation = InteractWithItemHelper.ScanForComponent<StaticGridLocation>(this.m_colliders, collidersInArc, this.m_GridLocationCondition);
		return this.m_tmpInteractionObjects;
	}

	// Token: 0x06003296 RID: 12950 RVA: 0x000ECB3C File Offset: 0x000EAF3C
	public ICatchable ScanForCatch()
	{
		Vector3 collidersInArc = InteractWithItemHelper.GetCollidersInArc(2f, 1.5707964f, this.m_Transform, this.m_colliders, this.m_catchMask, false);
		return InteractWithItemHelper.ScanForComponent<ServerCatchableItem>(this.m_colliders, collidersInArc, this.m_CatchCondition);
	}

	// Token: 0x06003297 RID: 12951 RVA: 0x000ECB85 File Offset: 0x000EAF85
	public IPlayerControlsImpl GetActiveControlsImpl()
	{
		return this.m_activeControlsImpl;
	}

	// Token: 0x06003298 RID: 12952 RVA: 0x000ECB8D File Offset: 0x000EAF8D
	private void SetActiveControlsImpl(IPlayerControlsImpl _iImpl)
	{
		if (this.m_activeControlsImpl != null)
		{
			this.m_activeControlsImpl.Disable();
		}
		_iImpl.Enable();
		this.m_activeControlsImpl = _iImpl;
	}

	// Token: 0x06003299 RID: 12953 RVA: 0x000ECBB2 File Offset: 0x000EAFB2
	private void OnGroundChanged(Collider groundCollider)
	{
		this.m_currentPhysicsSurface = ((!(groundCollider != null)) ? null : groundCollider.gameObject.RequestComponent<PlayerPhysicsSurface>());
	}

	// Token: 0x0600329A RID: 12954 RVA: 0x000ECBD8 File Offset: 0x000EAFD8
	public void SetControlSchemeData(PlayerControls.ControlSchemeData _controlScheme)
	{
		this.m_controlScheme = _controlScheme;
		this.m_impl_default.SetPlayerControlSchemeData(_controlScheme);
		if (_controlScheme.Player < (PlayerInputLookup.Player)ClientUserSystem.m_Users.Count)
		{
			User user = null;
			int num = 0;
			for (int i = 0; i < ClientUserSystem.m_Users.Count; i++)
			{
				if (ClientUserSystem.m_Users._items[i].IsLocal)
				{
					if (num == (int)_controlScheme.Player)
					{
						user = ClientUserSystem.m_Users._items[i];
						break;
					}
					num++;
				}
			}
			if (user != null)
			{
				int playerNum = ClientUserSystem.m_Users._items.FindIndex_Predicate((User x) => x == user);
				this.m_throwIndicator.UpdateCosmetics(user.Team, playerNum);
			}
		}
	}

	// Token: 0x0600329B RID: 12955 RVA: 0x000ECCB2 File Offset: 0x000EB0B2
	public void SetServerControlled(bool value)
	{
		this.m_bServerControlled = value;
	}

	// Token: 0x04002875 RID: 10357
	[SerializeField]
	private PlayerControls.MovementData m_movement;

	// Token: 0x04002876 RID: 10358
	[SerializeField]
	private PlayerControls.ThrowIndicators m_throwIndicator;

	// Token: 0x04002877 RID: 10359
	[SerializeField]
	private string m_chopTrigger;

	// Token: 0x04002878 RID: 10360
	[SerializeField]
	public float m_timeBeforeFalling = 0.2f;

	// Token: 0x04002879 RID: 10361
	[SerializeField]
	public float m_pickupDelay = 0.5f;

	// Token: 0x0400287A RID: 10362
	[SerializeField]
	private LayerMask m_interactMask;

	// Token: 0x0400287B RID: 10363
	[SerializeField]
	private LayerMask m_catchMask;

	// Token: 0x0400287C RID: 10364
	[SerializeField]
	private LevelConfigBase m_debugLevelConfig;

	// Token: 0x0400287D RID: 10365
	[SerializeField]
	private bool m_directlyUnderPlayerControl = true;

	// Token: 0x0400287E RID: 10366
	[SerializeField]
	public float m_analogEnableDeadzoneThreshold = 0.25f;

	// Token: 0x0400287F RID: 10367
	[SerializeField]
	public float m_analogEnableAngleThreshold = 15f;

	// Token: 0x04002880 RID: 10368
	private SuppressionController m_directControlSuppression = new SuppressionController();

	// Token: 0x04002881 RID: 10369
	public GameObject ImpactPFXPrefab;

	// Token: 0x04002882 RID: 10370
	public GameObject DashPFXPrefab;

	// Token: 0x04002883 RID: 10371
	public ParticleSystem CursePFXPrefab;

	// Token: 0x04002884 RID: 10372
	public GameObject CatchPFXPrefab;

	// Token: 0x04002885 RID: 10373
	[HideInInspector]
	public bool m_bRespawning;

	// Token: 0x04002886 RID: 10374
	[HideInInspector]
	public bool m_bApplyGravity = true;

	// Token: 0x04002887 RID: 10375
	private PlayerControls.ControlSchemeData m_controlScheme;

	// Token: 0x04002888 RID: 10376
	private PlayerIDProvider m_playerIDProvider;

	// Token: 0x04002889 RID: 10377
	private GroundCast m_groundCast;

	// Token: 0x0400288A RID: 10378
	private RigidbodyMotion m_motion;

	// Token: 0x0400288B RID: 10379
	private LevelConfigBase m_levelConfigBase;

	// Token: 0x0400288C RID: 10380
	private SurfaceMovable m_surfaceMovable;

	// Token: 0x0400288D RID: 10381
	private WindAccumulator m_windReceiver;

	// Token: 0x0400288E RID: 10382
	private PlayerPhysicsSurface m_currentPhysicsSurface;

	// Token: 0x0400288F RID: 10383
	private ServerPlayerAttachmentCarrier m_serverCarrier;

	// Token: 0x04002890 RID: 10384
	private ClientPlayerAttachmentCarrier m_clientCarrier;

	// Token: 0x04002891 RID: 10385
	private PlayerControlsImpl_Default m_impl_default;

	// Token: 0x04002892 RID: 10386
	private IPlayerControlsImpl m_activeControlsImpl;

	// Token: 0x04002893 RID: 10387
	private float m_movementScale = 1f;

	// Token: 0x04002894 RID: 10388
	private bool m_bServerControlled;

	// Token: 0x04002895 RID: 10389
	private bool m_bInit;

	// Token: 0x04002896 RID: 10390
	private const float c_InteractRadius = 1f;

	// Token: 0x04002897 RID: 10391
	private const float c_InteractArc = 3.1415927f;

	// Token: 0x04002898 RID: 10392
	private const float c_CatchRadius = 2f;

	// Token: 0x04002899 RID: 10393
	private const float c_CatchArc = 1.5707964f;

	// Token: 0x0400289A RID: 10394
	private const int c_NearbyObjectHitMax = 50;

	// Token: 0x0400289B RID: 10395
	private Collider[] m_colliders = new Collider[50];

	// Token: 0x0400289C RID: 10396
	private bool m_bGridSelection;

	// Token: 0x0400289D RID: 10397
	private PlayerControls.InteractionObjects m_interactionObjects = new PlayerControls.InteractionObjects();

	// Token: 0x0400289E RID: 10398
	private PlayerControls.InteractionObjects m_tmpInteractionObjects = new PlayerControls.InteractionObjects();

	// Token: 0x0400289F RID: 10399
	private Transform m_Transform;

	// Token: 0x040028A0 RID: 10400
	private Transform m_previousParent;

	// Token: 0x040028A1 RID: 10401
	private Vector3 m_previousPosition = default(Vector3);

	// Token: 0x040028A2 RID: 10402
	private Vector3 m_localVelocity = default(Vector3);

	// Token: 0x040028A3 RID: 10403
	private float m_xzSpeed;

	// Token: 0x040028A4 RID: 10404
	private bool m_allowSwitchWhenDisabled;

	// Token: 0x040028A5 RID: 10405
	private InteractWithItemHelper.ScanCondition<ClientInteractable> m_CanInteractCondition;

	// Token: 0x040028A6 RID: 10406
	private InteractWithItemHelper.ScanCondition m_PickupOrPlaceCondition;

	// Token: 0x040028A7 RID: 10407
	private InteractWithItemHelper.ScanCondition<ServerCatchableItem> m_CatchCondition = delegate(ServerCatchableItem _catchable)
	{
		IThrowable component = ComponentCache<IThrowable>.GetComponent(_catchable.gameObject);
		IAttachment component2 = ComponentCache<IAttachment>.GetComponent(_catchable.gameObject);
		bool flag = component != null && component.IsFlying();
		bool flag2 = component2 != null && component2.IsAttached();
		return _catchable.enabled && !flag2 && flag;
	};

	// Token: 0x040028A8 RID: 10408
	private InteractWithItemHelper.ScanCondition<StaticGridLocation> m_GridLocationCondition = (StaticGridLocation _gridLocation) => _gridLocation.IsGridOccupant();

	// Token: 0x02000A09 RID: 2569
	public enum InversionType
	{
		// Token: 0x040028AC RID: 10412
		Normal,
		// Token: 0x040028AD RID: 10413
		Inverted
	}

	// Token: 0x02000A0A RID: 2570
	[Serializable]
	public class MovementData
	{
		// Token: 0x040028AE RID: 10414
		public float GravityStrength = 10f;

		// Token: 0x040028AF RID: 10415
		public float GravityStrengthSlopedGround = 5f;

		// Token: 0x040028B0 RID: 10416
		public float RunSpeed = 4f;

		// Token: 0x040028B1 RID: 10417
		public float TurnSpeed = 20f;

		// Token: 0x040028B2 RID: 10418
		public float DashSpeed = 8f;

		// Token: 0x040028B3 RID: 10419
		public float DashTime = 1f;

		// Token: 0x040028B4 RID: 10420
		public float DashCooldown = 1f;

		// Token: 0x040028B5 RID: 10421
		public float FootstepLength = 0.2f;

		// Token: 0x040028B6 RID: 10422
		public PlayerControls.ImpactData DashImpactData;

		// Token: 0x040028B7 RID: 10423
		public PlayerControls.ImpactData ThrownImpactData;

		// Token: 0x040028B8 RID: 10424
		public PlayerControls.ImpactData HazardImpactData;

		// Token: 0x040028B9 RID: 10425
		public float MaxSpeed = 12f;

		// Token: 0x040028BA RID: 10426
		public float AutoSwitchTime = 0.5f;

		// Token: 0x040028BB RID: 10427
		public float StepHeightMax = 0.65f;

		// Token: 0x040028BC RID: 10428
		public PlayerControls.InversionType XAxisAllignment;

		// Token: 0x040028BD RID: 10429
		public PlayerControls.InversionType YAxisAllignment;
	}

	// Token: 0x02000A0B RID: 2571
	[Serializable]
	public class ImpactData
	{
		// Token: 0x040028BE RID: 10430
		public float Multiplier = 2f;

		// Token: 0x040028BF RID: 10431
		public float Time = 0.2f;
	}

	// Token: 0x02000A0C RID: 2572
	public class ControlSchemeData
	{
		// Token: 0x060032A0 RID: 12960 RVA: 0x000ECDD0 File Offset: 0x000EB1D0
		public ControlSchemeData(PlayerInputLookup.Player _playerID, PlayerControls _controls)
		{
			this.m_controls = _controls;
			this.Player = _playerID;
			this.m_pickupButton = this.GetGated(PlayerInputLookup.GetButton(PlayerInputLookup.LogicalButtonID.PickupAndDrop, _playerID));
			this.m_worksurfaceUseButton = this.GetGated(PlayerInputLookup.GetButton(PlayerInputLookup.LogicalButtonID.WorkstationInteract, _playerID));
			this.m_dashButton = this.GetGated(PlayerInputLookup.GetButton(PlayerInputLookup.LogicalButtonID.Dash, _playerID));
			this.m_curseButton = this.GetGated(PlayerInputLookup.GetButton(PlayerInputLookup.LogicalButtonID.Curse, _playerID));
			this.m_moveX = this.GetGated(PlayerInputLookup.GetValue(PlayerInputLookup.LogicalValueID.MovementX, _playerID));
			this.m_moveY = this.GetGated(PlayerInputLookup.GetValue(PlayerInputLookup.LogicalValueID.MovementY, _playerID));
		}

		// Token: 0x060032A1 RID: 12961 RVA: 0x000ECE64 File Offset: 0x000EB264
		public bool IsUseDown()
		{
			return this.m_worksurfaceUseButton.IsDown() && !this.m_supressUse;
		}

		// Token: 0x060032A2 RID: 12962 RVA: 0x000ECE82 File Offset: 0x000EB282
		public bool IsUseSuppressed()
		{
			return this.m_supressUse;
		}

		// Token: 0x060032A3 RID: 12963 RVA: 0x000ECE8A File Offset: 0x000EB28A
		public bool IsUseJustPressed()
		{
			return this.m_worksurfaceUseButton.JustPressed();
		}

		// Token: 0x060032A4 RID: 12964 RVA: 0x000ECE98 File Offset: 0x000EB298
		public bool IsUseJustReleased()
		{
			bool flag = this.m_worksurfaceUseButton.JustReleased();
			if (flag)
			{
				this.m_supressUse = false;
			}
			return flag;
		}

		// Token: 0x060032A5 RID: 12965 RVA: 0x000ECEC0 File Offset: 0x000EB2C0
		public void ClearEvents()
		{
			this.m_pickupButton.ClaimPressEvent();
			this.m_pickupButton.ClaimReleaseEvent();
			this.m_worksurfaceUseButton.ClaimPressEvent();
			this.m_worksurfaceUseButton.ClaimReleaseEvent();
			if (this.m_worksurfaceUseButton.IsDown())
			{
				this.m_supressUse = true;
			}
			this.m_dashButton.ClaimPressEvent();
			this.m_dashButton.ClaimReleaseEvent();
			this.m_curseButton.ClaimPressEvent();
			this.m_curseButton.ClaimReleaseEvent();
		}

		// Token: 0x060032A6 RID: 12966 RVA: 0x000ECF3C File Offset: 0x000EB33C
		private ILogicalButton GetGated(ILogicalButton _toProtect)
		{
			return new GateLogicalButton(_toProtect, new Generic<bool>(this.m_controls.CanButtonBePressed));
		}

		// Token: 0x060032A7 RID: 12967 RVA: 0x000ECF55 File Offset: 0x000EB355
		private ILogicalValue GetGated(ILogicalValue _toProtect)
		{
			return new GateLogicalValue(_toProtect, new Generic<bool>(this.m_controls.CanButtonBePressed));
		}

		// Token: 0x040028C0 RID: 10432
		public ILogicalButton m_pickupButton;

		// Token: 0x040028C1 RID: 10433
		public ILogicalButton m_worksurfaceUseButton;

		// Token: 0x040028C2 RID: 10434
		public ILogicalButton m_dashButton;

		// Token: 0x040028C3 RID: 10435
		public ILogicalButton m_curseButton;

		// Token: 0x040028C4 RID: 10436
		public ILogicalValue m_moveX;

		// Token: 0x040028C5 RID: 10437
		public ILogicalValue m_moveY;

		// Token: 0x040028C6 RID: 10438
		private PlayerControls m_controls;

		// Token: 0x040028C7 RID: 10439
		public PlayerInputLookup.Player Player;

		// Token: 0x040028C8 RID: 10440
		private bool m_supressUse;
	}

	// Token: 0x02000A0D RID: 2573
	public class InteractionObjects
	{
		// Token: 0x060032A9 RID: 12969 RVA: 0x000ECF76 File Offset: 0x000EB376
		public void Reset()
		{
			this.m_iHandlePickup = null;
			this.m_iHandleCatch = null;
			this.m_iHandlePlacement = null;
			this.m_interactable = null;
			this.m_gridLocation = null;
			this.m_TheOriginalHandlePickup = null;
		}

		// Token: 0x060032AA RID: 12970 RVA: 0x000ECFA4 File Offset: 0x000EB3A4
		public void Copy(PlayerControls.InteractionObjects other)
		{
			this.m_iHandlePickup = other.m_iHandlePickup;
			this.m_iHandleCatch = other.m_iHandleCatch;
			this.m_iHandlePlacement = other.m_iHandlePlacement;
			this.m_interactable = other.m_interactable;
			this.m_gridLocation = other.m_gridLocation;
			this.m_TheOriginalHandlePickup = other.m_TheOriginalHandlePickup;
		}

		// Token: 0x040028C9 RID: 10441
		public IClientHandlePickup m_iHandlePickup;

		// Token: 0x040028CA RID: 10442
		public ICatchable m_iHandleCatch;

		// Token: 0x040028CB RID: 10443
		public IClientHandlePlacement m_iHandlePlacement;

		// Token: 0x040028CC RID: 10444
		public ClientInteractable m_interactable;

		// Token: 0x040028CD RID: 10445
		public IGridLocation m_gridLocation;

		// Token: 0x040028CE RID: 10446
		public GameObject m_TheOriginalHandlePickup;
	}

	// Token: 0x02000A0E RID: 2574
	[Serializable]
	public class ThrowIndicators
	{
		// Token: 0x060032AC RID: 12972 RVA: 0x000ED004 File Offset: 0x000EB404
		public void Show(bool _isAiming)
		{
			if (this.m_isAiming == _isAiming)
			{
				return;
			}
			this.m_isAiming = new bool?(_isAiming);
			this.m_aimIndicator.gameObject.SetActive(_isAiming);
			this.m_aimIndicatorShadow.gameObject.SetActive(_isAiming);
			this.m_idleIndicator.gameObject.SetActive(!_isAiming);
			this.m_idleIndicatorShadow.gameObject.SetActive(!_isAiming);
		}

		// Token: 0x060032AD RID: 12973 RVA: 0x000ED088 File Offset: 0x000EB488
		public void UpdateCosmetics(TeamID _team, int _playerNum)
		{
			int num = _playerNum;
			if (_team != TeamID.None)
			{
				num = (int)(TeamID.Two - _team);
			}
			if (num > -1 && num < 4)
			{
				PlayerControls.ThrowIndicators.ThrowIndicatorsCosmetics throwIndicatorsCosmetics = this.m_throwingIndicatorCosmetics[num];
				this.m_aimIndicator.material.SetTexture("_MainTex", throwIndicatorsCosmetics.m_aimIndicator);
				this.m_aimIndicatorShadow.material.SetTexture("_MainTex", throwIndicatorsCosmetics.m_aimIndicatorShadow);
				this.m_idleIndicator.material.SetTexture("_MainTex", throwIndicatorsCosmetics.m_idleIndicator);
				this.m_idleIndicatorShadow.material.SetTexture("_MainTex", throwIndicatorsCosmetics.m_idleIndicatorShadow);
			}
		}

		// Token: 0x060032AE RID: 12974 RVA: 0x000ED134 File Offset: 0x000EB534
		public void Hide()
		{
			this.m_aimIndicator.gameObject.SetActive(false);
			this.m_aimIndicatorShadow.gameObject.SetActive(false);
			this.m_idleIndicator.gameObject.SetActive(false);
			this.m_idleIndicatorShadow.gameObject.SetActive(false);
			this.m_isAiming = null;
		}

		// Token: 0x040028CF RID: 10447
		[Header("Aiming Renderers")]
		[SerializeField]
		private MeshRenderer m_aimIndicator;

		// Token: 0x040028D0 RID: 10448
		[SerializeField]
		private MeshRenderer m_aimIndicatorShadow;

		// Token: 0x040028D1 RID: 10449
		[Header("Idle Renderers")]
		[SerializeField]
		private MeshRenderer m_idleIndicator;

		// Token: 0x040028D2 RID: 10450
		[SerializeField]
		private MeshRenderer m_idleIndicatorShadow;

		// Token: 0x040028D3 RID: 10451
		[Header("Cosmetics")]
		[SerializeField]
		private PlayerControls.ThrowIndicators.ThrowIndicatorsCosmetics[] m_throwingIndicatorCosmetics;

		// Token: 0x040028D4 RID: 10452
		private bool? m_isAiming;

		// Token: 0x02000A0F RID: 2575
		[Serializable]
		public struct ThrowIndicatorsCosmetics
		{
			// Token: 0x040028D5 RID: 10453
			[Header("Aiming Textures")]
			public Texture m_aimIndicator;

			// Token: 0x040028D6 RID: 10454
			public Texture m_aimIndicatorShadow;

			// Token: 0x040028D7 RID: 10455
			[Header("Idle Textures")]
			public Texture m_idleIndicator;

			// Token: 0x040028D8 RID: 10456
			public Texture m_idleIndicatorShadow;
		}
	}
}
