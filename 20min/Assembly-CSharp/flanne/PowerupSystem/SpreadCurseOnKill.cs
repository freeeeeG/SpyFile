using System;
using UnityEngine;

namespace flanne.PowerupSystem
{
	// Token: 0x02000241 RID: 577
	public class SpreadCurseOnKill : MonoBehaviour
	{
		// Token: 0x06000CA1 RID: 3233 RVA: 0x0002E140 File Offset: 0x0002C340
		private void OnCurseKill(object sender, object args)
		{
			GameObject gameObject = args as GameObject;
			foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(gameObject.transform.position, this.range, 1 << TagLayerUtil.Enemy))
			{
				if (collider2D.gameObject != gameObject)
				{
					this.CS.Curse(collider2D.gameObject);
				}
			}
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x0002E1B0 File Offset: 0x0002C3B0
		private void Start()
		{
			this.CS = CurseSystem.Instance;
			this.AddObserver(new Action<object, object>(this.OnCurseKill), CurseSystem.CurseKillNotification);
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x0002E1D4 File Offset: 0x0002C3D4
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnCurseKill), CurseSystem.CurseKillNotification);
		}

		// Token: 0x040008D7 RID: 2263
		[SerializeField]
		private float range = 2f;

		// Token: 0x040008D8 RID: 2264
		private CurseSystem CS;
	}
}
