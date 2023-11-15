using System;
using STRINGS;
using TUNING;

namespace Database
{
	// Token: 0x02000D0A RID: 3338
	public class AssignableSlots : ResourceSet<AssignableSlot>
	{
		// Token: 0x060069CA RID: 27082 RVA: 0x0029087C File Offset: 0x0028EA7C
		public AssignableSlots()
		{
			this.Bed = base.Add(new OwnableSlot("Bed", MISC.TAGS.BED));
			this.MessStation = base.Add(new OwnableSlot("MessStation", MISC.TAGS.MESSSTATION));
			this.Clinic = base.Add(new OwnableSlot("Clinic", MISC.TAGS.CLINIC));
			this.MedicalBed = base.Add(new OwnableSlot("MedicalBed", MISC.TAGS.CLINIC));
			this.MedicalBed.showInUI = false;
			this.GeneShuffler = base.Add(new OwnableSlot("GeneShuffler", MISC.TAGS.GENE_SHUFFLER));
			this.GeneShuffler.showInUI = false;
			this.Toilet = base.Add(new OwnableSlot("Toilet", MISC.TAGS.TOILET));
			this.MassageTable = base.Add(new OwnableSlot("MassageTable", MISC.TAGS.MASSAGE_TABLE));
			this.RocketCommandModule = base.Add(new OwnableSlot("RocketCommandModule", MISC.TAGS.COMMAND_MODULE));
			this.HabitatModule = base.Add(new OwnableSlot("HabitatModule", MISC.TAGS.HABITAT_MODULE));
			this.ResetSkillsStation = base.Add(new OwnableSlot("ResetSkillsStation", "ResetSkillsStation"));
			this.WarpPortal = base.Add(new OwnableSlot("WarpPortal", MISC.TAGS.WARP_PORTAL));
			this.WarpPortal.showInUI = false;
			this.Toy = base.Add(new EquipmentSlot(TUNING.EQUIPMENT.TOYS.SLOT, MISC.TAGS.TOY, false));
			this.Suit = base.Add(new EquipmentSlot(TUNING.EQUIPMENT.SUITS.SLOT, MISC.TAGS.SUIT, true));
			this.Tool = base.Add(new EquipmentSlot(TUNING.EQUIPMENT.TOOLS.TOOLSLOT, MISC.TAGS.MULTITOOL, false));
			this.Outfit = base.Add(new EquipmentSlot(TUNING.EQUIPMENT.CLOTHING.SLOT, MISC.TAGS.CLOTHES, true));
		}

		// Token: 0x04004C5F RID: 19551
		public AssignableSlot Bed;

		// Token: 0x04004C60 RID: 19552
		public AssignableSlot MessStation;

		// Token: 0x04004C61 RID: 19553
		public AssignableSlot Clinic;

		// Token: 0x04004C62 RID: 19554
		public AssignableSlot GeneShuffler;

		// Token: 0x04004C63 RID: 19555
		public AssignableSlot MedicalBed;

		// Token: 0x04004C64 RID: 19556
		public AssignableSlot Toilet;

		// Token: 0x04004C65 RID: 19557
		public AssignableSlot MassageTable;

		// Token: 0x04004C66 RID: 19558
		public AssignableSlot RocketCommandModule;

		// Token: 0x04004C67 RID: 19559
		public AssignableSlot HabitatModule;

		// Token: 0x04004C68 RID: 19560
		public AssignableSlot ResetSkillsStation;

		// Token: 0x04004C69 RID: 19561
		public AssignableSlot WarpPortal;

		// Token: 0x04004C6A RID: 19562
		public AssignableSlot Toy;

		// Token: 0x04004C6B RID: 19563
		public AssignableSlot Suit;

		// Token: 0x04004C6C RID: 19564
		public AssignableSlot Tool;

		// Token: 0x04004C6D RID: 19565
		public AssignableSlot Outfit;
	}
}
