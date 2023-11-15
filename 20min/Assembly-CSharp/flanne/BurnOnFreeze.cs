using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000D4 RID: 212
	public class BurnOnFreeze : MonoBehaviour
	{
		// Token: 0x06000680 RID: 1664 RVA: 0x0001D7E2 File Offset: 0x0001B9E2
		private void Start()
		{
			this.AddObserver(new Action<object, object>(this.OnFreeze), FreezeSystem.InflictFreezeEvent);
			this.BurnSys = BurnSystem.SharedInstance;
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0001D806 File Offset: 0x0001BA06
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnFreeze), FreezeSystem.InflictFreezeEvent);
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0001D820 File Offset: 0x0001BA20
		private void OnFreeze(object sender, object args)
		{
			GameObject gameObject = args as GameObject;
			if (gameObject.tag.Contains("Enemy"))
			{
				this.BurnSys.Burn(gameObject, this.burnDamage);
			}
		}

		// Token: 0x0400044E RID: 1102
		[SerializeField]
		private int burnDamage;

		// Token: 0x0400044F RID: 1103
		private BurnSystem BurnSys;
	}
}
