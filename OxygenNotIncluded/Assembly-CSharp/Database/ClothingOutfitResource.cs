using System;
using UnityEngine;

namespace Database
{
	// Token: 0x02000CF7 RID: 3319
	public class ClothingOutfitResource : Resource
	{
		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06006981 RID: 27009 RVA: 0x00289802 File Offset: 0x00287A02
		// (set) Token: 0x06006982 RID: 27010 RVA: 0x0028980A File Offset: 0x00287A0A
		public string[] itemsInOutfit { get; private set; }

		// Token: 0x06006983 RID: 27011 RVA: 0x00289814 File Offset: 0x00287A14
		public ClothingOutfitResource(string id, string[] items_in_outfit, LocString name, ClothingOutfitUtility.OutfitType outfitType) : base(id, name)
		{
			this.itemsInOutfit = items_in_outfit;
			this.outfitType = outfitType;
			string[] itemsInOutfit = this.itemsInOutfit;
			for (int i = 0; i < itemsInOutfit.Length; i++)
			{
				string itemId = itemsInOutfit[i];
				int num = Array.FindIndex<ClothingItems.Info>(ClothingItems.Infos_All, (ClothingItems.Info e) => e.id == itemId);
				if (num < 0)
				{
					DebugUtil.DevAssert(false, string.Concat(new string[]
					{
						"Outfit \"",
						this.Id,
						"\" contains an item that doesn't exist. Given item id: \"",
						itemId,
						"\""
					}), null);
				}
				else
				{
					ClothingItems.Info info = ClothingItems.Infos_All[num];
					if (info.outfitType != this.outfitType)
					{
						DebugUtil.DevAssert(false, string.Format("Outfit \"{0}\" contains an item that has a mis-matched outfit type. Defined outfit's type: \"{1}\". Given item: {{ id: \"{2}\" forOutfitType: \"{3}\" }}", new object[]
						{
							this.Id,
							this.outfitType,
							itemId,
							info.outfitType
						}), null);
					}
				}
			}
		}

		// Token: 0x06006984 RID: 27012 RVA: 0x00289922 File Offset: 0x00287B22
		public global::Tuple<Sprite, Color> GetUISprite()
		{
			Sprite sprite = Assets.GetSprite("unknown");
			return new global::Tuple<Sprite, Color>(sprite, (sprite != null) ? Color.white : Color.clear);
		}

		// Token: 0x04004AB9 RID: 19129
		public ClothingOutfitUtility.OutfitType outfitType;
	}
}
