using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.CharacterPassives
{
	// Token: 0x0200025D RID: 605
	public class ReloadOnShadowCloneDash : MonoBehaviour
	{
		// Token: 0x06000D1E RID: 3358 RVA: 0x0002FF24 File Offset: 0x0002E124
		private void OnShadowClone()
		{
			this.ammo.Reload();
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0002FF34 File Offset: 0x0002E134
		private void Start()
		{
			PlayerController component = base.transform.root.GetComponent<PlayerController>();
			this.ammo = component.ammo;
			this.shadowClone = component.GetComponentInChildren<ShadowClonePassive>();
			this.shadowClone.onUse.AddListener(new UnityAction(this.OnShadowClone));
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0002FF86 File Offset: 0x0002E186
		private void OnDestroy()
		{
			this.shadowClone.onUse.RemoveListener(new UnityAction(this.OnShadowClone));
		}

		// Token: 0x04000981 RID: 2433
		private ShadowClonePassive shadowClone;

		// Token: 0x04000982 RID: 2434
		private Ammo ammo;
	}
}
