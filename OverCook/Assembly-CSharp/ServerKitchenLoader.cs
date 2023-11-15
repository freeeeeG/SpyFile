using System;
using Team17.Online;
using UnityEngine;

// Token: 0x02000892 RID: 2194
public class ServerKitchenLoader : MonoBehaviour
{
	// Token: 0x06002AB3 RID: 10931 RVA: 0x000C7FD7 File Offset: 0x000C63D7
	private void Awake()
	{
		this.m_State = GameState.LoadKitchen;
		this.m_KitchenBootstap = GameUtils.RequireManager<KitchenBootstrapManager>();
	}

	// Token: 0x06002AB4 RID: 10932 RVA: 0x000C7FEB File Offset: 0x000C63EB
	private void Start()
	{
	}

	// Token: 0x06002AB5 RID: 10933 RVA: 0x000C7FF0 File Offset: 0x000C63F0
	private void Update()
	{
		switch (this.m_State)
		{
		case GameState.LoadKitchen:
			if (this.AreAllUsersInGameState(GameState.LoadedKitchen))
			{
				this.ChangeGameState(GameState.ScanNetworkEntities);
			}
			break;
		case GameState.ScanNetworkEntities:
			if (this.AreAllUsersInGameState(GameState.ScannedNetworkEntities))
			{
				this.ChangeGameState(GameState.StartSynchronising);
			}
			break;
		case GameState.StartSynchronising:
			if (this.AreAllUsersInGameState(GameState.StartedSyncronising))
			{
				this.ChangeGameState(GameState.AssignChefsToUsers);
				for (int i = 0; i < ServerUserSystem.m_Users.Count; i++)
				{
					User user = ServerUserSystem.m_Users._items[i];
					if (user.SelectedChefData == null)
					{
						user.SelectedChefData = this.m_KitchenBootstap.GetDefaultChef(i);
					}
				}
				KitchenLoaderManager kitchenLoaderManager = GameUtils.RequestManagerInterface<KitchenLoaderManager>();
				kitchenLoaderManager.AssignChefEntities(ServerUserSystem.m_Users);
				ServerMessenger.ChefOwnership();
			}
			break;
		case GameState.AssignChefsToUsers:
			if (this.AreAllUsersInGameState(GameState.AssignedChefsToUsers))
			{
				this.ChangeGameState(GameState.StartEntities);
			}
			break;
		case GameState.StartEntities:
			if (this.AreAllUsersInGameState(GameState.StartedEntities))
			{
				this.ChangeGameState(GameState.RunKitchen);
				IServerFlowController serverFlowController = GameUtils.RequireManagerInterface<IServerFlowController>();
				if (serverFlowController != null)
				{
					serverFlowController.StartFlow();
				}
			}
			break;
		}
	}

	// Token: 0x06002AB6 RID: 10934 RVA: 0x000C8125 File Offset: 0x000C6525
	private void OnDestroy()
	{
	}

	// Token: 0x06002AB7 RID: 10935 RVA: 0x000C8127 File Offset: 0x000C6527
	private void ChangeGameState(GameState state)
	{
		UserSystemUtils.ChangeGameState(state, null);
		this.m_State = state;
	}

	// Token: 0x06002AB8 RID: 10936 RVA: 0x000C8137 File Offset: 0x000C6537
	private bool AreAllUsersInGameState(GameState state)
	{
		return UserSystemUtils.AreAllUsersInGameState(ServerUserSystem.m_Users, state);
	}

	// Token: 0x040021BD RID: 8637
	private GameState m_State;

	// Token: 0x040021BE RID: 8638
	private KitchenBootstrapManager m_KitchenBootstap;
}
