using System;
using UnityEngine;

// Token: 0x02000513 RID: 1299
public class MeteorManager : MonoBehaviour
{
	// Token: 0x0600183A RID: 6202 RVA: 0x0007AF56 File Offset: 0x00079356
	private void Awake()
	{
		this.ResetTimer();
		this.m_gridManager = GameUtils.GetGridManager(base.transform);
	}

	// Token: 0x0600183B RID: 6203 RVA: 0x0007AF6F File Offset: 0x0007936F
	private void ResetTimer()
	{
		this.m_timer = UnityEngine.Random.Range(this.m_minTimePeriod, this.m_maxTimePeriod);
	}

	// Token: 0x0600183C RID: 6204 RVA: 0x0007AF88 File Offset: 0x00079388
	private void Update()
	{
		this.m_timer -= TimeManager.GetDeltaTime(base.gameObject.layer);
		if (this.m_timer < 0f)
		{
			int x = UnityEngine.Random.Range(this.m_minGridIndex.X, this.m_maxGridIndex.X + 1);
			int y = UnityEngine.Random.Range(this.m_minGridIndex.Y, this.m_maxGridIndex.Y + 1);
			int z = UnityEngine.Random.Range(this.m_minGridIndex.Z, this.m_maxGridIndex.Z + 1);
			Vector3 posFromGridLocation = this.m_gridManager.GetPosFromGridLocation(new GridIndex(x, y, z));
			this.m_meteorPrefab.Instantiate(posFromGridLocation, Quaternion.identity);
			this.ResetTimer();
		}
	}

	// Token: 0x0400137A RID: 4986
	[SerializeField]
	private GameObject m_meteorPrefab;

	// Token: 0x0400137B RID: 4987
	[SerializeField]
	private Point3 m_minGridIndex;

	// Token: 0x0400137C RID: 4988
	[SerializeField]
	private Point3 m_maxGridIndex;

	// Token: 0x0400137D RID: 4989
	[SerializeField]
	private float m_minTimePeriod;

	// Token: 0x0400137E RID: 4990
	[SerializeField]
	private float m_maxTimePeriod;

	// Token: 0x0400137F RID: 4991
	private GridManager m_gridManager;

	// Token: 0x04001380 RID: 4992
	private float m_timer;
}
