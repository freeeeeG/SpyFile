using System;
using UnityEngine;

namespace Helios.GUI
{
	// Token: 0x020000DE RID: 222
	public class PanelController : MonoBehaviour
	{
		// Token: 0x06000339 RID: 825 RVA: 0x0000E7DA File Offset: 0x0000C9DA
		public void Back()
		{
			SingletonPersistent<GameManager>.Instance.Back();
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000E7E6 File Offset: 0x0000C9E6
		public void LoadGameObject(GameObject go)
		{
			SingletonPersistent<GameManager>.Instance.LoadGameObject(go);
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000E7F3 File Offset: 0x0000C9F3
		public void LoadPopup(GameObject go)
		{
			SingletonPersistent<GameManager>.Instance.LoadPopup(go);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000E800 File Offset: 0x0000CA00
		public void BackHome()
		{
			SingletonPersistent<GameManager>.Instance.BackHome();
		}
	}
}
