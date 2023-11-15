using System;
using UnityEngine;

// Token: 0x02000115 RID: 277
public class StateMachineBehaviourEx : StateMachineBehaviour
{
	// Token: 0x06000518 RID: 1304 RVA: 0x00028BB0 File Offset: 0x00026FB0
	public StateMachineBehaviourEx GetInstance(Animator _animator)
	{
		StateMachineBehaviourEx[] behaviours = _animator.GetBehaviours<StateMachineBehaviourEx>();
		for (int i = 0; i < behaviours.Length; i++)
		{
			if (behaviours[i].SharedBehaviour == this.SharedBehaviour)
			{
				return behaviours[i];
			}
		}
		return null;
	}

	// Token: 0x0400047C RID: 1148
	[SelfAssignID(Visibility.Show)]
	public int SharedBehaviour;
}
