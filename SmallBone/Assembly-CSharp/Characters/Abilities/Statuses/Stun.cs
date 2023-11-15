using System;
using UnityEngine;

namespace Characters.Abilities.Statuses
{
	// Token: 0x02000B7D RID: 2941
	public class Stun : CharacterStatusAbility, IAbility, IAbilityInstance
	{
		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x06003B85 RID: 15237 RVA: 0x000AFD68 File Offset: 0x000ADF68
		// (set) Token: 0x06003B86 RID: 15238 RVA: 0x000AFD70 File Offset: 0x000ADF70
		public Character owner { get; private set; }

		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x06003B87 RID: 15239 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x06003B88 RID: 15240 RVA: 0x000AFD79 File Offset: 0x000ADF79
		// (set) Token: 0x06003B89 RID: 15241 RVA: 0x000AFD81 File Offset: 0x000ADF81
		public float remainTime { get; set; }

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x06003B8A RID: 15242 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x06003B8B RID: 15243 RVA: 0x00018C86 File Offset: 0x00016E86
		public Sprite icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x06003B8C RID: 15244 RVA: 0x000AFD8A File Offset: 0x000ADF8A
		public float iconFillAmount
		{
			get
			{
				return this.remainTime / this.duration;
			}
		}

		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x06003B8D RID: 15245 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x06003B8E RID: 15246 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x06003B8F RID: 15247 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x06003B90 RID: 15248 RVA: 0x000AFD99 File Offset: 0x000ADF99
		public bool expired
		{
			get
			{
				return this.remainTime <= 0f;
			}
		}

		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x06003B91 RID: 15249 RVA: 0x000AFDAC File Offset: 0x000ADFAC
		public float duration
		{
			get
			{
				return (float)(((double)CharacterStatusSetting.instance.stun.duration + base.attacker.stat.GetFinal(Stat.Kind.StunDuration)) * this.owner.stat.GetStatusResistacneFor(CharacterStatus.Kind.Stun)) * base.durationMultiplier;
			}
		}

		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x06003B92 RID: 15250 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x06003B93 RID: 15251 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool removeOnSwapWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x140000A6 RID: 166
		// (add) Token: 0x06003B94 RID: 15252 RVA: 0x000AFDFC File Offset: 0x000ADFFC
		// (remove) Token: 0x06003B95 RID: 15253 RVA: 0x000AFE34 File Offset: 0x000AE034
		public override event CharacterStatus.OnTimeDelegate onAttachEvents;

		// Token: 0x140000A7 RID: 167
		// (add) Token: 0x06003B96 RID: 15254 RVA: 0x000AFE6C File Offset: 0x000AE06C
		// (remove) Token: 0x06003B97 RID: 15255 RVA: 0x000AFEA4 File Offset: 0x000AE0A4
		public override event CharacterStatus.OnTimeDelegate onRefreshEvents;

		// Token: 0x140000A8 RID: 168
		// (add) Token: 0x06003B98 RID: 15256 RVA: 0x000AFEDC File Offset: 0x000AE0DC
		// (remove) Token: 0x06003B99 RID: 15257 RVA: 0x000AFF14 File Offset: 0x000AE114
		public override event CharacterStatus.OnTimeDelegate onDetachEvents;

		// Token: 0x06003B9A RID: 15258 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbilityInstance CreateInstance(Character owner)
		{
			return this;
		}

		// Token: 0x06003B9B RID: 15259 RVA: 0x000AFF49 File Offset: 0x000AE149
		public Stun(Character owner)
		{
			this.owner = owner;
		}

		// Token: 0x06003B9C RID: 15260 RVA: 0x000AFF58 File Offset: 0x000AE158
		public void UpdateTime(float deltaTime)
		{
			this.remainTime -= deltaTime;
		}

		// Token: 0x06003B9D RID: 15261 RVA: 0x000AFF68 File Offset: 0x000AE168
		public void Refresh()
		{
			this.remainTime = this.duration;
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
			base.effectHandler.HandleOnRefresh(base.attacker, this.owner);
		}

		// Token: 0x06003B9E RID: 15262 RVA: 0x000AFFD4 File Offset: 0x000AE1D4
		public void Attach()
		{
			this.remainTime = this.duration;
			if (this.owner.type == Character.Type.Boss)
			{
				this.owner.chronometer.animation.AttachTimeScale(this, 0f);
				this.owner.blockLook.Attach(this);
				if (this.owner.movement != null)
				{
					this.owner.movement.blocked.Attach(this);
				}
			}
			else
			{
				this.owner.CancelAction();
				this.owner.animationController.Stun();
			}
			this.owner.blockLook.Attach(this);
			if (this.owner.movement != null)
			{
				this.owner.movement.blocked.Attach(this);
			}
			CharacterStatus.OnTimeDelegate onTimeDelegate = this.onAttachEvents;
			if (onTimeDelegate != null)
			{
				onTimeDelegate(base.attacker, this.owner);
			}
			CharacterStatus.OnTimeDelegate onAttached = this.onAttached;
			if (onAttached != null)
			{
				onAttached(base.attacker, this.owner);
			}
			base.effectHandler.HandleOnAttach(base.attacker, this.owner);
		}

		// Token: 0x06003B9F RID: 15263 RVA: 0x000B00F8 File Offset: 0x000AE2F8
		public void Detach()
		{
			this.remainTime = 0f;
			if (this.owner.type == Character.Type.Boss)
			{
				this.owner.chronometer.animation.DetachTimeScale(this);
				this.owner.blockLook.Detach(this);
				if (this.owner.movement != null)
				{
					this.owner.movement.blocked.Detach(this);
				}
			}
			else
			{
				this.owner.animationController.StopAll();
			}
			this.owner.blockLook.Detach(this);
			if (this.owner.movement != null)
			{
				this.owner.movement.blocked.Detach(this);
			}
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
			base.effectHandler.HandleOnDetach(base.attacker, this.owner);
		}

		// Token: 0x06003BA0 RID: 15264 RVA: 0x000B0210 File Offset: 0x000AE410
		public void Initialize()
		{
			base.effectHandler = StatusEffect.GetDefaultStunEffectHandler(this.owner);
		}
	}
}
