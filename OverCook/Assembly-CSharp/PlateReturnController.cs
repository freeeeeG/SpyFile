using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000761 RID: 1889
public class PlateReturnController
{
	// Token: 0x06002461 RID: 9313 RVA: 0x000ACAFA File Offset: 0x000AAEFA
	public PlateReturnController(ref PlateReturnController.PlateReturnControllerConfig _plateReturnControllerDesc)
	{
		this.m_plateReturnControllerDesc = _plateReturnControllerDesc;
	}

	// Token: 0x06002462 RID: 9314 RVA: 0x000ACB24 File Offset: 0x000AAF24
	public void Init()
	{
		this.m_levelConfig = GameUtils.GetLevelConfig();
		this.m_platesToReturn.Clear();
		this.m_plateReturnStations.Clear();
		GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
		for (int i = 0; i < rootGameObjects.Length; i++)
		{
			ServerPlateReturnStation[] array = rootGameObjects[i].RequestComponentsRecursive<ServerPlateReturnStation>();
			for (int j = 0; j < array.Length; j++)
			{
				MonoBehaviour monoBehaviour = array[j];
				if (monoBehaviour.CompareTag("PlateReturn"))
				{
					this.m_plateReturnStations.Add(array[j]);
				}
			}
		}
	}

	// Token: 0x06002463 RID: 9315 RVA: 0x000ACBBC File Offset: 0x000AAFBC
	public void FoodDelivered(AssembledDefinitionNode _definition, PlatingStepData _plateType, ServerPlateStation _station)
	{
		ServerPlateReturnStation serverPlateReturnStation = _station.GetReturnStation(_plateType);
		if (serverPlateReturnStation != null)
		{
			this.m_platesToReturn.Add(new PlateReturnController.PlatesPendingReturn(serverPlateReturnStation, this.m_plateReturnControllerDesc.m_plateReturnTime, _plateType));
		}
		else if (this.m_plateReturnStations.Count > 0)
		{
			serverPlateReturnStation = this.FindBestReturnStation(_plateType);
			this.m_platesToReturn.Add(new PlateReturnController.PlatesPendingReturn(serverPlateReturnStation, this.m_plateReturnControllerDesc.m_plateReturnTime, _plateType));
		}
	}

	// Token: 0x06002464 RID: 9316 RVA: 0x000ACC38 File Offset: 0x000AB038
	public void Update()
	{
		for (int i = this.m_platesToReturn.Count - 1; i >= 0; i--)
		{
			PlateReturnController.PlatesPendingReturn platesPendingReturn = this.m_platesToReturn._items[i];
			platesPendingReturn.m_timer -= TimeManager.GetDeltaTime(LayerMask.NameToLayer("Default"));
			if (platesPendingReturn.m_timer < 0f)
			{
				if (platesPendingReturn.m_station == null || platesPendingReturn.m_station.gameObject == null || !platesPendingReturn.m_station.gameObject.activeInHierarchy || !this.IsInLevelBounds(platesPendingReturn.m_station))
				{
					platesPendingReturn.m_station = this.FindBestReturnStation(platesPendingReturn.m_platingStepData);
				}
				else if (platesPendingReturn.m_station.CanReturnPlate())
				{
					platesPendingReturn.m_station.ReturnPlate();
					this.m_platesToReturn.RemoveAt(i);
				}
			}
		}
	}

	// Token: 0x06002465 RID: 9317 RVA: 0x000ACD28 File Offset: 0x000AB128
	protected bool IsInLevelBounds(ServerPlateReturnStation _station)
	{
		return (this.m_levelConfig != null && !this.m_levelConfig.m_enableRespawnBounds) || LevelBounds.ActiveBoundsContain(_station.transform.position);
	}

	// Token: 0x06002466 RID: 9318 RVA: 0x000ACD60 File Offset: 0x000AB160
	private ServerPlateReturnStation FindBestReturnStation(PlatingStepData _plateType)
	{
		if (this.m_plateReturnStations.Count > 0)
		{
			return this.m_plateReturnStations.Find((ServerPlateReturnStation x) => x.GetPlatingStep() == _plateType && x.gameObject.activeInHierarchy && this.IsInLevelBounds(x));
		}
		return null;
	}

	// Token: 0x04001BD0 RID: 7120
	private PlateReturnController.PlateReturnControllerConfig m_plateReturnControllerDesc;

	// Token: 0x04001BD1 RID: 7121
	private FastList<ServerPlateReturnStation> m_plateReturnStations = new FastList<ServerPlateReturnStation>();

	// Token: 0x04001BD2 RID: 7122
	private FastList<PlateReturnController.PlatesPendingReturn> m_platesToReturn = new FastList<PlateReturnController.PlatesPendingReturn>();

	// Token: 0x04001BD3 RID: 7123
	private LevelConfigBase m_levelConfig;

	// Token: 0x02000762 RID: 1890
	public struct PlateReturnControllerConfig
	{
		// Token: 0x04001BD4 RID: 7124
		public float m_plateReturnTime;
	}

	// Token: 0x02000763 RID: 1891
	private class PlatesPendingReturn
	{
		// Token: 0x06002467 RID: 9319 RVA: 0x000ACDAB File Offset: 0x000AB1AB
		public PlatesPendingReturn(ServerPlateReturnStation _returnStation, float _delay, PlatingStepData platingStepData)
		{
			this.m_station = _returnStation;
			this.m_timer = _delay;
			this.m_platingStepData = platingStepData;
		}

		// Token: 0x04001BD5 RID: 7125
		public ServerPlateReturnStation m_station;

		// Token: 0x04001BD6 RID: 7126
		public float m_timer;

		// Token: 0x04001BD7 RID: 7127
		public PlatingStepData m_platingStepData;
	}
}
