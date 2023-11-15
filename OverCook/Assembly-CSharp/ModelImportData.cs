using System;
using UnityEngine;

// Token: 0x020001A7 RID: 423
[Serializable]
public class ModelImportData : ScriptableObject
{
	// Token: 0x040005EA RID: 1514
	[global::Tooltip("A model importer")]
	public GameObject ParentPrefab;

	// Token: 0x040005EB RID: 1515
	public PhysicMaterial ColliderMaterial;

	// Token: 0x040005EC RID: 1516
	public LayerMask Layer;

	// Token: 0x040005ED RID: 1517
	public PlayerPhysicsSurfaceProperties PlayerPhysicsSurfaceProperties;

	// Token: 0x040005EE RID: 1518
	public bool HasGridLocation;

	// Token: 0x040005EF RID: 1519
	public bool HasGridSnap;
}
