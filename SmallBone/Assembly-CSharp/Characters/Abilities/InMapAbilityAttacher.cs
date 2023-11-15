using System;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009F4 RID: 2548
	public class InMapAbilityAttacher : AbilityAttacher
	{
		// Token: 0x0600362C RID: 13868 RVA: 0x000A0A94 File Offset: 0x0009EC94
		public override void OnIntialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x0600362D RID: 13869 RVA: 0x000A0AA1 File Offset: 0x0009ECA1
		public override void StartAttach()
		{
			Singleton<Service>.Instance.levelManager.onMapLoaded += this.ResetAbility;
		}

		// Token: 0x0600362E RID: 13870 RVA: 0x000A0AC0 File Offset: 0x0009ECC0
		public override void StopAttach()
		{
			if (Service.quitting)
			{
				return;
			}
			if (base.owner == null)
			{
				return;
			}
			Singleton<Service>.Instance.levelManager.onMapLoaded -= this.ResetAbility;
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x000A0B1C File Offset: 0x0009ED1C
		private void ResetAbility()
		{
			Map.Type type = Map.Instance.type;
			if (this._exceptTypes != null)
			{
				Map.Type[] exceptTypes = this._exceptTypes;
				for (int i = 0; i < exceptTypes.Length; i++)
				{
					if (exceptTypes[i] == type)
					{
						return;
					}
				}
			}
			InMapAbilityAttacher.AttachType attachType = this._attachType;
			if (attachType == InMapAbilityAttacher.AttachType.Reset)
			{
				base.owner.ability.Remove(this._abilityComponent.ability);
				base.owner.ability.Add(this._abilityComponent.ability);
				return;
			}
			if (attachType != InMapAbilityAttacher.AttachType.Refresh)
			{
				return;
			}
			base.owner.ability.Add(this._abilityComponent.ability);
		}

		// Token: 0x06003630 RID: 13872 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04002B7C RID: 11132
		[SerializeField]
		private InMapAbilityAttacher.AttachType _attachType;

		// Token: 0x04002B7D RID: 11133
		[SerializeField]
		private Map.Type[] _exceptTypes;

		// Token: 0x04002B7E RID: 11134
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		private AbilityComponent _abilityComponent;

		// Token: 0x020009F5 RID: 2549
		private enum AttachType
		{
			// Token: 0x04002B80 RID: 11136
			Reset,
			// Token: 0x04002B81 RID: 11137
			Refresh
		}
	}
}
