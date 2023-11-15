using System;
using UnityEngine;

// Token: 0x02000649 RID: 1609
public class CampaignFlowController : KitchenFlowControllerBase
{
	// Token: 0x04001784 RID: 6020
	[SerializeField]
	public TeamMonitor m_teamMonitor = new TeamMonitor();

	// Token: 0x0200064A RID: 1610
	public abstract class OutroFlowroutine : FlowroutineComponent<CampaignFlowController.OutroData>
	{
	}

	// Token: 0x0200064B RID: 1611
	public class OutroData
	{
		// Token: 0x06001EB4 RID: 7860 RVA: 0x00095BD0 File Offset: 0x00093FD0
		public OutroData(object _scoreData, int _points, int _starsAwarded, GameProgress.UnlockData[] _unlocks)
		{
			this.ScoreData = _scoreData;
			this.Points = _points;
			this.StarsAwarded = _starsAwarded;
			this.Unlocks = _unlocks;
		}

		// Token: 0x04001785 RID: 6021
		public object ScoreData;

		// Token: 0x04001786 RID: 6022
		public int Points;

		// Token: 0x04001787 RID: 6023
		public int StarsAwarded;

		// Token: 0x04001788 RID: 6024
		public GameProgress.UnlockData[] Unlocks;
	}

	// Token: 0x0200064C RID: 1612
	public interface IOutroFlowSceneProvider
	{
		// Token: 0x06001EB5 RID: 7861
		string GetNextScene(out GameState o_loadState, out GameState o_loadEndState, out bool o_useLoadingScreen);
	}
}
