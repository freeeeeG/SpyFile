using System;

namespace TUNING
{
	// Token: 0x02000D92 RID: 3474
	public class MEDICINE
	{
		// Token: 0x04004FCB RID: 20427
		public const float DEFAULT_MASS = 1f;

		// Token: 0x04004FCC RID: 20428
		public const float RECUPERATION_DISEASE_MULTIPLIER = 1.1f;

		// Token: 0x04004FCD RID: 20429
		public const float RECUPERATION_DOCTORED_DISEASE_MULTIPLIER = 1.2f;

		// Token: 0x04004FCE RID: 20430
		public const float WORK_TIME = 10f;

		// Token: 0x04004FCF RID: 20431
		public static readonly MedicineInfo BASICBOOSTER = new MedicineInfo("BasicBooster", "Medicine_BasicBooster", MedicineInfo.MedicineType.Booster, null, null);

		// Token: 0x04004FD0 RID: 20432
		public static readonly MedicineInfo INTERMEDIATEBOOSTER = new MedicineInfo("IntermediateBooster", "Medicine_IntermediateBooster", MedicineInfo.MedicineType.Booster, null, null);

		// Token: 0x04004FD1 RID: 20433
		public static readonly MedicineInfo BASICCURE = new MedicineInfo("BasicCure", null, MedicineInfo.MedicineType.CureSpecific, null, new string[]
		{
			"FoodSickness"
		});

		// Token: 0x04004FD2 RID: 20434
		public static readonly MedicineInfo ANTIHISTAMINE = new MedicineInfo("Antihistamine", "HistamineSuppression", MedicineInfo.MedicineType.CureSpecific, null, new string[]
		{
			"Allergies"
		});

		// Token: 0x04004FD3 RID: 20435
		public static readonly MedicineInfo INTERMEDIATECURE = new MedicineInfo("IntermediateCure", null, MedicineInfo.MedicineType.CureSpecific, "DoctorStation", new string[]
		{
			"SlimeSickness"
		});

		// Token: 0x04004FD4 RID: 20436
		public static readonly MedicineInfo ADVANCEDCURE = new MedicineInfo("AdvancedCure", null, MedicineInfo.MedicineType.CureSpecific, "AdvancedDoctorStation", new string[]
		{
			"ZombieSickness"
		});

		// Token: 0x04004FD5 RID: 20437
		public static readonly MedicineInfo BASICRADPILL = new MedicineInfo("BasicRadPill", "Medicine_BasicRadPill", MedicineInfo.MedicineType.Booster, null, null);

		// Token: 0x04004FD6 RID: 20438
		public static readonly MedicineInfo INTERMEDIATERADPILL = new MedicineInfo("IntermediateRadPill", "Medicine_IntermediateRadPill", MedicineInfo.MedicineType.Booster, "AdvancedDoctorStation", null);
	}
}
