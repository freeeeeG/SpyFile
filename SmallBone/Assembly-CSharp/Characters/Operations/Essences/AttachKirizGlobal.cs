using System;
using Characters.Abilities.Essences;
using Level;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Essences
{
	// Token: 0x02000EB0 RID: 3760
	public class AttachKirizGlobal : CharacterOperation
	{
		// Token: 0x060049FA RID: 18938 RVA: 0x000D7E94 File Offset: 0x000D6094
		public override void Initialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x060049FB RID: 18939 RVA: 0x000D7EA4 File Offset: 0x000D60A4
		public override void Run(Character target)
		{
			this._abilityComponent.SetAttacker(target);
			foreach (Character character in Map.Instance.waveContainer)
			{
				if (!(character.ability == null))
				{
					character.ability.Add(this._abilityComponent.ability);
				}
			}
		}

		// Token: 0x060049FC RID: 18940 RVA: 0x000D7F20 File Offset: 0x000D6120
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

		// Token: 0x060049FD RID: 18941 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04003932 RID: 14642
		[UnityEditor.Subcomponent(typeof(KirizComponent))]
		[SerializeField]
		private KirizComponent _abilityComponent;
	}
}
