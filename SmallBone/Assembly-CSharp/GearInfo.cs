using System;
using Characters.Gear;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000A4 RID: 164
public class GearInfo : MonoBehaviour
{
	// Token: 0x06000346 RID: 838 RVA: 0x0000C5D4 File Offset: 0x0000A7D4
	private void Awake()
	{
		this._gear = base.GetComponentInParent<Gear>();
		this._rarity.text = this._gear.rarity.ToString();
		this._name.text = this._gear.displayName;
		this._icon.sprite = this._gear.dropped.GetComponent<SpriteRenderer>().sprite;
	}

	// Token: 0x040002A8 RID: 680
	private Gear _gear;

	// Token: 0x040002A9 RID: 681
	[SerializeField]
	private Text _rarity;

	// Token: 0x040002AA RID: 682
	[SerializeField]
	private Text _name;

	// Token: 0x040002AB RID: 683
	[SerializeField]
	private Image _icon;
}
