using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x0200082B RID: 2091
public class SandboxSprinkleTool : BrushTool
{
	// Token: 0x06003CAA RID: 15530 RVA: 0x00150250 File Offset: 0x0014E450
	public static void DestroyInstance()
	{
		SandboxSprinkleTool.instance = null;
	}

	// Token: 0x1700044D RID: 1101
	// (get) Token: 0x06003CAB RID: 15531 RVA: 0x00150258 File Offset: 0x0014E458
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06003CAC RID: 15532 RVA: 0x00150264 File Offset: 0x0014E464
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxSprinkleTool.instance = this;
	}

	// Token: 0x06003CAD RID: 15533 RVA: 0x00150272 File Offset: 0x0014E472
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06003CAE RID: 15534 RVA: 0x00150280 File Offset: 0x0014E480
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.noiseScaleSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.noiseDensitySlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.massSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.temperatureSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.elementSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseCountSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.brushRadiusSlider.SetValue((float)this.settings.GetIntSetting("SandboxTools.BrushSize"), true);
	}

	// Token: 0x06003CAF RID: 15535 RVA: 0x00150376 File Offset: 0x0014E576
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x06003CB0 RID: 15536 RVA: 0x00150390 File Offset: 0x0014E590
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int num in this.recentlyAffectedCells)
		{
			Color color = new Color(this.recentAffectedCellColor[num].r, this.recentAffectedCellColor[num].g, this.recentAffectedCellColor[num].b, MathUtil.ReRange(Mathf.Sin(Time.realtimeSinceStartup * 5f), -1f, 1f, 0.1f, 0.2f));
			colors.Add(new ToolMenu.CellColorData(num, color));
		}
		foreach (int num2 in this.cellsInRadius)
		{
			if (this.recentlyAffectedCells.Contains(num2))
			{
				Color radiusIndicatorColor = this.radiusIndicatorColor;
				Color color2 = this.recentAffectedCellColor[num2];
				color2.a = 0.2f;
				Color color3 = new Color((radiusIndicatorColor.r + color2.r) / 2f, (radiusIndicatorColor.g + color2.g) / 2f, (radiusIndicatorColor.b + color2.b) / 2f, radiusIndicatorColor.a + (1f - radiusIndicatorColor.a) * color2.a);
				colors.Add(new ToolMenu.CellColorData(num2, color3));
			}
			else
			{
				colors.Add(new ToolMenu.CellColorData(num2, this.radiusIndicatorColor));
			}
		}
	}

	// Token: 0x06003CB1 RID: 15537 RVA: 0x00150558 File Offset: 0x0014E758
	public override void SetBrushSize(int radius)
	{
		this.brushRadius = radius;
		this.brushOffsets.Clear();
		for (int i = 0; i < this.brushRadius * 2; i++)
		{
			for (int j = 0; j < this.brushRadius * 2; j++)
			{
				if (Vector2.Distance(new Vector2((float)i, (float)j), new Vector2((float)this.brushRadius, (float)this.brushRadius)) < (float)this.brushRadius - 0.8f)
				{
					Vector2 vector = Grid.CellToXY(Grid.OffsetCell(this.currentCell, i, j));
					float num = PerlinSimplexNoise.noise(vector.x / this.settings.GetFloatSetting("SandboxTools.NoiseDensity"), vector.y / this.settings.GetFloatSetting("SandboxTools.NoiseDensity"), Time.realtimeSinceStartup);
					if (this.settings.GetFloatSetting("SandboxTools.NoiseScale") <= num)
					{
						this.brushOffsets.Add(new Vector2((float)(i - this.brushRadius), (float)(j - this.brushRadius)));
					}
				}
			}
		}
	}

	// Token: 0x06003CB2 RID: 15538 RVA: 0x00150662 File Offset: 0x0014E862
	private void Update()
	{
		this.OnMouseMove(Grid.CellToPos(this.currentCell));
	}

	// Token: 0x06003CB3 RID: 15539 RVA: 0x00150675 File Offset: 0x0014E875
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		this.SetBrushSize(this.settings.GetIntSetting("SandboxTools.BrushSize"));
	}

	// Token: 0x06003CB4 RID: 15540 RVA: 0x00150694 File Offset: 0x0014E894
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		this.recentlyAffectedCells.Add(cell);
		Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
		if (!this.recentAffectedCellColor.ContainsKey(cell))
		{
			this.recentAffectedCellColor.Add(cell, element.substance.uiColour);
		}
		else
		{
			this.recentAffectedCellColor[cell] = element.substance.uiColour;
		}
		Game.CallbackInfo item = new Game.CallbackInfo(delegate()
		{
			this.recentlyAffectedCells.Remove(cell);
			this.recentAffectedCellColor.Remove(cell);
		}, false);
		int index = Game.Instance.callbackManager.Add(item).index;
		byte index2 = Db.Get().Diseases.GetIndex(Db.Get().Diseases.Get("FoodPoisoning").id);
		Disease disease = Db.Get().Diseases.TryGet(this.settings.GetStringSetting("SandboxTools.SelectedDisease"));
		if (disease != null)
		{
			index2 = Db.Get().Diseases.GetIndex(disease.id);
		}
		int cell2 = cell;
		SimHashes id = element.id;
		CellElementEvent sandBoxTool = CellEventLogger.Instance.SandBoxTool;
		float floatSetting = this.settings.GetFloatSetting("SandboxTools.Mass");
		float floatSetting2 = this.settings.GetFloatSetting("SandbosTools.Temperature");
		int callbackIdx = index;
		SimMessages.ReplaceElement(cell2, id, sandBoxTool, floatSetting, floatSetting2, index2, this.settings.GetIntSetting("SandboxTools.DiseaseCount"), callbackIdx);
		this.SetBrushSize(this.brushRadius);
	}

	// Token: 0x06003CB5 RID: 15541 RVA: 0x0015083C File Offset: 0x0014EA3C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.SandboxCopyElement))
		{
			int cell = Grid.PosToCell(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
			if (Grid.IsValidCell(cell))
			{
				SandboxSampleTool.Sample(cell);
			}
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x040027AD RID: 10157
	public static SandboxSprinkleTool instance;

	// Token: 0x040027AE RID: 10158
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x040027AF RID: 10159
	private Dictionary<int, Color> recentAffectedCellColor = new Dictionary<int, Color>();
}
