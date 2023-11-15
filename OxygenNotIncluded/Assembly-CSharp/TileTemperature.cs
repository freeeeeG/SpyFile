using System;
using UnityEngine;

// Token: 0x02000A05 RID: 2565
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/TileTemperature")]
public class TileTemperature : KMonoBehaviour
{
	// Token: 0x06004CAB RID: 19627 RVA: 0x001AE04C File Offset: 0x001AC24C
	protected override void OnPrefabInit()
	{
		this.primaryElement.getTemperatureCallback = new PrimaryElement.GetTemperatureCallback(TileTemperature.OnGetTemperature);
		this.primaryElement.setTemperatureCallback = new PrimaryElement.SetTemperatureCallback(TileTemperature.OnSetTemperature);
		base.OnPrefabInit();
	}

	// Token: 0x06004CAC RID: 19628 RVA: 0x001AE082 File Offset: 0x001AC282
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06004CAD RID: 19629 RVA: 0x001AE08C File Offset: 0x001AC28C
	private static float OnGetTemperature(PrimaryElement primary_element)
	{
		SimCellOccupier component = primary_element.GetComponent<SimCellOccupier>();
		if (component != null && component.IsReady())
		{
			int i = Grid.PosToCell(primary_element.transform.GetPosition());
			return Grid.Temperature[i];
		}
		return primary_element.InternalTemperature;
	}

	// Token: 0x06004CAE RID: 19630 RVA: 0x001AE0D4 File Offset: 0x001AC2D4
	private static void OnSetTemperature(PrimaryElement primary_element, float temperature)
	{
		SimCellOccupier component = primary_element.GetComponent<SimCellOccupier>();
		if (component != null && component.IsReady())
		{
			global::Debug.LogWarning("Only set a tile's temperature during initialization. Otherwise you should be modifying the cell via the sim!");
			return;
		}
		primary_element.InternalTemperature = temperature;
	}

	// Token: 0x04003205 RID: 12805
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x04003206 RID: 12806
	[MyCmpReq]
	private KSelectable selectable;
}
