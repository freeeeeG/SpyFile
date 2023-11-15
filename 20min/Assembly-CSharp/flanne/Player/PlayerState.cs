using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace flanne.Player
{
	// Token: 0x02000162 RID: 354
	public abstract class PlayerState : State
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060008FF RID: 2303 RVA: 0x000257E8 File Offset: 0x000239E8
		protected PlayerInput playerInput
		{
			get
			{
				return this.owner.playerInput;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000900 RID: 2304 RVA: 0x000257F5 File Offset: 0x000239F5
		protected SpriteRenderer playerSprite
		{
			get
			{
				return this.owner.playerSprite;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000901 RID: 2305 RVA: 0x00025802 File Offset: 0x00023A02
		protected Animator playerAnimator
		{
			get
			{
				return this.owner.playerAnimator;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x0002580F File Offset: 0x00023A0F
		protected StatsHolder stats
		{
			get
			{
				return this.owner.stats;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x0002581C File Offset: 0x00023A1C
		protected Gun gun
		{
			get
			{
				return this.owner.gun;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x00025829 File Offset: 0x00023A29
		protected Ammo ammo
		{
			get
			{
				return this.owner.ammo;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000905 RID: 2309 RVA: 0x00025836 File Offset: 0x00023A36
		protected Slider reloadBar
		{
			get
			{
				return this.owner.reloadBar;
			}
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x00025843 File Offset: 0x00023A43
		private void Awake()
		{
			this.owner = base.GetComponentInParent<PlayerController>();
		}

		// Token: 0x040006A6 RID: 1702
		protected PlayerController owner;
	}
}
