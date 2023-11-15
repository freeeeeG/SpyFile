using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DCF RID: 3535
	[SerializationConfig(MemberSerialization.OptIn)]
	[DebuggerDisplay("{amount.Name} {value} ({deltaAttribute.value}/{minAttribute.value}/{maxAttribute.value})")]
	public class AmountInstance : ModifierInstance<Amount>, ISaveLoadable, ISim200ms
	{
		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06006CCF RID: 27855 RVA: 0x002AEABC File Offset: 0x002ACCBC
		public Amount amount
		{
			get
			{
				return this.modifier;
			}
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06006CD0 RID: 27856 RVA: 0x002AEAC4 File Offset: 0x002ACCC4
		// (set) Token: 0x06006CD1 RID: 27857 RVA: 0x002AEACC File Offset: 0x002ACCCC
		public bool paused
		{
			get
			{
				return this._paused;
			}
			set
			{
				this._paused = this.paused;
				if (this._paused)
				{
					this.Deactivate();
					return;
				}
				this.Activate();
			}
		}

		// Token: 0x06006CD2 RID: 27858 RVA: 0x002AEAEF File Offset: 0x002ACCEF
		public float GetMin()
		{
			return this.minAttribute.GetTotalValue();
		}

		// Token: 0x06006CD3 RID: 27859 RVA: 0x002AEAFC File Offset: 0x002ACCFC
		public float GetMax()
		{
			return this.maxAttribute.GetTotalValue();
		}

		// Token: 0x06006CD4 RID: 27860 RVA: 0x002AEB09 File Offset: 0x002ACD09
		public float GetDelta()
		{
			return this.deltaAttribute.GetTotalValue();
		}

		// Token: 0x06006CD5 RID: 27861 RVA: 0x002AEB18 File Offset: 0x002ACD18
		public AmountInstance(Amount amount, GameObject game_object) : base(game_object, amount)
		{
			Attributes attributes = game_object.GetAttributes();
			this.minAttribute = attributes.Add(amount.minAttribute);
			this.maxAttribute = attributes.Add(amount.maxAttribute);
			this.deltaAttribute = attributes.Add(amount.deltaAttribute);
		}

		// Token: 0x06006CD6 RID: 27862 RVA: 0x002AEB6A File Offset: 0x002ACD6A
		public float SetValue(float value)
		{
			this.value = Mathf.Min(Mathf.Max(value, this.GetMin()), this.GetMax());
			return this.value;
		}

		// Token: 0x06006CD7 RID: 27863 RVA: 0x002AEB8F File Offset: 0x002ACD8F
		public void Publish(float delta, float previous_value)
		{
			if (this.OnDelta != null)
			{
				this.OnDelta(delta);
			}
			if (this.OnMaxValueReached != null && previous_value < this.GetMax() && this.value >= this.GetMax())
			{
				this.OnMaxValueReached();
			}
		}

		// Token: 0x06006CD8 RID: 27864 RVA: 0x002AEBD0 File Offset: 0x002ACDD0
		public float ApplyDelta(float delta)
		{
			float previous_value = this.value;
			this.SetValue(this.value + delta);
			this.Publish(delta, previous_value);
			return this.value;
		}

		// Token: 0x06006CD9 RID: 27865 RVA: 0x002AEC01 File Offset: 0x002ACE01
		public string GetValueString()
		{
			return this.amount.GetValueString(this);
		}

		// Token: 0x06006CDA RID: 27866 RVA: 0x002AEC0F File Offset: 0x002ACE0F
		public string GetDescription()
		{
			return this.amount.GetDescription(this);
		}

		// Token: 0x06006CDB RID: 27867 RVA: 0x002AEC1D File Offset: 0x002ACE1D
		public string GetTooltip()
		{
			return this.amount.GetTooltip(this);
		}

		// Token: 0x06006CDC RID: 27868 RVA: 0x002AEC2B File Offset: 0x002ACE2B
		public void Activate()
		{
			SimAndRenderScheduler.instance.Add(this, false);
		}

		// Token: 0x06006CDD RID: 27869 RVA: 0x002AEC39 File Offset: 0x002ACE39
		public void Sim200ms(float dt)
		{
		}

		// Token: 0x06006CDE RID: 27870 RVA: 0x002AEC3C File Offset: 0x002ACE3C
		public static void BatchUpdate(List<UpdateBucketWithUpdater<ISim200ms>.Entry> amount_instances, float time_delta)
		{
			if (time_delta == 0f)
			{
				return;
			}
			AmountInstance.BatchUpdateContext batchUpdateContext = new AmountInstance.BatchUpdateContext(amount_instances, time_delta);
			AmountInstance.batch_update_job.Reset(batchUpdateContext);
			int num = 512;
			for (int i = 0; i < amount_instances.Count; i += num)
			{
				int num2 = i + num;
				if (amount_instances.Count < num2)
				{
					num2 = amount_instances.Count;
				}
				AmountInstance.batch_update_job.Add(new AmountInstance.BatchUpdateTask(i, num2));
			}
			GlobalJobManager.Run(AmountInstance.batch_update_job);
			batchUpdateContext.Finish();
			AmountInstance.batch_update_job.Reset(null);
		}

		// Token: 0x06006CDF RID: 27871 RVA: 0x002AECBC File Offset: 0x002ACEBC
		public void Deactivate()
		{
			SimAndRenderScheduler.instance.Remove(this);
		}

		// Token: 0x040051B2 RID: 20914
		[Serialize]
		public float value;

		// Token: 0x040051B3 RID: 20915
		public AttributeInstance minAttribute;

		// Token: 0x040051B4 RID: 20916
		public AttributeInstance maxAttribute;

		// Token: 0x040051B5 RID: 20917
		public AttributeInstance deltaAttribute;

		// Token: 0x040051B6 RID: 20918
		public Action<float> OnDelta;

		// Token: 0x040051B7 RID: 20919
		public System.Action OnMaxValueReached;

		// Token: 0x040051B8 RID: 20920
		public bool hide;

		// Token: 0x040051B9 RID: 20921
		private bool _paused;

		// Token: 0x040051BA RID: 20922
		private static WorkItemCollection<AmountInstance.BatchUpdateTask, AmountInstance.BatchUpdateContext> batch_update_job = new WorkItemCollection<AmountInstance.BatchUpdateTask, AmountInstance.BatchUpdateContext>();

		// Token: 0x02001F5C RID: 8028
		private class BatchUpdateContext
		{
			// Token: 0x0600A243 RID: 41539 RVA: 0x00363A44 File Offset: 0x00361C44
			public BatchUpdateContext(List<UpdateBucketWithUpdater<ISim200ms>.Entry> amount_instances, float time_delta)
			{
				for (int num = 0; num != amount_instances.Count; num++)
				{
					UpdateBucketWithUpdater<ISim200ms>.Entry value = amount_instances[num];
					value.lastUpdateTime = 0f;
					amount_instances[num] = value;
				}
				this.amount_instances = amount_instances;
				this.time_delta = time_delta;
				this.results = ListPool<AmountInstance.BatchUpdateContext.Result, AmountInstance>.Allocate();
				this.results.Capacity = this.amount_instances.Count;
			}

			// Token: 0x0600A244 RID: 41540 RVA: 0x00363AB4 File Offset: 0x00361CB4
			public void Finish()
			{
				foreach (AmountInstance.BatchUpdateContext.Result result in this.results)
				{
					result.amount_instance.Publish(result.delta, result.previous);
				}
				this.results.Recycle();
			}

			// Token: 0x04008DE2 RID: 36322
			public List<UpdateBucketWithUpdater<ISim200ms>.Entry> amount_instances;

			// Token: 0x04008DE3 RID: 36323
			public float time_delta;

			// Token: 0x04008DE4 RID: 36324
			public ListPool<AmountInstance.BatchUpdateContext.Result, AmountInstance>.PooledList results;

			// Token: 0x02002F39 RID: 12089
			public struct Result
			{
				// Token: 0x0400C144 RID: 49476
				public AmountInstance amount_instance;

				// Token: 0x0400C145 RID: 49477
				public float previous;

				// Token: 0x0400C146 RID: 49478
				public float delta;
			}
		}

		// Token: 0x02001F5D RID: 8029
		private struct BatchUpdateTask : IWorkItem<AmountInstance.BatchUpdateContext>
		{
			// Token: 0x0600A245 RID: 41541 RVA: 0x00363B24 File Offset: 0x00361D24
			public BatchUpdateTask(int start, int end)
			{
				this.start = start;
				this.end = end;
			}

			// Token: 0x0600A246 RID: 41542 RVA: 0x00363B34 File Offset: 0x00361D34
			public void Run(AmountInstance.BatchUpdateContext context)
			{
				for (int num = this.start; num != this.end; num++)
				{
					AmountInstance amountInstance = (AmountInstance)context.amount_instances[num].data;
					float num2 = amountInstance.GetDelta() * context.time_delta;
					if (num2 != 0f)
					{
						context.results.Add(new AmountInstance.BatchUpdateContext.Result
						{
							amount_instance = amountInstance,
							previous = amountInstance.value,
							delta = num2
						});
						amountInstance.SetValue(amountInstance.value + num2);
					}
				}
			}

			// Token: 0x04008DE5 RID: 36325
			private int start;

			// Token: 0x04008DE6 RID: 36326
			private int end;
		}
	}
}
