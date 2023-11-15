using System;

namespace flanne
{
	// Token: 0x020000FA RID: 250
	public static class PointsTracker
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x0001FC11 File Offset: 0x0001DE11
		// (set) Token: 0x06000740 RID: 1856 RVA: 0x0001FC18 File Offset: 0x0001DE18
		public static int pts
		{
			get
			{
				return PointsTracker._pts;
			}
			set
			{
				PointsTracker._pts = value;
				EventHandler<int> pointsChangedEvent = PointsTracker.PointsChangedEvent;
				if (pointsChangedEvent == null)
				{
					return;
				}
				pointsChangedEvent(typeof(PointsTracker), PointsTracker._pts);
			}
		}

		// Token: 0x04000515 RID: 1301
		public static EventHandler<int> PointsChangedEvent;

		// Token: 0x04000516 RID: 1302
		private static int _pts;
	}
}
