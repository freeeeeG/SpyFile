using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000D1 RID: 209
	public class AutoKillBelowHP : MonoBehaviour
	{
		// Token: 0x06000673 RID: 1651 RVA: 0x0001D584 File Offset: 0x0001B784
		private void Start()
		{
			this.AddObserver(new Action<object, object>(this.OnImpact), Projectile.ImpactEvent, PlayerController.Instance.gameObject);
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0001D5A7 File Offset: 0x0001B7A7
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnImpact), Projectile.ImpactEvent, PlayerController.Instance.gameObject);
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0001D5CC File Offset: 0x0001B7CC
		private void OnImpact(object sender, object args)
		{
			if ((sender as MonoBehaviour).gameObject.tag == "Bullet")
			{
				GameObject gameObject = args as GameObject;
				if (gameObject.tag.Contains("Enemy"))
				{
					Health component = gameObject.GetComponent<Health>();
					if ((float)((component != null) ? new int?(component.HP) : null).Value / (float)((component != null) ? new int?(component.maxHP) : null).Value <= this.autoKillPercent && component.HP != 0)
					{
						component.AutoKill(true);
					}
				}
			}
		}

		// Token: 0x04000447 RID: 1095
		[Range(0f, 1f)]
		public float autoKillPercent;
	}
}
