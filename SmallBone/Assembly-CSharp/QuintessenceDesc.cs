using System;
using GameResources;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000AB RID: 171
public class QuintessenceDesc : MonoBehaviour
{
	// Token: 0x06000368 RID: 872 RVA: 0x0000CB8E File Offset: 0x0000AD8E
	private void Awake()
	{
		this._effectOfUse.text = Localization.GetLocalizedString("quintessence_effectOfUse");
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x06000369 RID: 873 RVA: 0x0000CBA5 File Offset: 0x0000ADA5
	// (set) Token: 0x0600036A RID: 874 RVA: 0x0000CBB2 File Offset: 0x0000ADB2
	public string text
	{
		get
		{
			return this._description.text;
		}
		set
		{
			this._description.text = value;
		}
	}

	// Token: 0x040002C6 RID: 710
	[SerializeField]
	private Text _effectOfUse;

	// Token: 0x040002C7 RID: 711
	[SerializeField]
	private Text _description;
}
