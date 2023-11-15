using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Klei.AI;
using UnityEngine;

// Token: 0x02000827 RID: 2087
public class SandboxFloodTool : FloodTool
{
	// Token: 0x06003C7F RID: 15487 RVA: 0x0014F343 File Offset: 0x0014D543
	public static void DestroyInstance()
	{
		SandboxFloodTool.instance = null;
	}

	// Token: 0x06003C80 RID: 15488 RVA: 0x0014F34B File Offset: 0x0014D54B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxFloodTool.instance = this;
		this.floodCriteria = ((int cell) => Grid.IsValidCell(cell) && Grid.Element[cell] == Grid.Element[this.mouseCell] && Grid.WorldIdx[cell] == Grid.WorldIdx[this.mouseCell]);
		this.paintArea = delegate(HashSet<int> cells)
		{
			foreach (int cell in cells)
			{
				this.PaintCell(cell);
			}
		};
	}

	// Token: 0x06003C81 RID: 15489 RVA: 0x0014F380 File Offset: 0x0014D580
	private void PaintCell(int cell)
	{
		this.recentlyAffectedCells.Add(cell);
		Game.CallbackInfo item = new Game.CallbackInfo(delegate()
		{
			this.recentlyAffectedCells.Remove(cell);
		}, false);
		Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
		byte index = Db.Get().Diseases.GetIndex(Db.Get().Diseases.Get("FoodPoisoning").id);
		Disease disease = Db.Get().Diseases.TryGet(this.settings.GetStringSetting("SandboxTools.SelectedDisease"));
		if (disease != null)
		{
			index = Db.Get().Diseases.GetIndex(disease.id);
		}
		int index2 = Game.Instance.callbackManager.Add(item).index;
		int cell2 = cell;
		SimHashes id = element.id;
		CellElementEvent sandBoxTool = CellEventLogger.Instance.SandBoxTool;
		float floatSetting = this.settings.GetFloatSetting("SandboxTools.Mass");
		float floatSetting2 = this.settings.GetFloatSetting("SandbosTools.Temperature");
		int callbackIdx = index2;
		SimMessages.ReplaceElement(cell2, id, sandBoxTool, floatSetting, floatSetting2, index, this.settings.GetIntSetting("SandboxTools.DiseaseCount"), callbackIdx);
	}

	// Token: 0x1700044B RID: 1099
	// (get) Token: 0x06003C82 RID: 15490 RVA: 0x0014F4B4 File Offset: 0x0014D6B4
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06003C83 RID: 15491 RVA: 0x0014F4C0 File Offset: 0x0014D6C0
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06003C84 RID: 15492 RVA: 0x0014F4D0 File Offset: 0x0014D6D0
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.massSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.temperatureSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.elementSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseCountSlider.row.SetActive(true);
	}

	// Token: 0x06003C85 RID: 15493 RVA: 0x0014F566 File Offset: 0x0014D766
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
		this.ev.release();
	}

	// Token: 0x06003C86 RID: 15494 RVA: 0x0014F58C File Offset: 0x0014D78C
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.recentlyAffectedCells)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.recentlyAffectedCellColor));
		}
		foreach (int cell2 in this.cellsToAffect)
		{
			colors.Add(new ToolMenu.CellColorData(cell2, this.areaColour));
		}
	}

	// Token: 0x06003C87 RID: 15495 RVA: 0x0014F648 File Offset: 0x0014D848
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		this.cellsToAffect = base.Flood(Grid.PosToCell(cursorPos));
	}

	// Token: 0x06003C88 RID: 15496 RVA: 0x0014F664 File Offset: 0x0014D864
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
		string sound;
		if (element.IsSolid)
		{
			sound = GlobalAssets.GetSound("Break_" + element.substance.GetMiningBreakSound(), false);
			if (sound == null)
			{
				sound = GlobalAssets.GetSound("Break_Rock", false);
			}
		}
		else if (element.IsGas)
		{
			sound = GlobalAssets.GetSound("SandboxTool_Bucket_Gas", false);
		}
		else if (element.IsLiquid)
		{
			sound = GlobalAssets.GetSound("SandboxTool_Bucket_Liquid", false);
		}
		else
		{
			sound = GlobalAssets.GetSound("Break_Rock", false);
		}
		this.ev = KFMOD.CreateInstance(sound);
		ATTRIBUTES_3D attributes = SoundListenerController.Instance.transform.GetPosition().To3DAttributes();
		this.ev.set3DAttributes(attributes);
		this.ev.setParameterByName("SandboxToggle", 1f, false);
		this.ev.start();
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Bucket", false));
	}

	// Token: 0x06003C89 RID: 15497 RVA: 0x0014F764 File Offset: 0x0014D964
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

	// Token: 0x0400279E RID: 10142
	public static SandboxFloodTool instance;

	// Token: 0x0400279F RID: 10143
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x040027A0 RID: 10144
	protected HashSet<int> cellsToAffect = new HashSet<int>();

	// Token: 0x040027A1 RID: 10145
	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);

	// Token: 0x040027A2 RID: 10146
	private EventInstance ev;
}
