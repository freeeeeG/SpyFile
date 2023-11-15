using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200014F RID: 335
public class MultiTriggerAdapter : MonoBehaviour
{
	// Token: 0x040004F9 RID: 1273
	[SerializeField]
	public List<MultiTriggerAdapter.Adapter> m_adapters = new List<MultiTriggerAdapter.Adapter>();

	// Token: 0x02000150 RID: 336
	[Serializable]
	public class Adapter
	{
		// Token: 0x040004FA RID: 1274
		[SerializeField]
		public string m_inputTrigger;

		// Token: 0x040004FB RID: 1275
		[SerializeField]
		public string m_outputTrigger;
	}
}
