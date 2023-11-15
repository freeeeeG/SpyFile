using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014CB RID: 5323
	[TaskIcon("{SkinColor}StackedConditionalIcon.png")]
	[TaskDescription("Allows multiple conditional tasks to be added to a single node.")]
	public class StackedConditional : Conditional
	{
		// Token: 0x0600677D RID: 26493 RVA: 0x0012B76C File Offset: 0x0012996C
		public override void OnAwake()
		{
			if (this.conditionals == null)
			{
				return;
			}
			for (int i = 0; i < this.conditionals.Length; i++)
			{
				if (this.conditionals[i] != null)
				{
					this.conditionals[i].GameObject = this.gameObject;
					this.conditionals[i].Transform = this.transform;
					this.conditionals[i].Owner = base.Owner;
					this.conditionals[i].OnAwake();
				}
			}
		}

		// Token: 0x0600677E RID: 26494 RVA: 0x0012B7E8 File Offset: 0x001299E8
		public override void OnStart()
		{
			if (this.conditionals == null)
			{
				return;
			}
			for (int i = 0; i < this.conditionals.Length; i++)
			{
				if (this.conditionals[i] != null)
				{
					this.conditionals[i].OnStart();
				}
			}
		}

		// Token: 0x0600677F RID: 26495 RVA: 0x0012B828 File Offset: 0x00129A28
		public override TaskStatus OnUpdate()
		{
			if (this.conditionals == null)
			{
				return TaskStatus.Failure;
			}
			for (int i = 0; i < this.conditionals.Length; i++)
			{
				if (this.conditionals[i] != null)
				{
					TaskStatus taskStatus = this.conditionals[i].OnUpdate();
					if (this.comparisonType == StackedConditional.ComparisonType.Sequence && taskStatus == TaskStatus.Failure)
					{
						return TaskStatus.Failure;
					}
					if (this.comparisonType == StackedConditional.ComparisonType.Selector && taskStatus == TaskStatus.Success)
					{
						return TaskStatus.Success;
					}
				}
			}
			if (this.comparisonType != StackedConditional.ComparisonType.Sequence)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006780 RID: 26496 RVA: 0x0012B894 File Offset: 0x00129A94
		public override void OnFixedUpdate()
		{
			if (this.conditionals == null)
			{
				return;
			}
			for (int i = 0; i < this.conditionals.Length; i++)
			{
				if (this.conditionals[i] != null)
				{
					this.conditionals[i].OnFixedUpdate();
				}
			}
		}

		// Token: 0x06006781 RID: 26497 RVA: 0x0012B8D4 File Offset: 0x00129AD4
		public override void OnLateUpdate()
		{
			if (this.conditionals == null)
			{
				return;
			}
			for (int i = 0; i < this.conditionals.Length; i++)
			{
				if (this.conditionals[i] != null)
				{
					this.conditionals[i].OnLateUpdate();
				}
			}
		}

		// Token: 0x06006782 RID: 26498 RVA: 0x0012B914 File Offset: 0x00129B14
		public override void OnEnd()
		{
			if (this.conditionals == null)
			{
				return;
			}
			for (int i = 0; i < this.conditionals.Length; i++)
			{
				if (this.conditionals[i] != null)
				{
					this.conditionals[i].OnEnd();
				}
			}
		}

		// Token: 0x06006783 RID: 26499 RVA: 0x0012B954 File Offset: 0x00129B54
		public override void OnTriggerEnter2D(Collider2D other)
		{
			if (this.conditionals == null)
			{
				return;
			}
			for (int i = 0; i < this.conditionals.Length; i++)
			{
				if (this.conditionals[i] != null)
				{
					this.conditionals[i].OnTriggerEnter2D(other);
				}
			}
		}

		// Token: 0x06006784 RID: 26500 RVA: 0x0012B998 File Offset: 0x00129B98
		public override void OnTriggerExit2D(Collider2D other)
		{
			if (this.conditionals == null)
			{
				return;
			}
			for (int i = 0; i < this.conditionals.Length; i++)
			{
				if (this.conditionals[i] != null)
				{
					this.conditionals[i].OnTriggerExit2D(other);
				}
			}
		}

		// Token: 0x06006785 RID: 26501 RVA: 0x0012B9DC File Offset: 0x00129BDC
		public override void OnCollisionEnter2D(Collision2D collision)
		{
			if (this.conditionals == null)
			{
				return;
			}
			for (int i = 0; i < this.conditionals.Length; i++)
			{
				if (this.conditionals[i] != null)
				{
					this.conditionals[i].OnCollisionEnter2D(collision);
				}
			}
		}

		// Token: 0x06006786 RID: 26502 RVA: 0x0012BA20 File Offset: 0x00129C20
		public override void OnCollisionExit2D(Collision2D collision)
		{
			if (this.conditionals == null)
			{
				return;
			}
			for (int i = 0; i < this.conditionals.Length; i++)
			{
				if (this.conditionals[i] != null)
				{
					this.conditionals[i].OnCollisionExit2D(collision);
				}
			}
		}

		// Token: 0x06006787 RID: 26503 RVA: 0x0012BA64 File Offset: 0x00129C64
		public override string OnDrawNodeText()
		{
			if (this.conditionals == null || !this.graphLabel)
			{
				return string.Empty;
			}
			string text = string.Empty;
			for (int i = 0; i < this.conditionals.Length; i++)
			{
				if (this.conditionals[i] != null)
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += "\n";
					}
					text += this.conditionals[i].GetType().Name;
				}
			}
			return text;
		}

		// Token: 0x06006788 RID: 26504 RVA: 0x0012BAD8 File Offset: 0x00129CD8
		public override void OnReset()
		{
			if (this.conditionals == null)
			{
				return;
			}
			for (int i = 0; i < this.conditionals.Length; i++)
			{
				if (this.conditionals[i] != null)
				{
					this.conditionals[i].OnReset();
				}
			}
		}

		// Token: 0x04005385 RID: 21381
		[InspectTask]
		public Conditional[] conditionals;

		// Token: 0x04005386 RID: 21382
		[Tooltip("Specifies if the tasks should be traversed with an AND (Sequence) or an OR (Selector).")]
		public StackedConditional.ComparisonType comparisonType;

		// Token: 0x04005387 RID: 21383
		[Tooltip("Should the tasks be labeled within the graph?")]
		public bool graphLabel;

		// Token: 0x020014CC RID: 5324
		public enum ComparisonType
		{
			// Token: 0x04005389 RID: 21385
			Sequence,
			// Token: 0x0400538A RID: 21386
			Selector
		}
	}
}
