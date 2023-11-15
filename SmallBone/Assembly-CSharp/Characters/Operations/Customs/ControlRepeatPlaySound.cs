using System;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FD4 RID: 4052
	public class ControlRepeatPlaySound : CharacterOperation
	{
		// Token: 0x06004E62 RID: 20066 RVA: 0x000EAC78 File Offset: 0x000E8E78
		public override void Run(Character owner)
		{
			ControlRepeatPlaySound.Type type = this._type;
			if (type == ControlRepeatPlaySound.Type.Play)
			{
				this._repeatPlaySound.Play();
				return;
			}
			if (type != ControlRepeatPlaySound.Type.Stop)
			{
				return;
			}
			this._repeatPlaySound.Stop();
		}

		// Token: 0x04003E71 RID: 15985
		[SerializeField]
		private RepeatPlaySound _repeatPlaySound;

		// Token: 0x04003E72 RID: 15986
		[SerializeField]
		private ControlRepeatPlaySound.Type _type;

		// Token: 0x02000FD5 RID: 4053
		private enum Type
		{
			// Token: 0x04003E74 RID: 15988
			Play,
			// Token: 0x04003E75 RID: 15989
			Stop
		}
	}
}
