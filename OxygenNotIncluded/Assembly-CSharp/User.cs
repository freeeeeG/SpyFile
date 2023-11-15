using System;
using UnityEngine;

// Token: 0x020003DE RID: 990
[AddComponentMenu("KMonoBehaviour/scripts/User")]
public class User : KMonoBehaviour
{
	// Token: 0x060014D0 RID: 5328 RVA: 0x0006E1D7 File Offset: 0x0006C3D7
	public void OnStateMachineStop(string reason, StateMachine.Status status)
	{
		if (status == StateMachine.Status.Success)
		{
			base.Trigger(58624316, null);
			return;
		}
		base.Trigger(1572098533, null);
	}
}
