using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000078 RID: 120
	public class CurseOnHit : MonoBehaviour
	{
		// Token: 0x060004FD RID: 1277 RVA: 0x00018EEF File Offset: 0x000170EF
		private void Start()
		{
			this.AddObserver(new Action<object, object>(this.OnImpact), Projectile.ImpactEvent, PlayerController.Instance.gameObject);
			this.CurseSys = CurseSystem.Instance;
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00018F1D File Offset: 0x0001711D
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnImpact), Projectile.ImpactEvent, PlayerController.Instance.gameObject);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00018F40 File Offset: 0x00017140
		private void OnImpact(object sender, object args)
		{
			if (Random.Range(0f, 1f) < this.chanceToHit && (sender as MonoBehaviour).gameObject.tag == "Bullet")
			{
				GameObject gameObject = args as GameObject;
				if (gameObject.tag.Contains("Enemy"))
				{
					this.CurseSys.Curse(gameObject);
				}
			}
		}

		// Token: 0x040002EC RID: 748
		[Range(0f, 1f)]
		public float chanceToHit;

		// Token: 0x040002ED RID: 749
		private CurseSystem CurseSys;
	}
}
