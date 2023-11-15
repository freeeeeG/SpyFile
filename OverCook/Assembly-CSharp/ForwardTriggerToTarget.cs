using System;
using UnityEngine;

// Token: 0x02000136 RID: 310
public class ForwardTriggerToTarget : MonoBehaviour
{
	// Token: 0x060005A7 RID: 1447 RVA: 0x0002AACD File Offset: 0x00028ECD
	private void OnTrigger(string _trigger)
	{
		if (this.m_inputTrigger == _trigger)
		{
			this.m_target.SendMessage("OnTrigger", this.m_outputTrigger, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x040004B5 RID: 1205
	[SerializeField]
	private GameObject m_target;

	// Token: 0x040004B6 RID: 1206
	[SerializeField]
	private string m_inputTrigger;

	// Token: 0x040004B7 RID: 1207
	[SerializeField]
	private string m_outputTrigger;
}
