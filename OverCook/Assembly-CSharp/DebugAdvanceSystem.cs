using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020001B7 RID: 439
public static class DebugAdvanceSystem
{
	// Token: 0x06000775 RID: 1909 RVA: 0x0002F464 File Offset: 0x0002D864
	public static void Add(Func<bool> completed, Action continueWith)
	{
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x0002F466 File Offset: 0x0002D866
	public static void Add(IEnumerator _coroutine)
	{
	}

	// Token: 0x0400060C RID: 1548
	private static readonly List<DebugAdvanceSystem.Job> jobs = new List<DebugAdvanceSystem.Job>();

	// Token: 0x0400060D RID: 1549
	private static readonly List<IEnumerator> coroutines = new List<IEnumerator>();

	// Token: 0x020001B8 RID: 440
	private class Job
	{
		// Token: 0x06000778 RID: 1912 RVA: 0x0002F47E File Offset: 0x0002D87E
		public Job(Func<bool> completed, Action continueWith)
		{
			this.Completed = completed;
			this.ContinueWith = continueWith;
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x0002F494 File Offset: 0x0002D894
		// (set) Token: 0x0600077A RID: 1914 RVA: 0x0002F49C File Offset: 0x0002D89C
		public Func<bool> Completed { get; private set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x0002F4A5 File Offset: 0x0002D8A5
		// (set) Token: 0x0600077C RID: 1916 RVA: 0x0002F4AD File Offset: 0x0002D8AD
		public Action ContinueWith { get; private set; }
	}
}
