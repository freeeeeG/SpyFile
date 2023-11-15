using System;
using System.Collections.Generic;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000A14 RID: 2580
public class ClientPlayerControlsImpl_Default : ClientSynchroniserBase
{
	// Token: 0x060032F2 RID: 13042 RVA: 0x000EE6BC File Offset: 0x000ECABC
	private void Awake()
	{
		base.enabled = false;
		this.m_playerIDProvider = base.gameObject.RequireComponent<PlayerIDProvider>();
		this.m_onChefEffectReceived = new OrderedMessageReceivedCallback(this.OnChefEffectReceived);
		this.m_Transform = base.transform;
		this.m_SlopedGroundMask = LayerMask.GetMask(new string[]
		{
			"SlopedGround"
		});
	}

	// Token: 0x060032F3 RID: 13043 RVA: 0x000EE720 File Offset: 0x000ECB20
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(base.gameObject);
		this.m_entityID = entry.m_Header.m_uEntityID;
		this.m_controlsImpl = (PlayerControlsImpl_Default)synchronisedObject;
		if (this.m_controlsImpl != null)
		{
			this.m_controlsImpl.m_clientImpl = this;
		}
		Mailbox.Client.RegisterForMessageType(MessageType.ChefEffect, this.m_onChefEffectReceived);
	}

	// Token: 0x060032F4 RID: 13044 RVA: 0x000EE78C File Offset: 0x000ECB8C
	public override EntityType GetEntityType()
	{
		return EntityType.InputEvent;
	}

	// Token: 0x060032F5 RID: 13045 RVA: 0x000EE790 File Offset: 0x000ECB90
	private void OnChefEffectReceived(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		ChefEffectMessage chefEffectMessage = (ChefEffectMessage)message;
		if (chefEffectMessage.ChefEntityID == this.m_entityID)
		{
			ChefEffectMessage.EffectType effect = chefEffectMessage.Effect;
			if (effect != ChefEffectMessage.EffectType.Impact)
			{
				if (effect == ChefEffectMessage.EffectType.Dash)
				{
					this.DoDashCollisionEffects(this.m_playerIDProvider, chefEffectMessage.RelativePosition);
				}
			}
			else
			{
				this.DoImpactEffect(chefEffectMessage.RelativePosition);
			}
		}
	}

	// Token: 0x060032F6 RID: 13046 RVA: 0x000EE7FB File Offset: 0x000ECBFB
	public void RegisterForInteractTrigger(VoidGeneric<ClientInteractable> _callback)
	{
		this.m_interactTriggerCallback = (VoidGeneric<ClientInteractable>)Delegate.Combine(this.m_interactTriggerCallback, _callback);
	}

	// Token: 0x060032F7 RID: 13047 RVA: 0x000EE814 File Offset: 0x000ECC14
	public void UnregisterForInteractTrigger(VoidGeneric<ClientInteractable> _callback)
	{
		this.m_interactTriggerCallback = (VoidGeneric<ClientInteractable>)Delegate.Remove(this.m_interactTriggerCallback, _callback);
	}

	// Token: 0x060032F8 RID: 13048 RVA: 0x000EE82D File Offset: 0x000ECC2D
	public void RegisterForThrowTrigger(VoidGeneric<GameObject> _callback)
	{
		this.m_throwTriggerCallback = (VoidGeneric<GameObject>)Delegate.Combine(this.m_throwTriggerCallback, _callback);
	}

	// Token: 0x060032F9 RID: 13049 RVA: 0x000EE846 File Offset: 0x000ECC46
	public void UnregisterForThrowTrigger(VoidGeneric<GameObject> _callback)
	{
		this.m_throwTriggerCallback = (VoidGeneric<GameObject>)Delegate.Remove(this.m_throwTriggerCallback, _callback);
	}

	// Token: 0x060032FA RID: 13050 RVA: 0x000EE85F File Offset: 0x000ECC5F
	public void RegisterForFallingTrigger(VoidGeneric<bool> _callback)
	{
		this.m_fallingTriggerCallback = (VoidGeneric<bool>)Delegate.Combine(this.m_fallingTriggerCallback, _callback);
	}

	// Token: 0x060032FB RID: 13051 RVA: 0x000EE878 File Offset: 0x000ECC78
	public void UnregisterForFallingTrigger(VoidGeneric<bool> _callback)
	{
		this.m_fallingTriggerCallback = (VoidGeneric<bool>)Delegate.Remove(this.m_fallingTriggerCallback, _callback);
	}

	// Token: 0x060032FC RID: 13052 RVA: 0x000EE894 File Offset: 0x000ECC94
	public void Init(PlayerControls _controls)
	{
		this.m_controls = _controls;
		this.m_controlScheme = _controls.ControlScheme;
		this.m_controlObject = _controls.gameObject;
		this.m_collisionRecorder = _controls.gameObject.RequireComponent<CollisionRecorder>();
		this.m_controlsPlayer = _controls.gameObject.RequireComponent<PlayerIDProvider>();
		this.m_iCarrier = _controls.gameObject.RequireInterface<ICarrier>();
		this.m_iThrower = _controls.gameObject.RequireInterface<IClientThrower>();
		this.m_iCatcher = _controls.gameObject.RequireInterface<IClientHandleCatch>();
		if (T17InGameFlow.Instance != null)
		{
			T17InGameFlow.Instance.RegisterOnPauseMenuVisibilityChanged(new BaseMenuBehaviour.BaseMenuBehaviourEvent(this.OnPauseMenuVisibilityChange));
		}
	}

	// Token: 0x060032FD RID: 13053 RVA: 0x000EE93B File Offset: 0x000ECD3B
	public void Enable()
	{
		this.m_iCarrier.RegisterCarriedItemChangeCallback(new VoidGeneric<GameObject, GameObject>(this.OnCarriedItemChanged));
		base.enabled = true;
	}

	// Token: 0x060032FE RID: 13054 RVA: 0x000EE95C File Offset: 0x000ECD5C
	public void Disable()
	{
		this.m_iCarrier.UnregisterCarriedItemChangeCallback(new VoidGeneric<GameObject, GameObject>(this.OnCarriedItemChanged));
		this.m_lastVelocity = Vector3.zero;
		this.m_dashTimer = float.MinValue;
		this.EndInteraction();
		base.enabled = false;
		base.GetComponent<Rigidbody>().velocity = Vector3.zero;
	}

	// Token: 0x060032FF RID: 13055 RVA: 0x000EE9B3 File Offset: 0x000ECDB3
	public void SetPlayerControlSchemeData(PlayerControls.ControlSchemeData _controlScheme)
	{
		this.m_controlScheme = _controlScheme;
	}

	// Token: 0x06003300 RID: 13056 RVA: 0x000EE9BC File Offset: 0x000ECDBC
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
	}

	// Token: 0x06003301 RID: 13057 RVA: 0x000EE9C4 File Offset: 0x000ECDC4
	public void Update_Impl()
	{
		float deltaTime = TimeManager.GetDeltaTime(base.gameObject);
		bool flag = this.m_playerIDProvider.IsLocallyControlled();
		if (TimeManager.IsPaused(TimeManager.PauseLayer.Network) && flag)
		{
			if (this.m_controlScheme != null)
			{
				this.Update_Movement(deltaTime, true);
			}
			return;
		}
		if (TimeManager.IsPaused(TimeManager.PauseLayer.Main) && flag)
		{
			return;
		}
		if (this.m_controlScheme != null)
		{
			bool isSuppressed = this.m_controlScheme.IsUseSuppressed();
			bool isUsePressed = this.m_controlScheme.IsUseDown();
			bool justPressed = this.m_controlScheme.IsUseJustPressed();
			bool justReleased = this.m_controlScheme.IsUseJustReleased();
			if (flag)
			{
				this.m_controls.UpdateNearbyObjects();
				this.Update_Carry();
				this.Update_Interact(deltaTime, isUsePressed, justPressed);
				this.Update_Throw(deltaTime, isUsePressed, justReleased, isSuppressed);
			}
			this.Update_Aim(deltaTime, isUsePressed);
			this.Update_Movement(deltaTime, false);
		}
		this.Update_Falling(deltaTime);
	}

	// Token: 0x06003302 RID: 13058 RVA: 0x000EEAA0 File Offset: 0x000ECEA0
	private void Update_Carry()
	{
		if (this.m_controls.ControlScheme.m_pickupButton.JustPressed())
		{
			IClientHandlePickup iHandlePickup = this.m_controls.CurrentInteractionObjects.m_iHandlePickup;
			GameObject x = this.m_iCarrier.InspectCarriedItem();
			if (x != null)
			{
				PlayerControlsHelper.PlaceHeldItem_Client(this.m_controls);
			}
			else if (iHandlePickup != null && iHandlePickup.CanHandlePickup(this.m_iCarrier))
			{
				float num = ClientTime.Time();
				if (num >= this.m_lastPickupTimestamp)
				{
					ClientMessenger.ChefEventMessage(ChefEventMessage.ChefEventType.PickUp, base.gameObject, this.m_controls.CurrentInteractionObjects.m_TheOriginalHandlePickup);
					this.m_lastPickupTimestamp = num + this.m_controls.m_pickupDelay;
				}
			}
			else if (this.m_controls.CurrentInteractionObjects.m_interactable != null && this.m_controls.CurrentInteractionObjects.m_interactable.UsePlacementButton)
			{
				ClientMessenger.ChefEventMessage(ChefEventMessage.ChefEventType.TriggerInteract, base.gameObject, this.m_controls.CurrentInteractionObjects.m_interactable);
			}
		}
	}

	// Token: 0x06003303 RID: 13059 RVA: 0x000EEBB0 File Offset: 0x000ECFB0
	private void Update_Interact(float _deltaTime, bool isUsePressed, bool justPressed)
	{
		ClientInteractable clientInteractable = null;
		if (this.m_controls.CurrentInteractionObjects.m_interactable != null)
		{
			clientInteractable = this.m_controls.CurrentInteractionObjects.m_interactable;
		}
		ClientInteractable clientInteractable2;
		if (isUsePressed && null != clientInteractable && null == this.m_predictedInteracted)
		{
			clientInteractable2 = clientInteractable;
		}
		else if (null == clientInteractable && null != this.m_predictedInteracted && this.m_predictedInteracted.InteractionIsSticky())
		{
			clientInteractable2 = null;
		}
		else if (!isUsePressed && null != this.m_predictedInteracted && !this.m_predictedInteracted.InteractionIsSticky())
		{
			clientInteractable2 = null;
		}
		else if (null != clientInteractable && clientInteractable != this.m_predictedInteracted)
		{
			clientInteractable2 = null;
		}
		else
		{
			clientInteractable2 = this.m_predictedInteracted;
		}
		if (clientInteractable2 != this.m_predictedInteracted)
		{
			this.m_predictedInteracted = clientInteractable2;
			ClientMessenger.ChefEventMessage(ChefEventMessage.ChefEventType.Interact, base.gameObject, clientInteractable2);
		}
		if (justPressed && clientInteractable != null)
		{
			ClientMessenger.ChefEventMessage(ChefEventMessage.ChefEventType.TriggerInteract, base.gameObject, clientInteractable);
		}
	}

	// Token: 0x06003304 RID: 13060 RVA: 0x000EECE8 File Offset: 0x000ED0E8
	private void Update_Throw(float _deltaTime, bool isUsePressed, bool justReleased, bool isSuppressed)
	{
		IClientThrowable clientThrowable = null;
		if (this.m_iCarrier.InspectCarriedItem() != null)
		{
			clientThrowable = this.m_iCarrier.InspectCarriedItem().RequestInterface<IClientThrowable>();
		}
		if (!isSuppressed && justReleased && clientThrowable != null)
		{
			Vector2 normalized = this.m_controlObject.transform.forward.XZ().normalized;
			if (clientThrowable.CanHandleThrow(this.m_iThrower, normalized))
			{
				ClientMessenger.ChefEventMessage(ChefEventMessage.ChefEventType.Throw, base.gameObject, this.m_iCarrier.InspectCarriedItem());
			}
		}
	}

	// Token: 0x06003305 RID: 13061 RVA: 0x000EED84 File Offset: 0x000ED184
	private void Update_Aim(float _deltaTime, bool isUsePressed)
	{
		bool aimingThrow = this.m_aimingThrow;
		IClientThrowable clientThrowable = null;
		if (this.m_iCarrier.InspectCarriedItem() != null)
		{
			clientThrowable = this.m_iCarrier.InspectCarriedItem().RequestInterface<IClientThrowable>();
		}
		if (clientThrowable as MonoBehaviour != null)
		{
			Vector2 normalized = this.m_controlObject.transform.forward.XZ().normalized;
			this.m_aimingThrow = isUsePressed;
			this.m_aimingThrow &= (clientThrowable != null && clientThrowable.CanHandleThrow(this.m_iThrower, normalized));
		}
		else
		{
			this.m_aimingThrow = false;
		}
		PlayerInputLookup.Player id = this.m_playerIDProvider.GetID();
		bool flag = this.m_playerIDProvider.IsLocallyControlled();
		bool directlyUnderPlayerControl = this.m_controls.GetDirectlyUnderPlayerControl();
		if (flag && directlyUnderPlayerControl)
		{
			if (this.m_aimingThrow)
			{
				this.m_controls.ThrowIndicator.Show(true);
			}
			else
			{
				this.m_controls.ThrowIndicator.Show(false);
			}
		}
		else
		{
			this.m_controls.ThrowIndicator.Hide();
		}
	}

	// Token: 0x06003306 RID: 13062 RVA: 0x000EEEA4 File Offset: 0x000ED2A4
	private void Update_MovementSuppression(float _deltaTime, Vector3 _targetDirection)
	{
		float num = this.m_controls.m_analogEnableDeadzoneThreshold * this.m_controls.m_analogEnableDeadzoneThreshold;
		if (this.m_aimingThrow || this.m_sessionInteraction != null)
		{
			this.m_movementInputSuppressed = true;
			if (_targetDirection.sqrMagnitude >= num)
			{
				this.m_lastMoveInputDirection = PlayerControlsHelper.GetControlAxis(this.m_controls, ref this.m_controlAxisData);
			}
		}
		else if (this.m_movementInputSuppressed)
		{
			if (_targetDirection.sqrMagnitude < num)
			{
				this.m_movementInputSuppressed = false;
			}
			else
			{
				Vector3 lhs = _targetDirection.SafeNormalised(Vector3.zero);
				Vector3 rhs = this.m_lastMoveInputDirection.SafeNormalised(Vector3.zero);
				float f = Vector3.Dot(lhs, rhs);
				float num2 = Mathf.Acos(f) * 57.29578f;
				if (num2 >= this.m_controls.m_analogEnableAngleThreshold)
				{
					this.m_movementInputSuppressed = false;
				}
			}
		}
	}

	// Token: 0x06003307 RID: 13063 RVA: 0x000EEF88 File Offset: 0x000ED388
	private void Update_Rotation(float _deltaTime, float xAxis, float yAxis, PlayerControls.InversionType xAxisAllignment, PlayerControls.InversionType yAxisAllignment)
	{
		if (this.m_controls.GetDirectlyUnderPlayerControl())
		{
			PlayerControlsHelper.TurnTowardsControlAxis(xAxis, yAxis, xAxisAllignment, yAxisAllignment, this.m_controlAxisData.TurnSpeed, this.m_controlObject, _deltaTime);
		}
		else
		{
			GameObject gameObject = null;
			ClientAttachmentCatcher clientAttachmentCatcher = this.m_iCatcher as ClientAttachmentCatcher;
			if (clientAttachmentCatcher != null)
			{
				gameObject = clientAttachmentCatcher.GetTrackedThrowable();
			}
			if (gameObject != null)
			{
				Vector3 direction = (gameObject.transform.position - this.m_Transform.position).SafeNormalised(this.m_Transform.forward);
				PlayerControlsHelper.TurnTowardsDirection(this.m_controlObject, direction, this.m_controls.Movement.TurnSpeed, _deltaTime);
			}
		}
	}

	// Token: 0x06003308 RID: 13064 RVA: 0x000EF040 File Offset: 0x000ED440
	private void Update_Movement(float _deltaTime, bool _netPaused)
	{
		this.m_LeftOverTime += _deltaTime;
		_deltaTime = 0.016666668f;
		Vector3 vector = Vector3.zero;
		PlayerControlsHelper.BuildControlAxisData(this.m_controls, ref this.m_controlAxisData);
		float value = this.m_controlAxisData.MoveX.GetValue();
		float value2 = this.m_controlAxisData.MoveY.GetValue();
		if (!_netPaused)
		{
			vector = PlayerControlsHelper.GetControlAxis(value, value2, this.m_controlAxisData.XAxisAllignment, this.m_controlAxisData.YAxisAllignment);
		}
		while (this.m_LeftOverTime >= 0.016666668f)
		{
			this.m_LeftOverTime -= 0.016666668f;
			this.Update_Rotation(_deltaTime, value, value2, this.m_controlAxisData.XAxisAllignment, this.m_controlAxisData.YAxisAllignment);
			this.Update_MovementSuppression(_deltaTime, vector);
			this.ApplyGravityForce();
			this.ApplyWindForce(_deltaTime);
			PlayerControls.MovementData movement = this.m_controls.Movement;
			float movementScale = this.m_controls.MovementScale;
			Vector3 vector2 = movementScale * vector * movement.RunSpeed;
			if (this.m_movementInputSuppressed)
			{
				vector2 = Vector3.zero;
			}
			if (this.m_dashTimer > 0f)
			{
				Vector3 a = movementScale * this.m_controlObject.transform.forward * movement.DashSpeed;
				float num = MathUtils.SinusoidalSCurve(this.m_dashTimer / movement.DashTime);
				vector2 = (1f - num) * vector2 + num * a;
			}
			this.m_dashTimer -= _deltaTime;
			if (this.m_impactTimer > 0f)
			{
				float num2 = MathUtils.SinusoidalSCurve(this.m_impactTimer / this.m_impactStartTime);
				vector2 = (1f - num2) * vector2 + num2 * this.m_impactVelocity;
			}
			this.m_impactTimer -= _deltaTime;
			vector2 = Vector3.ClampMagnitude(vector2, movement.MaxSpeed);
			Vector3 vector3 = this.ProgressVelocityWrtFriction(this.m_lastVelocity, this.m_controls.Motion.GetVelocity(), vector2, this.GetSurfaceData());
			this.m_controls.Motion.SetVelocity(vector3);
			this.m_lastVelocity = vector3;
			this.ApplyGroundMovement(_deltaTime);
			if (!_netPaused && this.m_controlScheme.m_dashButton.JustPressed() && movement.DashTime - this.m_dashTimer >= movement.DashCooldown && this.m_impactTimer < 0f)
			{
				this.m_dashTimer = movement.DashTime;
				if (this.m_controlsImpl.m_serverImpl != null)
				{
					this.m_controlsImpl.m_serverImpl.StartDash();
				}
				else
				{
					this.DoDash();
				}
				List<Collision> recentCollisions = this.m_collisionRecorder.GetRecentCollisions();
				for (int i = 0; i < recentCollisions.Count; i++)
				{
					Collision collision = recentCollisions[i];
					PlayerControls playerControls = collision.gameObject.RequestComponent<PlayerControls>();
					if (playerControls != null)
					{
						Vector3 relativeVelocity = collision.relativeVelocity + movementScale * this.m_controlObject.transform.forward * movement.DashSpeed;
						this.OnDashCollision(playerControls, collision.contacts[0].point, relativeVelocity);
					}
				}
			}
			this.m_attemptedDistanceCounter += vector2.magnitude * _deltaTime;
			if (this.m_attemptedDistanceCounter >= movement.FootstepLength)
			{
				this.m_attemptedDistanceCounter %= movement.FootstepLength;
			}
		}
	}

	// Token: 0x06003309 RID: 13065 RVA: 0x000EF3D0 File Offset: 0x000ED7D0
	private void Update_Falling(float _deltaTime)
	{
		if (this.m_controls.GroundCollider == null)
		{
			this.m_timeOffGround += _deltaTime;
			if (!this.m_isFalling && this.m_timeOffGround > this.m_controls.m_timeBeforeFalling)
			{
				this.m_fallingTriggerCallback(true);
				this.m_isFalling = true;
			}
		}
		else
		{
			if (this.m_isFalling)
			{
				this.m_isFalling = false;
				this.m_fallingTriggerCallback(false);
			}
			this.m_timeOffGround = 0f;
		}
	}

	// Token: 0x0600330A RID: 13066 RVA: 0x000EF463 File Offset: 0x000ED863
	public bool IsDashing()
	{
		return this.m_dashTimer > 0f;
	}

	// Token: 0x0600330B RID: 13067 RVA: 0x000EF474 File Offset: 0x000ED874
	public void OnCollisionEnter(Collision _collision)
	{
		GameObject gameObject = _collision.gameObject;
		if (this.m_dashTimer > 0f)
		{
			PlayerControls playerControls = gameObject.RequestComponent<PlayerControls>();
			if (playerControls != null)
			{
				this.OnDashCollision(playerControls, _collision.contacts[0].point, _collision.relativeVelocity);
			}
		}
		if (this.m_playerIDProvider.IsLocallyControlled())
		{
			IClientThrowable clientThrowable = gameObject.RequestInterfaceInImmediateChildren<IClientThrowable>();
			if (clientThrowable != null)
			{
				ContactPoint contactPoint = _collision.contacts[0];
				this.OnThrowableCollision(clientThrowable, _collision.collider, contactPoint.point, contactPoint.normal, _collision.relativeVelocity);
			}
			FireHazard x = gameObject.RequestInterface<FireHazard>();
			if (x != null)
			{
				ContactPoint contactPoint2 = _collision.contacts[0];
				this.OnFireHazardCollision(contactPoint2.point, _collision.contacts[0].normal, _collision.relativeVelocity);
			}
		}
		Steppable steppable = gameObject.RequestComponent<Steppable>();
		if (steppable != null)
		{
			ContactPoint contactPoint3 = _collision.contacts[0];
			this.OnStepCollision(steppable, contactPoint3.point, contactPoint3.normal, _collision.relativeVelocity);
		}
	}

	// Token: 0x0600330C RID: 13068 RVA: 0x000EF5AC File Offset: 0x000ED9AC
	private void OnDashCollision(PlayerControls _otherPlayer, Vector3 _contactPoint, Vector3 _relativeVelocity)
	{
		bool flag = this.m_dashTimer > 0f;
		if (flag)
		{
			GameObject gameObject = _otherPlayer.gameObject;
			Transform transform = gameObject.transform;
			if (this.m_controlsImpl.m_serverImpl != null)
			{
				this.m_controlsImpl.m_serverImpl.StartDashCollision(_contactPoint);
				ServerMessenger.SendChefEffectMessage(gameObject, ChefEffectMessage.EffectType.Dash, _contactPoint);
			}
			Vector3 normalized = (transform.position - this.m_Transform.position).normalized;
			float magnitude = _relativeVelocity.magnitude;
			ClientPlayerControlsImpl_Default clientImpl = (_otherPlayer.GetActiveControlsImpl() as PlayerControlsImpl_Default).m_clientImpl;
			bool flag2 = clientImpl != null && clientImpl.m_dashTimer > 0f;
			if (flag2)
			{
				this.m_dashTimer = float.MinValue;
				clientImpl.m_dashTimer = float.MinValue;
				Vector3 vector = this.m_Transform.forward;
				Vector3 vector2 = transform.forward;
				float num = Vector3.Dot(vector, normalized);
				vector -= (num + Mathf.Abs(num)) * normalized;
				this.m_Transform.forward = vector;
				float num2 = Vector3.Dot(vector2, -normalized);
				vector2 -= (num2 + Mathf.Abs(num2)) * -normalized;
				transform.forward = vector2;
				this.ApplyImpact(this.m_controls.Movement.DashImpactData.Multiplier * vector.XZ() * magnitude, this.m_controls.Movement.DashImpactData.Time);
				clientImpl.ApplyImpact(this.m_controls.Movement.DashImpactData.Multiplier * vector2.XZ() * magnitude, this.m_controls.Movement.DashImpactData.Time);
			}
			else
			{
				clientImpl.ApplyImpact(this.m_controls.Movement.DashImpactData.Multiplier * normalized.XZ() * magnitude, this.m_controls.Movement.DashImpactData.Time);
			}
		}
	}

	// Token: 0x0600330D RID: 13069 RVA: 0x000EF7CC File Offset: 0x000EDBCC
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		InputEventMessage inputEventMessage = (InputEventMessage)serialisable;
		InputEventMessage.InputEventType inputEventType = inputEventMessage.inputEventType;
		EntitySerialisationEntry entitySerialisationEntry = null;
		uint entityId = inputEventMessage.entityId;
		if (entityId != 0U)
		{
			entitySerialisationEntry = EntitySerialisationRegistry.GetEntry(entityId);
		}
		switch (inputEventType)
		{
		case InputEventMessage.InputEventType.Dash:
			if (this.m_controlsImpl.m_serverImpl != null || !this.m_controlsPlayer.IsLocallyControlled())
			{
				this.DoDash();
			}
			break;
		case InputEventMessage.InputEventType.DashCollision:
			if ((this.m_controlsImpl.m_serverImpl != null || !this.m_controlsPlayer.IsLocallyControlled()) && entitySerialisationEntry != null)
			{
				PlayerIDProvider otherPlayer = entitySerialisationEntry.m_GameObject.RequestComponent<PlayerIDProvider>();
				this.DoDashCollisionEffects(otherPlayer, inputEventMessage.collisionContactPoint);
			}
			break;
		case InputEventMessage.InputEventType.Catch:
			this.DoCatch();
			break;
		case InputEventMessage.InputEventType.Curse:
			this.DoCurse();
			break;
		case InputEventMessage.InputEventType.BeginInteraction:
		{
			ClientInteractable interactable = null;
			if (entitySerialisationEntry != null)
			{
				interactable = entitySerialisationEntry.m_GameObject.GetComponent<ClientInteractable>();
			}
			this.BeginInteraction(interactable);
			break;
		}
		case InputEventMessage.InputEventType.EndInteraction:
			this.EndInteraction();
			break;
		case InputEventMessage.InputEventType.TriggerInteraction:
		{
			ClientInteractable interactable2 = null;
			if (entitySerialisationEntry != null)
			{
				interactable2 = entitySerialisationEntry.m_GameObject.GetComponent<ClientInteractable>();
			}
			this.TriggerInteractable(interactable2);
			break;
		}
		case InputEventMessage.InputEventType.EndThrow:
			this.DoThrow(entitySerialisationEntry.m_GameObject);
			break;
		}
	}

	// Token: 0x0600330E RID: 13070 RVA: 0x000EF924 File Offset: 0x000EDD24
	private void OnCarriedItemChanged(GameObject _before, GameObject _after)
	{
		if (_before != null && _after == null)
		{
			GameUtils.TriggerAudio(GameOneShotAudioTag.PutDown, this.m_controlObject.layer);
		}
		else if (_before == null && _after != null)
		{
			GameUtils.TriggerAudio(GameOneShotAudioTag.Pickup, this.m_controlObject.layer);
		}
	}

	// Token: 0x0600330F RID: 13071 RVA: 0x000EF98A File Offset: 0x000EDD8A
	private void DoDash()
	{
		GameUtils.TriggerAudio(GameOneShotAudioTag.Dash, this.m_controlObject.layer);
		this.m_controls.DashPFXPrefab.InstantiateOnParent(this.m_Transform, true);
	}

	// Token: 0x06003310 RID: 13072 RVA: 0x000EF9B8 File Offset: 0x000EDDB8
	private void DoDashCollisionEffects(PlayerIDProvider _otherPlayer, Vector3 _contactPoint)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_controls.ImpactPFXPrefab, _contactPoint, Quaternion.identity);
		GameUtils.TriggerAudio(GameOneShotAudioTag.Impact, this.m_controlObject.layer);
	}

	// Token: 0x06003311 RID: 13073 RVA: 0x000EF9F2 File Offset: 0x000EDDF2
	private void DoCurse()
	{
		GameUtils.TriggerAudio(GameOneShotAudioTag.Curse, this.m_controlObject.layer);
		this.m_controls.CursePFXPrefab.InstantiatePFX(this.m_Transform);
	}

	// Token: 0x06003312 RID: 13074 RVA: 0x000EFA1E File Offset: 0x000EDE1E
	private void DoInteraction(ClientInteractable interactable)
	{
		this.m_interactTriggerCallback(interactable);
	}

	// Token: 0x06003313 RID: 13075 RVA: 0x000EFA2C File Offset: 0x000EDE2C
	private void DoThrow(GameObject _throwable)
	{
		GameUtils.TriggerAudio(GameOneShotAudioTag.Throw, base.gameObject.layer);
		this.m_throwTriggerCallback(_throwable);
		OvercookedAchievementManager overcookedAchievementManager = GameUtils.RequestManager<OvercookedAchievementManager>();
		if (overcookedAchievementManager != null)
		{
			ControlPadInput.PadNum padForPlayer = PlayerInputLookup.GetPadForPlayer(this.m_controlsPlayer.GetID());
			overcookedAchievementManager.IncStat(3, 1f, padForPlayer);
		}
	}

	// Token: 0x06003314 RID: 13076 RVA: 0x000EFA88 File Offset: 0x000EDE88
	private void DoCatch()
	{
		GameUtils.TriggerAudio(GameOneShotAudioTag.Catch, base.gameObject.layer);
		Transform transform = this.m_Transform.FindChildRecursive("Attachment");
		if (null != this.m_controls.CatchPFXPrefab)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_controls.CatchPFXPrefab, transform.position, Quaternion.identity);
		}
		OvercookedAchievementManager overcookedAchievementManager = GameUtils.RequestManager<OvercookedAchievementManager>();
		if (overcookedAchievementManager != null)
		{
			ControlPadInput.PadNum padForPlayer = PlayerInputLookup.GetPadForPlayer(this.m_controlsPlayer.GetID());
			overcookedAchievementManager.IncStat(4, 1f, padForPlayer);
		}
	}

	// Token: 0x06003315 RID: 13077 RVA: 0x000EFB20 File Offset: 0x000EDF20
	private void BeginInteraction(ClientInteractable _interactable)
	{
		this.m_lastInteracted = _interactable;
		this.m_predictedInteracted = _interactable;
		if (_interactable != null)
		{
			_interactable.AddInteractor(base.gameObject);
		}
	}

	// Token: 0x06003316 RID: 13078 RVA: 0x000EFB48 File Offset: 0x000EDF48
	private void EndInteraction()
	{
		if (this.m_lastInteracted != null)
		{
			this.m_lastInteracted.RemoveInteractor(base.gameObject);
		}
		this.m_lastInteracted = null;
		this.m_predictedInteracted = null;
	}

	// Token: 0x06003317 RID: 13079 RVA: 0x000EFB7A File Offset: 0x000EDF7A
	private void TriggerInteractable(ClientInteractable _interactable)
	{
		if (_interactable != null)
		{
			this.DoInteraction(_interactable);
		}
	}

	// Token: 0x06003318 RID: 13080 RVA: 0x000EFB90 File Offset: 0x000EDF90
	public ClientInteractable GetCurrentlyInteracting()
	{
		if (null != this.m_lastInteracted)
		{
			return this.m_lastInteracted;
		}
		if (null != this.m_predictedInteracted)
		{
			return this.m_predictedInteracted;
		}
		if (null != this.m_sessionInteraction)
		{
			return this.m_sessionInteraction;
		}
		return null;
	}

	// Token: 0x06003319 RID: 13081 RVA: 0x000EFBE8 File Offset: 0x000EDFE8
	public void NotifySessionInteractionStarted(ClientSessionInteractable _interaction)
	{
		if (_interaction != null && _interaction.gameObject != null)
		{
			this.m_sessionInteraction = _interaction.gameObject.RequireComponent<ClientInteractable>();
			if (this.m_controlScheme != null)
			{
				this.m_controlScheme.ClearEvents();
			}
		}
	}

	// Token: 0x0600331A RID: 13082 RVA: 0x000EFC39 File Offset: 0x000EE039
	public void NotifySessionInteractionEnded(ClientSessionInteractable _interaction)
	{
		this.m_sessionInteraction = null;
		if (this.m_controlScheme != null)
		{
			this.m_controlScheme.ClearEvents();
		}
	}

	// Token: 0x0600331B RID: 13083 RVA: 0x000EFC58 File Offset: 0x000EE058
	private void OnThrowableCollision(IClientThrowable _throwable, Collider _collider, Vector3 _contactPoint, Vector3 _contactNormal, Vector3 _relativeVelocity)
	{
		float magnitude = _relativeVelocity.magnitude;
		if (_throwable.IsFlying() && _throwable.GetThrower() != this.m_iThrower)
		{
			GameObject gameObject = (_throwable as MonoBehaviour).gameObject;
			ICatchable catchable = gameObject.RequestInterface<ICatchable>();
			bool flag;
			if (catchable != null && (catchable as MonoBehaviour).enabled)
			{
				Vector3 position = this.m_Transform.position;
				Vector3 normalized = this.m_Transform.forward.WithY(0f).normalized;
				flag = !InteractWithItemHelper.IsColliderInArc(_collider, position, normalized, 2f, 1.5707964f);
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				Vector2 vector = this.m_controls.Movement.ThrownImpactData.Multiplier * _contactNormal.XZ() * magnitude;
				Vector3 vector2 = _contactPoint - this.m_Transform.position;
				this.DoKnockback(ChefEventMessage.KnockbackType.Throw, vector, vector2);
				if (this.m_controlsImpl.m_serverImpl == null)
				{
					ClientMessenger.ChefKnockbackEventMessage(ChefEventMessage.KnockbackType.Throw, this.m_entityID, vector, vector2);
				}
				else
				{
					ServerMessenger.SendChefEffectMessage(this.m_entityID, ChefEffectMessage.EffectType.Impact, vector2);
				}
			}
		}
	}

	// Token: 0x0600331C RID: 13084 RVA: 0x000EFD88 File Offset: 0x000EE188
	public void DoKnockback(ChefEventMessage.KnockbackType _knockbackType, Vector2 _knockBackForce, Vector3 _relativeContactPoint)
	{
		bool flag = true;
		float impactTime = 0f;
		if (_knockbackType != ChefEventMessage.KnockbackType.Throw)
		{
			if (_knockbackType == ChefEventMessage.KnockbackType.Fire)
			{
				impactTime = this.m_controls.Movement.HazardImpactData.Time;
			}
		}
		else
		{
			flag = (this.m_dashTimer <= 0f);
			impactTime = this.m_controls.Movement.ThrownImpactData.Time;
		}
		if (flag)
		{
			this.ApplyImpact(_knockBackForce, impactTime);
		}
	}

	// Token: 0x0600331D RID: 13085 RVA: 0x000EFE04 File Offset: 0x000EE204
	public void DoImpactEffect(Vector3 _relativePosition)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_controls.ImpactPFXPrefab, this.m_Transform.position + _relativePosition, Quaternion.identity);
		GameUtils.TriggerAudio(GameOneShotAudioTag.Impact, this.m_controlObject.layer);
	}

	// Token: 0x0600331E RID: 13086 RVA: 0x000EFE50 File Offset: 0x000EE250
	private void OnStepCollision(Steppable _steppable, Vector3 _contactPoint, Vector3 _contactNormal, Vector3 _relativeVelocity)
	{
		float num = Vector3.Angle(_contactNormal, this.m_Transform.up);
		if (num > 30f)
		{
			Vector3 direction = _relativeVelocity.SafeNormalised(Vector3.zero);
			if (direction.sqrMagnitude > 0.001f)
			{
				this.StepOntoObject(_steppable, _contactPoint, direction, this.m_controls);
			}
		}
	}

	// Token: 0x0600331F RID: 13087 RVA: 0x000EFEA8 File Offset: 0x000EE2A8
	private void OnFireHazardCollision(Vector3 _contactPoint, Vector3 _contactNormal, Vector3 _relativeVelocity)
	{
		float magnitude = _relativeVelocity.magnitude;
		Vector3 input = _contactNormal.WithY(0f).SafeNormalised(-this.m_Transform.forward);
		Vector2 vector = this.m_controls.Movement.HazardImpactData.Multiplier * input.XZ() * magnitude;
		Vector3 vector2 = this.m_Transform.position - _contactPoint;
		this.DoKnockback(ChefEventMessage.KnockbackType.Fire, vector, vector2);
		if (this.m_controlsImpl.m_serverImpl == null)
		{
			ClientMessenger.ChefKnockbackEventMessage(ChefEventMessage.KnockbackType.Fire, this.m_entityID, vector, vector2);
		}
		else
		{
			ServerMessenger.SendChefEffectMessage(this.m_entityID, ChefEffectMessage.EffectType.Impact, vector2);
		}
	}

	// Token: 0x06003320 RID: 13088 RVA: 0x000EFF58 File Offset: 0x000EE358
	private Vector3 ProgressVelocityWrtFriction(Vector3 _gameplayVelocity, Vector3 _physicalVelocity, Vector3 _targetVelocity, PlayerPhysicsSurfaceProperties _surface)
	{
		Vector3 groundNormal = this.m_controls.GroundNormal;
		Vector3 normalized = Vector3.Cross(groundNormal, Vector3.forward).normalized;
		Vector3 normalized2 = Vector3.Cross(normalized, groundNormal).normalized;
		Vector3 a = new Vector3(Vector3.Dot(normalized, _gameplayVelocity), Vector3.Dot(groundNormal, _physicalVelocity), Vector3.Dot(normalized2, _gameplayVelocity));
		float d = (!(_surface != null)) ? 1f : _surface.SpeedMultiplier;
		float value = (!(_surface != null)) ? 0f : _surface.Slippiness;
		float num = (!(_surface != null)) ? 0f : _surface.Slidiness;
		float num2 = MathUtils.Remap(value, 0f, 1f, 1f, TimeManager.GetDeltaTime(this.m_controlObject));
		Vector3 b = new Vector3(num2, 0f, num2);
		Vector3 vector = d * _targetVelocity.MultipliedBy(b) + a.MultipliedBy(Vector3.one - b);
		Vector3 a2 = normalized * vector.x + groundNormal * vector.y + normalized2 * vector.z;
		float y = groundNormal.y;
		Vector3 vector2 = groundNormal.WithY(0f).SafeNormalised(Vector3.zero);
		Vector3 a3 = num * (vector2.x * normalized + vector2.z * normalized2);
		float num3 = (1f - y) * Mathf.Clamp01(num);
		return (1f - num3) * a2 + num3 * a3;
	}

	// Token: 0x06003321 RID: 13089 RVA: 0x000F011C File Offset: 0x000EE51C
	private PlayerPhysicsSurfaceProperties GetSurfaceData()
	{
		if (this.m_controls.PhysicsSurface != null)
		{
			return this.m_controls.PhysicsSurface.Properties;
		}
		return null;
	}

	// Token: 0x06003322 RID: 13090 RVA: 0x000F0148 File Offset: 0x000EE548
	private void StepOntoObject(Steppable _steppable, Vector3 _position, Vector3 _direction, PlayerControls _controls)
	{
		Vector3 forward = this.m_Transform.forward;
		Vector3 position = this.m_Transform.position;
		Vector3 vector = _steppable.ProjectPointOntoStep(_position, _direction);
		Vector3 vector2 = vector - position;
		Vector3 from = vector2.WithY(0f).SafeNormalised(forward);
		float num = Vector3.Angle(from, forward);
		if (num < 90f && Vector3.Project(vector2, this.m_Transform.up).sqrMagnitude < _controls.Movement.StepHeightMax * _controls.Movement.StepHeightMax)
		{
			this.m_Transform.position = vector;
		}
	}

	// Token: 0x06003323 RID: 13091 RVA: 0x000F01EB File Offset: 0x000EE5EB
	public void ApplyImpact(Vector2 _xzImpactVelocity, float _impactTime)
	{
		this.m_impactVelocity = VectorUtils.FromXZ(_xzImpactVelocity, 0f);
		this.m_impactStartTime = _impactTime;
		this.m_impactTimer = _impactTime;
	}

	// Token: 0x06003324 RID: 13092 RVA: 0x000F020C File Offset: 0x000EE60C
	private void ApplyWindForce(float _delta)
	{
		Vector3 velocity = this.m_controls.WindReceiver.GetVelocity();
		this.m_controls.Motion.Movement(velocity, _delta);
	}

	// Token: 0x06003325 RID: 13093 RVA: 0x000F023C File Offset: 0x000EE63C
	private void ApplyGravityForce()
	{
		if (DebugManager.Instance.GetOption("New Gravity"))
		{
			if (this.m_controls.m_bApplyGravity)
			{
				Vector3 a = -this.m_controls.GroundNormal;
				float d = (this.m_controls.GroundLayer != this.m_SlopedGroundMask) ? this.m_controls.Movement.GravityStrength : this.m_controls.Movement.GravityStrengthSlopedGround;
				this.m_controls.Motion.Accelerate(a * d);
			}
		}
		else if (!(this.m_controls.GroundCollider != null))
		{
			Vector3 a2 = -Vector3.up;
			this.m_controls.Motion.Accelerate(a2 * this.m_controls.Movement.GravityStrength);
		}
	}

	// Token: 0x06003326 RID: 13094 RVA: 0x000F032C File Offset: 0x000EE72C
	private void ApplyGroundMovement(float _deltaTime)
	{
		Vector3 velocity = this.m_controls.SurfaceMovable.GetVelocity();
		this.m_controls.Motion.Movement(velocity, _deltaTime);
	}

	// Token: 0x06003327 RID: 13095 RVA: 0x000F035C File Offset: 0x000EE75C
	protected void OnPauseMenuVisibilityChange(BaseMenuBehaviour _menu)
	{
		if (this.m_controlScheme != null)
		{
			this.m_controlScheme.ClearEvents();
		}
	}

	// Token: 0x06003328 RID: 13096 RVA: 0x000F0374 File Offset: 0x000EE774
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Mailbox.Client.UnregisterForMessageType(MessageType.ChefEffect, this.m_onChefEffectReceived);
		if (T17InGameFlow.Instance != null)
		{
			T17InGameFlow.Instance.UnRegisterOnPauseMenuVisibilityChanged(new BaseMenuBehaviour.BaseMenuBehaviourEvent(this.OnPauseMenuVisibilityChange));
		}
	}

	// Token: 0x040028F4 RID: 10484
	private PlayerControlsImpl_Default m_controlsImpl;

	// Token: 0x040028F5 RID: 10485
	private ClientInteractable m_lastInteracted;

	// Token: 0x040028F6 RID: 10486
	private ClientInteractable m_predictedInteracted;

	// Token: 0x040028F7 RID: 10487
	private Transform m_Transform;

	// Token: 0x040028F8 RID: 10488
	private ClientInteractable m_sessionInteraction;

	// Token: 0x040028F9 RID: 10489
	private PlayerControls m_controls;

	// Token: 0x040028FA RID: 10490
	private PlayerControls.ControlSchemeData m_controlScheme;

	// Token: 0x040028FB RID: 10491
	private GameObject m_controlObject;

	// Token: 0x040028FC RID: 10492
	private CollisionRecorder m_collisionRecorder;

	// Token: 0x040028FD RID: 10493
	private PlayerIDProvider m_controlsPlayer;

	// Token: 0x040028FE RID: 10494
	private ICarrier m_iCarrier;

	// Token: 0x040028FF RID: 10495
	private IClientThrower m_iThrower;

	// Token: 0x04002900 RID: 10496
	private IClientHandleCatch m_iCatcher;

	// Token: 0x04002901 RID: 10497
	private Vector3 m_lastVelocity;

	// Token: 0x04002902 RID: 10498
	private float m_dashTimer = float.MinValue;

	// Token: 0x04002903 RID: 10499
	private float m_attemptedDistanceCounter;

	// Token: 0x04002904 RID: 10500
	private bool m_aimingThrow;

	// Token: 0x04002905 RID: 10501
	private bool m_movementInputSuppressed;

	// Token: 0x04002906 RID: 10502
	private Vector3 m_lastMoveInputDirection;

	// Token: 0x04002907 RID: 10503
	private float m_impactStartTime = float.MaxValue;

	// Token: 0x04002908 RID: 10504
	private float m_impactTimer = float.MinValue;

	// Token: 0x04002909 RID: 10505
	private Vector3 m_impactVelocity = Vector3.zero;

	// Token: 0x0400290A RID: 10506
	private float m_timeOffGround;

	// Token: 0x0400290B RID: 10507
	private bool m_isFalling;

	// Token: 0x0400290C RID: 10508
	private VoidGeneric<ClientInteractable> m_interactTriggerCallback = delegate(ClientInteractable param1)
	{
	};

	// Token: 0x0400290D RID: 10509
	private VoidGeneric<GameObject> m_throwTriggerCallback = delegate(GameObject param1)
	{
	};

	// Token: 0x0400290E RID: 10510
	private VoidGeneric<bool> m_fallingTriggerCallback = delegate(bool param1)
	{
	};

	// Token: 0x0400290F RID: 10511
	private PlayerControlsHelper.ControlAxisData m_controlAxisData = default(PlayerControlsHelper.ControlAxisData);

	// Token: 0x04002910 RID: 10512
	private PlayerIDProvider m_playerIDProvider;

	// Token: 0x04002911 RID: 10513
	private const float sixtyFPS = 0.016666668f;

	// Token: 0x04002912 RID: 10514
	private float m_LeftOverTime;

	// Token: 0x04002913 RID: 10515
	private OrderedMessageReceivedCallback m_onChefEffectReceived;

	// Token: 0x04002914 RID: 10516
	private uint m_entityID;

	// Token: 0x04002915 RID: 10517
	private float m_lastPickupTimestamp;

	// Token: 0x04002916 RID: 10518
	private LayerMask m_SlopedGroundMask = 0;
}
