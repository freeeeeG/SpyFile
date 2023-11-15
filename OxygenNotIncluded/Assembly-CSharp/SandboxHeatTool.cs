using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000828 RID: 2088
public class SandboxHeatTool : BrushTool
{
	// Token: 0x06003C8D RID: 15501 RVA: 0x0014F870 File Offset: 0x0014DA70
	public static void DestroyInstance()
	{
		SandboxHeatTool.instance = null;
	}

	// Token: 0x1700044C RID: 1100
	// (get) Token: 0x06003C8E RID: 15502 RVA: 0x0014F878 File Offset: 0x0014DA78
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06003C8F RID: 15503 RVA: 0x0014F884 File Offset: 0x0014DA84
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxHeatTool.instance = this;
		this.viewMode = OverlayModes.Temperature.ID;
	}

	// Token: 0x06003C90 RID: 15504 RVA: 0x0014F89D File Offset: 0x0014DA9D
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x06003C91 RID: 15505 RVA: 0x0014F8A4 File Offset: 0x0014DAA4
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06003C92 RID: 15506 RVA: 0x0014F8B4 File Offset: 0x0014DAB4
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.temperatureAdditiveSlider.row.SetActive(true);
	}

	// Token: 0x06003C93 RID: 15507 RVA: 0x0014F90B File Offset: 0x0014DB0B
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x06003C94 RID: 15508 RVA: 0x0014F924 File Offset: 0x0014DB24
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.recentlyAffectedCells)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.recentlyAffectedCellColor));
		}
		foreach (int cell2 in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell2, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06003C95 RID: 15509 RVA: 0x0014F9DC File Offset: 0x0014DBDC
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06003C96 RID: 15510 RVA: 0x0014F9E8 File Offset: 0x0014DBE8
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		if (this.recentlyAffectedCells.Contains(cell))
		{
			return;
		}
		this.recentlyAffectedCells.Add(cell);
		Game.CallbackInfo item = new Game.CallbackInfo(delegate()
		{
			this.recentlyAffectedCells.Remove(cell);
		}, false);
		int index = Game.Instance.callbackManager.Add(item).index;
		float num = Grid.Temperature[cell];
		num += SandboxToolParameterMenu.instance.settings.GetFloatSetting("SandbosTools.TemperatureAdditive");
		GameUtil.TemperatureUnit temperatureUnit = GameUtil.temperatureUnit;
		if (temperatureUnit != GameUtil.TemperatureUnit.Celsius)
		{
			if (temperatureUnit == GameUtil.TemperatureUnit.Fahrenheit)
			{
				num -= 255.372f;
			}
		}
		else
		{
			num -= 273.15f;
		}
		num = Mathf.Clamp(num, 1f, 9999f);
		int cell2 = cell;
		SimHashes id = Grid.Element[cell].id;
		CellElementEvent sandBoxTool = CellEventLogger.Instance.SandBoxTool;
		float mass = Grid.Mass[cell];
		float temperature = num;
		int callbackIdx = index;
		SimMessages.ReplaceElement(cell2, id, sandBoxTool, mass, temperature, Grid.DiseaseIdx[cell], Grid.DiseaseCount[cell], callbackIdx);
		float currentValue = SandboxToolParameterMenu.instance.temperatureAdditiveSlider.inputField.currentValue;
		KFMOD.PlayUISoundWithLabeledParameter(GlobalAssets.GetSound("SandboxTool_HeatGun", false), "TemperatureSetting", (currentValue <= 0f) ? "Cooling" : "Heating");
	}

	// Token: 0x040027A3 RID: 10147
	public static SandboxHeatTool instance;

	// Token: 0x040027A4 RID: 10148
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x040027A5 RID: 10149
	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);
}
