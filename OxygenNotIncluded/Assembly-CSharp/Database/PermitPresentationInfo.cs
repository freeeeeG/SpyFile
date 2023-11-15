using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000D0D RID: 3341
	public struct PermitPresentationInfo
	{
		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x060069D1 RID: 27089 RVA: 0x00290DBA File Offset: 0x0028EFBA
		// (set) Token: 0x060069D2 RID: 27090 RVA: 0x00290DC2 File Offset: 0x0028EFC2
		public string facadeFor { readonly get; private set; }

		// Token: 0x060069D3 RID: 27091 RVA: 0x00290DCB File Offset: 0x0028EFCB
		public static Sprite GetUnknownSprite()
		{
			return Assets.GetSprite("unknown");
		}

		// Token: 0x060069D4 RID: 27092 RVA: 0x00290DDC File Offset: 0x0028EFDC
		public void SetFacadeForPrefabName(string prefabName)
		{
			this.facadeFor = UI.KLEI_INVENTORY_SCREEN.ITEM_FACADE_FOR.Replace("{ConfigProperName}", prefabName);
		}

		// Token: 0x060069D5 RID: 27093 RVA: 0x00290DF4 File Offset: 0x0028EFF4
		public void SetFacadeForPrefabID(string prefabId)
		{
			this.facadeFor = UI.KLEI_INVENTORY_SCREEN.ITEM_FACADE_FOR.Replace("{ConfigProperName}", Assets.GetPrefab(prefabId).GetProperName());
		}

		// Token: 0x060069D6 RID: 27094 RVA: 0x00290E1B File Offset: 0x0028F01B
		public void SetFacadeForText(string text)
		{
			this.facadeFor = text;
		}

		// Token: 0x04004C82 RID: 19586
		public Sprite sprite;
	}
}
