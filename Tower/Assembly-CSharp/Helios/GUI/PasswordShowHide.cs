using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Helios.GUI
{
	// Token: 0x020000E0 RID: 224
	public class PasswordShowHide : MonoBehaviour
	{
		// Token: 0x06000341 RID: 833 RVA: 0x0000E88A File Offset: 0x0000CA8A
		public void ShowPassword(bool activate)
		{
			this._ipPasswordInput.contentType = (activate ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password);
			this._ipPasswordInput.ForceLabelUpdate();
		}

		// Token: 0x04000315 RID: 789
		[SerializeField]
		private Toggle _tgShowPassword;

		// Token: 0x04000316 RID: 790
		[SerializeField]
		private TMP_InputField _ipPasswordInput;
	}
}
