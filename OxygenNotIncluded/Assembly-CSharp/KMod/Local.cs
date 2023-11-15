using System;
using System.IO;
using Klei;
using STRINGS;

namespace KMod
{
	// Token: 0x02000D76 RID: 3446
	public class Local : IDistributionPlatform
	{
		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06006B60 RID: 27488 RVA: 0x002A05E2 File Offset: 0x0029E7E2
		// (set) Token: 0x06006B61 RID: 27489 RVA: 0x002A05EA File Offset: 0x0029E7EA
		public string folder { get; private set; }

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06006B62 RID: 27490 RVA: 0x002A05F3 File Offset: 0x0029E7F3
		// (set) Token: 0x06006B63 RID: 27491 RVA: 0x002A05FB File Offset: 0x0029E7FB
		public Label.DistributionPlatform distribution_platform { get; private set; }

		// Token: 0x06006B64 RID: 27492 RVA: 0x002A0604 File Offset: 0x0029E804
		public string GetDirectory()
		{
			return FileSystem.Normalize(Path.Combine(Manager.GetDirectory(), this.folder));
		}

		// Token: 0x06006B65 RID: 27493 RVA: 0x002A061C File Offset: 0x0029E81C
		private void Subscribe(string directoryName, long timestamp, IFileSource file_source, bool isDevMod)
		{
			Label label = new Label
			{
				id = directoryName,
				distribution_platform = this.distribution_platform,
				version = (long)directoryName.GetHashCode(),
				title = directoryName
			};
			KModHeader header = KModUtil.GetHeader(file_source, label.defaultStaticID, directoryName, directoryName, isDevMod);
			label.title = header.title;
			Mod mod = new Mod(label, header.staticID, header.description, file_source, UI.FRONTEND.MODS.TOOLTIPS.MANAGE_LOCAL_MOD, delegate()
			{
				App.OpenWebURL("file://" + file_source.GetRoot());
			});
			if (file_source.GetType() == typeof(Directory))
			{
				mod.status = Mod.Status.Installed;
			}
			Global.Instance.modManager.Subscribe(mod, this);
		}

		// Token: 0x06006B66 RID: 27494 RVA: 0x002A06F0 File Offset: 0x0029E8F0
		public Local(string folder, Label.DistributionPlatform distribution_platform, bool isDevFolder)
		{
			this.folder = folder;
			this.distribution_platform = distribution_platform;
			DirectoryInfo directoryInfo = new DirectoryInfo(this.GetDirectory());
			if (!directoryInfo.Exists)
			{
				return;
			}
			foreach (DirectoryInfo directoryInfo2 in directoryInfo.GetDirectories())
			{
				string name = directoryInfo2.Name;
				this.Subscribe(name, directoryInfo2.LastWriteTime.ToFileTime(), new Directory(directoryInfo2.FullName), isDevFolder);
			}
		}
	}
}
