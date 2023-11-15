using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000D64 RID: 3428
	public class RunReactorForXDays : ColonyAchievementRequirement
	{
		// Token: 0x06006B26 RID: 27430 RVA: 0x0029B264 File Offset: 0x00299464
		public RunReactorForXDays(int numCycles)
		{
			this.numCycles = numCycles;
		}

		// Token: 0x06006B27 RID: 27431 RVA: 0x0029B274 File Offset: 0x00299474
		public override string GetProgress(bool complete)
		{
			int num = 0;
			foreach (Reactor reactor in Components.NuclearReactors.Items)
			{
				if (reactor.numCyclesRunning > num)
				{
					num = reactor.numCyclesRunning;
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.RUN_A_REACTOR, complete ? this.numCycles : num, this.numCycles);
		}

		// Token: 0x06006B28 RID: 27432 RVA: 0x0029B304 File Offset: 0x00299504
		public override bool Success()
		{
			using (List<Reactor>.Enumerator enumerator = Components.NuclearReactors.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.numCyclesRunning >= this.numCycles)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04004DCB RID: 19915
		private int numCycles;
	}
}
