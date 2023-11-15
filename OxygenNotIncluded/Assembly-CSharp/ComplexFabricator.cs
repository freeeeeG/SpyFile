using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005CE RID: 1486
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ComplexFabricator")]
public class ComplexFabricator : KMonoBehaviour, ISim200ms, ISim1000ms
{
	// Token: 0x170001CF RID: 463
	// (get) Token: 0x060024A9 RID: 9385 RVA: 0x000C83B2 File Offset: 0x000C65B2
	public ComplexFabricatorWorkable Workable
	{
		get
		{
			return this.workable;
		}
	}

	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x060024AA RID: 9386 RVA: 0x000C83BA File Offset: 0x000C65BA
	// (set) Token: 0x060024AB RID: 9387 RVA: 0x000C83C2 File Offset: 0x000C65C2
	public bool ForbidMutantSeeds
	{
		get
		{
			return this.forbidMutantSeeds;
		}
		set
		{
			this.forbidMutantSeeds = value;
			this.ToggleMutantSeedFetches();
			this.UpdateMutantSeedStatusItem();
		}
	}

	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x060024AC RID: 9388 RVA: 0x000C83D7 File Offset: 0x000C65D7
	public Tag[] ForbiddenTags
	{
		get
		{
			if (!this.forbidMutantSeeds)
			{
				return null;
			}
			return this.forbiddenMutantTags;
		}
	}

	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x060024AD RID: 9389 RVA: 0x000C83E9 File Offset: 0x000C65E9
	public int CurrentOrderIdx
	{
		get
		{
			return this.nextOrderIdx;
		}
	}

	// Token: 0x170001D3 RID: 467
	// (get) Token: 0x060024AE RID: 9390 RVA: 0x000C83F1 File Offset: 0x000C65F1
	public ComplexRecipe CurrentWorkingOrder
	{
		get
		{
			if (!this.HasWorkingOrder)
			{
				return null;
			}
			return this.recipe_list[this.workingOrderIdx];
		}
	}

	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x060024AF RID: 9391 RVA: 0x000C840A File Offset: 0x000C660A
	public ComplexRecipe NextOrder
	{
		get
		{
			if (!this.nextOrderIsWorkable)
			{
				return null;
			}
			return this.recipe_list[this.nextOrderIdx];
		}
	}

	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x060024B0 RID: 9392 RVA: 0x000C8423 File Offset: 0x000C6623
	// (set) Token: 0x060024B1 RID: 9393 RVA: 0x000C842B File Offset: 0x000C662B
	public float OrderProgress
	{
		get
		{
			return this.orderProgress;
		}
		set
		{
			this.orderProgress = value;
		}
	}

	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x060024B2 RID: 9394 RVA: 0x000C8434 File Offset: 0x000C6634
	public bool HasAnyOrder
	{
		get
		{
			return this.HasWorkingOrder || this.hasOpenOrders;
		}
	}

	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x060024B3 RID: 9395 RVA: 0x000C8446 File Offset: 0x000C6646
	public bool HasWorker
	{
		get
		{
			return !this.duplicantOperated || this.workable.worker != null;
		}
	}

	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x060024B4 RID: 9396 RVA: 0x000C8463 File Offset: 0x000C6663
	public bool WaitingForWorker
	{
		get
		{
			return this.HasWorkingOrder && !this.HasWorker;
		}
	}

	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x060024B5 RID: 9397 RVA: 0x000C8478 File Offset: 0x000C6678
	private bool HasWorkingOrder
	{
		get
		{
			return this.workingOrderIdx > -1;
		}
	}

	// Token: 0x170001DA RID: 474
	// (get) Token: 0x060024B6 RID: 9398 RVA: 0x000C8483 File Offset: 0x000C6683
	public List<FetchList2> DebugFetchLists
	{
		get
		{
			return this.fetchListList;
		}
	}

	// Token: 0x060024B7 RID: 9399 RVA: 0x000C848C File Offset: 0x000C668C
	[OnDeserialized]
	protected virtual void OnDeserializedMethod()
	{
		List<string> list = new List<string>();
		foreach (string text in this.recipeQueueCounts.Keys)
		{
			if (ComplexRecipeManager.Get().GetRecipe(text) == null)
			{
				list.Add(text);
			}
		}
		foreach (string text2 in list)
		{
			global::Debug.LogWarningFormat("{1} removing missing recipe from queue: {0}", new object[]
			{
				text2,
				base.name
			});
			this.recipeQueueCounts.Remove(text2);
		}
	}

	// Token: 0x060024B8 RID: 9400 RVA: 0x000C855C File Offset: 0x000C675C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.GetRecipes();
		this.simRenderLoadBalance = true;
		this.choreType = Db.Get().ChoreTypes.Fabricate;
		base.Subscribe<ComplexFabricator>(-1957399615, ComplexFabricator.OnDroppedAllDelegate);
		base.Subscribe<ComplexFabricator>(-592767678, ComplexFabricator.OnOperationalChangedDelegate);
		base.Subscribe<ComplexFabricator>(-905833192, ComplexFabricator.OnCopySettingsDelegate);
		base.Subscribe<ComplexFabricator>(-1697596308, ComplexFabricator.OnStorageChangeDelegate);
		base.Subscribe<ComplexFabricator>(-1837862626, ComplexFabricator.OnParticleStorageChangedDelegate);
		this.workable = base.GetComponent<ComplexFabricatorWorkable>();
		Components.ComplexFabricators.Add(this);
		base.Subscribe<ComplexFabricator>(493375141, ComplexFabricator.OnRefreshUserMenuDelegate);
	}

	// Token: 0x060024B9 RID: 9401 RVA: 0x000C8610 File Offset: 0x000C6810
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.InitRecipeQueueCount();
		foreach (string key in this.recipeQueueCounts.Keys)
		{
			if (this.recipeQueueCounts[key] == 100)
			{
				this.recipeQueueCounts[key] = ComplexFabricator.QUEUE_INFINITE;
			}
		}
		this.buildStorage.Transfer(this.inStorage, true, true);
		this.DropExcessIngredients(this.inStorage);
		int num = this.FindRecipeIndex(this.lastWorkingRecipe);
		if (num > -1)
		{
			this.nextOrderIdx = num;
		}
		this.UpdateMutantSeedStatusItem();
	}

	// Token: 0x060024BA RID: 9402 RVA: 0x000C86CC File Offset: 0x000C68CC
	protected override void OnCleanUp()
	{
		this.CancelAllOpenOrders();
		this.CancelChore();
		Components.ComplexFabricators.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060024BB RID: 9403 RVA: 0x000C86EC File Offset: 0x000C68EC
	private void OnRefreshUserMenu(object data)
	{
		if (DlcManager.IsContentActive("EXPANSION1_ID") && this.HasRecipiesWithSeeds())
		{
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_switch_toggle", this.ForbidMutantSeeds ? UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.ACCEPT : UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.REJECT, delegate()
			{
				this.ForbidMutantSeeds = !this.ForbidMutantSeeds;
				this.OnRefreshUserMenu(null);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.TOOLTIP, true), 1f);
		}
	}

	// Token: 0x060024BC RID: 9404 RVA: 0x000C876C File Offset: 0x000C696C
	private bool HasRecipiesWithSeeds()
	{
		bool result = false;
		ComplexRecipe[] array = this.recipe_list;
		for (int i = 0; i < array.Length; i++)
		{
			ComplexRecipe.RecipeElement[] ingredients = array[i].ingredients;
			for (int j = 0; j < ingredients.Length; j++)
			{
				GameObject prefab = Assets.GetPrefab(ingredients[j].material);
				if (prefab != null && prefab.GetComponent<PlantableSeed>() != null)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x060024BD RID: 9405 RVA: 0x000C87DC File Offset: 0x000C69DC
	private void UpdateMutantSeedStatusItem()
	{
		base.gameObject.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.FabricatorAcceptsMutantSeeds, DlcManager.IsContentActive("EXPANSION1_ID") && this.HasRecipiesWithSeeds() && !this.forbidMutantSeeds, null);
	}

	// Token: 0x060024BE RID: 9406 RVA: 0x000C882D File Offset: 0x000C6A2D
	private void OnOperationalChanged(object data)
	{
		if ((bool)data)
		{
			this.queueDirty = true;
		}
		else
		{
			this.CancelAllOpenOrders();
		}
		this.UpdateChore();
	}

	// Token: 0x060024BF RID: 9407 RVA: 0x000C884C File Offset: 0x000C6A4C
	public void Sim1000ms(float dt)
	{
		this.RefreshAndStartNextOrder();
		if (this.materialNeedCache.Count > 0 && this.fetchListList.Count == 0)
		{
			global::Debug.LogWarningFormat(base.gameObject, "{0} has material needs cached, but no open fetches. materialNeedCache={1}, fetchListList={2}", new object[]
			{
				base.gameObject,
				this.materialNeedCache.Count,
				this.fetchListList.Count
			});
			this.queueDirty = true;
		}
	}

	// Token: 0x060024C0 RID: 9408 RVA: 0x000C88C8 File Offset: 0x000C6AC8
	public void Sim200ms(float dt)
	{
		if (!this.operational.IsOperational)
		{
			return;
		}
		this.operational.SetActive(this.HasWorkingOrder && this.HasWorker, false);
		if (!this.duplicantOperated && this.HasWorkingOrder)
		{
			ComplexRecipe complexRecipe = this.recipe_list[this.workingOrderIdx];
			this.orderProgress += dt / complexRecipe.time;
			if (this.orderProgress >= 1f)
			{
				this.CompleteWorkingOrder();
			}
		}
	}

	// Token: 0x060024C1 RID: 9409 RVA: 0x000C8946 File Offset: 0x000C6B46
	private void RefreshAndStartNextOrder()
	{
		if (!this.operational.IsOperational)
		{
			return;
		}
		if (this.queueDirty)
		{
			this.RefreshQueue();
		}
		if (!this.HasWorkingOrder && this.nextOrderIsWorkable)
		{
			this.StartWorkingOrder(this.nextOrderIdx);
		}
	}

	// Token: 0x060024C2 RID: 9410 RVA: 0x000C8980 File Offset: 0x000C6B80
	public void SetQueueDirty()
	{
		this.queueDirty = true;
	}

	// Token: 0x060024C3 RID: 9411 RVA: 0x000C8989 File Offset: 0x000C6B89
	private void RefreshQueue()
	{
		this.queueDirty = false;
		this.ValidateWorkingOrder();
		this.ValidateNextOrder();
		this.UpdateOpenOrders();
		this.DropExcessIngredients(this.inStorage);
		base.Trigger(1721324763, this);
	}

	// Token: 0x060024C4 RID: 9412 RVA: 0x000C89BC File Offset: 0x000C6BBC
	private void StartWorkingOrder(int index)
	{
		global::Debug.Assert(!this.HasWorkingOrder, "machineOrderIdx already set");
		this.workingOrderIdx = index;
		if (this.recipe_list[this.workingOrderIdx].id != this.lastWorkingRecipe)
		{
			this.orderProgress = 0f;
			this.lastWorkingRecipe = this.recipe_list[this.workingOrderIdx].id;
		}
		this.TransferCurrentRecipeIngredientsForBuild();
		global::Debug.Assert(this.openOrderCounts[this.workingOrderIdx] > 0, "openOrderCount invalid");
		List<int> list = this.openOrderCounts;
		int index2 = this.workingOrderIdx;
		int num = list[index2];
		list[index2] = num - 1;
		this.UpdateChore();
		this.AdvanceNextOrder();
	}

	// Token: 0x060024C5 RID: 9413 RVA: 0x000C8A73 File Offset: 0x000C6C73
	private void CancelWorkingOrder()
	{
		global::Debug.Assert(this.HasWorkingOrder, "machineOrderIdx not set");
		this.buildStorage.Transfer(this.inStorage, true, true);
		this.workingOrderIdx = -1;
		this.orderProgress = 0f;
		this.UpdateChore();
	}

	// Token: 0x060024C6 RID: 9414 RVA: 0x000C8AB0 File Offset: 0x000C6CB0
	public void CompleteWorkingOrder()
	{
		if (!this.HasWorkingOrder)
		{
			global::Debug.LogWarning("CompleteWorkingOrder called with no working order.", base.gameObject);
			return;
		}
		ComplexRecipe recipe = this.recipe_list[this.workingOrderIdx];
		this.SpawnOrderProduct(recipe);
		float num = this.buildStorage.MassStored();
		if (num != 0f)
		{
			global::Debug.LogWarningFormat(base.gameObject, "{0} build storage contains mass {1} after order completion.", new object[]
			{
				base.gameObject,
				num
			});
			this.buildStorage.Transfer(this.inStorage, true, true);
		}
		this.DecrementRecipeQueueCountInternal(recipe, true);
		this.workingOrderIdx = -1;
		this.orderProgress = 0f;
		this.CancelChore();
		if (!this.cancelling)
		{
			this.RefreshAndStartNextOrder();
		}
	}

	// Token: 0x060024C7 RID: 9415 RVA: 0x000C8B6C File Offset: 0x000C6D6C
	private void ValidateWorkingOrder()
	{
		if (!this.HasWorkingOrder)
		{
			return;
		}
		ComplexRecipe recipe = this.recipe_list[this.workingOrderIdx];
		if (!this.IsRecipeQueued(recipe))
		{
			this.CancelWorkingOrder();
		}
	}

	// Token: 0x060024C8 RID: 9416 RVA: 0x000C8BA0 File Offset: 0x000C6DA0
	private void UpdateChore()
	{
		if (!this.duplicantOperated)
		{
			return;
		}
		bool flag = this.operational.IsOperational && this.HasWorkingOrder;
		if (flag && this.chore == null)
		{
			this.CreateChore();
			return;
		}
		if (!flag && this.chore != null)
		{
			this.CancelChore();
		}
	}

	// Token: 0x060024C9 RID: 9417 RVA: 0x000C8BF0 File Offset: 0x000C6DF0
	private void AdvanceNextOrder()
	{
		for (int i = 0; i < this.recipe_list.Length; i++)
		{
			this.nextOrderIdx = (this.nextOrderIdx + 1) % this.recipe_list.Length;
			ComplexRecipe recipe = this.recipe_list[this.nextOrderIdx];
			this.nextOrderIsWorkable = (this.GetRemainingQueueCount(recipe) > 0 && this.HasIngredients(recipe, this.inStorage));
			if (this.nextOrderIsWorkable)
			{
				break;
			}
		}
	}

	// Token: 0x060024CA RID: 9418 RVA: 0x000C8C60 File Offset: 0x000C6E60
	private void ValidateNextOrder()
	{
		ComplexRecipe recipe = this.recipe_list[this.nextOrderIdx];
		this.nextOrderIsWorkable = (this.GetRemainingQueueCount(recipe) > 0 && this.HasIngredients(recipe, this.inStorage));
		if (!this.nextOrderIsWorkable)
		{
			this.AdvanceNextOrder();
		}
	}

	// Token: 0x060024CB RID: 9419 RVA: 0x000C8CAC File Offset: 0x000C6EAC
	private void CancelAllOpenOrders()
	{
		for (int i = 0; i < this.openOrderCounts.Count; i++)
		{
			this.openOrderCounts[i] = 0;
		}
		this.ClearMaterialNeeds();
		this.CancelFetches();
	}

	// Token: 0x060024CC RID: 9420 RVA: 0x000C8CE8 File Offset: 0x000C6EE8
	private void UpdateOpenOrders()
	{
		ComplexRecipe[] recipes = this.GetRecipes();
		if (recipes.Length != this.openOrderCounts.Count)
		{
			global::Debug.LogErrorFormat(base.gameObject, "Recipe count {0} doesn't match open order count {1}", new object[]
			{
				recipes.Length,
				this.openOrderCounts.Count
			});
		}
		bool flag = false;
		this.hasOpenOrders = false;
		for (int i = 0; i < recipes.Length; i++)
		{
			ComplexRecipe recipe = recipes[i];
			int recipePrefetchCount = this.GetRecipePrefetchCount(recipe);
			if (recipePrefetchCount > 0)
			{
				this.hasOpenOrders = true;
			}
			int num = this.openOrderCounts[i];
			if (num != recipePrefetchCount)
			{
				if (recipePrefetchCount < num)
				{
					flag = true;
				}
				this.openOrderCounts[i] = recipePrefetchCount;
			}
		}
		DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary pooledDictionary = DictionaryPool<Tag, float, ComplexFabricator>.Allocate();
		DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary pooledDictionary2 = DictionaryPool<Tag, float, ComplexFabricator>.Allocate();
		for (int j = 0; j < this.openOrderCounts.Count; j++)
		{
			if (this.openOrderCounts[j] > 0)
			{
				foreach (ComplexRecipe.RecipeElement recipeElement in this.recipe_list[j].ingredients)
				{
					pooledDictionary[recipeElement.material] = this.inStorage.GetAmountAvailable(recipeElement.material);
				}
			}
		}
		for (int l = 0; l < this.recipe_list.Length; l++)
		{
			int num2 = this.openOrderCounts[l];
			if (num2 > 0)
			{
				foreach (ComplexRecipe.RecipeElement recipeElement2 in this.recipe_list[l].ingredients)
				{
					float num3 = recipeElement2.amount * (float)num2;
					float num4 = num3 - pooledDictionary[recipeElement2.material];
					if (num4 > 0f)
					{
						float num5;
						pooledDictionary2.TryGetValue(recipeElement2.material, out num5);
						pooledDictionary2[recipeElement2.material] = num5 + num4;
						pooledDictionary[recipeElement2.material] = 0f;
					}
					else
					{
						DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary pooledDictionary3 = pooledDictionary;
						Tag material = recipeElement2.material;
						pooledDictionary3[material] -= num3;
					}
				}
			}
		}
		if (flag)
		{
			this.CancelFetches();
		}
		if (pooledDictionary2.Count > 0)
		{
			this.UpdateFetches(pooledDictionary2);
		}
		this.UpdateMaterialNeeds(pooledDictionary2);
		pooledDictionary2.Recycle();
		pooledDictionary.Recycle();
	}

	// Token: 0x060024CD RID: 9421 RVA: 0x000C8F34 File Offset: 0x000C7134
	private void UpdateMaterialNeeds(Dictionary<Tag, float> missingAmounts)
	{
		this.ClearMaterialNeeds();
		foreach (KeyValuePair<Tag, float> keyValuePair in missingAmounts)
		{
			MaterialNeeds.UpdateNeed(keyValuePair.Key, keyValuePair.Value, base.gameObject.GetMyWorldId());
			this.materialNeedCache.Add(keyValuePair.Key, keyValuePair.Value);
		}
	}

	// Token: 0x060024CE RID: 9422 RVA: 0x000C8FB8 File Offset: 0x000C71B8
	private void ClearMaterialNeeds()
	{
		foreach (KeyValuePair<Tag, float> keyValuePair in this.materialNeedCache)
		{
			MaterialNeeds.UpdateNeed(keyValuePair.Key, -keyValuePair.Value, base.gameObject.GetMyWorldId());
		}
		this.materialNeedCache.Clear();
	}

	// Token: 0x060024CF RID: 9423 RVA: 0x000C9030 File Offset: 0x000C7230
	public int HighestHEPQueued()
	{
		int num = 0;
		foreach (KeyValuePair<string, int> keyValuePair in this.recipeQueueCounts)
		{
			if (keyValuePair.Value > 0)
			{
				num = Math.Max(this.recipe_list[this.FindRecipeIndex(keyValuePair.Key)].consumedHEP, num);
			}
		}
		return num;
	}

	// Token: 0x060024D0 RID: 9424 RVA: 0x000C90AC File Offset: 0x000C72AC
	private void OnFetchComplete()
	{
		for (int i = this.fetchListList.Count - 1; i >= 0; i--)
		{
			if (this.fetchListList[i].IsComplete)
			{
				this.fetchListList.RemoveAt(i);
				this.queueDirty = true;
			}
		}
	}

	// Token: 0x060024D1 RID: 9425 RVA: 0x000C90F7 File Offset: 0x000C72F7
	private void OnStorageChange(object data)
	{
		this.queueDirty = true;
	}

	// Token: 0x060024D2 RID: 9426 RVA: 0x000C9100 File Offset: 0x000C7300
	private void OnDroppedAll(object data)
	{
		if (this.HasWorkingOrder)
		{
			this.CancelWorkingOrder();
		}
		this.CancelAllOpenOrders();
		this.RefreshQueue();
	}

	// Token: 0x060024D3 RID: 9427 RVA: 0x000C911C File Offset: 0x000C731C
	private void DropExcessIngredients(Storage storage)
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		if (this.keepAdditionalTag != Tag.Invalid)
		{
			hashSet.Add(this.keepAdditionalTag);
		}
		for (int i = 0; i < this.recipe_list.Length; i++)
		{
			ComplexRecipe complexRecipe = this.recipe_list[i];
			if (this.IsRecipeQueued(complexRecipe))
			{
				foreach (ComplexRecipe.RecipeElement recipeElement in complexRecipe.ingredients)
				{
					hashSet.Add(recipeElement.material);
				}
			}
		}
		for (int k = storage.items.Count - 1; k >= 0; k--)
		{
			GameObject gameObject = storage.items[k];
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null) && (!this.keepExcessLiquids || !component.Element.IsLiquid))
				{
					KPrefabID component2 = gameObject.GetComponent<KPrefabID>();
					if (component2 && !hashSet.Contains(component2.PrefabID()))
					{
						storage.Drop(gameObject, true);
					}
				}
			}
		}
	}

	// Token: 0x060024D4 RID: 9428 RVA: 0x000C922C File Offset: 0x000C742C
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		ComplexFabricator component = gameObject.GetComponent<ComplexFabricator>();
		if (component == null)
		{
			return;
		}
		this.ForbidMutantSeeds = component.ForbidMutantSeeds;
		foreach (ComplexRecipe complexRecipe in this.recipe_list)
		{
			int count;
			if (!component.recipeQueueCounts.TryGetValue(complexRecipe.id, out count))
			{
				count = 0;
			}
			this.SetRecipeQueueCountInternal(complexRecipe, count);
		}
		this.RefreshQueue();
	}

	// Token: 0x060024D5 RID: 9429 RVA: 0x000C92AA File Offset: 0x000C74AA
	private int CompareRecipe(ComplexRecipe a, ComplexRecipe b)
	{
		if (a.sortOrder != b.sortOrder)
		{
			return a.sortOrder - b.sortOrder;
		}
		return StringComparer.InvariantCulture.Compare(a.id, b.id);
	}

	// Token: 0x060024D6 RID: 9430 RVA: 0x000C92E0 File Offset: 0x000C74E0
	public ComplexRecipe[] GetRecipes()
	{
		if (this.recipe_list == null)
		{
			Tag prefabTag = base.GetComponent<KPrefabID>().PrefabTag;
			List<ComplexRecipe> recipes = ComplexRecipeManager.Get().recipes;
			List<ComplexRecipe> list = new List<ComplexRecipe>();
			foreach (ComplexRecipe complexRecipe in recipes)
			{
				using (List<Tag>.Enumerator enumerator2 = complexRecipe.fabricators.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current == prefabTag)
						{
							list.Add(complexRecipe);
						}
					}
				}
			}
			this.recipe_list = list.ToArray();
			Array.Sort<ComplexRecipe>(this.recipe_list, new Comparison<ComplexRecipe>(this.CompareRecipe));
		}
		return this.recipe_list;
	}

	// Token: 0x060024D7 RID: 9431 RVA: 0x000C93C0 File Offset: 0x000C75C0
	private void InitRecipeQueueCount()
	{
		foreach (ComplexRecipe complexRecipe in this.GetRecipes())
		{
			bool flag = false;
			using (Dictionary<string, int>.KeyCollection.Enumerator enumerator = this.recipeQueueCounts.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == complexRecipe.id)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				this.recipeQueueCounts.Add(complexRecipe.id, 0);
			}
			this.openOrderCounts.Add(0);
		}
	}

	// Token: 0x060024D8 RID: 9432 RVA: 0x000C9460 File Offset: 0x000C7660
	private int FindRecipeIndex(string id)
	{
		for (int i = 0; i < this.recipe_list.Length; i++)
		{
			if (this.recipe_list[i].id == id)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060024D9 RID: 9433 RVA: 0x000C9498 File Offset: 0x000C7698
	public int GetRecipeQueueCount(ComplexRecipe recipe)
	{
		return this.recipeQueueCounts[recipe.id];
	}

	// Token: 0x060024DA RID: 9434 RVA: 0x000C94AC File Offset: 0x000C76AC
	public bool IsRecipeQueued(ComplexRecipe recipe)
	{
		int num = this.recipeQueueCounts[recipe.id];
		global::Debug.Assert(num >= 0 || num == ComplexFabricator.QUEUE_INFINITE);
		return num != 0;
	}

	// Token: 0x060024DB RID: 9435 RVA: 0x000C94E4 File Offset: 0x000C76E4
	public int GetRecipePrefetchCount(ComplexRecipe recipe)
	{
		int remainingQueueCount = this.GetRemainingQueueCount(recipe);
		global::Debug.Assert(remainingQueueCount >= 0);
		return Mathf.Min(2, remainingQueueCount);
	}

	// Token: 0x060024DC RID: 9436 RVA: 0x000C950C File Offset: 0x000C770C
	private int GetRemainingQueueCount(ComplexRecipe recipe)
	{
		int num = this.recipeQueueCounts[recipe.id];
		global::Debug.Assert(num >= 0 || num == ComplexFabricator.QUEUE_INFINITE);
		if (num == ComplexFabricator.QUEUE_INFINITE)
		{
			return ComplexFabricator.MAX_QUEUE_SIZE;
		}
		if (num > 0)
		{
			if (this.IsCurrentRecipe(recipe))
			{
				num--;
			}
			return num;
		}
		return 0;
	}

	// Token: 0x060024DD RID: 9437 RVA: 0x000C9561 File Offset: 0x000C7761
	private bool IsCurrentRecipe(ComplexRecipe recipe)
	{
		return this.workingOrderIdx >= 0 && this.recipe_list[this.workingOrderIdx].id == recipe.id;
	}

	// Token: 0x060024DE RID: 9438 RVA: 0x000C958B File Offset: 0x000C778B
	public void SetRecipeQueueCount(ComplexRecipe recipe, int count)
	{
		this.SetRecipeQueueCountInternal(recipe, count);
		this.RefreshQueue();
	}

	// Token: 0x060024DF RID: 9439 RVA: 0x000C959B File Offset: 0x000C779B
	private void SetRecipeQueueCountInternal(ComplexRecipe recipe, int count)
	{
		this.recipeQueueCounts[recipe.id] = count;
	}

	// Token: 0x060024E0 RID: 9440 RVA: 0x000C95B0 File Offset: 0x000C77B0
	public void IncrementRecipeQueueCount(ComplexRecipe recipe)
	{
		if (this.recipeQueueCounts[recipe.id] == ComplexFabricator.QUEUE_INFINITE)
		{
			this.recipeQueueCounts[recipe.id] = 0;
		}
		else if (this.recipeQueueCounts[recipe.id] >= ComplexFabricator.MAX_QUEUE_SIZE)
		{
			this.recipeQueueCounts[recipe.id] = ComplexFabricator.QUEUE_INFINITE;
		}
		else
		{
			Dictionary<string, int> dictionary = this.recipeQueueCounts;
			string id = recipe.id;
			int num = dictionary[id];
			dictionary[id] = num + 1;
		}
		this.RefreshQueue();
	}

	// Token: 0x060024E1 RID: 9441 RVA: 0x000C963D File Offset: 0x000C783D
	public void DecrementRecipeQueueCount(ComplexRecipe recipe, bool respectInfinite = true)
	{
		this.DecrementRecipeQueueCountInternal(recipe, respectInfinite);
		this.RefreshQueue();
	}

	// Token: 0x060024E2 RID: 9442 RVA: 0x000C9650 File Offset: 0x000C7850
	private void DecrementRecipeQueueCountInternal(ComplexRecipe recipe, bool respectInfinite = true)
	{
		if (!respectInfinite || this.recipeQueueCounts[recipe.id] != ComplexFabricator.QUEUE_INFINITE)
		{
			if (this.recipeQueueCounts[recipe.id] == ComplexFabricator.QUEUE_INFINITE)
			{
				this.recipeQueueCounts[recipe.id] = ComplexFabricator.MAX_QUEUE_SIZE;
				return;
			}
			if (this.recipeQueueCounts[recipe.id] == 0)
			{
				this.recipeQueueCounts[recipe.id] = ComplexFabricator.QUEUE_INFINITE;
				return;
			}
			Dictionary<string, int> dictionary = this.recipeQueueCounts;
			string id = recipe.id;
			int num = dictionary[id];
			dictionary[id] = num - 1;
		}
	}

	// Token: 0x060024E3 RID: 9443 RVA: 0x000C96EF File Offset: 0x000C78EF
	private void CreateChore()
	{
		global::Debug.Assert(this.chore == null, "chore should be null");
		this.chore = this.workable.CreateWorkChore(this.choreType, this.orderProgress);
	}

	// Token: 0x060024E4 RID: 9444 RVA: 0x000C9721 File Offset: 0x000C7921
	private void CancelChore()
	{
		if (this.cancelling)
		{
			return;
		}
		this.cancelling = true;
		if (this.chore != null)
		{
			this.chore.Cancel("order cancelled");
			this.chore = null;
		}
		this.cancelling = false;
	}

	// Token: 0x060024E5 RID: 9445 RVA: 0x000C975C File Offset: 0x000C795C
	private void UpdateFetches(DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary missingAmounts)
	{
		ChoreType byHash = Db.Get().ChoreTypes.GetByHash(this.fetchChoreTypeIdHash);
		foreach (KeyValuePair<Tag, float> keyValuePair in missingAmounts)
		{
			if (!this.allowManualFluidDelivery)
			{
				Element element = ElementLoader.GetElement(keyValuePair.Key);
				if (element != null && (element.IsLiquid || element.IsGas))
				{
					continue;
				}
			}
			if (keyValuePair.Value >= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT && !this.HasPendingFetch(keyValuePair.Key))
			{
				FetchList2 fetchList = new FetchList2(this.inStorage, byHash);
				FetchList2 fetchList2 = fetchList;
				Tag key = keyValuePair.Key;
				float value = keyValuePair.Value;
				fetchList2.Add(key, this.ForbiddenTags, value, Operational.State.None);
				fetchList.ShowStatusItem = false;
				fetchList.Submit(new System.Action(this.OnFetchComplete), false);
				this.fetchListList.Add(fetchList);
			}
		}
	}

	// Token: 0x060024E6 RID: 9446 RVA: 0x000C985C File Offset: 0x000C7A5C
	private bool HasPendingFetch(Tag tag)
	{
		foreach (FetchList2 fetchList in this.fetchListList)
		{
			float num;
			fetchList.MinimumAmount.TryGetValue(tag, out num);
			if (num > 0f)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060024E7 RID: 9447 RVA: 0x000C98C4 File Offset: 0x000C7AC4
	private void CancelFetches()
	{
		foreach (FetchList2 fetchList in this.fetchListList)
		{
			fetchList.Cancel("cancel all orders");
		}
		this.fetchListList.Clear();
	}

	// Token: 0x060024E8 RID: 9448 RVA: 0x000C9924 File Offset: 0x000C7B24
	protected virtual void TransferCurrentRecipeIngredientsForBuild()
	{
		ComplexRecipe.RecipeElement[] ingredients = this.recipe_list[this.workingOrderIdx].ingredients;
		int i = 0;
		while (i < ingredients.Length)
		{
			ComplexRecipe.RecipeElement recipeElement = ingredients[i];
			float num;
			for (;;)
			{
				num = recipeElement.amount - this.buildStorage.GetAmountAvailable(recipeElement.material);
				if (num <= 0f)
				{
					break;
				}
				if (this.inStorage.GetAmountAvailable(recipeElement.material) <= 0f)
				{
					goto Block_2;
				}
				this.inStorage.Transfer(this.buildStorage, recipeElement.material, num, false, true);
			}
			IL_9D:
			i++;
			continue;
			Block_2:
			global::Debug.LogWarningFormat("TransferCurrentRecipeIngredientsForBuild ran out of {0} but still needed {1} more.", new object[]
			{
				recipeElement.material,
				num
			});
			goto IL_9D;
		}
	}

	// Token: 0x060024E9 RID: 9449 RVA: 0x000C99DC File Offset: 0x000C7BDC
	protected virtual bool HasIngredients(ComplexRecipe recipe, Storage storage)
	{
		ComplexRecipe.RecipeElement[] ingredients = recipe.ingredients;
		if (recipe.consumedHEP > 0)
		{
			HighEnergyParticleStorage component = base.GetComponent<HighEnergyParticleStorage>();
			if (component == null || component.Particles < (float)recipe.consumedHEP)
			{
				return false;
			}
		}
		foreach (ComplexRecipe.RecipeElement recipeElement in ingredients)
		{
			float amountAvailable = storage.GetAmountAvailable(recipeElement.material);
			if (recipeElement.amount - amountAvailable >= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060024EA RID: 9450 RVA: 0x000C9A54 File Offset: 0x000C7C54
	private void ToggleMutantSeedFetches()
	{
		if (this.HasAnyOrder)
		{
			ChoreType byHash = Db.Get().ChoreTypes.GetByHash(this.fetchChoreTypeIdHash);
			List<FetchList2> list = new List<FetchList2>();
			foreach (FetchList2 fetchList in this.fetchListList)
			{
				foreach (FetchOrder2 fetchOrder in fetchList.FetchOrders)
				{
					foreach (Tag tag in fetchOrder.Tags)
					{
						GameObject prefab = Assets.GetPrefab(tag);
						if (prefab != null && prefab.GetComponent<PlantableSeed>() != null)
						{
							fetchList.Cancel("MutantSeedTagChanged");
							list.Add(fetchList);
						}
					}
				}
			}
			foreach (FetchList2 fetchList2 in list)
			{
				this.fetchListList.Remove(fetchList2);
				foreach (FetchOrder2 fetchOrder2 in fetchList2.FetchOrders)
				{
					foreach (Tag tag2 in fetchOrder2.Tags)
					{
						FetchList2 fetchList3 = new FetchList2(this.inStorage, byHash);
						FetchList2 fetchList4 = fetchList3;
						Tag tag3 = tag2;
						float totalAmount = fetchOrder2.TotalAmount;
						fetchList4.Add(tag3, this.ForbiddenTags, totalAmount, Operational.State.None);
						fetchList3.ShowStatusItem = false;
						fetchList3.Submit(new System.Action(this.OnFetchComplete), false);
						this.fetchListList.Add(fetchList3);
					}
				}
			}
		}
	}

	// Token: 0x060024EB RID: 9451 RVA: 0x000C9C94 File Offset: 0x000C7E94
	protected virtual List<GameObject> SpawnOrderProduct(ComplexRecipe recipe)
	{
		List<GameObject> list = new List<GameObject>();
		SimUtil.DiseaseInfo diseaseInfo;
		diseaseInfo.count = 0;
		diseaseInfo.idx = 0;
		float num = 0f;
		float num2 = 0f;
		string text = null;
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.ingredients)
		{
			num2 += recipeElement.amount;
		}
		Element element = null;
		foreach (ComplexRecipe.RecipeElement recipeElement2 in recipe.ingredients)
		{
			float num3 = recipeElement2.amount / num2;
			if (recipe.ProductHasFacade && text.IsNullOrWhiteSpace())
			{
				RepairableEquipment component = this.buildStorage.FindFirst(recipeElement2.material).GetComponent<RepairableEquipment>();
				if (component != null)
				{
					text = component.facadeID;
				}
			}
			if (recipeElement2.inheritElement)
			{
				element = this.buildStorage.FindFirst(recipeElement2.material).GetComponent<PrimaryElement>().Element;
			}
			float num4;
			SimUtil.DiseaseInfo diseaseInfo2;
			float num5;
			this.buildStorage.ConsumeAndGetDisease(recipeElement2.material, recipeElement2.amount, out num4, out diseaseInfo2, out num5);
			if (diseaseInfo2.count > diseaseInfo.count)
			{
				diseaseInfo = diseaseInfo2;
			}
			num += num5 * num3;
		}
		if (recipe.consumedHEP > 0)
		{
			base.GetComponent<HighEnergyParticleStorage>().ConsumeAndGet((float)recipe.consumedHEP);
		}
		foreach (ComplexRecipe.RecipeElement recipeElement3 in recipe.results)
		{
			GameObject gameObject = this.buildStorage.FindFirst(recipeElement3.material);
			if (gameObject != null)
			{
				Edible component2 = gameObject.GetComponent<Edible>();
				if (component2)
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -component2.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.CRAFTED_USED, "{0}", component2.GetProperName()), UI.ENDOFDAYREPORT.NOTES.CRAFTED_CONTEXT);
				}
			}
			ComplexRecipe.RecipeElement.TemperatureOperation temperatureOperation = recipeElement3.temperatureOperation;
			if (temperatureOperation > ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
			{
				if (temperatureOperation == ComplexRecipe.RecipeElement.TemperatureOperation.Melted)
				{
					if (this.storeProduced || recipeElement3.storeElement)
					{
						float temperature = ElementLoader.GetElement(recipeElement3.material).defaultValues.temperature;
						this.outStorage.AddLiquid(ElementLoader.GetElementID(recipeElement3.material), recipeElement3.amount, temperature, 0, 0, false, true);
					}
				}
			}
			else
			{
				GameObject gameObject2 = GameUtil.KInstantiate(Assets.GetPrefab(recipeElement3.material), Grid.SceneLayer.Ore, null, 0);
				int cell = Grid.PosToCell(this);
				gameObject2.transform.SetPosition(Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore) + this.outputOffset);
				PrimaryElement component3 = gameObject2.GetComponent<PrimaryElement>();
				component3.Units = recipeElement3.amount;
				component3.Temperature = ((recipeElement3.temperatureOperation == ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature) ? num : this.heatedTemperature);
				if (element != null)
				{
					component3.SetElement(element.id, false);
				}
				if (recipe.ProductHasFacade && !text.IsNullOrWhiteSpace())
				{
					Equippable component4 = gameObject2.GetComponent<Equippable>();
					if (component4 != null)
					{
						EquippableFacade.AddFacadeToEquippable(component4, text);
					}
				}
				gameObject2.SetActive(true);
				float num6 = recipeElement3.amount / recipe.TotalResultUnits();
				component3.AddDisease(diseaseInfo.idx, Mathf.RoundToInt((float)diseaseInfo.count * num6), "ComplexFabricator.CompleteOrder");
				if (!recipeElement3.facadeID.IsNullOrWhiteSpace())
				{
					Equippable component5 = gameObject2.GetComponent<Equippable>();
					if (component5 != null)
					{
						EquippableFacade.AddFacadeToEquippable(component5, recipeElement3.facadeID);
					}
				}
				gameObject2.GetComponent<KMonoBehaviour>().Trigger(748399584, null);
				list.Add(gameObject2);
				if (this.storeProduced || recipeElement3.storeElement)
				{
					this.outStorage.Store(gameObject2, false, false, true, false);
				}
			}
			if (list.Count > 0)
			{
				SymbolOverrideController component6 = base.GetComponent<SymbolOverrideController>();
				if (component6 != null)
				{
					KAnim.Build build = list[0].GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build;
					KAnim.Build.Symbol symbol = build.GetSymbol(build.name);
					if (symbol != null)
					{
						component6.TryRemoveSymbolOverride("output_tracker", 0);
						component6.AddSymbolOverride("output_tracker", symbol, 0);
					}
					else
					{
						global::Debug.LogWarning(component6.name + " is missing symbol " + build.name);
					}
				}
			}
		}
		if (recipe.producedHEP > 0)
		{
			base.GetComponent<HighEnergyParticleStorage>().Store((float)recipe.producedHEP);
		}
		return list;
	}

	// Token: 0x060024EC RID: 9452 RVA: 0x000CA0EC File Offset: 0x000C82EC
	public virtual List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		ComplexRecipe[] recipes = this.GetRecipes();
		if (recipes.Length != 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.BUILDINGEFFECTS.PROCESSES, UI.BUILDINGEFFECTS.TOOLTIPS.PROCESSES, Descriptor.DescriptorType.Effect);
			list.Add(item);
		}
		foreach (ComplexRecipe complexRecipe in recipes)
		{
			string text = "";
			string uiname = complexRecipe.GetUIName(false);
			foreach (ComplexRecipe.RecipeElement recipeElement in complexRecipe.ingredients)
			{
				text = text + "• " + string.Format(UI.BUILDINGEFFECTS.PROCESSEDITEM, recipeElement.material.ProperName(), recipeElement.amount) + "\n";
			}
			Descriptor item2 = new Descriptor(uiname, string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.FABRICATOR_INGREDIENTS, text), Descriptor.DescriptorType.Effect, false);
			item2.IncreaseIndent();
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x060024ED RID: 9453 RVA: 0x000CA1E4 File Offset: 0x000C83E4
	public virtual List<Descriptor> AdditionalEffectsForRecipe(ComplexRecipe recipe)
	{
		return new List<Descriptor>();
	}

	// Token: 0x060024EE RID: 9454 RVA: 0x000CA1EC File Offset: 0x000C83EC
	public string GetConversationTopic()
	{
		if (this.HasWorkingOrder)
		{
			ComplexRecipe complexRecipe = this.recipe_list[this.workingOrderIdx];
			if (complexRecipe != null)
			{
				return complexRecipe.results[0].material.Name;
			}
		}
		return null;
	}

	// Token: 0x060024EF RID: 9455 RVA: 0x000CA228 File Offset: 0x000C8428
	public bool NeedsMoreHEPForQueuedRecipe()
	{
		if (this.hasOpenOrders)
		{
			HighEnergyParticleStorage component = base.GetComponent<HighEnergyParticleStorage>();
			foreach (KeyValuePair<string, int> keyValuePair in this.recipeQueueCounts)
			{
				if (keyValuePair.Value > 0)
				{
					foreach (ComplexRecipe complexRecipe in this.GetRecipes())
					{
						if (complexRecipe.id == keyValuePair.Key && (float)complexRecipe.consumedHEP > component.Particles)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x04001502 RID: 5378
	private const int MaxPrefetchCount = 2;

	// Token: 0x04001503 RID: 5379
	public bool duplicantOperated = true;

	// Token: 0x04001504 RID: 5380
	protected ComplexFabricatorWorkable workable;

	// Token: 0x04001505 RID: 5381
	public string SideScreenSubtitleLabel = UI.UISIDESCREENS.FABRICATORSIDESCREEN.SUBTITLE;

	// Token: 0x04001506 RID: 5382
	public string SideScreenRecipeScreenTitle = UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_DETAILS;

	// Token: 0x04001507 RID: 5383
	[SerializeField]
	public HashedString fetchChoreTypeIdHash = Db.Get().ChoreTypes.FabricateFetch.IdHash;

	// Token: 0x04001508 RID: 5384
	[SerializeField]
	public float heatedTemperature;

	// Token: 0x04001509 RID: 5385
	[SerializeField]
	public bool storeProduced;

	// Token: 0x0400150A RID: 5386
	[SerializeField]
	public bool allowManualFluidDelivery = true;

	// Token: 0x0400150B RID: 5387
	public ComplexFabricatorSideScreen.StyleSetting sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;

	// Token: 0x0400150C RID: 5388
	public bool labelByResult = true;

	// Token: 0x0400150D RID: 5389
	public Vector3 outputOffset = Vector3.zero;

	// Token: 0x0400150E RID: 5390
	public ChoreType choreType;

	// Token: 0x0400150F RID: 5391
	public bool keepExcessLiquids;

	// Token: 0x04001510 RID: 5392
	public Tag keepAdditionalTag = Tag.Invalid;

	// Token: 0x04001511 RID: 5393
	public StatusItem workingStatusItem = Db.Get().BuildingStatusItems.ComplexFabricatorProducing;

	// Token: 0x04001512 RID: 5394
	public static int MAX_QUEUE_SIZE = 99;

	// Token: 0x04001513 RID: 5395
	public static int QUEUE_INFINITE = -1;

	// Token: 0x04001514 RID: 5396
	[Serialize]
	private Dictionary<string, int> recipeQueueCounts = new Dictionary<string, int>();

	// Token: 0x04001515 RID: 5397
	private int nextOrderIdx;

	// Token: 0x04001516 RID: 5398
	private bool nextOrderIsWorkable;

	// Token: 0x04001517 RID: 5399
	private int workingOrderIdx = -1;

	// Token: 0x04001518 RID: 5400
	[Serialize]
	private string lastWorkingRecipe;

	// Token: 0x04001519 RID: 5401
	[Serialize]
	private float orderProgress;

	// Token: 0x0400151A RID: 5402
	private List<int> openOrderCounts = new List<int>();

	// Token: 0x0400151B RID: 5403
	[Serialize]
	private bool forbidMutantSeeds;

	// Token: 0x0400151C RID: 5404
	private Tag[] forbiddenMutantTags = new Tag[]
	{
		GameTags.MutatedSeed
	};

	// Token: 0x0400151D RID: 5405
	private bool queueDirty = true;

	// Token: 0x0400151E RID: 5406
	private bool hasOpenOrders;

	// Token: 0x0400151F RID: 5407
	private List<FetchList2> fetchListList = new List<FetchList2>();

	// Token: 0x04001520 RID: 5408
	private Chore chore;

	// Token: 0x04001521 RID: 5409
	private bool cancelling;

	// Token: 0x04001522 RID: 5410
	private ComplexRecipe[] recipe_list;

	// Token: 0x04001523 RID: 5411
	private Dictionary<Tag, float> materialNeedCache = new Dictionary<Tag, float>();

	// Token: 0x04001524 RID: 5412
	[SerializeField]
	public Storage inStorage;

	// Token: 0x04001525 RID: 5413
	[SerializeField]
	public Storage buildStorage;

	// Token: 0x04001526 RID: 5414
	[SerializeField]
	public Storage outStorage;

	// Token: 0x04001527 RID: 5415
	[MyCmpAdd]
	private LoopingSounds loopingSounds;

	// Token: 0x04001528 RID: 5416
	[MyCmpReq]
	protected Operational operational;

	// Token: 0x04001529 RID: 5417
	[MyCmpAdd]
	protected ComplexFabricatorSM fabricatorSM;

	// Token: 0x0400152A RID: 5418
	private ProgressBar progressBar;

	// Token: 0x0400152B RID: 5419
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x0400152C RID: 5420
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnParticleStorageChangedDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x0400152D RID: 5421
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnDroppedAllDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnDroppedAll(data);
	});

	// Token: 0x0400152E RID: 5422
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x0400152F RID: 5423
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001530 RID: 5424
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
