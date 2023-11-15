using System;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D28 RID: 3368
	[Serializable]
	public class AwakenDarkPaladinPassive : Ability, IAbilityInstance
	{
		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x060043DF RID: 17375 RVA: 0x000C567E File Offset: 0x000C387E
		// (set) Token: 0x060043E0 RID: 17376 RVA: 0x000C5686 File Offset: 0x000C3886
		public Character owner { get; set; }

		// Token: 0x17000E17 RID: 3607
		// (get) Token: 0x060043E1 RID: 17377 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x060043E2 RID: 17378 RVA: 0x000C568F File Offset: 0x000C388F
		// (set) Token: 0x060043E3 RID: 17379 RVA: 0x000C5697 File Offset: 0x000C3897
		public float remainTime { get; set; }

		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x060043E4 RID: 17380 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E1A RID: 3610
		// (get) Token: 0x060043E5 RID: 17381 RVA: 0x0009ADBE File Offset: 0x00098FBE
		public Sprite icon
		{
			get
			{
				return this._defaultIcon;
			}
		}

		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x060043E6 RID: 17382 RVA: 0x000C56A0 File Offset: 0x000C38A0
		public float iconFillAmount
		{
			get
			{
				return 1f - this.remainTime / base.duration;
			}
		}

		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x060043E7 RID: 17383 RVA: 0x000C56B5 File Offset: 0x000C38B5
		public bool expired
		{
			get
			{
				return this.remainTime <= 0f;
			}
		}

		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x060043E8 RID: 17384 RVA: 0x00099F2B File Offset: 0x0009812B
		public int iconStacks
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060043E9 RID: 17385 RVA: 0x000C56C7 File Offset: 0x000C38C7
		public void Refresh()
		{
			this.remainTime = base.duration;
		}

		// Token: 0x060043EA RID: 17386 RVA: 0x000C56D5 File Offset: 0x000C38D5
		private void OnShieldBroke()
		{
			this.owner.ability.Remove(this);
		}

		// Token: 0x060043EC RID: 17388 RVA: 0x000C56E9 File Offset: 0x000C38E9
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this.owner = owner;
			return this;
		}

		// Token: 0x060043ED RID: 17389 RVA: 0x000C56F3 File Offset: 0x000C38F3
		public void UpdateTime(float deltaTime)
		{
			this.remainTime -= deltaTime;
		}

		// Token: 0x060043EE RID: 17390 RVA: 0x000C56C7 File Offset: 0x000C38C7
		public void Attach()
		{
			this.remainTime = base.duration;
		}

		// Token: 0x060043EF RID: 17391 RVA: 0x00002191 File Offset: 0x00000391
		public void Detach()
		{
		}
	}
}
