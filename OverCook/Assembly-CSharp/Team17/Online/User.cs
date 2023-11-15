using System;

namespace Team17.Online
{
	// Token: 0x02000998 RID: 2456
	public class User
	{
		// Token: 0x06003000 RID: 12288 RVA: 0x000E1DEC File Offset: 0x000E01EC
		public User(bool bLocal = false, User.MachineID machine = User.MachineID.Count, IOnlineMultiplayerSessionUserId sessionId = null, uint entityId = 0U, uint entity2Id = 0U, EngagementSlot engagement = EngagementSlot.Count, TeamID team = TeamID.None, uint colour = 7U, uint uSelectedChefAvatar = 127U, PadSide padSide = PadSide.Both, User.SplitStatus splitStatus = User.SplitStatus.Count, User.PartyPersistance partyPersist = User.PartyPersistance.NotSet, string displayName = "Unknown", OnlineUserPlatformId platformUserId = null, uint chefAvatarID = 127U)
		{
			this.m_bIsLocal = bLocal;
			this.Machine = machine;
			this.SessionId = sessionId;
			this.EntityID = entityId;
			this.Entity2ID = entity2Id;
			this.Engagement = engagement;
			this.Team = team;
			this.Colour = colour;
			this.SelectedChefAvatar = chefAvatarID;
			this.PadSide = padSide;
			this.m_splitStatus = splitStatus;
			this.m_partyPersist = partyPersist;
			this.m_displayName = displayName;
			this.m_PlatformID = platformUserId;
		}

		// Token: 0x06003001 RID: 12289 RVA: 0x000E1EB0 File Offset: 0x000E02B0
		private void SetProperty<T>(ref T member, T value)
		{
			if ((member != null && value == null) || (member == null && value != null) || (member != null && value != null && !member.Equals(value)))
			{
				member = value;
				this.ChangedThisFrame = true;
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06003002 RID: 12290 RVA: 0x000E1F33 File Offset: 0x000E0333
		// (set) Token: 0x06003003 RID: 12291 RVA: 0x000E1F3B File Offset: 0x000E033B
		public User.MachineID Machine
		{
			get
			{
				return this.m_MachineID;
			}
			set
			{
				this.SetProperty<User.MachineID>(ref this.m_MachineID, value);
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06003004 RID: 12292 RVA: 0x000E1F4A File Offset: 0x000E034A
		// (set) Token: 0x06003005 RID: 12293 RVA: 0x000E1F52 File Offset: 0x000E0352
		public IOnlineMultiplayerSessionUserId SessionId
		{
			get
			{
				return this.m_SessionId;
			}
			set
			{
				this.SetProperty<IOnlineMultiplayerSessionUserId>(ref this.m_SessionId, value);
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06003006 RID: 12294 RVA: 0x000E1F61 File Offset: 0x000E0361
		// (set) Token: 0x06003007 RID: 12295 RVA: 0x000E1F69 File Offset: 0x000E0369
		public uint EntityID
		{
			get
			{
				return this.m_uEntityID;
			}
			set
			{
				this.SetProperty<uint>(ref this.m_uEntityID, value);
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06003008 RID: 12296 RVA: 0x000E1F78 File Offset: 0x000E0378
		// (set) Token: 0x06003009 RID: 12297 RVA: 0x000E1F80 File Offset: 0x000E0380
		public uint Entity2ID
		{
			get
			{
				return this.m_uEntity2ID;
			}
			set
			{
				this.SetProperty<uint>(ref this.m_uEntity2ID, value);
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x0600300A RID: 12298 RVA: 0x000E1F8F File Offset: 0x000E038F
		// (set) Token: 0x0600300B RID: 12299 RVA: 0x000E1F98 File Offset: 0x000E0398
		public EngagementSlot Engagement
		{
			get
			{
				return this.m_Engagement;
			}
			set
			{
				this.SetProperty<EngagementSlot>(ref this.m_Engagement, value);
				if (this.IsLocal)
				{
					IPlayerManager playerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
					this.GamepadUser = playerManager.GetUser(this.Engagement);
					if (this.GamepadUser != null)
					{
						this.DisplayName = this.GamepadUser.DisplayName;
					}
				}
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x0600300C RID: 12300 RVA: 0x000E1FF7 File Offset: 0x000E03F7
		// (set) Token: 0x0600300D RID: 12301 RVA: 0x000E1FFF File Offset: 0x000E03FF
		public TeamID Team
		{
			get
			{
				return this.m_Team;
			}
			set
			{
				this.SetProperty<TeamID>(ref this.m_Team, value);
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x0600300E RID: 12302 RVA: 0x000E200E File Offset: 0x000E040E
		// (set) Token: 0x0600300F RID: 12303 RVA: 0x000E2016 File Offset: 0x000E0416
		public uint Colour
		{
			get
			{
				return this.m_Colour;
			}
			set
			{
				this.SetProperty<uint>(ref this.m_Colour, value);
				this.RefreshSelectedChefData();
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06003010 RID: 12304 RVA: 0x000E202B File Offset: 0x000E042B
		// (set) Token: 0x06003011 RID: 12305 RVA: 0x000E2033 File Offset: 0x000E0433
		public GameState GameState
		{
			get
			{
				return this.m_GameState;
			}
			set
			{
				this.SetProperty<GameState>(ref this.m_GameState, value);
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06003012 RID: 12306 RVA: 0x000E2042 File Offset: 0x000E0442
		// (set) Token: 0x06003013 RID: 12307 RVA: 0x000E204A File Offset: 0x000E044A
		public uint SelectedChefAvatar
		{
			get
			{
				return this.m_uSelectedChefAvatar;
			}
			set
			{
				this.SetProperty<uint>(ref this.m_uSelectedChefAvatar, value);
				this.RefreshSelectedChefData();
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06003014 RID: 12308 RVA: 0x000E205F File Offset: 0x000E045F
		// (set) Token: 0x06003015 RID: 12309 RVA: 0x000E2068 File Offset: 0x000E0468
		public GameSession.SelectedChefData SelectedChefData
		{
			get
			{
				return this.m_SelectedChefData;
			}
			set
			{
				this.m_SelectedChefData = value;
				if (this.m_SelectedChefData != null)
				{
					if (this.SelectedChefAvatar == 127U)
					{
						AvatarDirectoryData avatarDirectoryData = GameUtils.GetAvatarDirectoryData();
						for (int i = 0; i < avatarDirectoryData.Avatars.Length; i++)
						{
							if (avatarDirectoryData.Avatars[i] == this.m_SelectedChefData.Character)
							{
								this.m_uSelectedChefAvatar = (uint)i;
								break;
							}
						}
					}
					if (this.m_Colour == 7U)
					{
						AvatarDirectoryData avatarDirectoryData2 = GameUtils.GetAvatarDirectoryData();
						uint num = 0U;
						while ((ulong)num < (ulong)((long)avatarDirectoryData2.Colours.Length))
						{
							if (avatarDirectoryData2.Colours[(int)((UIntPtr)num)] == this.m_SelectedChefData.Colour)
							{
								this.m_Colour = num;
								break;
							}
							num += 1U;
						}
					}
				}
			}
		}

		// Token: 0x06003016 RID: 12310 RVA: 0x000E2134 File Offset: 0x000E0534
		private void RefreshSelectedChefData()
		{
			if (this.SelectedChefAvatar != 127U && this.Colour != 7U)
			{
				AvatarDirectoryData avatarDirectoryData = GameUtils.GetAvatarDirectoryData();
				if (avatarDirectoryData != null)
				{
					if (this.m_SelectedChefData == null)
					{
						this.m_SelectedChefData = new GameSession.SelectedChefData(avatarDirectoryData.Avatars[(int)((UIntPtr)this.SelectedChefAvatar)], avatarDirectoryData.Colours[(int)this.Colour]);
					}
					else
					{
						this.m_SelectedChefData.Character = avatarDirectoryData.Avatars[(int)((UIntPtr)this.SelectedChefAvatar)];
						this.m_SelectedChefData.Colour = avatarDirectoryData.Colours[(int)this.Colour];
					}
				}
			}
			else
			{
				this.m_SelectedChefData = null;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06003017 RID: 12311 RVA: 0x000E21DF File Offset: 0x000E05DF
		// (set) Token: 0x06003018 RID: 12312 RVA: 0x000E21E7 File Offset: 0x000E05E7
		public bool IsLocal
		{
			get
			{
				return this.m_bIsLocal;
			}
			set
			{
				this.m_bIsLocal = value;
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06003019 RID: 12313 RVA: 0x000E21F0 File Offset: 0x000E05F0
		// (set) Token: 0x0600301A RID: 12314 RVA: 0x000E21F8 File Offset: 0x000E05F8
		public GamepadUser GamepadUser
		{
			get
			{
				return this.m_GamepadUser;
			}
			private set
			{
				this.SetProperty<GamepadUser>(ref this.m_GamepadUser, value);
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x0600301B RID: 12315 RVA: 0x000E2207 File Offset: 0x000E0607
		// (set) Token: 0x0600301C RID: 12316 RVA: 0x000E220F File Offset: 0x000E060F
		public PadSide PadSide
		{
			get
			{
				return this.m_padSide;
			}
			set
			{
				this.SetProperty<PadSide>(ref this.m_padSide, value);
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x0600301D RID: 12317 RVA: 0x000E221E File Offset: 0x000E061E
		// (set) Token: 0x0600301E RID: 12318 RVA: 0x000E2226 File Offset: 0x000E0626
		public User.SplitStatus Split
		{
			get
			{
				return this.m_splitStatus;
			}
			set
			{
				this.SetProperty<User.SplitStatus>(ref this.m_splitStatus, value);
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x0600301F RID: 12319 RVA: 0x000E2235 File Offset: 0x000E0635
		// (set) Token: 0x06003020 RID: 12320 RVA: 0x000E223D File Offset: 0x000E063D
		public User.PartyPersistance PartyPersist
		{
			get
			{
				return this.m_partyPersist;
			}
			set
			{
				this.SetProperty<User.PartyPersistance>(ref this.m_partyPersist, value);
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06003021 RID: 12321 RVA: 0x000E224C File Offset: 0x000E064C
		// (set) Token: 0x06003022 RID: 12322 RVA: 0x000E2254 File Offset: 0x000E0654
		public string DisplayName
		{
			get
			{
				return this.m_displayName;
			}
			set
			{
				this.SetProperty<string>(ref this.m_displayName, value);
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06003023 RID: 12323 RVA: 0x000E2263 File Offset: 0x000E0663
		// (set) Token: 0x06003024 RID: 12324 RVA: 0x000E226B File Offset: 0x000E066B
		public OnlineUserPlatformId PlatformID
		{
			get
			{
				return this.m_PlatformID;
			}
			set
			{
				this.SetProperty<OnlineUserPlatformId>(ref this.m_PlatformID, value);
			}
		}

		// Token: 0x04002681 RID: 9857
		public const uint kInvalidChefAvatar = 127U;

		// Token: 0x04002682 RID: 9858
		public const uint kInvalidChefColour = 7U;

		// Token: 0x04002683 RID: 9859
		public bool ChangedThisFrame;

		// Token: 0x04002684 RID: 9860
		private User.MachineID m_MachineID = User.MachineID.Count;

		// Token: 0x04002685 RID: 9861
		private IOnlineMultiplayerSessionUserId m_SessionId;

		// Token: 0x04002686 RID: 9862
		private uint m_uEntityID;

		// Token: 0x04002687 RID: 9863
		private uint m_uEntity2ID;

		// Token: 0x04002688 RID: 9864
		private EngagementSlot m_Engagement = EngagementSlot.Count;

		// Token: 0x04002689 RID: 9865
		private TeamID m_Team = TeamID.None;

		// Token: 0x0400268A RID: 9866
		private uint m_Colour = 7U;

		// Token: 0x0400268B RID: 9867
		private GameState m_GameState;

		// Token: 0x0400268C RID: 9868
		private uint m_uSelectedChefAvatar = 127U;

		// Token: 0x0400268D RID: 9869
		private GameSession.SelectedChefData m_SelectedChefData;

		// Token: 0x0400268E RID: 9870
		private bool m_bIsLocal = true;

		// Token: 0x0400268F RID: 9871
		private GamepadUser m_GamepadUser;

		// Token: 0x04002690 RID: 9872
		private PadSide m_padSide = PadSide.Both;

		// Token: 0x04002691 RID: 9873
		private User.SplitStatus m_splitStatus = User.SplitStatus.Count;

		// Token: 0x04002692 RID: 9874
		private User.PartyPersistance m_partyPersist;

		// Token: 0x04002693 RID: 9875
		private string m_displayName = "Unknown";

		// Token: 0x04002694 RID: 9876
		private OnlineUserPlatformId m_PlatformID;

		// Token: 0x02000999 RID: 2457
		public enum MachineID
		{
			// Token: 0x04002696 RID: 9878
			One,
			// Token: 0x04002697 RID: 9879
			Two,
			// Token: 0x04002698 RID: 9880
			Three,
			// Token: 0x04002699 RID: 9881
			Four,
			// Token: 0x0400269A RID: 9882
			Count
		}

		// Token: 0x0200099A RID: 2458
		public enum SplitStatus
		{
			// Token: 0x0400269C RID: 9884
			SplitPadHost,
			// Token: 0x0400269D RID: 9885
			SplitPadGuest,
			// Token: 0x0400269E RID: 9886
			NotSplit,
			// Token: 0x0400269F RID: 9887
			Count
		}

		// Token: 0x0200099B RID: 2459
		public enum PartyPersistance
		{
			// Token: 0x040026A1 RID: 9889
			NotSet,
			// Token: 0x040026A2 RID: 9890
			Remain,
			// Token: 0x040026A3 RID: 9891
			Kick
		}
	}
}
