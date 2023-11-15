using System;

// Token: 0x0200057E RID: 1406
public class DynamicGridLocation : StaticGridLocation
{
	// Token: 0x06001AA4 RID: 6820 RVA: 0x00085710 File Offset: 0x00083B10
	private void Update()
	{
		GridIndex gridLocationFromPos = this.m_gridManager.GetGridLocationFromPos(base.transform.position);
		if (this.m_gridIndex != gridLocationFromPos)
		{
			if (this.m_occupying)
			{
				if (this.m_gridManager.GetGridOccupant(this.m_gridIndex) == base.gameObject)
				{
					this.m_gridManager.DeoccupyGrid(this.m_gridIndex);
				}
				this.m_occupying = false;
			}
			if (this.m_gridManager.GetGridOccupant(gridLocationFromPos) == null)
			{
				this.m_occupying = true;
				this.m_gridManager.OccupyGrid(base.gameObject, gridLocationFromPos);
				this.m_gridIndex = gridLocationFromPos;
			}
		}
	}

	// Token: 0x06001AA5 RID: 6821 RVA: 0x000857C0 File Offset: 0x00083BC0
	protected override void OnEnable()
	{
		base.OnEnable();
		if (this.m_gridManager != null && this.m_gridManager.GetGridOccupant(this.m_gridIndex) == null)
		{
			this.m_gridManager.OccupyGrid(base.gameObject, this.m_gridIndex);
		}
	}

	// Token: 0x06001AA6 RID: 6822 RVA: 0x00085818 File Offset: 0x00083C18
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_gridManager != null && this.m_gridManager.GetGridOccupant(this.m_gridIndex) == base.gameObject)
		{
			this.m_gridManager.DeoccupyGrid(this.m_gridIndex);
		}
	}

	// Token: 0x04001514 RID: 5396
	private bool m_occupying = true;
}
