using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000356 RID: 854
public class StickerBombConfig : IEntityConfig
{
	// Token: 0x0600117B RID: 4475 RVA: 0x0005E11A File Offset: 0x0005C31A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600117C RID: 4476 RVA: 0x0005E124 File Offset: 0x0005C324
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateBasicEntity("StickerBomb", STRINGS.BUILDINGS.PREFABS.STICKERBOMB.NAME, STRINGS.BUILDINGS.PREFABS.STICKERBOMB.DESC, 1f, true, Assets.GetAnim("sticker_a_kanim"), "off", Grid.SceneLayer.Backwall, SimHashes.Creature, null, 293f);
		EntityTemplates.AddCollision(gameObject, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f);
		gameObject.AddOrGet<StickerBomb>();
		return gameObject;
	}

	// Token: 0x0600117D RID: 4477 RVA: 0x0005E18E File Offset: 0x0005C38E
	public void OnPrefabInit(GameObject inst)
	{
		inst.AddOrGet<OccupyArea>().SetCellOffsets(new CellOffset[1]);
		inst.AddComponent<Modifiers>();
		inst.AddOrGet<DecorProvider>().SetValues(DECOR.BONUS.TIER2);
	}

	// Token: 0x0600117E RID: 4478 RVA: 0x0005E1B8 File Offset: 0x0005C3B8
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000987 RID: 2439
	public const string ID = "StickerBomb";
}
