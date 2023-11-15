using System;
using System.Diagnostics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000A0 RID: 160
	public class Profile
	{
		// Token: 0x06000761 RID: 1889 RVA: 0x0002D1B7 File Offset: 0x0002B3B7
		public int ControlValue()
		{
			return this.control;
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0002D1BF File Offset: 0x0002B3BF
		public Profile(string name)
		{
			this.name = name;
			this.watch = new Stopwatch();
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0002D1E4 File Offset: 0x0002B3E4
		public static void WriteCSV(string path, params Profile[] profiles)
		{
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0002D1E6 File Offset: 0x0002B3E6
		public void Run(Action action)
		{
			action();
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0002D1EE File Offset: 0x0002B3EE
		[Conditional("PROFILE")]
		public void Start()
		{
			this.watch.Start();
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0002D1FB File Offset: 0x0002B3FB
		[Conditional("PROFILE")]
		public void Stop()
		{
			this.counter++;
			this.watch.Stop();
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0002D216 File Offset: 0x0002B416
		[Conditional("PROFILE")]
		public void Log()
		{
			Debug.Log(this.ToString());
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0002D223 File Offset: 0x0002B423
		[Conditional("PROFILE")]
		public void ConsoleLog()
		{
			Console.WriteLine(this.ToString());
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0002D230 File Offset: 0x0002B430
		[Conditional("PROFILE")]
		public void Stop(int control)
		{
			this.counter++;
			this.watch.Stop();
			if (this.control == 1073741824)
			{
				this.control = control;
				return;
			}
			if (this.control != control)
			{
				throw new Exception("Control numbers do not match " + this.control.ToString() + " != " + control.ToString());
			}
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0002D29C File Offset: 0x0002B49C
		[Conditional("PROFILE")]
		public void Control(Profile other)
		{
			if (this.ControlValue() != other.ControlValue())
			{
				throw new Exception(string.Concat(new string[]
				{
					"Control numbers do not match (",
					this.name,
					" ",
					other.name,
					") ",
					this.ControlValue().ToString(),
					" != ",
					other.ControlValue().ToString()
				}));
			}
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0002D31C File Offset: 0x0002B51C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.name,
				" #",
				this.counter.ToString(),
				" ",
				this.watch.Elapsed.TotalMilliseconds.ToString("0.0 ms"),
				" avg: ",
				(this.watch.Elapsed.TotalMilliseconds / (double)this.counter).ToString("0.00 ms")
			});
		}

		// Token: 0x04000431 RID: 1073
		private const bool PROFILE_MEM = false;

		// Token: 0x04000432 RID: 1074
		public readonly string name;

		// Token: 0x04000433 RID: 1075
		private readonly Stopwatch watch;

		// Token: 0x04000434 RID: 1076
		private int counter;

		// Token: 0x04000435 RID: 1077
		private long mem;

		// Token: 0x04000436 RID: 1078
		private long smem;

		// Token: 0x04000437 RID: 1079
		private int control = 1073741824;

		// Token: 0x04000438 RID: 1080
		private const bool dontCountFirst = false;
	}
}
