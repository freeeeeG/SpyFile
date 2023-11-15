using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x0200099B RID: 2459
	public abstract class AbilityComponent<T> : AbilityComponent where T : Ability
	{
		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x060034D8 RID: 13528 RVA: 0x0009C84D File Offset: 0x0009AA4D
		public override IAbility ability
		{
			get
			{
				return this._ability;
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x060034D9 RID: 13529 RVA: 0x0009C85A File Offset: 0x0009AA5A
		public T baseAbility
		{
			get
			{
				return this._ability;
			}
		}

		// Token: 0x060034DA RID: 13530 RVA: 0x0009C862 File Offset: 0x0009AA62
		public override void Initialize()
		{
			this._ability.Initialize();
		}

		// Token: 0x060034DB RID: 13531 RVA: 0x0009C874 File Offset: 0x0009AA74
		private void OnDestroy()
		{
			this._ability = default(T);
		}

		// Token: 0x060034DC RID: 13532 RVA: 0x0009C882 File Offset: 0x0009AA82
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return this._ability.CreateInstance(owner);
		}

		// Token: 0x04002A89 RID: 10889
		[SerializeField]
		protected T _ability;
	}
}
