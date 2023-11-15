using System;

// Token: 0x020008ED RID: 2285
public class MultiplayerOperation
{
	// Token: 0x06002C6D RID: 11373 RVA: 0x000D2358 File Offset: 0x000D0758
	public void Start(IMultiplayerTask[] tasks)
	{
		this.m_bDone = false;
		this.m_Tasks = tasks;
		this.m_CurrentTaskIndex = 0;
		this.m_Status.bFinalTask = false;
		this.m_Status.m_TaskSubStatus = this.m_Tasks[this.m_CurrentTaskIndex].GetStatus();
		this.m_Tasks[this.m_CurrentTaskIndex].Start(null);
	}

	// Token: 0x06002C6E RID: 11374 RVA: 0x000D23B8 File Offset: 0x000D07B8
	public void Stop()
	{
		if (this.m_Tasks != null)
		{
			for (int i = 0; i < this.m_Tasks.Length; i++)
			{
				this.m_Tasks[i].Stop();
			}
		}
		this.m_bDone = false;
		this.m_Tasks = null;
		this.m_CurrentTaskIndex = 0;
		this.m_Status.bFinalTask = false;
		this.m_Status.m_TaskSubStatus = null;
	}

	// Token: 0x06002C6F RID: 11375 RVA: 0x000D2423 File Offset: 0x000D0823
	public object GetTaskData()
	{
		return this.m_Tasks[this.m_CurrentTaskIndex].GetData();
	}

	// Token: 0x06002C70 RID: 11376 RVA: 0x000D2437 File Offset: 0x000D0837
	public IConnectionModeSwitchStatus GetStatus()
	{
		return this.m_Status;
	}

	// Token: 0x06002C71 RID: 11377 RVA: 0x000D2440 File Offset: 0x000D0840
	public void Update()
	{
		if (!this.m_bDone)
		{
			IMultiplayerTask multiplayerTask = this.m_Tasks[this.m_CurrentTaskIndex];
			if (multiplayerTask.GetStatus().GetProgress() == eConnectionModeSwitchProgress.Complete)
			{
				if (multiplayerTask.GetStatus().GetResult() == eConnectionModeSwitchResult.Success)
				{
					if (this.m_CurrentTaskIndex < this.m_Tasks.Length - 1)
					{
						this.m_CurrentTaskIndex++;
						this.m_Status.m_TaskSubStatus = this.m_Tasks[this.m_CurrentTaskIndex].GetStatus();
						this.m_Status.bFinalTask = (this.m_CurrentTaskIndex == this.m_Tasks.Length - 1);
						this.m_Tasks[this.m_CurrentTaskIndex].Start(multiplayerTask.GetData());
					}
					else
					{
						this.m_bDone = true;
						this.m_Status.bFinalTask = true;
					}
				}
				else
				{
					this.m_bDone = true;
					this.m_Status.bFinalTask = true;
				}
			}
			else
			{
				multiplayerTask.Update();
			}
		}
	}

	// Token: 0x040023C1 RID: 9153
	private bool m_bDone;

	// Token: 0x040023C2 RID: 9154
	private IMultiplayerTask[] m_Tasks;

	// Token: 0x040023C3 RID: 9155
	private int m_CurrentTaskIndex;

	// Token: 0x040023C4 RID: 9156
	private CompositeStatus m_Status = new CompositeStatus();
}
