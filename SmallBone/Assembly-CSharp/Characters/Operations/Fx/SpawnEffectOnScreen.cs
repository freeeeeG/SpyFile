using System;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Fx
{
	// Token: 0x02000F1E RID: 3870
	public class SpawnEffectOnScreen : Operation
	{
		// Token: 0x06004B82 RID: 19330 RVA: 0x000DE453 File Offset: 0x000DC653
		public override void Run()
		{
			Singleton<ScreenEffectSpawner>.Instance.Spawn(this._info, this._positionOffset);
		}

		// Token: 0x06004B83 RID: 19331 RVA: 0x000DE470 File Offset: 0x000DC670
		public override void Stop()
		{
			this._info.DespawnChildren();
		}

		// Token: 0x04003ABE RID: 15038
		[SerializeField]
		private Vector3 _positionOffset;

		// Token: 0x04003ABF RID: 15039
		[SerializeField]
		private EffectInfo _info;
	}
}
