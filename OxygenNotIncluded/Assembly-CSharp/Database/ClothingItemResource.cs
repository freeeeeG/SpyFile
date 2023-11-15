using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000CF5 RID: 3317
	public class ClothingItemResource : PermitResource
	{
		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06006976 RID: 26998 RVA: 0x00288D9A File Offset: 0x00286F9A
		// (set) Token: 0x06006977 RID: 26999 RVA: 0x00288DA2 File Offset: 0x00286FA2
		public string animFilename { get; private set; }

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06006978 RID: 27000 RVA: 0x00288DAB File Offset: 0x00286FAB
		// (set) Token: 0x06006979 RID: 27001 RVA: 0x00288DB3 File Offset: 0x00286FB3
		public KAnimFile AnimFile { get; private set; }

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x0600697A RID: 27002 RVA: 0x00288DBC File Offset: 0x00286FBC
		// (set) Token: 0x0600697B RID: 27003 RVA: 0x00288DC4 File Offset: 0x00286FC4
		public ClothingOutfitUtility.OutfitType outfitType { get; private set; }

		// Token: 0x0600697C RID: 27004 RVA: 0x00288DD0 File Offset: 0x00286FD0
		public ClothingItemResource(string id, string name, string desc, ClothingOutfitUtility.OutfitType outfitType, PermitCategory category, PermitRarity rarity, string animFile) : base(id, name, desc, category, rarity)
		{
			this.AnimFile = Assets.GetAnim(animFile);
			this.animFilename = animFile;
			this.outfitType = outfitType;
			DebugUtil.DevAssert(outfitType == PermitCategories.GetOutfitTypeFor(category), "Assert Failed.", null);
		}

		// Token: 0x0600697D RID: 27005 RVA: 0x00288E28 File Offset: 0x00287028
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			if (this.AnimFile == null)
			{
				Debug.LogError("Clothing kanim is missing from bundle: " + this.animFilename);
			}
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(this.AnimFile, "ui", false, "");
			result.SetFacadeForText(UI.KLEI_INVENTORY_SCREEN.CLOTHING_ITEM_FACADE_FOR);
			return result;
		}
	}
}
