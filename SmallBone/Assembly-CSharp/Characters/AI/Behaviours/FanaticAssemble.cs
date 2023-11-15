using System;
using System.Collections;
using Characters.Actions;
using FX;
using Level;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012CD RID: 4813
	public sealed class FanaticAssemble : Behaviour
	{
		// Token: 0x06005F3B RID: 24379 RVA: 0x00116DF5 File Offset: 0x00114FF5
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			if (this._waveToSpawn == null)
			{
				Debug.LogError("_waveToSpawn of " + controller.character.name + " is not assigned!");
				yield break;
			}
			if (!this._action.TryStart())
			{
				base.result = Behaviour.Result.Fail;
				yield break;
			}
			while (this._action.running)
			{
				yield return null;
			}
			base.result = Behaviour.Result.Success;
			this.SpawnSpawnEffect();
			float delay = 0f;
			while (delay < this._spawnDelay)
			{
				delay += controller.character.chronometer.master.deltaTime;
				yield return null;
			}
			this._waveToSpawn.Spawn(false);
			yield return this._sacrifice.CRun(controller);
			yield break;
		}

		// Token: 0x06005F3C RID: 24380 RVA: 0x00116E0C File Offset: 0x0011500C
		private void SpawnSpawnEffect()
		{
			foreach (Character character in this._waveToSpawn.characters)
			{
				this._spawnEffect.Spawn(character.transform.position, 0f, 1f);
			}
		}

		// Token: 0x04004C82 RID: 19586
		[SerializeField]
		private EffectInfo _spawnEffect;

		// Token: 0x04004C83 RID: 19587
		[SerializeField]
		private float _spawnDelay = 0.5f;

		// Token: 0x04004C84 RID: 19588
		[SerializeField]
		private EnemyWave _waveToSpawn;

		// Token: 0x04004C85 RID: 19589
		[SerializeField]
		private Characters.Actions.Action _action;

		// Token: 0x04004C86 RID: 19590
		[UnityEditor.Subcomponent(typeof(Sacrifice))]
		[SerializeField]
		private Sacrifice _sacrifice;
	}
}
