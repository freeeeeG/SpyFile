using System;
using Characters;
using Services;
using Singletons;
using UnityEngine;

namespace BT.Conditions
{
	// Token: 0x02001431 RID: 5169
	public class PlayerInRange : Condition
	{
		// Token: 0x0600656D RID: 25965 RVA: 0x00125880 File Offset: 0x00123A80
		protected override bool Check(Context context)
		{
			Transform transform = context.Get<Transform>(Key.OwnerTransform);
			Character player = Singleton<Service>.Instance.levelManager.player;
			return !(transform == null) && !(player == null) && Vector2.Distance(player.transform.position, transform.position) < this._distance;
		}

		// Token: 0x040051AD RID: 20909
		[SerializeField]
		private float _distance;
	}
}
