using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AD0 RID: 2768
public class CodexConfigurableConsumerRecipePanel : CodexWidget<CodexConfigurableConsumerRecipePanel>
{
	// Token: 0x06005537 RID: 21815 RVA: 0x001EF6B9 File Offset: 0x001ED8B9
	public CodexConfigurableConsumerRecipePanel(IConfigurableConsumerOption data)
	{
		this.data = data;
	}

	// Token: 0x06005538 RID: 21816 RVA: 0x001EF6C8 File Offset: 0x001ED8C8
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
		this.title = component.GetReference<LocText>("Title");
		this.result_description = component.GetReference<LocText>("ResultDescription");
		this.resultIcon = component.GetReference<Image>("ResultIcon");
		this.ingredient_original = component.GetReference<RectTransform>("IngredientPrefab").gameObject;
		this.ingredient_original.SetActive(false);
		CodexText codexText = new CodexText();
		LocText reference = this.ingredient_original.GetComponent<HierarchyReferences>().GetReference<LocText>("Name");
		codexText.ConfigureLabel(reference, textStyles);
		this.Clear();
		if (this.data != null)
		{
			this.title.text = this.data.GetName();
			this.result_description.text = this.data.GetDescription();
			this.result_description.color = Color.black;
			this.resultIcon.sprite = this.data.GetIcon();
			IConfigurableConsumerIngredient[] ingredients = this.data.GetIngredients();
			this._ingredientRows = new GameObject[ingredients.Length];
			for (int i = 0; i < this._ingredientRows.Length; i++)
			{
				this._ingredientRows[i] = this.CreateIngredientRow(ingredients[i]);
			}
		}
	}

	// Token: 0x06005539 RID: 21817 RVA: 0x001EF7F4 File Offset: 0x001ED9F4
	public GameObject CreateIngredientRow(IConfigurableConsumerIngredient ingredient)
	{
		Tag[] idsets = ingredient.GetIDSets();
		if (this.ingredient_original != null && idsets.Length != 0)
		{
			GameObject gameObject = Util.KInstantiateUI(this.ingredient_original, this.ingredient_original.transform.parent.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(idsets[0], "ui", false);
			component.GetReference<Image>("Icon").sprite = uisprite.first;
			component.GetReference<Image>("Icon").color = uisprite.second;
			component.GetReference<LocText>("Name").text = idsets[0].ProperName();
			component.GetReference<LocText>("Amount").text = GameUtil.GetFormattedByTag(idsets[0], ingredient.GetAmount(), GameUtil.TimeSlice.None);
			component.GetReference<LocText>("Amount").color = Color.black;
			return gameObject;
		}
		return null;
	}

	// Token: 0x0600553A RID: 21818 RVA: 0x001EF8E0 File Offset: 0x001EDAE0
	public void Clear()
	{
		if (this._ingredientRows != null)
		{
			for (int i = 0; i < this._ingredientRows.Length; i++)
			{
				UnityEngine.Object.Destroy(this._ingredientRows[i]);
			}
			this._ingredientRows = null;
		}
	}

	// Token: 0x040038CB RID: 14539
	private LocText title;

	// Token: 0x040038CC RID: 14540
	private LocText result_description;

	// Token: 0x040038CD RID: 14541
	private Image resultIcon;

	// Token: 0x040038CE RID: 14542
	private GameObject ingredient_original;

	// Token: 0x040038CF RID: 14543
	private IConfigurableConsumerOption data;

	// Token: 0x040038D0 RID: 14544
	private GameObject[] _ingredientRows;
}
