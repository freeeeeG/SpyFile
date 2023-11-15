using System;
using Characters.Actions;
using Characters.AI;
using FX;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006C2 RID: 1730
	public class CharacterHit : MonoBehaviour
	{
		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x060022C2 RID: 8898 RVA: 0x00068C99 File Offset: 0x00066E99
		public Characters.Actions.Action action
		{
			get
			{
				return this._action;
			}
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x00068CA1 File Offset: 0x00066EA1
		private void Awake()
		{
			this._health.onTookDamage += new TookDamageDelegate(this.onTookDamage);
		}

		// Token: 0x060022C4 RID: 8900 RVA: 0x00068CBA File Offset: 0x00066EBA
		private void onTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (damageDealt > 0.0)
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(this._hitSound, base.transform.position);
			}
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x00068CE4 File Offset: 0x00066EE4
		public void Stop(float stoppingPower)
		{
			stoppingPower *= (float)this._character.stat.GetFinal(Stat.Kind.StoppingResistance);
			if (stoppingPower <= 0f || this._action == null || this._action.currentMotion == null || this._character.stunedOrFreezed || this._character.health.dead)
			{
				return;
			}
			if (this._deadAction != null && this._deadAction.diedAction.running)
			{
				return;
			}
			this._action.currentMotion.length = stoppingPower;
			this._action.TryStart();
		}

		// Token: 0x04001DA0 RID: 7584
		[SerializeField]
		[GetComponent]
		private Character _character;

		// Token: 0x04001DA1 RID: 7585
		[SerializeField]
		[GetComponent]
		private CharacterHealth _health;

		// Token: 0x04001DA2 RID: 7586
		[SerializeField]
		protected SoundInfo _hitSound;

		// Token: 0x04001DA3 RID: 7587
		[SerializeField]
		[Subcomponent(true, typeof(SequentialAction))]
		private SequentialAction _action;

		// Token: 0x04001DA4 RID: 7588
		[SerializeField]
		private EnemyDiedAction _deadAction;

		// Token: 0x04001DA5 RID: 7589
		private int _motionIndex;
	}
}
