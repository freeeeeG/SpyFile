using System;
using UnityEngine;

namespace Characters.Abilities.Statuses
{
	// Token: 0x02000B6F RID: 2927
	public class Freeze : CharacterStatusAbility, IAbility, IAbilityInstance
	{
		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x06003AE9 RID: 15081 RVA: 0x000ADCAE File Offset: 0x000ABEAE
		// (set) Token: 0x06003AEA RID: 15082 RVA: 0x000ADCB6 File Offset: 0x000ABEB6
		public Character owner { get; private set; }

		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x06003AEB RID: 15083 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000C17 RID: 3095
		// (get) Token: 0x06003AEC RID: 15084 RVA: 0x000ADCBF File Offset: 0x000ABEBF
		// (set) Token: 0x06003AED RID: 15085 RVA: 0x000ADCC7 File Offset: 0x000ABEC7
		public float remainTime { get; set; }

		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x06003AEE RID: 15086 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x06003AEF RID: 15087 RVA: 0x00018C86 File Offset: 0x00016E86
		public Sprite icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x06003AF0 RID: 15088 RVA: 0x000ADCD0 File Offset: 0x000ABED0
		public float iconFillAmount
		{
			get
			{
				return this.remainTime / this.duration;
			}
		}

		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x06003AF1 RID: 15089 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x06003AF2 RID: 15090 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x06003AF3 RID: 15091 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x06003AF4 RID: 15092 RVA: 0x000ADCDF File Offset: 0x000ABEDF
		public bool expired
		{
			get
			{
				return this.remainTime <= 0f;
			}
		}

		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x06003AF5 RID: 15093 RVA: 0x000ADCF4 File Offset: 0x000ABEF4
		public float duration
		{
			get
			{
				return (float)(((double)CharacterStatusSetting.instance.freeze.duration + base.attacker.stat.GetFinal(Stat.Kind.FreezeDuration)) * this.owner.stat.GetStatusResistacneFor(CharacterStatus.Kind.Freeze)) * base.durationMultiplier;
			}
		}

		// Token: 0x17000C20 RID: 3104
		// (get) Token: 0x06003AF6 RID: 15094 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C21 RID: 3105
		// (get) Token: 0x06003AF7 RID: 15095 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool removeOnSwapWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x06003AF8 RID: 15096 RVA: 0x000ADD41 File Offset: 0x000ABF41
		// (set) Token: 0x06003AF9 RID: 15097 RVA: 0x000ADD49 File Offset: 0x000ABF49
		public int hitStack { get; set; }

		// Token: 0x06003AFA RID: 15098 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbilityInstance CreateInstance(Character owner)
		{
			return this;
		}

		// Token: 0x140000A0 RID: 160
		// (add) Token: 0x06003AFB RID: 15099 RVA: 0x000ADD54 File Offset: 0x000ABF54
		// (remove) Token: 0x06003AFC RID: 15100 RVA: 0x000ADD8C File Offset: 0x000ABF8C
		public override event CharacterStatus.OnTimeDelegate onAttachEvents;

		// Token: 0x140000A1 RID: 161
		// (add) Token: 0x06003AFD RID: 15101 RVA: 0x000ADDC4 File Offset: 0x000ABFC4
		// (remove) Token: 0x06003AFE RID: 15102 RVA: 0x000ADDFC File Offset: 0x000ABFFC
		public override event CharacterStatus.OnTimeDelegate onRefreshEvents;

		// Token: 0x140000A2 RID: 162
		// (add) Token: 0x06003AFF RID: 15103 RVA: 0x000ADE34 File Offset: 0x000AC034
		// (remove) Token: 0x06003B00 RID: 15104 RVA: 0x000ADE6C File Offset: 0x000AC06C
		public override event CharacterStatus.OnTimeDelegate onDetachEvents;

		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x06003B01 RID: 15105 RVA: 0x000ADEA1 File Offset: 0x000AC0A1
		// (set) Token: 0x06003B02 RID: 15106 RVA: 0x000ADEA9 File Offset: 0x000AC0A9
		public new StatusEffect.FreezeHandler effectHandler { get; set; }

		// Token: 0x06003B03 RID: 15107 RVA: 0x000ADEB2 File Offset: 0x000AC0B2
		public Freeze(Character owner)
		{
			this.owner = owner;
		}

		// Token: 0x06003B04 RID: 15108 RVA: 0x000ADEC1 File Offset: 0x000AC0C1
		public void UpdateTime(float deltaTime)
		{
			this.remainTime -= deltaTime;
			this.effectHandler.UpdateTime(deltaTime);
		}

		// Token: 0x06003B05 RID: 15109 RVA: 0x000ADEE0 File Offset: 0x000AC0E0
		public void Refresh()
		{
			this.remainTime = this.duration;
			this._breakableTime = this.remainTime - CharacterStatusSetting.instance.freeze.minimumTime;
			this._remainHitStack = this.hitStack;
			CharacterStatus.OnTimeDelegate onTimeDelegate = this.onRefreshEvents;
			if (onTimeDelegate != null)
			{
				onTimeDelegate(base.attacker, this.owner);
			}
			CharacterStatus.OnTimeDelegate onRefreshed = this.onRefreshed;
			if (onRefreshed != null)
			{
				onRefreshed(base.attacker, this.owner);
			}
			this.effectHandler.HandleOnRefresh(base.attacker, this.owner);
		}

		// Token: 0x06003B06 RID: 15110 RVA: 0x000ADF74 File Offset: 0x000AC174
		public void Attach()
		{
			this.remainTime = this.duration;
			this._breakableTime = this.remainTime - CharacterStatusSetting.instance.freeze.minimumTime;
			if (this.owner.movement != null)
			{
				this.owner.movement.blocked.Attach(this);
			}
			this.owner.chronometer.animation.AttachTimeScale(this, 0f);
			this.owner.blockLook.Attach(this);
			this._remainHitStack = this.hitStack;
			this.owner.health.onTookDamage += new TookDamageDelegate(this.OnTookDamage);
			CharacterStatus.OnTimeDelegate onAttached = this.onAttached;
			if (onAttached != null)
			{
				onAttached(base.attacker, this.owner);
			}
			CharacterStatus.OnTimeDelegate onTimeDelegate = this.onAttachEvents;
			if (onTimeDelegate != null)
			{
				onTimeDelegate(base.attacker, this.owner);
			}
			this.effectHandler.HandleOnAttach(base.attacker, this.owner);
		}

		// Token: 0x06003B07 RID: 15111 RVA: 0x000AE078 File Offset: 0x000AC278
		private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (tookDamage.motionType == Damage.MotionType.Status)
			{
				return;
			}
			if (this.remainTime >= this._breakableTime)
			{
				return;
			}
			foreach (string value in CharacterStatusSetting.instance.freeze.nonHitCountAttackKeys)
			{
				if (tookDamage.key.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
			}
			this.effectHandler.HandleOnTakeDamage(base.attacker, this.owner);
			this._remainHitStack--;
			if (this._remainHitStack > 0)
			{
				return;
			}
			this.remainTime = 0f;
		}

		// Token: 0x06003B08 RID: 15112 RVA: 0x000AE10C File Offset: 0x000AC30C
		public void Detach()
		{
			this.remainTime = 0f;
			this.owner.chronometer.animation.DetachTimeScale(this);
			this.owner.movement.push.Expire();
			this.owner.blockLook.Detach(this);
			if (this.owner.movement != null)
			{
				this.owner.movement.blocked.Detach(this);
			}
			this.owner.health.onTookDamage -= new TookDamageDelegate(this.OnTookDamage);
			CharacterStatus.OnTimeDelegate onTimeDelegate = this.onDetachEvents;
			if (onTimeDelegate != null)
			{
				onTimeDelegate(base.attacker, this.owner);
			}
			CharacterStatus.OnTimeDelegate onDetached = this.onDetached;
			if (onDetached != null)
			{
				onDetached(base.attacker, this.owner);
			}
			this.effectHandler.HandleOnDetach(base.attacker, this.owner);
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x000AE1F9 File Offset: 0x000AC3F9
		public void Initialize()
		{
			this.effectHandler = new StatusEffect.Freeze(this.owner);
		}

		// Token: 0x06003B0A RID: 15114 RVA: 0x000AE20C File Offset: 0x000AC40C
		public void AddRemainHitStack()
		{
			this.AddRemainHitStack(1);
		}

		// Token: 0x06003B0B RID: 15115 RVA: 0x000AE215 File Offset: 0x000AC415
		public void AddRemainHitStack(int count)
		{
			if (this.remainTime <= 0f)
			{
				return;
			}
			this._remainHitStack += count;
		}

		// Token: 0x04002EAB RID: 11947
		private int _remainHitStack;

		// Token: 0x04002EAC RID: 11948
		private float _breakableTime;
	}
}
