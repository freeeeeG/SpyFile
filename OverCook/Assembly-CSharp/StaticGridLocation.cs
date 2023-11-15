using System;
using UnityEngine;

// Token: 0x02000593 RID: 1427
[ExecutionDependency(typeof(CampaignKitchenLoaderManager))]
[ExecutionDependency(typeof(CompetitiveKitchenLoaderManager))]
public class StaticGridLocation : MonoBehaviour, IGridLocation
{
	// Token: 0x1700025A RID: 602
	// (get) Token: 0x06001B0D RID: 6925 RVA: 0x000855F0 File Offset: 0x000839F0
	public GridIndex GridIndex
	{
		get
		{
			return this.m_gridIndex;
		}
	}

	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06001B0E RID: 6926 RVA: 0x000855F8 File Offset: 0x000839F8
	public GridManager AccessGridManager
	{
		get
		{
			return this.m_gridManager;
		}
	}

	// Token: 0x06001B0F RID: 6927 RVA: 0x00085600 File Offset: 0x00083A00
	private void Awake()
	{
		this.Initialise();
	}

	// Token: 0x06001B10 RID: 6928 RVA: 0x00085608 File Offset: 0x00083A08
	public void Initialise()
	{
		this.m_gridManager = GameUtils.GetGridManager(base.transform);
		this.m_gridIndex = this.m_gridManager.GetGridLocationFromPos(base.transform.position);
		this.m_gridManager.OccupyGrid(base.gameObject, this.m_gridIndex);
	}

	// Token: 0x06001B11 RID: 6929 RVA: 0x00085659 File Offset: 0x00083A59
	private void OnTrigger(string _message)
	{
		if (_message == "destroyed")
		{
			this.OnDestroy();
		}
	}

	// Token: 0x06001B12 RID: 6930 RVA: 0x00085671 File Offset: 0x00083A71
	protected virtual void OnEnable()
	{
	}

	// Token: 0x06001B13 RID: 6931 RVA: 0x00085673 File Offset: 0x00083A73
	protected virtual void OnDisable()
	{
	}

	// Token: 0x06001B14 RID: 6932 RVA: 0x00085678 File Offset: 0x00083A78
	private void OnDestroy()
	{
		if (this.m_gridManager != null)
		{
			if (this.m_gridManager.GetGridOccupant(this.m_gridIndex) == base.gameObject)
			{
				this.m_gridManager.DeoccupyGrid(this.m_gridIndex);
			}
			this.m_gridManager = null;
		}
	}

	// Token: 0x06001B15 RID: 6933 RVA: 0x000856CF File Offset: 0x00083ACF
	public bool IsGridOccupant()
	{
		return this.m_gridManager != null && this.m_gridManager.GetGridOccupant(this.m_gridIndex) == base.gameObject;
	}

	// Token: 0x0400155A RID: 5466
	protected GridManager m_gridManager;

	// Token: 0x0400155B RID: 5467
	protected GridIndex m_gridIndex;
}
