using System;
using UnityEngine;

// Token: 0x02000124 RID: 292
[AddComponentMenu("Scripts/Core/Components/AnimatorTriggerReceiver")]
public class AnimatorTriggerReceiver : MonoBehaviour, ITriggerReceiver
{
	// Token: 0x06000561 RID: 1377 RVA: 0x0002A176 File Offset: 0x00028576
	public void OnTrigger(string _name)
	{
		if (base.gameObject)
		{
			base.gameObject.SendTrigger(_name);
		}
	}
}
