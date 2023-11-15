using System;
using FX;
using UnityEngine;

namespace Characters.Operations.Fx
{
	// Token: 0x02000F1D RID: 3869
	public class SpawnEffect : CharacterOperation
	{
		// Token: 0x06004B7D RID: 19325 RVA: 0x000DE322 File Offset: 0x000DC522
		private void Awake()
		{
			if (this._spawnPosition == null)
			{
				this._spawnPosition = base.transform;
			}
			this._info.LoadReference();
		}

		// Token: 0x06004B7E RID: 19326 RVA: 0x000DE349 File Offset: 0x000DC549
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._info.Dispose();
		}

		// Token: 0x06004B7F RID: 19327 RVA: 0x000DE35C File Offset: 0x000DC55C
		public override void Run(Character owner)
		{
			EffectPoolInstance effectPoolInstance;
			if (this._extraAngleBySpawnPositionRotation)
			{
				effectPoolInstance = this._info.Spawn(this._spawnPosition.position, owner, this._spawnPosition.rotation.eulerAngles.z, 1f);
			}
			else
			{
				effectPoolInstance = this._info.Spawn(this._spawnPosition.position, owner, 0f, 1f);
			}
			if (this._attachToSpawnPosition)
			{
				effectPoolInstance.transform.parent = this._spawnPosition;
			}
			if (this._scaleBySpawnPositionScale)
			{
				Vector3 lossyScale = this._spawnPosition.lossyScale;
				lossyScale.x = Mathf.Abs(lossyScale.x);
				lossyScale.y = Mathf.Abs(lossyScale.y);
				effectPoolInstance.transform.localScale = Vector3.Scale(effectPoolInstance.transform.localScale, lossyScale);
			}
		}

		// Token: 0x06004B80 RID: 19328 RVA: 0x000DE437 File Offset: 0x000DC637
		public override void Stop()
		{
			this._info.DespawnChildren();
		}

		// Token: 0x04003AB9 RID: 15033
		[SerializeField]
		private Transform _spawnPosition;

		// Token: 0x04003ABA RID: 15034
		[SerializeField]
		private bool _extraAngleBySpawnPositionRotation = true;

		// Token: 0x04003ABB RID: 15035
		[SerializeField]
		private bool _attachToSpawnPosition;

		// Token: 0x04003ABC RID: 15036
		[SerializeField]
		private bool _scaleBySpawnPositionScale;

		// Token: 0x04003ABD RID: 15037
		[SerializeField]
		private EffectInfo _info;
	}
}
