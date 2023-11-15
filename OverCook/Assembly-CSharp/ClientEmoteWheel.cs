using System;
using System.Collections;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000A8A RID: 2698
public class ClientEmoteWheel : ClientSynchroniserBase
{
	// Token: 0x06003555 RID: 13653 RVA: 0x000F8880 File Offset: 0x000F6C80
	protected virtual void Awake()
	{
		this.m_emoteWheel = base.gameObject.RequireComponent<EmoteWheel>();
		Mailbox.Client.RegisterForMessageType(MessageType.EmoteWheel, new OrderedMessageReceivedCallback(this.ProcessServerMessage));
		this.m_iPlayerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		this.RefreshInputVars(false);
		PlayerInputLookup.OnRegenerateControls = (CallbackVoid)Delegate.Combine(PlayerInputLookup.OnRegenerateControls, new CallbackVoid(this.OnRegenerateControls));
	}

	// Token: 0x06003556 RID: 13654 RVA: 0x000F88E8 File Offset: 0x000F6CE8
	protected virtual void Start()
	{
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Combine(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChanged));
		this.m_iPlayerManager.EngagementChangeCallback += this.OnEngagementChanged;
		this.m_emoteSelector = new EmoteSelector(this.m_emoteWheel, base.transform);
		this.m_emoteSelector.Hide();
		if (this.m_emoteWheel.m_animationTarget == null)
		{
			this.m_emoteWheel.m_animationTarget = base.gameObject.RequestComponentInImmediateChildren<Animator>();
		}
	}

	// Token: 0x06003557 RID: 13655 RVA: 0x000F897C File Offset: 0x000F6D7C
	private PlayerInputLookup.Player NetworkToLocalPlayerNumber(PlayerInputLookup.Player _player)
	{
		int num = 0;
		int i = 0;
		while (i < ClientUserSystem.m_Users.Count)
		{
			if (i == (int)_player)
			{
				if (!ClientUserSystem.m_Users._items[i].IsLocal)
				{
					break;
				}
				return (PlayerInputLookup.Player)num;
			}
			else
			{
				if (ClientUserSystem.m_Users._items[i].IsLocal)
				{
					num++;
				}
				i++;
			}
		}
		return PlayerInputLookup.Player.Count;
	}

	// Token: 0x06003558 RID: 13656 RVA: 0x000F89E8 File Offset: 0x000F6DE8
	protected void ProcessServerMessage(IOnlineMultiplayerSessionUserId _sessionId, Serialisable _serialisable)
	{
		EmoteWheelMessage emoteWheelMessage = _serialisable as EmoteWheelMessage;
		if (emoteWheelMessage.m_player == this.m_emoteWheel.m_player && emoteWheelMessage.m_player != PlayerInputLookup.Player.Count && !this.m_emoteWheel.IsLocal && emoteWheelMessage.m_forUI == this.m_emoteWheel.ForUI)
		{
			this.StartEmote(emoteWheelMessage.m_emoteIdx);
		}
	}

	// Token: 0x06003559 RID: 13657 RVA: 0x000F8A51 File Offset: 0x000F6E51
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		if (!this.m_emoteWheel.ForUI)
		{
			this.UpdateEmoteWheel();
		}
	}

	// Token: 0x0600355A RID: 13658 RVA: 0x000F8A6F File Offset: 0x000F6E6F
	private void Update()
	{
		if (this.m_emoteWheel.ForUI)
		{
			this.UpdateEmoteWheel();
		}
	}

	// Token: 0x0600355B RID: 13659 RVA: 0x000F8A88 File Offset: 0x000F6E88
	private void UpdateEmoteWheel()
	{
		if (this.m_emoteRoutine != null)
		{
			this.m_emoteRoutine.MoveNext();
		}
		bool flag = this.m_emoteWheel.CanShow();
		if (!flag && this.m_emoteSelector.IsActive())
		{
			this.m_emoteSelector.Hide();
			if (this.m_enableInputRoutine != null)
			{
				base.StopCoroutine(this.m_enableInputRoutine);
				this.m_enableInputRoutine = null;
			}
			this.ForceReEnableInput();
		}
		if (flag && this.m_wheelButton != null)
		{
			if (this.m_emoteWheel.ForUI && !ConnectionStatus.IsInSession() && UserSystemUtils.AnySplitPadUsers())
			{
				return;
			}
			if (!this.m_emoteSelector.IsActive())
			{
				if (this.m_wheelButton.JustPressed())
				{
					this.OpenWheel();
				}
			}
			else if (this.m_wheelButton.IsDown())
			{
				PlayerInputLookup.Player player;
				if (this.m_emoteWheel.m_playerIDProvider != null)
				{
					player = this.m_emoteWheel.m_playerIDProvider.GetID();
				}
				else
				{
					player = this.NetworkToLocalPlayerNumber(this.m_emoteWheel.m_player);
				}
				if (PCPadInputProvider.IsKeyboard(PlayerInputLookup.GetPadForPlayer(player)))
				{
					this.ProcessDirectionalInput();
				}
				else
				{
					float x = this.m_xMovement.GetValue() * this.m_emoteWheel.m_emoteWheelOptions.m_radius;
					float y = -this.m_yMovement.GetValue() * this.m_emoteWheel.m_emoteWheelOptions.m_radius;
					this.m_emoteSelector.AnalogUpdate(x, y);
				}
				if (Input.mousePresent)
				{
					if (Input.GetMouseButtonDown(0))
					{
						this.CloseWheel();
					}
					else
					{
						this.m_emoteSelector.PointerUpdate();
					}
				}
			}
			else
			{
				this.CloseWheel();
			}
		}
	}

	// Token: 0x0600355C RID: 13660 RVA: 0x000F8C48 File Offset: 0x000F7048
	private IEnumerator EmoteRoutine(int _emoteIdx)
	{
		EmoteWheelOption emote = this.m_emoteWheel.m_emoteWheelOptions.m_options[_emoteIdx];
		if ((emote.m_type == EmoteWheelOption.EmoteType.Animation || emote.m_type == EmoteWheelOption.EmoteType.Both) && !emote.m_triggerForCode && this.m_emoteWheel.m_animationTarget != null)
		{
			this.m_emoteWheel.m_animationTarget.SetTrigger(emote.m_animTriggerHash);
		}
		if (emote.m_type == EmoteWheelOption.EmoteType.Dialog || emote.m_type == EmoteWheelOption.EmoteType.Both)
		{
			if (this.m_emoteWheel.ForUI)
			{
				if (emote.m_dialogPrefab != null)
				{
					this.m_emoteDialog = UnityEngine.Object.Instantiate<GameObject>(emote.m_dialogPrefab);
					this.m_emoteDialog.transform.SetParent(this.m_emoteWheel.m_uiPlayer.DialogAnchor, false);
					RectTransform rectTransform = this.m_emoteDialog.transform as RectTransform;
					rectTransform.anchoredPosition = new Vector2(emote.m_anchorOffset.x, emote.m_anchorOffset.y);
				}
			}
			else if (emote.m_dialogPrefab != null)
			{
				this.m_emoteDialog = GameUtils.InstantiateUIController(emote.m_dialogPrefab, "HoverIconCanvas");
				HoverIconUIController hoverIconUIController = this.m_emoteDialog.RequestComponent<HoverIconUIController>();
				if (hoverIconUIController == null)
				{
					hoverIconUIController = this.m_emoteDialog.AddComponent<HoverIconUIController>();
				}
				if (hoverIconUIController != null)
				{
					hoverIconUIController.SetFollowTransform(base.transform, emote.m_anchorOffset);
					hoverIconUIController.ShouldFollow = true;
				}
			}
			IEnumerator delay = CoroutineUtils.TimerRoutine(this.m_emoteWheel.m_emoteWheelOptions.m_options[_emoteIdx].m_duration, base.gameObject.layer);
			while (delay.MoveNext())
			{
				if (this.m_emoteWheel.m_playerRespawn != null && this.m_emoteWheel.m_playerRespawn.IsRespawning)
				{
					break;
				}
				yield return null;
			}
			this.CleanUpEmote();
		}
		yield break;
	}

	// Token: 0x0600355D RID: 13661 RVA: 0x000F8C6C File Offset: 0x000F706C
	private void OpenWheel()
	{
		if (this.m_enableInputRoutine != null)
		{
			base.StopCoroutine(this.m_enableInputRoutine);
			this.m_enableInputRoutine = null;
		}
		if (this.m_emoteWheel.ForUI)
		{
			this.DisableEventSystem();
		}
		else
		{
			IPlayerControlsImpl activeControlsImpl = this.m_emoteWheel.m_playerControls.GetActiveControlsImpl();
			if (activeControlsImpl == null)
			{
				return;
			}
			if (this.m_suppressor == null)
			{
				this.m_suppressor = this.m_emoteWheel.m_playerControls.Suppress(this);
			}
		}
		this.m_emoteSelector.Show();
	}

	// Token: 0x0600355E RID: 13662 RVA: 0x000F8CFC File Offset: 0x000F70FC
	public void StartEmote(int _emoteIdx)
	{
		if (this.m_emoteRoutine != null)
		{
			this.CleanUpEmote();
		}
		this.m_emoteRoutine = this.EmoteRoutine(_emoteIdx);
	}

	// Token: 0x0600355F RID: 13663 RVA: 0x000F8D1C File Offset: 0x000F711C
	public void RequestEmoteStart(int _emoteIdx)
	{
		this.m_message.InitialiseStartEmote(_emoteIdx, this.m_emoteWheel.m_player, this.m_emoteWheel.ForUI);
		ClientMessenger.EmoteWheelMessage(this.m_message);
		this.StartEmote(_emoteIdx);
	}

	// Token: 0x06003560 RID: 13664 RVA: 0x000F8D54 File Offset: 0x000F7154
	private void CloseWheel()
	{
		int selected = this.m_emoteSelector.GetSelected();
		if (selected != -1)
		{
			this.RequestEmoteStart(selected);
			string emoteId = this.m_emoteWheel.m_emoteWheelOptions.m_options[selected].m_emoteId;
			if (emoteId != null)
			{
				OvercookedAchievementManager overcookedAchievementManager = GameUtils.RequestManager<OvercookedAchievementManager>();
				if (overcookedAchievementManager != null)
				{
					PlayerInputLookup.Player player = (!this.m_emoteWheel.ForUI) ? this.m_emoteWheel.m_playerIDProvider.GetID() : this.m_emoteWheel.m_player;
					overcookedAchievementManager.AddIDStat(21, emoteId.GetHashCode(), PlayerInputLookup.GetPadForPlayer(player));
				}
			}
		}
		this.m_emoteSelector.Hide();
		if (this.m_enableInputRoutine == null)
		{
			this.m_enableInputRoutine = base.StartCoroutine(this.ReEnableInputRoutine(new Vector2(this.m_xMovement.GetValue(), this.m_yMovement.GetValue())));
		}
	}

	// Token: 0x06003561 RID: 13665 RVA: 0x000F8E34 File Offset: 0x000F7234
	private IEnumerator ReEnableInputRoutine(Vector2 _posOnClose)
	{
		while (this.m_xMovement != null && this.m_yMovement != null && (this.m_xMovement.GetValue() != 0f || this.m_yMovement.GetValue() != 0f))
		{
			if (!this.m_emoteWheel.ForUI && this.m_renableInputButton.IsDown())
			{
				break;
			}
			float num = this.m_emoteWheel.m_analogEnableThreshold * this.m_emoteWheel.m_analogEnableThreshold;
			Vector2 b = new Vector2(this.m_xMovement.GetValue(), this.m_yMovement.GetValue());
			if (Mathf.Abs((_posOnClose - b).sqrMagnitude) > num)
			{
				break;
			}
			yield return null;
		}
		this.ForceReEnableInput();
		yield break;
	}

	// Token: 0x06003562 RID: 13666 RVA: 0x000F8E58 File Offset: 0x000F7258
	private void ForceReEnableInput()
	{
		if (this.m_suppressor != null)
		{
			if (this.m_emoteWheel.ForUI)
			{
				this.m_suppressor.Release();
			}
			else
			{
				this.m_emoteWheel.m_playerControls.ReleaseSuppressor(this.m_suppressor);
			}
			this.m_suppressor = null;
		}
		this.m_enableInputRoutine = null;
	}

	// Token: 0x06003563 RID: 13667 RVA: 0x000F8EB4 File Offset: 0x000F72B4
	private void ProcessDirectionalInput()
	{
		EmoteWheelOption.Connection.Direction direction = EmoteWheelOption.Connection.Direction.COUNT;
		bool flag = this.m_rightButton.JustPressed();
		bool flag2 = this.m_downButton.JustPressed();
		bool flag3 = this.m_leftButton.JustPressed();
		bool flag4 = this.m_upButton.JustPressed();
		if (flag4)
		{
			if (flag3)
			{
				direction = EmoteWheelOption.Connection.Direction.UpLeft;
			}
			else if (flag)
			{
				direction = EmoteWheelOption.Connection.Direction.UpRight;
			}
			else
			{
				direction = EmoteWheelOption.Connection.Direction.Up;
			}
		}
		else if (flag2)
		{
			if (flag3)
			{
				direction = EmoteWheelOption.Connection.Direction.DownLeft;
			}
			else if (flag)
			{
				direction = EmoteWheelOption.Connection.Direction.DownRight;
			}
			else
			{
				direction = EmoteWheelOption.Connection.Direction.Down;
			}
		}
		else if (flag3)
		{
			direction = EmoteWheelOption.Connection.Direction.Left;
		}
		else if (flag)
		{
			direction = EmoteWheelOption.Connection.Direction.Right;
		}
		if (direction != EmoteWheelOption.Connection.Direction.COUNT)
		{
			this.m_emoteSelector.Update(direction);
		}
	}

	// Token: 0x06003564 RID: 13668 RVA: 0x000F8F6C File Offset: 0x000F736C
	private void DisableEventSystem()
	{
		if (this.m_suppressor != null)
		{
			return;
		}
		PlayerInputLookup.Player player = this.NetworkToLocalPlayerNumber(this.m_emoteWheel.m_player);
		if (player != PlayerInputLookup.Player.Count)
		{
			GamepadUser user = this.m_iPlayerManager.GetUser((EngagementSlot)player);
			if (user != null)
			{
				T17EventSystem eventSystemForGamepadUser = T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(user);
				if (eventSystemForGamepadUser != null)
				{
					this.m_suppressor = eventSystemForGamepadUser.Disable(this);
				}
			}
		}
	}

	// Token: 0x06003565 RID: 13669 RVA: 0x000F8FDD File Offset: 0x000F73DD
	protected void CleanUpEmote()
	{
		if (this.m_emoteDialog != null)
		{
			UnityEngine.Object.Destroy(this.m_emoteDialog);
		}
		this.m_emoteRoutine = null;
	}

	// Token: 0x06003566 RID: 13670 RVA: 0x000F9002 File Offset: 0x000F7402
	protected void OnUsersChanged()
	{
		this.RefreshInputVars(false);
		if (this.m_emoteWheel.m_animationTarget == null)
		{
			this.m_emoteWheel.m_animationTarget = base.gameObject.RequestComponentInImmediateChildren<Animator>();
		}
	}

	// Token: 0x06003567 RID: 13671 RVA: 0x000F9037 File Offset: 0x000F7437
	private void OnRegenerateControls()
	{
		this.RefreshInputVars(true);
	}

	// Token: 0x06003568 RID: 13672 RVA: 0x000F9040 File Offset: 0x000F7440
	private void RefreshInputVars(bool _force = false)
	{
		PlayerInputLookup.Player player = (!this.m_emoteWheel.ForUI) ? this.m_emoteWheel.m_playerIDProvider.GetID() : this.NetworkToLocalPlayerNumber(this.m_emoteWheel.m_player);
		User user = null;
		int num = 0;
		for (int i = 0; i < ClientUserSystem.m_Users.Count; i++)
		{
			User user2 = ClientUserSystem.m_Users._items[i];
			if (user2.IsLocal)
			{
				if (num == (int)player)
				{
					user = user2;
					break;
				}
				num++;
			}
		}
		GamepadUser gamepadUser = (user == null) ? null : user.GamepadUser;
		if (_force || (gamepadUser != null && (this.m_gamepadOnLastInputRefresh != gamepadUser || this.m_playerIDOnLastInputRefresh != player)))
		{
			if (player != PlayerInputLookup.Player.Count)
			{
				this.m_wheelButton = PlayerInputLookup.GetButton(PlayerInputLookup.LogicalButtonID.Curse, player);
				this.m_xMovement = PlayerInputLookup.GetValue(PlayerInputLookup.LogicalValueID.MovementX, player);
				this.m_yMovement = PlayerInputLookup.GetValue(PlayerInputLookup.LogicalValueID.MovementY, player);
				this.m_rightButton = PlayerInputLookup.GetButton(PlayerInputLookup.LogicalButtonID.UIRight, player);
				this.m_downButton = PlayerInputLookup.GetButton(PlayerInputLookup.LogicalButtonID.UIDown, player);
				this.m_leftButton = PlayerInputLookup.GetButton(PlayerInputLookup.LogicalButtonID.UILeft, player);
				this.m_upButton = PlayerInputLookup.GetButton(PlayerInputLookup.LogicalButtonID.UIUp, player);
				this.m_renableInputButton = new ComboLogicalButton(new ILogicalButton[]
				{
					PlayerInputLookup.GetButton(PlayerInputLookup.LogicalButtonID.Dash, player),
					PlayerInputLookup.GetButton(PlayerInputLookup.LogicalButtonID.PickupAndDrop, player),
					PlayerInputLookup.GetButton(PlayerInputLookup.LogicalButtonID.WorkstationInteract, player)
				}, ComboLogicalButton.ComboType.Or);
			}
			else
			{
				this.m_wheelButton = null;
				this.m_xMovement = null;
				this.m_yMovement = null;
				this.m_rightButton = null;
				this.m_downButton = null;
				this.m_leftButton = null;
				this.m_upButton = null;
				this.m_renableInputButton = null;
			}
			this.m_gamepadOnLastInputRefresh = gamepadUser;
			this.m_playerIDOnLastInputRefresh = player;
		}
	}

	// Token: 0x06003569 RID: 13673 RVA: 0x000F91F6 File Offset: 0x000F75F6
	private void OnEngagementChanged(EngagementSlot _slot, GamepadUser _old, GamepadUser _new)
	{
		this.RefreshInputVars(false);
	}

	// Token: 0x0600356A RID: 13674 RVA: 0x000F9200 File Offset: 0x000F7600
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Mailbox.Client.UnregisterForMessageType(MessageType.EmoteWheel, new OrderedMessageReceivedCallback(this.ProcessServerMessage));
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Remove(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChanged));
		this.m_iPlayerManager.EngagementChangeCallback -= this.OnEngagementChanged;
		if (this.m_suppressor != null)
		{
			this.m_suppressor.Release();
			this.m_suppressor = null;
		}
		if (this.m_emoteSelector != null)
		{
			this.m_emoteSelector.Destroy();
		}
		PlayerInputLookup.OnRegenerateControls = (CallbackVoid)Delegate.Remove(PlayerInputLookup.OnRegenerateControls, new CallbackVoid(this.OnRegenerateControls));
	}

	// Token: 0x04002AC2 RID: 10946
	private EmoteWheel m_emoteWheel;

	// Token: 0x04002AC3 RID: 10947
	private EmoteWheelMessage m_message = new EmoteWheelMessage();

	// Token: 0x04002AC4 RID: 10948
	private IEnumerator m_emoteRoutine;

	// Token: 0x04002AC5 RID: 10949
	private GameObject m_emoteDialog;

	// Token: 0x04002AC6 RID: 10950
	private Suppressor m_suppressor;

	// Token: 0x04002AC7 RID: 10951
	private IPlayerManager m_iPlayerManager;

	// Token: 0x04002AC8 RID: 10952
	private GamepadUser m_gamepadOnLastInputRefresh;

	// Token: 0x04002AC9 RID: 10953
	private PlayerInputLookup.Player m_playerIDOnLastInputRefresh = PlayerInputLookup.Player.Count;

	// Token: 0x04002ACA RID: 10954
	private ILogicalButton m_wheelButton;

	// Token: 0x04002ACB RID: 10955
	private ILogicalButton m_rightButton;

	// Token: 0x04002ACC RID: 10956
	private ILogicalButton m_downButton;

	// Token: 0x04002ACD RID: 10957
	private ILogicalButton m_leftButton;

	// Token: 0x04002ACE RID: 10958
	private ILogicalButton m_upButton;

	// Token: 0x04002ACF RID: 10959
	private ILogicalButton m_renableInputButton;

	// Token: 0x04002AD0 RID: 10960
	private ILogicalValue m_xMovement;

	// Token: 0x04002AD1 RID: 10961
	private ILogicalValue m_yMovement;

	// Token: 0x04002AD2 RID: 10962
	private EmoteSelector m_emoteSelector;

	// Token: 0x04002AD3 RID: 10963
	private Coroutine m_enableInputRoutine;
}
