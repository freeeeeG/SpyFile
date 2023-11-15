using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A16 RID: 2582
[AddComponentMenu("Scripts/Game/Player/PlayerIDProvider")]
public class PlayerIDProvider : MonoBehaviour
{
	// Token: 0x06003334 RID: 13108 RVA: 0x000F04E5 File Offset: 0x000EE8E5
	public void Awake()
	{
		PlayerIDProvider.s_AllProviders.Add(this);
	}

	// Token: 0x06003335 RID: 13109 RVA: 0x000F04F2 File Offset: 0x000EE8F2
	public void OnDestroy()
	{
		PlayerIDProvider.s_AllProviders.Remove(this);
		if (PlayerIDProvider.OnPlayerIDProviderDestroyed != null)
		{
			PlayerIDProvider.OnPlayerIDProviderDestroyed();
		}
	}

	// Token: 0x06003336 RID: 13110 RVA: 0x000F0514 File Offset: 0x000EE914
	public void OverridePlayerId(PlayerInputLookup.Player _player)
	{
		this.m_player = _player;
	}

	// Token: 0x06003337 RID: 13111 RVA: 0x000F051D File Offset: 0x000EE91D
	public void AssignToTeam(TeamID _teamID)
	{
		this.m_teamID = _teamID;
	}

	// Token: 0x06003338 RID: 13112 RVA: 0x000F0526 File Offset: 0x000EE926
	public bool IsLocallyControlled()
	{
		return this.m_player != PlayerInputLookup.Player.Count;
	}

	// Token: 0x06003339 RID: 13113 RVA: 0x000F0535 File Offset: 0x000EE935
	public PlayerInputLookup.Player GetID()
	{
		return this.m_player;
	}

	// Token: 0x0600333A RID: 13114 RVA: 0x000F053D File Offset: 0x000EE93D
	public TeamID GetTeam()
	{
		return this.m_teamID;
	}

	// Token: 0x0400291D RID: 10525
	public static FastList<PlayerIDProvider> s_AllProviders = new FastList<PlayerIDProvider>();

	// Token: 0x0400291E RID: 10526
	[SerializeField]
	private PlayerInputLookup.Player m_player;

	// Token: 0x0400291F RID: 10527
	private TeamID m_teamID;

	// Token: 0x04002920 RID: 10528
	public static GenericVoid OnPlayerIDProviderDestroyed;
}
