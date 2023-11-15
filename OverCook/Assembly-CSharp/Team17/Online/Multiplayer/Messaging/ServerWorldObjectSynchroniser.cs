using System;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x02000929 RID: 2345
	public class ServerWorldObjectSynchroniser : ServerSynchroniserBase
	{
		// Token: 0x06002E25 RID: 11813 RVA: 0x000D5671 File Offset: 0x000D3A71
		protected WorldObjectMessage GetMessageData()
		{
			return this.m_ServerData;
		}

		// Token: 0x06002E26 RID: 11814 RVA: 0x000D567C File Offset: 0x000D3A7C
		public virtual void Awake()
		{
			this.m_Transform = base.transform;
			IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
			this.m_SessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x000D56A7 File Offset: 0x000D3AA7
		public void PausePositions()
		{
			this.m_bSyncPositions = false;
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x000D56B0 File Offset: 0x000D3AB0
		public void ResumePositions()
		{
			this.m_bSyncPositions = true;
			this.m_bSentReliableRestPosition = false;
			this.m_LastUnreliableActiveSend = 0f;
			this.RefreshParent();
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x000D56D4 File Offset: 0x000D3AD4
		public override void StartSynchronising(Component synchronisedObject)
		{
			this.m_Transform = base.transform;
			if (this.m_Transform.parent != null)
			{
				IParentable parentable = this.m_Transform.parent.gameObject.RequestInterfaceUpwardsRecursive<IParentable>();
				if (parentable != null)
				{
					this.m_Transform.SetParent(parentable.GetAttachPoint(base.gameObject), true);
				}
			}
			this.RefreshParent();
			UserSystemUtils.OnServerChangedGameState = (GenericVoid<GameState, GameStateMessage.GameStatePayload>)Delegate.Combine(UserSystemUtils.OnServerChangedGameState, new GenericVoid<GameState, GameStateMessage.GameStatePayload>(this.OnGameStateChanged));
			FastList<User> users = ServerUserSystem.m_Users;
			User.MachineID s_LocalMachineId = ServerUserSystem.s_LocalMachineId;
			User user = UserSystemUtils.FindUser(users, null, s_LocalMachineId, EngagementSlot.One, TeamID.Count, User.SplitStatus.Count);
			if (user != null)
			{
				switch (user.GameState)
				{
				case GameState.LoadKitchen:
				case GameState.LoadedKitchen:
				case GameState.ScanNetworkEntities:
				case GameState.ScannedNetworkEntities:
				case GameState.StartSynchronising:
				case GameState.StartedSyncronising:
				case GameState.AssignChefsToUsers:
				case GameState.AssignedChefsToUsers:
				case GameState.StartEntities:
				case GameState.StartedEntities:
				case GameState.LoadMap:
				case GameState.LoadedMap:
				case GameState.MapScanNetworkEntities:
				case GameState.MapScannedNetworkEntities:
				case GameState.MapStartSynchronising:
				case GameState.MapStartedSyncronising:
				case GameState.MapStartEntities:
				case GameState.MapStartedEntities:
					this.m_bSleepAllowed = false;
					break;
				}
			}
			this.m_bStartedSynchronising = true;
			this.m_bSyncPositions = true;
		}

		// Token: 0x06002E2A RID: 11818 RVA: 0x000D5814 File Offset: 0x000D3C14
		public override void StopSynchronising()
		{
			UserSystemUtils.OnServerChangedGameState = (GenericVoid<GameState, GameStateMessage.GameStatePayload>)Delegate.Remove(UserSystemUtils.OnServerChangedGameState, new GenericVoid<GameState, GameStateMessage.GameStatePayload>(this.OnGameStateChanged));
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x000D5837 File Offset: 0x000D3C37
		protected virtual void OnGameStateChanged(GameState state, GameStateMessage.GameStatePayload payload)
		{
			if (state != GameState.StartSynchronising)
			{
				if (state != GameState.RunKitchen)
				{
					if (state == GameState.MapStartSynchronising)
					{
						goto IL_25;
					}
					if (state != GameState.RunMapUnfoldRoutine)
					{
						return;
					}
				}
				this.m_bSleepAllowed = true;
				return;
			}
			IL_25:
			this.m_bSleepAllowed = false;
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x000D5876 File Offset: 0x000D3C76
		public override EntityType GetEntityType()
		{
			return EntityType.WorldObject;
		}

		// Token: 0x06002E2D RID: 11821 RVA: 0x000D587C File Offset: 0x000D3C7C
		public override Serialisable GetServerUpdate()
		{
			if (!this.m_bStartedSynchronising)
			{
				return null;
			}
			if (null == this.m_Transform)
			{
				return null;
			}
			this.m_bActive = false;
			if (!this.m_bSleepAllowed)
			{
				this.m_bActive = true;
			}
			this.m_bActive |= this.PopulateMessage();
			if (this.m_bActive)
			{
				this.m_bSentReliableRestPosition = false;
				this.m_LastUnreliableActiveSend = Time.time;
				return this.m_ServerData;
			}
			if (this.m_bSleepAllowed && !this.m_bSentReliableRestPosition && Time.time > this.m_LastUnreliableActiveSend + 1f)
			{
				this.m_bSentReliableRestPosition = true;
				this.m_bParentChanged = false;
				this.SendServerEvent(this.m_ServerData);
				return null;
			}
			if (this.m_bParentChanged)
			{
				return this.m_ServerData;
			}
			return null;
		}

		// Token: 0x06002E2E RID: 11822 RVA: 0x000D5954 File Offset: 0x000D3D54
		protected bool IsWorldObjectActive()
		{
			return this.m_bActive;
		}

		// Token: 0x06002E2F RID: 11823 RVA: 0x000D595C File Offset: 0x000D3D5C
		private bool PopulateMessage()
		{
			bool flag = false;
			bool flag2 = this.m_bSyncPositions;
			if (this.m_CachedParentTransform != this.m_Transform.parent)
			{
				flag = true;
				this.m_bParentChanged = true;
				this.RefreshParent();
			}
			if (this.m_bParentChanged)
			{
				flag2 = true;
			}
			this.m_ServerData.HasPositions = flag2;
			bool flag3 = (this.m_ServerData.LocalPosition - this.m_Transform.localPosition).sqrMagnitude > 0.001f;
			bool flag4 = Quaternion.Dot(this.m_ServerData.LocalRotation, this.m_Transform.localRotation) < 0.99f;
			if (flag || (flag2 && (flag3 || flag4)))
			{
				flag |= this.m_bSyncPositions;
				this.m_ServerData.LocalPosition = this.m_Transform.localPosition;
				this.m_ServerData.LocalRotation = this.m_Transform.localRotation;
			}
			else if (this.m_bParentChanged)
			{
				this.m_ServerData.HasPositions = true;
				this.m_ServerData.LocalPosition = this.m_Transform.localPosition;
				this.m_ServerData.LocalRotation = this.m_Transform.localRotation;
			}
			return flag;
		}

		// Token: 0x06002E30 RID: 11824 RVA: 0x000D5A9C File Offset: 0x000D3E9C
		protected void RefreshParent()
		{
			Transform parent = this.m_Transform.parent;
			if (this.m_CachedParentTransform != parent)
			{
				this.m_bParentChanged = true;
			}
			this.m_CachedParentTransform = parent;
			this.m_ServerData.HasParent = (this.m_CachedParentTransform != null);
			this.m_ServerData.ParentEntityID = 0U;
			if (this.m_CachedParentTransform != null)
			{
				IParentable parentable = this.m_CachedParentTransform.gameObject.RequestInterfaceUpwardsRecursive<IParentable>();
				if (parentable != null)
				{
					MonoBehaviour monoBehaviour = parentable as MonoBehaviour;
					GameObject gameObject = monoBehaviour.gameObject;
					EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(gameObject);
					if (entry != null)
					{
						this.m_ServerData.ParentEntityID = entry.m_Header.m_uEntityID;
					}
				}
			}
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x000D5B54 File Offset: 0x000D3F54
		public void ResumeAllClients(bool _resumeServerClient = true)
		{
			bool flag = true;
			if (this.m_SessionCoordinator != null)
			{
				IOnlineMultiplayerSessionUserId[] array = this.m_SessionCoordinator.Members();
				if (array != null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						flag &= this.SendResumeData(array[i]);
					}
				}
			}
			if (_resumeServerClient)
			{
				flag &= this.SendResumeData(null);
			}
			if (flag)
			{
				this.m_bPaused = false;
			}
		}

		// Token: 0x06002E32 RID: 11826 RVA: 0x000D5BC0 File Offset: 0x000D3FC0
		public void ResumeClient(IOnlineMultiplayerSessionUserId sessionUserId)
		{
			bool flag = this.SendResumeData(sessionUserId);
			if (flag)
			{
				this.m_bPaused = false;
			}
		}

		// Token: 0x06002E33 RID: 11827 RVA: 0x000D5BE4 File Offset: 0x000D3FE4
		protected virtual bool SendResumeData(IOnlineMultiplayerSessionUserId sessionUserId)
		{
			this.RefreshParent();
			this.m_ServerData.LocalPosition = this.m_Transform.localPosition;
			this.m_ServerData.LocalRotation = this.m_Transform.localRotation;
			ServerMessenger.ResumeWorldObjectSync(base.gameObject, this.m_ServerData, sessionUserId);
			return true;
		}

		// Token: 0x04002512 RID: 9490
		public const float m_TimeUntilReliableUpdate = 1f;

		// Token: 0x04002513 RID: 9491
		private WorldObjectMessage m_ServerData = new WorldObjectMessage();

		// Token: 0x04002514 RID: 9492
		protected Transform m_Transform;

		// Token: 0x04002515 RID: 9493
		private Transform m_CachedParentTransform;

		// Token: 0x04002516 RID: 9494
		private float m_LastUnreliableActiveSend;

		// Token: 0x04002517 RID: 9495
		private bool m_bSentReliableRestPosition;

		// Token: 0x04002518 RID: 9496
		protected bool m_bStartedSynchronising;

		// Token: 0x04002519 RID: 9497
		protected bool m_bSleepAllowed = true;

		// Token: 0x0400251A RID: 9498
		private bool m_bActive;

		// Token: 0x0400251B RID: 9499
		private bool m_bSyncPositions = true;

		// Token: 0x0400251C RID: 9500
		private bool m_bParentChanged = true;

		// Token: 0x0400251D RID: 9501
		protected bool m_bPaused;

		// Token: 0x0400251E RID: 9502
		private IOnlineMultiplayerSessionCoordinator m_SessionCoordinator;
	}
}
