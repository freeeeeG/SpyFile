using System;
using FX;
using UnityEngine;

namespace Characters.Projectiles.Operations.Customs
{
	// Token: 0x020007A4 RID: 1956
	public class SpawnEffectProportionToScale : Operation
	{
		// Token: 0x060027F9 RID: 10233 RVA: 0x00078CFA File Offset: 0x00076EFA
		private void Awake()
		{
			if (this._spawnPosition == null)
			{
				this._spawnPosition = base.transform;
			}
		}

		// Token: 0x060027FA RID: 10234 RVA: 0x00078D18 File Offset: 0x00076F18
		public override void Run(IProjectile projectile)
		{
			EffectPoolInstance effectPoolInstance = this._info.Spawn(this._spawnPosition.position, this._spawnPosition.rotation.eulerAngles.z, 1f);
			if (this._attachToSpawnPosition)
			{
				effectPoolInstance.transform.parent = this._spawnPosition;
			}
			effectPoolInstance.transform.localScale = base.transform.lossyScale;
		}

		// Token: 0x0400222B RID: 8747
		[SerializeField]
		private Transform _spawnPosition;

		// Token: 0x0400222C RID: 8748
		[SerializeField]
		private bool _attachToSpawnPosition;

		// Token: 0x0400222D RID: 8749
		[SerializeField]
		private EffectInfo _info;
	}
}
