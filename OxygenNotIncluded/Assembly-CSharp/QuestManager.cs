using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020008FC RID: 2300
[SerializationConfig(MemberSerialization.OptIn)]
public class QuestManager : KMonoBehaviour
{
	// Token: 0x060042B3 RID: 17075 RVA: 0x00175688 File Offset: 0x00173888
	protected override void OnPrefabInit()
	{
		if (QuestManager.instance != null)
		{
			UnityEngine.Object.Destroy(QuestManager.instance);
			return;
		}
		QuestManager.instance = this;
		base.OnPrefabInit();
	}

	// Token: 0x060042B4 RID: 17076 RVA: 0x001756B0 File Offset: 0x001738B0
	public static QuestInstance InitializeQuest(Tag ownerId, Quest quest)
	{
		QuestInstance questInstance;
		if (!QuestManager.TryGetQuest(ownerId.GetHash(), quest, out questInstance))
		{
			questInstance = (QuestManager.instance.ownerToQuests[ownerId.GetHash()][quest.IdHash] = new QuestInstance(quest));
		}
		questInstance.Initialize(quest);
		return questInstance;
	}

	// Token: 0x060042B5 RID: 17077 RVA: 0x00175704 File Offset: 0x00173904
	public static QuestInstance InitializeQuest(HashedString ownerId, Quest quest)
	{
		QuestInstance questInstance;
		if (!QuestManager.TryGetQuest(ownerId.HashValue, quest, out questInstance))
		{
			questInstance = (QuestManager.instance.ownerToQuests[ownerId.HashValue][quest.IdHash] = new QuestInstance(quest));
		}
		questInstance.Initialize(quest);
		return questInstance;
	}

	// Token: 0x060042B6 RID: 17078 RVA: 0x00175758 File Offset: 0x00173958
	public static QuestInstance GetInstance(Tag ownerId, Quest quest)
	{
		QuestInstance result;
		QuestManager.TryGetQuest(ownerId.GetHash(), quest, out result);
		return result;
	}

	// Token: 0x060042B7 RID: 17079 RVA: 0x00175778 File Offset: 0x00173978
	public static QuestInstance GetInstance(HashedString ownerId, Quest quest)
	{
		QuestInstance result;
		QuestManager.TryGetQuest(ownerId.HashValue, quest, out result);
		return result;
	}

	// Token: 0x060042B8 RID: 17080 RVA: 0x00175798 File Offset: 0x00173998
	public static bool CheckState(HashedString ownerId, Quest quest, Quest.State state)
	{
		QuestInstance questInstance;
		QuestManager.TryGetQuest(ownerId.HashValue, quest, out questInstance);
		return questInstance != null && questInstance.CurrentState == state;
	}

	// Token: 0x060042B9 RID: 17081 RVA: 0x001757C4 File Offset: 0x001739C4
	public static bool CheckState(Tag ownerId, Quest quest, Quest.State state)
	{
		QuestInstance questInstance;
		QuestManager.TryGetQuest(ownerId.GetHash(), quest, out questInstance);
		return questInstance != null && questInstance.CurrentState == state;
	}

	// Token: 0x060042BA RID: 17082 RVA: 0x001757F0 File Offset: 0x001739F0
	private static bool TryGetQuest(int ownerId, Quest quest, out QuestInstance qInst)
	{
		qInst = null;
		Dictionary<HashedString, QuestInstance> dictionary;
		if (!QuestManager.instance.ownerToQuests.TryGetValue(ownerId, out dictionary))
		{
			dictionary = (QuestManager.instance.ownerToQuests[ownerId] = new Dictionary<HashedString, QuestInstance>());
		}
		return dictionary.TryGetValue(quest.IdHash, out qInst);
	}

	// Token: 0x04002B7E RID: 11134
	private static QuestManager instance;

	// Token: 0x04002B7F RID: 11135
	[Serialize]
	private Dictionary<int, Dictionary<HashedString, QuestInstance>> ownerToQuests = new Dictionary<int, Dictionary<HashedString, QuestInstance>>();
}
