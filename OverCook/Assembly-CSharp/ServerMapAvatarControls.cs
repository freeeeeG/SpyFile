using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020009F7 RID: 2551
public class ServerMapAvatarControls : ServerSynchroniserBase
{
	// Token: 0x060031C7 RID: 12743 RVA: 0x000E93F2 File Offset: 0x000E77F2
	public override EntityType GetEntityType()
	{
		return EntityType.WorldMapVanControls;
	}

	// Token: 0x060031C8 RID: 12744 RVA: 0x000E93F6 File Offset: 0x000E77F6
	private bool CanButtonBePressed()
	{
		return !T17DialogBoxManager.HasAnyOpenDialogs() && (!(T17InGameFlow.Instance != null) || !(T17InGameFlow.Instance.m_Rootmenu.GetCurrentOpenMenu() != null));
	}

	// Token: 0x060031C9 RID: 12745 RVA: 0x000E9430 File Offset: 0x000E7830
	private void InitControls()
	{
		this.m_moveX = new GateLogicalValue(PlayerInputLookup.GetAnyValueForLocals(PlayerInputLookup.LogicalValueID.MovementX), new Generic<bool>(this.CanButtonBePressed));
		this.m_moveY = new GateLogicalValue(PlayerInputLookup.GetAnyValueForLocals(PlayerInputLookup.LogicalValueID.MovementY), new Generic<bool>(this.CanButtonBePressed));
		this.m_selectButton = new GateLogicalButton(PlayerInputLookup.GetAnyButton(PlayerInputLookup.LogicalButtonID.UISelectNotStart), new Generic<bool>(this.CanButtonBePressed));
		this.m_dashButton = new GateLogicalButton(PlayerInputLookup.GetAnyButton(PlayerInputLookup.LogicalButtonID.Dash), new Generic<bool>(this.CanButtonBePressed));
	}

	// Token: 0x060031CA RID: 12746 RVA: 0x000E94B4 File Offset: 0x000E78B4
	private PortalMapNode FindNodeForLevel(int _levelIndex)
	{
		PortalMapNode[] array = UnityEngine.Object.FindObjectsOfType<PortalMapNode>();
		PortalMapNode portalMapNode = Array.Find<PortalMapNode>(array, (PortalMapNode x) => x.LevelIndex == _levelIndex);
		if (portalMapNode == null)
		{
			MultiLevelMiniPortalMapNode[] array2 = UnityEngine.Object.FindObjectsOfType<MultiLevelMiniPortalMapNode>();
			portalMapNode = Array.Find<MultiLevelMiniPortalMapNode>(array2, (MultiLevelMiniPortalMapNode x) => x.AlternateLevelIndexes.Contains(_levelIndex));
		}
		return portalMapNode;
	}

	// Token: 0x060031CB RID: 12747 RVA: 0x000E9510 File Offset: 0x000E7910
	public void MoveToStartingLocation()
	{
		GameUtils.EnsureBootstrapSetup();
		int lastLevelEntered = GameUtils.GetGameSession().Progress.SaveData.LastLevelEntered;
		if (lastLevelEntered != -1)
		{
			PortalMapNode portalMapNode = this.FindNodeForLevel(lastLevelEntered);
			if (portalMapNode != null)
			{
				base.transform.position = this.m_groundCast.GetClosestPointOnGround(portalMapNode.transform.position);
			}
		}
		else if (this.m_avatarControls.m_bStartOnFirstNode)
		{
			PortalMapNode portalMapNode2 = this.FindNodeForLevel(0);
			if (portalMapNode2 != null)
			{
				base.transform.position = this.m_groundCast.GetClosestPointOnGround(portalMapNode2.transform.position);
			}
		}
		else
		{
			base.transform.position = this.m_groundCast.GetClosestPointOnGround(base.transform.position);
		}
	}

	// Token: 0x060031CC RID: 12748 RVA: 0x000E95E8 File Offset: 0x000E79E8
	public void RotateTowardsNextLevel()
	{
		GameProgress.GameProgressData saveData = GameUtils.GetGameSession().Progress.SaveData;
		int num = saveData.FarthestProgressedLevel(false, true);
		PortalMapNode portalMapNode = this.FindNodeForLevel((num == -1) ? 0 : num);
		int lastLevelEntered = saveData.LastLevelEntered;
		PortalMapNode portalMapNode2 = this.FindNodeForLevel((lastLevelEntered == -1) ? 0 : lastLevelEntered);
		if (portalMapNode != null && portalMapNode2 != null)
		{
			Vector3 closestPointOnGround = this.m_groundCast.GetClosestPointOnGround(portalMapNode.transform.position);
			Vector3 closestPointOnGround2 = this.m_groundCast.GetClosestPointOnGround(portalMapNode2.transform.position);
			Vector3 vector = closestPointOnGround - closestPointOnGround2;
			if (vector.sqrMagnitude > 0f)
			{
				vector = Vector3.Normalize(vector);
				base.gameObject.transform.rotation = Quaternion.LookRotation(vector, base.transform.up);
			}
		}
	}

	// Token: 0x060031CD RID: 12749 RVA: 0x000E96D1 File Offset: 0x000E7AD1
	public virtual void Awake()
	{
		this.m_rigidBody = base.gameObject.RequireComponent<Rigidbody>();
		if (this.m_avatarControls == null)
		{
			this.m_avatarControls = base.GetComponent<MapAvatarControls>();
		}
	}

	// Token: 0x060031CE RID: 12750 RVA: 0x000E9704 File Offset: 0x000E7B04
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_groundCast = base.gameObject.RequireComponent<MapAvatarGroundCast>();
		this.MoveToStartingLocation();
		this.InitControls();
		this.m_avatarControls = base.gameObject.RequireComponent<MapAvatarControls>();
		this.m_saveManager = GameUtils.RequireManager<SaveManager>();
		this.RotateTowardsNextLevel();
		Mailbox.Server.RegisterForMessageType(MessageType.MapAvatarHorn, new OrderedMessageReceivedCallback(this.OnMapAvatarHornMessage));
		UserSystemUtils.OnServerChangedGameState = (GenericVoid<GameState, GameStateMessage.GameStatePayload>)Delegate.Combine(UserSystemUtils.OnServerChangedGameState, new GenericVoid<GameState, GameStateMessage.GameStatePayload>(this.OnServerChangedGameState));
		base.StartSynchronising(synchronisedObject);
		if ((ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession()) && DebugManager.Instance != null && DebugManager.Instance.GetOption("Auto Load Levels"))
		{
			base.StartCoroutine(this.m_avatarControls.DebugAutoLoadRandomLevel());
		}
	}

	// Token: 0x060031CF RID: 12751 RVA: 0x000E97D9 File Offset: 0x000E7BD9
	public override void StopSynchronising()
	{
		UserSystemUtils.OnServerChangedGameState = (GenericVoid<GameState, GameStateMessage.GameStatePayload>)Delegate.Remove(UserSystemUtils.OnServerChangedGameState, new GenericVoid<GameState, GameStateMessage.GameStatePayload>(this.OnServerChangedGameState));
	}

	// Token: 0x060031D0 RID: 12752 RVA: 0x000E97FB File Offset: 0x000E7BFB
	private void OnServerChangedGameState(GameState state, GameStateMessage.GameStatePayload payload)
	{
		if (state == GameState.InMap)
		{
			this.m_initialised = true;
		}
	}

	// Token: 0x060031D1 RID: 12753 RVA: 0x000E980C File Offset: 0x000E7C0C
	public override void OnDestroy()
	{
		Mailbox.Server.UnregisterForMessageType(MessageType.MapAvatarHorn, new OrderedMessageReceivedCallback(this.OnMapAvatarHornMessage));
		base.OnDestroy();
	}

	// Token: 0x060031D2 RID: 12754 RVA: 0x000E982C File Offset: 0x000E7C2C
	private void OnMapAvatarHornMessage(IOnlineMultiplayerSessionUserId userID, Serialisable message)
	{
		MapAvatarHornMessage mapAvatarHornMessage = (MapAvatarHornMessage)message;
		if (mapAvatarHornMessage.m_playerIdx < this.m_ServerData.m_bHorns.Length)
		{
			this.m_ServerData.m_bHorns[mapAvatarHornMessage.m_playerIdx] = true;
		}
		this.SendServerEvent(this.m_ServerData);
		for (int i = 0; i < this.m_ServerData.m_bHorns.Length; i++)
		{
			this.m_ServerData.m_bHorns[i] = false;
		}
	}

	// Token: 0x060031D3 RID: 12755 RVA: 0x000E98A3 File Offset: 0x000E7CA3
	protected override void OnEnable()
	{
		base.OnEnable();
		if (this.m_dashButton != null)
		{
			this.m_dashButton.ClaimPressEvent();
		}
		if (this.m_selectButton != null)
		{
			this.m_selectButton.ClaimPressEvent();
		}
	}

	// Token: 0x060031D4 RID: 12756 RVA: 0x000E98D7 File Offset: 0x000E7CD7
	protected override void OnDisable()
	{
		base.OnDisable();
		this.m_rigidBody.velocity = Vector3.zero;
	}

	// Token: 0x060031D5 RID: 12757 RVA: 0x000E98F0 File Offset: 0x000E7CF0
	private void Update()
	{
		if (this.m_initialised && this.m_avatarControls != null)
		{
			bool enabled = this.m_avatarControls.enabled;
			bool flag = TimeManager.IsPaused(base.gameObject) || TimeManager.IsPaused(TimeManager.PauseLayer.Network) || T17DialogBoxManager.HasAnyOpenDialogs();
			if (!enabled || flag)
			{
				this.StopMovingAvatar();
				return;
			}
			float deltaTime = TimeManager.GetDeltaTime(base.gameObject);
			this.Update_Movement(deltaTime);
			this.Update_Selection(deltaTime);
		}
	}

	// Token: 0x060031D6 RID: 12758 RVA: 0x000E997B File Offset: 0x000E7D7B
	private void FixedUpdate()
	{
		if (this.m_initialised && this.m_avatarControls != null && this.m_avatarControls.enabled)
		{
			this.m_avatarControls.UpdateMovement();
		}
	}

	// Token: 0x060031D7 RID: 12759 RVA: 0x000E99B4 File Offset: 0x000E7DB4
	private void StopMovingAvatar()
	{
		this.m_dashButton.ClaimPressEvent();
		this.m_selectButton.ClaimPressEvent();
		if (!this.m_rigidBody.IsSleeping())
		{
			this.m_rigidBody.velocity = new Vector3(0f, 0f, 0f);
		}
	}

	// Token: 0x060031D8 RID: 12760 RVA: 0x000E9A08 File Offset: 0x000E7E08
	private float CalculateSpeed()
	{
		float num = 0f;
		if (this.m_moveX.GetValue() != 0f || this.m_moveY.GetValue() != 0f)
		{
			num = this.m_avatarControls.m_movementSpeed;
		}
		if (this.m_dashTimer > 0f)
		{
			float num2 = MathUtils.SinusoidalSCurve(this.m_dashTimer / this.m_avatarControls.m_dashTime);
			num = (1f - num2) * num + num2 * this.m_avatarControls.m_dashSpeed;
		}
		this.m_dashTimer -= TimeManager.GetDeltaTime(base.gameObject);
		return num;
	}

	// Token: 0x060031D9 RID: 12761 RVA: 0x000E9AAC File Offset: 0x000E7EAC
	private void Update_Movement(float _deltaTime)
	{
		if (this.m_dashButton != null && this.m_dashButton.JustPressed() && this.m_avatarControls != null && this.m_avatarControls.m_dashTime - this.m_dashTimer >= this.m_avatarControls.m_dashCooldown)
		{
			this.m_dashTimer = this.m_avatarControls.m_dashTime;
			this.m_ServerData.m_bDash = true;
			this.SendServerEvent(this.m_ServerData);
			this.m_ServerData.m_bDash = false;
		}
		float num = this.CalculateSpeed();
		PlayerControlsHelper.ControlAxisData controlAxisData = default(PlayerControlsHelper.ControlAxisData);
		controlAxisData.XAxisAllignment = PlayerControls.InversionType.Normal;
		controlAxisData.YAxisAllignment = PlayerControls.InversionType.Normal;
		controlAxisData.MoveX = this.m_moveX;
		controlAxisData.MoveY = this.m_moveY;
		controlAxisData.TurnSpeed = num / this.m_avatarControls.m_turningCircle;
		controlAxisData.QuantiseDirection = false;
		PlayerControlsHelper.TurnTowardsControlAxis(ref controlAxisData, base.gameObject, _deltaTime);
		Vector3 forward = base.gameObject.transform.forward;
		Vector3 vector = num * forward;
		Vector3 groundNormal = this.m_groundCast.GetGroundNormal();
		Vector3 normalized = Vector3.Cross(groundNormal, Vector3.forward).normalized;
		Vector3 normalized2 = Vector3.Cross(normalized, groundNormal).normalized;
		Vector3 vector2 = vector;
		Vector3 velocity = normalized * vector2.x + groundNormal * vector2.y + normalized2 * vector2.z;
		this.ApplyGravity(ref velocity, this.m_rigidBody.velocity);
		this.m_rigidBody.velocity = velocity;
	}

	// Token: 0x060031DA RID: 12762 RVA: 0x000E9C4C File Offset: 0x000E804C
	private float ApplyGravity(ref Vector3 _gameplayVelocity, Vector3 _physicalVelocity)
	{
		Vector3 vector = -this.m_groundCast.GetGroundNormal();
		float num = Vector3.Project(_physicalVelocity, vector).magnitude;
		if (!this.m_groundCast.HasGroundContact())
		{
			num += this.m_avatarControls.m_gravityStrength * TimeManager.GetDeltaTime(base.gameObject);
		}
		_gameplayVelocity += num * vector;
		return num;
	}

	// Token: 0x060031DB RID: 12763 RVA: 0x000E9CC0 File Offset: 0x000E80C0
	private void Update_Selection(float _deltaTime)
	{
		IServerMapSelectable serverMapSelectable = null;
		bool flag = this.m_avatarControls.CalculateCurrentSelectable<IServerMapSelectable>(this.m_avatarControls.GridManager, this.m_groundCast, out serverMapSelectable);
		if (flag && this.m_selectButton.JustPressed() && serverMapSelectable != null && !Application.isLoadingLevel && !this.m_saveManager.IsSaving)
		{
			serverMapSelectable.OnSelected(this.m_avatarControls);
		}
		if (flag && this.m_currentSelectable != serverMapSelectable)
		{
			if (this.m_currentSelectable != null)
			{
				this.m_currentSelectable.AvatarLeavingSelectable(this.m_avatarControls);
			}
			if (serverMapSelectable != null)
			{
				serverMapSelectable.AvatarEnteringSelectable(this.m_avatarControls);
			}
			this.m_currentSelectable = serverMapSelectable;
			ServerSynchroniserBase serverSynchroniserBase = serverMapSelectable as ServerSynchroniserBase;
			if (null != serverSynchroniserBase)
			{
				this.m_currentSelectableEntityID = serverSynchroniserBase.GetEntityId();
			}
			else
			{
				this.m_currentSelectableEntityID = 0U;
			}
			this.m_ServerData.CurrentSelectableEntityId = this.m_currentSelectableEntityID;
			this.SendServerEvent(this.m_ServerData);
		}
	}

	// Token: 0x04002805 RID: 10245
	private MapAvatarControls m_avatarControls;

	// Token: 0x04002806 RID: 10246
	private Quaternion m_originalVanRotation;

	// Token: 0x04002807 RID: 10247
	private ILogicalValue m_moveX;

	// Token: 0x04002808 RID: 10248
	private ILogicalValue m_moveY;

	// Token: 0x04002809 RID: 10249
	private ILogicalButton m_selectButton;

	// Token: 0x0400280A RID: 10250
	private ILogicalButton m_dashButton;

	// Token: 0x0400280B RID: 10251
	private float m_dashTimer = float.MinValue;

	// Token: 0x0400280C RID: 10252
	private MapAvatarControlsMessage m_ServerData = new MapAvatarControlsMessage();

	// Token: 0x0400280D RID: 10253
	private bool m_initialised;

	// Token: 0x0400280E RID: 10254
	private MapAvatarGroundCast m_groundCast;

	// Token: 0x0400280F RID: 10255
	private Rigidbody m_rigidBody;

	// Token: 0x04002810 RID: 10256
	private SaveManager m_saveManager;

	// Token: 0x04002811 RID: 10257
	private IServerMapSelectable m_currentSelectable;

	// Token: 0x04002812 RID: 10258
	private uint m_currentSelectableEntityID;
}
