using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000EE RID: 238
	public class ThunderOnHit : MonoBehaviour
	{
		// Token: 0x060006EA RID: 1770 RVA: 0x0001E9B7 File Offset: 0x0001CBB7
		private void Start()
		{
			this.AddObserver(new Action<object, object>(this.OnImpact), Projectile.ImpactEvent, PlayerController.Instance.gameObject);
			this.TGen = ThunderGenerator.SharedInstance;
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0001E9E5 File Offset: 0x0001CBE5
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnImpact), Projectile.ImpactEvent, PlayerController.Instance.gameObject);
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0001EA08 File Offset: 0x0001CC08
		private void OnImpact(object sender, object args)
		{
			if (Random.Range(0f, 1f) < this.chanceToHit && (args as GameObject).tag.Contains("Enemy"))
			{
				this.TGen.GenerateAt(args as GameObject, this.baseDamage);
			}
		}

		// Token: 0x040004B6 RID: 1206
		[Range(0f, 1f)]
		public float chanceToHit;

		// Token: 0x040004B7 RID: 1207
		public int baseDamage;

		// Token: 0x040004B8 RID: 1208
		private ThunderGenerator TGen;
	}
}
