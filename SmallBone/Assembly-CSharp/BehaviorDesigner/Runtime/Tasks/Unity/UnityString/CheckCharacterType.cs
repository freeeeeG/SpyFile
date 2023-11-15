using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString
{
	// Token: 0x02001645 RID: 5701
	public class CheckCharacterType : Conditional
	{
		// Token: 0x06006CC8 RID: 27848 RVA: 0x0013712A File Offset: 0x0013532A
		public override void OnAwake()
		{
			this._ownerValue = this._character.Value;
		}

		// Token: 0x06006CC9 RID: 27849 RVA: 0x0013713D File Offset: 0x0013533D
		public override TaskStatus OnUpdate()
		{
			if (this._ownerValue.type == this._type)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		// Token: 0x04005892 RID: 22674
		[SerializeField]
		private SharedCharacter _character;

		// Token: 0x04005893 RID: 22675
		[SerializeField]
		private Character.Type _type;

		// Token: 0x04005894 RID: 22676
		private Character _ownerValue;
	}
}
