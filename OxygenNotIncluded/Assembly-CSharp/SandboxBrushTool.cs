using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Klei.AI;
using UnityEngine;

// Token: 0x02000822 RID: 2082
public class SandboxBrushTool : BrushTool
{
	// Token: 0x06003C3E RID: 15422 RVA: 0x0014E1A5 File Offset: 0x0014C3A5
	public static void DestroyInstance()
	{
		SandboxBrushTool.instance = null;
	}

	// Token: 0x17000447 RID: 1095
	// (get) Token: 0x06003C3F RID: 15423 RVA: 0x0014E1AD File Offset: 0x0014C3AD
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06003C40 RID: 15424 RVA: 0x0014E1B9 File Offset: 0x0014C3B9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxBrushTool.instance = this;
	}

	// Token: 0x06003C41 RID: 15425 RVA: 0x0014E1C7 File Offset: 0x0014C3C7
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06003C42 RID: 15426 RVA: 0x0014E1D4 File Offset: 0x0014C3D4
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.massSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.temperatureSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.elementSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseCountSlider.row.SetActive(true);
		SandboxToolParameterMenu.SelectorValue elementSelector = SandboxToolParameterMenu.instance.elementSelector;
		elementSelector.onValueChanged = (Action<object>)Delegate.Combine(elementSelector.onValueChanged, new Action<object>(this.OnElementChanged));
	}

	// Token: 0x06003C43 RID: 15427 RVA: 0x0014E2AA File Offset: 0x0014C4AA
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
		this.audioEvent.release();
	}

	// Token: 0x06003C44 RID: 15428 RVA: 0x0014E2D0 File Offset: 0x0014C4D0
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int num in this.recentlyAffectedCells)
		{
			Color color = new Color(this.recentAffectedCellColor[num].r, this.recentAffectedCellColor[num].g, this.recentAffectedCellColor[num].b, MathUtil.ReRange(Mathf.Sin(Time.realtimeSinceStartup * 10f), -1f, 1f, 0.1f, 0.2f));
			colors.Add(new ToolMenu.CellColorData(num, color));
		}
		foreach (int cell in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06003C45 RID: 15429 RVA: 0x0014E3E8 File Offset: 0x0014C5E8
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
					this.brushOffsets.Add(new Vector2((float)(i - this.brushRadius), (float)(j - this.brushRadius)));
				}
			}
		}
	}

	// Token: 0x06003C46 RID: 15430 RVA: 0x0014E480 File Offset: 0x0014C680
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
	}

	// Token: 0x06003C47 RID: 15431 RVA: 0x0014E61C File Offset: 0x0014C81C
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

	// Token: 0x06003C48 RID: 15432 RVA: 0x0014E663 File Offset: 0x0014C863
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click", false));
	}

	// Token: 0x06003C49 RID: 15433 RVA: 0x0014E67C File Offset: 0x0014C87C
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		base.OnLeftClickUp(cursor_pos);
		this.StopSound();
	}

	// Token: 0x06003C4A RID: 15434 RVA: 0x0014E68B File Offset: 0x0014C88B
	private void OnElementChanged(object new_element)
	{
		this.clearVisitedCells();
	}

	// Token: 0x06003C4B RID: 15435 RVA: 0x0014E694 File Offset: 0x0014C894
	protected override string GetDragSound()
	{
		string str = (ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")].state & Element.State.Solid).ToString();
		return "SandboxTool_Brush_" + str + "_Add";
	}

	// Token: 0x06003C4C RID: 15436 RVA: 0x0014E6E4 File Offset: 0x0014C8E4
	protected override void PlaySound()
	{
		base.PlaySound();
		Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
		string sound;
		switch (element.state & Element.State.Solid)
		{
		case Element.State.Vacuum:
			sound = GlobalAssets.GetSound("SandboxTool_Brush_Gas", false);
			break;
		case Element.State.Gas:
			sound = GlobalAssets.GetSound("SandboxTool_Brush_Gas", false);
			break;
		case Element.State.Liquid:
			sound = GlobalAssets.GetSound("SandboxTool_Brush_Liquid", false);
			break;
		case Element.State.Solid:
			sound = GlobalAssets.GetSound("Brush_" + element.substance.GetOreBumpSound(), false);
			if (sound == null)
			{
				sound = GlobalAssets.GetSound("Brush_Rock", false);
			}
			break;
		default:
			sound = GlobalAssets.GetSound("Brush_Rock", false);
			break;
		}
		this.audioEvent = KFMOD.CreateInstance(sound);
		ATTRIBUTES_3D attributes = SoundListenerController.Instance.transform.GetPosition().To3DAttributes();
		this.audioEvent.set3DAttributes(attributes);
		this.audioEvent.start();
	}

	// Token: 0x06003C4D RID: 15437 RVA: 0x0014E7D0 File Offset: 0x0014C9D0
	private void StopSound()
	{
		this.audioEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.audioEvent.release();
	}

	// Token: 0x0400278F RID: 10127
	public static SandboxBrushTool instance;

	// Token: 0x04002790 RID: 10128
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x04002791 RID: 10129
	private Dictionary<int, Color> recentAffectedCellColor = new Dictionary<int, Color>();

	// Token: 0x04002792 RID: 10130
	private EventInstance audioEvent;
}
