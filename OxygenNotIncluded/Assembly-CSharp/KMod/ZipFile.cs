using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;
using Klei;
using UnityEngine;

namespace KMod
{
	// Token: 0x02000D7B RID: 3451
	internal struct ZipFile : IFileSource
	{
		// Token: 0x06006B7C RID: 27516 RVA: 0x002A0D62 File Offset: 0x0029EF62
		public ZipFile(string filename)
		{
			this.filename = filename;
			this.zipfile = ZipFile.Read(filename);
			this.file_system = new ZipFileDirectory(this.zipfile.Name, this.zipfile, Application.streamingAssetsPath, true);
		}

		// Token: 0x06006B7D RID: 27517 RVA: 0x002A0D99 File Offset: 0x0029EF99
		public string GetRoot()
		{
			return this.filename;
		}

		// Token: 0x06006B7E RID: 27518 RVA: 0x002A0DA1 File Offset: 0x0029EFA1
		public bool Exists()
		{
			return File.Exists(this.GetRoot());
		}

		// Token: 0x06006B7F RID: 27519 RVA: 0x002A0DB0 File Offset: 0x0029EFB0
		public bool Exists(string relative_path)
		{
			if (!this.Exists())
			{
				return false;
			}
			using (IEnumerator<ZipEntry> enumerator = this.zipfile.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (FileSystem.Normalize(enumerator.Current.FileName).StartsWith(relative_path))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006B80 RID: 27520 RVA: 0x002A0E18 File Offset: 0x0029F018
		public void GetTopLevelItems(List<FileSystemItem> file_system_items, string relative_root)
		{
			HashSetPool<string, ZipFile>.PooledHashSet pooledHashSet = HashSetPool<string, ZipFile>.Allocate();
			string[] array;
			if (!string.IsNullOrEmpty(relative_root))
			{
				relative_root = (relative_root ?? "");
				relative_root = FileSystem.Normalize(relative_root);
				array = relative_root.Split(new char[]
				{
					'/'
				});
			}
			else
			{
				array = new string[0];
			}
			foreach (ZipEntry zipEntry in this.zipfile)
			{
				List<string> list = (from part in FileSystem.Normalize(zipEntry.FileName).Split(new char[]
				{
					'/'
				})
				where !string.IsNullOrEmpty(part)
				select part).ToList<string>();
				if (this.IsSharedRoot(array, list))
				{
					list = list.GetRange(array.Length, list.Count - array.Length);
					if (list.Count != 0)
					{
						string text = list[0];
						if (pooledHashSet.Add(text))
						{
							file_system_items.Add(new FileSystemItem
							{
								name = text,
								type = ((1 < list.Count) ? FileSystemItem.ItemType.Directory : FileSystemItem.ItemType.File)
							});
						}
					}
				}
			}
			pooledHashSet.Recycle();
		}

		// Token: 0x06006B81 RID: 27521 RVA: 0x002A0F50 File Offset: 0x0029F150
		private bool IsSharedRoot(string[] root_path, List<string> check_path)
		{
			for (int i = 0; i < root_path.Length; i++)
			{
				if (i >= check_path.Count || root_path[i] != check_path[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06006B82 RID: 27522 RVA: 0x002A0F88 File Offset: 0x0029F188
		public IFileDirectory GetFileSystem()
		{
			return this.file_system;
		}

		// Token: 0x06006B83 RID: 27523 RVA: 0x002A0F90 File Offset: 0x0029F190
		public void CopyTo(string path, List<string> extensions = null)
		{
			foreach (ZipEntry zipEntry in this.zipfile.Entries)
			{
				bool flag = extensions == null || extensions.Count == 0;
				if (extensions != null)
				{
					foreach (string value in extensions)
					{
						if (zipEntry.FileName.ToLower().EndsWith(value))
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					string path2 = FileSystem.Normalize(Path.Combine(path, zipEntry.FileName));
					string directoryName = Path.GetDirectoryName(path2);
					if (string.IsNullOrEmpty(directoryName) || FileUtil.CreateDirectory(directoryName, 0))
					{
						using (MemoryStream memoryStream = new MemoryStream((int)zipEntry.UncompressedSize))
						{
							zipEntry.Extract(memoryStream);
							using (FileStream fileStream = FileUtil.Create(path2, 0))
							{
								fileStream.Write(memoryStream.GetBuffer(), 0, memoryStream.GetBuffer().Length);
							}
						}
					}
				}
			}
		}

		// Token: 0x06006B84 RID: 27524 RVA: 0x002A10E0 File Offset: 0x0029F2E0
		public string Read(string relative_path)
		{
			ICollection<ZipEntry> collection = this.zipfile.SelectEntries(relative_path);
			if (collection.Count == 0)
			{
				return string.Empty;
			}
			foreach (ZipEntry zipEntry in collection)
			{
				using (MemoryStream memoryStream = new MemoryStream((int)zipEntry.UncompressedSize))
				{
					zipEntry.Extract(memoryStream);
					return Encoding.UTF8.GetString(memoryStream.GetBuffer());
				}
			}
			return string.Empty;
		}

		// Token: 0x06006B85 RID: 27525 RVA: 0x002A1184 File Offset: 0x0029F384
		public void Dispose()
		{
			this.zipfile.Dispose();
		}

		// Token: 0x04004EB2 RID: 20146
		private string filename;

		// Token: 0x04004EB3 RID: 20147
		private ZipFile zipfile;

		// Token: 0x04004EB4 RID: 20148
		private ZipFileDirectory file_system;
	}
}
