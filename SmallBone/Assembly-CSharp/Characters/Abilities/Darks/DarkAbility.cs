using System;
using GameResources;
using UnityEngine;

namespace Characters.Abilities.Darks
{
	// Token: 0x02000BC9 RID: 3017
	[Serializable]
	public sealed class DarkAbility : MonoBehaviour
	{
		// Token: 0x17000D2E RID: 3374
		// (get) Token: 0x06003E16 RID: 15894 RVA: 0x000B486D File Offset: 0x000B2A6D
		public string displayName
		{
			get
			{
				return Localization.GetLocalizedString("darkEnemy/ability/" + base.gameObject.name + "/name");
			}
		}

		// Token: 0x06003E17 RID: 15895 RVA: 0x000B4890 File Offset: 0x000B2A90
		public void Initialize()
		{
			AbilityComponent[] abilityComponents = this._abilityComponents;
			for (int i = 0; i < abilityComponents.Length; i++)
			{
				abilityComponents[i].Initialize();
			}
		}

		// Token: 0x06003E18 RID: 15896 RVA: 0x000B48BC File Offset: 0x000B2ABC
		public bool Available(Character owner)
		{
			foreach (Key key in this._exceptTarget)
			{
				if (key.Equals(owner.key))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003E19 RID: 15897 RVA: 0x000B4900 File Offset: 0x000B2B00
		public void AttachTo(Character owner, DarkAbilityAttacher attacher)
		{
			foreach (AbilityComponent abilityComponent in this._abilityComponents)
			{
				owner.ability.Add(abilityComponent.ability);
			}
			attacher.gauge = this._gauge;
		}

		// Token: 0x06003E1A RID: 15898 RVA: 0x000B4944 File Offset: 0x000B2B44
		public void RemoveFrom(Character owner)
		{
			foreach (AbilityComponent abilityComponent in this._abilityComponents)
			{
				owner.ability.Remove(abilityComponent.ability);
			}
		}

		// Token: 0x04002FFA RID: 12282
		[SerializeField]
		private DarkAbilityGauge _gauge;

		// Token: 0x04002FFB RID: 12283
		[SerializeField]
		private Key[] _exceptTarget;

		// Token: 0x04002FFC RID: 12284
		[SerializeField]
		private AbilityComponent[] _abilityComponents;
	}
}
