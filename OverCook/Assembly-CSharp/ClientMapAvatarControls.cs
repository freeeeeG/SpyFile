using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020009F8 RID: 2552
public class ClientMapAvatarControls : ClientSynchroniserBase
{
	// Token: 0x060031DD RID: 12765 RVA: 0x000E9DF4 File Offset: 0x000E81F4
	protected void Awake()
	{
		if (this.m_avatarControls == null)
		{
			this.m_avatarControls = base.GetComponent<MapAvatarControls>();
		}
		this.m_groundCast = base.gameObject.RequireComponent<MapAvatarGroundCast>();
		this.m_avatarTransformer = base.gameObject.RequireComponent<MapAvatarTransformer>();
		this.m_initialPosition = base.transform.position;
		this.m_Transform = base.transform;
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		this.m_avatarControls.SetOriginalVanRotation(this.m_avatarControls.m_vanMesh.transform.rotation);
	}

	// Token: 0x060031DE RID: 12766 RVA: 0x000E9E95 File Offset: 0x000E8295
	public void Update()
	{
		if (!this.m_bStarted)
		{
			return;
		}
		if (this.m_avatarControls != null && this.m_avatarControls.enabled)
		{
			this.Update_Horn();
			this.Update_Movement();
		}
	}

	// Token: 0x060031DF RID: 12767 RVA: 0x000E9ED0 File Offset: 0x000E82D0
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x060031E0 RID: 12768 RVA: 0x000E9EF0 File Offset: 0x000E82F0
	private void Update_Horn()
	{
		if (!TimeManager.IsPaused(base.gameObject) && this.m_hornButtons != null)
		{
			for (int i = 0; i < this.m_hornButtons.Length; i++)
			{
				if (this.m_hornButtons[i] != null && this.m_hornButtons[i].JustPressed())
				{
					ClientMessenger.MapAvatarHorn(i);
				}
			}
		}
	}

	// Token: 0x060031E1 RID: 12769 RVA: 0x000E9F58 File Offset: 0x000E8358
	private void Update_Movement()
	{
		if (!ConnectionStatus.IsHost() && ConnectionStatus.IsInSession())
		{
			this.m_avatarControls.UpdateMovement();
		}
		this.m_avatarControls.m_animator.SetFloat(ClientMapAvatarControls.m_iSpeed, this.m_avatarControls.GetSpeed());
		this.m_avatarControls.OrientateVan(TimeManager.GetDeltaTime(base.gameObject), this.m_groundCast.GetGroundNormal());
	}

	// Token: 0x060031E2 RID: 12770 RVA: 0x000E9FC5 File Offset: 0x000E83C5
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_avatarControls = (MapAvatarControls)synchronisedObject;
		this.m_hornButtons = this.BuildHornButtons();
	}

	// Token: 0x060031E3 RID: 12771 RVA: 0x000E9FDF File Offset: 0x000E83DF
	public override void ApplyServerUpdate(Serialisable serialisable)
	{
		this.HandleMapAvatarMessage(serialisable);
	}

	// Token: 0x060031E4 RID: 12772 RVA: 0x000E9FE8 File Offset: 0x000E83E8
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		this.HandleMapAvatarMessage(serialisable);
	}

	// Token: 0x060031E5 RID: 12773 RVA: 0x000E9FF4 File Offset: 0x000E83F4
	private void HandleMapAvatarMessage(Serialisable serialisable)
	{
		MapAvatarControlsMessage mapAvatarControlsMessage = (MapAvatarControlsMessage)serialisable;
		if (mapAvatarControlsMessage.m_bHorns[0])
		{
			GameUtils.TriggerAudio(GameOneShotAudioTag.VanHorn, base.gameObject.layer);
		}
		if (mapAvatarControlsMessage.m_bHorns[1])
		{
			GameUtils.TriggerAudio(GameOneShotAudioTag.VanHorn02, base.gameObject.layer);
		}
		if (mapAvatarControlsMessage.m_bHorns[2])
		{
			GameUtils.TriggerAudio(GameOneShotAudioTag.VanHorn03, base.gameObject.layer);
		}
		if (mapAvatarControlsMessage.m_bHorns[3])
		{
			GameUtils.TriggerAudio(GameOneShotAudioTag.VanHorn04, base.gameObject.layer);
		}
		if (mapAvatarControlsMessage.m_bDash)
		{
			this.m_avatarControls.DashStarted();
			switch (this.m_avatarTransformer.CurrentType)
			{
			case MapAvatarTransformer.VanType.LAND:
				GameUtils.TriggerAudio(this.m_avatarControls.m_dashTags.m_landTag, base.gameObject.layer);
				break;
			case MapAvatarTransformer.VanType.WATER:
				GameUtils.TriggerAudio(this.m_avatarControls.m_dashTags.m_waterTag, base.gameObject.layer);
				break;
			case MapAvatarTransformer.VanType.FLYING:
				GameUtils.TriggerAudio(this.m_avatarControls.m_dashTags.m_flyingTag, base.gameObject.layer);
				break;
			}
		}
		if (this.m_currentSelectableEntityID != mapAvatarControlsMessage.CurrentSelectableEntityId)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(mapAvatarControlsMessage.CurrentSelectableEntityId);
			if ((mapAvatarControlsMessage.CurrentSelectableEntityId == 0U || entry != this.m_currentSelectable) && this.m_currentSelectable != null)
			{
				this.m_currentSelectable.AvatarLeavingSelectable(this.m_avatarControls);
				this.m_currentSelectable = null;
			}
			if (entry != null)
			{
				IClientMapSelectable clientMapSelectable = entry.m_GameObject.RequestInterfaceRecursive<IClientMapSelectable>();
				if (clientMapSelectable != null)
				{
					this.m_currentSelectable = clientMapSelectable;
					clientMapSelectable.AvatarEnteringSelectable(this.m_avatarControls);
				}
			}
			this.m_currentSelectableEntityID = mapAvatarControlsMessage.CurrentSelectableEntityId;
		}
	}

	// Token: 0x060031E6 RID: 12774 RVA: 0x000EA1C6 File Offset: 0x000E85C6
	public override EntityType GetEntityType()
	{
		return EntityType.WorldMapVanControls;
	}

	// Token: 0x060031E7 RID: 12775 RVA: 0x000EA1CC File Offset: 0x000E85CC
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		GameState state = gameStateMessage.m_State;
		if (state == GameState.RunMapUnfoldRoutine)
		{
			ParticleSystem[] array = base.gameObject.RequestComponentsRecursive<ParticleSystem>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].isPlaying)
				{
					array[i].RestartPFX();
				}
			}
			this.m_bStarted = true;
		}
	}

	// Token: 0x060031E8 RID: 12776 RVA: 0x000EA238 File Offset: 0x000E8638
	private ILogicalButton[] BuildHornButtons()
	{
		ILogicalButton[] array = new ILogicalButton[4];
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			ILogicalButton logicalButton;
			if (i < ClientUserSystem.m_Users.Count && ClientUserSystem.m_Users._items[i].IsLocal)
			{
				logicalButton = PlayerInputLookup.GetEngagedButton(PlayerInputLookup.LogicalButtonID.Horn, (PlayerInputLookup.Player)num);
				num++;
			}
			else
			{
				logicalButton = new ComboLogicalButton(new ILogicalButton[0], ComboLogicalButton.ComboType.Or);
			}
			array[i] = logicalButton;
		}
		return array;
	}

	// Token: 0x04002813 RID: 10259
	private bool m_bStarted;

	// Token: 0x04002814 RID: 10260
	private MapAvatarControls m_avatarControls;

	// Token: 0x04002815 RID: 10261
	private MapAvatarGroundCast m_groundCast;

	// Token: 0x04002816 RID: 10262
	private Vector3 m_initialPosition;

	// Token: 0x04002817 RID: 10263
	private Transform m_Transform;

	// Token: 0x04002818 RID: 10264
	private ILogicalButton[] m_hornButtons;

	// Token: 0x04002819 RID: 10265
	private MapAvatarTransformer m_avatarTransformer;

	// Token: 0x0400281A RID: 10266
	private IClientMapSelectable m_currentSelectable;

	// Token: 0x0400281B RID: 10267
	private uint m_currentSelectableEntityID;

	// Token: 0x0400281C RID: 10268
	private static readonly int m_iSpeed = Animator.StringToHash("Speed");
}
