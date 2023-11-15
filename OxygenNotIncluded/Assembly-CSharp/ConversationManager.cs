using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020006FC RID: 1788
[AddComponentMenu("KMonoBehaviour/scripts/ConversationManager")]
public class ConversationManager : KMonoBehaviour, ISim200ms
{
	// Token: 0x0600312D RID: 12589 RVA: 0x0010512A File Offset: 0x0010332A
	protected override void OnPrefabInit()
	{
		this.activeSetups = new List<Conversation>();
		this.lastConvoTimeByMinion = new Dictionary<MinionIdentity, float>();
		this.simRenderLoadBalance = true;
	}

	// Token: 0x0600312E RID: 12590 RVA: 0x0010514C File Offset: 0x0010334C
	public void Sim200ms(float dt)
	{
		for (int i = this.activeSetups.Count - 1; i >= 0; i--)
		{
			Conversation conversation = this.activeSetups[i];
			for (int j = conversation.minions.Count - 1; j >= 0; j--)
			{
				if (!this.ValidMinionTags(conversation.minions[j]) || !this.MinionCloseEnoughToConvo(conversation.minions[j], conversation))
				{
					conversation.minions.RemoveAt(j);
				}
				else
				{
					this.setupsByMinion[conversation.minions[j]] = conversation;
				}
			}
			if (conversation.minions.Count <= 1)
			{
				this.activeSetups.RemoveAt(i);
			}
			else
			{
				bool flag = true;
				bool flag2 = conversation.minions.Find((MinionIdentity match) => !match.HasTag(GameTags.Partying)) == null;
				if ((conversation.numUtterances == 0 && flag2 && GameClock.Instance.GetTime() > conversation.lastTalkedTime) || GameClock.Instance.GetTime() > conversation.lastTalkedTime + TuningData<ConversationManager.Tuning>.Get().delayBeforeStart)
				{
					MinionIdentity minionIdentity = conversation.minions[UnityEngine.Random.Range(0, conversation.minions.Count)];
					conversation.conversationType.NewTarget(minionIdentity);
					flag = this.DoTalking(conversation, minionIdentity);
				}
				else if (conversation.numUtterances > 0 && conversation.numUtterances < TuningData<ConversationManager.Tuning>.Get().maxUtterances && ((flag2 && GameClock.Instance.GetTime() > conversation.lastTalkedTime + TuningData<ConversationManager.Tuning>.Get().speakTime / 4f) || GameClock.Instance.GetTime() > conversation.lastTalkedTime + TuningData<ConversationManager.Tuning>.Get().speakTime + TuningData<ConversationManager.Tuning>.Get().delayBetweenUtterances))
				{
					int index = (conversation.minions.IndexOf(conversation.lastTalked) + UnityEngine.Random.Range(1, conversation.minions.Count)) % conversation.minions.Count;
					MinionIdentity new_speaker = conversation.minions[index];
					flag = this.DoTalking(conversation, new_speaker);
				}
				else if (conversation.numUtterances >= TuningData<ConversationManager.Tuning>.Get().maxUtterances)
				{
					flag = false;
				}
				if (!flag)
				{
					this.activeSetups.RemoveAt(i);
				}
			}
		}
		foreach (MinionIdentity minionIdentity2 in Components.LiveMinionIdentities.Items)
		{
			if (this.ValidMinionTags(minionIdentity2) && !this.setupsByMinion.ContainsKey(minionIdentity2) && !this.MinionOnCooldown(minionIdentity2))
			{
				foreach (MinionIdentity minionIdentity3 in Components.LiveMinionIdentities.Items)
				{
					if (!(minionIdentity3 == minionIdentity2) && this.ValidMinionTags(minionIdentity3))
					{
						if (this.setupsByMinion.ContainsKey(minionIdentity3))
						{
							Conversation conversation2 = this.setupsByMinion[minionIdentity3];
							if (conversation2.minions.Count < TuningData<ConversationManager.Tuning>.Get().maxDupesPerConvo && (this.GetCentroid(conversation2) - minionIdentity2.transform.GetPosition()).magnitude < TuningData<ConversationManager.Tuning>.Get().maxDistance * 0.5f)
							{
								conversation2.minions.Add(minionIdentity2);
								this.setupsByMinion[minionIdentity2] = conversation2;
								break;
							}
						}
						else if (!this.MinionOnCooldown(minionIdentity3) && (minionIdentity3.transform.GetPosition() - minionIdentity2.transform.GetPosition()).magnitude < TuningData<ConversationManager.Tuning>.Get().maxDistance)
						{
							Conversation conversation3 = new Conversation();
							conversation3.minions.Add(minionIdentity2);
							conversation3.minions.Add(minionIdentity3);
							Type type = this.convoTypes[UnityEngine.Random.Range(0, this.convoTypes.Count)];
							conversation3.conversationType = (ConversationType)Activator.CreateInstance(type);
							conversation3.lastTalkedTime = GameClock.Instance.GetTime();
							this.activeSetups.Add(conversation3);
							this.setupsByMinion[minionIdentity2] = conversation3;
							this.setupsByMinion[minionIdentity3] = conversation3;
							break;
						}
					}
				}
			}
		}
		this.setupsByMinion.Clear();
	}

	// Token: 0x0600312F RID: 12591 RVA: 0x001055FC File Offset: 0x001037FC
	private bool DoTalking(Conversation setup, MinionIdentity new_speaker)
	{
		DebugUtil.Assert(setup != null, "setup was null");
		DebugUtil.Assert(new_speaker != null, "new_speaker was null");
		if (setup.lastTalked != null)
		{
			setup.lastTalked.Trigger(25860745, setup.lastTalked.gameObject);
		}
		DebugUtil.Assert(setup.conversationType != null, "setup.conversationType was null");
		Conversation.Topic nextTopic = setup.conversationType.GetNextTopic(new_speaker, setup.lastTopic);
		if (nextTopic == null || nextTopic.mode == Conversation.ModeType.End || nextTopic.mode == Conversation.ModeType.Segue)
		{
			return false;
		}
		Thought thoughtForTopic = this.GetThoughtForTopic(setup, nextTopic);
		if (thoughtForTopic == null)
		{
			return false;
		}
		ThoughtGraph.Instance smi = new_speaker.GetSMI<ThoughtGraph.Instance>();
		if (smi == null)
		{
			return false;
		}
		smi.AddThought(thoughtForTopic);
		setup.lastTopic = nextTopic;
		setup.lastTalked = new_speaker;
		setup.lastTalkedTime = GameClock.Instance.GetTime();
		DebugUtil.Assert(this.lastConvoTimeByMinion != null, "lastConvoTimeByMinion was null");
		this.lastConvoTimeByMinion[setup.lastTalked] = GameClock.Instance.GetTime();
		Effects component = setup.lastTalked.GetComponent<Effects>();
		DebugUtil.Assert(component != null, "effects was null");
		component.Add("GoodConversation", true);
		Conversation.Mode mode = Conversation.Topic.Modes[(int)nextTopic.mode];
		DebugUtil.Assert(mode != null, "mode was null");
		ConversationManager.StartedTalkingEvent data = new ConversationManager.StartedTalkingEvent
		{
			talker = new_speaker.gameObject,
			anim = mode.anim
		};
		foreach (MinionIdentity minionIdentity in setup.minions)
		{
			if (!minionIdentity)
			{
				DebugUtil.DevAssert(false, "minion in setup.minions was null", null);
			}
			else
			{
				minionIdentity.Trigger(-594200555, data);
			}
		}
		setup.numUtterances++;
		return true;
	}

	// Token: 0x06003130 RID: 12592 RVA: 0x001057D8 File Offset: 0x001039D8
	public bool TryGetConversation(MinionIdentity minion, out Conversation conversation)
	{
		return this.setupsByMinion.TryGetValue(minion, out conversation);
	}

	// Token: 0x06003131 RID: 12593 RVA: 0x001057E8 File Offset: 0x001039E8
	private Vector3 GetCentroid(Conversation setup)
	{
		Vector3 a = Vector3.zero;
		foreach (MinionIdentity minionIdentity in setup.minions)
		{
			if (!(minionIdentity == null))
			{
				a += minionIdentity.transform.GetPosition();
			}
		}
		return a / (float)setup.minions.Count;
	}

	// Token: 0x06003132 RID: 12594 RVA: 0x00105868 File Offset: 0x00103A68
	private Thought GetThoughtForTopic(Conversation setup, Conversation.Topic topic)
	{
		if (string.IsNullOrEmpty(topic.topic))
		{
			DebugUtil.DevAssert(false, "topic.topic was null", null);
			return null;
		}
		Sprite sprite = setup.conversationType.GetSprite(topic.topic);
		if (sprite != null)
		{
			Conversation.Mode mode = Conversation.Topic.Modes[(int)topic.mode];
			return new Thought("Topic_" + topic.topic, null, sprite, mode.icon, mode.voice, "bubble_chatter", mode.mouth, DUPLICANTS.THOUGHTS.CONVERSATION.TOOLTIP, true, TuningData<ConversationManager.Tuning>.Get().speakTime);
		}
		return null;
	}

	// Token: 0x06003133 RID: 12595 RVA: 0x001058FC File Offset: 0x00103AFC
	private bool ValidMinionTags(MinionIdentity minion)
	{
		return !(minion == null) && !minion.GetComponent<KPrefabID>().HasAnyTags(ConversationManager.invalidConvoTags);
	}

	// Token: 0x06003134 RID: 12596 RVA: 0x0010591C File Offset: 0x00103B1C
	private bool MinionCloseEnoughToConvo(MinionIdentity minion, Conversation setup)
	{
		return (this.GetCentroid(setup) - minion.transform.GetPosition()).magnitude < TuningData<ConversationManager.Tuning>.Get().maxDistance * 0.5f;
	}

	// Token: 0x06003135 RID: 12597 RVA: 0x0010595C File Offset: 0x00103B5C
	private bool MinionOnCooldown(MinionIdentity minion)
	{
		return !minion.GetComponent<KPrefabID>().HasTag(GameTags.AlwaysConverse) && ((this.lastConvoTimeByMinion.ContainsKey(minion) && GameClock.Instance.GetTime() < this.lastConvoTimeByMinion[minion] + TuningData<ConversationManager.Tuning>.Get().minionCooldownTime) || GameClock.Instance.GetTime() / 600f < TuningData<ConversationManager.Tuning>.Get().cyclesBeforeFirstConversation);
	}

	// Token: 0x04001D8C RID: 7564
	private List<Conversation> activeSetups;

	// Token: 0x04001D8D RID: 7565
	private Dictionary<MinionIdentity, float> lastConvoTimeByMinion;

	// Token: 0x04001D8E RID: 7566
	private Dictionary<MinionIdentity, Conversation> setupsByMinion = new Dictionary<MinionIdentity, Conversation>();

	// Token: 0x04001D8F RID: 7567
	private List<Type> convoTypes = new List<Type>
	{
		typeof(RecentThingConversation),
		typeof(AmountStateConversation),
		typeof(CurrentJobConversation)
	};

	// Token: 0x04001D90 RID: 7568
	private static readonly Tag[] invalidConvoTags = new Tag[]
	{
		GameTags.Asleep,
		GameTags.HoldingBreath,
		GameTags.Dead
	};

	// Token: 0x0200144D RID: 5197
	public class Tuning : TuningData<ConversationManager.Tuning>
	{
		// Token: 0x04006516 RID: 25878
		public float cyclesBeforeFirstConversation;

		// Token: 0x04006517 RID: 25879
		public float maxDistance;

		// Token: 0x04006518 RID: 25880
		public int maxDupesPerConvo;

		// Token: 0x04006519 RID: 25881
		public float minionCooldownTime;

		// Token: 0x0400651A RID: 25882
		public float speakTime;

		// Token: 0x0400651B RID: 25883
		public float delayBetweenUtterances;

		// Token: 0x0400651C RID: 25884
		public float delayBeforeStart;

		// Token: 0x0400651D RID: 25885
		public int maxUtterances;
	}

	// Token: 0x0200144E RID: 5198
	public class StartedTalkingEvent
	{
		// Token: 0x0400651E RID: 25886
		public GameObject talker;

		// Token: 0x0400651F RID: 25887
		public string anim;
	}
}
