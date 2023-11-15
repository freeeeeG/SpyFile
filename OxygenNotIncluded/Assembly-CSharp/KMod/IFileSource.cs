using System;
using System.Collections.Generic;
using Klei;

namespace KMod
{
	// Token: 0x02000D79 RID: 3449
	public interface IFileSource
	{
		// Token: 0x06006B6A RID: 27498
		string GetRoot();

		// Token: 0x06006B6B RID: 27499
		bool Exists();

		// Token: 0x06006B6C RID: 27500
		bool Exists(string relative_path);

		// Token: 0x06006B6D RID: 27501
		void GetTopLevelItems(List<FileSystemItem> file_system_items, string relative_root = "");

		// Token: 0x06006B6E RID: 27502
		IFileDirectory GetFileSystem();

		// Token: 0x06006B6F RID: 27503
		void CopyTo(string path, List<string> extensions = null);

		// Token: 0x06006B70 RID: 27504
		string Read(string relative_path);

		// Token: 0x06006B71 RID: 27505
		void Dispose();
	}
}
