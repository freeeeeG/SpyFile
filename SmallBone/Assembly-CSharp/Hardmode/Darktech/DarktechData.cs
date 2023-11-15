using System;

namespace Hardmode.Darktech
{
	// Token: 0x02000155 RID: 341
	[Serializable]
	public struct DarktechData
	{
		// Token: 0x060006C0 RID: 1728 RVA: 0x000137F4 File Offset: 0x000119F4
		public DarktechData(DarktechData.Type type)
		{
			this.type = type;
		}

		// Token: 0x040004F2 RID: 1266
		public DarktechData.Type type;

		// Token: 0x02000156 RID: 342
		public enum Type
		{
			// Token: 0x040004F4 RID: 1268
			SkullManufacturingMachine,
			// Token: 0x040004F5 RID: 1269
			SuppliesManufacturingMachine,
			// Token: 0x040004F6 RID: 1270
			OmenAmplifier,
			// Token: 0x040004F7 RID: 1271
			ItemRotationEquipment,
			// Token: 0x040004F8 RID: 1272
			HopeExtractor,
			// Token: 0x040004F9 RID: 1273
			HealthAuxiliaryEquipment,
			// Token: 0x040004FA RID: 1274
			LuckyMeasuringInstrument,
			// Token: 0x040004FB RID: 1275
			InscriptionSynthesisEquipment,
			// Token: 0x040004FC RID: 1276
			NextGenerationHopeExtractor,
			// Token: 0x040004FD RID: 1277
			BoneParticleMagnetoscope,
			// Token: 0x040004FE RID: 1278
			GoldenCalculator,
			// Token: 0x040004FF RID: 1279
			AnxietyAccelerator,
			// Token: 0x04000500 RID: 1280
			ObservationInstrument
		}
	}
}
