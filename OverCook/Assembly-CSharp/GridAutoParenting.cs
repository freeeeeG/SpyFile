using System;
using UnityEngine;

// Token: 0x02000715 RID: 1813
[ExecutionDependency(typeof(IGridLocation))]
public class GridAutoParenting : MonoBehaviour
{
	// Token: 0x06002274 RID: 8820 RVA: 0x000A66C0 File Offset: 0x000A4AC0
	private void Awake()
	{
		this.m_gridManager = GameUtils.GetGridManager(base.transform);
		GridIndex gridLocationFromPos = this.m_gridManager.GetGridLocationFromPos(base.transform.position);
		GameObject gridOccupant = this.m_gridManager.GetGridOccupant(gridLocationFromPos);
		if (gridOccupant != null && gridOccupant.transform != null)
		{
			base.transform.SetParent(gridOccupant.transform, this.m_keepWorldPosition);
		}
	}

	// Token: 0x04001A7F RID: 6783
	private GridManager m_gridManager;

	// Token: 0x04001A80 RID: 6784
	[SerializeField]
	private bool m_keepWorldPosition = true;
}
