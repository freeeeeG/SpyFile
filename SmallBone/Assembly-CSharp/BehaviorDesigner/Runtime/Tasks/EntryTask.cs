using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014DB RID: 5339
	[TaskIcon("{SkinColor}EntryIcon.png")]
	public class EntryTask : ParentTask
	{
		// Token: 0x060067D0 RID: 26576 RVA: 0x000076D4 File Offset: 0x000058D4
		public override int MaxChildren()
		{
			return 1;
		}
	}
}
