using System;
using System.Collections;
using Characters.Abilities;
using Characters.Abilities.Upgrades;
using Characters.Operations;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x02000868 RID: 2152
	public class Arson : SimpleStatBonusKeyword
	{
		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x06002CFE RID: 11518 RVA: 0x0008903C File Offset: 0x0008723C
		protected override double[] statBonusByStep
		{
			get
			{
				return this._statBonusByLevel;
			}
		}

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x06002CFF RID: 11519 RVA: 0x00089044 File Offset: 0x00087244
		protected override Stat.Category statCategory
		{
			get
			{
				return Stat.Category.Percent;
			}
		}

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x06002D00 RID: 11520 RVA: 0x0008904B File Offset: 0x0008724B
		protected override Stat.Kind statKind
		{
			get
			{
				return Stat.Kind.EmberDamage;
			}
		}

		// Token: 0x06002D01 RID: 11521 RVA: 0x00089052 File Offset: 0x00087252
		protected override void Initialize()
		{
			base.Initialize();
			this._deathrattle.Initialize(this);
			this._step1Ability.Initialize();
		}

		// Token: 0x06002D02 RID: 11522 RVA: 0x00089074 File Offset: 0x00087274
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			base.UpdateStat();
			if (this.keyword.step >= 1)
			{
				if (!base.character.ability.Contains(this._step1Ability.ability) && base.character.ability.GetInstance<KettleOfSwampWitch>() == null)
				{
					base.character.ability.Add(this._step1Ability.ability);
				}
			}
			else
			{
				base.character.ability.Remove(this._step1Ability.ability);
			}
			if (this.keyword.isMaxStep)
			{
				this._deathrattle.Attach();
				return;
			}
			this._deathrattle.Detach();
		}

		// Token: 0x06002D03 RID: 11523 RVA: 0x00089122 File Offset: 0x00087322
		private void Update()
		{
			if (!this.keyword.isMaxStep)
			{
				return;
			}
			this._deathrattle.UpdateTime(base.character.chronometer.master.deltaTime);
		}

		// Token: 0x06002D04 RID: 11524 RVA: 0x00089152 File Offset: 0x00087352
		public override void Detach()
		{
			base.character.ability.Remove(this._step1Ability.ability);
			this._deathrattle.Detach();
		}

		// Token: 0x040025C3 RID: 9667
		[SerializeField]
		private double[] _statBonusByLevel;

		// Token: 0x040025C4 RID: 9668
		[SerializeField]
		[Header("1 Step 효과")]
		private OperationByTriggerComponent _step1Ability;

		// Token: 0x040025C5 RID: 9669
		[SerializeField]
		[Header("4 Step 효과")]
		private Arson.Deathrattle _deathrattle;

		// Token: 0x02000869 RID: 2153
		[Serializable]
		private class Deathrattle
		{
			// Token: 0x06002D06 RID: 11526 RVA: 0x0008917B File Offset: 0x0008737B
			public void Initialize(Arson volcano)
			{
				this._arson = volcano;
				this._waitForExplosionDelay = new WaitForSeconds(this._explosionDelay);
				this._operation.Initialize();
			}

			// Token: 0x06002D07 RID: 11527 RVA: 0x000891A0 File Offset: 0x000873A0
			public void Attach()
			{
				if (this._arson != null)
				{
					Character character = this._arson.character;
					character.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character.onKilled, new Character.OnKilledDelegate(this.OnKilled));
					this._arson.character.status.Register(CharacterStatus.Kind.Burn, CharacterStatus.Timing.Refresh, new CharacterStatus.OnTimeDelegate(this.HandleOnReleaseBurn));
					this._arson.character.status.Register(CharacterStatus.Kind.Burn, CharacterStatus.Timing.Release, new CharacterStatus.OnTimeDelegate(this.HandleOnReleaseBurn));
				}
			}

			// Token: 0x06002D08 RID: 11528 RVA: 0x0008922D File Offset: 0x0008742D
			public void UpdateTime(float deltaTime)
			{
				this._remainCooldownTime -= deltaTime;
			}

			// Token: 0x06002D09 RID: 11529 RVA: 0x00089240 File Offset: 0x00087440
			public void Detach()
			{
				if (this._arson != null)
				{
					Character character = this._arson.character;
					character.onKilled = (Character.OnKilledDelegate)Delegate.Remove(character.onKilled, new Character.OnKilledDelegate(this.OnKilled));
					this._arson.character.status.Unregister(CharacterStatus.Kind.Burn, CharacterStatus.Timing.Refresh, new CharacterStatus.OnTimeDelegate(this.HandleOnReleaseBurn));
					this._arson.character.status.Unregister(CharacterStatus.Kind.Burn, CharacterStatus.Timing.Release, new CharacterStatus.OnTimeDelegate(this.HandleOnReleaseBurn));
				}
			}

			// Token: 0x06002D0A RID: 11530 RVA: 0x000892D0 File Offset: 0x000874D0
			private void HandleOnReleaseBurn(Character attacker, Character target)
			{
				if (this._remainCooldownTime > 0f)
				{
					return;
				}
				if (target == null || target.status == null || target.health.dead)
				{
					return;
				}
				this._remainCooldownTime = this._cooldownTime;
				this._arson.StartCoroutine(this.CSwampExplode(target));
			}

			// Token: 0x06002D0B RID: 11531 RVA: 0x00089330 File Offset: 0x00087530
			private void OnKilled(ITarget target, ref Damage damage)
			{
				if (this._remainCooldownTime > 0f)
				{
					return;
				}
				if (target == null || target.character == null || target.character.status == null || !target.character.status.burning)
				{
					return;
				}
				this._remainCooldownTime = this._cooldownTime;
				this._arson.StartCoroutine(this.CSwampExplode(target.character));
			}

			// Token: 0x06002D0C RID: 11532 RVA: 0x000893A6 File Offset: 0x000875A6
			private IEnumerator CSwampExplode(Character target)
			{
				Vector3 position = target.transform.position;
				Vector2 offset = target.collider.offset;
				position.x += offset.x;
				position.y += offset.y;
				yield return this._waitForExplosionDelay;
				this._range.transform.position = position;
				this._operation.Run(this._arson.character);
				yield break;
			}

			// Token: 0x040025C6 RID: 9670
			[SerializeField]
			[Space]
			private Transform _range;

			// Token: 0x040025C7 RID: 9671
			[SerializeField]
			private float _explosionDelay;

			// Token: 0x040025C8 RID: 9672
			[SerializeField]
			private float _cooldownTime;

			// Token: 0x040025C9 RID: 9673
			private WaitForSeconds _waitForExplosionDelay;

			// Token: 0x040025CA RID: 9674
			[Space]
			[SerializeField]
			[CharacterOperation.SubcomponentAttribute]
			private CharacterOperation.Subcomponents _operation;

			// Token: 0x040025CB RID: 9675
			private float _remainCooldownTime;

			// Token: 0x040025CC RID: 9676
			private Arson _arson;
		}
	}
}
