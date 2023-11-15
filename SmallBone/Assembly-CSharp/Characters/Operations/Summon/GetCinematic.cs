using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F2E RID: 3886
	[Serializable]
	public class GetCinematic : IBDCharacterSetting
	{
		// Token: 0x06004BB1 RID: 19377 RVA: 0x000DEB02 File Offset: 0x000DCD02
		public void ApplyTo(Character character)
		{
			character.StartCoroutineWithReference(this.CAttachCinematic(character));
		}

		// Token: 0x06004BB2 RID: 19378 RVA: 0x000DEB12 File Offset: 0x000DCD12
		private IEnumerator CAttachCinematic(Character character)
		{
			character.cinematic.Attach(this);
			yield return new WaitForSeconds(this._duration.value);
			character.cinematic.Detach(this);
			yield break;
		}

		// Token: 0x04003AE8 RID: 15080
		[SerializeField]
		private CustomFloat _duration;
	}
}
