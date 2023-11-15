using System;
using Data;
using TMPro;
using UnityEngine;

namespace Level.BlackMarket
{
	// Token: 0x02000625 RID: 1573
	public class CollectorGearSlot : MonoBehaviour
	{
		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06001F8D RID: 8077 RVA: 0x0005FF35 File Offset: 0x0005E135
		public Vector3 itemPosition
		{
			get
			{
				return this._itemPosition.position;
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06001F8E RID: 8078 RVA: 0x0005FF42 File Offset: 0x0005E142
		// (set) Token: 0x06001F8F RID: 8079 RVA: 0x0005FF4A File Offset: 0x0005E14A
		public DroppedGear droppedGear
		{
			get
			{
				return this._droppedGear;
			}
			set
			{
				this._droppedGear = value;
				this._text.text = this._droppedGear.price.ToString();
			}
		}

		// Token: 0x06001F90 RID: 8080 RVA: 0x0005FF70 File Offset: 0x0005E170
		private void Update()
		{
			if (this._droppedGear == null)
			{
				this._text.text = this.soldOutText;
				this._text.color = Color.white;
				return;
			}
			if (this._droppedGear.price > 0)
			{
				this._text.color = (GameData.Currency.gold.Has(this._droppedGear.price) ? Color.white : Color.red);
				return;
			}
			this._text.text = this.soldOutText;
			this._text.color = Color.white;
		}

		// Token: 0x04001AB9 RID: 6841
		[SerializeField]
		private Transform _itemPosition;

		// Token: 0x04001ABA RID: 6842
		[SerializeField]
		private TMP_Text _text;

		// Token: 0x04001ABB RID: 6843
		private DroppedGear _droppedGear;

		// Token: 0x04001ABC RID: 6844
		private string soldOutText = "---";
	}
}
