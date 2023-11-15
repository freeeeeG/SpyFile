using System;
using Characters.Gear;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200009C RID: 156
public class FlavorText : MonoBehaviour
{
	// Token: 0x06000303 RID: 771 RVA: 0x0000BA3B File Offset: 0x00009C3B
	private void Awake()
	{
		this._gear = base.GetComponentInParent<Gear>();
		if (!this._gear.hasFlavor)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this._text.text = this._gear.flavor;
	}

	// Token: 0x0400027B RID: 635
	[SerializeField]
	private Text _text;

	// Token: 0x0400027C RID: 636
	private Gear _gear;
}
