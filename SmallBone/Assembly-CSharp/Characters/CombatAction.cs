using System;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006DB RID: 1755
	public class CombatAction : MonoBehaviour
	{
		// Token: 0x06002399 RID: 9113 RVA: 0x0006AC7C File Offset: 0x00068E7C
		private void Awake()
		{
			this._mightyBlow = new CombatAction.MightyBlow(this._character);
			this._massacre = new CombatAction.Massacre(this._character);
			Character character = this._character;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(this.onGaveDamage));
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x0006ACD4 File Offset: 0x00068ED4
		private void Update()
		{
			this._mightyBlow.Update(this._character.chronometer.master.deltaTime);
			this._massacre.Update(this._character.chronometer.master.deltaTime);
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x0006AD24 File Offset: 0x00068F24
		private void onGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (target.character == null || target.character.health.currentHealth > 0.0)
			{
				return;
			}
			CombatAction.MightyBlow mightyBlow = this._mightyBlow;
			int streaks = mightyBlow.streaks;
			mightyBlow.streaks = streaks + 1;
			CombatAction.Massacre massacre = this._massacre;
			streaks = massacre.streaks;
			massacre.streaks = streaks + 1;
		}

		// Token: 0x04001E3B RID: 7739
		[GetComponent]
		[SerializeField]
		private Character _character;

		// Token: 0x04001E3C RID: 7740
		private CombatAction.MightyBlow _mightyBlow;

		// Token: 0x04001E3D RID: 7741
		private CombatAction.Massacre _massacre;

		// Token: 0x020006DC RID: 1756
		private class MightyBlow
		{
			// Token: 0x17000789 RID: 1929
			// (get) Token: 0x0600239D RID: 9117 RVA: 0x0006AD85 File Offset: 0x00068F85
			// (set) Token: 0x0600239E RID: 9118 RVA: 0x0006AD8D File Offset: 0x00068F8D
			public int streaks
			{
				get
				{
					return this._streaks;
				}
				set
				{
					if (this._streaks == value)
					{
						return;
					}
					this._streaks = value;
					this._remainTime = 0.5f;
					this.UpdateStat();
				}
			}

			// Token: 0x0600239F RID: 9119 RVA: 0x0006ADB4 File Offset: 0x00068FB4
			internal MightyBlow(Character owner)
			{
				this._owner = owner;
				this._movementSpeed = new Stat.Value(Stat.Category.Percent, Stat.Kind.MovementSpeed, 1.0);
				this._stat = new Stat.Values(new Stat.Value[]
				{
					this._movementSpeed
				});
			}

			// Token: 0x060023A0 RID: 9120 RVA: 0x0006AE11 File Offset: 0x00069011
			internal void Update(float deltaTime)
			{
				this._remainTime -= deltaTime;
				if (this._remainTime <= 0f)
				{
					this._remainTime = 0.5f;
					this.streaks = 0;
				}
			}

			// Token: 0x060023A1 RID: 9121 RVA: 0x0006AE40 File Offset: 0x00069040
			internal void UpdateStat()
			{
				float duration = 0f;
				if (this._streaks > 5)
				{
					this._movementSpeed.value = 1.100000023841858;
					duration = 3f;
				}
				else if (this._streaks > 7)
				{
					this._movementSpeed.value = 1.2000000476837158;
					duration = 4f;
				}
				else if (this._streaks > 10)
				{
					this._movementSpeed.value = 1.2999999523162842;
					duration = 5f;
				}
				this._owner.stat.AttachOrUpdateTimedValues(this._stat, duration, null);
			}

			// Token: 0x04001E3E RID: 7742
			private const float _timeout = 0.5f;

			// Token: 0x04001E3F RID: 7743
			private Stat.Value _movementSpeed;

			// Token: 0x04001E40 RID: 7744
			private Stat.Values _stat;

			// Token: 0x04001E41 RID: 7745
			private readonly Character _owner;

			// Token: 0x04001E42 RID: 7746
			private int _streaks;

			// Token: 0x04001E43 RID: 7747
			private float _remainTime = 0.5f;
		}

		// Token: 0x020006DD RID: 1757
		private class Massacre
		{
			// Token: 0x1700078A RID: 1930
			// (get) Token: 0x060023A2 RID: 9122 RVA: 0x0006AED9 File Offset: 0x000690D9
			// (set) Token: 0x060023A3 RID: 9123 RVA: 0x0006AEE4 File Offset: 0x000690E4
			public int streaks
			{
				get
				{
					return this._streaks;
				}
				set
				{
					if (this._streaks == value)
					{
						return;
					}
					this._streaks = value;
					this._remainTime = 5f;
					if (this.streaks > 100)
					{
						this._remainTime = 1f;
					}
					else if (this.streaks > 75)
					{
						this._remainTime = 2f;
					}
					else if (this.streaks > 50)
					{
						this._remainTime = 3f;
					}
					else if (this.streaks > 25)
					{
						this._remainTime = 4f;
					}
					else if (this.streaks > 10)
					{
						this._remainTime = 5f;
					}
					this.UpdateStat();
				}
			}

			// Token: 0x060023A4 RID: 9124 RVA: 0x0006AF84 File Offset: 0x00069184
			internal Massacre(Character owner)
			{
				this._owner = owner;
				this._movementSpeed = new Stat.Value(Stat.Category.Percent, Stat.Kind.MovementSpeed, 1.0);
				this._stat = new Stat.Values(new Stat.Value[]
				{
					this._movementSpeed
				});
			}

			// Token: 0x060023A5 RID: 9125 RVA: 0x0006AFE1 File Offset: 0x000691E1
			internal void Update(float deltaTime)
			{
				this._remainTime -= deltaTime;
				if (this._remainTime <= 0f)
				{
					this._remainTime = 10f;
					this.streaks = 0;
				}
			}

			// Token: 0x060023A6 RID: 9126 RVA: 0x0006B010 File Offset: 0x00069210
			internal void UpdateStat()
			{
				float duration = 0f;
				if (this.streaks > 100)
				{
					this._movementSpeed.value = 1.5;
					duration = 10f;
				}
				else if (this.streaks > 75)
				{
					this._movementSpeed.value = 1.399999976158142;
					duration = 10f;
				}
				else if (this.streaks > 50)
				{
					this._movementSpeed.value = 1.2999999523162842;
					duration = 10f;
				}
				else if (this.streaks > 25)
				{
					this._movementSpeed.value = 1.2000000476837158;
					duration = 10f;
				}
				else if (this.streaks > 10)
				{
					this._movementSpeed.value = 1.100000023841858;
					duration = 10f;
				}
				this._owner.stat.AttachOrUpdateTimedValues(this._stat, duration, null);
			}

			// Token: 0x04001E44 RID: 7748
			private const float _defaultTimeout = 10f;

			// Token: 0x04001E45 RID: 7749
			private Stat.Value _movementSpeed;

			// Token: 0x04001E46 RID: 7750
			private Stat.Values _stat;

			// Token: 0x04001E47 RID: 7751
			private readonly Character _owner;

			// Token: 0x04001E48 RID: 7752
			private int _streaks;

			// Token: 0x04001E49 RID: 7753
			private float _remainTime = 10f;
		}
	}
}
