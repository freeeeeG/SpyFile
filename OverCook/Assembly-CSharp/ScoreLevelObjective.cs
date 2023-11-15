using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A62 RID: 2658
[Serializable]
public class ScoreLevelObjective : LevelObjectiveBase
{
	// Token: 0x0600346D RID: 13421 RVA: 0x000F6486 File Offset: 0x000F4886
	public override void Initialise()
	{
		this.SetCallback(true);
	}

	// Token: 0x0600346E RID: 13422 RVA: 0x000F648F File Offset: 0x000F488F
	public override void CleanUp()
	{
		this.SetCallback(false);
	}

	// Token: 0x0600346F RID: 13423 RVA: 0x000F6498 File Offset: 0x000F4898
	private void SetCallback(bool bRegister)
	{
		IFlowController flowController = GameUtils.RequestManagerInterface<IFlowController>();
		if (flowController != null)
		{
			MonoBehaviour monoBehaviour = flowController as MonoBehaviour;
			if (flowController != null && monoBehaviour.gameObject != null)
			{
				ClientKitchenFlowControllerBase clientKitchenFlowControllerBase = monoBehaviour.gameObject.RequireComponent<ClientKitchenFlowControllerBase>();
				if (bRegister)
				{
					clientKitchenFlowControllerBase.m_onMealDelivered += this.OnMealDelivered;
				}
				else
				{
					clientKitchenFlowControllerBase.m_onMealDelivered -= this.OnMealDelivered;
				}
			}
		}
	}

	// Token: 0x06003470 RID: 13424 RVA: 0x000F650C File Offset: 0x000F490C
	private void OnMealDelivered(int mealId, bool bWasCombo)
	{
		IServerFlowController serverFlowController = GameUtils.RequireManagerInterface<IServerFlowController>();
		ServerKitchenFlowControllerBase serverKitchenFlowControllerBase = serverFlowController as ServerKitchenFlowControllerBase;
		if (serverKitchenFlowControllerBase != null)
		{
			this.m_times.Add(serverKitchenFlowControllerBase.RoundTimer.TimeElapsed);
			this.m_scores.Add(serverKitchenFlowControllerBase.GetPoints(TeamID.One));
		}
	}

	// Token: 0x06003471 RID: 13425 RVA: 0x000F655C File Offset: 0x000F495C
	public override bool IsObjectiveComplete()
	{
		IServerFlowController serverFlowController = GameUtils.RequireManagerInterface<IServerFlowController>();
		ServerKitchenFlowControllerBase serverKitchenFlowControllerBase = serverFlowController as ServerKitchenFlowControllerBase;
		if (serverKitchenFlowControllerBase != null && serverKitchenFlowControllerBase.GetPoints(TeamID.One) < this.m_scoreRequired)
		{
			return false;
		}
		for (int i = 0; i < this.m_times.Count; i++)
		{
			float num = 0f;
			int num2 = (i != 0) ? 0 : this.m_scores[i];
			for (int j = i + 1; j < this.m_times.Count; j++)
			{
				num += this.m_times[j] - this.m_times[j - 1];
				num2 += this.m_scores[j] - this.m_scores[j - 1];
				if (num <= this.m_timeLimit && num2 >= this.m_scoreRequired)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x04002A0F RID: 10767
	public int m_scoreRequired;

	// Token: 0x04002A10 RID: 10768
	public float m_timeLimit;

	// Token: 0x04002A11 RID: 10769
	private bool m_objectiveComplete;

	// Token: 0x04002A12 RID: 10770
	private List<float> m_times = new List<float>();

	// Token: 0x04002A13 RID: 10771
	private List<int> m_scores = new List<int>();
}
