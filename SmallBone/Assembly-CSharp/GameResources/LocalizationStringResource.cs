using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameResources
{
	// Token: 0x02000187 RID: 391
	[PreferBinarySerialization]
	public class LocalizationStringResource : ScriptableObject
	{
		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600087D RID: 2173 RVA: 0x00018B3E File Offset: 0x00016D3E
		// (set) Token: 0x0600087E RID: 2174 RVA: 0x00018B45 File Offset: 0x00016D45
		public static LocalizationStringResource instance { get; private set; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x0600087F RID: 2175 RVA: 0x00018B4D File Offset: 0x00016D4D
		public int columnCount
		{
			get
			{
				return this._columnCount;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000880 RID: 2176 RVA: 0x00018B55 File Offset: 0x00016D55
		public int rowCount
		{
			get
			{
				return this._rowCount;
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000881 RID: 2177 RVA: 0x00018B5D File Offset: 0x00016D5D
		public LocalizationStringResource.StringsByLanguage[] stringsByLanguage
		{
			get
			{
				return this._stringsByLanguage;
			}
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00018B65 File Offset: 0x00016D65
		public void Initialize()
		{
			LocalizationStringResource.instance = this;
			base.hideFlags |= HideFlags.DontUnloadUnusedAsset;
			this._emptyStrings = new string[this._columnCount];
			this.CreateHashToIndexDictionary();
			Localization.Initialize();
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00018B98 File Offset: 0x00016D98
		public void CreateHashToIndexDictionary()
		{
			this._keyHashToIndex = new Dictionary<int, int>(this._rowCount);
			for (int i = 0; i < this._rowCount; i++)
			{
				this._keyHashToIndex.Add(this._keyHashes[i], i);
			}
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x00018BDC File Offset: 0x00016DDC
		public bool TryGetStrings(int keyHash, out string[] result)
		{
			int num;
			if (!this._keyHashToIndex.TryGetValue(keyHash, out num))
			{
				Debug.LogWarning(string.Format("[{0}] There is no hash : {1}.", "LocalizationStringResource", keyHash));
				result = this._emptyStrings;
				return false;
			}
			result = this.stringsByLanguage[num].strings;
			return true;
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x00018C34 File Offset: 0x00016E34
		public string[] GetStrings(int keyHash)
		{
			string[] result;
			this.TryGetStrings(keyHash, out result);
			return result;
		}

		// Token: 0x040006CD RID: 1741
		[SerializeField]
		private int _columnCount;

		// Token: 0x040006CE RID: 1742
		[SerializeField]
		private int _rowCount;

		// Token: 0x040006CF RID: 1743
		[SerializeField]
		[HideInInspector]
		private int[] _keyHashes;

		// Token: 0x040006D0 RID: 1744
		[SerializeField]
		[HideInInspector]
		private LocalizationStringResource.StringsByLanguage[] _stringsByLanguage;

		// Token: 0x040006D1 RID: 1745
		private Dictionary<int, int> _keyHashToIndex;

		// Token: 0x040006D2 RID: 1746
		private string[] _emptyStrings;

		// Token: 0x02000188 RID: 392
		[Serializable]
		public struct StringsByLanguage
		{
			// Token: 0x040006D3 RID: 1747
			public string[] strings;
		}
	}
}
