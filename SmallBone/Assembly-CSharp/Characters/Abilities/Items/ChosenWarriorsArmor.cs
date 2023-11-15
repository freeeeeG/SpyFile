using System;
using System.Collections.Generic;
using FX;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CA9 RID: 3241
	[Serializable]
	public sealed class ChosenWarriorsArmor : Ability
	{
		// Token: 0x17000DCD RID: 3533
		// (get) Token: 0x060041DD RID: 16861 RVA: 0x000BFA3D File Offset: 0x000BDC3D
		// (set) Token: 0x060041DE RID: 16862 RVA: 0x000BFA71 File Offset: 0x000BDC71
		public float amount
		{
			get
			{
				if (this._instance == null)
				{
					return 0f;
				}
				if (this._instance._shieldInstance == null)
				{
					return 0f;
				}
				return (float)this._instance._shieldInstance.amount;
			}
			set
			{
				if (this._instance == null)
				{
					return;
				}
				this._instance.LoadShield(value);
			}
		}

		// Token: 0x060041DF RID: 16863 RVA: 0x000BFA88 File Offset: 0x000BDC88
		public void Load(Character owner, int stack)
		{
			owner.ability.Add(this);
			this.amount = (float)stack;
		}

		// Token: 0x060041E0 RID: 16864 RVA: 0x000BFA9F File Offset: 0x000BDC9F
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this._instance = new ChosenWarriorsArmor.Instance(owner, this);
			return this._instance;
		}

		// Token: 0x04003277 RID: 12919
		[SerializeField]
		private float _maxAmount;

		// Token: 0x04003278 RID: 12920
		[SerializeField]
		private float _normalMapAmount;

		// Token: 0x04003279 RID: 12921
		[SerializeField]
		private float _bossMapAmount;

		// Token: 0x0400327A RID: 12922
		[SerializeField]
		private EffectInfo _loopEffectInfo = new EffectInfo
		{
			subordinated = true
		};

		// Token: 0x0400327B RID: 12923
		private ChosenWarriorsArmor.Instance _instance;

		// Token: 0x02000CAA RID: 3242
		public class Instance : AbilityInstance<ChosenWarriorsArmor>
		{
			// Token: 0x060041E2 RID: 16866 RVA: 0x000BFACE File Offset: 0x000BDCCE
			public Instance(Character owner, ChosenWarriorsArmor ability) : base(owner, ability)
			{
			}

			// Token: 0x060041E3 RID: 16867 RVA: 0x000BFAD8 File Offset: 0x000BDCD8
			protected override void OnAttach()
			{
				Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn += this.HandleOnMapChangedAndFadedIn;
			}

			// Token: 0x060041E4 RID: 16868 RVA: 0x000BFAF8 File Offset: 0x000BDCF8
			private void OnBreak()
			{
				if (this._shieldInstance != null)
				{
					this.owner.health.shield.Remove(this.ability);
					this._shieldInstance = null;
				}
				if (this._loopEffectInstance != null)
				{
					this._loopEffectInstance.Stop();
					this._loopEffectInstance = null;
				}
			}

			// Token: 0x060041E5 RID: 16869 RVA: 0x000BFB50 File Offset: 0x000BDD50
			private void HandleOnMapChangedAndFadedIn(Map old, Map @new)
			{
				if (this.IsBossMap())
				{
					if (this._shieldInstance == null)
					{
						EffectInfo loopEffectInfo = this.ability._loopEffectInfo;
						this._loopEffectInstance = ((loopEffectInfo != null) ? loopEffectInfo.Spawn(this.owner.transform.position, this.owner, 0f, 1f) : null);
						this._shieldInstance = this.owner.health.shield.Add(this.ability, this.ability._bossMapAmount, new Action(this.OnBreak));
						return;
					}
					this._shieldInstance.amount = (double)Mathf.Min(this.ability._maxAmount, (float)this._shieldInstance.amount + this.ability._bossMapAmount);
					return;
				}
				else
				{
					if (this._shieldInstance == null)
					{
						EffectInfo loopEffectInfo2 = this.ability._loopEffectInfo;
						this._loopEffectInstance = ((loopEffectInfo2 != null) ? loopEffectInfo2.Spawn(this.owner.transform.position, this.owner, 0f, 1f) : null);
						this._shieldInstance = this.owner.health.shield.Add(this.ability, this.ability._normalMapAmount, new Action(this.OnBreak));
						return;
					}
					this._shieldInstance.amount = (double)Mathf.Min(this.ability._maxAmount, (float)this._shieldInstance.amount + this.ability._normalMapAmount);
					return;
				}
			}

			// Token: 0x060041E6 RID: 16870 RVA: 0x000BFCD0 File Offset: 0x000BDED0
			private bool IsBossMap()
			{
				Map instance = Map.Instance;
				if (instance.type == Map.Type.Manual)
				{
					List<Character> allEnemies = instance.waveContainer.GetAllEnemies();
					for (int i = 0; i < allEnemies.Count; i++)
					{
						if (allEnemies[i].type == Character.Type.Boss)
						{
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x060041E7 RID: 16871 RVA: 0x000BFD1C File Offset: 0x000BDF1C
			protected override void OnDetach()
			{
				Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn -= this.HandleOnMapChangedAndFadedIn;
				this.owner.health.shield.Remove(this.ability);
				if (this._shieldInstance != null)
				{
					this.owner.health.shield.Remove(this.ability);
					this._shieldInstance = null;
				}
				if (this._loopEffectInstance != null)
				{
					this._loopEffectInstance.Stop();
					this._loopEffectInstance = null;
				}
			}

			// Token: 0x060041E8 RID: 16872 RVA: 0x000BFDAC File Offset: 0x000BDFAC
			internal void LoadShield(float value)
			{
				if (this._shieldInstance == null)
				{
					this._shieldInstance = this.owner.health.shield.Add(this.ability, value, new Action(this.OnBreak));
					return;
				}
				this._shieldInstance.amount = (double)Mathf.Min(this.ability._maxAmount, value);
			}

			// Token: 0x0400327C RID: 12924
			internal Shield.Instance _shieldInstance;

			// Token: 0x0400327D RID: 12925
			private EffectPoolInstance _loopEffectInstance;
		}
	}
}
