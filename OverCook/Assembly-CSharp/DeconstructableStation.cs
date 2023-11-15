using System;
using UnityEngine;

// Token: 0x0200057D RID: 1405
public class DeconstructableStation : MonoBehaviour, IHandleBuild
{
	// Token: 0x06001AA0 RID: 6816 RVA: 0x00085551 File Offset: 0x00083951
	public bool CanHandleBuild()
	{
		return true;
	}

	// Token: 0x06001AA1 RID: 6817 RVA: 0x00085554 File Offset: 0x00083954
	public void HandleBuild(PlayerControls _controls)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.DeconstructedStationPrefab);
		gameObject.name = this.DeconstructedStationPrefab.name;
		gameObject.transform.SetParent(base.gameObject.transform.parent, false);
		gameObject.transform.position = base.gameObject.transform.position;
		gameObject.transform.rotation = base.gameObject.transform.rotation;
		base.gameObject.Destroy();
	}

	// Token: 0x06001AA2 RID: 6818 RVA: 0x000855DB File Offset: 0x000839DB
	private void Awake()
	{
		this.m_flowController = GameUtils.GetFlowController();
	}

	// Token: 0x04001511 RID: 5393
	public StationData StationData;

	// Token: 0x04001512 RID: 5394
	public GameObject DeconstructedStationPrefab;

	// Token: 0x04001513 RID: 5395
	private IFlowController m_flowController;
}
