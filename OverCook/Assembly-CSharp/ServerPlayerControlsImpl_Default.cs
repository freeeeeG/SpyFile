using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000A13 RID: 2579
public class ServerPlayerControlsImpl_Default : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x060032D3 RID: 13011 RVA: 0x000EDCBC File Offset: 0x000EC0BC
	private void Awake()
	{
		base.enabled = false;
		Mailbox.Server.RegisterForMessageType(MessageType.ChefEvent, new OrderedMessageReceivedCallback(this.OnChefEvent));
		this.m_playerIDProvider = base.GetComponent<PlayerIDProvider>();
	}

	// Token: 0x060032D4 RID: 13012 RVA: 0x000EDCEC File Offset: 0x000EC0EC
	public override void OnDestroy()
	{
		base.OnDestroy();
		Mailbox.Server.UnregisterForMessageType(MessageType.ChefEvent, new OrderedMessageReceivedCallback(this.OnChefEvent));
		if (this.m_iCarrier != null)
		{
			this.m_iCarrier.UnregisterCarriedItemChangeCallback(new VoidGeneric<GameObject, GameObject>(this.OnCarriedItemChanged));
		}
	}

	// Token: 0x060032D5 RID: 13013 RVA: 0x000EDD39 File Offset: 0x000EC139
	public override EntityType GetEntityType()
	{
		return EntityType.InputEvent;
	}

	// Token: 0x060032D6 RID: 13014 RVA: 0x000EDD40 File Offset: 0x000EC140
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(base.gameObject);
		this.m_entityID = entry.m_Header.m_uEntityID;
		this.m_playerControlsImpl_Default = (PlayerControlsImpl_Default)synchronisedObject;
		if (this.m_playerControlsImpl_Default != null)
		{
			this.m_playerControlsImpl_Default.m_serverImpl = this;
		}
	}

	// Token: 0x060032D7 RID: 13015 RVA: 0x000EDD9C File Offset: 0x000EC19C
	public void Init(PlayerControls _controls)
	{
		this.m_controls = _controls;
		this.m_controlScheme = _controls.ControlScheme;
		this.m_controlObject = _controls.gameObject;
		this.m_iCarrier = _controls.gameObject.RequireInterface<ICarrier>();
		this.m_iCatcher = _controls.gameObject.RequireInterface<IHandleCatch>();
		this.m_iThrower = _controls.gameObject.RequireInterface<IThrower>();
		this.m_switchingManager = GameUtils.RequireManager<PlayerSwitchingManager>();
		this.m_switchingManager.AvatarSelectChangeCallback += this.OnAvatarSelectionChange;
		base.enabled = true;
		this.m_iCarrier.RegisterCarriedItemChangeCallback(new VoidGeneric<GameObject, GameObject>(this.OnCarriedItemChanged));
	}

	// Token: 0x060032D8 RID: 13016 RVA: 0x000EDE3B File Offset: 0x000EC23B
	private void OnCarriedItemChanged(GameObject _before, GameObject _after)
	{
		if (this.m_lastInteracted != null && this.m_lastInteracted.GetComponent<UsableItem>())
		{
			this.EndInteraction();
		}
	}

	// Token: 0x060032D9 RID: 13017 RVA: 0x000EDE69 File Offset: 0x000EC269
	private void OnAvatarSelectionChange(PlayerInputLookup.Player _player, PlayerControls _controls)
	{
		if (this.m_controls == _controls)
		{
			this.m_autoSwitchTimer = float.MinValue;
			if (_controls != null)
			{
				_controls.ControlScheme.ClearEvents();
			}
		}
	}

	// Token: 0x060032DA RID: 13018 RVA: 0x000EDE9E File Offset: 0x000EC29E
	public void Enable()
	{
		base.enabled = true;
	}

	// Token: 0x060032DB RID: 13019 RVA: 0x000EDEA7 File Offset: 0x000EC2A7
	public void Disable()
	{
		this.EndInteraction();
		base.enabled = false;
	}

	// Token: 0x060032DC RID: 13020 RVA: 0x000EDEB6 File Offset: 0x000EC2B6
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
	}

	// Token: 0x060032DD RID: 13021 RVA: 0x000EDEC0 File Offset: 0x000EC2C0
	public void Update_Impl()
	{
		float deltaTime = TimeManager.GetDeltaTime(base.gameObject);
		this.Update_Carry(deltaTime);
		this.Update_Catch(deltaTime);
		this.Update_AutoSwitch(deltaTime, this.m_controlScheme);
		this.Update_Interacting();
	}

	// Token: 0x060032DE RID: 13022 RVA: 0x000EDEFA File Offset: 0x000EC2FA
	public void SetPlayerControlSchemeData(PlayerControls.ControlSchemeData _controlScheme)
	{
		this.m_controlScheme = _controlScheme;
	}

	// Token: 0x060032DF RID: 13023 RVA: 0x000EDF04 File Offset: 0x000EC304
	private void Update_AutoSwitch(float _deltaTime, PlayerControls.ControlSchemeData _controlSceme)
	{
		if (this.m_autoSwitchTimer > 0f)
		{
			this.m_autoSwitchTimer -= _deltaTime;
			if (this.m_autoSwitchTimer <= 0f)
			{
				this.m_switchingManager.ForceSwitchToNext(_controlSceme.Player);
			}
		}
	}

	// Token: 0x060032E0 RID: 13024 RVA: 0x000EDF50 File Offset: 0x000EC350
	private void Update_Carry(float _deltaTime)
	{
		if (this.m_iCarrier.InspectCarriedItem() != null && PlayerControlsHelper.WouldFallIfHoldingNothing(this.m_controls))
		{
			Vector2 directionXZ = base.transform.forward.XZ();
			PlayerControlsHelper.DropHeldItem(this.m_controls, directionXZ);
		}
	}

	// Token: 0x060032E1 RID: 13025 RVA: 0x000EDFA0 File Offset: 0x000EC3A0
	public void ReceivePickUpEvent(GameObject _target)
	{
		if (_target != null && this.m_iCarrier.InspectCarriedItem() == null)
		{
			IHandlePickup controllingPickupHandler_Server = PlayerControlsHelper.GetControllingPickupHandler_Server(_target);
			if (controllingPickupHandler_Server != null && controllingPickupHandler_Server.CanHandlePickup(this.m_iCarrier))
			{
				Vector2 normalized = this.m_controls.transform.forward.XZ().normalized;
				controllingPickupHandler_Server.HandlePickup(this.m_iCarrier, normalized);
			}
		}
	}

	// Token: 0x060032E2 RID: 13026 RVA: 0x000EE018 File Offset: 0x000EC418
	public void ReceivePlaceEvent(GameObject _target)
	{
		if (_target != null && this.m_iCarrier.InspectCarriedItem() != null)
		{
			PlayerControlsHelper.PlaceHeldItem_Server(this.m_controls, _target);
		}
	}

	// Token: 0x060032E3 RID: 13027 RVA: 0x000EE048 File Offset: 0x000EC448
	public void ReceiveTakeEvent()
	{
		if (this.m_iCarrier.InspectCarriedItem() != null)
		{
			this.m_iCarrier.TakeItem();
		}
	}

	// Token: 0x060032E4 RID: 13028 RVA: 0x000EE06C File Offset: 0x000EC46C
	public void ReceiveInteractEvent(GameObject _target)
	{
		if (null != _target)
		{
			ServerInteractable component = _target.GetComponent<ServerInteractable>();
			bool flag = component != null && component.CanInteract(this.m_controlObject);
			bool flag2 = component != this.m_lastInteracted;
			if (flag)
			{
				if (flag2)
				{
					this.EndInteraction();
					this.BeginInteraction(component);
				}
			}
			else
			{
				this.EndInteraction();
			}
		}
		else
		{
			this.EndInteraction();
		}
	}

	// Token: 0x060032E5 RID: 13029 RVA: 0x000EE0E4 File Offset: 0x000EC4E4
	public void ReceiveTriggerInteractEvent(GameObject _target)
	{
		ServerInteractable serverInteractable = null;
		if (_target != null)
		{
			serverInteractable = _target.RequestComponent<ServerInteractable>();
		}
		bool flag = serverInteractable != null && serverInteractable.CanInteract(this.m_controlObject);
		if (flag)
		{
			this.TriggerInteractable(serverInteractable);
		}
	}

	// Token: 0x060032E6 RID: 13030 RVA: 0x000EE130 File Offset: 0x000EC530
	public void ReceiveThrowEvent(GameObject _target)
	{
		if (_target != null)
		{
			if (this.m_iCarrier.InspectCarriedItem() == null || PlayerControlsHelper.IsHeldItemInsideStaticCollision(this.m_controls))
			{
				return;
			}
			IThrowable throwable = _target.RequireInterface<IThrowable>();
			Vector3 v = this.m_controlObject.transform.forward.normalized.XZ();
			if (throwable.CanHandleThrow(this.m_iThrower, v))
			{
				this.m_iCarrier.TakeItem();
				throwable.HandleThrow(this.m_iThrower, v);
				EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(_target);
				this.SendServerEvent(new InputEventMessage(InputEventMessage.InputEventType.EndThrow)
				{
					entityId = entry.m_Header.m_uEntityID
				});
			}
			else
			{
				this.m_iThrower.OnFailedToThrowItem(this.m_iCarrier.InspectCarriedItem());
			}
		}
	}

	// Token: 0x060032E7 RID: 13031 RVA: 0x000EE218 File Offset: 0x000EC618
	private void Update_Catch(float _deltaTime)
	{
		if (this.m_controls.m_bRespawning)
		{
			return;
		}
		ICatchable catchable = this.m_controls.ScanForCatch();
		if (catchable != null)
		{
			MonoBehaviour monoBehaviour = (MonoBehaviour)catchable;
			if (monoBehaviour == null || monoBehaviour.gameObject == null)
			{
				return;
			}
			Vector2 normalized = this.m_controlObject.transform.forward.XZ().normalized;
			if (this.m_iCatcher.CanHandleCatch(catchable, normalized))
			{
				this.m_iCatcher.HandleCatch(catchable, normalized);
				EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(monoBehaviour.gameObject);
				this.SendServerEvent(new InputEventMessage(InputEventMessage.InputEventType.Catch)
				{
					entityId = entry.m_Header.m_uEntityID
				});
			}
		}
	}

	// Token: 0x060032E8 RID: 13032 RVA: 0x000EE2E0 File Offset: 0x000EC6E0
	public void OnTrigger(string _trigger)
	{
		if (_trigger == "Curse")
		{
			InputEventMessage message = new InputEventMessage(InputEventMessage.InputEventType.Curse);
			this.SendServerEvent(message);
		}
	}

	// Token: 0x060032E9 RID: 13033 RVA: 0x000EE30B File Offset: 0x000EC70B
	private void Update_Interacting()
	{
		if (null != this.m_lastInteracted && (!this.m_lastInteracted.CanInteract(base.gameObject) || this.m_controls.m_bRespawning))
		{
			this.EndInteraction();
		}
	}

	// Token: 0x060032EA RID: 13034 RVA: 0x000EE34C File Offset: 0x000EC74C
	private void TriggerInteractable(ServerInteractable _interactable)
	{
		InputEventMessage inputEventMessage = new InputEventMessage(InputEventMessage.InputEventType.TriggerInteraction);
		if (_interactable != null)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(_interactable.gameObject);
			uint uEntityID = entry.m_Header.m_uEntityID;
			inputEventMessage.entityId = uEntityID;
		}
		else
		{
			inputEventMessage.entityId = 0U;
		}
		this.SendServerEvent(inputEventMessage);
		if (_interactable != null)
		{
			_interactable.TriggerInteract(this.m_controlObject, this.m_controlObject.transform.forward.XZ().normalized);
		}
	}

	// Token: 0x060032EB RID: 13035 RVA: 0x000EE3D4 File Offset: 0x000EC7D4
	public void StartDash()
	{
		InputEventMessage message = new InputEventMessage(InputEventMessage.InputEventType.Dash);
		this.SendServerEvent(message);
	}

	// Token: 0x060032EC RID: 13036 RVA: 0x000EE3F0 File Offset: 0x000EC7F0
	public void StartDashCollision(Vector3 collisionPoint)
	{
		this.SendServerEvent(new InputEventMessage(InputEventMessage.InputEventType.DashCollision)
		{
			collisionContactPoint = collisionPoint
		});
	}

	// Token: 0x060032ED RID: 13037 RVA: 0x000EE414 File Offset: 0x000EC814
	private void OnChefEvent(IOnlineMultiplayerSessionUserId userID, Serialisable serialisable)
	{
		ChefEventMessage chefEventMessage = (ChefEventMessage)serialisable;
		if (this.m_entityID != chefEventMessage.ChefEntityID)
		{
			return;
		}
		EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(chefEventMessage.EntityID);
		EntitySerialisationEntry entry2 = EntitySerialisationRegistry.GetEntry(chefEventMessage.ChefEntityID);
		if (entry2 != null)
		{
			GameObject target = null;
			if (entry != null)
			{
				entry.SetRequiresUrgentUpdate(true);
				target = entry.m_GameObject;
			}
			switch (chefEventMessage.EventType)
			{
			case ChefEventMessage.ChefEventType.PickUp:
				this.ReceivePickUpEvent(target);
				break;
			case ChefEventMessage.ChefEventType.Place:
				this.ReceivePlaceEvent(target);
				break;
			case ChefEventMessage.ChefEventType.Take:
				this.ReceiveTakeEvent();
				break;
			case ChefEventMessage.ChefEventType.Interact:
				this.ReceiveInteractEvent(target);
				break;
			case ChefEventMessage.ChefEventType.TriggerInteract:
				this.ReceiveTriggerInteractEvent(target);
				break;
			case ChefEventMessage.ChefEventType.Throw:
				this.ReceiveThrowEvent(target);
				break;
			case ChefEventMessage.ChefEventType.KnockBack:
				this.ReceiveKnockbackEvent(chefEventMessage.Knockback_Type, chefEventMessage.KnockbackForce, chefEventMessage.RelativeContactPoint);
				break;
			}
		}
	}

	// Token: 0x060032EE RID: 13038 RVA: 0x000EE502 File Offset: 0x000EC902
	private void ReceiveKnockbackEvent(ChefEventMessage.KnockbackType _knockbackType, Vector2 _knockbackForce, Vector3 _relativeCollisionPoint)
	{
		this.m_playerControlsImpl_Default.m_clientImpl.DoKnockback(_knockbackType, _knockbackForce, _relativeCollisionPoint);
		ServerMessenger.SendChefEffectMessage(this.m_entityID, ChefEffectMessage.EffectType.Impact, _relativeCollisionPoint);
	}

	// Token: 0x060032EF RID: 13039 RVA: 0x000EE524 File Offset: 0x000EC924
	private void BeginInteraction(ServerInteractable _interactable)
	{
		InputEventMessage inputEventMessage = new InputEventMessage(InputEventMessage.InputEventType.BeginInteraction);
		if (_interactable != null)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(_interactable.gameObject);
			uint uEntityID = entry.m_Header.m_uEntityID;
			inputEventMessage.entityId = uEntityID;
		}
		else
		{
			inputEventMessage.entityId = 0U;
		}
		this.SendServerEvent(inputEventMessage);
		if (_interactable != null)
		{
			_interactable.BeginInteract(this.m_controlObject, this.m_controlObject.transform.forward.XZ().normalized);
		}
		this.m_lastInteracted = _interactable;
	}

	// Token: 0x060032F0 RID: 13040 RVA: 0x000EE5B4 File Offset: 0x000EC9B4
	private void EndInteraction()
	{
		InputEventMessage message = new InputEventMessage(InputEventMessage.InputEventType.EndInteraction);
		this.SendServerEvent(message);
		if (this.m_lastInteracted != null)
		{
			this.m_lastInteracted.EndInteract(this.m_controlObject);
			this.m_lastInteracted = null;
		}
	}

	// Token: 0x040028E7 RID: 10471
	private PlayerControls m_controls;

	// Token: 0x040028E8 RID: 10472
	private PlayerControls.ControlSchemeData m_controlScheme;

	// Token: 0x040028E9 RID: 10473
	private GameObject m_controlObject;

	// Token: 0x040028EA RID: 10474
	private ServerInteractable m_lastInteracted;

	// Token: 0x040028EB RID: 10475
	private PlayerSwitchingManager m_switchingManager;

	// Token: 0x040028EC RID: 10476
	private ICarrier m_iCarrier;

	// Token: 0x040028ED RID: 10477
	private IHandleCatch m_iCatcher;

	// Token: 0x040028EE RID: 10478
	private IThrower m_iThrower;

	// Token: 0x040028EF RID: 10479
	private float m_autoSwitchTimer = float.MinValue;

	// Token: 0x040028F0 RID: 10480
	private const string m_curseTrigger = "Curse";

	// Token: 0x040028F1 RID: 10481
	private uint m_entityID;

	// Token: 0x040028F2 RID: 10482
	private PlayerControlsImpl_Default m_playerControlsImpl_Default;

	// Token: 0x040028F3 RID: 10483
	private PlayerIDProvider m_playerIDProvider;
}
