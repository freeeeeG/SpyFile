using System;
using System.Collections;
using UnityEngine;

namespace Tutorial
{
	// Token: 0x020000BE RID: 190
	public class Flow : MonoBehaviour
	{
		// Token: 0x060003AC RID: 940 RVA: 0x00002191 File Offset: 0x00000391
		public void OnServerInitialized()
		{
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000D33D File Offset: 0x0000B53D
		private IEnumerator Process()
		{
			foreach (Task task in this._tasks)
			{
				yield return task.Play();
			}
			Task[] array = null;
			yield break;
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000D34C File Offset: 0x0000B54C
		private IEnumerator Process1_1()
		{
			yield return null;
			yield break;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000D354 File Offset: 0x0000B554
		private IEnumerator Process1_2()
		{
			yield return null;
			yield break;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000D35C File Offset: 0x0000B55C
		private IEnumerator Process1_3()
		{
			yield return null;
			yield break;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000D364 File Offset: 0x0000B564
		private IEnumerator Process1_4()
		{
			yield return null;
			yield break;
		}

		// Token: 0x040002E7 RID: 743
		[SerializeField]
		private Task[] _tasks;

		// Token: 0x040002E8 RID: 744
		private Flow.StartCondition _startCondition;

		// Token: 0x020000BF RID: 191
		private enum StartCondition
		{
			// Token: 0x040002EA RID: 746
			TimeOutAfterSpawn,
			// Token: 0x040002EB RID: 747
			RemainMonsters,
			// Token: 0x040002EC RID: 748
			EnterZone
		}
	}
}
