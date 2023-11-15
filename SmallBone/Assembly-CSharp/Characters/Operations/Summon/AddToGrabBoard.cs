using System;
using UnityEngine;
using Utils;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F29 RID: 3881
	[Serializable]
	public class AddToGrabBoard : IBDCharacterSetting
	{
		// Token: 0x06004BA2 RID: 19362 RVA: 0x000DE9FC File Offset: 0x000DCBFC
		public void ApplyTo(Character character)
		{
			Target component = character.GetComponent<Target>();
			if (component != null)
			{
				this._grabBoard.Add(component);
			}
		}

		// Token: 0x04003AE2 RID: 15074
		[SerializeField]
		private GrabBoard _grabBoard;
	}
}
