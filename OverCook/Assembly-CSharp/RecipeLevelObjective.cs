using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A61 RID: 2657
[Serializable]
public class RecipeLevelObjective : LevelObjectiveBase
{
	// Token: 0x06003467 RID: 13415 RVA: 0x000F62ED File Offset: 0x000F46ED
	public override void Initialise()
	{
		this.SetCallback(true);
	}

	// Token: 0x06003468 RID: 13416 RVA: 0x000F62F6 File Offset: 0x000F46F6
	public override void CleanUp()
	{
		this.SetCallback(false);
	}

	// Token: 0x06003469 RID: 13417 RVA: 0x000F6300 File Offset: 0x000F4700
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

	// Token: 0x0600346A RID: 13418 RVA: 0x000F6374 File Offset: 0x000F4774
	private void OnMealDelivered(int mealId, bool bWasCombo)
	{
		IServerFlowController serverFlowController = GameUtils.RequireManagerInterface<IServerFlowController>();
		ServerKitchenFlowControllerBase serverKitchenFlowControllerBase = serverFlowController as ServerKitchenFlowControllerBase;
		if (serverKitchenFlowControllerBase != null)
		{
			this.m_times.Add(serverKitchenFlowControllerBase.RoundTimer.TimeElapsed);
		}
	}

	// Token: 0x0600346B RID: 13419 RVA: 0x000F63B0 File Offset: 0x000F47B0
	public override bool IsObjectiveComplete()
	{
		if (this.m_times.Count < this.m_recipesRequired)
		{
			return false;
		}
		int num = 0;
		while (num < this.m_times.Count && this.m_times.Count - num >= this.m_recipesRequired)
		{
			float num2 = 0f;
			int num3 = num + 1;
			while (num3 < this.m_times.Count && num3 - num <= this.m_recipesRequired)
			{
				num2 += this.m_times[num3] - this.m_times[num3 - 1];
				num3++;
			}
			if (num2 <= this.m_timeLimit)
			{
				return true;
			}
			num++;
		}
		return false;
	}

	// Token: 0x04002A0B RID: 10763
	[SerializeField]
	public int m_recipesRequired;

	// Token: 0x04002A0C RID: 10764
	[SerializeField]
	public float m_timeLimit;

	// Token: 0x04002A0D RID: 10765
	private bool m_objectiveComplete;

	// Token: 0x04002A0E RID: 10766
	private List<float> m_times = new List<float>();
}
