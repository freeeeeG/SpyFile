using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004D RID: 77
[Serializable]
public class MB_MultiMaterial
{
	// Token: 0x04000164 RID: 356
	public Material combinedMaterial;

	// Token: 0x04000165 RID: 357
	public bool considerMeshUVs;

	// Token: 0x04000166 RID: 358
	public List<Material> sourceMaterials = new List<Material>();
}
