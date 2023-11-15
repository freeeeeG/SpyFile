using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000349 RID: 841
	public struct VersionInfo : IComparable<VersionInfo>
	{
		// Token: 0x06001018 RID: 4120 RVA: 0x0005D11C File Offset: 0x0005B51C
		public VersionInfo(int major, int minor, int patch, int build)
		{
			this.Major = major;
			this.Minor = minor;
			this.Patch = patch;
			this.Build = build;
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0005D13C File Offset: 0x0005B53C
		public static VersionInfo InControlVersion()
		{
			return new VersionInfo
			{
				Major = 1,
				Minor = 5,
				Patch = 12,
				Build = 6556
			};
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0005D178 File Offset: 0x0005B578
		internal static VersionInfo UnityVersion()
		{
			Match match = Regex.Match(Application.unityVersion, "^(\\d+)\\.(\\d+)\\.(\\d+)");
			int build = 0;
			return new VersionInfo
			{
				Major = Convert.ToInt32(match.Groups[1].Value),
				Minor = Convert.ToInt32(match.Groups[2].Value),
				Patch = Convert.ToInt32(match.Groups[3].Value),
				Build = build
			};
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x0005D200 File Offset: 0x0005B600
		public int CompareTo(VersionInfo other)
		{
			if (this.Major < other.Major)
			{
				return -1;
			}
			if (this.Major > other.Major)
			{
				return 1;
			}
			if (this.Minor < other.Minor)
			{
				return -1;
			}
			if (this.Minor > other.Minor)
			{
				return 1;
			}
			if (this.Patch < other.Patch)
			{
				return -1;
			}
			if (this.Patch > other.Patch)
			{
				return 1;
			}
			if (this.Build < other.Build)
			{
				return -1;
			}
			if (this.Build > other.Build)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x0005D2AE File Offset: 0x0005B6AE
		public static bool operator ==(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) == 0;
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0005D2BB File Offset: 0x0005B6BB
		public static bool operator !=(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) != 0;
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x0005D2CB File Offset: 0x0005B6CB
		public static bool operator <=(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) <= 0;
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x0005D2DB File Offset: 0x0005B6DB
		public static bool operator >=(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) >= 0;
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x0005D2EB File Offset: 0x0005B6EB
		public static bool operator <(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) < 0;
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0005D2F8 File Offset: 0x0005B6F8
		public static bool operator >(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) > 0;
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0005D305 File Offset: 0x0005B705
		public override bool Equals(object other)
		{
			return other is VersionInfo && this == (VersionInfo)other;
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x0005D328 File Offset: 0x0005B728
		public override int GetHashCode()
		{
			return this.Major.GetHashCode() ^ this.Minor.GetHashCode() ^ this.Patch.GetHashCode() ^ this.Build.GetHashCode();
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x0005D37C File Offset: 0x0005B77C
		public override string ToString()
		{
			if (this.Build == 0)
			{
				return string.Format("{0}.{1}.{2}", this.Major, this.Minor, this.Patch);
			}
			return string.Format("{0}.{1}.{2} build {3}", new object[]
			{
				this.Major,
				this.Minor,
				this.Patch,
				this.Build
			});
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0005D408 File Offset: 0x0005B808
		public string ToShortString()
		{
			if (this.Build == 0)
			{
				return string.Format("{0}.{1}.{2}", this.Major, this.Minor, this.Patch);
			}
			return string.Format("{0}.{1}.{2}b{3}", new object[]
			{
				this.Major,
				this.Minor,
				this.Patch,
				this.Build
			});
		}

		// Token: 0x04000C1E RID: 3102
		public int Major;

		// Token: 0x04000C1F RID: 3103
		public int Minor;

		// Token: 0x04000C20 RID: 3104
		public int Patch;

		// Token: 0x04000C21 RID: 3105
		public int Build;
	}
}
