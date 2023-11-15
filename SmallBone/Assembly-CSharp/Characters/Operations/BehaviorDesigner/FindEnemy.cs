using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Characters.Operations.FindOptions;
using UnityEngine;

namespace Characters.Operations.BehaviorDesigner
{
	// Token: 0x02000F67 RID: 3943
	public class FindEnemy : Operation
	{
		// Token: 0x06004C8D RID: 19597 RVA: 0x000E31D0 File Offset: 0x000E13D0
		public override void Run()
		{
			List<Character> enemyList = this._scope.GetEnemyList();
			if (this._exceptSelf && enemyList.Contains(this._owner))
			{
				enemyList.Remove(this._owner);
			}
			SharedCharacter variable = this._communicator.GetVariable<SharedCharacter>(this._storeVariableName);
			if (variable == null)
			{
				return;
			}
			if (enemyList.Count == 0)
			{
				variable.SetValue(null);
				return;
			}
			IFilter[] filters = this._filters;
			for (int i = 0; i < filters.Length; i++)
			{
				filters[i].Filtered(enemyList);
			}
			if (this._shuffle)
			{
				enemyList.PseudoShuffle<Character>();
			}
			foreach (Character character in enemyList)
			{
				bool flag = true;
				ICondition[] condition = this._condition;
				for (int i = 0; i < condition.Length; i++)
				{
					if (!condition[i].Satisfied(character))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					variable.SetValue(character);
					return;
				}
			}
			if (enemyList.Count > 0 && this._condition.Length == 0)
			{
				variable.SetValue(enemyList[0]);
				return;
			}
			variable.SetValue(null);
		}

		// Token: 0x04003C34 RID: 15412
		[SerializeField]
		private BehaviorDesignerCommunicator _communicator;

		// Token: 0x04003C35 RID: 15413
		[SerializeField]
		private Character _owner;

		// Token: 0x04003C36 RID: 15414
		[SerializeField]
		private string _storeVariableName = "Target";

		// Token: 0x04003C37 RID: 15415
		[SerializeField]
		private bool _shuffle = true;

		// Token: 0x04003C38 RID: 15416
		[SerializeField]
		private bool _exceptSelf;

		// Token: 0x04003C39 RID: 15417
		[Header("Find Option")]
		[SerializeReference]
		[SubclassSelector]
		private IScope _scope;

		// Token: 0x04003C3A RID: 15418
		[SubclassSelector]
		[SerializeReference]
		private IFilter[] _filters;

		// Token: 0x04003C3B RID: 15419
		[SerializeReference]
		[SubclassSelector]
		private ICondition[] _condition;
	}
}
