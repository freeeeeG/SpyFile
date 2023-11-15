using System;

namespace Characters.Player
{
	// Token: 0x020007E9 RID: 2025
	public sealed class CombatDetector
	{
		// Token: 0x14000064 RID: 100
		// (add) Token: 0x060028F4 RID: 10484 RVA: 0x0007C764 File Offset: 0x0007A964
		// (remove) Token: 0x060028F5 RID: 10485 RVA: 0x0007C79C File Offset: 0x0007A99C
		public event Action onBeginCombat;

		// Token: 0x14000065 RID: 101
		// (add) Token: 0x060028F6 RID: 10486 RVA: 0x0007C7D4 File Offset: 0x0007A9D4
		// (remove) Token: 0x060028F7 RID: 10487 RVA: 0x0007C80C File Offset: 0x0007AA0C
		public event Action onFinishCombat;

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x060028F8 RID: 10488 RVA: 0x0007C841 File Offset: 0x0007AA41
		// (set) Token: 0x060028F9 RID: 10489 RVA: 0x0007C849 File Offset: 0x0007AA49
		public bool inCombat { get; private set; }

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x060028FA RID: 10490 RVA: 0x0007C852 File Offset: 0x0007AA52
		public float remainTimePercent
		{
			get
			{
				return this._remainCombatTime / 3f;
			}
		}

		// Token: 0x060028FB RID: 10491 RVA: 0x0007C860 File Offset: 0x0007AA60
		internal CombatDetector(Character owner)
		{
			this._owner = owner;
			Character owner2 = this._owner;
			owner2.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner2.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			if (this._owner.health != null)
			{
				this._owner.health.onTookDamage += new TookDamageDelegate(this.OnTookDamage);
			}
		}

		// Token: 0x060028FC RID: 10492 RVA: 0x0007C8D0 File Offset: 0x0007AAD0
		private void Begin()
		{
			if (!this.inCombat)
			{
				this.inCombat = true;
				Action action = this.onBeginCombat;
				if (action != null)
				{
					action();
				}
			}
			this._remainCombatTime = 3f;
		}

		// Token: 0x060028FD RID: 10493 RVA: 0x0007C8FD File Offset: 0x0007AAFD
		private void Finish()
		{
			this.inCombat = false;
			Action action = this.onFinishCombat;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x060028FE RID: 10494 RVA: 0x0007C918 File Offset: 0x0007AB18
		private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			Character character = target.character;
			if (character == null)
			{
				return;
			}
			if (character.type == Character.Type.Dummy)
			{
				return;
			}
			this.Begin();
		}

		// Token: 0x060028FF RID: 10495 RVA: 0x0007C948 File Offset: 0x0007AB48
		private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			Character character = tookDamage.attacker.character;
			if (character == null)
			{
				return;
			}
			if (character.type == Character.Type.Dummy)
			{
				return;
			}
			this.Begin();
		}

		// Token: 0x06002900 RID: 10496 RVA: 0x0007C97B File Offset: 0x0007AB7B
		public void Update(float deltaTime)
		{
			if (!this.inCombat)
			{
				return;
			}
			this._remainCombatTime -= deltaTime;
			if (this._remainCombatTime <= 0f)
			{
				this.Finish();
			}
		}

		// Token: 0x04002365 RID: 9061
		private const float _combatRetentionTime = 3f;

		// Token: 0x04002368 RID: 9064
		private readonly Character _owner;

		// Token: 0x04002369 RID: 9065
		private float _remainCombatTime;
	}
}
