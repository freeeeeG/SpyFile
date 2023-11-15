using System;
using System.Collections.Generic;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000894 RID: 2196
public class HighScoreRepository
{
	// Token: 0x17000333 RID: 819
	// (get) Token: 0x06002AC1 RID: 10945 RVA: 0x000C8242 File Offset: 0x000C6642
	public int DLC
	{
		get
		{
			return this.m_iDLC;
		}
	}

	// Token: 0x06002AC2 RID: 10946 RVA: 0x000C824C File Offset: 0x000C664C
	public void Initialise(int DLC)
	{
		this.m_iDLC = DLC;
		Mailbox.Server.RegisterForMessageType(MessageType.HighScores, new OrderedMessageReceivedCallback(this.OnMessageReceived));
		Mailbox.Client.RegisterForMessageType(MessageType.HighScores, new OrderedMessageReceivedCallback(this.OnMessageReceived));
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Combine(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChanged));
	}

	// Token: 0x06002AC3 RID: 10947 RVA: 0x000C82B0 File Offset: 0x000C66B0
	public void Shutdown()
	{
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Remove(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChanged));
		Mailbox.Client.UnregisterForMessageType(MessageType.HighScores, new OrderedMessageReceivedCallback(this.OnMessageReceived));
		Mailbox.Server.UnregisterForMessageType(MessageType.HighScores, new OrderedMessageReceivedCallback(this.OnMessageReceived));
	}

	// Token: 0x06002AC4 RID: 10948 RVA: 0x000C830D File Offset: 0x000C670D
	public void Clear()
	{
		this.m_Data.Clear();
	}

	// Token: 0x06002AC5 RID: 10949 RVA: 0x000C831C File Offset: 0x000C671C
	public void OnMessageReceived(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		HighScoresMessage highScoresMessage = (HighScoresMessage)message;
		this.SetScoresForMachine(highScoresMessage.m_Machine, highScoresMessage.DLC, highScoresMessage.HighScores);
	}

	// Token: 0x06002AC6 RID: 10950 RVA: 0x000C8348 File Offset: 0x000C6748
	public void SetScoresForMachine(User.MachineID machine, int DLC, GameProgress.HighScores highScores)
	{
		if (this.m_iDLC != DLC)
		{
			this.Clear();
			this.m_iDLC = DLC;
		}
		if (this.m_Data.ContainsKey((int)machine))
		{
			this.m_Data[(int)machine] = highScores.Copy();
		}
		else
		{
			this.m_Data.Add((int)machine, highScores.Copy());
		}
	}

	// Token: 0x06002AC7 RID: 10951 RVA: 0x000C83A8 File Offset: 0x000C67A8
	public void LevelProgress(GameProgress.HighScores.Score score)
	{
		for (int i = 0; i < 4; i++)
		{
			User.MachineID machineID = (User.MachineID)i;
			if (UserSystemUtils.FindUser(ClientUserSystem.m_Users, null, machineID, EngagementSlot.Count, TeamID.Count, User.SplitStatus.Count) != null)
			{
				GameProgress.HighScores highScores;
				if (!this.m_Data.TryGetValue((int)machineID, out highScores))
				{
					highScores = new GameProgress.HighScores();
					this.m_Data.Add((int)machineID, highScores);
				}
				int num = highScores.Scores.FindIndex((GameProgress.HighScores.Score item) => item.iLevelID == score.iLevelID);
				if (num >= 0 && num < highScores.Scores.Count)
				{
					int iHighScore = highScores.Scores[num].iHighScore;
					if (iHighScore < score.iHighScore || iHighScore == 65535)
					{
						highScores.Scores[num].iHighScore = score.iHighScore;
					}
					int iSurvivalModeTime = highScores.Scores[num].iSurvivalModeTime;
					if (iSurvivalModeTime < score.iSurvivalModeTime || iSurvivalModeTime == 0)
					{
						highScores.Scores[num].iSurvivalModeTime = score.iSurvivalModeTime;
					}
				}
				else
				{
					highScores.Scores.Add(score);
				}
			}
		}
	}

	// Token: 0x06002AC8 RID: 10952 RVA: 0x000C84F4 File Offset: 0x000C68F4
	public bool GetScore(User.MachineID machine, int iLevelID, ref GameProgress.HighScores.Score score)
	{
		GameProgress.HighScores highScores;
		if (this.m_Data.TryGetValue((int)machine, out highScores))
		{
			score = highScores.Scores.Find((GameProgress.HighScores.Score find) => find.iLevelID == iLevelID);
			return score != null;
		}
		return false;
	}

	// Token: 0x06002AC9 RID: 10953 RVA: 0x000C8544 File Offset: 0x000C6944
	public void Fill(HighScoreRepository otherRepo, bool copy)
	{
		if (copy)
		{
			this.Clear();
			foreach (KeyValuePair<int, GameProgress.HighScores> keyValuePair in otherRepo.m_Data)
			{
				this.m_Data.Add(keyValuePair.Key, keyValuePair.Value.Copy());
			}
		}
		else
		{
			this.m_Data = otherRepo.m_Data;
		}
	}

	// Token: 0x06002ACA RID: 10954 RVA: 0x000C85D4 File Offset: 0x000C69D4
	private void OnUsersChanged()
	{
		this.m_TempDelete.Clear();
		foreach (KeyValuePair<int, GameProgress.HighScores> keyValuePair in this.m_Data)
		{
			if (UserSystemUtils.FindUser(ClientUserSystem.m_Users, null, (User.MachineID)keyValuePair.Key, EngagementSlot.Count, TeamID.Count, User.SplitStatus.Count) == null)
			{
				this.m_TempDelete.Add(keyValuePair.Key);
			}
		}
		for (int i = 0; i < this.m_TempDelete.Count; i++)
		{
			this.m_Data.Remove(this.m_TempDelete[i]);
		}
		this.m_TempDelete.Clear();
	}

	// Token: 0x040021C0 RID: 8640
	private int m_iDLC = -1;

	// Token: 0x040021C1 RID: 8641
	private List<int> m_TempDelete = new List<int>();

	// Token: 0x040021C2 RID: 8642
	private Dictionary<int, GameProgress.HighScores> m_Data = new Dictionary<int, GameProgress.HighScores>();
}
