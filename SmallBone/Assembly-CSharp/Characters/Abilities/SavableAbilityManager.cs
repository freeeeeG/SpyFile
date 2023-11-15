using System;
using Characters.Abilities.Savable;
using Data;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000AB3 RID: 2739
	public sealed class SavableAbilityManager : MonoBehaviour
	{
		// Token: 0x06003864 RID: 14436 RVA: 0x000A6474 File Offset: 0x000A4674
		private void Awake()
		{
			this._abilities = new EnumArray<SavableAbilityManager.Name, ISavableAbility>();
			this.LoadAll();
		}

		// Token: 0x06003865 RID: 14437 RVA: 0x000A6487 File Offset: 0x000A4687
		public float GetStack(SavableAbilityManager.Name name)
		{
			return this._abilities[name].stack;
		}

		// Token: 0x06003866 RID: 14438 RVA: 0x000A649A File Offset: 0x000A469A
		public void Remove(SavableAbilityManager.Name name)
		{
			this._character.ability.Remove(this._abilities[name].ability);
		}

		// Token: 0x06003867 RID: 14439 RVA: 0x000A64BE File Offset: 0x000A46BE
		public void Apply(SavableAbilityManager.Name name)
		{
			this._character.ability.Add(this._abilities[name].ability);
		}

		// Token: 0x06003868 RID: 14440 RVA: 0x000A64E2 File Offset: 0x000A46E2
		public void Apply(SavableAbilityManager.Name name, int stack)
		{
			this._abilities[name].stack = (float)stack;
			this._character.ability.Add(this._abilities[name].ability);
		}

		// Token: 0x06003869 RID: 14441 RVA: 0x000A6519 File Offset: 0x000A4719
		public void Apply(SavableAbilityManager.Name name, float stack)
		{
			this._abilities[name].stack = stack;
			this._character.ability.Add(this._abilities[name].ability);
		}

		// Token: 0x0600386A RID: 14442 RVA: 0x000A6550 File Offset: 0x000A4750
		public void Apply(SavableAbilityManager.Name name, float stack, float remainTime)
		{
			this._abilities[name].stack = stack;
			this._abilities[name].remainTime = remainTime;
			this._character.ability.Add(this._abilities[name].ability);
		}

		// Token: 0x0600386B RID: 14443 RVA: 0x000A65A3 File Offset: 0x000A47A3
		public void IncreaseStack(SavableAbilityManager.Name name, float plus)
		{
			this._abilities[name].stack += plus;
			this.Apply(name);
		}

		// Token: 0x0600386C RID: 14444 RVA: 0x000A65C8 File Offset: 0x000A47C8
		public void SaveAll()
		{
			foreach (object obj in Enum.GetValues(typeof(SavableAbilityManager.Name)))
			{
				SavableAbilityManager.Name name = (SavableAbilityManager.Name)obj;
				if (this._abilities[name] != null)
				{
					GameData.Buff buff = GameData.Buff.Get(name);
					buff.remainTime = this._abilities[name].remainTime;
					buff.stack = this._abilities[name].stack;
					buff.attached = this._character.ability.Contains(this._abilities[name].ability);
					buff.Save();
				}
			}
		}

		// Token: 0x0600386D RID: 14445 RVA: 0x000A6694 File Offset: 0x000A4894
		public void ResetAll()
		{
			foreach (ISavableAbility savableAbility in this._abilities)
			{
				this._character.ability.Remove(savableAbility.ability);
			}
			GameData.Buff.ResetAll();
		}

		// Token: 0x0600386E RID: 14446 RVA: 0x000A66F8 File Offset: 0x000A48F8
		public void LoadAll()
		{
			this.Load(new Curse(), SavableAbilityManager.Name.Curse);
			this.Load(new FogWolfBuff(), SavableAbilityManager.Name.FogWolfBuff);
			this.Load(new HealthAuxiliaryDamage(), SavableAbilityManager.Name.HealthAuxiliaryDamage);
			this.Load(new HealthAuxiliaryHealth(), SavableAbilityManager.Name.HealthAuxiliaryHealth);
			this.Load(new HealthAuxiliarySpeed(), SavableAbilityManager.Name.HealthAuxiliarySpeed);
			this.Load(new PurchasedMaxHealth(), SavableAbilityManager.Name.PurchasedMaxHealth);
			this.Load(new PurchasedShield(), SavableAbilityManager.Name.PurchasedShield);
			this.Load(new BrutalityBuff(), SavableAbilityManager.Name.BrutalityBuff);
			this.Load(new RageBuff(), SavableAbilityManager.Name.RageBuff);
			this.Load(new FortitudeBuff(), SavableAbilityManager.Name.FortitudeBuff);
			this.Load(new EssenceSpirit(), SavableAbilityManager.Name.EssenceSpirit);
			this.Load(new LifeChange(), SavableAbilityManager.Name.LifeChange);
			this.Load(new PurchasedShield(), SavableAbilityManager.Name.OrganicIcedTea);
		}

		// Token: 0x0600386F RID: 14447 RVA: 0x000A67A8 File Offset: 0x000A49A8
		private void Load(ISavableAbility savableAbility, SavableAbilityManager.Name name)
		{
			GameData.Buff buff = GameData.Buff.Get(name);
			if (buff.attached)
			{
				savableAbility.remainTime = buff.remainTime;
				savableAbility.stack = buff.stack;
				this._character.ability.Add(savableAbility.ability);
			}
			this._abilities[name] = savableAbility;
		}

		// Token: 0x04002CE4 RID: 11492
		[GetComponent]
		[SerializeField]
		private Character _character;

		// Token: 0x04002CE5 RID: 11493
		private EnumArray<SavableAbilityManager.Name, ISavableAbility> _abilities;

		// Token: 0x02000AB4 RID: 2740
		public enum Name
		{
			// Token: 0x04002CE7 RID: 11495
			Curse,
			// Token: 0x04002CE8 RID: 11496
			FogWolfBuff,
			// Token: 0x04002CE9 RID: 11497
			HealthAuxiliaryDamage,
			// Token: 0x04002CEA RID: 11498
			HealthAuxiliaryHealth,
			// Token: 0x04002CEB RID: 11499
			HealthAuxiliarySpeed,
			// Token: 0x04002CEC RID: 11500
			PurchasedMaxHealth,
			// Token: 0x04002CED RID: 11501
			PurchasedShield,
			// Token: 0x04002CEE RID: 11502
			OrganicIcedTea,
			// Token: 0x04002CEF RID: 11503
			BrutalityBuff,
			// Token: 0x04002CF0 RID: 11504
			RageBuff,
			// Token: 0x04002CF1 RID: 11505
			FortitudeBuff,
			// Token: 0x04002CF2 RID: 11506
			EssenceSpirit,
			// Token: 0x04002CF3 RID: 11507
			LifeChange
		}
	}
}
