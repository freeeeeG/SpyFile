using System;
using System.Collections;
using UnityEngine;

namespace Level.Objects.DecorationCharacter.Command
{
	// Token: 0x02000583 RID: 1411
	[Serializable]
	public class WaitCommand : ICommand
	{
		// Token: 0x06001BB2 RID: 7090 RVA: 0x000563CF File Offset: 0x000545CF
		public IEnumerator CRun()
		{
			yield return this._owner.chronometer.master.WaitForSeconds(this._waitSeconds.value);
			yield break;
		}

		// Token: 0x040017D2 RID: 6098
		[SerializeField]
		private DecorationCharacter _owner;

		// Token: 0x040017D3 RID: 6099
		[SerializeField]
		private CustomFloat _waitSeconds;
	}
}
