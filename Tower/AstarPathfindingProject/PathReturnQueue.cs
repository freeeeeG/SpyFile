using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000046 RID: 70
	internal class PathReturnQueue
	{
		// Token: 0x06000352 RID: 850 RVA: 0x00012AD6 File Offset: 0x00010CD6
		public PathReturnQueue(object pathsClaimedSilentlyBy)
		{
			this.pathsClaimedSilentlyBy = pathsClaimedSilentlyBy;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00012AF0 File Offset: 0x00010CF0
		public void Enqueue(Path path)
		{
			Queue<Path> obj = this.pathReturnQueue;
			lock (obj)
			{
				this.pathReturnQueue.Enqueue(path);
			}
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00012B38 File Offset: 0x00010D38
		public void ReturnPaths(bool timeSlice)
		{
			long num = timeSlice ? (DateTime.UtcNow.Ticks + 10000L) : 0L;
			int num2 = 0;
			for (;;)
			{
				Queue<Path> obj = this.pathReturnQueue;
				Path path;
				lock (obj)
				{
					if (this.pathReturnQueue.Count == 0)
					{
						break;
					}
					path = this.pathReturnQueue.Dequeue();
				}
				try
				{
					((IPathInternals)path).ReturnPath();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
				((IPathInternals)path).AdvanceState(PathState.Returned);
				path.Release(this.pathsClaimedSilentlyBy, true);
				num2++;
				if (num2 > 5 && timeSlice)
				{
					num2 = 0;
					if (DateTime.UtcNow.Ticks >= num)
					{
						break;
					}
				}
			}
		}

		// Token: 0x04000219 RID: 537
		private Queue<Path> pathReturnQueue = new Queue<Path>();

		// Token: 0x0400021A RID: 538
		private object pathsClaimedSilentlyBy;
	}
}
