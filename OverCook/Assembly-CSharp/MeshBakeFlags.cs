using System;

// Token: 0x02000004 RID: 4
[Flags]
public enum MeshBakeFlags
{
	// Token: 0x04000005 RID: 5
	None = 0,
	// Token: 0x04000006 RID: 6
	RecieveShadows = 1,
	// Token: 0x04000007 RID: 7
	CastShadowOn = 4,
	// Token: 0x04000008 RID: 8
	CastShadowOff = 8,
	// Token: 0x04000009 RID: 9
	CastShadowTwoSided = 16,
	// Token: 0x0400000A RID: 10
	CastShadowOnly = 32,
	// Token: 0x0400000B RID: 11
	GIContributeOff = 64,
	// Token: 0x0400000C RID: 12
	GIContributeOn = 128,
	// Token: 0x0400000D RID: 13
	GIContributeProbe = 256,
	// Token: 0x0400000E RID: 14
	RenderVisible = 512,
	// Token: 0x0400000F RID: 15
	ObjVisible = 1024,
	// Token: 0x04000010 RID: 16
	lightProbeUsageOff = 2048,
	// Token: 0x04000011 RID: 17
	reflProbeUsageOff = 4096,
	// Token: 0x04000012 RID: 18
	lightProbeUsageAnc = 8192,
	// Token: 0x04000013 RID: 19
	lightProbeUsageBlnd = 16384,
	// Token: 0x04000014 RID: 20
	reflProbeUsageBlnd = 32768,
	// Token: 0x04000015 RID: 21
	requiresNormals = 65536
}
