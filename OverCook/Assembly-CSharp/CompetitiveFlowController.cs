using System;
using UnityEngine;

// Token: 0x0200064F RID: 1615
public class CompetitiveFlowController : KitchenFlowControllerBase
{
	// Token: 0x04001794 RID: 6036
	[SerializeField]
	[AssignComponent(Visibility.Show)]
	public CompetitiveFlowController.OutroFlowroutine m_outroFlowroutine;

	// Token: 0x04001795 RID: 6037
	[SerializeField]
	public TeamMonitor m_teamOneData;

	// Token: 0x04001796 RID: 6038
	[SerializeField]
	public TeamMonitor m_teamTwoData;

	// Token: 0x02000650 RID: 1616
	public abstract class OutroFlowroutine : FlowroutineComponent<CompetitiveFlowController.OutroData>
	{
	}

	// Token: 0x02000651 RID: 1617
	public class OutroData
	{
		// Token: 0x06001ECB RID: 7883 RVA: 0x00096BA9 File Offset: 0x00094FA9
		public OutroData(object _scoreData)
		{
			this.ScoreData = _scoreData;
		}

		// Token: 0x04001797 RID: 6039
		public object ScoreData;
	}
}
