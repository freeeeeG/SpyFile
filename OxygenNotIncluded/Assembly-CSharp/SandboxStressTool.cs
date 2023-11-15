using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200082C RID: 2092
public class SandboxStressTool : BrushTool
{
	// Token: 0x06003CB7 RID: 15543 RVA: 0x001508A1 File Offset: 0x0014EAA1
	public static void DestroyInstance()
	{
		SandboxStressTool.instance = null;
	}

	// Token: 0x1700044E RID: 1102
	// (get) Token: 0x06003CB8 RID: 15544 RVA: 0x001508A9 File Offset: 0x0014EAA9
	public override string[] DlcIDs
	{
		get
		{
			return DlcManager.AVAILABLE_ALL_VERSIONS;
		}
	}

	// Token: 0x1700044F RID: 1103
	// (get) Token: 0x06003CB9 RID: 15545 RVA: 0x001508B0 File Offset: 0x0014EAB0
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06003CBA RID: 15546 RVA: 0x001508BC File Offset: 0x0014EABC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxStressTool.instance = this;
	}

	// Token: 0x06003CBB RID: 15547 RVA: 0x001508CA File Offset: 0x0014EACA
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x06003CBC RID: 15548 RVA: 0x001508D1 File Offset: 0x0014EAD1
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06003CBD RID: 15549 RVA: 0x001508E0 File Offset: 0x0014EAE0
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.stressAdditiveSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.stressAdditiveSlider.SetValue(5f, true);
		SandboxToolParameterMenu.instance.moraleSlider.SetValue(0f, true);
		if (DebugHandler.InstantBuildMode)
		{
			SandboxToolParameterMenu.instance.moraleSlider.row.SetActive(true);
		}
	}

	// Token: 0x06003CBE RID: 15550 RVA: 0x0015097D File Offset: 0x0014EB7D
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
		this.StopSound();
	}

	// Token: 0x06003CBF RID: 15551 RVA: 0x0015099C File Offset: 0x0014EB9C
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

	// Token: 0x06003CC0 RID: 15552 RVA: 0x00150A54 File Offset: 0x0014EC54
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06003CC1 RID: 15553 RVA: 0x00150A5D File Offset: 0x0014EC5D
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click", false));
	}

	// Token: 0x06003CC2 RID: 15554 RVA: 0x00150A78 File Offset: 0x0014EC78
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			GameObject gameObject = Components.LiveMinionIdentities[i].gameObject;
			if (Grid.PosToCell(gameObject) == cell)
			{
				float num = -1f * SandboxToolParameterMenu.instance.settings.GetFloatSetting("SandbosTools.StressAdditive");
				Db.Get().Amounts.Stress.Lookup(Components.LiveMinionIdentities[i].gameObject).ApplyDelta(num);
				if (num >= 0f)
				{
					PopFXManager.Instance.SpawnFX(Assets.GetSprite("crew_state_angry"), GameUtil.GetFormattedPercent(num, GameUtil.TimeSlice.None), gameObject.transform, 1.5f, false);
				}
				else
				{
					PopFXManager.Instance.SpawnFX(Assets.GetSprite("crew_state_happy"), GameUtil.GetFormattedPercent(num, GameUtil.TimeSlice.None), gameObject.transform, 1.5f, false);
				}
				this.PlaySound(num, gameObject.transform.GetPosition());
				int intSetting = SandboxToolParameterMenu.instance.settings.GetIntSetting("SandbosTools.MoraleAdjustment");
				AttributeInstance attributeInstance = gameObject.GetAttributes().Get(Db.Get().Attributes.QualityOfLife);
				MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
				if (this.moraleAdjustments.ContainsKey(component))
				{
					attributeInstance.Remove(this.moraleAdjustments[component]);
					this.moraleAdjustments.Remove(component);
				}
				if (intSetting != 0)
				{
					AttributeModifier attributeModifier = new AttributeModifier(attributeInstance.Id, (float)intSetting, () => DUPLICANTS.MODIFIERS.SANDBOXMORALEADJUSTMENT.NAME, false, false);
					attributeModifier.SetValue((float)intSetting);
					attributeInstance.Add(attributeModifier);
					this.moraleAdjustments.Add(component, attributeModifier);
				}
			}
		}
	}

	// Token: 0x06003CC3 RID: 15555 RVA: 0x00150C40 File Offset: 0x0014EE40
	private void PlaySound(float sliderValue, Vector3 position)
	{
		this.ev = KFMOD.CreateInstance(this.UISoundPath);
		ATTRIBUTES_3D attributes = position.To3DAttributes();
		this.ev.set3DAttributes(attributes);
		this.ev.setParameterByNameWithLabel("SanboxTool_Effect", (sliderValue >= 0f) ? "Decrease" : "Increase", false);
		this.ev.start();
	}

	// Token: 0x06003CC4 RID: 15556 RVA: 0x00150CA4 File Offset: 0x0014EEA4
	private void StopSound()
	{
		this.ev.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.ev.release();
	}

	// Token: 0x040027B0 RID: 10160
	public static SandboxStressTool instance;

	// Token: 0x040027B1 RID: 10161
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x040027B2 RID: 10162
	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);

	// Token: 0x040027B3 RID: 10163
	private string UISoundPath = GlobalAssets.GetSound("SandboxTool_Happy", false);

	// Token: 0x040027B4 RID: 10164
	private EventInstance ev;

	// Token: 0x040027B5 RID: 10165
	private Dictionary<MinionIdentity, AttributeModifier> moraleAdjustments = new Dictionary<MinionIdentity, AttributeModifier>();
}
