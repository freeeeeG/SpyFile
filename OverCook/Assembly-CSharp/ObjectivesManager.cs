using System;
using System.Collections.Generic;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000514 RID: 1300
public class ObjectivesManager : Manager
{
	// Token: 0x0600183E RID: 6206 RVA: 0x0007B05C File Offset: 0x0007945C
	private void Start()
	{
		LevelConfigBase levelConfig = GameUtils.GetLevelConfig();
		if (levelConfig.m_objectives == null)
		{
			return;
		}
		for (int i = 0; i < levelConfig.m_objectives.Length; i++)
		{
			LevelObjectiveBase item = UnityEngine.Object.Instantiate<LevelObjectiveBase>(levelConfig.m_objectives[i]);
			this.m_levelObjectives.Add(item);
		}
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x0600183F RID: 6207 RVA: 0x0007B0C8 File Offset: 0x000794C8
	public void OnDestroy()
	{
		int count = this.m_levelObjectives.Count;
		for (int i = 0; i < count; i++)
		{
			this.m_levelObjectives[i].CleanUp();
			UnityEngine.Object.Destroy(this.m_levelObjectives[i]);
		}
		this.m_levelObjectives.Clear();
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06001840 RID: 6208 RVA: 0x0007B138 File Offset: 0x00079538
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.StartEntities)
		{
			int count = this.m_levelObjectives.Count;
			for (int i = 0; i < count; i++)
			{
				this.m_levelObjectives[i].Initialise();
			}
		}
	}

	// Token: 0x06001841 RID: 6209 RVA: 0x0007B188 File Offset: 0x00079588
	public bool HasObjectives(bool includeComplete = true)
	{
		if (!includeComplete)
		{
			for (int i = 0; i < this.m_levelObjectives.Count; i++)
			{
				if (!this.m_levelObjectives[i].IsObjectiveComplete())
				{
					return true;
				}
			}
		}
		return this.m_levelObjectives.Count > 0;
	}

	// Token: 0x06001842 RID: 6210 RVA: 0x0007B1E0 File Offset: 0x000795E0
	public bool HasObjective(Type objectiveType, bool includeComplete = true)
	{
		for (int i = 0; i < this.m_levelObjectives.Count; i++)
		{
			if (includeComplete || !this.m_levelObjectives[i].IsObjectiveComplete())
			{
				if (this.m_levelObjectives[i].GetType() == objectiveType)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06001843 RID: 6211 RVA: 0x0007B244 File Offset: 0x00079644
	public bool AllObjectivesComplete()
	{
		bool flag = true;
		for (int i = 0; i < this.m_levelObjectives.Count; i++)
		{
			flag &= this.m_levelObjectives[i].IsObjectiveComplete();
		}
		return flag;
	}

	// Token: 0x06001844 RID: 6212 RVA: 0x0007B284 File Offset: 0x00079684
	public bool IsObjectiveComplete(Type objectiveType)
	{
		for (int i = 0; i < this.m_levelObjectives.Count; i++)
		{
			if (this.m_levelObjectives[i].GetType() == objectiveType)
			{
				return this.m_levelObjectives[i].IsObjectiveComplete();
			}
		}
		return false;
	}

	// Token: 0x04001381 RID: 4993
	protected List<LevelObjectiveBase> m_levelObjectives = new List<LevelObjectiveBase>();
}
