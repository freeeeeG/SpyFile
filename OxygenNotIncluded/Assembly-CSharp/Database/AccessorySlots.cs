using System;

namespace Database
{
	// Token: 0x02000CDE RID: 3294
	public class AccessorySlots : ResourceSet<AccessorySlot>
	{
		// Token: 0x06006925 RID: 26917 RVA: 0x00279E5C File Offset: 0x0027805C
		public AccessorySlots(ResourceSet parent) : base("AccessorySlots", parent)
		{
			parent = Db.Get().Accessories;
			KAnimFile anim = Assets.GetAnim("head_swap_kanim");
			KAnimFile anim2 = Assets.GetAnim("body_comp_default_kanim");
			KAnimFile anim3 = Assets.GetAnim("body_swap_kanim");
			KAnimFile anim4 = Assets.GetAnim("hair_swap_kanim");
			KAnimFile anim5 = Assets.GetAnim("hat_swap_kanim");
			this.Eyes = new AccessorySlot("Eyes", this, anim, 0);
			this.Hair = new AccessorySlot("Hair", this, anim4, 0);
			this.HeadShape = new AccessorySlot("HeadShape", this, anim, 0);
			this.Mouth = new AccessorySlot("Mouth", this, anim, 0);
			this.Hat = new AccessorySlot("Hat", this, anim5, 4);
			this.HatHair = new AccessorySlot("Hat_Hair", this, anim4, 0);
			this.HeadEffects = new AccessorySlot("HeadFX", this, anim, 0);
			this.Body = new AccessorySlot("Torso", this, new KAnimHashedString("torso"), anim3, null, 0);
			this.Arm = new AccessorySlot("Arm_Sleeve", this, new KAnimHashedString("arm_sleeve"), anim3, null, 0);
			this.ArmLower = new AccessorySlot("Arm_Lower_Sleeve", this, new KAnimHashedString("arm_lower_sleeve"), anim3, null, 0);
			this.Belt = new AccessorySlot("Belt", this, new KAnimHashedString("belt"), anim2, null, 0);
			this.Neck = new AccessorySlot("Neck", this, new KAnimHashedString("neck"), anim2, null, 0);
			this.Pelvis = new AccessorySlot("Pelvis", this, new KAnimHashedString("pelvis"), anim2, null, 0);
			this.Foot = new AccessorySlot("Foot", this, new KAnimHashedString("foot"), anim2, Assets.GetAnim("shoes_basic_black_kanim"), 0);
			this.Leg = new AccessorySlot("Leg", this, new KAnimHashedString("leg"), anim2, null, 0);
			this.Necklace = new AccessorySlot("Necklace", this, new KAnimHashedString("necklace"), anim2, null, 0);
			this.Cuff = new AccessorySlot("Cuff", this, new KAnimHashedString("cuff"), anim2, null, 0);
			this.Hand = new AccessorySlot("Hand", this, new KAnimHashedString("hand_paint"), anim2, null, 0);
			this.Skirt = new AccessorySlot("Skirt", this, new KAnimHashedString("skirt"), anim3, null, 0);
			this.ArmLowerSkin = new AccessorySlot("Arm_Lower", this, new KAnimHashedString("arm_lower"), anim3, null, 0);
			this.ArmUpperSkin = new AccessorySlot("Arm_Upper", this, new KAnimHashedString("arm_upper"), anim3, null, 0);
			this.LegSkin = new AccessorySlot("Leg_Skin", this, new KAnimHashedString("leg_skin"), anim3, null, 0);
			foreach (AccessorySlot accessorySlot in this.resources)
			{
				accessorySlot.AddAccessories(accessorySlot.AnimFile, parent);
			}
			Db.Get().Accessories.AddCustomAccessories(Assets.GetAnim("body_lonelyminion_kanim"), parent, this);
		}

		// Token: 0x06006926 RID: 26918 RVA: 0x0027A194 File Offset: 0x00278394
		public AccessorySlot Find(KAnimHashedString symbol_name)
		{
			foreach (AccessorySlot accessorySlot in Db.Get().AccessorySlots.resources)
			{
				if (symbol_name == accessorySlot.targetSymbolId)
				{
					return accessorySlot;
				}
			}
			return null;
		}

		// Token: 0x0400485C RID: 18524
		public AccessorySlot Eyes;

		// Token: 0x0400485D RID: 18525
		public AccessorySlot Hair;

		// Token: 0x0400485E RID: 18526
		public AccessorySlot HeadShape;

		// Token: 0x0400485F RID: 18527
		public AccessorySlot Mouth;

		// Token: 0x04004860 RID: 18528
		public AccessorySlot Body;

		// Token: 0x04004861 RID: 18529
		public AccessorySlot Arm;

		// Token: 0x04004862 RID: 18530
		public AccessorySlot ArmLower;

		// Token: 0x04004863 RID: 18531
		public AccessorySlot Hat;

		// Token: 0x04004864 RID: 18532
		public AccessorySlot HatHair;

		// Token: 0x04004865 RID: 18533
		public AccessorySlot HeadEffects;

		// Token: 0x04004866 RID: 18534
		public AccessorySlot Belt;

		// Token: 0x04004867 RID: 18535
		public AccessorySlot Neck;

		// Token: 0x04004868 RID: 18536
		public AccessorySlot Pelvis;

		// Token: 0x04004869 RID: 18537
		public AccessorySlot Leg;

		// Token: 0x0400486A RID: 18538
		public AccessorySlot Foot;

		// Token: 0x0400486B RID: 18539
		public AccessorySlot Skirt;

		// Token: 0x0400486C RID: 18540
		public AccessorySlot Necklace;

		// Token: 0x0400486D RID: 18541
		public AccessorySlot Cuff;

		// Token: 0x0400486E RID: 18542
		public AccessorySlot Hand;

		// Token: 0x0400486F RID: 18543
		public AccessorySlot ArmLowerSkin;

		// Token: 0x04004870 RID: 18544
		public AccessorySlot ArmUpperSkin;

		// Token: 0x04004871 RID: 18545
		public AccessorySlot LegSkin;
	}
}
