using System;
using FX;
using GameResources;
using UnityEngine;

namespace Characters.Abilities.Statuses
{
	// Token: 0x02000B7E RID: 2942
	public class Unmoving : IAbility, IAbilityInstance
	{
		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x06003BA1 RID: 15265 RVA: 0x000B0223 File Offset: 0x000AE423
		// (set) Token: 0x06003BA2 RID: 15266 RVA: 0x000B022B File Offset: 0x000AE42B
		public Character owner { get; private set; }

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x06003BA3 RID: 15267 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x06003BA4 RID: 15268 RVA: 0x000B0234 File Offset: 0x000AE434
		// (set) Token: 0x06003BA5 RID: 15269 RVA: 0x000B023C File Offset: 0x000AE43C
		public float remainTime { get; set; }

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x06003BA6 RID: 15270 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x06003BA7 RID: 15271 RVA: 0x00018C86 File Offset: 0x00016E86
		public Sprite icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x06003BA8 RID: 15272 RVA: 0x000B0245 File Offset: 0x000AE445
		public float iconFillAmount
		{
			get
			{
				return this.remainTime / this.duration;
			}
		}

		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x06003BA9 RID: 15273 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x06003BAA RID: 15274 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x06003BAB RID: 15275 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x06003BAC RID: 15276 RVA: 0x000B0254 File Offset: 0x000AE454
		public bool expired
		{
			get
			{
				return this.remainTime <= 0f;
			}
		}

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x06003BAD RID: 15277 RVA: 0x000B0266 File Offset: 0x000AE466
		// (set) Token: 0x06003BAE RID: 15278 RVA: 0x000B026E File Offset: 0x000AE46E
		public float duration { get; set; }

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x06003BAF RID: 15279 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x06003BB0 RID: 15280 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool removeOnSwapWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003BB1 RID: 15281 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbilityInstance CreateInstance(Character owner)
		{
			return this;
		}

		// Token: 0x06003BB2 RID: 15282 RVA: 0x000B0277 File Offset: 0x000AE477
		public Unmoving(Character owner)
		{
			this.owner = owner;
			this.duration = float.MaxValue;
		}

		// Token: 0x06003BB3 RID: 15283 RVA: 0x000B0291 File Offset: 0x000AE491
		public void UpdateTime(float deltaTime)
		{
			this.remainTime -= deltaTime;
		}

		// Token: 0x06003BB4 RID: 15284 RVA: 0x000B02A1 File Offset: 0x000AE4A1
		public void Refresh()
		{
			this.remainTime = this.duration;
		}

		// Token: 0x06003BB5 RID: 15285 RVA: 0x000B02B0 File Offset: 0x000AE4B0
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
				if (this.owner.hit == null || this.owner.hit.action == null)
				{
					return;
				}
				this.owner.CancelAction();
				if (this.owner.hit.action.motions.Length >= 1)
				{
					int num = UnityEngine.Random.Range(0, this.owner.hit.action.motions.Length);
					this.owner.animationController.Unmove(this.owner.hit.action.motions[num].animationInfo);
				}
			}
			this.owner.blockLook.Attach(this);
			if (this.owner.movement != null)
			{
				this.owner.movement.blocked.Attach(this);
			}
		}

		// Token: 0x06003BB6 RID: 15286 RVA: 0x000B040C File Offset: 0x000AE60C
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
		}

		// Token: 0x06003BB7 RID: 15287 RVA: 0x000B04D3 File Offset: 0x000AE6D3
		public void Initialize()
		{
			this._effect = new EffectInfo(CommonResource.instance.bindingEffect)
			{
				attachInfo = new EffectInfo.AttachInfo(true, false, 1, EffectInfo.AttachInfo.Pivot.Center),
				loop = true,
				trackChildren = true
			};
		}

		// Token: 0x06003BB8 RID: 15288 RVA: 0x000B0507 File Offset: 0x000AE707
		private void SpawnFloatingText()
		{
			MMMaths.RandomPointWithinBounds(this.owner.collider.bounds);
		}

		// Token: 0x04002F00 RID: 12032
		private const string _floatingTextKey = "floating/status/stun";

		// Token: 0x04002F01 RID: 12033
		private const string _floatingTextColor = "#ffffff";

		// Token: 0x04002F02 RID: 12034
		private EffectInfo _effect;

		// Token: 0x04002F06 RID: 12038
		public Character attacker;
	}
}
