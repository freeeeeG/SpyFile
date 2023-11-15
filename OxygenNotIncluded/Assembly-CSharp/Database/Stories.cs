using System;
using ProcGen;
using UnityEngine;

namespace Database
{
	// Token: 0x02000D26 RID: 3366
	public class Stories : ResourceSet<Story>
	{
		// Token: 0x06006A1E RID: 27166 RVA: 0x002952E4 File Offset: 0x002934E4
		public Stories(ResourceSet parent) : base("Stories", parent)
		{
			this.MegaBrainTank = base.Add(new Story("MegaBrainTank", "storytraits/MegaBrainTank", 0, 1, 43).SetKeepsake("keepsake_megabrain"));
			this.CreatureManipulator = base.Add(new Story("CreatureManipulator", "storytraits/CritterManipulator", 1, 2, 43).SetKeepsake("keepsake_crittermanipulator"));
			this.LonelyMinion = base.Add(new Story("LonelyMinion", "storytraits/LonelyMinion", 2, 3, 44).SetKeepsake("keepsake_lonelyminion"));
			this.FossilHunt = base.Add(new Story("FossilHunt", "storytraits/FossilHunt", 3, 4, 44).SetKeepsake("keepsake_fossilhunt"));
			this.resources.Sort();
		}

		// Token: 0x06006A1F RID: 27167 RVA: 0x002953AC File Offset: 0x002935AC
		public void AddStoryMod(Story mod)
		{
			mod.kleiUseOnlyCoordinateOffset = -1;
			base.Add(mod);
			this.resources.Sort();
		}

		// Token: 0x06006A20 RID: 27168 RVA: 0x002953C8 File Offset: 0x002935C8
		public int GetHighestCoordinateOffset()
		{
			int num = 0;
			foreach (Story story in this.resources)
			{
				num = Mathf.Max(num, story.kleiUseOnlyCoordinateOffset);
			}
			return num;
		}

		// Token: 0x06006A21 RID: 27169 RVA: 0x00295424 File Offset: 0x00293624
		public WorldTrait GetStoryTrait(string id, bool assertMissingTrait = false)
		{
			Story story = this.resources.Find((Story x) => x.Id == id);
			if (story != null)
			{
				return SettingsCache.GetCachedStoryTrait(story.worldgenStoryTraitKey, assertMissingTrait);
			}
			return null;
		}

		// Token: 0x06006A22 RID: 27170 RVA: 0x00295468 File Offset: 0x00293668
		public Story GetStoryFromStoryTrait(string storyTraitTemplate)
		{
			return this.resources.Find((Story x) => x.worldgenStoryTraitKey == storyTraitTemplate);
		}

		// Token: 0x04004D3F RID: 19775
		public Story MegaBrainTank;

		// Token: 0x04004D40 RID: 19776
		public Story CreatureManipulator;

		// Token: 0x04004D41 RID: 19777
		public Story LonelyMinion;

		// Token: 0x04004D42 RID: 19778
		public Story FossilHunt;
	}
}
