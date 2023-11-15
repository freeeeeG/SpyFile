using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200061B RID: 1563
[AddComponentMenu("KMonoBehaviour/scripts/ItemPedestal")]
public class ItemPedestal : KMonoBehaviour
{
	// Token: 0x06002770 RID: 10096 RVA: 0x000D6090 File Offset: 0x000D4290
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<ItemPedestal>(-731304873, ItemPedestal.OnOccupantChangedDelegate);
		if (this.receptacle.Occupant)
		{
			KBatchedAnimController component = this.receptacle.Occupant.GetComponent<KBatchedAnimController>();
			if (component)
			{
				component.enabled = true;
				component.sceneLayer = Grid.SceneLayer.Move;
			}
			this.OnOccupantChanged(this.receptacle.Occupant);
		}
	}

	// Token: 0x06002771 RID: 10097 RVA: 0x000D6100 File Offset: 0x000D4300
	private void OnOccupantChanged(object data)
	{
		Attributes attributes = this.GetAttributes();
		if (this.decorModifier != null)
		{
			attributes.Remove(this.decorModifier);
			attributes.Remove(this.decorRadiusModifier);
			this.decorModifier = null;
			this.decorRadiusModifier = null;
		}
		if (data != null)
		{
			GameObject gameObject = (GameObject)data;
			UnityEngine.Object component = gameObject.GetComponent<DecorProvider>();
			float value = 5f;
			float value2 = 3f;
			if (component != null)
			{
				value = Mathf.Max(Db.Get().BuildingAttributes.Decor.Lookup(gameObject).GetTotalValue() * 2f, 5f);
				value2 = Db.Get().BuildingAttributes.DecorRadius.Lookup(gameObject).GetTotalValue() + 2f;
			}
			string description = string.Format(BUILDINGS.PREFABS.ITEMPEDESTAL.DISPLAYED_ITEM_FMT, gameObject.GetComponent<KPrefabID>().PrefabTag.ProperName());
			this.decorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, value, description, false, false, true);
			this.decorRadiusModifier = new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, value2, description, false, false, true);
			attributes.Add(this.decorModifier);
			attributes.Add(this.decorRadiusModifier);
		}
	}

	// Token: 0x040016C1 RID: 5825
	[MyCmpReq]
	protected SingleEntityReceptacle receptacle;

	// Token: 0x040016C2 RID: 5826
	[MyCmpReq]
	private DecorProvider decorProvider;

	// Token: 0x040016C3 RID: 5827
	private const float MINIMUM_DECOR = 5f;

	// Token: 0x040016C4 RID: 5828
	private const float STORED_DECOR_MODIFIER = 2f;

	// Token: 0x040016C5 RID: 5829
	private const int RADIUS_BONUS = 2;

	// Token: 0x040016C6 RID: 5830
	private AttributeModifier decorModifier;

	// Token: 0x040016C7 RID: 5831
	private AttributeModifier decorRadiusModifier;

	// Token: 0x040016C8 RID: 5832
	private static readonly EventSystem.IntraObjectHandler<ItemPedestal> OnOccupantChangedDelegate = new EventSystem.IntraObjectHandler<ItemPedestal>(delegate(ItemPedestal component, object data)
	{
		component.OnOccupantChanged(data);
	});
}
