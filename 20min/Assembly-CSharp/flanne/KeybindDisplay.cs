using System;
using flanne.InputExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace flanne
{
	// Token: 0x0200013A RID: 314
	public class KeybindDisplay : MonoBehaviour
	{
		// Token: 0x0600084A RID: 2122 RVA: 0x00022E61 File Offset: 0x00021061
		private void Update()
		{
			this.Refresh();
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x00022E6C File Offset: 0x0002106C
		private void Refresh()
		{
			string text = string.Empty;
			string text2 = null;
			string controlPath = null;
			InputActionReference inputActionReference = this.actionRef;
			InputAction inputAction = (inputActionReference != null) ? inputActionReference.action : null;
			if (inputAction != null)
			{
				int num = inputAction.bindings.IndexOf((InputBinding x) => x.id.ToString() == this.bindingId);
				if (num != -1)
				{
					text = inputAction.GetBindingDisplayString(num, out text2, out controlPath, this.displayOptions);
				}
			}
			GamepadIcons gamepadIcons = this.gamepadIcons;
			Sprite sprite = (gamepadIcons != null) ? gamepadIcons.GetIcon(text2, controlPath) : null;
			if (this.bindingIcon == null)
			{
				this.bindingTMP.text = text;
				this.bindingTMP.gameObject.SetActive(true);
				return;
			}
			if (sprite == null)
			{
				this.bindingTMP.text = text;
				this.bindingTMP.gameObject.SetActive(true);
				this.bindingIcon.gameObject.SetActive(false);
				return;
			}
			if (text2 == "Keyboard")
			{
				this.bindingTMP.text = text;
				this.bindingIcon.sprite = sprite;
				this.bindingTMP.gameObject.SetActive(true);
				this.bindingIcon.gameObject.SetActive(true);
				return;
			}
			this.bindingIcon.sprite = sprite;
			this.bindingIcon.gameObject.SetActive(true);
			this.bindingTMP.gameObject.SetActive(false);
		}

		// Token: 0x04000617 RID: 1559
		[SerializeField]
		private InputActionReference actionRef;

		// Token: 0x04000618 RID: 1560
		[SerializeField]
		private string bindingId;

		// Token: 0x04000619 RID: 1561
		[SerializeField]
		private InputBinding.DisplayStringOptions displayOptions;

		// Token: 0x0400061A RID: 1562
		[SerializeField]
		private TMP_Text bindingTMP;

		// Token: 0x0400061B RID: 1563
		[Tooltip("Can leave null if not using icons.")]
		[SerializeField]
		private Image bindingIcon;

		// Token: 0x0400061C RID: 1564
		[Tooltip("Can leave null if not using icons.")]
		[SerializeField]
		private GamepadIcons gamepadIcons;

		// Token: 0x0400061D RID: 1565
		private string _currentControlScheme;
	}
}
