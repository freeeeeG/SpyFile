using System;

// Token: 0x02000544 RID: 1348
public class OrbitalData : Resource
{
	// Token: 0x060020BA RID: 8378 RVA: 0x000AF634 File Offset: 0x000AD834
	public OrbitalData(string id, ResourceSet parent, string animFile = "earth_kanim", string initialAnim = "", OrbitalData.OrbitalType orbitalType = OrbitalData.OrbitalType.poi, float periodInCycles = 1f, float xGridPercent = 0.5f, float yGridPercent = 0.5f, float minAngle = -350f, float maxAngle = 350f, float radiusScale = 1.05f, bool rotatesBehind = true, float behindZ = 0.05f, float distance = 25f, float renderZ = 1f) : base(id, parent, null)
	{
		this.animFile = animFile;
		this.initialAnim = initialAnim;
		this.orbitalType = orbitalType;
		this.periodInCycles = periodInCycles;
		this.xGridPercent = xGridPercent;
		this.yGridPercent = yGridPercent;
		this.minAngle = minAngle;
		this.maxAngle = maxAngle;
		this.radiusScale = radiusScale;
		this.rotatesBehind = rotatesBehind;
		this.behindZ = behindZ;
		this.distance = distance;
		this.renderZ = renderZ;
	}

	// Token: 0x04001276 RID: 4726
	public string animFile;

	// Token: 0x04001277 RID: 4727
	public string initialAnim;

	// Token: 0x04001278 RID: 4728
	public float periodInCycles;

	// Token: 0x04001279 RID: 4729
	public float xGridPercent;

	// Token: 0x0400127A RID: 4730
	public float yGridPercent;

	// Token: 0x0400127B RID: 4731
	public float minAngle;

	// Token: 0x0400127C RID: 4732
	public float maxAngle;

	// Token: 0x0400127D RID: 4733
	public float radiusScale;

	// Token: 0x0400127E RID: 4734
	public bool rotatesBehind;

	// Token: 0x0400127F RID: 4735
	public float behindZ;

	// Token: 0x04001280 RID: 4736
	public float distance;

	// Token: 0x04001281 RID: 4737
	public float renderZ;

	// Token: 0x04001282 RID: 4738
	public OrbitalData.OrbitalType orbitalType;

	// Token: 0x020011E8 RID: 4584
	public enum OrbitalType
	{
		// Token: 0x04005DF2 RID: 24050
		world,
		// Token: 0x04005DF3 RID: 24051
		poi,
		// Token: 0x04005DF4 RID: 24052
		inOrbit,
		// Token: 0x04005DF5 RID: 24053
		landed
	}
}
