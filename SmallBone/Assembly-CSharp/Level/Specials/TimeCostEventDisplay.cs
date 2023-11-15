using System;
using System.Collections;
using Data;
using TMPro;
using UnityEngine;

namespace Level.Specials
{
	// Token: 0x0200062C RID: 1580
	public class TimeCostEventDisplay : MonoBehaviour
	{
		// Token: 0x06001FAA RID: 8106 RVA: 0x000603CB File Offset: 0x0005E5CB
		private void Awake()
		{
			this.UpdateText((int)this._costEvent.GetValue());
		}

		// Token: 0x06001FAB RID: 8107 RVA: 0x000603E0 File Offset: 0x0005E5E0
		private void Update()
		{
			if (this._costEvent == null)
			{
				return;
			}
			if (!this._reward.activated && this._soldOutStringcache != "----------")
			{
				this._text.color = Color.white;
				this._text.text = "----------";
				return;
			}
			this.UpdateTextColor((int)this._costEvent.GetValue());
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x0006044E File Offset: 0x0005E64E
		public void UpdateDisplay()
		{
			base.StartCoroutine("CAnimate");
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x0006045C File Offset: 0x0005E65C
		private IEnumerator CAnimate()
		{
			float elapsed = 0f;
			int start = int.Parse(this._text.text);
			int dest = (int)this._costEvent.GetValue();
			while (elapsed < this._costEvent.updateInterval && this._reward.activated)
			{
				int value = (int)Mathf.Lerp((float)start, (float)dest, elapsed / this._costEvent.updateInterval);
				this.UpdateText(value);
				elapsed += Chronometer.global.deltaTime;
				yield return null;
			}
			this.UpdateText(dest);
			yield break;
		}

		// Token: 0x06001FAE RID: 8110 RVA: 0x0006046B File Offset: 0x0005E66B
		private void UpdateText(int value)
		{
			this._text.text = value.ToString();
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x0006047F File Offset: 0x0005E67F
		private void UpdateTextColor(int cost)
		{
			this._text.color = (GameData.Currency.gold.Has(cost) ? Color.white : Color.red);
		}

		// Token: 0x04001ADA RID: 6874
		[SerializeField]
		private TimeCostEvent _costEvent;

		// Token: 0x04001ADB RID: 6875
		[SerializeField]
		private TextMeshPro _text;

		// Token: 0x04001ADC RID: 6876
		[SerializeField]
		private InteractiveObject _reward;

		// Token: 0x04001ADD RID: 6877
		private const string soldOutString = "----------";

		// Token: 0x04001ADE RID: 6878
		private string _soldOutStringcache;
	}
}
