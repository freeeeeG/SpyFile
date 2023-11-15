using System;
using System.Collections;
using UnityEngine;

namespace Level.Objects.DecorationCharacter.Command
{
	// Token: 0x02000580 RID: 1408
	public class RandomCommandRunner : MonoBehaviour
	{
		// Token: 0x06001BA8 RID: 7080 RVA: 0x0005621E File Offset: 0x0005441E
		private void Start()
		{
			base.StartCoroutine(this.CRunSequance());
		}

		// Token: 0x06001BA9 RID: 7081 RVA: 0x0005622D File Offset: 0x0005442D
		private IEnumerator CRunSequance()
		{
			foreach (ICommand command in this._startCommands)
			{
				yield return command.CRun();
			}
			ICommand[] array = null;
			if (this._weightedCommands.Length == 0)
			{
				yield break;
			}
			for (;;)
			{
				this._currentIndex = this.GetWeightedRandomIndex();
				yield return this._weightedCommands[this._currentIndex].command.CRun();
			}
		}

		// Token: 0x06001BAA RID: 7082 RVA: 0x0005623C File Offset: 0x0005443C
		private int GetWeightedRandomIndex()
		{
			int num = 0;
			foreach (RandomCommandRunner.WeightedCommand weightedCommand in this._weightedCommands)
			{
				num += weightedCommand.weight;
			}
			if (num == 0)
			{
				return UnityEngine.Random.Range(0, this._weightedCommands.Length);
			}
			int num2 = UnityEngine.Random.Range(0, num);
			for (int j = 0; j < this._weightedCommands.Length; j++)
			{
				if (num2 < this._weightedCommands[j].weight)
				{
					return j;
				}
				num2 -= this._weightedCommands[j].weight;
			}
			return 0;
		}

		// Token: 0x040017C8 RID: 6088
		[SubclassSelector]
		[SerializeReference]
		private ICommand[] _startCommands;

		// Token: 0x040017C9 RID: 6089
		[SerializeField]
		private RandomCommandRunner.WeightedCommand[] _weightedCommands;

		// Token: 0x040017CA RID: 6090
		private int _currentIndex;

		// Token: 0x02000581 RID: 1409
		[Serializable]
		private struct WeightedCommand
		{
			// Token: 0x040017CB RID: 6091
			[SerializeReference]
			[SubclassSelector]
			public ICommand command;

			// Token: 0x040017CC RID: 6092
			public int weight;
		}
	}
}
