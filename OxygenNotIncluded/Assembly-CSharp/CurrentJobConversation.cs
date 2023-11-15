using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006FF RID: 1791
public class CurrentJobConversation : ConversationType
{
	// Token: 0x0600313D RID: 12605 RVA: 0x00105A79 File Offset: 0x00103C79
	public CurrentJobConversation()
	{
		this.id = "CurrentJobConversation";
	}

	// Token: 0x0600313E RID: 12606 RVA: 0x00105A8C File Offset: 0x00103C8C
	public override void NewTarget(MinionIdentity speaker)
	{
		this.target = "hows_role";
	}

	// Token: 0x0600313F RID: 12607 RVA: 0x00105A9C File Offset: 0x00103C9C
	public override Conversation.Topic GetNextTopic(MinionIdentity speaker, Conversation.Topic lastTopic)
	{
		if (lastTopic == null)
		{
			return new Conversation.Topic(this.target, Conversation.ModeType.Query);
		}
		List<Conversation.ModeType> list = CurrentJobConversation.transitions[lastTopic.mode];
		Conversation.ModeType modeType = list[UnityEngine.Random.Range(0, list.Count)];
		if (modeType == Conversation.ModeType.Statement)
		{
			this.target = this.GetRoleForSpeaker(speaker);
			Conversation.ModeType modeForRole = this.GetModeForRole(speaker, this.target);
			return new Conversation.Topic(this.target, modeForRole);
		}
		return new Conversation.Topic(this.target, modeType);
	}

	// Token: 0x06003140 RID: 12608 RVA: 0x00105B18 File Offset: 0x00103D18
	public override Sprite GetSprite(string topic)
	{
		if (topic == "hows_role")
		{
			return Assets.GetSprite("crew_state_role");
		}
		if (Db.Get().Skills.TryGet(topic) != null)
		{
			return Assets.GetSprite(Db.Get().Skills.Get(topic).hat);
		}
		return null;
	}

	// Token: 0x06003141 RID: 12609 RVA: 0x00105B75 File Offset: 0x00103D75
	private Conversation.ModeType GetModeForRole(MinionIdentity speaker, string roleId)
	{
		return Conversation.ModeType.Nominal;
	}

	// Token: 0x06003142 RID: 12610 RVA: 0x00105B78 File Offset: 0x00103D78
	private string GetRoleForSpeaker(MinionIdentity speaker)
	{
		return speaker.GetComponent<MinionResume>().CurrentRole;
	}

	// Token: 0x04001D99 RID: 7577
	public static Dictionary<Conversation.ModeType, List<Conversation.ModeType>> transitions = new Dictionary<Conversation.ModeType, List<Conversation.ModeType>>
	{
		{
			Conversation.ModeType.Query,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Statement
			}
		},
		{
			Conversation.ModeType.Satisfaction,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Agreement
			}
		},
		{
			Conversation.ModeType.Nominal,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Musing
			}
		},
		{
			Conversation.ModeType.Dissatisfaction,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Disagreement
			}
		},
		{
			Conversation.ModeType.Stressing,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Disagreement
			}
		},
		{
			Conversation.ModeType.Agreement,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Query,
				Conversation.ModeType.End
			}
		},
		{
			Conversation.ModeType.Disagreement,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Query,
				Conversation.ModeType.End
			}
		},
		{
			Conversation.ModeType.Musing,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Query,
				Conversation.ModeType.End
			}
		}
	};
}
