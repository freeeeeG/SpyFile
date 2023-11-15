using System;
using UnityEngine;

// Token: 0x02000C9F RID: 3231
[AddComponentMenu("KMonoBehaviour/scripts/SimpleUIShowHide")]
public class SimpleUIShowHide : KMonoBehaviour
{
	// Token: 0x060066DA RID: 26330 RVA: 0x00265D50 File Offset: 0x00263F50
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnClick));
		if (!this.saveStatePreferenceKey.IsNullOrWhiteSpace() && KPlayerPrefs.GetInt(this.saveStatePreferenceKey, 1) != 1 && this.toggle.CurrentState == 0)
		{
			this.OnClick();
		}
	}

	// Token: 0x060066DB RID: 26331 RVA: 0x00265DBC File Offset: 0x00263FBC
	private void OnClick()
	{
		this.toggle.NextState();
		this.content.SetActive(this.toggle.CurrentState == 0);
		if (!this.saveStatePreferenceKey.IsNullOrWhiteSpace())
		{
			KPlayerPrefs.SetInt(this.saveStatePreferenceKey, (this.toggle.CurrentState == 0) ? 1 : 0);
		}
	}

	// Token: 0x04004724 RID: 18212
	[MyCmpReq]
	private MultiToggle toggle;

	// Token: 0x04004725 RID: 18213
	[SerializeField]
	public GameObject content;

	// Token: 0x04004726 RID: 18214
	[SerializeField]
	private string saveStatePreferenceKey;

	// Token: 0x04004727 RID: 18215
	private const int onState = 0;
}
