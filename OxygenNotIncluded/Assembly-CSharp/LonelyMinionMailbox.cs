using System;
using UnityEngine;

// Token: 0x02000648 RID: 1608
public class LonelyMinionMailbox : KMonoBehaviour
{
	// Token: 0x06002A3D RID: 10813 RVA: 0x000E1A18 File Offset: 0x000DFC18
	public void Initialize(LonelyMinionHouse.Instance house)
	{
		this.House = house;
		SingleEntityReceptacle component = base.GetComponent<SingleEntityReceptacle>();
		component.occupyingObjectRelativePosition = base.transform.InverseTransformPoint(house.GetParcelPosition());
		component.occupyingObjectRelativePosition.z = -1f;
		StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.LonelyMinion.HashId);
		StoryInstance storyInstance2 = storyInstance;
		storyInstance2.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Combine(storyInstance2.StoryStateChanged, new Action<StoryInstance.State>(this.OnStoryStateChanged));
		this.OnStoryStateChanged(storyInstance.CurrentState);
	}

	// Token: 0x06002A3E RID: 10814 RVA: 0x000E1AA5 File Offset: 0x000DFCA5
	protected override void OnSpawn()
	{
		if (StoryManager.Instance.CheckState(StoryInstance.State.COMPLETE, Db.Get().Stories.LonelyMinion))
		{
			base.gameObject.AddOrGet<Deconstructable>().allowDeconstruction = true;
		}
	}

	// Token: 0x06002A3F RID: 10815 RVA: 0x000E1AD4 File Offset: 0x000DFCD4
	protected override void OnCleanUp()
	{
		StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.LonelyMinion.HashId);
		storyInstance.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Remove(storyInstance.StoryStateChanged, new Action<StoryInstance.State>(this.OnStoryStateChanged));
	}

	// Token: 0x06002A40 RID: 10816 RVA: 0x000E1B20 File Offset: 0x000DFD20
	private void OnStoryStateChanged(StoryInstance.State state)
	{
		QuestInstance quest = QuestManager.GetInstance(this.House.QuestOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
		if (state == StoryInstance.State.IN_PROGRESS)
		{
			base.Subscribe(-731304873, new Action<object>(this.OnStorageChanged));
			SingleEntityReceptacle singleEntityReceptacle = base.gameObject.AddOrGet<SingleEntityReceptacle>();
			singleEntityReceptacle.enabled = true;
			singleEntityReceptacle.AddAdditionalCriteria(delegate(GameObject candidate)
			{
				EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(candidate.GetComponent<KPrefabID>().PrefabTag.Name);
				int num = 0;
				return foodInfo != null && quest.DataSatisfiesCriteria(new Quest.ItemData
				{
					CriteriaId = LonelyMinionConfig.FoodCriteriaId,
					QualifyingTag = GameTags.Edible,
					CurrentValue = (float)foodInfo.Quality
				}, ref num);
			});
			RootMenu.Instance.Refresh();
			this.OnStorageChanged(singleEntityReceptacle.Occupant);
		}
		if (state == StoryInstance.State.COMPLETE)
		{
			base.Unsubscribe(-731304873, new Action<object>(this.OnStorageChanged));
			base.gameObject.AddOrGet<Deconstructable>().allowDeconstruction = true;
		}
	}

	// Token: 0x06002A41 RID: 10817 RVA: 0x000E1BDB File Offset: 0x000DFDDB
	private void OnStorageChanged(object data)
	{
		this.House.MailboxContentChanged(data as GameObject);
	}

	// Token: 0x040018B0 RID: 6320
	public LonelyMinionHouse.Instance House;
}
