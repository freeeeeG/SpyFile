using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020001DC RID: 476
public class DreamJournalConfig : IEntityConfig
{
	// Token: 0x0600097E RID: 2430 RVA: 0x00037C93 File Offset: 0x00035E93
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x00037C9A File Offset: 0x00035E9A
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x00037C9C File Offset: 0x00035E9C
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000981 RID: 2433 RVA: 0x00037CA0 File Offset: 0x00035EA0
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dream_journal_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DreamJournalConfig.ID.Name, ITEMS.DREAMJOURNAL.NAME, ITEMS.DREAMJOURNAL.DESC, 1f, true, anim, "object", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.StoryTraitResource
		});
		gameObject.AddOrGet<EntitySplitter>().maxStackSize = 25f;
		return gameObject;
	}

	// Token: 0x040005F8 RID: 1528
	public static Tag ID = new Tag("DreamJournal");

	// Token: 0x040005F9 RID: 1529
	public const float MASS = 1f;

	// Token: 0x040005FA RID: 1530
	public const int FABRICATION_TIME_SECONDS = 300;

	// Token: 0x040005FB RID: 1531
	private const string ANIM_FILE = "dream_journal_kanim";

	// Token: 0x040005FC RID: 1532
	private const string INITIAL_ANIM = "object";

	// Token: 0x040005FD RID: 1533
	public const int MAX_STACK_SIZE = 25;
}
