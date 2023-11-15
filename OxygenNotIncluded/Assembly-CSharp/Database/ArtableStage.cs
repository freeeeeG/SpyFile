using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000CE2 RID: 3298
	public class ArtableStage : PermitResource
	{
		// Token: 0x0600692D RID: 26925 RVA: 0x0027AD14 File Offset: 0x00278F14
		public ArtableStage(string id, string name, string desc, PermitRarity rarity, string animFile, string anim, int decor_value, bool cheer_on_complete, ArtableStatusItem status_item, string prefabId, string symbolName = "") : base(id, name, desc, PermitCategory.Artwork, rarity)
		{
			this.id = id;
			this.animFile = animFile;
			this.anim = anim;
			this.symbolName = symbolName;
			this.decor = decor_value;
			this.cheerOnComplete = cheer_on_complete;
			this.statusItem = status_item;
			this.prefabId = prefabId;
		}

		// Token: 0x0600692E RID: 26926 RVA: 0x0027AD70 File Offset: 0x00278F70
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim(this.animFile), "ui", false, "");
			result.SetFacadeForText(UI.KLEI_INVENTORY_SCREEN.ARTABLE_ITEM_FACADE_FOR.Replace("{ConfigProperName}", Assets.GetPrefab(this.prefabId).GetProperName()).Replace("{ArtableQuality}", this.statusItem.GetName(null)));
			return result;
		}

		// Token: 0x04004896 RID: 18582
		public string id;

		// Token: 0x04004897 RID: 18583
		public string anim;

		// Token: 0x04004898 RID: 18584
		public string animFile;

		// Token: 0x04004899 RID: 18585
		public string prefabId;

		// Token: 0x0400489A RID: 18586
		public string symbolName;

		// Token: 0x0400489B RID: 18587
		public int decor;

		// Token: 0x0400489C RID: 18588
		public bool cheerOnComplete;

		// Token: 0x0400489D RID: 18589
		public ArtableStatusItem statusItem;
	}
}
