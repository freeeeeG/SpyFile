using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tantawowa.Demo.DemoScripts
{
	// Token: 0x0200007A RID: 122
	public class UIManager : MonoBehaviour
	{
		// Token: 0x060001CC RID: 460 RVA: 0x00008120 File Offset: 0x00006320
		public void ChangeUI(UIType type)
		{
			for (int i = 0; i <= 3; i++)
			{
				this.UIElements[i].SetActive(i == (int)type);
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000814E File Offset: 0x0000634E
		public void ToggleLight(bool isOn)
		{
			this.Light.SetActive(isOn);
		}

		// Token: 0x040001AD RID: 429
		public List<GameObject> UIElements;

		// Token: 0x040001AE RID: 430
		public GameObject Light;
	}
}
