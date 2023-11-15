using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200009A RID: 154
	public class AstarProfiler
	{
		// Token: 0x0600072C RID: 1836 RVA: 0x0002B103 File Offset: 0x00029303
		private AstarProfiler()
		{
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0002B10C File Offset: 0x0002930C
		[Conditional("ProfileAstar")]
		public static void InitializeFastProfile(string[] profileNames)
		{
			AstarProfiler.fastProfileNames = new string[profileNames.Length + 2];
			Array.Copy(profileNames, AstarProfiler.fastProfileNames, profileNames.Length);
			AstarProfiler.fastProfileNames[AstarProfiler.fastProfileNames.Length - 2] = "__Control1__";
			AstarProfiler.fastProfileNames[AstarProfiler.fastProfileNames.Length - 1] = "__Control2__";
			AstarProfiler.fastProfiles = new AstarProfiler.ProfilePoint[AstarProfiler.fastProfileNames.Length];
			for (int i = 0; i < AstarProfiler.fastProfiles.Length; i++)
			{
				AstarProfiler.fastProfiles[i] = new AstarProfiler.ProfilePoint();
			}
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0002B18D File Offset: 0x0002938D
		[Conditional("ProfileAstar")]
		public static void StartFastProfile(int tag)
		{
			AstarProfiler.fastProfiles[tag].watch.Start();
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0002B1A0 File Offset: 0x000293A0
		[Conditional("ProfileAstar")]
		public static void EndFastProfile(int tag)
		{
			AstarProfiler.ProfilePoint profilePoint = AstarProfiler.fastProfiles[tag];
			profilePoint.totalCalls++;
			profilePoint.watch.Stop();
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0002B1C1 File Offset: 0x000293C1
		[Conditional("ASTAR_UNITY_PRO_PROFILER")]
		public static void EndProfile()
		{
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0002B1C4 File Offset: 0x000293C4
		[Conditional("ProfileAstar")]
		public static void StartProfile(string tag)
		{
			AstarProfiler.ProfilePoint profilePoint;
			AstarProfiler.profiles.TryGetValue(tag, out profilePoint);
			if (profilePoint == null)
			{
				profilePoint = new AstarProfiler.ProfilePoint();
				AstarProfiler.profiles[tag] = profilePoint;
			}
			profilePoint.tmpBytes = GC.GetTotalMemory(false);
			profilePoint.watch.Start();
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0002B20C File Offset: 0x0002940C
		[Conditional("ProfileAstar")]
		public static void EndProfile(string tag)
		{
			if (!AstarProfiler.profiles.ContainsKey(tag))
			{
				Debug.LogError("Can only end profiling for a tag which has already been started (tag was " + tag + ")");
				return;
			}
			AstarProfiler.ProfilePoint profilePoint = AstarProfiler.profiles[tag];
			profilePoint.totalCalls++;
			profilePoint.watch.Stop();
			profilePoint.totalBytes += GC.GetTotalMemory(false) - profilePoint.tmpBytes;
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0002B27C File Offset: 0x0002947C
		[Conditional("ProfileAstar")]
		public static void Reset()
		{
			AstarProfiler.profiles.Clear();
			AstarProfiler.startTime = DateTime.UtcNow;
			if (AstarProfiler.fastProfiles != null)
			{
				for (int i = 0; i < AstarProfiler.fastProfiles.Length; i++)
				{
					AstarProfiler.fastProfiles[i] = new AstarProfiler.ProfilePoint();
				}
			}
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0002B2C4 File Offset: 0x000294C4
		[Conditional("ProfileAstar")]
		public static void PrintFastResults()
		{
			if (AstarProfiler.fastProfiles == null)
			{
				return;
			}
			for (int i = 0; i < 1000; i++)
			{
			}
			double num = AstarProfiler.fastProfiles[AstarProfiler.fastProfiles.Length - 2].watch.Elapsed.TotalMilliseconds / 1000.0;
			TimeSpan timeSpan = DateTime.UtcNow - AstarProfiler.startTime;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("============================\n\t\t\t\tProfile results:\n============================\n");
			stringBuilder.Append("Name\t\t|\tTotal Time\t|\tTotal Calls\t|\tAvg/Call\t|\tBytes");
			for (int j = 0; j < AstarProfiler.fastProfiles.Length; j++)
			{
				string text = AstarProfiler.fastProfileNames[j];
				AstarProfiler.ProfilePoint profilePoint = AstarProfiler.fastProfiles[j];
				int totalCalls = profilePoint.totalCalls;
				double num2 = profilePoint.watch.Elapsed.TotalMilliseconds - num * (double)totalCalls;
				if (totalCalls >= 1)
				{
					stringBuilder.Append("\n").Append(text.PadLeft(10)).Append("|   ");
					stringBuilder.Append(num2.ToString("0.0 ").PadLeft(10)).Append(profilePoint.watch.Elapsed.TotalMilliseconds.ToString("(0.0)").PadLeft(10)).Append("|   ");
					stringBuilder.Append(totalCalls.ToString().PadLeft(10)).Append("|   ");
					stringBuilder.Append((num2 / (double)totalCalls).ToString("0.000").PadLeft(10));
				}
			}
			stringBuilder.Append("\n\n============================\n\t\tTotal runtime: ");
			stringBuilder.Append(timeSpan.TotalSeconds.ToString("F3"));
			stringBuilder.Append(" seconds\n============================");
			Debug.Log(stringBuilder.ToString());
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0002B49C File Offset: 0x0002969C
		[Conditional("ProfileAstar")]
		public static void PrintResults()
		{
			TimeSpan timeSpan = DateTime.UtcNow - AstarProfiler.startTime;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("============================\n\t\t\t\tProfile results:\n============================\n");
			int num = 5;
			foreach (KeyValuePair<string, AstarProfiler.ProfilePoint> keyValuePair in AstarProfiler.profiles)
			{
				num = Math.Max(keyValuePair.Key.Length, num);
			}
			stringBuilder.Append(" Name ".PadRight(num)).Append("|").Append(" Total Time\t".PadRight(20)).Append("|").Append(" Total Calls ".PadRight(20)).Append("|").Append(" Avg/Call ".PadRight(20));
			foreach (KeyValuePair<string, AstarProfiler.ProfilePoint> keyValuePair2 in AstarProfiler.profiles)
			{
				double totalMilliseconds = keyValuePair2.Value.watch.Elapsed.TotalMilliseconds;
				int totalCalls = keyValuePair2.Value.totalCalls;
				if (totalCalls >= 1)
				{
					string key = keyValuePair2.Key;
					stringBuilder.Append("\n").Append(key.PadRight(num)).Append("| ");
					stringBuilder.Append(totalMilliseconds.ToString("0.0").PadRight(20)).Append("| ");
					stringBuilder.Append(totalCalls.ToString().PadRight(20)).Append("| ");
					stringBuilder.Append((totalMilliseconds / (double)totalCalls).ToString("0.000").PadRight(20));
					stringBuilder.Append(AstarMath.FormatBytesBinary((int)keyValuePair2.Value.totalBytes).PadLeft(10));
				}
			}
			stringBuilder.Append("\n\n============================\n\t\tTotal runtime: ");
			stringBuilder.Append(timeSpan.TotalSeconds.ToString("F3"));
			stringBuilder.Append(" seconds\n============================");
			Debug.Log(stringBuilder.ToString());
		}

		// Token: 0x0400041B RID: 1051
		private static readonly Dictionary<string, AstarProfiler.ProfilePoint> profiles = new Dictionary<string, AstarProfiler.ProfilePoint>();

		// Token: 0x0400041C RID: 1052
		private static DateTime startTime = DateTime.UtcNow;

		// Token: 0x0400041D RID: 1053
		public static AstarProfiler.ProfilePoint[] fastProfiles;

		// Token: 0x0400041E RID: 1054
		public static string[] fastProfileNames;

		// Token: 0x02000158 RID: 344
		public class ProfilePoint
		{
			// Token: 0x040007D0 RID: 2000
			public Stopwatch watch = new Stopwatch();

			// Token: 0x040007D1 RID: 2001
			public int totalCalls;

			// Token: 0x040007D2 RID: 2002
			public long tmpBytes;

			// Token: 0x040007D3 RID: 2003
			public long totalBytes;
		}
	}
}
