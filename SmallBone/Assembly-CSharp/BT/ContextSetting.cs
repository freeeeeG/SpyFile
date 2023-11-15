using System;
using BT.SharedValues;
using Characters;
using Services;
using Singletons;
using UnityEngine;

namespace BT
{
	// Token: 0x02001406 RID: 5126
	[Serializable]
	public class ContextSetting
	{
		// Token: 0x060064EA RID: 25834 RVA: 0x001245C4 File Offset: 0x001227C4
		public void ApplyTo(Context context)
		{
			context.Set<Transform>(Key.OwnerTransform, new SharedValue<Transform>(this._ownerTransform));
			context.Set<Character>(Key.OwnerCharacter, new SharedValue<Character>(this._ownerCharacter));
			if (this._playerIsTarget)
			{
				context.Set<Character>(Key.Target, new SharedValue<Character>(Singleton<Service>.Instance.levelManager.player));
			}
		}

		// Token: 0x04005150 RID: 20816
		[SerializeField]
		private Transform _ownerTransform;

		// Token: 0x04005151 RID: 20817
		[SerializeField]
		private Character _ownerCharacter;

		// Token: 0x04005152 RID: 20818
		[SerializeField]
		private bool _playerIsTarget;
	}
}
