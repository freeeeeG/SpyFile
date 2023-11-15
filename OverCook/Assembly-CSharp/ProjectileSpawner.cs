using System;
using UnityEngine;

// Token: 0x020004A1 RID: 1185
public class ProjectileSpawner : MonoBehaviour
{
	// Token: 0x040010B5 RID: 4277
	[SerializeField]
	[AssignChild("SpawnPoint", Editorbility.NonEditable)]
	public Transform m_spawnPoint;

	// Token: 0x040010B6 RID: 4278
	[SerializeField]
	public GameObject m_projectilePrefab;

	// Token: 0x040010B7 RID: 4279
	[SerializeField]
	public ProjectileSpawner.FireMode m_fireMode;

	// Token: 0x040010B8 RID: 4280
	[SerializeField]
	public float m_airTime;

	// Token: 0x040010B9 RID: 4281
	[SerializeField]
	public bool m_bUseTransformPositions;

	// Token: 0x040010BA RID: 4282
	[SerializeField]
	public Vector3[] m_targetPositions;

	// Token: 0x040010BB RID: 4283
	[SerializeField]
	public Transform[] m_transformTargetPositions;

	// Token: 0x040010BC RID: 4284
	[SerializeField]
	public GameOneShotAudioTag m_spawnAudioTag = GameOneShotAudioTag.FireProjectiles;

	// Token: 0x040010BD RID: 4285
	[Space]
	[SerializeField]
	public string m_fireTrigger;

	// Token: 0x040010BE RID: 4286
	[SerializeField]
	public string m_fireAnimTrigger;

	// Token: 0x040010BF RID: 4287
	[SerializeField]
	public string m_reachedTargetTrigger;

	// Token: 0x040010C0 RID: 4288
	[SerializeField]
	public string m_collidedTrigger;

	// Token: 0x040010C1 RID: 4289
	[Space]
	[SerializeField]
	public bool m_randomTargetOrder = true;

	// Token: 0x040010C2 RID: 4290
	[SerializeField]
	public bool m_alignTargetsToGrid = true;

	// Token: 0x020004A2 RID: 1186
	public enum FireMode
	{
		// Token: 0x040010C4 RID: 4292
		Direct,
		// Token: 0x040010C5 RID: 4293
		Parabolic
	}
}
