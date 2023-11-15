using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000E4 RID: 228
	public class FreezeOnHit : MonoBehaviour
	{
		// Token: 0x060006BF RID: 1727 RVA: 0x0001E381 File Offset: 0x0001C581
		private void Start()
		{
			this.AddObserver(new Action<object, object>(this.OnImpact), Projectile.ImpactEvent, PlayerController.Instance.gameObject);
			this.FreezeSys = FreezeSystem.SharedInstance;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0001E3AF File Offset: 0x0001C5AF
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnImpact), Projectile.ImpactEvent, PlayerController.Instance.gameObject);
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0001E3D4 File Offset: 0x0001C5D4
		private void OnImpact(object sender, object args)
		{
			if (Random.Range(0f, 1f) < this.chanceToHit && (sender as MonoBehaviour).gameObject.tag == "Bullet")
			{
				GameObject gameObject = args as GameObject;
				if (gameObject.tag.Contains("Enemy"))
				{
					this.FreezeSys.Freeze(gameObject);
				}
			}
		}

		// Token: 0x0400048A RID: 1162
		[Range(0f, 1f)]
		public float chanceToHit;

		// Token: 0x0400048B RID: 1163
		public float freezeDuration;

		// Token: 0x0400048C RID: 1164
		private FreezeSystem FreezeSys;
	}
}
