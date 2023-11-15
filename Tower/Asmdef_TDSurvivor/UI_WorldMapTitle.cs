using System;
using TMPro;
using UnityEngine;

// Token: 0x02000193 RID: 403
public class UI_WorldMapTitle : AUISituational
{
	// Token: 0x06000ACB RID: 2763 RVA: 0x000287F4 File Offset: 0x000269F4
	private void OnEnable()
	{
		EventMgr.Register<bool>(eMapSceneEvents.ToggleMapSceneBasicUI, new Action<bool>(this.OnToggleMapSceneBasicUI));
		EventMgr.Register<eWorldType>(eGameEvents.OnWorldChange, new Action<eWorldType>(this.OnWorldChange));
		EventMgr.Register(eGameEvents.OnLanguageChanged, new Action(this.OnLanguageChanged));
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x00028848 File Offset: 0x00026A48
	private void OnDisable()
	{
		EventMgr.Remove<bool>(eMapSceneEvents.ToggleMapSceneBasicUI, new Action<bool>(this.OnToggleMapSceneBasicUI));
		EventMgr.Remove<eWorldType>(eGameEvents.OnWorldChange, new Action<eWorldType>(this.OnWorldChange));
		EventMgr.Remove(eGameEvents.OnLanguageChanged, new Action(this.OnLanguageChanged));
	}

	// Token: 0x06000ACD RID: 2765 RVA: 0x0002889A File Offset: 0x00026A9A
	private void OnToggleMapSceneBasicUI(bool isOn)
	{
		base.Toggle(isOn);
	}

	// Token: 0x06000ACE RID: 2766 RVA: 0x000288A3 File Offset: 0x00026AA3
	private void OnWorldChange(eWorldType curWorld)
	{
		this.UpdateText(curWorld);
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x000288AC File Offset: 0x00026AAC
	private void Start()
	{
		this.curWorld = GameDataManager.instance.GameplayData.CurWorld;
		this.UpdateText(this.curWorld);
	}

	// Token: 0x06000AD0 RID: 2768 RVA: 0x000288CF File Offset: 0x00026ACF
	private void OnLanguageChanged()
	{
		this.UpdateText(this.curWorld);
	}

	// Token: 0x06000AD1 RID: 2769 RVA: 0x000288DD File Offset: 0x00026ADD
	private void UpdateText(eWorldType curWorld)
	{
		this.text_WorldName.text = LocalizationManager.Instance.GetString("WorldName", curWorld.ToString(), Array.Empty<object>());
	}

	// Token: 0x06000AD2 RID: 2770 RVA: 0x0002890B File Offset: 0x00026B0B
	private void Update()
	{
	}

	// Token: 0x04000845 RID: 2117
	[SerializeField]
	private TMP_Text text_WorldName;

	// Token: 0x04000846 RID: 2118
	private eWorldType curWorld;
}
