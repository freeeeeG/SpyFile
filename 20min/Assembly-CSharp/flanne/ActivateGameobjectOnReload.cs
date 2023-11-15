using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x020000CE RID: 206
	public class ActivateGameobjectOnReload : MonoBehaviour
	{
		// Token: 0x06000665 RID: 1637 RVA: 0x0001D31A File Offset: 0x0001B51A
		private void OnReload()
		{
			this.obj.SetActive(true);
			SoundEffectSO soundEffectSO = this.sfx;
			if (soundEffectSO == null)
			{
				return;
			}
			soundEffectSO.Play(null);
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x0001D33C File Offset: 0x0001B53C
		private void Start()
		{
			PlayerController componentInParent = base.transform.GetComponentInParent<PlayerController>();
			this.ammo = componentInParent.ammo;
			this.ammo.OnReload.AddListener(new UnityAction(this.OnReload));
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0001D37D File Offset: 0x0001B57D
		private void OnDestroy()
		{
			this.ammo.OnReload.RemoveListener(new UnityAction(this.OnReload));
		}

		// Token: 0x0400043A RID: 1082
		[SerializeField]
		private GameObject obj;

		// Token: 0x0400043B RID: 1083
		[SerializeField]
		private SoundEffectSO sfx;

		// Token: 0x0400043C RID: 1084
		private Ammo ammo;
	}
}
