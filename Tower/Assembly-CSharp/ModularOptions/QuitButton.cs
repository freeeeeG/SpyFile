using System;
using UnityEngine;
using UnityEngine.UI;

namespace ModularOptions
{
	// Token: 0x02000080 RID: 128
	[AddComponentMenu("Modular Options/Button/Quit")]
	[RequireComponent(typeof(Button))]
	public class QuitButton : MonoBehaviour
	{
		// Token: 0x060001DE RID: 478 RVA: 0x00008432 File Offset: 0x00006632
		private void Awake()
		{
			base.GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.Quit();
			});
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00008450 File Offset: 0x00006650
		private void Quit()
		{
			Application.Quit();
		}
	}
}
