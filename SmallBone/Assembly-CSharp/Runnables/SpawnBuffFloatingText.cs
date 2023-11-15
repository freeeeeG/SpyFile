using System;
using Characters;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables
{
	// Token: 0x02000331 RID: 817
	public class SpawnBuffFloatingText : Runnable
	{
		// Token: 0x06000F96 RID: 3990 RVA: 0x0002F3D8 File Offset: 0x0002D5D8
		public override void Run()
		{
			Vector2 v = base.transform.position;
			if (this._floatingPoint != null)
			{
				v = this._floatingPoint.position;
			}
			if (this._toPlayerPosition)
			{
				Character player = Singleton<Service>.Instance.levelManager.player;
				v = new Vector2(player.collider.bounds.center.x, player.collider.bounds.max.y + 0.5f);
			}
			Singleton<Service>.Instance.floatingTextSpawner.SpawnBuff(Localization.GetLocalizedString(this._floatingTextkey), v, "#F2F2F2");
		}

		// Token: 0x04000CD1 RID: 3281
		[SerializeField]
		private string _floatingTextkey;

		// Token: 0x04000CD2 RID: 3282
		[SerializeField]
		private Transform _floatingPoint;

		// Token: 0x04000CD3 RID: 3283
		[SerializeField]
		private bool _toPlayerPosition;
	}
}
