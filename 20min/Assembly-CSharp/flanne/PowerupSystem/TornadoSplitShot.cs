using System;
using UnityEngine;

namespace flanne.PowerupSystem
{
	// Token: 0x02000259 RID: 601
	public class TornadoSplitShot : MonoBehaviour
	{
		// Token: 0x06000D09 RID: 3337 RVA: 0x0002F854 File Offset: 0x0002DA54
		private void OnTornadoCollideBullet(object sender, object args)
		{
			Projectile component = (args as GameObject).GetComponent<Projectile>();
			if (component == null || component.isSecondary)
			{
				return;
			}
			component.isSecondary = true;
			component.damage /= 2f;
			Projectile component2 = Object.Instantiate<GameObject>(component.gameObject).GetComponent<Projectile>();
			float magnitude = component.vector.magnitude;
			component.vector = Vector2.up.Rotate((float)Random.Range(0, 360)) * magnitude;
			component2.vector = Vector2.up.Rotate((float)Random.Range(0, 360)) * magnitude;
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x0002F8FA File Offset: 0x0002DAFA
		private void Start()
		{
			this.AddObserver(new Action<object, object>(this.OnTornadoCollideBullet), "TornadoBulletCollision");
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x0002F913 File Offset: 0x0002DB13
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnTornadoCollideBullet), "TornadoBulletCollision");
		}
	}
}
