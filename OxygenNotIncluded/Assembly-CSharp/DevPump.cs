using System;
using UnityEngine;

// Token: 0x020005EE RID: 1518
public class DevPump : Filterable, ISim1000ms
{
	// Token: 0x060025DF RID: 9695 RVA: 0x000CDC8C File Offset: 0x000CBE8C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.elementState == Filterable.ElementState.Liquid)
		{
			base.SelectedTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
			return;
		}
		if (this.elementState == Filterable.ElementState.Gas)
		{
			base.SelectedTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
		}
	}

	// Token: 0x060025E0 RID: 9696 RVA: 0x000CDCDC File Offset: 0x000CBEDC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.filterElementState = this.elementState;
	}

	// Token: 0x060025E1 RID: 9697 RVA: 0x000CDCF0 File Offset: 0x000CBEF0
	public void Sim1000ms(float dt)
	{
		if (!base.SelectedTag.IsValid)
		{
			return;
		}
		float num = 10f - this.storage.GetAmountAvailable(base.SelectedTag);
		if (num <= 0f)
		{
			return;
		}
		Element element = ElementLoader.GetElement(base.SelectedTag);
		GameObject gameObject = Assets.TryGetPrefab(base.SelectedTag);
		if (element != null)
		{
			this.storage.AddElement(element.id, num, element.defaultValues.temperature, byte.MaxValue, 0, false, false);
			return;
		}
		if (gameObject != null)
		{
			Grid.SceneLayer sceneLayer = gameObject.GetComponent<KBatchedAnimController>().sceneLayer;
			GameObject gameObject2 = GameUtil.KInstantiate(gameObject, sceneLayer, null, 0);
			gameObject2.GetComponent<PrimaryElement>().Units = num;
			gameObject2.SetActive(true);
			this.storage.Store(gameObject2, true, false, true, false);
		}
	}

	// Token: 0x040015A0 RID: 5536
	public Filterable.ElementState elementState = Filterable.ElementState.Liquid;

	// Token: 0x040015A1 RID: 5537
	[MyCmpReq]
	private Storage storage;
}
