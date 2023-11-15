﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Klei;
using UnityEngine;

namespace KMod
{
	// Token: 0x02000D7A RID: 3450
	internal struct Directory : IFileSource
	{
		// Token: 0x06006B72 RID: 27506 RVA: 0x002A0A64 File Offset: 0x0029EC64
		public Directory(string root)
		{
			this.root = root;
			this.file_system = new AliasDirectory(root, root, Application.streamingAssetsPath, true);
		}

		// Token: 0x06006B73 RID: 27507 RVA: 0x002A0A80 File Offset: 0x0029EC80
		public string GetRoot()
		{
			return this.root;
		}

		// Token: 0x06006B74 RID: 27508 RVA: 0x002A0A88 File Offset: 0x0029EC88
		public bool Exists()
		{
			return Directory.Exists(this.GetRoot());
		}

		// Token: 0x06006B75 RID: 27509 RVA: 0x002A0A95 File Offset: 0x0029EC95
		public bool Exists(string relative_path)
		{
			return this.Exists() && new DirectoryInfo(FileSystem.Normalize(Path.Combine(this.root, relative_path))).Exists;
		}

		// Token: 0x06006B76 RID: 27510 RVA: 0x002A0ABC File Offset: 0x0029ECBC
		public void GetTopLevelItems(List<FileSystemItem> file_system_items, string relative_root)
		{
			relative_root = (relative_root ?? "");
			string text = FileSystem.Normalize(Path.Combine(this.root, relative_root));
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			if (!directoryInfo.Exists)
			{
				global::Debug.LogError("Cannot iterate over $" + text + ", this directory does not exist");
				return;
			}
			foreach (FileSystemInfo fileSystemInfo in directoryInfo.GetFileSystemInfos())
			{
				file_system_items.Add(new FileSystemItem
				{
					name = fileSystemInfo.Name,
					type = ((fileSystemInfo is DirectoryInfo) ? FileSystemItem.ItemType.Directory : FileSystemItem.ItemType.File)
				});
			}
		}

		// Token: 0x06006B77 RID: 27511 RVA: 0x002A0B58 File Offset: 0x0029ED58
		public IFileDirectory GetFileSystem()
		{
			return this.file_system;
		}

		// Token: 0x06006B78 RID: 27512 RVA: 0x002A0B60 File Offset: 0x0029ED60
		public void CopyTo(string path, List<string> extensions = null)
		{
			try
			{
				Directory.CopyDirectory(this.root, path, extensions);
			}
			catch (UnauthorizedAccessException)
			{
				FileUtil.ErrorDialog(FileUtil.ErrorType.UnauthorizedAccess, path, null, null);
			}
			catch (IOException)
			{
				FileUtil.ErrorDialog(FileUtil.ErrorType.IOError, path, null, null);
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x06006B79 RID: 27513 RVA: 0x002A0BC0 File Offset: 0x0029EDC0
		public string Read(string relative_path)
		{
			string result;
			try
			{
				using (FileStream fileStream = File.OpenRead(Path.Combine(this.root, relative_path)))
				{
					byte[] array = new byte[fileStream.Length];
					fileStream.Read(array, 0, (int)fileStream.Length);
					result = Encoding.UTF8.GetString(array);
				}
			}
			catch
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x06006B7A RID: 27514 RVA: 0x002A0C3C File Offset: 0x0029EE3C
		private static int CopyDirectory(string sourceDirName, string destDirName, List<string> extensions)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);
			if (!directoryInfo.Exists)
			{
				return 0;
			}
			if (!FileUtil.CreateDirectory(destDirName, 0))
			{
				return 0;
			}
			FileInfo[] files = directoryInfo.GetFiles();
			int num = 0;
			foreach (FileInfo fileInfo in files)
			{
				bool flag = extensions == null || extensions.Count == 0;
				if (extensions != null)
				{
					using (List<string>.Enumerator enumerator = extensions.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current == Path.GetExtension(fileInfo.Name).ToLower())
							{
								flag = true;
								break;
							}
						}
					}
				}
				if (flag)
				{
					string destFileName = Path.Combine(destDirName, fileInfo.Name);
					fileInfo.CopyTo(destFileName, false);
					num++;
				}
			}
			foreach (DirectoryInfo directoryInfo2 in directoryInfo.GetDirectories())
			{
				string destDirName2 = Path.Combine(destDirName, directoryInfo2.Name);
				num += Directory.CopyDirectory(directoryInfo2.FullName, destDirName2, extensions);
			}
			if (num == 0)
			{
				FileUtil.DeleteDirectory(destDirName, 0);
			}
			return num;
		}

		// Token: 0x06006B7B RID: 27515 RVA: 0x002A0D60 File Offset: 0x0029EF60
		public void Dispose()
		{
		}

		// Token: 0x04004EB0 RID: 20144
		private AliasDirectory file_system;

		// Token: 0x04004EB1 RID: 20145
		private string root;
	}
}
