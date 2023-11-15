using System;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FDA RID: 4058
	public class FlipByPlayerPosition : Operation
	{
		// Token: 0x06004E72 RID: 20082 RVA: 0x000EB0FC File Offset: 0x000E92FC
		public override void Run()
		{
			Transform transform = Singleton<Service>.Instance.levelManager.player.transform;
			if (this._body.position.x < transform.position.x)
			{
				this._body.localScale = new Vector2(1f, 1f);
				return;
			}
			this._body.localScale = new Vector2(-1f, 1f);
		}

		// Token: 0x04003E8C RID: 16012
		[SerializeField]
		private Transform _body;
	}
}
