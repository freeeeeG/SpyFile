using System;

// Token: 0x020008BB RID: 2235
public class NonEssentialEnergyConsumer : EnergyConsumer
{
	// Token: 0x1700047C RID: 1148
	// (get) Token: 0x060040BC RID: 16572 RVA: 0x00169C8D File Offset: 0x00167E8D
	// (set) Token: 0x060040BD RID: 16573 RVA: 0x00169C95 File Offset: 0x00167E95
	public override bool IsPowered
	{
		get
		{
			return this.isPowered;
		}
		protected set
		{
			if (value == this.isPowered)
			{
				return;
			}
			this.isPowered = value;
			Action<bool> poweredStateChanged = this.PoweredStateChanged;
			if (poweredStateChanged == null)
			{
				return;
			}
			poweredStateChanged(this.isPowered);
		}
	}

	// Token: 0x04002A26 RID: 10790
	public Action<bool> PoweredStateChanged;

	// Token: 0x04002A27 RID: 10791
	private bool isPowered;
}
