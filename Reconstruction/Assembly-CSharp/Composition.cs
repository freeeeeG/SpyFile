using System;

// Token: 0x020001F1 RID: 497
[Serializable]
public class Composition
{
	// Token: 0x06000C94 RID: 3220 RVA: 0x00020B74 File Offset: 0x0001ED74
	public Composition(int levelRequirement, int elementRequirement)
	{
		this.qualityRequeirement = levelRequirement;
		this.elementRequirement = elementRequirement;
		this.obtained = false;
	}

	// Token: 0x04000635 RID: 1589
	public int qualityRequeirement;

	// Token: 0x04000636 RID: 1590
	public int elementRequirement;

	// Token: 0x04000637 RID: 1591
	public bool obtained;

	// Token: 0x04000638 RID: 1592
	public bool isPerfect;

	// Token: 0x04000639 RID: 1593
	public ElementTurret turret;
}
