using System;
using System.Collections.Generic;
using GameModes;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000BB9 RID: 3001
public abstract class ClientPortalMapNode : ClientSynchroniserBase, IClientMapSelectable
{
	// Token: 0x06003D62 RID: 15714 RVA: 0x001245DD File Offset: 0x001229DD
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_basePortalMapNode = (PortalMapNode)synchronisedObject;
		this.m_basePortalMapNode.RegisterPostFlipCallback(new VoidGeneric<FlipDirection, FlipType>(this.OnFlipFinished));
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06003D63 RID: 15715 RVA: 0x0012461A File Offset: 0x00122A1A
	public override EntityType GetEntityType()
	{
		return EntityType.PortalMapNode;
	}

	// Token: 0x06003D64 RID: 15716 RVA: 0x00124620 File Offset: 0x00122A20
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.InMap)
		{
			this.EnsureIconAllocated();
			this.m_UIState = ClientPortalMapNode.UIState.RequestedUpdate;
			NetworkUtils.OnGameProgressLoadedFromNetwork = (GenericVoid)Delegate.Combine(NetworkUtils.OnGameProgressLoadedFromNetwork, new GenericVoid(this.OnGameProgressLoadedFromNetwork));
		}
	}

	// Token: 0x06003D65 RID: 15717 RVA: 0x0012466E File Offset: 0x00122A6E
	public void OnGameProgressLoadedFromNetwork()
	{
		this.m_UIState = ClientPortalMapNode.UIState.RequestedUpdate;
	}

	// Token: 0x06003D66 RID: 15718 RVA: 0x00124678 File Offset: 0x00122A78
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_basePortalMapNode != null)
		{
			this.m_basePortalMapNode.UnregisterPostFlipCallback(new VoidGeneric<FlipDirection, FlipType>(this.OnFlipFinished));
		}
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		NetworkUtils.OnGameProgressLoadedFromNetwork = (GenericVoid)Delegate.Remove(NetworkUtils.OnGameProgressLoadedFromNetwork, new GenericVoid(this.OnGameProgressLoadedFromNetwork));
		this.RemoveIcon();
		if (this.m_uiInstance.m_uiInstance != null)
		{
			UnityEngine.Object.Destroy(this.m_uiInstance.m_uiInstance.gameObject);
			this.m_uiInstance.Reset();
		}
	}

	// Token: 0x06003D67 RID: 15719 RVA: 0x00124728 File Offset: 0x00122B28
	protected override void OnEnable()
	{
		base.OnEnable();
		if (this.m_uiInstance.m_uiInstance != null)
		{
			this.m_uiInstance.m_uiInstance.gameObject.SetActive(true);
		}
		GameSession gameSession = GameUtils.GetGameSession();
		if (gameSession != null)
		{
			GameSession gameSession2 = gameSession;
			gameSession2.OnGameModeSessionConfigChanged = (OnSessionConfigChanged)Delegate.Combine(gameSession2.OnGameModeSessionConfigChanged, new OnSessionConfigChanged(this.OnGameModeSessionConfigChanged));
		}
	}

	// Token: 0x06003D68 RID: 15720 RVA: 0x0012479C File Offset: 0x00122B9C
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_uiInstance.m_uiInstance != null)
		{
			this.m_uiInstance.m_uiInstance.gameObject.SetActive(false);
		}
		GameSession gameSession = GameUtils.GetGameSession();
		if (gameSession != null)
		{
			GameSession gameSession2 = gameSession;
			gameSession2.OnGameModeSessionConfigChanged = (OnSessionConfigChanged)Delegate.Remove(gameSession2.OnGameModeSessionConfigChanged, new OnSessionConfigChanged(this.OnGameModeSessionConfigChanged));
		}
	}

	// Token: 0x06003D69 RID: 15721 RVA: 0x00124810 File Offset: 0x00122C10
	private void OnGameModeSessionConfigChanged(SessionConfig _config)
	{
		WorldMapLevelIconUI uiprefab = this.m_basePortalMapNode.GetUIPrefab(_config.m_kind);
		if (this.m_uiInstance.m_prefab != uiprefab || this.m_inSelectable)
		{
			this.RecreateIcon();
		}
	}

	// Token: 0x06003D6A RID: 15722 RVA: 0x00124858 File Offset: 0x00122C58
	public override void UpdateSynchronising()
	{
		switch (this.m_UIState)
		{
		case ClientPortalMapNode.UIState.RequestedUpdate:
			if (null != this.m_basePortalMapNode && this.m_basePortalMapNode.Unfolded && null != this.m_uiInstance.m_uiInstance)
			{
				if (this.m_basePortalMapNode.m_uiAlwaysActive)
				{
					this.m_UIState = ClientPortalMapNode.UIState.Enabling;
				}
				else
				{
					bool flag = this.CalculateOnScreen();
					this.m_uiInstance.m_uiInstance.gameObject.SetActive(flag);
					if (flag)
					{
						this.m_UIState = ClientPortalMapNode.UIState.Enabling;
					}
					else
					{
						this.m_UIState = ClientPortalMapNode.UIState.Disabled;
					}
				}
			}
			break;
		case ClientPortalMapNode.UIState.Enabling:
			if (null != this.m_uiInstance.m_uiInstance && this.m_uiInstance.m_uiInstance.IsReady())
			{
				this.SetupUI(this.m_uiInstance.m_uiInstance);
				this.m_uiInstance.m_uiInstance.SetAvatarProximity(this.InSelectable);
				this.m_UIState = ClientPortalMapNode.UIState.Enabled;
			}
			break;
		}
	}

	// Token: 0x17000428 RID: 1064
	// (get) Token: 0x06003D6B RID: 15723 RVA: 0x0012497C File Offset: 0x00122D7C
	protected bool InSelectable
	{
		get
		{
			return this.m_inSelectable;
		}
	}

	// Token: 0x06003D6C RID: 15724 RVA: 0x00124984 File Offset: 0x00122D84
	private void OnFlipFinished(FlipDirection _direction, FlipType _flipType)
	{
		this.EnsureIconAllocated();
	}

	// Token: 0x06003D6D RID: 15725 RVA: 0x0012498C File Offset: 0x00122D8C
	private void EnsureIconAllocated()
	{
		if (null != this.m_basePortalMapNode)
		{
			if (this.m_basePortalMapNode.Unfolded || this.m_basePortalMapNode.Unfolding)
			{
				if (this.m_uiInstance.m_uiInstance == null)
				{
					if (ClientPortalMapNode.s_allIcons == null)
					{
						ClientPortalMapNode.s_allIcons = new List<HoverIconUIController>();
					}
					Transform transform = base.transform;
					GameSession gameSession = GameUtils.GetGameSession();
					WorldMapLevelIconUI uiprefab = this.m_basePortalMapNode.GetUIPrefab(gameSession.GameModeKind);
					this.m_uiInstance.m_prefab = uiprefab;
					GameObject obj = GameUtils.InstantiateHoverIconUIController(uiprefab.gameObject, transform, "HoverIconCanvas", default(Vector3));
					this.m_uiInstance.m_uiInstance = obj.RequireComponent<WorldMapLevelIconUI>();
					if (ClientPortalMapNode.s_allIcons.Count == 0)
					{
						ClientPortalMapNode.s_allIcons.Add(this.m_uiInstance.m_uiInstance);
					}
					else
					{
						int num = this.FindClosestIcon(this.m_uiInstance.m_uiInstance);
						int num2 = 0;
						HoverIconUIController hoverIconUIController = ClientPortalMapNode.s_allIcons[num];
						if (hoverIconUIController.GetFollowTransform().position.z >= transform.position.z)
						{
							num2 = 1;
						}
						ClientPortalMapNode.s_allIcons.Insert(num + num2, this.m_uiInstance.m_uiInstance);
						this.m_uiInstance.m_uiInstance.transform.SetSiblingIndex(hoverIconUIController.transform.GetSiblingIndex() + num2);
					}
				}
			}
			else
			{
				this.RemoveIcon();
				if (this.m_uiInstance.m_uiInstance != null)
				{
					UnityEngine.Object.Destroy(this.m_uiInstance.m_uiInstance.gameObject);
					this.m_uiInstance.Reset();
				}
			}
		}
	}

	// Token: 0x06003D6E RID: 15726 RVA: 0x00124B44 File Offset: 0x00122F44
	private int FindClosestIcon(HoverIconUIController _icon)
	{
		int i = 0;
		int num = ClientPortalMapNode.s_allIcons.Count - 1;
		int num2 = 0;
		float z = _icon.GetFollowTransform().position.z;
		while (i <= num)
		{
			num2 = i + (num - i) / 2;
			float z2 = ClientPortalMapNode.s_allIcons[num2].GetFollowTransform().position.z;
			if (z2 > z)
			{
				i = num2 + 1;
			}
			else
			{
				num = num2 - 1;
			}
			if (Mathf.Approximately(z2, z))
			{
				return num2;
			}
		}
		return num2;
	}

	// Token: 0x06003D6F RID: 15727 RVA: 0x00124BD0 File Offset: 0x00122FD0
	private bool CalculateOnScreen()
	{
		Vector2 vector = Camera.main.WorldToViewportPoint(base.transform.position);
		return vector.x > -0.05f && vector.x < 1.05f && vector.y > -0.1f && vector.y < 1f;
	}

	// Token: 0x06003D70 RID: 15728 RVA: 0x00124C3C File Offset: 0x0012303C
	private void RemoveIcon()
	{
		if (ClientPortalMapNode.s_allIcons != null)
		{
			if (this.m_uiInstance.m_uiInstance != null)
			{
				ClientPortalMapNode.s_allIcons.Remove(this.m_uiInstance.m_uiInstance);
			}
			else
			{
				for (int i = ClientPortalMapNode.s_allIcons.Count - 1; i >= 0; i--)
				{
					if (ClientPortalMapNode.s_allIcons[i] == null)
					{
						ClientPortalMapNode.s_allIcons.RemoveAt(i);
					}
				}
			}
			if (ClientPortalMapNode.s_allIcons.Count == 0)
			{
				ClientPortalMapNode.s_allIcons = null;
			}
		}
	}

	// Token: 0x06003D71 RID: 15729 RVA: 0x00124CD8 File Offset: 0x001230D8
	private void RecreateIcon()
	{
		this.RemoveIcon();
		if (this.m_uiInstance.m_uiInstance != null)
		{
			UnityEngine.Object.Destroy(this.m_uiInstance.m_uiInstance.gameObject);
			this.m_uiInstance.Reset();
		}
		this.EnsureIconAllocated();
		this.m_UIState = ClientPortalMapNode.UIState.RequestedUpdate;
	}

	// Token: 0x06003D72 RID: 15730
	protected abstract void SetupUI(WorldMapLevelIconUI _ui);

	// Token: 0x06003D73 RID: 15731 RVA: 0x00124D30 File Offset: 0x00123130
	public void AvatarEnteringSelectable(MapAvatarControls _avatar)
	{
		this.m_inSelectable = true;
		this.m_UIState = ClientPortalMapNode.UIState.RequestedUpdate;
		GameSession gameSession = GameUtils.GetGameSession();
		if (gameSession != null && this.m_uiInstance.m_prefab != this.m_basePortalMapNode.GetUIPrefab(gameSession.GameModeKind))
		{
			this.RecreateIcon();
		}
	}

	// Token: 0x06003D74 RID: 15732 RVA: 0x00124D89 File Offset: 0x00123189
	public void AvatarLeavingSelectable(MapAvatarControls _avatar)
	{
		this.m_inSelectable = false;
		this.m_UIState = ClientPortalMapNode.UIState.RequestedUpdate;
	}

	// Token: 0x0400314F RID: 12623
	private static List<HoverIconUIController> s_allIcons;

	// Token: 0x04003150 RID: 12624
	private PortalMapNode m_basePortalMapNode;

	// Token: 0x04003151 RID: 12625
	private ClientPortalMapNode.UIInstance m_uiInstance;

	// Token: 0x04003152 RID: 12626
	private string m_label = string.Empty;

	// Token: 0x04003153 RID: 12627
	private bool m_inSelectable;

	// Token: 0x04003154 RID: 12628
	protected ClientPortalMapNode.UIState m_UIState;

	// Token: 0x02000BBA RID: 3002
	private struct UIInstance
	{
		// Token: 0x06003D76 RID: 15734 RVA: 0x00124D9B File Offset: 0x0012319B
		public void Reset()
		{
			this.m_uiInstance = null;
			this.m_prefab = null;
		}

		// Token: 0x04003155 RID: 12629
		public WorldMapLevelIconUI m_uiInstance;

		// Token: 0x04003156 RID: 12630
		public WorldMapLevelIconUI m_prefab;
	}

	// Token: 0x02000BBB RID: 3003
	protected enum UIState
	{
		// Token: 0x04003158 RID: 12632
		Inactive,
		// Token: 0x04003159 RID: 12633
		RequestedUpdate,
		// Token: 0x0400315A RID: 12634
		Enabling,
		// Token: 0x0400315B RID: 12635
		Enabled,
		// Token: 0x0400315C RID: 12636
		Disabled
	}
}
