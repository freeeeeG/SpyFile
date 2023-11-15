using System;
using UnityEngine;

// Token: 0x02000287 RID: 647
public class OneshotReactableLocator : IEntityConfig
{
	// Token: 0x06000D1B RID: 3355 RVA: 0x00048D48 File Offset: 0x00046F48
	public static EmoteReactable CreateOneshotReactable(GameObject source, float lifetime, string id, ChoreType chore_type, int range_width = 15, int range_height = 15, float min_reactor_time = 20f)
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(OneshotReactableLocator.ID), source.transform.GetPosition());
		EmoteReactable emoteReactable = new EmoteReactable(gameObject, id, chore_type, range_width, range_height, 100000f, min_reactor_time, float.PositiveInfinity, 0f);
		emoteReactable.AddPrecondition(OneshotReactableLocator.ReactorIsNotSource(source));
		OneshotReactableHost component = gameObject.GetComponent<OneshotReactableHost>();
		component.lifetime = lifetime;
		component.SetReactable(emoteReactable);
		gameObject.SetActive(true);
		return emoteReactable;
	}

	// Token: 0x06000D1C RID: 3356 RVA: 0x00048DBE File Offset: 0x00046FBE
	private static Reactable.ReactablePrecondition ReactorIsNotSource(GameObject source)
	{
		return (GameObject reactor, Navigator.ActiveTransition transition) => reactor != source;
	}

	// Token: 0x06000D1D RID: 3357 RVA: 0x00048DD7 File Offset: 0x00046FD7
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D1E RID: 3358 RVA: 0x00048DDE File Offset: 0x00046FDE
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(OneshotReactableLocator.ID, OneshotReactableLocator.ID, false);
		gameObject.AddTag(GameTags.NotConversationTopic);
		gameObject.AddOrGet<OneshotReactableHost>();
		return gameObject;
	}

	// Token: 0x06000D1F RID: 3359 RVA: 0x00048E02 File Offset: 0x00047002
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000D20 RID: 3360 RVA: 0x00048E04 File Offset: 0x00047004
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000796 RID: 1942
	public static readonly string ID = "OneshotReactableLocator";
}
