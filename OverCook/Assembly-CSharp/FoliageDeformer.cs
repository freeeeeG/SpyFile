using System;
using UnityEngine;

// Token: 0x02000795 RID: 1941
[AddComponentMenu("Scripts/Game/Environment/FoliageDeformer")]
public class FoliageDeformer : MonoBehaviour
{
	// Token: 0x06002588 RID: 9608 RVA: 0x000B1A50 File Offset: 0x000AFE50
	private void Update()
	{
		int num = Physics.OverlapSphereNonAlloc(base.transform.position, this.m_radiusOfEffect, FoliageDeformer.ms_colliders, this.m_layerToEffect.value);
		for (int i = 0; i < num; i++)
		{
			Collider collider = FoliageDeformer.ms_colliders[i];
			GameObject gameObject = collider.gameObject;
			DeformingFoliage deformingFoliage = gameObject.GetComponent<DeformingFoliage>();
			if (deformingFoliage == null)
			{
				deformingFoliage = gameObject.AddComponent<DeformingFoliage>();
				deformingFoliage.WobbleTime = this.m_wobbleTime;
				deformingFoliage.MinWobbleAmount = this.m_wobbleAmount;
				deformingFoliage.MaxWobbleAmount = this.m_wobbleAmount;
			}
			deformingFoliage.AddDeformer(base.transform);
		}
	}

	// Token: 0x04001D1A RID: 7450
	[SerializeField]
	private float m_radiusOfEffect = 2f;

	// Token: 0x04001D1B RID: 7451
	[SerializeField]
	private LayerMask m_layerToEffect;

	// Token: 0x04001D1C RID: 7452
	[SerializeField]
	private float m_wobbleTime = 0.5f;

	// Token: 0x04001D1D RID: 7453
	[SerializeField]
	private float m_wobbleAmount = 0.3f;

	// Token: 0x04001D1E RID: 7454
	private static Collider[] ms_colliders = new Collider[30];
}
