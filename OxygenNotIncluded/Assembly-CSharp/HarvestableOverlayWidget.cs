using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B14 RID: 2836
[AddComponentMenu("KMonoBehaviour/scripts/HarvestableOverlayWidget")]
public class HarvestableOverlayWidget : KMonoBehaviour
{
	// Token: 0x06005773 RID: 22387 RVA: 0x002004AC File Offset: 0x001FE6AC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.condition_sprites.Add(WiltCondition.Condition.AtmosphereElement, this.sprite_atmosphere);
		this.condition_sprites.Add(WiltCondition.Condition.Darkness, this.sprite_light);
		this.condition_sprites.Add(WiltCondition.Condition.Drowning, this.sprite_liquid);
		this.condition_sprites.Add(WiltCondition.Condition.DryingOut, this.sprite_liquid);
		this.condition_sprites.Add(WiltCondition.Condition.Fertilized, this.sprite_resource);
		this.condition_sprites.Add(WiltCondition.Condition.IlluminationComfort, this.sprite_light);
		this.condition_sprites.Add(WiltCondition.Condition.Irrigation, this.sprite_liquid);
		this.condition_sprites.Add(WiltCondition.Condition.Pressure, this.sprite_pressure);
		this.condition_sprites.Add(WiltCondition.Condition.Temperature, this.sprite_temperature);
		this.condition_sprites.Add(WiltCondition.Condition.Receptacle, this.sprite_receptacle);
		for (int i = 0; i < this.horizontal_containers.Length; i++)
		{
			GameObject gameObject = Util.KInstantiateUI(this.horizontal_container_prefab, this.vertical_container, false);
			this.horizontal_containers[i] = gameObject;
		}
		for (int j = 0; j < 13; j++)
		{
			if (this.condition_sprites.ContainsKey((WiltCondition.Condition)j))
			{
				GameObject gameObject2 = Util.KInstantiateUI(this.icon_gameobject_prefab, base.gameObject, false);
				gameObject2.GetComponent<Image>().sprite = this.condition_sprites[(WiltCondition.Condition)j];
				this.condition_icons.Add((WiltCondition.Condition)j, gameObject2);
			}
		}
	}

	// Token: 0x06005774 RID: 22388 RVA: 0x002005F8 File Offset: 0x001FE7F8
	public void Refresh(HarvestDesignatable target_harvestable)
	{
		Image image = this.bar.GetComponent<HierarchyReferences>().GetReference("Fill") as Image;
		AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(target_harvestable);
		if (amountInstance != null)
		{
			float num = amountInstance.value / amountInstance.GetMax();
			image.rectTransform.offsetMin = new Vector2(image.rectTransform.offsetMin.x, 3f);
			if (this.bar.activeSelf != !target_harvestable.CanBeHarvested())
			{
				this.bar.SetActive(!target_harvestable.CanBeHarvested());
			}
			float num2 = target_harvestable.CanBeHarvested() ? 3f : (19f - 19f * num + 3f);
			image.rectTransform.offsetMax = new Vector2(image.rectTransform.offsetMax.x, -num2);
		}
		else if (this.bar.activeSelf)
		{
			this.bar.SetActive(false);
		}
		WiltCondition component = target_harvestable.GetComponent<WiltCondition>();
		if (component != null)
		{
			for (int i = 0; i < this.horizontal_containers.Length; i++)
			{
				this.horizontal_containers[i].SetActive(false);
			}
			foreach (KeyValuePair<WiltCondition.Condition, GameObject> keyValuePair in this.condition_icons)
			{
				keyValuePair.Value.SetActive(false);
			}
			if (!component.IsWilting())
			{
				this.vertical_container.SetActive(false);
				image.color = HarvestableOverlayWidget.growing_color;
				return;
			}
			this.vertical_container.SetActive(true);
			image.color = HarvestableOverlayWidget.wilting_color;
			List<WiltCondition.Condition> list = component.CurrentWiltSources();
			if (list.Count > 0)
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (this.condition_icons.ContainsKey(list[j]))
					{
						this.condition_icons[list[j]].SetActive(true);
						this.horizontal_containers[j / 2].SetActive(true);
						this.condition_icons[list[j]].transform.SetParent(this.horizontal_containers[j / 2].transform);
					}
				}
				return;
			}
		}
		else
		{
			image.color = HarvestableOverlayWidget.growing_color;
			this.vertical_container.SetActive(false);
		}
	}

	// Token: 0x04003B14 RID: 15124
	[SerializeField]
	private GameObject vertical_container;

	// Token: 0x04003B15 RID: 15125
	[SerializeField]
	private GameObject bar;

	// Token: 0x04003B16 RID: 15126
	private const int icons_per_row = 2;

	// Token: 0x04003B17 RID: 15127
	private const float bar_fill_range = 19f;

	// Token: 0x04003B18 RID: 15128
	private const float bar_fill_offset = 3f;

	// Token: 0x04003B19 RID: 15129
	private static Color growing_color = new Color(0.9843137f, 0.6901961f, 0.23137255f, 1f);

	// Token: 0x04003B1A RID: 15130
	private static Color wilting_color = new Color(0.5647059f, 0.5647059f, 0.5647059f, 1f);

	// Token: 0x04003B1B RID: 15131
	[SerializeField]
	private Sprite sprite_liquid;

	// Token: 0x04003B1C RID: 15132
	[SerializeField]
	private Sprite sprite_atmosphere;

	// Token: 0x04003B1D RID: 15133
	[SerializeField]
	private Sprite sprite_pressure;

	// Token: 0x04003B1E RID: 15134
	[SerializeField]
	private Sprite sprite_temperature;

	// Token: 0x04003B1F RID: 15135
	[SerializeField]
	private Sprite sprite_resource;

	// Token: 0x04003B20 RID: 15136
	[SerializeField]
	private Sprite sprite_light;

	// Token: 0x04003B21 RID: 15137
	[SerializeField]
	private Sprite sprite_receptacle;

	// Token: 0x04003B22 RID: 15138
	[SerializeField]
	private GameObject horizontal_container_prefab;

	// Token: 0x04003B23 RID: 15139
	private GameObject[] horizontal_containers = new GameObject[6];

	// Token: 0x04003B24 RID: 15140
	[SerializeField]
	private GameObject icon_gameobject_prefab;

	// Token: 0x04003B25 RID: 15141
	private Dictionary<WiltCondition.Condition, GameObject> condition_icons = new Dictionary<WiltCondition.Condition, GameObject>();

	// Token: 0x04003B26 RID: 15142
	private Dictionary<WiltCondition.Condition, Sprite> condition_sprites = new Dictionary<WiltCondition.Condition, Sprite>();
}
