using System;
using System.Collections.Generic;
using Steamworks;

namespace Team17.Online
{
	// Token: 0x02000986 RID: 2438
	public class SteamOnlineMultiplayerSessionEnumerateCoordinator : IOnlineMultiplayerSessionEnumerateCoordinator
	{
		// Token: 0x06002F9B RID: 12187 RVA: 0x000DFB1C File Offset: 0x000DDF1C
		public void Initialize(OnlineMultiplayerSessionPropertyCoordinator sessionPropertyCoordinator)
		{
			if (!this.m_isInitialized && sessionPropertyCoordinator != null)
			{
				try
				{
					if (SteamPlayerManager.Initialized)
					{
						SteamOnlineMultiplayerSessionEnumerateCoordinator.s_steamLobbyListCallback = Callback<LobbyMatchList_t>.Create(new Callback<LobbyMatchList_t>.DispatchDelegate(this.OnSteamLobbyList));
						this.m_sessionPropertyCoordinator = sessionPropertyCoordinator;
						this.m_isInitialized = true;
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06002F9C RID: 12188 RVA: 0x000DFB84 File Offset: 0x000DDF84
		public bool IsIdle()
		{
			return this.m_isInitialized && SteamOnlineMultiplayerSessionEnumerateCoordinator.Status.eIdle == this.m_status;
		}

		// Token: 0x06002F9D RID: 12189 RVA: 0x000DFB9C File Offset: 0x000DDF9C
		public bool Start(OnlineMultiplayerLocalUserId localUserId, List<OnlineMultiplayerSessionPropertySearchValue> filterParameters, ushort maxResults, OnlineMultiplayerSessionEnumerateCallback enumerateCallback)
		{
			if (this.m_isInitialized && this.m_status == SteamOnlineMultiplayerSessionEnumerateCoordinator.Status.eIdle && SteamUser.BLoggedOn() && localUserId != null && maxResults > 0 && enumerateCallback != null)
			{
				bool flag = this.ValidateFilterParameters(filterParameters);
				if (flag)
				{
					this.m_localUserId = localUserId;
					this.m_filterParameters = filterParameters;
					this.m_maxResults = (uint)maxResults;
					this.m_resultsCallback = enumerateCallback;
					this.m_timeoutGameTime = this.m_gameTimeAtStartOfFrame + this.m_delayedStartTimeInSeconds;
					this.m_status = SteamOnlineMultiplayerSessionEnumerateCoordinator.Status.eDelayedStart;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F9E RID: 12190 RVA: 0x000DFC25 File Offset: 0x000DE025
		public void Cancel()
		{
			if (this.m_isInitialized)
			{
				this.m_localUserId = null;
				this.m_filterParameters = null;
				this.m_maxResults = 0U;
				this.m_timeoutGameTime = 0f;
				this.m_resultsCallback = null;
				this.m_status = SteamOnlineMultiplayerSessionEnumerateCoordinator.Status.eIdle;
			}
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x000DFC60 File Offset: 0x000DE060
		public void Update(float gameTimeAtStartOfFrame)
		{
			if (this.m_isInitialized)
			{
				this.m_gameTimeAtStartOfFrame = gameTimeAtStartOfFrame;
				switch (this.m_status)
				{
				case SteamOnlineMultiplayerSessionEnumerateCoordinator.Status.eDelayedStart:
					if (this.m_gameTimeAtStartOfFrame >= this.m_timeoutGameTime)
					{
						this.StartEnumeration();
					}
					break;
				case SteamOnlineMultiplayerSessionEnumerateCoordinator.Status.eEnumerating:
					if (this.m_gameTimeAtStartOfFrame >= this.m_timeoutGameTime)
					{
						this.CompleteRequest(null, false);
					}
					break;
				}
			}
		}

		// Token: 0x06002FA0 RID: 12192 RVA: 0x000DFCE4 File Offset: 0x000DE0E4
		private void StartEnumeration()
		{
			if (SteamUser.BLoggedOn() && this.ValidateFilterParameters(this.m_filterParameters))
			{
				try
				{
					if (this.m_filterParameters != null)
					{
						for (int i = 0; i < this.m_filterParameters.Count; i++)
						{
							OnlineMultiplayerSessionPropertySearchValue onlineMultiplayerSessionPropertySearchValue = this.m_filterParameters[i];
							string name = onlineMultiplayerSessionPropertySearchValue.m_property.Name;
							string pchValueToMatch = onlineMultiplayerSessionPropertySearchValue.m_value.ToString();
							ELobbyComparison eComparisonType = ELobbyComparison.k_ELobbyComparisonEqual;
							switch (onlineMultiplayerSessionPropertySearchValue.m_operator)
							{
							case OnlineMultiplayerSessionPropertySearchValue.Operator.eEquals:
								eComparisonType = ELobbyComparison.k_ELobbyComparisonEqual;
								break;
							case OnlineMultiplayerSessionPropertySearchValue.Operator.eNotEquals:
								eComparisonType = ELobbyComparison.k_ELobbyComparisonNotEqual;
								break;
							case OnlineMultiplayerSessionPropertySearchValue.Operator.eLessThan:
								eComparisonType = ELobbyComparison.k_ELobbyComparisonLessThan;
								break;
							case OnlineMultiplayerSessionPropertySearchValue.Operator.eLessEqualsThan:
								eComparisonType = ELobbyComparison.k_ELobbyComparisonEqualToOrLessThan;
								break;
							case OnlineMultiplayerSessionPropertySearchValue.Operator.eGreaterThan:
								eComparisonType = ELobbyComparison.k_ELobbyComparisonGreaterThan;
								break;
							case OnlineMultiplayerSessionPropertySearchValue.Operator.eGreaterEqualsThan:
								eComparisonType = ELobbyComparison.k_ELobbyComparisonEqualToOrGreaterThan;
								break;
							}
							SteamMatchmaking.AddRequestLobbyListStringFilter(name, pchValueToMatch, eComparisonType);
						}
					}
					SteamMatchmaking.AddRequestLobbyListDistanceFilter(ELobbyDistanceFilter.k_ELobbyDistanceFilterDefault);
					if (SteamMatchmaking.RequestLobbyList().m_SteamAPICall != 0UL)
					{
						this.m_status = SteamOnlineMultiplayerSessionEnumerateCoordinator.Status.eEnumerating;
						this.m_timeoutGameTime = this.m_gameTimeAtStartOfFrame + this.m_requestMaxTimeInSeconds;
						return;
					}
				}
				catch (Exception)
				{
				}
			}
			this.CompleteRequest(null, false);
		}

		// Token: 0x06002FA1 RID: 12193 RVA: 0x000DFE2C File Offset: 0x000DE22C
		private void CompleteRequest(List<OnlineMultiplayerSessionEnumeratedRoom> enumeratedSessions, bool wasSuccessful)
		{
			if (this.m_resultsCallback != null)
			{
				try
				{
					this.m_resultsCallback(enumeratedSessions, wasSuccessful);
				}
				catch (Exception)
				{
				}
			}
			this.Cancel();
		}

		// Token: 0x06002FA2 RID: 12194 RVA: 0x000DFE74 File Offset: 0x000DE274
		private bool ValidateFilterParameters(List<OnlineMultiplayerSessionPropertySearchValue> searchProperties)
		{
			if (searchProperties != null)
			{
				for (int i = 0; i < searchProperties.Count; i++)
				{
					OnlineMultiplayerSessionPropertySearchValue onlineMultiplayerSessionPropertySearchValue = searchProperties[i];
					if (onlineMultiplayerSessionPropertySearchValue == null || onlineMultiplayerSessionPropertySearchValue.m_property == null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06002FA3 RID: 12195 RVA: 0x000DFEBC File Offset: 0x000DE2BC
		private void OnSteamLobbyList(LobbyMatchList_t param)
		{
			if (this.m_status == SteamOnlineMultiplayerSessionEnumerateCoordinator.Status.eEnumerating)
			{
				bool wasSuccessful = true;
				List<OnlineMultiplayerSessionEnumeratedRoom> list = null;
				try
				{
					if (param.m_nLobbiesMatching > 0U)
					{
						list = new List<OnlineMultiplayerSessionEnumeratedRoom>((int)this.m_maxResults);
						uint num = 0U;
						while (num < param.m_nLobbiesMatching && list.Count < (int)this.m_maxResults)
						{
							CSteamID lobbyByIndex = SteamMatchmaking.GetLobbyByIndex((int)num);
							if (lobbyByIndex.IsValid() && lobbyByIndex.IsLobby())
							{
								list.Add(new OnlineMultiplayerSessionEnumeratedRoom
								{
									m_steamLobbyId = lobbyByIndex
								});
							}
							num += 1U;
						}
					}
				}
				catch (Exception)
				{
					list = null;
					wasSuccessful = false;
				}
				this.CompleteRequest(list, wasSuccessful);
			}
		}

		// Token: 0x0400262E RID: 9774
		private readonly float m_delayedStartTimeInSeconds = 2f;

		// Token: 0x0400262F RID: 9775
		private readonly float m_requestMaxTimeInSeconds = 10f;

		// Token: 0x04002630 RID: 9776
		private static Callback<LobbyMatchList_t> s_steamLobbyListCallback;

		// Token: 0x04002631 RID: 9777
		private bool m_isInitialized;

		// Token: 0x04002632 RID: 9778
		private OnlineMultiplayerSessionPropertyCoordinator m_sessionPropertyCoordinator;

		// Token: 0x04002633 RID: 9779
		private float m_gameTimeAtStartOfFrame;

		// Token: 0x04002634 RID: 9780
		private float m_timeoutGameTime;

		// Token: 0x04002635 RID: 9781
		private SteamOnlineMultiplayerSessionEnumerateCoordinator.Status m_status;

		// Token: 0x04002636 RID: 9782
		private OnlineMultiplayerSessionEnumerateCallback m_resultsCallback;

		// Token: 0x04002637 RID: 9783
		private OnlineMultiplayerLocalUserId m_localUserId;

		// Token: 0x04002638 RID: 9784
		private List<OnlineMultiplayerSessionPropertySearchValue> m_filterParameters;

		// Token: 0x04002639 RID: 9785
		private uint m_maxResults;

		// Token: 0x02000987 RID: 2439
		private enum Status : short
		{
			// Token: 0x0400263B RID: 9787
			eIdle,
			// Token: 0x0400263C RID: 9788
			eDelayedStart,
			// Token: 0x0400263D RID: 9789
			eEnumerating
		}
	}
}
