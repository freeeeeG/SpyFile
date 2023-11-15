using System;
using UnityEngine;

namespace Characters.Abilities.Darks
{
	// Token: 0x02000BCC RID: 3020
	[Serializable]
	public sealed class DarkAbilityConstructor
	{
		// Token: 0x06003E2C RID: 15916 RVA: 0x000B4B7C File Offset: 0x000B2D7C
		public bool Provide(Character target)
		{
			if (target == null)
			{
				Debug.LogError("target is null");
				return false;
			}
			DarkAbilityAttacher darkAbilityAttacher = UnityEngine.Object.Instantiate<DarkAbilityAttacher>(this._attacher, target.attach.transform);
			darkAbilityAttacher.Initialize(target);
			DarkEnemy component = target.GetComponent<DarkEnemy>();
			if (component != null)
			{
				component.Initialize(darkAbilityAttacher);
			}
			target.type = Character.Type.Named;
			return true;
		}

		// Token: 0x04003007 RID: 12295
		[SerializeField]
		private DarkAbilityAttacher _attacher;
	}
}
