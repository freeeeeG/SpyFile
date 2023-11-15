using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameResources
{
	// Token: 0x02000183 RID: 387
	public class LevelResource : ScriptableObject
	{
		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x00018575 File Offset: 0x00016775
		// (set) Token: 0x0600085E RID: 2142 RVA: 0x0001857C File Offset: 0x0001677C
		public static LevelResource instance { get; private set; }

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x00018584 File Offset: 0x00016784
		public AssetReference[] chapters
		{
			get
			{
				return this._chapters;
			}
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0001858C File Offset: 0x0001678C
		public void Initialize()
		{
			LevelResource.instance = this;
			base.hideFlags |= HideFlags.DontUnloadUnusedAsset;
			for (int i = 1; i < this._chapters.Length; i++)
			{
			}
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x000185C1 File Offset: 0x000167C1
		public AssetReference GetChapter(int index)
		{
			return this._chapters[index];
		}

		// Token: 0x040006A6 RID: 1702
		[SerializeField]
		private AssetReference[] _chapters;
	}
}
