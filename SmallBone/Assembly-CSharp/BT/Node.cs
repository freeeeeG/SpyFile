using System;
using UnityEditor;
using UnityEngine;

namespace BT
{
	// Token: 0x02001423 RID: 5155
	public abstract class Node : MonoBehaviour, INode
	{
		// Token: 0x06006545 RID: 25925 RVA: 0x00125470 File Offset: 0x00123670
		public NodeState Tick(Context context)
		{
			if (this._state == NodeState.Ready)
			{
				this.OnInitialize();
			}
			this._state = this.UpdateDeltatime(context);
			if (this._state == NodeState.Ready)
			{
				throw new InvalidOperationException("Warning infinite loop!!");
			}
			if (this._state != NodeState.Running)
			{
				this.OnTerminate(this._state);
			}
			return this._state;
		}

		// Token: 0x06006546 RID: 25926 RVA: 0x001254C8 File Offset: 0x001236C8
		public void ResetState()
		{
			if (this._state == NodeState.Ready)
			{
				return;
			}
			this.DoReset(this._state);
			this._state = NodeState.Ready;
		}

		// Token: 0x06006547 RID: 25927
		protected abstract NodeState UpdateDeltatime(Context context);

		// Token: 0x06006548 RID: 25928 RVA: 0x00002191 File Offset: 0x00000391
		protected virtual void OnInitialize()
		{
		}

		// Token: 0x06006549 RID: 25929 RVA: 0x00002191 File Offset: 0x00000391
		protected virtual void OnStop()
		{
		}

		// Token: 0x0600654A RID: 25930 RVA: 0x00002191 File Offset: 0x00000391
		protected virtual void OnTerminate(NodeState state)
		{
		}

		// Token: 0x0600654B RID: 25931 RVA: 0x00002191 File Offset: 0x00000391
		protected virtual void DoReset(NodeState state)
		{
		}

		// Token: 0x0600654C RID: 25932 RVA: 0x00002191 File Offset: 0x00000391
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x0400519B RID: 20891
		protected NodeState _state = NodeState.Ready;

		// Token: 0x02001424 RID: 5156
		[AttributeUsage(AttributeTargets.Field)]
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x0600654E RID: 25934 RVA: 0x001254F6 File Offset: 0x001236F6
			public SubcomponentAttribute(bool allowCustom = true) : base(allowCustom, Node.SubcomponentAttribute.types)
			{
			}

			// Token: 0x0400519C RID: 20892
			public new static readonly Type[] types = new Type[]
			{
				typeof(AutoReset),
				typeof(CheckWithInSight),
				typeof(CheckRaySight),
				typeof(CheckWithInMasterSight),
				typeof(Cooldown),
				typeof(Conditional),
				typeof(Despawn),
				typeof(Failer),
				typeof(Inverter),
				typeof(MoveToTarget),
				typeof(MoveTowards),
				typeof(MoveToLookingDirection),
				typeof(BT.Random),
				typeof(RangeWander),
				typeof(RepeatForever),
				typeof(Repeat),
				typeof(RunAction),
				typeof(RunOperations),
				typeof(SetPositionTo),
				typeof(Selector),
				typeof(Sequence),
				typeof(Succeder),
				typeof(TimeLimit),
				typeof(TranslateTowards),
				typeof(UntilFail),
				typeof(UntilSuccess),
				typeof(WaitForDuration)
			};
		}

		// Token: 0x02001425 RID: 5157
		[Serializable]
		public class Subcomponents : SubcomponentArray<Node>
		{
		}
	}
}
