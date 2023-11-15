using System;
using BehaviorDesigner.Runtime;
using Characters;

namespace Runnables
{
	// Token: 0x0200031E RID: 798
	[Serializable]
	public class BehaviorDesignerInterrupt : IStatusEvent
	{
		// Token: 0x06000F66 RID: 3942 RVA: 0x0002EF48 File Offset: 0x0002D148
		void IStatusEvent.Apply(Character owner, Character target)
		{
			BehaviorTree component = target.GetComponent<BehaviorTree>();
			if (component != null)
			{
				component.DisableBehavior();
			}
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x0002EF6C File Offset: 0x0002D16C
		void IStatusEvent.Release(Character owner, Character target)
		{
			BehaviorTree component = target.GetComponent<BehaviorTree>();
			if (component != null)
			{
				component.EnableBehavior();
			}
		}
	}
}
