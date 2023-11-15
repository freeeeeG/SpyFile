using System;

namespace Characters
{
	// Token: 0x020006BB RID: 1723
	public class CharacterChronometer
	{
		// Token: 0x060022A4 RID: 8868 RVA: 0x00068070 File Offset: 0x00066270
		public CharacterChronometer()
		{
			this.master = new Chronometer();
			this.effect = new Chronometer(this.master);
			this.projectile = new Chronometer(this.master);
			this.animation = new Chronometer(this.master);
		}

		// Token: 0x04001D6B RID: 7531
		public readonly Chronometer master;

		// Token: 0x04001D6C RID: 7532
		public readonly Chronometer effect;

		// Token: 0x04001D6D RID: 7533
		public readonly Chronometer projectile;

		// Token: 0x04001D6E RID: 7534
		public readonly Chronometer animation;
	}
}
