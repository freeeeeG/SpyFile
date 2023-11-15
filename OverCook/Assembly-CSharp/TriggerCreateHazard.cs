using System;
using UnityEngine;

// Token: 0x020004A5 RID: 1189
public class TriggerCreateHazard : MonoBehaviour
{
	// Token: 0x06001630 RID: 5680 RVA: 0x00076138 File Offset: 0x00074538
	private GameObject CreateHazard(GameObject _prefab, Transform _transform, bool _alignToGrid = true)
	{
		GridManager gridManager = GameUtils.GetGridManager(_transform);
		GridIndex gridLocationFromPos = gridManager.GetGridLocationFromPos(_transform.position);
		GameObject gridOccupant = gridManager.GetGridOccupant(gridLocationFromPos);
		if (gridOccupant == null || gridOccupant.RequestInterface<HazardBase>() != null)
		{
			GameObject gameObject = _prefab.Instantiate(_transform.position, Quaternion.identity);
			if (_alignToGrid)
			{
				gameObject.transform.position = gridManager.GetPosFromGridLocation(gridLocationFromPos);
			}
			return gameObject;
		}
		return null;
	}

	// Token: 0x06001631 RID: 5681 RVA: 0x000761AB File Offset: 0x000745AB
	private void OnTrigger(string _message)
	{
		if (this.m_spawnTrigger == _message)
		{
			this.CreateHazard(this.m_hazardPrefab, base.transform, this.m_alignToGrid);
		}
	}

	// Token: 0x040010C7 RID: 4295
	[SerializeField]
	private GameObject m_hazardPrefab;

	// Token: 0x040010C8 RID: 4296
	[SerializeField]
	private string m_spawnTrigger;

	// Token: 0x040010C9 RID: 4297
	[SerializeField]
	private bool m_alignToGrid = true;
}
