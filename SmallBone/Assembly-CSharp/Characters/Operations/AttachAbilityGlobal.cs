using System;
using Characters.Abilities;
using Level;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DAB RID: 3499
	public class AttachAbilityGlobal : CharacterOperation
	{
		// Token: 0x06004672 RID: 18034 RVA: 0x000CB74B File Offset: 0x000C994B
		public override void Initialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x06004673 RID: 18035 RVA: 0x000CB758 File Offset: 0x000C9958
		public override void Run(Character target)
		{
			foreach (Character character in Map.Instance.waveContainer)
			{
				if (!(character.ability == null))
				{
					character.ability.Add(this._abilityComponent.ability);
				}
			}
		}

		// Token: 0x06004674 RID: 18036 RVA: 0x000CB7C8 File Offset: 0x000C99C8
		public override void Stop()
		{
			foreach (Character character in Map.Instance.waveContainer)
			{
				if (!(character.ability == null))
				{
					character.ability.Remove(this._abilityComponent.ability);
				}
			}
		}

		// Token: 0x06004675 RID: 18037 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04003555 RID: 13653
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _abilityComponent;
	}
}
