using System;
using System.Collections.Generic;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020004FE RID: 1278
[ExecutionDependency(typeof(PlayerInputLookup))]
public class CompetitiveKitchenLoaderManager : KitchenLoaderManager
{
	// Token: 0x060017D4 RID: 6100 RVA: 0x00079B74 File Offset: 0x00077F74
	private void AssignChefsForTeam(TeamID team, FastList<User> teamUsers)
	{
		if (teamUsers.Count == 0)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < PlayerIDProvider.s_AllProviders.Count; i++)
		{
			PlayerIDProvider playerIDProvider = PlayerIDProvider.s_AllProviders._items[i];
			if (playerIDProvider != null && playerIDProvider.GetTeam() == team)
			{
				if (num >= teamUsers.Count)
				{
					teamUsers._items[0].Entity2ID = EntitySerialisationRegistry.GetEntry(playerIDProvider.gameObject).m_Header.m_uEntityID;
					break;
				}
				teamUsers._items[num].EntityID = EntitySerialisationRegistry.GetEntry(playerIDProvider.gameObject).m_Header.m_uEntityID;
				num++;
			}
		}
	}

	// Token: 0x060017D5 RID: 6101 RVA: 0x00079C30 File Offset: 0x00078030
	private void AutoAssignUnassignedUsersATeam(FastList<User> users)
	{
		int count = users.Count;
		for (int i = 0; i < count; i++)
		{
			User user = users._items[i];
			if (user.Team == TeamID.None || user.Team == TeamID.Count)
			{
				if (this.m_Team1Users.Count == this.m_Team2Users.Count)
				{
					if (UnityEngine.Random.Range(0, 1) == 0)
					{
						user.Team = TeamID.One;
						this.m_Team1Users.Add(user);
					}
					else
					{
						user.Team = TeamID.Two;
						this.m_Team2Users.Add(user);
					}
				}
				else if (this.m_Team1Users.Count > this.m_Team2Users.Count)
				{
					user.Team = TeamID.Two;
					this.m_Team2Users.Add(user);
				}
				else
				{
					user.Team = TeamID.One;
					this.m_Team1Users.Add(user);
				}
			}
		}
	}

	// Token: 0x060017D6 RID: 6102 RVA: 0x00079D18 File Offset: 0x00078118
	public override void AssignChefEntities(FastList<User> users)
	{
		for (int i = 0; i < PlayerIDProvider.s_AllProviders.Count; i++)
		{
			PlayerIDProvider playerIDProvider = PlayerIDProvider.s_AllProviders._items[i];
			PlayerInputLookup.Player id = playerIDProvider.GetID();
			if (this.m_redPlayersInScene.Contains(id))
			{
				playerIDProvider.AssignToTeam(TeamID.One);
			}
			else
			{
				playerIDProvider.AssignToTeam(TeamID.Two);
			}
		}
		this.m_Team1Users.Clear();
		this.m_Team2Users.Clear();
		int count = users.Count;
		for (int j = 0; j < count; j++)
		{
			User user = users._items[j];
			if (user.Team == TeamID.One)
			{
				this.m_Team1Users.Add(user);
			}
			else if (user.Team == TeamID.Two)
			{
				this.m_Team2Users.Add(user);
			}
		}
		this.AutoAssignUnassignedUsersATeam(users);
		uint colour = 7U;
		uint colour2 = 7U;
		AvatarDirectoryData avatarDirectoryData = GameUtils.GetAvatarDirectoryData();
		uint num = 0U;
		while ((ulong)num < (ulong)((long)avatarDirectoryData.Colours.Length))
		{
			if (avatarDirectoryData.Colours[(int)((UIntPtr)num)] == this.m_red)
			{
				colour = num;
			}
			if (avatarDirectoryData.Colours[(int)((UIntPtr)num)] == this.m_blue)
			{
				colour2 = num;
			}
			num += 1U;
		}
		for (int k = 0; k < this.m_Team1Users.Count; k++)
		{
			this.m_Team1Users._items[k].Colour = colour;
		}
		for (int l = 0; l < this.m_Team2Users.Count; l++)
		{
			this.m_Team2Users._items[l].Colour = colour2;
		}
		this.AssignChefsForTeam(TeamID.One, this.m_Team1Users);
		this.AssignChefsForTeam(TeamID.Two, this.m_Team2Users);
		if ((this.m_Team1Users.Count == 0 || this.m_Team2Users.Count == 0) && ConnectionStatus.IsInSession() && ConnectionStatus.IsHost())
		{
			NetworkErrors.CachedErrorTitle = "Text.Versus.NotEnoughPlayers.Title";
			NetworkErrors.CachedErrorMessage = "Text.Versus.NotEnoughPlayers.Message";
			ServerMessenger.LoadLevel(GameUtils.GetGameSession().TypeSettings.WorldMapScene, GameState.VSLobby, true, GameState.NotSet);
		}
	}

	// Token: 0x04001184 RID: 4484
	[SerializeField]
	[AssignResource("Red", Editorbility.Editable)]
	private ChefColourData m_red;

	// Token: 0x04001185 RID: 4485
	[SerializeField]
	[AssignResource("Blue", Editorbility.Editable)]
	private ChefColourData m_blue;

	// Token: 0x04001186 RID: 4486
	[SerializeField]
	private PlayerInputLookup.Player[] m_redPlayersInScene = new PlayerInputLookup.Player[]
	{
		PlayerInputLookup.Player.One,
		PlayerInputLookup.Player.Three
	};

	// Token: 0x04001187 RID: 4487
	[SerializeField]
	private PlayerInputLookup.Player[] m_bluePlayersInScene = new PlayerInputLookup.Player[]
	{
		PlayerInputLookup.Player.Two,
		PlayerInputLookup.Player.Four
	};

	// Token: 0x04001188 RID: 4488
	private FastList<User> m_Team1Users = new FastList<User>(2);

	// Token: 0x04001189 RID: 4489
	private FastList<User> m_Team2Users = new FastList<User>(2);
}
