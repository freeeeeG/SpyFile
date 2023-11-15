using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001484 RID: 5252
	[TaskDescription("Allows multiple action tasks to be added to a single node.")]
	[TaskIcon("{SkinColor}StackedActionIcon.png")]
	public class StackedAction : Action
	{
		// Token: 0x06006654 RID: 26196 RVA: 0x0012800C File Offset: 0x0012620C
		public override void OnAwake()
		{
			if (this.actions == null)
			{
				return;
			}
			for (int i = 0; i < this.actions.Length; i++)
			{
				if (this.actions[i] != null)
				{
					this.actions[i].GameObject = this.gameObject;
					this.actions[i].Transform = this.transform;
					this.actions[i].Owner = base.Owner;
					this.actions[i].OnAwake();
				}
			}
		}

		// Token: 0x06006655 RID: 26197 RVA: 0x00128088 File Offset: 0x00126288
		public override void OnStart()
		{
			if (this.actions == null)
			{
				return;
			}
			for (int i = 0; i < this.actions.Length; i++)
			{
				if (this.actions[i] != null)
				{
					this.actions[i].OnStart();
				}
			}
		}

		// Token: 0x06006656 RID: 26198 RVA: 0x001280C8 File Offset: 0x001262C8
		public override TaskStatus OnUpdate()
		{
			if (this.actions == null)
			{
				return TaskStatus.Failure;
			}
			for (int i = 0; i < this.actions.Length; i++)
			{
				if (this.actions[i] != null)
				{
					TaskStatus taskStatus = this.actions[i].OnUpdate();
					if (this.comparisonType == StackedAction.ComparisonType.Sequence && taskStatus == TaskStatus.Failure)
					{
						return TaskStatus.Failure;
					}
					if (this.comparisonType == StackedAction.ComparisonType.Selector && taskStatus == TaskStatus.Success)
					{
						return TaskStatus.Success;
					}
				}
			}
			if (this.comparisonType != StackedAction.ComparisonType.Sequence)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006657 RID: 26199 RVA: 0x00128134 File Offset: 0x00126334
		public override void OnFixedUpdate()
		{
			if (this.actions == null)
			{
				return;
			}
			for (int i = 0; i < this.actions.Length; i++)
			{
				if (this.actions[i] != null)
				{
					this.actions[i].OnFixedUpdate();
				}
			}
		}

		// Token: 0x06006658 RID: 26200 RVA: 0x00128174 File Offset: 0x00126374
		public override void OnLateUpdate()
		{
			if (this.actions == null)
			{
				return;
			}
			for (int i = 0; i < this.actions.Length; i++)
			{
				if (this.actions[i] != null)
				{
					this.actions[i].OnLateUpdate();
				}
			}
		}

		// Token: 0x06006659 RID: 26201 RVA: 0x001281B4 File Offset: 0x001263B4
		public override void OnEnd()
		{
			if (this.actions == null)
			{
				return;
			}
			for (int i = 0; i < this.actions.Length; i++)
			{
				if (this.actions[i] != null)
				{
					this.actions[i].OnEnd();
				}
			}
		}

		// Token: 0x0600665A RID: 26202 RVA: 0x001281F4 File Offset: 0x001263F4
		public override void OnTriggerEnter2D(Collider2D other)
		{
			if (this.actions == null)
			{
				return;
			}
			for (int i = 0; i < this.actions.Length; i++)
			{
				if (this.actions[i] != null)
				{
					this.actions[i].OnTriggerEnter2D(other);
				}
			}
		}

		// Token: 0x0600665B RID: 26203 RVA: 0x00128238 File Offset: 0x00126438
		public override void OnTriggerExit2D(Collider2D other)
		{
			if (this.actions == null)
			{
				return;
			}
			for (int i = 0; i < this.actions.Length; i++)
			{
				if (this.actions[i] != null)
				{
					this.actions[i].OnTriggerExit2D(other);
				}
			}
		}

		// Token: 0x0600665C RID: 26204 RVA: 0x0012827C File Offset: 0x0012647C
		public override void OnCollisionEnter2D(Collision2D collision)
		{
			if (this.actions == null)
			{
				return;
			}
			for (int i = 0; i < this.actions.Length; i++)
			{
				if (this.actions[i] != null)
				{
					this.actions[i].OnCollisionEnter2D(collision);
				}
			}
		}

		// Token: 0x0600665D RID: 26205 RVA: 0x001282C0 File Offset: 0x001264C0
		public override void OnCollisionExit2D(Collision2D collision)
		{
			if (this.actions == null)
			{
				return;
			}
			for (int i = 0; i < this.actions.Length; i++)
			{
				if (this.actions[i] != null)
				{
					this.actions[i].OnCollisionExit2D(collision);
				}
			}
		}

		// Token: 0x0600665E RID: 26206 RVA: 0x00128304 File Offset: 0x00126504
		public override string OnDrawNodeText()
		{
			if (this.actions == null || !this.graphLabel)
			{
				return string.Empty;
			}
			string text = string.Empty;
			for (int i = 0; i < this.actions.Length; i++)
			{
				if (this.actions[i] != null)
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += "\n";
					}
					text += this.actions[i].GetType().Name;
				}
			}
			return text;
		}

		// Token: 0x0600665F RID: 26207 RVA: 0x00128378 File Offset: 0x00126578
		public override void OnReset()
		{
			if (this.actions == null)
			{
				return;
			}
			for (int i = 0; i < this.actions.Length; i++)
			{
				if (this.actions[i] != null)
				{
					this.actions[i].OnReset();
				}
			}
		}

		// Token: 0x04005268 RID: 21096
		[InspectTask]
		public Action[] actions;

		// Token: 0x04005269 RID: 21097
		[Tooltip("Specifies if the tasks should be traversed with an AND (Sequence) or an OR (Selector).")]
		public StackedAction.ComparisonType comparisonType;

		// Token: 0x0400526A RID: 21098
		[Tooltip("Should the tasks be labeled within the graph?")]
		public bool graphLabel;

		// Token: 0x02001485 RID: 5253
		public enum ComparisonType
		{
			// Token: 0x0400526C RID: 21100
			Sequence,
			// Token: 0x0400526D RID: 21101
			Selector
		}
	}
}
