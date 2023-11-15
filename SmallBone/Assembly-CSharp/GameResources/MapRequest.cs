using System;
using Level;
using UnityEngine.AddressableAssets;

namespace GameResources
{
	// Token: 0x02000191 RID: 401
	public sealed class MapRequest : Request<Map>
	{
		// Token: 0x060008B5 RID: 2229 RVA: 0x00018EBC File Offset: 0x000170BC
		public MapRequest(AssetReference reference) : base(reference)
		{
		}
	}
}
