using System;
using Level;
using UnityEngine.AddressableAssets;

namespace GameResources
{
	// Token: 0x02000189 RID: 393
	[Serializable]
	public class MapReference
	{
		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000887 RID: 2183 RVA: 0x00018C4C File Offset: 0x00016E4C
		public bool empty
		{
			get
			{
				return string.IsNullOrWhiteSpace(this.reference.AssetGUID);
			}
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x00003709 File Offset: 0x00001909
		public MapReference()
		{
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00018C5E File Offset: 0x00016E5E
		public MapReference(AssetReference reference)
		{
			this.reference = reference;
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x00018C6D File Offset: 0x00016E6D
		public MapRequest LoadAsync()
		{
			return new MapRequest(this.reference);
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x00018C7A File Offset: 0x00016E7A
		public Map Load()
		{
			throw new NotImplementedException("어드레서블 이용하도록 바꿔야함");
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00018C86 File Offset: 0x00016E86
		public static MapReference FromPath(string path)
		{
			return null;
		}

		// Token: 0x040006D4 RID: 1748
		public Map.Type type;

		// Token: 0x040006D5 RID: 1749
		public SpecialMap.Type specialMapType;

		// Token: 0x040006D6 RID: 1750
		[NonSerialized]
		public bool darkEnemy;

		// Token: 0x040006D7 RID: 1751
		public AssetReference reference;

		// Token: 0x040006D8 RID: 1752
		public string path;
	}
}
