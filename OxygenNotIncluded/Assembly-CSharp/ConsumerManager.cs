using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020006FB RID: 1787
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ConsumerManager")]
public class ConsumerManager : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06003123 RID: 12579 RVA: 0x00104D76 File Offset: 0x00102F76
	public static void DestroyInstance()
	{
		ConsumerManager.instance = null;
	}

	// Token: 0x14000014 RID: 20
	// (add) Token: 0x06003124 RID: 12580 RVA: 0x00104D80 File Offset: 0x00102F80
	// (remove) Token: 0x06003125 RID: 12581 RVA: 0x00104DB8 File Offset: 0x00102FB8
	public event Action<Tag> OnDiscover;

	// Token: 0x17000371 RID: 881
	// (get) Token: 0x06003126 RID: 12582 RVA: 0x00104DED File Offset: 0x00102FED
	public List<Tag> DefaultForbiddenTagsList
	{
		get
		{
			return this.defaultForbiddenTagsList;
		}
	}

	// Token: 0x06003127 RID: 12583 RVA: 0x00104DF8 File Offset: 0x00102FF8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ConsumerManager.instance = this;
		this.RefreshDiscovered(null);
		DiscoveredResources.Instance.OnDiscover += this.OnWorldInventoryDiscover;
		Game.Instance.Subscribe(-107300940, new Action<object>(this.RefreshDiscovered));
	}

	// Token: 0x06003128 RID: 12584 RVA: 0x00104E4A File Offset: 0x0010304A
	public bool isDiscovered(Tag id)
	{
		return !this.undiscoveredConsumableTags.Contains(id);
	}

	// Token: 0x06003129 RID: 12585 RVA: 0x00104E5B File Offset: 0x0010305B
	private void OnWorldInventoryDiscover(Tag category_tag, Tag tag)
	{
		if (this.undiscoveredConsumableTags.Contains(tag))
		{
			this.RefreshDiscovered(null);
		}
	}

	// Token: 0x0600312A RID: 12586 RVA: 0x00104E74 File Offset: 0x00103074
	public void RefreshDiscovered(object data = null)
	{
		foreach (EdiblesManager.FoodInfo foodInfo in EdiblesManager.GetAllFoodTypes())
		{
			if (!this.ShouldBeDiscovered(foodInfo.Id.ToTag()) && !this.undiscoveredConsumableTags.Contains(foodInfo.Id.ToTag()))
			{
				this.undiscoveredConsumableTags.Add(foodInfo.Id.ToTag());
				if (this.OnDiscover != null)
				{
					this.OnDiscover("UndiscoveredSomething".ToTag());
				}
			}
			else if (this.undiscoveredConsumableTags.Contains(foodInfo.Id.ToTag()) && this.ShouldBeDiscovered(foodInfo.Id.ToTag()))
			{
				this.undiscoveredConsumableTags.Remove(foodInfo.Id.ToTag());
				if (this.OnDiscover != null)
				{
					this.OnDiscover(foodInfo.Id.ToTag());
				}
				if (!DiscoveredResources.Instance.IsDiscovered(foodInfo.Id.ToTag()))
				{
					if (foodInfo.CaloriesPerUnit == 0f)
					{
						DiscoveredResources.Instance.Discover(foodInfo.Id.ToTag(), GameTags.CookingIngredient);
					}
					else
					{
						DiscoveredResources.Instance.Discover(foodInfo.Id.ToTag(), GameTags.Edible);
					}
				}
			}
		}
	}

	// Token: 0x0600312B RID: 12587 RVA: 0x00104FF8 File Offset: 0x001031F8
	private bool ShouldBeDiscovered(Tag food_id)
	{
		if (DiscoveredResources.Instance.IsDiscovered(food_id))
		{
			return true;
		}
		foreach (Recipe recipe in RecipeManager.Get().recipes)
		{
			if (recipe.Result == food_id)
			{
				foreach (string id in recipe.fabricators)
				{
					if (Db.Get().TechItems.IsTechItemComplete(id))
					{
						return true;
					}
				}
			}
		}
		foreach (Crop crop in Components.Crops.Items)
		{
			if (Grid.IsVisible(Grid.PosToCell(crop.gameObject)) && crop.cropId == food_id.Name)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04001D88 RID: 7560
	public static ConsumerManager instance;

	// Token: 0x04001D8A RID: 7562
	[Serialize]
	private List<Tag> undiscoveredConsumableTags = new List<Tag>();

	// Token: 0x04001D8B RID: 7563
	[Serialize]
	private List<Tag> defaultForbiddenTagsList = new List<Tag>();
}
