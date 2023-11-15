using System;
using System.Collections.Generic;
using Characters.Abilities;
using Characters.Abilities.Constraints;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006AE RID: 1710
	public class CharacterAbilityManager : MonoBehaviour
	{
		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x0600224F RID: 8783 RVA: 0x00066E25 File Offset: 0x00065025
		public int Count
		{
			get
			{
				return this._abilities.Count;
			}
		}

		// Token: 0x17000722 RID: 1826
		public IAbilityInstance this[int index]
		{
			get
			{
				return this._abilities[index];
			}
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x00066E40 File Offset: 0x00065040
		public static CharacterAbilityManager AddComponent(Character character)
		{
			CharacterAbilityManager characterAbilityManager = character.gameObject.AddComponent<CharacterAbilityManager>();
			characterAbilityManager._character = character;
			characterAbilityManager.Initialize();
			return characterAbilityManager;
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x00066E5C File Offset: 0x0006505C
		public IAbilityInstance Add(IAbility ability)
		{
			CharacterAbilityManager.<>c__DisplayClass8_0 CS$<>8__locals1 = new CharacterAbilityManager.<>c__DisplayClass8_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.ability = ability;
			IAbilityInstance abilityInstance = this.GetInstance(CS$<>8__locals1.ability);
			if (abilityInstance != null)
			{
				abilityInstance.Refresh();
				return abilityInstance;
			}
			abilityInstance = CS$<>8__locals1.ability.CreateInstance(this._character);
			if (CS$<>8__locals1.ability.removeOnSwapWeapon && this._character.playerComponents.inventory.weapon != null)
			{
				this._character.playerComponents.inventory.weapon.onSwap += CS$<>8__locals1.<Add>g__RemoveAbilityOnSwap|0;
			}
			if (this._character.liveAndActive)
			{
				abilityInstance.Attach();
				this._abilities.Add(CS$<>8__locals1.ability.iconPriority, abilityInstance);
			}
			return abilityInstance;
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x00066F24 File Offset: 0x00065124
		public bool Remove(IAbility ability)
		{
			for (int i = this._abilities.Count - 1; i >= 0; i--)
			{
				if (this._abilities[i].ability.Equals(ability))
				{
					this._abilities[i].Detach();
					this._abilities.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x00066F84 File Offset: 0x00065184
		public bool Remove(AbilityInstance abilityInstance)
		{
			for (int i = this._abilities.Count - 1; i >= 0; i--)
			{
				if (this._abilities[i].Equals(abilityInstance))
				{
					this._abilities[i].Detach();
					this._abilities.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x00066FE0 File Offset: 0x000651E0
		public void Clear()
		{
			for (int i = this._abilities.Count - 1; i >= 0; i--)
			{
				this._abilities[i].Detach();
			}
			this._abilities.Clear();
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x00067024 File Offset: 0x00065224
		public IAbilityInstance GetInstance(IAbility ability)
		{
			for (int i = 0; i < this._abilities.Count; i++)
			{
				if (this._abilities[i].ability.Equals(ability))
				{
					return this._abilities[i];
				}
			}
			return null;
		}

		// Token: 0x06002257 RID: 8791 RVA: 0x00067070 File Offset: 0x00065270
		public IAbilityInstance GetInstanceType(IAbility ability)
		{
			Type type = ability.GetType();
			for (int i = 0; i < this._abilities.Count; i++)
			{
				if (this._abilities[i].ability.GetType() == type)
				{
					return this._abilities[i];
				}
			}
			return null;
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x000670C8 File Offset: 0x000652C8
		public IAbilityInstance GetInstance<T>() where T : IAbility
		{
			for (int i = 0; i < this._abilities.Count; i++)
			{
				if (this._abilities[i].ability is T)
				{
					return this._abilities[i];
				}
			}
			return null;
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x00067114 File Offset: 0x00065314
		public T GetInstanceByInstanceType<T>() where T : IAbilityInstance
		{
			for (int i = 0; i < this._abilities.Count; i++)
			{
				IAbilityInstance abilityInstance = this._abilities[i];
				if (abilityInstance is T)
				{
					return (T)((object)abilityInstance);
				}
			}
			return default(T);
		}

		// Token: 0x0600225A RID: 8794 RVA: 0x00067160 File Offset: 0x00065360
		public List<T> GetInstancesByInstanceType<T>() where T : IAbilityInstance
		{
			List<T> list = new List<T>();
			for (int i = 0; i < this._abilities.Count; i++)
			{
				IAbilityInstance abilityInstance = this._abilities[i];
				if (abilityInstance is T)
				{
					T item = (T)((object)abilityInstance);
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x0600225B RID: 8795 RVA: 0x000671B0 File Offset: 0x000653B0
		public bool Contains(IAbility ability)
		{
			for (int i = 0; i < this._abilities.Count; i++)
			{
				if (this._abilities[i].ability.Equals(ability))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600225C RID: 8796 RVA: 0x000671EF File Offset: 0x000653EF
		private void Initialize()
		{
			if (this._character.health != null)
			{
				this._character.health.onDied += this.OnDied;
			}
		}

		// Token: 0x0600225D RID: 8797 RVA: 0x00067220 File Offset: 0x00065420
		private void OnDied()
		{
			this.Clear();
			if (this._character.health != null)
			{
				this._character.health.onDied -= this.OnDied;
			}
		}

		// Token: 0x0600225E RID: 8798 RVA: 0x00067258 File Offset: 0x00065458
		private void Update()
		{
			if (this._character.playerComponents != null && !this._constraints.Pass())
			{
				return;
			}
			try
			{
				float deltaTime = this._character.chronometer.master.deltaTime;
				int num = this._abilities.Count - 1;
				while (num >= 0 && num < this._abilities.Count)
				{
					IAbilityInstance abilityInstance = this._abilities[num];
					abilityInstance.UpdateTime(deltaTime);
					if (abilityInstance.expired)
					{
						if (this._abilities[num] != abilityInstance)
						{
							break;
						}
						this._abilities.RemoveAt(num);
						abilityInstance.Detach();
					}
					num--;
				}
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}

		// Token: 0x04001D2E RID: 7470
		[GetComponent]
		[SerializeField]
		private Character _character;

		// Token: 0x04001D2F RID: 7471
		private Constraint[] _constraints = new Constraint[]
		{
			new LetterBox(),
			new Dialogue(),
			new Story(),
			new EndingCredit()
		};

		// Token: 0x04001D30 RID: 7472
		private readonly PriorityList<IAbilityInstance> _abilities = new PriorityList<IAbilityInstance>();
	}
}
