using System;
using System.Collections.Generic;
using Level;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DDD RID: 3549
	public class GiveBuff : CharacterOperation
	{
		// Token: 0x0600472A RID: 18218 RVA: 0x000CEA33 File Offset: 0x000CCC33
		static GiveBuff()
		{
			GiveBuff._enemyOverlapper.contactFilter.SetLayerMask(1024);
		}

		// Token: 0x0600472B RID: 18219 RVA: 0x000CEA5A File Offset: 0x000CCC5A
		private void Start()
		{
			if (this._enemyWaveContainer == null)
			{
				this._enemyWaveContainer = base.GetComponentInParent<EnemyWaveContainer>();
			}
		}

		// Token: 0x0600472C RID: 18220 RVA: 0x000CEA78 File Offset: 0x000CCC78
		private List<Character> FindRandomEnemy(Character except)
		{
			List<Character> allSpawnedEnemies = this._enemyWaveContainer.GetAllSpawnedEnemies();
			List<Character> list = new List<Character>();
			foreach (Character character in allSpawnedEnemies)
			{
				if (character != except)
				{
					list.Add(character);
				}
			}
			if (list.Count <= 0)
			{
				return list;
			}
			int count = Mathf.Min(list.Count, 1);
			list.PseudoShuffle<Character>();
			return list.GetRange(0, count);
		}

		// Token: 0x0600472D RID: 18221 RVA: 0x000CEB08 File Offset: 0x000CCD08
		public override void Run(Character owner)
		{
			this._buffTargets = this.FindRandomEnemy(owner);
			foreach (Character owner2 in this._buffTargets)
			{
				this._attachAbility.Run(owner2);
			}
		}

		// Token: 0x0600472E RID: 18222 RVA: 0x000CEB70 File Offset: 0x000CCD70
		public override void Stop()
		{
			if (this._attachAbility == null)
			{
				return;
			}
			base.Stop();
			this._attachAbility.Stop();
		}

		// Token: 0x0400360E RID: 13838
		[UnityEditor.Subcomponent(typeof(AttachAbility))]
		[SerializeField]
		private AttachAbility _attachAbility;

		// Token: 0x0400360F RID: 13839
		[SerializeField]
		private EnemyWaveContainer _enemyWaveContainer;

		// Token: 0x04003610 RID: 13840
		private List<Character> _buffTargets;

		// Token: 0x04003611 RID: 13841
		private static readonly NonAllocOverlapper _enemyOverlapper = new NonAllocOverlapper(15);

		// Token: 0x04003612 RID: 13842
		private const int _targetCount = 1;
	}
}
