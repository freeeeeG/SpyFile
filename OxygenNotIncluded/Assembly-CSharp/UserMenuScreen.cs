using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000A82 RID: 2690
public class UserMenuScreen : KIconButtonMenu
{
	// Token: 0x060051F3 RID: 20979 RVA: 0x001D4E34 File Offset: 0x001D3034
	protected override void OnPrefabInit()
	{
		this.keepMenuOpen = true;
		base.OnPrefabInit();
		this.priorityScreen = Util.KInstantiateUI<PriorityScreen>(this.priorityScreenPrefab.gameObject, this.priorityScreenParent, false);
		this.priorityScreen.InstantiateButtons(new Action<PrioritySetting>(this.OnPriorityClicked), true);
		this.buttonParent.transform.SetAsLastSibling();
	}

	// Token: 0x060051F4 RID: 20980 RVA: 0x001D4E93 File Offset: 0x001D3093
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.Subscribe(1980521255, new Action<object>(this.OnUIRefresh));
		KInputManager.InputChange.AddListener(new UnityAction(base.RefreshButtonTooltip));
	}

	// Token: 0x060051F5 RID: 20981 RVA: 0x001D4ECD File Offset: 0x001D30CD
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(base.RefreshButtonTooltip));
		base.OnForcedCleanUp();
	}

	// Token: 0x060051F6 RID: 20982 RVA: 0x001D4EEB File Offset: 0x001D30EB
	public void SetSelected(GameObject go)
	{
		this.ClearPrioritizable();
		this.selected = go;
		this.RefreshPrioritizable();
	}

	// Token: 0x060051F7 RID: 20983 RVA: 0x001D4F00 File Offset: 0x001D3100
	private void ClearPrioritizable()
	{
		if (this.selected != null)
		{
			Prioritizable component = this.selected.GetComponent<Prioritizable>();
			if (component != null)
			{
				Prioritizable prioritizable = component;
				prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Remove(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnPriorityChanged));
			}
		}
	}

	// Token: 0x060051F8 RID: 20984 RVA: 0x001D4F54 File Offset: 0x001D3154
	private void RefreshPrioritizable()
	{
		if (this.selected != null)
		{
			Prioritizable component = this.selected.GetComponent<Prioritizable>();
			if (component != null && component.IsPrioritizable())
			{
				Prioritizable prioritizable = component;
				prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnPriorityChanged));
				this.priorityScreen.gameObject.SetActive(true);
				this.priorityScreen.SetScreenPriority(component.GetMasterPriority(), false);
				return;
			}
			this.priorityScreen.gameObject.SetActive(false);
		}
	}

	// Token: 0x060051F9 RID: 20985 RVA: 0x001D4FE4 File Offset: 0x001D31E4
	public void Refresh(GameObject go)
	{
		if (go != this.selected)
		{
			return;
		}
		this.buttonInfos.Clear();
		this.slidersInfos.Clear();
		Game.Instance.userMenu.AppendToScreen(go, this);
		base.SetButtons(this.buttonInfos);
		base.RefreshButtons();
		this.RefreshSliders();
		this.ClearPrioritizable();
		this.RefreshPrioritizable();
		if ((this.sliders == null || this.sliders.Count == 0) && (this.buttonInfos == null || this.buttonInfos.Count == 0) && !this.priorityScreen.gameObject.activeSelf)
		{
			base.transform.parent.gameObject.SetActive(false);
			return;
		}
		base.transform.parent.gameObject.SetActive(true);
	}

	// Token: 0x060051FA RID: 20986 RVA: 0x001D50B4 File Offset: 0x001D32B4
	public void AddSliders(IList<UserMenu.SliderInfo> sliders)
	{
		this.slidersInfos.AddRange(sliders);
	}

	// Token: 0x060051FB RID: 20987 RVA: 0x001D50C2 File Offset: 0x001D32C2
	public void AddButtons(IList<KIconButtonMenu.ButtonInfo> buttons)
	{
		this.buttonInfos.AddRange(buttons);
	}

	// Token: 0x060051FC RID: 20988 RVA: 0x001D50D0 File Offset: 0x001D32D0
	private void OnUIRefresh(object data)
	{
		this.Refresh(data as GameObject);
	}

	// Token: 0x060051FD RID: 20989 RVA: 0x001D50E0 File Offset: 0x001D32E0
	public void RefreshSliders()
	{
		if (this.sliders != null)
		{
			for (int i = 0; i < this.sliders.Count; i++)
			{
				UnityEngine.Object.Destroy(this.sliders[i].gameObject);
			}
			this.sliders = null;
		}
		if (this.slidersInfos == null || this.slidersInfos.Count == 0)
		{
			return;
		}
		this.sliders = new List<MinMaxSlider>();
		for (int j = 0; j < this.slidersInfos.Count; j++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.sliderPrefab.gameObject, Vector3.zero, Quaternion.identity);
			this.slidersInfos[j].sliderGO = gameObject;
			MinMaxSlider component = gameObject.GetComponent<MinMaxSlider>();
			this.sliders.Add(component);
			Transform parent = (this.sliderParent != null) ? this.sliderParent.transform : base.transform;
			gameObject.transform.SetParent(parent, false);
			gameObject.SetActive(true);
			gameObject.name = "Slider";
			if (component.toolTip)
			{
				component.toolTip.toolTip = this.slidersInfos[j].toolTip;
			}
			component.lockType = this.slidersInfos[j].lockType;
			component.interactable = this.slidersInfos[j].interactable;
			component.minLimit = this.slidersInfos[j].minLimit;
			component.maxLimit = this.slidersInfos[j].maxLimit;
			component.currentMinValue = this.slidersInfos[j].currentMinValue;
			component.currentMaxValue = this.slidersInfos[j].currentMaxValue;
			component.onMinChange = this.slidersInfos[j].onMinChange;
			component.onMaxChange = this.slidersInfos[j].onMaxChange;
			component.direction = this.slidersInfos[j].direction;
			component.SetMode(this.slidersInfos[j].mode);
			component.SetMinMaxValue(this.slidersInfos[j].currentMinValue, this.slidersInfos[j].currentMaxValue, this.slidersInfos[j].minLimit, this.slidersInfos[j].maxLimit);
		}
	}

	// Token: 0x060051FE RID: 20990 RVA: 0x001D5344 File Offset: 0x001D3544
	private void OnPriorityClicked(PrioritySetting priority)
	{
		if (this.selected != null)
		{
			Prioritizable component = this.selected.GetComponent<Prioritizable>();
			if (component != null)
			{
				component.SetMasterPriority(priority);
			}
		}
	}

	// Token: 0x060051FF RID: 20991 RVA: 0x001D537B File Offset: 0x001D357B
	private void OnPriorityChanged(PrioritySetting priority)
	{
		this.priorityScreen.SetScreenPriority(priority, false);
	}

	// Token: 0x040035DB RID: 13787
	private GameObject selected;

	// Token: 0x040035DC RID: 13788
	public MinMaxSlider sliderPrefab;

	// Token: 0x040035DD RID: 13789
	public GameObject sliderParent;

	// Token: 0x040035DE RID: 13790
	public PriorityScreen priorityScreenPrefab;

	// Token: 0x040035DF RID: 13791
	public GameObject priorityScreenParent;

	// Token: 0x040035E0 RID: 13792
	private List<MinMaxSlider> sliders = new List<MinMaxSlider>();

	// Token: 0x040035E1 RID: 13793
	private List<UserMenu.SliderInfo> slidersInfos = new List<UserMenu.SliderInfo>();

	// Token: 0x040035E2 RID: 13794
	private List<KIconButtonMenu.ButtonInfo> buttonInfos = new List<KIconButtonMenu.ButtonInfo>();

	// Token: 0x040035E3 RID: 13795
	private PriorityScreen priorityScreen;
}
