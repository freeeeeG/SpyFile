using System;
using UnityEngine;

// Token: 0x02000630 RID: 1584
[AddComponentMenu("Scripts/Game/Environment/WashingStation")]
[RequireComponent(typeof(Interactable))]
public class WashingStation : MonoBehaviour
{
	// Token: 0x06001E1C RID: 7708 RVA: 0x00091E28 File Offset: 0x00090228
	public bool CanHandlePlacement(ICarrier _carrier, Vector2 _directionXZ, int _plateCount)
	{
		GameObject gameObject = _carrier.InspectCarriedItem();
		DirtyPlateStack x = (!(gameObject != null)) ? null : gameObject.GetComponent<DirtyPlateStack>();
		return x != null;
	}

	// Token: 0x04001739 RID: 5945
	[SerializeField]
	public PlateReturnStation m_dryingStation;

	// Token: 0x0400173A RID: 5946
	[SerializeField]
	public float m_cleanPlateTime = 2f;

	// Token: 0x0400173B RID: 5947
	[SerializeField]
	public ProgressUIController m_progressUIPrefab;

	// Token: 0x0400173C RID: 5948
	[SerializeField]
	public GameObject[] m_dirtyPlates = new GameObject[0];
}
