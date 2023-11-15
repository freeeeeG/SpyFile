using System;
using System.Collections.Generic;

// Token: 0x020006FD RID: 1789
public class Conversation
{
	// Token: 0x04001D91 RID: 7569
	public List<MinionIdentity> minions = new List<MinionIdentity>();

	// Token: 0x04001D92 RID: 7570
	public MinionIdentity lastTalked;

	// Token: 0x04001D93 RID: 7571
	public ConversationType conversationType;

	// Token: 0x04001D94 RID: 7572
	public float lastTalkedTime;

	// Token: 0x04001D95 RID: 7573
	public Conversation.Topic lastTopic;

	// Token: 0x04001D96 RID: 7574
	public int numUtterances;

	// Token: 0x02001450 RID: 5200
	public enum ModeType
	{
		// Token: 0x04006523 RID: 25891
		Query,
		// Token: 0x04006524 RID: 25892
		Statement,
		// Token: 0x04006525 RID: 25893
		Agreement,
		// Token: 0x04006526 RID: 25894
		Disagreement,
		// Token: 0x04006527 RID: 25895
		Musing,
		// Token: 0x04006528 RID: 25896
		Satisfaction,
		// Token: 0x04006529 RID: 25897
		Nominal,
		// Token: 0x0400652A RID: 25898
		Dissatisfaction,
		// Token: 0x0400652B RID: 25899
		Stressing,
		// Token: 0x0400652C RID: 25900
		Segue,
		// Token: 0x0400652D RID: 25901
		End
	}

	// Token: 0x02001451 RID: 5201
	public class Mode
	{
		// Token: 0x06008450 RID: 33872 RVA: 0x0030231F File Offset: 0x0030051F
		public Mode(Conversation.ModeType type, string voice, string icon, string mouth, string anim, bool newTopic = false)
		{
			this.type = type;
			this.voice = voice;
			this.mouth = mouth;
			this.anim = anim;
			this.icon = icon;
			this.newTopic = newTopic;
		}

		// Token: 0x0400652E RID: 25902
		public Conversation.ModeType type;

		// Token: 0x0400652F RID: 25903
		public string voice;

		// Token: 0x04006530 RID: 25904
		public string mouth;

		// Token: 0x04006531 RID: 25905
		public string anim;

		// Token: 0x04006532 RID: 25906
		public string icon;

		// Token: 0x04006533 RID: 25907
		public bool newTopic;
	}

	// Token: 0x02001452 RID: 5202
	public class Topic
	{
		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06008451 RID: 33873 RVA: 0x00302354 File Offset: 0x00300554
		public static Dictionary<int, Conversation.Mode> Modes
		{
			get
			{
				if (Conversation.Topic._modes == null)
				{
					Conversation.Topic._modes = new Dictionary<int, Conversation.Mode>();
					foreach (Conversation.Mode mode in Conversation.Topic.modeList)
					{
						Conversation.Topic._modes[(int)mode.type] = mode;
					}
				}
				return Conversation.Topic._modes;
			}
		}

		// Token: 0x06008452 RID: 33874 RVA: 0x003023C8 File Offset: 0x003005C8
		public Topic(string topic, Conversation.ModeType mode)
		{
			this.topic = topic;
			this.mode = mode;
		}

		// Token: 0x04006534 RID: 25908
		public static List<Conversation.Mode> modeList = new List<Conversation.Mode>
		{
			new Conversation.Mode(Conversation.ModeType.Query, "conversation_question", "mode_query", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Statement, "conversation_answer", "mode_statement", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Agreement, "conversation_answer", "mode_agreement", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Disagreement, "conversation_answer", "mode_disagreement", SpeechMonitor.PREFIX_SAD, "unhappy", false),
			new Conversation.Mode(Conversation.ModeType.Musing, "conversation_short", "mode_musing", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Satisfaction, "conversation_short", "mode_satisfaction", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Nominal, "conversation_short", "mode_nominal", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Dissatisfaction, "conversation_short", "mode_dissatisfaction", SpeechMonitor.PREFIX_SAD, "unhappy", false),
			new Conversation.Mode(Conversation.ModeType.Stressing, "conversation_short", "mode_stressing", SpeechMonitor.PREFIX_SAD, "unhappy", false),
			new Conversation.Mode(Conversation.ModeType.Segue, "conversation_question", "mode_segue", SpeechMonitor.PREFIX_HAPPY, "happy", true)
		};

		// Token: 0x04006535 RID: 25909
		private static Dictionary<int, Conversation.Mode> _modes;

		// Token: 0x04006536 RID: 25910
		public string topic;

		// Token: 0x04006537 RID: 25911
		public Conversation.ModeType mode;
	}
}
