using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x020000E8 RID: 232
	public class SetGOActiveOnReload : MonoBehaviour
	{
		// Token: 0x060006CF RID: 1743 RVA: 0x0001E63C File Offset: 0x0001C83C
		private void OnReload()
		{
			this.obj.SetActive(true);
			this.sfx.Play(null);
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0001E657 File Offset: 0x0001C857
		private void Start()
		{
			this.ammo = base.GetComponentInParent<PlayerController>().ammo;
			this.ammo.OnReload.AddListener(new UnityAction(this.OnReload));
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0001E686 File Offset: 0x0001C886
		private void OnDestroy()
		{
			this.ammo.OnReload.RemoveListener(new UnityAction(this.OnReload));
		}

		// Token: 0x04000499 RID: 1177
		[SerializeField]
		private SoundEffectSO sfx;

		// Token: 0x0400049A RID: 1178
		[SerializeField]
		private GameObject obj;

		// Token: 0x0400049B RID: 1179
		private Ammo ammo;
	}
}
