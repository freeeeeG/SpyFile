using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace flanne.UI
{
	// Token: 0x02000206 RID: 518
	public class ControlSchemeUISwitcher : MonoBehaviour
	{
		// Token: 0x06000BAE RID: 2990 RVA: 0x0002B800 File Offset: 0x00029A00
		private void Update()
		{
			if (this.previousControlScheme != this.playerInput.currentControlScheme)
			{
				if (this.playerInput.currentControlScheme == "Keyboard&Mouse")
				{
					GameObject gameObject = this.kbmUI;
					if (gameObject != null)
					{
						gameObject.SetActive(true);
					}
					GameObject gameObject2 = this.gamepadUI;
					if (gameObject2 == null)
					{
						return;
					}
					gameObject2.SetActive(false);
					return;
				}
				else if (this.playerInput.currentControlScheme == "Gamepad")
				{
					GameObject gameObject3 = this.gamepadUI;
					if (gameObject3 != null)
					{
						gameObject3.SetActive(true);
					}
					GameObject gameObject4 = this.kbmUI;
					if (gameObject4 == null)
					{
						return;
					}
					gameObject4.SetActive(false);
				}
			}
		}

		// Token: 0x04000808 RID: 2056
		[SerializeField]
		private PlayerInput playerInput;

		// Token: 0x04000809 RID: 2057
		[SerializeField]
		private GameObject gamepadUI;

		// Token: 0x0400080A RID: 2058
		[SerializeField]
		private GameObject kbmUI;

		// Token: 0x0400080B RID: 2059
		private const string gamepadScheme = "Gamepad";

		// Token: 0x0400080C RID: 2060
		private const string mouseScheme = "Keyboard&Mouse";

		// Token: 0x0400080D RID: 2061
		private string previousControlScheme = "";
	}
}
