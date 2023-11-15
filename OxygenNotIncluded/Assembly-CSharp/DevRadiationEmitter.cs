using System;
using STRINGS;

// Token: 0x020005EF RID: 1519
public class DevRadiationEmitter : KMonoBehaviour, ISingleSliderControl, ISliderControl
{
	// Token: 0x060025E3 RID: 9699 RVA: 0x000CDDC9 File Offset: 0x000CBFC9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.radiationEmitter != null)
		{
			this.radiationEmitter.SetEmitting(true);
		}
	}

	// Token: 0x17000203 RID: 515
	// (get) Token: 0x060025E4 RID: 9700 RVA: 0x000CDDEB File Offset: 0x000CBFEB
	public string SliderTitleKey
	{
		get
		{
			return BUILDINGS.PREFABS.DEVRADIATIONGENERATOR.NAME;
		}
	}

	// Token: 0x17000204 RID: 516
	// (get) Token: 0x060025E5 RID: 9701 RVA: 0x000CDDF7 File Offset: 0x000CBFF7
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.RADIATION.RADS;
		}
	}

	// Token: 0x060025E6 RID: 9702 RVA: 0x000CDE03 File Offset: 0x000CC003
	public float GetSliderMax(int index)
	{
		return 5000f;
	}

	// Token: 0x060025E7 RID: 9703 RVA: 0x000CDE0A File Offset: 0x000CC00A
	public float GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x060025E8 RID: 9704 RVA: 0x000CDE11 File Offset: 0x000CC011
	public string GetSliderTooltip(int index)
	{
		return "";
	}

	// Token: 0x060025E9 RID: 9705 RVA: 0x000CDE18 File Offset: 0x000CC018
	public string GetSliderTooltipKey(int index)
	{
		return "";
	}

	// Token: 0x060025EA RID: 9706 RVA: 0x000CDE1F File Offset: 0x000CC01F
	public float GetSliderValue(int index)
	{
		return this.radiationEmitter.emitRads;
	}

	// Token: 0x060025EB RID: 9707 RVA: 0x000CDE2C File Offset: 0x000CC02C
	public void SetSliderValue(float value, int index)
	{
		this.radiationEmitter.emitRads = value;
		this.radiationEmitter.Refresh();
	}

	// Token: 0x060025EC RID: 9708 RVA: 0x000CDE45 File Offset: 0x000CC045
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x040015A2 RID: 5538
	[MyCmpReq]
	private RadiationEmitter radiationEmitter;
}
