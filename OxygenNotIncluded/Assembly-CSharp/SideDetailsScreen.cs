using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BF0 RID: 3056
public class SideDetailsScreen : KScreen
{
	// Token: 0x06006094 RID: 24724 RVA: 0x0023B329 File Offset: 0x00239529
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SideDetailsScreen.Instance = this;
		this.Initialize();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06006095 RID: 24725 RVA: 0x0023B349 File Offset: 0x00239549
	protected override void OnForcedCleanUp()
	{
		SideDetailsScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06006096 RID: 24726 RVA: 0x0023B358 File Offset: 0x00239558
	private void Initialize()
	{
		if (this.screens == null)
		{
			return;
		}
		this.rectTransform = base.GetComponent<RectTransform>();
		this.screenMap = new Dictionary<string, SideTargetScreen>();
		List<SideTargetScreen> list = new List<SideTargetScreen>();
		foreach (SideTargetScreen sideTargetScreen in this.screens)
		{
			SideTargetScreen sideTargetScreen2 = Util.KInstantiateUI<SideTargetScreen>(sideTargetScreen.gameObject, this.body.gameObject, false);
			sideTargetScreen2.gameObject.SetActive(false);
			list.Add(sideTargetScreen2);
		}
		list.ForEach(delegate(SideTargetScreen s)
		{
			this.screenMap.Add(s.name, s);
		});
		this.backButton.onClick += delegate()
		{
			this.Show(false);
		};
	}

	// Token: 0x06006097 RID: 24727 RVA: 0x0023B41C File Offset: 0x0023961C
	public void SetTitle(string newTitle)
	{
		this.title.text = newTitle;
	}

	// Token: 0x06006098 RID: 24728 RVA: 0x0023B42C File Offset: 0x0023962C
	public void SetScreen(string screenName, object content, float x)
	{
		if (!this.screenMap.ContainsKey(screenName))
		{
			global::Debug.LogError("Tried to open a screen that does exist on the manager!");
			return;
		}
		if (content == null)
		{
			global::Debug.LogError("Tried to set " + screenName + " with null content!");
			return;
		}
		if (!base.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(true);
		}
		Rect rect = this.rectTransform.rect;
		this.rectTransform.offsetMin = new Vector2(x, this.rectTransform.offsetMin.y);
		this.rectTransform.offsetMax = new Vector2(x + rect.width, this.rectTransform.offsetMax.y);
		if (this.activeScreen != null)
		{
			this.activeScreen.gameObject.SetActive(false);
		}
		this.activeScreen = this.screenMap[screenName];
		this.activeScreen.gameObject.SetActive(true);
		this.SetTitle(this.activeScreen.displayName);
		this.activeScreen.SetTarget(content);
	}

	// Token: 0x040041CC RID: 16844
	[SerializeField]
	private List<SideTargetScreen> screens;

	// Token: 0x040041CD RID: 16845
	[SerializeField]
	private LocText title;

	// Token: 0x040041CE RID: 16846
	[SerializeField]
	private KButton backButton;

	// Token: 0x040041CF RID: 16847
	[SerializeField]
	private RectTransform body;

	// Token: 0x040041D0 RID: 16848
	private RectTransform rectTransform;

	// Token: 0x040041D1 RID: 16849
	private Dictionary<string, SideTargetScreen> screenMap;

	// Token: 0x040041D2 RID: 16850
	private SideTargetScreen activeScreen;

	// Token: 0x040041D3 RID: 16851
	public static SideDetailsScreen Instance;
}
