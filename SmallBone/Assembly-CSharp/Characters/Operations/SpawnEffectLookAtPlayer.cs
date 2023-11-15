using System;
using System.Collections.Generic;
using Characters.AI;
using FX;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E07 RID: 3591
	public class SpawnEffectLookAtPlayer : CharacterOperation
	{
		// Token: 0x060047C2 RID: 18370 RVA: 0x000D0D2C File Offset: 0x000CEF2C
		private void Awake()
		{
			if (this._spawnPosition == null)
			{
				this._spawnPosition = base.transform;
			}
		}

		// Token: 0x060047C3 RID: 18371 RVA: 0x000D0D48 File Offset: 0x000CEF48
		public override void Run(Character owner)
		{
			float x = this._spawnPosition.position.x;
			float x2 = owner.transform.position.x;
			float x3 = this._aIController.target.transform.position.x;
			this._info.flipXByOwnerDirection = (Mathf.Abs(x2 - x) < Mathf.Abs(x2 - x3));
			EffectPoolInstance effectPoolInstance = this._info.Spawn(this._spawnPosition.position, owner, this._spawnPosition.rotation.eulerAngles.z, 1f);
			if (this._attachToSpawnPosition)
			{
				effectPoolInstance.transform.parent = this._spawnPosition;
			}
		}

		// Token: 0x060047C4 RID: 18372 RVA: 0x000D0E02 File Offset: 0x000CF002
		public override void Stop()
		{
			this._info.DespawnChildren();
		}

		// Token: 0x040036DE RID: 14046
		[SerializeField]
		private Transform _spawnPosition;

		// Token: 0x040036DF RID: 14047
		[SerializeField]
		private bool _attachToSpawnPosition;

		// Token: 0x040036E0 RID: 14048
		[SerializeField]
		private AIController _aIController;

		// Token: 0x040036E1 RID: 14049
		[SerializeField]
		private EffectInfo _info;

		// Token: 0x040036E2 RID: 14050
		private readonly List<ReusableChronoSpriteEffect> _effects = new List<ReusableChronoSpriteEffect>();
	}
}
