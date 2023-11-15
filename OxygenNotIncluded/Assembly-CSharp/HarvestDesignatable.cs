using System;
using KSerialization;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020007F2 RID: 2034
[AddComponentMenu("KMonoBehaviour/scripts/HarvestDesignatable")]
public class HarvestDesignatable : KMonoBehaviour
{
	// Token: 0x1700042F RID: 1071
	// (get) Token: 0x060039D9 RID: 14809 RVA: 0x0014281C File Offset: 0x00140A1C
	public bool InPlanterBox
	{
		get
		{
			return this.isInPlanterBox;
		}
	}

	// Token: 0x17000430 RID: 1072
	// (get) Token: 0x060039DA RID: 14810 RVA: 0x00142824 File Offset: 0x00140A24
	// (set) Token: 0x060039DB RID: 14811 RVA: 0x0014282C File Offset: 0x00140A2C
	public bool MarkedForHarvest
	{
		get
		{
			return this.isMarkedForHarvest;
		}
		set
		{
			this.isMarkedForHarvest = value;
		}
	}

	// Token: 0x17000431 RID: 1073
	// (get) Token: 0x060039DC RID: 14812 RVA: 0x00142835 File Offset: 0x00140A35
	public bool HarvestWhenReady
	{
		get
		{
			return this.harvestWhenReady;
		}
	}

	// Token: 0x060039DD RID: 14813 RVA: 0x0014283D File Offset: 0x00140A3D
	protected HarvestDesignatable()
	{
		this.onEnableOverlayDelegate = new Action<object>(this.OnEnableOverlay);
	}

	// Token: 0x060039DE RID: 14814 RVA: 0x00142865 File Offset: 0x00140A65
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<HarvestDesignatable>(1309017699, HarvestDesignatable.SetInPlanterBoxTrueDelegate);
	}

	// Token: 0x060039DF RID: 14815 RVA: 0x00142880 File Offset: 0x00140A80
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.isMarkedForHarvest)
		{
			this.MarkForHarvest();
		}
		Components.HarvestDesignatables.Add(this);
		base.Subscribe<HarvestDesignatable>(493375141, HarvestDesignatable.OnRefreshUserMenuDelegate);
		base.Subscribe<HarvestDesignatable>(2127324410, HarvestDesignatable.OnCancelDelegate);
		Game.Instance.Subscribe(1248612973, this.onEnableOverlayDelegate);
		Game.Instance.Subscribe(1798162660, this.onEnableOverlayDelegate);
		Game.Instance.Subscribe(2015652040, new Action<object>(this.OnDisableOverlay));
		Game.Instance.Subscribe(1983128072, new Action<object>(this.RefreshOverlayIcon));
		this.area = base.GetComponent<OccupyArea>();
	}

	// Token: 0x060039E0 RID: 14816 RVA: 0x00142940 File Offset: 0x00140B40
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.HarvestDesignatables.Remove(this);
		this.DestroyOverlayIcon();
		Game.Instance.Unsubscribe(1248612973, this.onEnableOverlayDelegate);
		Game.Instance.Unsubscribe(2015652040, new Action<object>(this.OnDisableOverlay));
		Game.Instance.Unsubscribe(1798162660, this.onEnableOverlayDelegate);
		Game.Instance.Unsubscribe(1983128072, new Action<object>(this.RefreshOverlayIcon));
	}

	// Token: 0x060039E1 RID: 14817 RVA: 0x001429C4 File Offset: 0x00140BC4
	private void DestroyOverlayIcon()
	{
		if (this.HarvestWhenReadyOverlayIcon != null)
		{
			UnityEngine.Object.Destroy(this.HarvestWhenReadyOverlayIcon.gameObject);
			this.HarvestWhenReadyOverlayIcon = null;
		}
	}

	// Token: 0x060039E2 RID: 14818 RVA: 0x001429EC File Offset: 0x00140BEC
	private void CreateOverlayIcon()
	{
		if (this.HarvestWhenReadyOverlayIcon != null)
		{
			return;
		}
		if (base.GetComponent<AttackableBase>() == null)
		{
			this.HarvestWhenReadyOverlayIcon = Util.KInstantiate(Assets.UIPrefabs.HarvestWhenReadyOverlayIcon, GameScreenManager.Instance.worldSpaceCanvas, null).GetComponent<RectTransform>();
			Extents extents = base.GetComponent<OccupyArea>().GetExtents();
			Vector3 position;
			if (base.GetComponent<KPrefabID>().HasTag(GameTags.Hanging))
			{
				position = new Vector3((float)(extents.x + extents.width / 2) + 0.5f, (float)(extents.y + extents.height));
			}
			else
			{
				position = new Vector3((float)(extents.x + extents.width / 2) + 0.5f, (float)extents.y);
			}
			this.HarvestWhenReadyOverlayIcon.transform.SetPosition(position);
			this.RefreshOverlayIcon(null);
		}
	}

	// Token: 0x060039E3 RID: 14819 RVA: 0x00142AC6 File Offset: 0x00140CC6
	private void OnDisableOverlay(object data)
	{
		this.DestroyOverlayIcon();
	}

	// Token: 0x060039E4 RID: 14820 RVA: 0x00142ACE File Offset: 0x00140CCE
	private void OnEnableOverlay(object data)
	{
		if ((HashedString)data == OverlayModes.Harvest.ID)
		{
			this.CreateOverlayIcon();
			return;
		}
		this.DestroyOverlayIcon();
	}

	// Token: 0x060039E5 RID: 14821 RVA: 0x00142AF0 File Offset: 0x00140CF0
	private void RefreshOverlayIcon(object data = null)
	{
		if (this.HarvestWhenReadyOverlayIcon != null)
		{
			if ((Grid.IsVisible(Grid.PosToCell(base.gameObject)) && base.gameObject.GetMyWorldId() == ClusterManager.Instance.activeWorldId) || (CameraController.Instance != null && CameraController.Instance.FreeCameraEnabled))
			{
				if (!this.HarvestWhenReadyOverlayIcon.gameObject.activeSelf)
				{
					this.HarvestWhenReadyOverlayIcon.gameObject.SetActive(true);
				}
			}
			else if (this.HarvestWhenReadyOverlayIcon.gameObject.activeSelf)
			{
				this.HarvestWhenReadyOverlayIcon.gameObject.SetActive(false);
			}
			HierarchyReferences component = this.HarvestWhenReadyOverlayIcon.GetComponent<HierarchyReferences>();
			if (this.harvestWhenReady)
			{
				Image image = (Image)component.GetReference("On");
				image.gameObject.SetActive(true);
				image.color = GlobalAssets.Instance.colorSet.harvestEnabled;
				component.GetReference("Off").gameObject.SetActive(false);
				return;
			}
			component.GetReference("On").gameObject.SetActive(false);
			Image image2 = (Image)component.GetReference("Off");
			image2.gameObject.SetActive(true);
			image2.color = GlobalAssets.Instance.colorSet.harvestDisabled;
		}
	}

	// Token: 0x060039E6 RID: 14822 RVA: 0x00142C44 File Offset: 0x00140E44
	public bool CanBeHarvested()
	{
		Harvestable component = base.GetComponent<Harvestable>();
		return !(component != null) || component.CanBeHarvested;
	}

	// Token: 0x060039E7 RID: 14823 RVA: 0x00142C69 File Offset: 0x00140E69
	public void SetInPlanterBox(bool state)
	{
		if (state)
		{
			if (!this.isInPlanterBox)
			{
				this.isInPlanterBox = true;
				this.SetHarvestWhenReady(this.defaultHarvestStateWhenPlanted);
				return;
			}
		}
		else
		{
			this.isInPlanterBox = false;
		}
	}

	// Token: 0x060039E8 RID: 14824 RVA: 0x00142C94 File Offset: 0x00140E94
	public void SetHarvestWhenReady(bool state)
	{
		this.harvestWhenReady = state;
		if (this.harvestWhenReady && this.CanBeHarvested() && !this.isMarkedForHarvest)
		{
			this.MarkForHarvest();
		}
		if (this.isMarkedForHarvest && !this.harvestWhenReady)
		{
			this.OnCancel(null);
			if (this.CanBeHarvested() && this.isInPlanterBox)
			{
				base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, this);
			}
		}
		base.Trigger(-266953818, null);
		this.RefreshOverlayIcon(null);
	}

	// Token: 0x060039E9 RID: 14825 RVA: 0x00142D1C File Offset: 0x00140F1C
	protected virtual void OnCancel(object data = null)
	{
	}

	// Token: 0x060039EA RID: 14826 RVA: 0x00142D20 File Offset: 0x00140F20
	public virtual void MarkForHarvest()
	{
		if (!this.CanBeHarvested())
		{
			return;
		}
		this.isMarkedForHarvest = true;
		Harvestable component = base.GetComponent<Harvestable>();
		if (component != null)
		{
			component.OnMarkedForHarvest();
		}
	}

	// Token: 0x060039EB RID: 14827 RVA: 0x00142D53 File Offset: 0x00140F53
	protected virtual void OnClickHarvestWhenReady()
	{
		this.SetHarvestWhenReady(true);
	}

	// Token: 0x060039EC RID: 14828 RVA: 0x00142D5C File Offset: 0x00140F5C
	protected virtual void OnClickCancelHarvestWhenReady()
	{
		this.SetHarvestWhenReady(false);
	}

	// Token: 0x060039ED RID: 14829 RVA: 0x00142D68 File Offset: 0x00140F68
	public virtual void OnRefreshUserMenu(object data)
	{
		if (this.showUserMenuButtons)
		{
			KIconButtonMenu.ButtonInfo button = this.harvestWhenReady ? new KIconButtonMenu.ButtonInfo("action_harvest", UI.USERMENUACTIONS.CANCEL_HARVEST_WHEN_READY.NAME, delegate()
			{
				this.OnClickCancelHarvestWhenReady();
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, UI.GAMEOBJECTEFFECTS.PLANT_DO_NOT_HARVEST, base.transform, 1.5f, false);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCEL_HARVEST_WHEN_READY.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_harvest", UI.USERMENUACTIONS.HARVEST_WHEN_READY.NAME, delegate()
			{
				this.OnClickHarvestWhenReady();
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, UI.GAMEOBJECTEFFECTS.PLANT_MARK_FOR_HARVEST, base.transform, 1.5f, false);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.HARVEST_WHEN_READY.TOOLTIP, true);
			Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
		}
	}

	// Token: 0x04002689 RID: 9865
	public bool defaultHarvestStateWhenPlanted = true;

	// Token: 0x0400268A RID: 9866
	public OccupyArea area;

	// Token: 0x0400268B RID: 9867
	[Serialize]
	protected bool isMarkedForHarvest;

	// Token: 0x0400268C RID: 9868
	[Serialize]
	private bool isInPlanterBox;

	// Token: 0x0400268D RID: 9869
	public bool showUserMenuButtons = true;

	// Token: 0x0400268E RID: 9870
	[Serialize]
	protected bool harvestWhenReady;

	// Token: 0x0400268F RID: 9871
	public RectTransform HarvestWhenReadyOverlayIcon;

	// Token: 0x04002690 RID: 9872
	private Action<object> onEnableOverlayDelegate;

	// Token: 0x04002691 RID: 9873
	private static readonly EventSystem.IntraObjectHandler<HarvestDesignatable> OnCancelDelegate = new EventSystem.IntraObjectHandler<HarvestDesignatable>(delegate(HarvestDesignatable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x04002692 RID: 9874
	private static readonly EventSystem.IntraObjectHandler<HarvestDesignatable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<HarvestDesignatable>(delegate(HarvestDesignatable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04002693 RID: 9875
	private static readonly EventSystem.IntraObjectHandler<HarvestDesignatable> SetInPlanterBoxTrueDelegate = new EventSystem.IntraObjectHandler<HarvestDesignatable>(delegate(HarvestDesignatable component, object data)
	{
		component.SetInPlanterBox(true);
	});
}
