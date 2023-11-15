using System;
using System.Collections;
using Characters;
using PhysicsUtils;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x0200068A RID: 1674
	public class TrapController : MonoBehaviour
	{
		// Token: 0x0600217A RID: 8570 RVA: 0x000649FB File Offset: 0x00062BFB
		static TrapController()
		{
			TrapController._lapper.contactFilter.SetLayerMask(512);
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x00064A22 File Offset: 0x00062C22
		private void Start()
		{
			this.Initialize(this._activate, new Action(this._targetTrap.Activate));
			this.Initialize(this._deactivate, new Action(this._targetTrap.Deactivate));
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x00064A60 File Offset: 0x00062C60
		private void Initialize(TrapController.Config config, Action run)
		{
			switch (config.condition)
			{
			case TrapController.Config.Condition.None:
				if (config == this._activate)
				{
					this._targetTrap.Activate();
					return;
				}
				break;
			case TrapController.Config.Condition.OnCharacterDie:
				config.character.health.onDied += run;
				return;
			case TrapController.Config.Condition.OnTargetWaveSpawn:
				if (config.targetWave.state != Wave.State.Spawned)
				{
					config.targetWave.onSpawn += run;
					return;
				}
				if (run != null)
				{
					run();
					return;
				}
				break;
			case TrapController.Config.Condition.OnTargetWaveClear:
				config.targetWave.onClear += run;
				return;
			case TrapController.Config.Condition.PlayerOnTriggerEnter:
				base.StartCoroutine(this.CTriggerEnter(config));
				this.OnColliderEnter = (Action)Delegate.Combine(this.OnColliderEnter, run);
				return;
			case TrapController.Config.Condition.PlayerOnTriggerExit:
				base.StartCoroutine(this.CTriggerExit(config));
				this.OnColliderExit = (Action)Delegate.Combine(this.OnColliderExit, run);
				return;
			case TrapController.Config.Condition.OnPropDestory:
				config.prop.onDestroy += run;
				break;
			default:
				return;
			}
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x00064B4B File Offset: 0x00062D4B
		private IEnumerator CTriggerEnter(TrapController.Config config)
		{
			for (;;)
			{
				if (!(TrapController._lapper.OverlapCollider(config.range).GetComponent<Character>() == null))
				{
					Action onColliderEnter = this.OnColliderEnter;
					if (onColliderEnter != null)
					{
						onColliderEnter();
					}
					if (config.once)
					{
						break;
					}
					while (TrapController._lapper.OverlapCollider(config.range).GetComponent<Character>() != null)
					{
						yield return null;
					}
				}
				else
				{
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x00064B61 File Offset: 0x00062D61
		private IEnumerator CTriggerExit(TrapController.Config config)
		{
			for (;;)
			{
				if (!(TrapController._lapper.OverlapCollider(config.range).GetComponent<Character>() == null))
				{
					while (TrapController._lapper.OverlapCollider(config.range).GetComponent<Character>() != null)
					{
						yield return null;
					}
					Action onColliderExit = this.OnColliderExit;
					if (onColliderExit != null)
					{
						onColliderExit();
					}
					if (config.once)
					{
						break;
					}
				}
				else
				{
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x04001C86 RID: 7302
		[SerializeField]
		private ControlableTrap _targetTrap;

		// Token: 0x04001C87 RID: 7303
		[SerializeField]
		private TrapController.Config _activate;

		// Token: 0x04001C88 RID: 7304
		[SerializeField]
		private TrapController.Config _deactivate;

		// Token: 0x04001C89 RID: 7305
		private Action OnColliderEnter;

		// Token: 0x04001C8A RID: 7306
		private Action OnColliderExit;

		// Token: 0x04001C8B RID: 7307
		private static readonly NonAllocOverlapper _lapper = new NonAllocOverlapper(15);

		// Token: 0x0200068B RID: 1675
		[Serializable]
		public class Config
		{
			// Token: 0x04001C8C RID: 7308
			[SerializeField]
			internal TrapController.Config.Condition condition;

			// Token: 0x04001C8D RID: 7309
			[SerializeField]
			internal Character character;

			// Token: 0x04001C8E RID: 7310
			[SerializeField]
			internal EnemyWave targetWave;

			// Token: 0x04001C8F RID: 7311
			[SerializeField]
			internal Collider2D range;

			// Token: 0x04001C90 RID: 7312
			[SerializeField]
			internal Prop prop;

			// Token: 0x04001C91 RID: 7313
			[SerializeField]
			internal bool once;

			// Token: 0x0200068C RID: 1676
			internal enum Condition
			{
				// Token: 0x04001C93 RID: 7315
				None,
				// Token: 0x04001C94 RID: 7316
				OnCharacterDie,
				// Token: 0x04001C95 RID: 7317
				OnTargetWaveSpawn,
				// Token: 0x04001C96 RID: 7318
				OnTargetWaveClear,
				// Token: 0x04001C97 RID: 7319
				PlayerOnTriggerEnter,
				// Token: 0x04001C98 RID: 7320
				PlayerOnTriggerExit,
				// Token: 0x04001C99 RID: 7321
				OnPropDestory
			}
		}
	}
}
