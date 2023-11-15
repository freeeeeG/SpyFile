using System;
using System.Collections.Generic;
using Characters.Operations.FindOptions;
using UnityEngine;
using Utils;

namespace Characters.Operations.GrabBorad
{
	// Token: 0x02000E90 RID: 3728
	public class AddToGrabBoard : Operation
	{
		// Token: 0x060049C0 RID: 18880 RVA: 0x000D76A4 File Offset: 0x000D58A4
		public override void Run()
		{
			List<Character> enemyList = this._scope.GetEnemyList();
			if (this._exceptSelf && enemyList.Contains(this._owner))
			{
				enemyList.Remove(this._owner);
			}
			if (enemyList.Count == 0)
			{
				return;
			}
			IFilter[] filters = this._filters;
			for (int i = 0; i < filters.Length; i++)
			{
				filters[i].Filtered(enemyList);
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
					Target target = character.GetComponent<Target>();
					if (target != null)
					{
						this._grabBoard.Add(target);
						character.onDie += delegate()
						{
							this._grabBoard.Remove(target);
						};
					}
				}
			}
		}

		// Token: 0x040038FA RID: 14586
		[SerializeField]
		private GrabBoard _grabBoard;

		// Token: 0x040038FB RID: 14587
		[SerializeField]
		private Character _owner;

		// Token: 0x040038FC RID: 14588
		[SerializeField]
		private bool _exceptSelf;

		// Token: 0x040038FD RID: 14589
		[SerializeReference]
		[SubclassSelector]
		[Header("Find Option")]
		private IScope _scope;

		// Token: 0x040038FE RID: 14590
		[SerializeReference]
		[SubclassSelector]
		private IFilter[] _filters;

		// Token: 0x040038FF RID: 14591
		[SerializeReference]
		[SubclassSelector]
		private ICondition[] _condition;
	}
}
