using System;
using FX;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x02000797 RID: 1943
	public class SpawnEffect : Operation
	{
		// Token: 0x060027BD RID: 10173 RVA: 0x00077616 File Offset: 0x00075816
		private void Awake()
		{
			if (this._spawnPosition == null)
			{
				this._spawnPosition = base.transform;
			}
			this._info.LoadReference();
		}

		// Token: 0x060027BE RID: 10174 RVA: 0x0007763D File Offset: 0x0007583D
		private void OnDestroy()
		{
			this._info.Dispose();
		}

		// Token: 0x060027BF RID: 10175 RVA: 0x0007764C File Offset: 0x0007584C
		public override void Run(IProjectile projectile)
		{
			EffectPoolInstance effectPoolInstance;
			if (this._extraAngleBySpawnPositionRotation)
			{
				effectPoolInstance = this._info.Spawn(this._spawnPosition.position, this._spawnPosition.rotation.eulerAngles.z, 1f);
			}
			else
			{
				effectPoolInstance = this._info.Spawn(this._spawnPosition.position, 0f, 1f);
			}
			if (this._attachToSpawnPosition)
			{
				effectPoolInstance.transform.parent = this._spawnPosition;
			}
			if (this._scaleBySpawnPositionScale)
			{
				Vector3 lossyScale = this._spawnPosition.lossyScale;
				lossyScale.x = Mathf.Abs(lossyScale.x);
				if (Mathf.Abs(this._spawnPosition.rotation.eulerAngles.y) == 180f)
				{
					lossyScale.x *= -1f;
				}
				lossyScale.y = Mathf.Abs(lossyScale.y);
				effectPoolInstance.transform.localScale = Vector3.Scale(effectPoolInstance.transform.localScale, lossyScale);
			}
		}

		// Token: 0x040021D9 RID: 8665
		[SerializeField]
		private Transform _spawnPosition;

		// Token: 0x040021DA RID: 8666
		[SerializeField]
		private bool _extraAngleBySpawnPositionRotation = true;

		// Token: 0x040021DB RID: 8667
		[SerializeField]
		private bool _attachToSpawnPosition;

		// Token: 0x040021DC RID: 8668
		[SerializeField]
		private bool _scaleBySpawnPositionScale;

		// Token: 0x040021DD RID: 8669
		[SerializeField]
		private EffectInfo _info;
	}
}
