using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Team17.Online
{
	// Token: 0x02000976 RID: 2422
	public class SteamOnlineAvatarImageCoordinator : IOnlineAvatarImageCoordinator
	{
		// Token: 0x06002F42 RID: 12098 RVA: 0x000DCD26 File Offset: 0x000DB126
		public void Initialize()
		{
			if (!this.m_isInitialized)
			{
				SteamOnlineAvatarImageCoordinator.s_steamPersonalStateChangeCallback = Callback<PersonaStateChange_t>.Create(new Callback<PersonaStateChange_t>.DispatchDelegate(this.OnSteamPersonaStateChange));
				SteamOnlineAvatarImageCoordinator.s_steamAvatarImageLoadedCallback = Callback<AvatarImageLoaded_t>.Create(new Callback<AvatarImageLoaded_t>.DispatchDelegate(this.OnSteamAvatarImageLoaded));
				this.m_isInitialized = true;
			}
		}

		// Token: 0x06002F43 RID: 12099 RVA: 0x000DCD68 File Offset: 0x000DB168
		public void Update(float gameTime)
		{
			if (this.m_isInitialized)
			{
				this.m_gameTime = gameTime;
				if (this.m_activeRequest != null)
				{
					if (this.m_activeRequestTimeoutGameTime > this.m_gameTime)
					{
						if (this.m_activeRequest.m_steamImageId > 0)
						{
							try
							{
								Texture2D texture2D = this.MakeTexture(this.m_activeRequest.m_steamImageId);
								if (texture2D != null)
								{
									this.m_activeRequest.m_callback(texture2D, this.m_activeRequest.m_uniqueId);
								}
								this.m_activeRequest.Clear();
								this.m_activeRequest = null;
							}
							catch (Exception ex)
							{
								this.m_activeRequest.Clear();
								this.m_activeRequest = null;
							}
						}
						else if (this.m_activeRequest.m_steamImageId == 0)
						{
							this.m_activeRequest.Clear();
							this.m_activeRequest = null;
						}
						else
						{
							SteamOnlineAvatarImageCoordinator.Request.PersonalInfoState personalInfoState = this.m_activeRequest.m_personalInfoState;
							if (personalInfoState == SteamOnlineAvatarImageCoordinator.Request.PersonalInfoState.eValid)
							{
								this.m_activeRequest.m_personalInfoState = SteamOnlineAvatarImageCoordinator.Request.PersonalInfoState.eComplete;
								this.m_activeRequest.m_steamImageId = SteamFriends.GetLargeFriendAvatar(this.m_activeRequest.m_steamId);
							}
						}
					}
					else
					{
						this.m_activeRequestTimeoutGameTime = 0f;
						this.m_activeRequest.Clear();
						this.m_activeRequest = null;
					}
				}
				else if (this.m_requests.Count > 0)
				{
					this.m_activeRequest = this.m_requests.Dequeue();
					try
					{
						if (this.m_activeRequest.m_isLocal)
						{
							this.m_activeRequestTimeoutGameTime = this.m_gameTime + this.m_maxRequestTimeInSeconds;
							this.m_activeRequest.m_steamImageId = SteamFriends.GetLargeFriendAvatar(this.m_activeRequest.m_steamId);
						}
						else
						{
							this.m_activeRequestTimeoutGameTime = this.m_gameTime + this.m_maxRequestTimeInSeconds;
							if (SteamFriends.RequestUserInformation(this.m_activeRequest.m_steamId, false))
							{
								this.m_activeRequest.m_personalInfoState = SteamOnlineAvatarImageCoordinator.Request.PersonalInfoState.eInProgress;
							}
							else
							{
								this.m_activeRequest.m_steamImageId = SteamFriends.GetLargeFriendAvatar(this.m_activeRequest.m_steamId);
							}
						}
					}
					catch (Exception ex2)
					{
						this.m_activeRequest.Clear();
						this.m_activeRequest = null;
					}
				}
			}
		}

		// Token: 0x06002F44 RID: 12100 RVA: 0x000DCFA8 File Offset: 0x000DB3A8
		public bool RequestAvatarImage(GamepadUser localUser, AvatarImageRequestCompletionCallback completionCallback, out ulong uniqueRequestId)
		{
			if (this.m_isInitialized && null != localUser && completionCallback != null)
			{
				try
				{
					SteamOnlineAvatarImageCoordinator.Request request = new SteamOnlineAvatarImageCoordinator.Request
					{
						m_isLocal = true,
						m_steamId = SteamUser.GetSteamID(),
						m_callback = completionCallback,
						m_uniqueId = this.MakeUniqueRequestId()
					};
					this.m_requests.Enqueue(request);
					uniqueRequestId = request.m_uniqueId;
					return true;
				}
				catch (Exception ex)
				{
				}
			}
			uniqueRequestId = 0UL;
			return false;
		}

		// Token: 0x06002F45 RID: 12101 RVA: 0x000DD038 File Offset: 0x000DB438
		public bool RequestAvatarImage(GamepadUser primaryLocalUser, OnlineUserPlatformId remoteUser, AvatarImageRequestCompletionCallback completionCallback, out ulong uniqueRequestId)
		{
			if (this.m_isInitialized && null != primaryLocalUser && remoteUser != null && completionCallback != null)
			{
				try
				{
					SteamOnlineAvatarImageCoordinator.Request request = new SteamOnlineAvatarImageCoordinator.Request
					{
						m_isLocal = false,
						m_steamId = remoteUser.m_steamId,
						m_callback = completionCallback,
						m_uniqueId = this.MakeUniqueRequestId()
					};
					this.m_requests.Enqueue(request);
					uniqueRequestId = request.m_uniqueId;
					return true;
				}
				catch (Exception ex)
				{
				}
			}
			uniqueRequestId = 0UL;
			return false;
		}

		// Token: 0x06002F46 RID: 12102 RVA: 0x000DD0D0 File Offset: 0x000DB4D0
		private ulong MakeUniqueRequestId()
		{
			while ((this.m_id += 1UL) == 0UL)
			{
			}
			return this.m_id;
		}

		// Token: 0x06002F47 RID: 12103 RVA: 0x000DD104 File Offset: 0x000DB504
		private Texture2D MakeTexture(int steamImageId)
		{
			uint num = 0U;
			uint num2 = 0U;
			if (SteamUtils.GetImageSize(steamImageId, out num, out num2))
			{
				uint num3 = 4U * num2 * num;
				byte[] array = new byte[num3];
				if (SteamUtils.GetImageRGBA(steamImageId, array, array.Length))
				{
					Texture2D texture2D = new Texture2D((int)num, (int)num2, TextureFormat.RGBA32, false);
					texture2D.LoadRawTextureData(array);
					texture2D.Apply();
					return texture2D;
				}
			}
			return null;
		}

		// Token: 0x06002F48 RID: 12104 RVA: 0x000DD160 File Offset: 0x000DB560
		private void OnSteamPersonaStateChange(PersonaStateChange_t param)
		{
			if (this.m_activeRequest != null && param.m_ulSteamID == this.m_activeRequest.m_steamId.m_SteamID)
			{
				int nChangeFlags = (int)param.m_nChangeFlags;
				bool flag = (nChangeFlags & 64) != 0;
				SteamOnlineAvatarImageCoordinator.Request.PersonalInfoState personalInfoState = this.m_activeRequest.m_personalInfoState;
				if (personalInfoState == SteamOnlineAvatarImageCoordinator.Request.PersonalInfoState.eInProgress)
				{
					if (flag)
					{
						this.m_activeRequest.m_personalInfoState = SteamOnlineAvatarImageCoordinator.Request.PersonalInfoState.eValid;
					}
				}
			}
		}

		// Token: 0x06002F49 RID: 12105 RVA: 0x000DD1D6 File Offset: 0x000DB5D6
		private void OnSteamAvatarImageLoaded(AvatarImageLoaded_t param)
		{
			if (this.m_activeRequest != null && param.m_steamID == this.m_activeRequest.m_steamId)
			{
				this.m_activeRequest.m_steamImageId = param.m_iImage;
			}
		}

		// Token: 0x040025BE RID: 9662
		private readonly float m_maxRequestTimeInSeconds = 5f;

		// Token: 0x040025BF RID: 9663
		private static Callback<PersonaStateChange_t> s_steamPersonalStateChangeCallback;

		// Token: 0x040025C0 RID: 9664
		private static Callback<AvatarImageLoaded_t> s_steamAvatarImageLoadedCallback;

		// Token: 0x040025C1 RID: 9665
		private bool m_isInitialized;

		// Token: 0x040025C2 RID: 9666
		private Queue<SteamOnlineAvatarImageCoordinator.Request> m_requests = new Queue<SteamOnlineAvatarImageCoordinator.Request>();

		// Token: 0x040025C3 RID: 9667
		private SteamOnlineAvatarImageCoordinator.Request m_activeRequest;

		// Token: 0x040025C4 RID: 9668
		private float m_activeRequestTimeoutGameTime;

		// Token: 0x040025C5 RID: 9669
		private ulong m_id;

		// Token: 0x040025C6 RID: 9670
		private float m_gameTime;

		// Token: 0x02000977 RID: 2423
		private class Request
		{
			// Token: 0x06002F4C RID: 12108 RVA: 0x000DD222 File Offset: 0x000DB622
			public void Clear()
			{
				this.m_isLocal = false;
				this.m_steamId.Clear();
				this.m_callback = null;
				this.m_uniqueId = 0UL;
				this.m_steamImageId = -1;
				this.m_personalInfoState = SteamOnlineAvatarImageCoordinator.Request.PersonalInfoState.eIdle;
			}

			// Token: 0x040025C7 RID: 9671
			public bool m_isLocal;

			// Token: 0x040025C8 RID: 9672
			public CSteamID m_steamId;

			// Token: 0x040025C9 RID: 9673
			public AvatarImageRequestCompletionCallback m_callback;

			// Token: 0x040025CA RID: 9674
			public ulong m_uniqueId;

			// Token: 0x040025CB RID: 9675
			public int m_steamImageId = -1;

			// Token: 0x040025CC RID: 9676
			public SteamOnlineAvatarImageCoordinator.Request.PersonalInfoState m_personalInfoState;

			// Token: 0x02000978 RID: 2424
			public enum PersonalInfoState
			{
				// Token: 0x040025CE RID: 9678
				eIdle,
				// Token: 0x040025CF RID: 9679
				eInProgress,
				// Token: 0x040025D0 RID: 9680
				eValid,
				// Token: 0x040025D1 RID: 9681
				eComplete
			}
		}
	}
}
