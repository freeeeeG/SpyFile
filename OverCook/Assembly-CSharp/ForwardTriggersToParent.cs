using System;
using UnityEngine;

// Token: 0x02000135 RID: 309
[AddComponentMenu("Scripts/Core/Components/ForwardTriggersToParent")]
public class ForwardTriggersToParent : MonoBehaviour, ITriggerReceiver
{
	// Token: 0x060005A5 RID: 1445 RVA: 0x0002AA8E File Offset: 0x00028E8E
	public void OnTrigger(string _name)
	{
		if (base.gameObject.transform.parent)
		{
			base.gameObject.transform.parent.gameObject.SendTrigger(_name);
		}
	}
}
