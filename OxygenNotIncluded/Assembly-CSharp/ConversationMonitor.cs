using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x0200086D RID: 2157
public class ConversationMonitor : GameStateMachine<ConversationMonitor, ConversationMonitor.Instance, IStateMachineTarget, ConversationMonitor.Def>
{
	// Token: 0x06003F1E RID: 16158 RVA: 0x00160354 File Offset: 0x0015E554
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventHandler(GameHashes.TopicDiscussed, delegate(ConversationMonitor.Instance smi, object obj)
		{
			smi.OnTopicDiscussed(obj);
		}).EventHandler(GameHashes.TopicDiscovered, delegate(ConversationMonitor.Instance smi, object obj)
		{
			smi.OnTopicDiscovered(obj);
		});
	}

	// Token: 0x040028DD RID: 10461
	private const int MAX_RECENT_TOPICS = 5;

	// Token: 0x040028DE RID: 10462
	private const int MAX_FAVOURITE_TOPICS = 5;

	// Token: 0x040028DF RID: 10463
	private const float FAVOURITE_CHANCE = 0.033333335f;

	// Token: 0x040028E0 RID: 10464
	private const float LEARN_CHANCE = 0.33333334f;

	// Token: 0x02001651 RID: 5713
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001652 RID: 5714
	[SerializationConfig(MemberSerialization.OptIn)]
	public new class Instance : GameStateMachine<ConversationMonitor, ConversationMonitor.Instance, IStateMachineTarget, ConversationMonitor.Def>.GameInstance
	{
		// Token: 0x06008A58 RID: 35416 RVA: 0x003138FC File Offset: 0x00311AFC
		public Instance(IStateMachineTarget master, ConversationMonitor.Def def) : base(master, def)
		{
			this.recentTopics = new Queue<string>();
			this.favouriteTopics = new List<string>
			{
				ConversationMonitor.Instance.randomTopics[UnityEngine.Random.Range(0, ConversationMonitor.Instance.randomTopics.Count)]
			};
			this.personalTopics = new List<string>();
		}

		// Token: 0x06008A59 RID: 35417 RVA: 0x00313954 File Offset: 0x00311B54
		public string GetATopic()
		{
			int maxExclusive = this.recentTopics.Count + this.favouriteTopics.Count * 2 + this.personalTopics.Count;
			int num = UnityEngine.Random.Range(0, maxExclusive);
			if (num < this.recentTopics.Count)
			{
				return this.recentTopics.Dequeue();
			}
			num -= this.recentTopics.Count;
			if (num < this.favouriteTopics.Count)
			{
				return this.favouriteTopics[num];
			}
			num -= this.favouriteTopics.Count;
			if (num < this.favouriteTopics.Count)
			{
				return this.favouriteTopics[num];
			}
			num -= this.favouriteTopics.Count;
			if (num < this.personalTopics.Count)
			{
				return this.personalTopics[num];
			}
			return "";
		}

		// Token: 0x06008A5A RID: 35418 RVA: 0x00313A2C File Offset: 0x00311C2C
		public void OnTopicDiscovered(object data)
		{
			string item = (string)data;
			if (!this.recentTopics.Contains(item))
			{
				this.recentTopics.Enqueue(item);
				if (this.recentTopics.Count > 5)
				{
					string topic = this.recentTopics.Dequeue();
					this.TryMakeFavouriteTopic(topic);
				}
			}
		}

		// Token: 0x06008A5B RID: 35419 RVA: 0x00313A7C File Offset: 0x00311C7C
		public void OnTopicDiscussed(object data)
		{
			string data2 = (string)data;
			if (UnityEngine.Random.value < 0.33333334f)
			{
				this.OnTopicDiscovered(data2);
			}
		}

		// Token: 0x06008A5C RID: 35420 RVA: 0x00313AA4 File Offset: 0x00311CA4
		private void TryMakeFavouriteTopic(string topic)
		{
			if (UnityEngine.Random.value < 0.033333335f)
			{
				if (this.favouriteTopics.Count < 5)
				{
					this.favouriteTopics.Add(topic);
					return;
				}
				this.favouriteTopics[UnityEngine.Random.Range(0, this.favouriteTopics.Count)] = topic;
			}
		}

		// Token: 0x04006B4A RID: 27466
		[Serialize]
		private Queue<string> recentTopics;

		// Token: 0x04006B4B RID: 27467
		[Serialize]
		private List<string> favouriteTopics;

		// Token: 0x04006B4C RID: 27468
		private List<string> personalTopics;

		// Token: 0x04006B4D RID: 27469
		private static readonly List<string> randomTopics = new List<string>
		{
			"Headquarters"
		};
	}
}
