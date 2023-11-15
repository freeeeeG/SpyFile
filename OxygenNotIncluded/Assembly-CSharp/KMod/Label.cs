using System;
using System.Diagnostics;
using System.IO;
using Klei;
using Newtonsoft.Json;

namespace KMod
{
	// Token: 0x02000D7C RID: 3452
	[JsonObject(MemberSerialization.Fields)]
	[DebuggerDisplay("{title}")]
	public struct Label
	{
		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06006B86 RID: 27526 RVA: 0x002A1191 File Offset: 0x0029F391
		[JsonIgnore]
		private string distribution_platform_name
		{
			get
			{
				return this.distribution_platform.ToString();
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06006B87 RID: 27527 RVA: 0x002A11A4 File Offset: 0x0029F3A4
		[JsonIgnore]
		public string install_path
		{
			get
			{
				return FileSystem.Normalize(Path.Combine(Manager.GetDirectory(), this.distribution_platform_name, this.id));
			}
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06006B88 RID: 27528 RVA: 0x002A11C1 File Offset: 0x0029F3C1
		[JsonIgnore]
		public string defaultStaticID
		{
			get
			{
				return this.id + "." + this.distribution_platform.ToString();
			}
		}

		// Token: 0x06006B89 RID: 27529 RVA: 0x002A11E4 File Offset: 0x0029F3E4
		public override string ToString()
		{
			return this.title;
		}

		// Token: 0x06006B8A RID: 27530 RVA: 0x002A11EC File Offset: 0x0029F3EC
		public bool Match(Label rhs)
		{
			return this.id == rhs.id && this.distribution_platform == rhs.distribution_platform;
		}

		// Token: 0x04004EB5 RID: 20149
		public Label.DistributionPlatform distribution_platform;

		// Token: 0x04004EB6 RID: 20150
		public string id;

		// Token: 0x04004EB7 RID: 20151
		public string title;

		// Token: 0x04004EB8 RID: 20152
		public long version;

		// Token: 0x02001C4F RID: 7247
		public enum DistributionPlatform
		{
			// Token: 0x04008068 RID: 32872
			Local,
			// Token: 0x04008069 RID: 32873
			Steam,
			// Token: 0x0400806A RID: 32874
			Epic,
			// Token: 0x0400806B RID: 32875
			Rail,
			// Token: 0x0400806C RID: 32876
			Dev
		}
	}
}
