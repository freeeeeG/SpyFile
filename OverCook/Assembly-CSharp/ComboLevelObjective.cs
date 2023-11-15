using System;
using UnityEngine;

// Token: 0x02000A5E RID: 2654
[Serializable]
public class ComboLevelObjective : LevelObjectiveBase
{
	// Token: 0x0600345A RID: 13402 RVA: 0x000F61D8 File Offset: 0x000F45D8
	public override void Initialise()
	{
		this.SetCallbacks(true);
	}

	// Token: 0x0600345B RID: 13403 RVA: 0x000F61E1 File Offset: 0x000F45E1
	public override void CleanUp()
	{
		this.SetCallbacks(false);
	}

	// Token: 0x0600345C RID: 13404 RVA: 0x000F61EC File Offset: 0x000F45EC
	private void SetCallbacks(bool bRegister)
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
					clientKitchenFlowControllerBase.m_onMealDelivered += this.OnSuccessfulDelivery;
					clientKitchenFlowControllerBase.m_onFailedDelivery += this.OnFailedDelivery;
				}
				else
				{
					clientKitchenFlowControllerBase.m_onMealDelivered -= this.OnSuccessfulDelivery;
					clientKitchenFlowControllerBase.m_onFailedDelivery -= this.OnFailedDelivery;
				}
			}
		}
	}

	// Token: 0x0600345D RID: 13405 RVA: 0x000F6282 File Offset: 0x000F4682
	private void OnSuccessfulDelivery(int mealId, bool bWasCombo)
	{
		this.SetRunningCombo((!bWasCombo) ? 0 : (this.m_runningCombo + 1));
	}

	// Token: 0x0600345E RID: 13406 RVA: 0x000F629E File Offset: 0x000F469E
	private void OnFailedDelivery()
	{
		this.SetRunningCombo(0);
	}

	// Token: 0x0600345F RID: 13407 RVA: 0x000F62A7 File Offset: 0x000F46A7
	private void SetRunningCombo(int value)
	{
		this.m_runningCombo = value;
		this.m_maxAttainedCombo = Mathf.Max(this.m_maxAttainedCombo, this.m_runningCombo);
	}

	// Token: 0x06003460 RID: 13408 RVA: 0x000F62C7 File Offset: 0x000F46C7
	public override bool IsObjectiveComplete()
	{
		return this.m_maxAttainedCombo >= this.m_comboRequired;
	}

	// Token: 0x04002A08 RID: 10760
	[SerializeField]
	public int m_comboRequired;

	// Token: 0x04002A09 RID: 10761
	private int m_runningCombo;

	// Token: 0x04002A0A RID: 10762
	private int m_maxAttainedCombo;
}
