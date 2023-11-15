using System;
using System.Runtime.CompilerServices;
using Characters.Abilities;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FD0 RID: 4048
	public class AttachSilence : TargetedCharacterOperation
	{
		// Token: 0x06004E56 RID: 20054 RVA: 0x000EAA15 File Offset: 0x000E8C15
		public override void Initialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x06004E57 RID: 20055 RVA: 0x000EAA24 File Offset: 0x000E8C24
		public override void Run(Character owner, Character target)
		{
			if (target == null || !target.liveAndActive)
			{
				return;
			}
			if (this._cache != null && this._cache.attached)
			{
				this.RunOnCached();
				return;
			}
			this._cache = target.ability.GetInstance<GetSilence>();
			if (this._cache != null)
			{
				this._cache.Refresh();
				return;
			}
			target.ability.Add(this._abilityComponent.ability);
		}

		// Token: 0x06004E58 RID: 20056 RVA: 0x000EAA9B File Offset: 0x000E8C9B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void RunOnCached()
		{
			this._cache.Refresh();
		}

		// Token: 0x06004E59 RID: 20057 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04003E62 RID: 15970
		[UnityEditor.Subcomponent(typeof(GetSilenceComponent))]
		[SerializeField]
		private GetSilenceComponent _abilityComponent;

		// Token: 0x04003E63 RID: 15971
		private IAbilityInstance _cache;
	}
}
