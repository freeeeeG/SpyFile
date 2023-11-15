using System;
using System.Collections;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F2C RID: 3884
	[Serializable]
	public class EnableBDAfterWaiting : IBDCharacterSetting
	{
		// Token: 0x06004BA8 RID: 19368 RVA: 0x000DEA4C File Offset: 0x000DCC4C
		public void ApplyTo(Character character)
		{
			BehaviorTree component = character.GetComponent<BehaviorTree>();
			component.enabled = false;
			character.StartCoroutineWithReference(this.CWaitAndThenEnable(component));
		}

		// Token: 0x06004BA9 RID: 19369 RVA: 0x000DEA75 File Offset: 0x000DCC75
		private IEnumerator CWaitAndThenEnable(BehaviorTree behaviorDesigner)
		{
			yield return new WaitForSeconds(this._waitSeconds.value);
			behaviorDesigner.enabled = true;
			yield break;
		}

		// Token: 0x04003AE3 RID: 15075
		[SerializeField]
		private CustomFloat _waitSeconds;
	}
}
