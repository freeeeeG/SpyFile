using System;
using Characters;
using TMPro;
using UnityEngine;

namespace UI.TestingTool
{
	// Token: 0x020003FC RID: 1020
	public sealed class DetailModeHealth : MonoBehaviour
	{
		// Token: 0x06001343 RID: 4931 RVA: 0x00039F5F File Offset: 0x0003815F
		public void Initialize(Character owner)
		{
			this._owner = owner;
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x00039F68 File Offset: 0x00038168
		private void Update()
		{
			if (this._owner == null)
			{
				return;
			}
			float num = (float)this._owner.health.currentHealth;
			if (Mathf.Approximately(this._healthCache, num))
			{
				return;
			}
			this._healthCache = num;
			this._text.text = num.ToString();
		}

		// Token: 0x04001029 RID: 4137
		[SerializeField]
		private TMP_Text _text;

		// Token: 0x0400102A RID: 4138
		private Character _owner;

		// Token: 0x0400102B RID: 4139
		private float _healthCache;
	}
}
