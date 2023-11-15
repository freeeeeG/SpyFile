using System;

namespace Database
{
	// Token: 0x02000D09 RID: 3337
	public class OrbitalTypeCategories : ResourceSet<OrbitalData>
	{
		// Token: 0x060069C9 RID: 27081 RVA: 0x00290534 File Offset: 0x0028E734
		public OrbitalTypeCategories(ResourceSet parent) : base("OrbitalTypeCategories", parent)
		{
			this.backgroundEarth = new OrbitalData("backgroundEarth", this, "earth_kanim", "", OrbitalData.OrbitalType.world, 1f, 0.5f, 0.95f, 10f, 10f, 1.05f, true, 0.05f, 25f, 1f);
			this.frozenOre = new OrbitalData("frozenOre", this, "starmap_frozen_ore_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1f, true, 0.05f, 25f, 1f);
			this.heliumCloud = new OrbitalData("heliumCloud", this, "starmap_helium_cloud_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.iceCloud = new OrbitalData("iceCloud", this, "starmap_ice_cloud_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.iceRock = new OrbitalData("iceRock", this, "starmap_ice_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.purpleGas = new OrbitalData("purpleGas", this, "starmap_purple_gas_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.radioactiveGas = new OrbitalData("radioactiveGas", this, "starmap_radioactive_gas_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.rocky = new OrbitalData("rocky", this, "starmap_rocky_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.gravitas = new OrbitalData("gravitas", this, "starmap_space_junk_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.orbit = new OrbitalData("orbit", this, "starmap_orbit_kanim", "", OrbitalData.OrbitalType.inOrbit, 1f, 0.25f, 0.5f, -350f, 350f, 1.05f, false, 0.05f, 4f, 1f);
			this.landed = new OrbitalData("landed", this, "starmap_landed_surface_kanim", "", OrbitalData.OrbitalType.landed, 0f, 0.5f, 0.35f, -350f, 350f, 1.05f, false, 0.05f, 4f, 1f);
		}

		// Token: 0x04004C54 RID: 19540
		public OrbitalData backgroundEarth;

		// Token: 0x04004C55 RID: 19541
		public OrbitalData frozenOre;

		// Token: 0x04004C56 RID: 19542
		public OrbitalData heliumCloud;

		// Token: 0x04004C57 RID: 19543
		public OrbitalData iceCloud;

		// Token: 0x04004C58 RID: 19544
		public OrbitalData iceRock;

		// Token: 0x04004C59 RID: 19545
		public OrbitalData purpleGas;

		// Token: 0x04004C5A RID: 19546
		public OrbitalData radioactiveGas;

		// Token: 0x04004C5B RID: 19547
		public OrbitalData rocky;

		// Token: 0x04004C5C RID: 19548
		public OrbitalData gravitas;

		// Token: 0x04004C5D RID: 19549
		public OrbitalData orbit;

		// Token: 0x04004C5E RID: 19550
		public OrbitalData landed;
	}
}
