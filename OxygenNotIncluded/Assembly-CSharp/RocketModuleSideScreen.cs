using System;
using System.Collections;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C41 RID: 3137
public class RocketModuleSideScreen : SideScreenContent
{
	// Token: 0x06006357 RID: 25431 RVA: 0x0024B7C8 File Offset: 0x002499C8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		RocketModuleSideScreen.instance = this;
	}

	// Token: 0x06006358 RID: 25432 RVA: 0x0024B7D6 File Offset: 0x002499D6
	protected override void OnForcedCleanUp()
	{
		RocketModuleSideScreen.instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06006359 RID: 25433 RVA: 0x0024B7E4 File Offset: 0x002499E4
	public override int GetSideScreenSortOrder()
	{
		return 500;
	}

	// Token: 0x0600635A RID: 25434 RVA: 0x0024B7EC File Offset: 0x002499EC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.addNewModuleButton.onClick += delegate()
		{
			Vector2 vector = Vector2.zero;
			if (SelectModuleSideScreen.Instance != null)
			{
				vector = SelectModuleSideScreen.Instance.mainContents.GetComponent<KScrollRect>().content.rectTransform().anchoredPosition;
			}
			this.ClickAddNew(vector.y, null);
		};
		this.removeModuleButton.onClick += this.ClickRemove;
		this.moveModuleUpButton.onClick += this.ClickSwapUp;
		this.moveModuleDownButton.onClick += this.ClickSwapDown;
		this.changeModuleButton.onClick += delegate()
		{
			Vector2 vector = Vector2.zero;
			if (SelectModuleSideScreen.Instance != null)
			{
				vector = SelectModuleSideScreen.Instance.mainContents.GetComponent<KScrollRect>().content.rectTransform().anchoredPosition;
			}
			this.ClickChangeModule(vector.y);
		};
		this.viewInteriorButton.onClick += this.ClickViewInterior;
		this.moduleNameLabel.textStyleSetting = this.nameSetting;
		this.moduleDescriptionLabel.textStyleSetting = this.descriptionSetting;
		this.moduleNameLabel.ApplySettings();
		this.moduleDescriptionLabel.ApplySettings();
	}

	// Token: 0x0600635B RID: 25435 RVA: 0x0024B8C1 File Offset: 0x00249AC1
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		DetailsScreen.Instance.ClearSecondarySideScreen();
	}

	// Token: 0x0600635C RID: 25436 RVA: 0x0024B8D3 File Offset: 0x00249AD3
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x0600635D RID: 25437 RVA: 0x0024B8DB File Offset: 0x00249ADB
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<ReorderableBuilding>() != null;
	}

	// Token: 0x0600635E RID: 25438 RVA: 0x0024B8EC File Offset: 0x00249AEC
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.reorderable = new_target.GetComponent<ReorderableBuilding>();
		this.moduleIcon.sprite = Def.GetUISprite(this.reorderable.gameObject, "ui", false).first;
		this.moduleNameLabel.SetText(this.reorderable.GetProperName());
		this.moduleDescriptionLabel.SetText(this.reorderable.GetComponent<Building>().Desc);
		this.UpdateButtonStates();
	}

	// Token: 0x0600635F RID: 25439 RVA: 0x0024B978 File Offset: 0x00249B78
	public void UpdateButtonStates()
	{
		this.changeModuleButton.isInteractable = this.reorderable.CanChangeModule();
		this.changeModuleButton.GetComponent<ToolTip>().SetSimpleTooltip(this.changeModuleButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONCHANGEMODULE.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONCHANGEMODULE.INVALID.text);
		this.addNewModuleButton.isInteractable = true;
		this.addNewModuleButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.ADDMODULE.DESC.text);
		this.removeModuleButton.isInteractable = this.reorderable.CanRemoveModule();
		this.removeModuleButton.GetComponent<ToolTip>().SetSimpleTooltip(this.removeModuleButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONREMOVEMODULE.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONREMOVEMODULE.INVALID.text);
		this.moveModuleDownButton.isInteractable = this.reorderable.CanSwapDown(true);
		this.moveModuleDownButton.GetComponent<ToolTip>().SetSimpleTooltip(this.moveModuleDownButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONSWAPMODULEDOWN.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONSWAPMODULEDOWN.INVALID.text);
		this.moveModuleUpButton.isInteractable = this.reorderable.CanSwapUp(true);
		this.moveModuleUpButton.GetComponent<ToolTip>().SetSimpleTooltip(this.moveModuleUpButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONSWAPMODULEUP.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONSWAPMODULEUP.INVALID.text);
		ClustercraftExteriorDoor component = this.reorderable.GetComponent<ClustercraftExteriorDoor>();
		if (!(component != null) || !component.HasTargetWorld())
		{
			this.viewInteriorButton.isInteractable = false;
			this.viewInteriorButton.GetComponentInChildren<LocText>().SetText(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWINTERIOR.LABEL);
			this.viewInteriorButton.GetComponent<ToolTip>().SetSimpleTooltip(this.viewInteriorButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWINTERIOR.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWINTERIOR.INVALID.text);
			return;
		}
		if (ClusterManager.Instance.activeWorld == component.GetTargetWorld())
		{
			this.changeModuleButton.isInteractable = false;
			this.addNewModuleButton.isInteractable = false;
			this.removeModuleButton.isInteractable = false;
			this.moveModuleDownButton.isInteractable = false;
			this.moveModuleUpButton.isInteractable = false;
			this.viewInteriorButton.isInteractable = (component.GetMyWorldId() != 255);
			this.viewInteriorButton.GetComponentInChildren<LocText>().SetText(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWEXTERIOR.LABEL);
			this.viewInteriorButton.GetComponent<ToolTip>().SetSimpleTooltip(this.viewInteriorButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWEXTERIOR.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWEXTERIOR.INVALID.text);
			return;
		}
		this.viewInteriorButton.isInteractable = (this.reorderable.GetComponent<PassengerRocketModule>() != null);
		this.viewInteriorButton.GetComponentInChildren<LocText>().SetText(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWINTERIOR.LABEL);
		this.viewInteriorButton.GetComponent<ToolTip>().SetSimpleTooltip(this.viewInteriorButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWINTERIOR.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWINTERIOR.INVALID.text);
	}

	// Token: 0x06006360 RID: 25440 RVA: 0x0024BC78 File Offset: 0x00249E78
	public void ClickAddNew(float scrollViewPosition, BuildingDef autoSelectDef = null)
	{
		SelectModuleSideScreen selectModuleSideScreen = (SelectModuleSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.changeModuleSideScreen, UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.CHANGEMODULEPANEL);
		selectModuleSideScreen.addingNewModule = true;
		selectModuleSideScreen.SetTarget(this.reorderable.gameObject);
		if (autoSelectDef != null)
		{
			selectModuleSideScreen.SelectModule(autoSelectDef);
		}
		this.ScrollToTargetPoint(scrollViewPosition);
	}

	// Token: 0x06006361 RID: 25441 RVA: 0x0024BCD4 File Offset: 0x00249ED4
	private void ScrollToTargetPoint(float scrollViewPosition)
	{
		if (SelectModuleSideScreen.Instance != null)
		{
			SelectModuleSideScreen.Instance.mainContents.GetComponent<KScrollRect>().content.anchoredPosition = new Vector2(0f, scrollViewPosition);
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.DelayedScrollToTargetPoint(scrollViewPosition));
			}
		}
	}

	// Token: 0x06006362 RID: 25442 RVA: 0x0024BD2D File Offset: 0x00249F2D
	private IEnumerator DelayedScrollToTargetPoint(float scrollViewPosition)
	{
		if (SelectModuleSideScreen.Instance != null)
		{
			yield return SequenceUtil.WaitForEndOfFrame;
			SelectModuleSideScreen.Instance.mainContents.GetComponent<KScrollRect>().content.anchoredPosition = new Vector2(0f, scrollViewPosition);
		}
		yield break;
	}

	// Token: 0x06006363 RID: 25443 RVA: 0x0024BD3C File Offset: 0x00249F3C
	private void ClickRemove()
	{
		this.reorderable.Trigger(-790448070, null);
		this.UpdateButtonStates();
	}

	// Token: 0x06006364 RID: 25444 RVA: 0x0024BD55 File Offset: 0x00249F55
	private void ClickSwapUp()
	{
		this.reorderable.SwapWithAbove(true);
		this.UpdateButtonStates();
	}

	// Token: 0x06006365 RID: 25445 RVA: 0x0024BD69 File Offset: 0x00249F69
	private void ClickSwapDown()
	{
		this.reorderable.SwapWithBelow(true);
		this.UpdateButtonStates();
	}

	// Token: 0x06006366 RID: 25446 RVA: 0x0024BD7D File Offset: 0x00249F7D
	private void ClickChangeModule(float scrollViewPosition)
	{
		SelectModuleSideScreen selectModuleSideScreen = (SelectModuleSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.changeModuleSideScreen, UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.CHANGEMODULEPANEL);
		selectModuleSideScreen.addingNewModule = false;
		selectModuleSideScreen.SetTarget(this.reorderable.gameObject);
		this.ScrollToTargetPoint(scrollViewPosition);
	}

	// Token: 0x06006367 RID: 25447 RVA: 0x0024BDBC File Offset: 0x00249FBC
	private void ClickViewInterior()
	{
		ClustercraftExteriorDoor component = this.reorderable.GetComponent<ClustercraftExteriorDoor>();
		PassengerRocketModule component2 = this.reorderable.GetComponent<PassengerRocketModule>();
		WorldContainer targetWorld = component.GetTargetWorld();
		WorldContainer myWorld = component.GetMyWorld();
		if (ClusterManager.Instance.activeWorld == targetWorld)
		{
			if (myWorld.id != 255)
			{
				AudioMixer.instance.Stop(component2.interiorReverbSnapshot, STOP_MODE.ALLOWFADEOUT);
				ClusterManager.Instance.SetActiveWorld(myWorld.id);
			}
		}
		else
		{
			AudioMixer.instance.Start(component2.interiorReverbSnapshot);
			ClusterManager.Instance.SetActiveWorld(targetWorld.id);
		}
		DetailsScreen.Instance.ClearSecondarySideScreen();
		this.UpdateButtonStates();
	}

	// Token: 0x040043CB RID: 17355
	public static RocketModuleSideScreen instance;

	// Token: 0x040043CC RID: 17356
	private ReorderableBuilding reorderable;

	// Token: 0x040043CD RID: 17357
	public KScreen changeModuleSideScreen;

	// Token: 0x040043CE RID: 17358
	public Image moduleIcon;

	// Token: 0x040043CF RID: 17359
	[Header("Buttons")]
	public KButton addNewModuleButton;

	// Token: 0x040043D0 RID: 17360
	public KButton removeModuleButton;

	// Token: 0x040043D1 RID: 17361
	public KButton changeModuleButton;

	// Token: 0x040043D2 RID: 17362
	public KButton moveModuleUpButton;

	// Token: 0x040043D3 RID: 17363
	public KButton moveModuleDownButton;

	// Token: 0x040043D4 RID: 17364
	public KButton viewInteriorButton;

	// Token: 0x040043D5 RID: 17365
	[Header("Labels")]
	public LocText moduleNameLabel;

	// Token: 0x040043D6 RID: 17366
	public LocText moduleDescriptionLabel;

	// Token: 0x040043D7 RID: 17367
	public TextStyleSetting nameSetting;

	// Token: 0x040043D8 RID: 17368
	public TextStyleSetting descriptionSetting;
}
