using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000CFA RID: 3322
	public class Deaths : ResourceSet<Death>
	{
		// Token: 0x06006988 RID: 27016 RVA: 0x0028AE14 File Offset: 0x00289014
		public Deaths(ResourceSet parent) : base("Deaths", parent)
		{
			this.Generic = new Death("Generic", this, DUPLICANTS.DEATHS.GENERIC.NAME, DUPLICANTS.DEATHS.GENERIC.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.Frozen = new Death("Frozen", this, DUPLICANTS.DEATHS.FROZEN.NAME, DUPLICANTS.DEATHS.FROZEN.DESCRIPTION, "death_freeze_trans", "death_freeze_solid");
			this.Suffocation = new Death("Suffocation", this, DUPLICANTS.DEATHS.SUFFOCATION.NAME, DUPLICANTS.DEATHS.SUFFOCATION.DESCRIPTION, "death_suffocation", "dead_on_back");
			this.Starvation = new Death("Starvation", this, DUPLICANTS.DEATHS.STARVATION.NAME, DUPLICANTS.DEATHS.STARVATION.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.Overheating = new Death("Overheating", this, DUPLICANTS.DEATHS.OVERHEATING.NAME, DUPLICANTS.DEATHS.OVERHEATING.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.Drowned = new Death("Drowned", this, DUPLICANTS.DEATHS.DROWNED.NAME, DUPLICANTS.DEATHS.DROWNED.DESCRIPTION, "death_suffocation", "dead_on_back");
			this.Explosion = new Death("Explosion", this, DUPLICANTS.DEATHS.EXPLOSION.NAME, DUPLICANTS.DEATHS.EXPLOSION.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.Slain = new Death("Combat", this, DUPLICANTS.DEATHS.COMBAT.NAME, DUPLICANTS.DEATHS.COMBAT.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.FatalDisease = new Death("FatalDisease", this, DUPLICANTS.DEATHS.FATALDISEASE.NAME, DUPLICANTS.DEATHS.FATALDISEASE.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.Radiation = new Death("Radiation", this, DUPLICANTS.DEATHS.RADIATION.NAME, DUPLICANTS.DEATHS.RADIATION.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.HitByHighEnergyParticle = new Death("HitByHighEnergyParticle", this, DUPLICANTS.DEATHS.HITBYHIGHENERGYPARTICLE.NAME, DUPLICANTS.DEATHS.HITBYHIGHENERGYPARTICLE.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.DeadBattery = new Death("DeadBattery", this, DUPLICANTS.DEATHS.HITBYHIGHENERGYPARTICLE.NAME, DUPLICANTS.DEATHS.HITBYHIGHENERGYPARTICLE.DESCRIPTION, "dead_on_back", "dead_on_back");
		}

		// Token: 0x04004B07 RID: 19207
		public Death Generic;

		// Token: 0x04004B08 RID: 19208
		public Death Frozen;

		// Token: 0x04004B09 RID: 19209
		public Death Suffocation;

		// Token: 0x04004B0A RID: 19210
		public Death Starvation;

		// Token: 0x04004B0B RID: 19211
		public Death Slain;

		// Token: 0x04004B0C RID: 19212
		public Death Overheating;

		// Token: 0x04004B0D RID: 19213
		public Death Drowned;

		// Token: 0x04004B0E RID: 19214
		public Death Explosion;

		// Token: 0x04004B0F RID: 19215
		public Death FatalDisease;

		// Token: 0x04004B10 RID: 19216
		public Death Radiation;

		// Token: 0x04004B11 RID: 19217
		public Death HitByHighEnergyParticle;

		// Token: 0x04004B12 RID: 19218
		public Death DeadBattery;
	}
}
