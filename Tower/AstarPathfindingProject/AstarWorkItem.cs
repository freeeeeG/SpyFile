using System;

namespace Pathfinding
{
	// Token: 0x02000048 RID: 72
	public struct AstarWorkItem
	{
		// Token: 0x06000364 RID: 868 RVA: 0x000131D8 File Offset: 0x000113D8
		public AstarWorkItem(Func<bool, bool> update)
		{
			this.init = null;
			this.initWithContext = null;
			this.updateWithContext = null;
			this.update = update;
		}

		// Token: 0x06000365 RID: 869 RVA: 0x000131F6 File Offset: 0x000113F6
		public AstarWorkItem(Func<IWorkItemContext, bool, bool> update)
		{
			this.init = null;
			this.initWithContext = null;
			this.updateWithContext = update;
			this.update = null;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00013214 File Offset: 0x00011414
		public AstarWorkItem(Action init, Func<bool, bool> update = null)
		{
			this.init = init;
			this.initWithContext = null;
			this.update = update;
			this.updateWithContext = null;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00013232 File Offset: 0x00011432
		public AstarWorkItem(Action<IWorkItemContext> init, Func<IWorkItemContext, bool, bool> update = null)
		{
			this.init = null;
			this.initWithContext = init;
			this.update = null;
			this.updateWithContext = update;
		}

		// Token: 0x04000224 RID: 548
		public Action init;

		// Token: 0x04000225 RID: 549
		public Action<IWorkItemContext> initWithContext;

		// Token: 0x04000226 RID: 550
		public Func<bool, bool> update;

		// Token: 0x04000227 RID: 551
		public Func<IWorkItemContext, bool, bool> updateWithContext;
	}
}
