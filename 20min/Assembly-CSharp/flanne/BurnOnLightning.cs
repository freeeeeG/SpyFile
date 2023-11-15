using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000D7 RID: 215
	public class BurnOnLightning : MonoBehaviour
	{
		// Token: 0x0600068D RID: 1677 RVA: 0x0001DA44 File Offset: 0x0001BC44
		private void Start()
		{
			this.BS = BurnSystem.SharedInstance;
			this.AddObserver(new Action<object, object>(this.OnThunderHit), ThunderGenerator.ThunderHitEvent);
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0001DA68 File Offset: 0x0001BC68
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnThunderHit), ThunderGenerator.ThunderHitEvent);
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0001DA84 File Offset: 0x0001BC84
		private void OnThunderHit(object sender, object args)
		{
			GameObject target = args as GameObject;
			this.BS.Burn(target, 3);
		}

		// Token: 0x0400045B RID: 1115
		private BurnSystem BS;
	}
}
