using System;
using UnityEngine;

// Token: 0x02000A6B RID: 2667
public class TriggerLevelExit : MonoBehaviour, ITriggerReceiver
{
	// Token: 0x060034B5 RID: 13493 RVA: 0x000F752E File Offset: 0x000F592E
	public void OnTrigger(string _trigger)
	{
		if (this.m_triggerToExit == _trigger)
		{
			this.EndLevel();
		}
	}

	// Token: 0x060034B6 RID: 13494 RVA: 0x000F7548 File Offset: 0x000F5948
	private void EndLevel()
	{
		IServerFlowController serverFlowController = GameUtils.RequestManagerInterface<IServerFlowController>();
		if (serverFlowController != null)
		{
			serverFlowController.SkipToEnd();
		}
	}

	// Token: 0x04002A3F RID: 10815
	[SerializeField]
	private string m_triggerToExit;
}
