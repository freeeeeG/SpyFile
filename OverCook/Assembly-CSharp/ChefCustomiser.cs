using System;
using System.Collections.Generic;
using Team17.Online;
using UnityEngine;

// Token: 0x02000AA9 RID: 2729
public class ChefCustomiser
{
	// Token: 0x06003617 RID: 13847 RVA: 0x000FD540 File Offset: 0x000FB940
	public void CacheCurrentAvatars()
	{
		this.m_cachedAvatars.Clear();
		for (int i = 0; i < ClientUserSystem.m_Users.Count; i++)
		{
			User user = ClientUserSystem.m_Users._items[i];
			if (user.IsLocal)
			{
				ChefCustomiser.CachedKey key = new ChefCustomiser.CachedKey(user.Engagement, user.Split);
				this.m_cachedAvatars[key] = user.SelectedChefAvatar;
			}
		}
	}

	// Token: 0x06003618 RID: 13848 RVA: 0x000FD5B0 File Offset: 0x000FB9B0
	public void RevertAvatars()
	{
		foreach (KeyValuePair<ChefCustomiser.CachedKey, uint> keyValuePair in this.m_cachedAvatars)
		{
			ChefCustomiser.CachedKey key = keyValuePair.Key;
			FastList<User> users = ClientUserSystem.m_Users;
			User.MachineID s_LocalMachineId = ClientUserSystem.s_LocalMachineId;
			User user = UserSystemUtils.FindUser(users, null, s_LocalMachineId, keyValuePair.Key.slot, TeamID.Count, keyValuePair.Key.splitStatus);
			if (user != null)
			{
				ClientMessenger.ChefAvatar(keyValuePair.Value, user);
			}
		}
	}

	// Token: 0x06003619 RID: 13849 RVA: 0x000FD65C File Offset: 0x000FBA5C
	public void ActivateChefCustomisation(bool activate)
	{
		if (this.m_playerManager == null)
		{
			this.m_playerManager = GameUtils.RequireManager<PlayerManager>();
		}
		if (activate && this.m_audioLayer == -1)
		{
			this.m_audioLayer = LayerMask.NameToLayer("Administration");
		}
		bool flag = false;
		for (int i = 0; i < this.m_chefCustomisations.Length; i++)
		{
			if (this.m_chefCustomisations[i].IsActive())
			{
				flag = true;
				break;
			}
		}
		if (activate && !flag)
		{
			this.m_initialInputConfig = PlayerInputLookup.GetInputConfig();
			UserSystemUtils.BuildGameInputConfig();
		}
		else if (!activate && flag)
		{
			if (this.m_initialInputConfig != null)
			{
				PlayerInputLookup.SetInputConfig(this.m_initialInputConfig);
			}
			else
			{
				PlayerInputLookup.ResetToDefaultInputConfig();
			}
		}
		int num = -1;
		for (int j = 0; j < this.m_chefCustomisations.Length; j++)
		{
			User user = null;
			if (j < ClientUserSystem.m_Users.Count)
			{
				user = ClientUserSystem.m_Users._items[j];
			}
			bool flag2 = user != null && user.IsLocal;
			if (flag2)
			{
				num++;
			}
			if (activate)
			{
				if (flag2)
				{
					ControlPadInput.PadNum engagement = (ControlPadInput.PadNum)user.Engagement;
					AmbiControlsMappingData mappingData = (user.PadSide != PadSide.Both) ? this.m_playerManager.SidedAmbiMapping : this.m_playerManager.UnsidedAmbiMapping;
					PlayerGameInput playerGameInput = new PlayerGameInput(engagement, user.PadSide, mappingData);
					if (playerGameInput != null && user.SelectedChefData != null)
					{
						if (!this.m_chefCustomisations[j].IsActive())
						{
							this.m_chefCustomisations[j].Activate(playerGameInput, user.SelectedChefData);
						}
						else
						{
							this.m_chefCustomisations[j].RebindInput(playerGameInput);
						}
					}
				}
				else if (user != null)
				{
					this.m_chefCustomisations[j].ActivateLayoutOnly();
				}
				else
				{
					this.m_chefCustomisations[j].Deactivate();
				}
			}
			else
			{
				this.m_chefCustomisations[j].Deactivate();
			}
		}
	}

	// Token: 0x0600361A RID: 13850 RVA: 0x000FD863 File Offset: 0x000FBC63
	public void SetChefs(FrontendChefCustomisation[] _chefCustomisation)
	{
		this.m_chefCustomisations = _chefCustomisation;
	}

	// Token: 0x0600361B RID: 13851 RVA: 0x000FD86C File Offset: 0x000FBC6C
	public void PlaySelectedAnimations()
	{
		bool flag = false;
		int num = this.m_chefCustomisations.Length;
		for (int i = 0; i < num; i++)
		{
			User user = null;
			if (i < ClientUserSystem.m_Users.Count)
			{
				user = ClientUserSystem.m_Users._items[i];
			}
			bool flag2 = user != null && user.IsLocal;
			if (flag2)
			{
				FrontendChefCustomisation frontendChefCustomisation = this.m_chefCustomisations[i];
				frontendChefCustomisation.PlaySelectedAnimation();
				if (!flag && this.m_audioLayer != -1)
				{
					GameUtils.TriggerAudio(GameOneShotAudioTag.UIChefSelected, this.m_audioLayer);
					flag = true;
				}
			}
		}
	}

	// Token: 0x04002B7E RID: 11134
	private PlayerManager m_playerManager;

	// Token: 0x04002B7F RID: 11135
	private FrontendChefCustomisation[] m_chefCustomisations;

	// Token: 0x04002B80 RID: 11136
	private int m_audioLayer = -1;

	// Token: 0x04002B81 RID: 11137
	private GameInputConfig m_initialInputConfig;

	// Token: 0x04002B82 RID: 11138
	private Dictionary<ChefCustomiser.CachedKey, uint> m_cachedAvatars = new Dictionary<ChefCustomiser.CachedKey, uint>();

	// Token: 0x02000AAA RID: 2730
	private struct CachedKey
	{
		// Token: 0x0600361C RID: 13852 RVA: 0x000FD903 File Offset: 0x000FBD03
		public CachedKey(EngagementSlot _slot, User.SplitStatus _status)
		{
			this.slot = _slot;
			this.splitStatus = _status;
		}

		// Token: 0x04002B83 RID: 11139
		public EngagementSlot slot;

		// Token: 0x04002B84 RID: 11140
		public User.SplitStatus splitStatus;
	}
}
