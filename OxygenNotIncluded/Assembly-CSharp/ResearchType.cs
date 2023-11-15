using System;
using UnityEngine;

// Token: 0x0200092B RID: 2347
public class ResearchType
{
	// Token: 0x06004423 RID: 17443 RVA: 0x0017E6E9 File Offset: 0x0017C8E9
	public ResearchType(string id, string name, string description, Sprite sprite, Color color, Recipe.Ingredient[] fabricationIngredients, float fabricationTime, HashedString kAnim_ID, string[] fabricators, string recipeDescription)
	{
		this._id = id;
		this._name = name;
		this._description = description;
		this._sprite = sprite;
		this._color = color;
		this.CreatePrefab(fabricationIngredients, fabricationTime, kAnim_ID, fabricators, recipeDescription, color);
	}

	// Token: 0x06004424 RID: 17444 RVA: 0x0017E729 File Offset: 0x0017C929
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06004425 RID: 17445 RVA: 0x0017E730 File Offset: 0x0017C930
	public GameObject CreatePrefab(Recipe.Ingredient[] fabricationIngredients, float fabricationTime, HashedString kAnim_ID, string[] fabricators, string recipeDescription, Color color)
	{
		GameObject gameObject = EntityTemplates.CreateBasicEntity(this.id, this.name, this.description, 1f, true, Assets.GetAnim(kAnim_ID), "ui", Grid.SceneLayer.BuildingFront, SimHashes.Creature, null, 293f);
		gameObject.AddOrGet<ResearchPointObject>().TypeID = this.id;
		this._recipe = new Recipe(this.id, 1f, (SimHashes)0, this.name, recipeDescription, 0);
		this._recipe.SetFabricators(fabricators, fabricationTime);
		this._recipe.SetIcon(Assets.GetSprite("research_type_icon"), color);
		if (fabricationIngredients != null)
		{
			foreach (Recipe.Ingredient ingredient in fabricationIngredients)
			{
				this._recipe.AddIngredient(ingredient);
			}
		}
		return gameObject;
	}

	// Token: 0x06004426 RID: 17446 RVA: 0x0017E7F5 File Offset: 0x0017C9F5
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06004427 RID: 17447 RVA: 0x0017E7F7 File Offset: 0x0017C9F7
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x170004BB RID: 1211
	// (get) Token: 0x06004428 RID: 17448 RVA: 0x0017E7F9 File Offset: 0x0017C9F9
	public string id
	{
		get
		{
			return this._id;
		}
	}

	// Token: 0x170004BC RID: 1212
	// (get) Token: 0x06004429 RID: 17449 RVA: 0x0017E801 File Offset: 0x0017CA01
	public string name
	{
		get
		{
			return this._name;
		}
	}

	// Token: 0x170004BD RID: 1213
	// (get) Token: 0x0600442A RID: 17450 RVA: 0x0017E809 File Offset: 0x0017CA09
	public string description
	{
		get
		{
			return this._description;
		}
	}

	// Token: 0x170004BE RID: 1214
	// (get) Token: 0x0600442B RID: 17451 RVA: 0x0017E811 File Offset: 0x0017CA11
	public string recipe
	{
		get
		{
			return this.recipe;
		}
	}

	// Token: 0x170004BF RID: 1215
	// (get) Token: 0x0600442C RID: 17452 RVA: 0x0017E819 File Offset: 0x0017CA19
	public Color color
	{
		get
		{
			return this._color;
		}
	}

	// Token: 0x170004C0 RID: 1216
	// (get) Token: 0x0600442D RID: 17453 RVA: 0x0017E821 File Offset: 0x0017CA21
	public Sprite sprite
	{
		get
		{
			return this._sprite;
		}
	}

	// Token: 0x04002D2F RID: 11567
	private string _id;

	// Token: 0x04002D30 RID: 11568
	private string _name;

	// Token: 0x04002D31 RID: 11569
	private string _description;

	// Token: 0x04002D32 RID: 11570
	private Recipe _recipe;

	// Token: 0x04002D33 RID: 11571
	private Sprite _sprite;

	// Token: 0x04002D34 RID: 11572
	private Color _color;
}
