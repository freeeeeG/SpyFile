using System;
using FX;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DF7 RID: 3575
	public sealed class SpawnEffectInContainer : CharacterOperation
	{
		// Token: 0x0600478F RID: 18319 RVA: 0x000CFCB0 File Offset: 0x000CDEB0
		public override void Run(Character owner)
		{
			foreach (object obj in this._container)
			{
				Transform transform = (Transform)obj;
				EffectPoolInstance effectPoolInstance = this._info.Spawn(transform.position, owner, transform.rotation.eulerAngles.z, 1f);
				if (this._attachToSpawnPosition)
				{
					effectPoolInstance.transform.parent = transform;
				}
				if (this._scaleBySpawnPositionScale)
				{
					Vector3 lossyScale = transform.lossyScale;
					lossyScale.x = Mathf.Abs(lossyScale.x);
					lossyScale.y = Mathf.Abs(lossyScale.y);
					effectPoolInstance.transform.localScale = Vector3.Scale(effectPoolInstance.transform.localScale, lossyScale);
				}
			}
		}

		// Token: 0x06004790 RID: 18320 RVA: 0x000CFDA0 File Offset: 0x000CDFA0
		public override void Stop()
		{
			this._info.DespawnChildren();
		}

		// Token: 0x0400368E RID: 13966
		[SerializeField]
		private Transform _container;

		// Token: 0x0400368F RID: 13967
		[SerializeField]
		private bool _attachToSpawnPosition;

		// Token: 0x04003690 RID: 13968
		[SerializeField]
		private bool _scaleBySpawnPositionScale;

		// Token: 0x04003691 RID: 13969
		[SerializeField]
		private EffectInfo _info;
	}
}
