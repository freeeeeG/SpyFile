using System;
using Characters.Gear.Weapons.Gauges;
using FX;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D71 RID: 3441
	[Serializable]
	public class MummyPassive : Ability, IAbilityInstance
	{
		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x0600455A RID: 17754 RVA: 0x000C9312 File Offset: 0x000C7512
		// (set) Token: 0x0600455B RID: 17755 RVA: 0x000C931A File Offset: 0x000C751A
		public Character owner { get; set; }

		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x0600455C RID: 17756 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x0600455D RID: 17757 RVA: 0x000C9323 File Offset: 0x000C7523
		// (set) Token: 0x0600455E RID: 17758 RVA: 0x000C932B File Offset: 0x000C752B
		public float remainTime { get; set; }

		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x0600455F RID: 17759 RVA: 0x000C9334 File Offset: 0x000C7534
		// (set) Token: 0x06004560 RID: 17760 RVA: 0x000C933C File Offset: 0x000C753C
		public bool attached { get; private set; }

		// Token: 0x17000E6F RID: 3695
		// (get) Token: 0x06004561 RID: 17761 RVA: 0x00018C86 File Offset: 0x00016E86
		public Sprite icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x06004562 RID: 17762 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000E71 RID: 3697
		// (get) Token: 0x06004563 RID: 17763 RVA: 0x00071719 File Offset: 0x0006F919
		public float iconFillAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000E72 RID: 3698
		// (get) Token: 0x06004564 RID: 17764 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004565 RID: 17765 RVA: 0x000C9345 File Offset: 0x000C7545
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this.owner = owner;
			return this;
		}

		// Token: 0x06004566 RID: 17766 RVA: 0x000C9350 File Offset: 0x000C7550
		private MummyPassive.GunMap GetGunMap(string key)
		{
			foreach (MummyPassive.GunMap gunMap in this._gunMaps)
			{
				if (gunMap.key.Equals(key, StringComparison.OrdinalIgnoreCase))
				{
					return gunMap;
				}
			}
			return null;
		}

		// Token: 0x06004567 RID: 17767 RVA: 0x000C9388 File Offset: 0x000C7588
		public void PickUpWeapon(string key)
		{
			MummyPassive.GunMap gunMap = this.GetGunMap(key);
			if (gunMap == null)
			{
				Debug.LogError("There is no Mummy gun for key " + key);
				return;
			}
			this._gauge.maxValue = (float)gunMap.ammo;
			this._gauge.Set((float)gunMap.ammo);
			this._gauge.defaultBarColor = gunMap.gaugeColor;
			if (this._current == gunMap)
			{
				return;
			}
			if (!this.attached)
			{
				this._current = gunMap;
				this._current.polymorphBody.character = this.owner;
				return;
			}
			if (this._current != null)
			{
				this._current.losingEffect.Spawn(this.owner.transform.position, this.owner, 0f, 1f);
				this._current.polymorphBody.EndPolymorph();
			}
			this._current = gunMap;
			this._current.polymorphBody.character = this.owner;
			this._current.polymorphBody.StartPolymorph();
		}

		// Token: 0x06004568 RID: 17768 RVA: 0x00002191 File Offset: 0x00000391
		public void UpdateTime(float deltaTime)
		{
		}

		// Token: 0x06004569 RID: 17769 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x0600456A RID: 17770 RVA: 0x000C948C File Offset: 0x000C768C
		public void Attach()
		{
			this.attached = true;
			this._gauge.onChanged += this.OnGaugeChanged;
			if (this._current == null)
			{
				return;
			}
			this._current.polymorphBody.StartPolymorph();
		}

		// Token: 0x0600456B RID: 17771 RVA: 0x000C94C5 File Offset: 0x000C76C5
		public void Detach()
		{
			this.attached = false;
			this._gauge.onChanged -= this.OnGaugeChanged;
			if (this._current == null)
			{
				return;
			}
			this._current.polymorphBody.EndPolymorph();
		}

		// Token: 0x0600456C RID: 17772 RVA: 0x000C9500 File Offset: 0x000C7700
		private void OnGaugeChanged(float oldValue, float newValue)
		{
			if (newValue > 0f)
			{
				return;
			}
			if (oldValue == newValue)
			{
				return;
			}
			if (this._current == null)
			{
				return;
			}
			this._current.losingEffect.Spawn(this.owner.transform.position, this.owner, 0f, 1f);
			this._current.polymorphBody.EndPolymorph();
			this._current = null;
		}

		// Token: 0x040034BF RID: 13503
		[SerializeField]
		private ValueGauge _gauge;

		// Token: 0x040034C0 RID: 13504
		[SerializeField]
		[Space]
		private MummyPassive.GunMap[] _gunMaps;

		// Token: 0x040034C1 RID: 13505
		private MummyPassive.GunMap _current;

		// Token: 0x02000D72 RID: 3442
		[Serializable]
		private class GunMap
		{
			// Token: 0x040034C5 RID: 13509
			public string key;

			// Token: 0x040034C6 RID: 13510
			public int ammo;

			// Token: 0x040034C7 RID: 13511
			public Color gaugeColor;

			// Token: 0x040034C8 RID: 13512
			public EffectInfo losingEffect;

			// Token: 0x040034C9 RID: 13513
			public PolymorphBody polymorphBody;
		}
	}
}
