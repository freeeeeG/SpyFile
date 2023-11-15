using System;
using Klei.AI;

namespace Database
{
	// Token: 0x02000D05 RID: 3333
	public class GameplaySeasons : ResourceSet<GameplaySeason>
	{
		// Token: 0x060069B4 RID: 27060 RVA: 0x0028E829 File Offset: 0x0028CA29
		public GameplaySeasons(ResourceSet parent) : base("GameplaySeasons", parent)
		{
			this.VanillaSeasons();
			this.Expansion1Seasons();
			this.UnusedSeasons();
		}

		// Token: 0x060069B5 RID: 27061 RVA: 0x0028E84C File Offset: 0x0028CA4C
		private void VanillaSeasons()
		{
			this.MeteorShowers = base.Add(new MeteorShowerSeason("MeteorShowers", GameplaySeason.Type.World, "", 14f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, -1f).AddEvent(Db.Get().GameplayEvents.MeteorShowerIronEvent).AddEvent(Db.Get().GameplayEvents.MeteorShowerGoldEvent).AddEvent(Db.Get().GameplayEvents.MeteorShowerCopperEvent));
		}

		// Token: 0x060069B6 RID: 27062 RVA: 0x0028E8D0 File Offset: 0x0028CAD0
		private void Expansion1Seasons()
		{
			this.RegolithMoonMeteorShowers = base.Add(new MeteorShowerSeason("RegolithMoonMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.MeteorShowerDustEvent).AddEvent(Db.Get().GameplayEvents.ClusterIronShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower));
			this.TemporalTearMeteorShowers = base.Add(new MeteorShowerSeason("TemporalTearMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 1f, false, 0f, false, -1, 0f, float.PositiveInfinity, 1, false, -1f).AddEvent(Db.Get().GameplayEvents.MeteorShowerFullereneEvent));
			this.GassyMooteorShowers = base.Add(new MeteorShowerSeason("GassyMooteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, false, 6000f).AddEvent(Db.Get().GameplayEvents.GassyMooteorEvent));
			this.SpacedOutStyleStartMeteorShowers = base.Add(new MeteorShowerSeason("SpacedOutStyleStartMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.SpacedOutStyleRocketMeteorShowers = base.Add(new MeteorShowerSeason("SpacedOutStyleRocketMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterOxyliteShower));
			this.SpacedOutStyleWarpMeteorShowers = base.Add(new MeteorShowerSeason("SpacedOutStyleWarpMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterCopperShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower));
			this.ClassicStyleStartMeteorShowers = base.Add(new MeteorShowerSeason("ClassicStyleStartMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterCopperShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower));
			this.ClassicStyleWarpMeteorShowers = base.Add(new MeteorShowerSeason("ClassicStyleWarpMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterGoldShower).AddEvent(Db.Get().GameplayEvents.ClusterIronShower));
			this.TundraMoonletMeteorShowers = base.Add(new MeteorShowerSeason("TundraMoonletMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.MarshyMoonletMeteorShowers = base.Add(new MeteorShowerSeason("MarshyMoonletMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.NiobiumMoonletMeteorShowers = base.Add(new MeteorShowerSeason("NiobiumMoonletMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.WaterMoonletMeteorShowers = base.Add(new MeteorShowerSeason("WaterMoonletMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.MiniMetallicSwampyMeteorShowers = base.Add(new MeteorShowerSeason("MiniMetallicSwampyMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower).AddEvent(Db.Get().GameplayEvents.ClusterGoldShower));
			this.MiniForestFrozenMeteorShowers = base.Add(new MeteorShowerSeason("MiniForestFrozenMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterOxyliteShower));
			this.MiniBadlandsMeteorShowers = base.Add(new MeteorShowerSeason("MiniBadlandsMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterIceShower));
			this.MiniFlippedMeteorShowers = base.Add(new MeteorShowerSeason("MiniFlippedMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.MiniRadioactiveOceanMeteorShowers = base.Add(new MeteorShowerSeason("MiniRadioactiveOceanMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterUraniumShower));
		}

		// Token: 0x060069B7 RID: 27063 RVA: 0x0028EE33 File Offset: 0x0028D033
		private void UnusedSeasons()
		{
		}

		// Token: 0x04004C00 RID: 19456
		public GameplaySeason NaturalRandomEvents;

		// Token: 0x04004C01 RID: 19457
		public GameplaySeason DupeRandomEvents;

		// Token: 0x04004C02 RID: 19458
		public GameplaySeason PrickleCropSeason;

		// Token: 0x04004C03 RID: 19459
		public GameplaySeason BonusEvents;

		// Token: 0x04004C04 RID: 19460
		public GameplaySeason MeteorShowers;

		// Token: 0x04004C05 RID: 19461
		public GameplaySeason TemporalTearMeteorShowers;

		// Token: 0x04004C06 RID: 19462
		public GameplaySeason SpacedOutStyleStartMeteorShowers;

		// Token: 0x04004C07 RID: 19463
		public GameplaySeason SpacedOutStyleRocketMeteorShowers;

		// Token: 0x04004C08 RID: 19464
		public GameplaySeason SpacedOutStyleWarpMeteorShowers;

		// Token: 0x04004C09 RID: 19465
		public GameplaySeason ClassicStyleStartMeteorShowers;

		// Token: 0x04004C0A RID: 19466
		public GameplaySeason ClassicStyleWarpMeteorShowers;

		// Token: 0x04004C0B RID: 19467
		public GameplaySeason TundraMoonletMeteorShowers;

		// Token: 0x04004C0C RID: 19468
		public GameplaySeason MarshyMoonletMeteorShowers;

		// Token: 0x04004C0D RID: 19469
		public GameplaySeason NiobiumMoonletMeteorShowers;

		// Token: 0x04004C0E RID: 19470
		public GameplaySeason WaterMoonletMeteorShowers;

		// Token: 0x04004C0F RID: 19471
		public GameplaySeason GassyMooteorShowers;

		// Token: 0x04004C10 RID: 19472
		public GameplaySeason RegolithMoonMeteorShowers;

		// Token: 0x04004C11 RID: 19473
		public GameplaySeason MiniMetallicSwampyMeteorShowers;

		// Token: 0x04004C12 RID: 19474
		public GameplaySeason MiniForestFrozenMeteorShowers;

		// Token: 0x04004C13 RID: 19475
		public GameplaySeason MiniBadlandsMeteorShowers;

		// Token: 0x04004C14 RID: 19476
		public GameplaySeason MiniFlippedMeteorShowers;

		// Token: 0x04004C15 RID: 19477
		public GameplaySeason MiniRadioactiveOceanMeteorShowers;
	}
}
