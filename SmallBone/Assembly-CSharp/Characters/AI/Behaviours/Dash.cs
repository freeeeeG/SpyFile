using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using Hardmode;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012E0 RID: 4832
	public class Dash : Behaviour
	{
		// Token: 0x06005F91 RID: 24465 RVA: 0x00117E50 File Offset: 0x00116050
		protected void Start()
		{
			this._childs = new List<Behaviour>
			{
				this._moveToDestination
			};
			this._hardmodeStat = new Stat.Values(new Stat.Value[]
			{
				new Stat.Value(Stat.Category.Constant, Stat.Kind.MovementSpeed, 50.0)
			});
		}

		// Token: 0x06005F92 RID: 24466 RVA: 0x00117EA0 File Offset: 0x001160A0
		public override IEnumerator CRun(AIController controller)
		{
			Character target = controller.target;
			Character character = controller.character;
			float num = Mathf.Abs(character.transform.position.x - target.transform.position.x);
			float x = this._minMaxDistance.x;
			float y = this._minMaxDistance.y;
			this._motion.TryStart();
			base.result = Behaviour.Result.Doing;
			if (Singleton<HardmodeManager>.Instance.hardmode)
			{
				character.stat.AttachValues(this._hardmodeStat);
			}
			if (num >= x && num < y)
			{
				controller.destination = target.transform.position;
				yield return this._moveToDestination.CRun(controller);
			}
			else if (num >= y)
			{
				if (character.transform.position.x < target.transform.position.x)
				{
					controller.destination = new Vector2(character.transform.position.x + y, character.transform.position.y);
				}
				else
				{
					controller.destination = new Vector2(character.transform.position.x - y, character.transform.position.y);
				}
				yield return this._moveToDestination.CRun(controller);
			}
			if (Singleton<HardmodeManager>.Instance.hardmode)
			{
				character.stat.DetachValues(this._hardmodeStat);
			}
			character.CancelAction();
			base.result = Behaviour.Result.Done;
			yield break;
		}

		// Token: 0x04004CD1 RID: 19665
		[SerializeField]
		[MinMaxSlider(0f, 20f)]
		private Vector2 _minMaxDistance;

		// Token: 0x04004CD2 RID: 19666
		[UnityEditor.Subcomponent(typeof(MoveToDestination))]
		[SerializeField]
		private MoveToDestination _moveToDestination;

		// Token: 0x04004CD3 RID: 19667
		[SerializeField]
		private Characters.Actions.Action _motion;

		// Token: 0x04004CD4 RID: 19668
		private Stat.Values _hardmodeStat;
	}
}
