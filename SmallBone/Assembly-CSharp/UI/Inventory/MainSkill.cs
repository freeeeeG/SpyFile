using System;
using TMPro;
using UnityEngine;

namespace UI.Inventory
{
	// Token: 0x02000438 RID: 1080
	public class MainSkill : MonoBehaviour
	{
		// Token: 0x06001493 RID: 5267 RVA: 0x0003F741 File Offset: 0x0003D941
		public void Set(string name, string description)
		{
			this._name.text = name;
			this._description.text = description;
		}

		// Token: 0x0400119E RID: 4510
		[SerializeField]
		private TMP_Text _name;

		// Token: 0x0400119F RID: 4511
		[SerializeField]
		private TMP_Text _description;
	}
}
