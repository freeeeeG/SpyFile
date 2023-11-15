using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using AssetBundles;
using T17.Analytics;
using Team17.Online;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000B83 RID: 2947
public static class GameUtils
{
	// Token: 0x06003C3B RID: 15419 RVA: 0x00120038 File Offset: 0x0011E438
	public static Helper GetT17AnalyticsHelper()
	{
		return GameUtils.s_AnalyticsHelper;
	}

	// Token: 0x06003C3C RID: 15420 RVA: 0x0012003F File Offset: 0x0011E43F
	public static void EnableAnalytics(bool enable)
	{
		GameUtils.s_AnalyticsEnabled = enable;
		if (GameUtils.s_AnalyticsHelper != null)
		{
			GameUtils.s_AnalyticsHelper.EnableAnalytics(enable);
		}
	}

	// Token: 0x06003C3D RID: 15421 RVA: 0x0012005C File Offset: 0x0011E45C
	public static GameObject GetGameEnvironment()
	{
		return GameObject.FindGameObjectWithTag("GameController");
	}

	// Token: 0x06003C3E RID: 15422 RVA: 0x00120078 File Offset: 0x0011E478
	public static GameObject GetGameMetaEnvironment()
	{
		return GameObject.FindGameObjectWithTag("GameMetaEnvironment");
	}

	// Token: 0x06003C3F RID: 15423 RVA: 0x00120094 File Offset: 0x0011E494
	public static GameSession GetGameSession()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("GameSession");
		if (gameObject != null)
		{
			return gameObject.RequireComponent<GameSession>();
		}
		return null;
	}

	// Token: 0x06003C40 RID: 15424 RVA: 0x001200C0 File Offset: 0x0011E4C0
	public static int GetLevelID()
	{
		GameSession gameSession = GameUtils.GetGameSession();
		SceneDirectoryData sceneDirectory = gameSession.Progress.GetSceneDirectory();
		GameSession.GameLevelSettings levelSettings = gameSession.LevelSettings;
		Predicate<SceneDirectoryData.SceneDirectoryEntry> matchFunction = (SceneDirectoryData.SceneDirectoryEntry x) => x.SceneVarients.Contains(levelSettings.SceneDirectoryVarientEntry);
		return sceneDirectory.Scenes.FindIndex_Predicate(matchFunction);
	}

	// Token: 0x06003C41 RID: 15425 RVA: 0x0012010C File Offset: 0x0011E50C
	public static MetaGameProgress GetMetaGameProgress()
	{
		SaveManager saveManager = GameUtils.RequireManager<SaveManager>();
		return saveManager.GetMetaGameProgress();
	}

	// Token: 0x06003C42 RID: 15426 RVA: 0x00120128 File Offset: 0x0011E528
	public static AvatarDirectoryData GetAvatarDirectoryData()
	{
		MetaGameProgress metaGameProgress = GameUtils.GetMetaGameProgress();
		if (metaGameProgress != null)
		{
			return metaGameProgress.AvatarDirectory;
		}
		return null;
	}

	// Token: 0x06003C43 RID: 15427 RVA: 0x00120150 File Offset: 0x0011E550
	public static string DebugGetState()
	{
		GameSession gameSession = GameUtils.GetGameSession();
		if (gameSession != null && gameSession.LevelSettings != null && gameSession.LevelSettings.SceneDirectoryVarientEntry != null)
		{
			return "InLevel - " + gameSession.LevelSettings.SceneDirectoryVarientEntry.SceneName;
		}
		return "OutsideLevel";
	}

	// Token: 0x06003C44 RID: 15428 RVA: 0x001201AA File Offset: 0x0011E5AA
	public static GameDebugManager GetDebugManager()
	{
		return GameUtils.RequireManager<GameDebugManager>();
	}

	// Token: 0x06003C45 RID: 15429 RVA: 0x001201B4 File Offset: 0x0011E5B4
	public static GridManager GetGridManager(Transform _areaReference)
	{
		if (_areaReference != null)
		{
			GridManager gridManager = _areaReference.gameObject.RequestComponentUpwardsRecursive<GridManager>();
			if (gridManager != null)
			{
				return gridManager;
			}
		}
		return GameUtils.RequireManagerInterface<GridManager>();
	}

	// Token: 0x06003C46 RID: 15430 RVA: 0x001201EC File Offset: 0x0011E5EC
	public static GridNavSpace GetGridNavSpace()
	{
		return GameUtils.RequireManager<GridNavSpace>();
	}

	// Token: 0x06003C47 RID: 15431 RVA: 0x001201F3 File Offset: 0x0011E5F3
	public static IFlowController GetFlowController()
	{
		return GameUtils.RequireManagerInterface<IFlowController>();
	}

	// Token: 0x06003C48 RID: 15432 RVA: 0x001201FC File Offset: 0x0011E5FC
	public static GameDebugConfig GetDebugConfig()
	{
		if (GameUtils.s_config)
		{
			return GameUtils.s_config;
		}
		GameDebugManager debugManager = GameUtils.GetDebugManager();
		GameUtils.s_config = debugManager.GetDebugConfig();
		return GameUtils.s_config;
	}

	// Token: 0x06003C49 RID: 15433 RVA: 0x00120234 File Offset: 0x0011E634
	public static T RequireManager<T>() where T : Manager
	{
		return GameUtils.RequestManager<T>();
	}

	// Token: 0x06003C4A RID: 15434 RVA: 0x00120248 File Offset: 0x0011E648
	public static T RequestManager<T>() where T : Manager
	{
		foreach (GameObject gameObject in new GameObject[]
		{
			GameUtils.GetGameEnvironment(),
			GameUtils.GetGameMetaEnvironment()
		})
		{
			if (gameObject != null)
			{
				ManagerDirectory managerDirectory = gameObject.RequestComponent<ManagerDirectory>();
				if (managerDirectory != null)
				{
					T t = managerDirectory.RequestManager<T>();
					if (t != null)
					{
						return t;
					}
				}
			}
		}
		return (T)((object)null);
	}

	// Token: 0x06003C4B RID: 15435 RVA: 0x001202C8 File Offset: 0x0011E6C8
	public static T RequestManagerInterface<T>() where T : class
	{
		T t = GameUtils.RequestManagerInterfaceFromEnvironment<T>(GameUtils.GetGameEnvironment());
		if (t == null)
		{
			t = GameUtils.RequestManagerInterfaceFromEnvironment<T>(GameUtils.GetGameMetaEnvironment());
		}
		return t;
	}

	// Token: 0x06003C4C RID: 15436 RVA: 0x001202F8 File Offset: 0x0011E6F8
	public static T RequestManagerInterfaceFromEnvironment<T>(GameObject environment) where T : class
	{
		if (environment != null)
		{
			ManagerDirectory managerDirectory = environment.RequestComponent<ManagerDirectory>();
			if (managerDirectory != null)
			{
				T t = managerDirectory.RequestManagerInterface<T>();
				if (t != null)
				{
					return t;
				}
			}
		}
		return (T)((object)null);
	}

	// Token: 0x06003C4D RID: 15437 RVA: 0x00120340 File Offset: 0x0011E740
	public static T RequireManagerInterface<T>() where T : class
	{
		return GameUtils.RequestManagerInterface<T>();
	}

	// Token: 0x06003C4E RID: 15438 RVA: 0x00120354 File Offset: 0x0011E754
	public static void EnsureBootstrapSetup()
	{
		BootstrapManager bootstrapManager = UnityEngine.Object.FindObjectOfType<BootstrapManager>();
		if (bootstrapManager)
		{
			bootstrapManager.EnsureSetup();
		}
	}

	// Token: 0x06003C4F RID: 15439 RVA: 0x00120378 File Offset: 0x0011E778
	public static LevelConfigBase GetLevelConfig()
	{
		LevelConfigBase levelConfigBase = null;
		IFlowController flowController = GameUtils.RequestManagerInterface<IFlowController>();
		if (flowController != null)
		{
			levelConfigBase = flowController.GetLevelConfig();
		}
		if (levelConfigBase == null)
		{
			GameSession.GameLevelSettings levelSettings = GameUtils.GetGameSession().LevelSettings;
			if (levelSettings != null && levelSettings.SceneDirectoryVarientEntry != null)
			{
				levelConfigBase = levelSettings.SceneDirectoryVarientEntry.LevelConfig;
			}
		}
		return levelConfigBase;
	}

	// Token: 0x06003C50 RID: 15440 RVA: 0x001203D0 File Offset: 0x0011E7D0
	public static GameConfig GetGameConfig()
	{
		IFlowController flowController = GameUtils.RequireManagerInterface<IFlowController>();
		return flowController.GetGameConfig();
	}

	// Token: 0x06003C51 RID: 15441 RVA: 0x001203EC File Offset: 0x0011E7EC
	public static AudioSource TriggerAudio(GameOneShotAudioTag _audio, int _layer)
	{
		AudioManager audioManager = GameUtils.RequireManager<AudioManager>();
		return audioManager.TriggerAudio(_audio, _layer);
	}

	// Token: 0x06003C52 RID: 15442 RVA: 0x00120408 File Offset: 0x0011E808
	public static AudioSource StartAudio(GameLoopingAudioTag _audio, object _token, int _layer)
	{
		AudioManager audioManager = GameUtils.RequireManager<AudioManager>();
		return audioManager.StartAudio(_audio, _token, _layer);
	}

	// Token: 0x06003C53 RID: 15443 RVA: 0x00120424 File Offset: 0x0011E824
	public static void StopAudio(GameLoopingAudioTag _audio, object _token)
	{
		AudioManager audioManager = GameUtils.RequireManager<AudioManager>();
		if (audioManager)
		{
			audioManager.StopAudio(_audio, _token);
		}
	}

	// Token: 0x06003C54 RID: 15444 RVA: 0x0012044A File Offset: 0x0011E84A
	public static void TriggerNXRumble(PlayerInputLookup.Player _player, GameOneShotAudioTag _audio)
	{
	}

	// Token: 0x06003C55 RID: 15445 RVA: 0x0012044C File Offset: 0x0011E84C
	public static void StartNXRumble(PlayerInputLookup.Player _player, GameLoopingAudioTag _audio)
	{
	}

	// Token: 0x06003C56 RID: 15446 RVA: 0x0012044E File Offset: 0x0011E84E
	public static void StopNXRumble(PlayerInputLookup.Player _player, GameLoopingAudioTag _audio)
	{
	}

	// Token: 0x06003C57 RID: 15447 RVA: 0x00120450 File Offset: 0x0011E850
	public static void TriggerNXRumble(GameOneShotAudioTag _audio)
	{
	}

	// Token: 0x06003C58 RID: 15448 RVA: 0x00120452 File Offset: 0x0011E852
	public static void StartNXRumble(GameLoopingAudioTag _audio)
	{
	}

	// Token: 0x06003C59 RID: 15449 RVA: 0x00120454 File Offset: 0x0011E854
	public static void StopNXRumble(GameLoopingAudioTag _audio)
	{
	}

	// Token: 0x06003C5A RID: 15450 RVA: 0x00120456 File Offset: 0x0011E856
	public static void StopAllNXRumblers()
	{
	}

	// Token: 0x06003C5B RID: 15451 RVA: 0x00120458 File Offset: 0x0011E858
	public static GameObject GetNamedCanvas(string _name)
	{
		foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Canvas"))
		{
			if (gameObject.name == _name)
			{
				return gameObject;
			}
		}
		return null;
	}

	// Token: 0x06003C5C RID: 15452 RVA: 0x0012049C File Offset: 0x0011E89C
	public static GameObject InstantiateUIController(GameObject _source, string _canvasName)
	{
		GameObject namedCanvas = GameUtils.GetNamedCanvas(_canvasName);
		return GameUtils.InstantiateUIController(_source, namedCanvas.transform as RectTransform);
	}

	// Token: 0x06003C5D RID: 15453 RVA: 0x001204C4 File Offset: 0x0011E8C4
	public static GameObject InstantiateUIControllerOnScalingHUDCanvas(GameObject _source)
	{
		GameObject namedCanvas = GameUtils.GetNamedCanvas("ScalingHUDCanvas");
		Transform transform = namedCanvas.transform.Find("SafeZoneElements");
		return GameUtils.InstantiateUIController(_source, transform as RectTransform);
	}

	// Token: 0x06003C5E RID: 15454 RVA: 0x001204FC File Offset: 0x0011E8FC
	public static GameObject InstantiateUIController(GameObject _source, RectTransform _parent)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(_source);
		gameObject.transform.SetParent(_parent, false);
		RectTransform rectTransform = gameObject.RequireComponent<RectTransform>();
		rectTransform.localScale = _source.transform.localScale;
		return gameObject;
	}

	// Token: 0x06003C5F RID: 15455 RVA: 0x00120538 File Offset: 0x0011E938
	public static GameObject InstantiateHoverIconUIController<T>(out T controller, GameObject _source, Transform _follower, string _canvasName, Vector3 _offset = default(Vector3)) where T : HoverIconUIController
	{
		GameObject gameObject = GameUtils.InstantiateUIController(_source, _canvasName);
		controller = gameObject.RequireComponent<T>();
		controller.SetFollowTransform(_follower, _offset);
		return gameObject;
	}

	// Token: 0x06003C60 RID: 15456 RVA: 0x0012056C File Offset: 0x0011E96C
	public static GameObject InstantiateHoverIconUIController<T>(out T controller, GameObject _source, Transform _follower, RectTransform _parent, Vector3 _offset = default(Vector3)) where T : HoverIconUIController
	{
		GameObject gameObject = GameUtils.InstantiateUIController(_source, _parent);
		controller = gameObject.RequireComponent<T>();
		controller.SetFollowTransform(_follower, _offset);
		return gameObject;
	}

	// Token: 0x06003C61 RID: 15457 RVA: 0x001205A0 File Offset: 0x0011E9A0
	public static GameObject InstantiateHoverIconUIController(GameObject _source, Transform _follower, string _canvasName, Vector3 _offset = default(Vector3))
	{
		GameObject gameObject = GameUtils.InstantiateUIController(_source, _canvasName);
		HoverIconUIController hoverIconUIController = gameObject.RequireComponent<HoverIconUIController>();
		hoverIconUIController.SetFollowTransform(_follower, _offset);
		return gameObject;
	}

	// Token: 0x06003C62 RID: 15458 RVA: 0x001205C8 File Offset: 0x0011E9C8
	public static ParticleSystem InstantiatePFX(this ParticleSystem _pfxPrefab, Transform _parent)
	{
		GameObject obj = _pfxPrefab.gameObject.InstantiateOnParent(_parent, true);
		ParticleSystem particleSystem = obj.RequireComponent<ParticleSystem>();
		if (particleSystem.main.playOnAwake)
		{
			particleSystem.RestartPFX();
			particleSystem.gameObject.RequireComponent<AutoDestructParticleSystem>();
		}
		return particleSystem;
	}

	// Token: 0x06003C63 RID: 15459 RVA: 0x00120610 File Offset: 0x0011EA10
	public static ParticleSystem InstantiatePFX(this ParticleSystem _pfxPrefab, Vector3 _position)
	{
		GameObject gameObject = _pfxPrefab.gameObject.InstantiateOnParent(null, true);
		gameObject.transform.position = _position + _pfxPrefab.transform.position;
		ParticleSystem particleSystem = gameObject.RequireComponent<ParticleSystem>();
		if (particleSystem.main.playOnAwake)
		{
			particleSystem.RestartPFX();
			particleSystem.gameObject.RequireComponent<AutoDestructParticleSystem>();
		}
		return particleSystem;
	}

	// Token: 0x06003C64 RID: 15460 RVA: 0x00120674 File Offset: 0x0011EA74
	public static void RestartPFX(this ParticleSystem _pfx)
	{
		_pfx.Clear();
		_pfx.Simulate(0f, false, true);
		_pfx.Play();
	}

	// Token: 0x06003C65 RID: 15461 RVA: 0x00120690 File Offset: 0x0011EA90
	private static OrderDefinitionNode[] GetAllOrderNodes()
	{
		OrderDefinitionNode[] array = null;
		IRecipeListCache recipeListCache = GameUtils.RequestManagerInterface<IRecipeListCache>();
		if (recipeListCache != null)
		{
			return recipeListCache.GetCachedRecipeList();
		}
		KitchenLevelConfigBase kitchenLevelConfigBase = GameUtils.GetLevelConfig() as KitchenLevelConfigBase;
		if (kitchenLevelConfigBase != null && kitchenLevelConfigBase.m_recipeMatchingList != null)
		{
			array = kitchenLevelConfigBase.m_recipeMatchingList.m_recipes;
			for (int i = 0; i < kitchenLevelConfigBase.m_recipeMatchingList.m_includeLists.Length; i++)
			{
				array = array.Union(kitchenLevelConfigBase.m_recipeMatchingList.m_includeLists[i].m_recipes);
			}
		}
		return array;
	}

	// Token: 0x06003C66 RID: 15462 RVA: 0x00120720 File Offset: 0x0011EB20
	private static AssembledDefinitionNode[] GetAllOrderNodesSimplified()
	{
		IRecipeListCache recipeListCache = GameUtils.RequestManagerInterface<IRecipeListCache>();
		if (recipeListCache != null)
		{
			return recipeListCache.GetCachedAssembledRecipes();
		}
		KitchenLevelConfigBase kitchenLevelConfigBase = GameUtils.GetLevelConfig() as KitchenLevelConfigBase;
		OrderDefinitionNode[] allOrderNodes = GameUtils.GetAllOrderNodes();
		AssembledDefinitionNode[] array = new AssembledDefinitionNode[allOrderNodes.Length];
		for (int i = 0; i < allOrderNodes.Length; i++)
		{
			array[i] = allOrderNodes[i].Convert().Simpilfy();
		}
		return array;
	}

	// Token: 0x06003C67 RID: 15463 RVA: 0x00120788 File Offset: 0x0011EB88
	private static CookingStepData[] GetAllCookingSteps()
	{
		CookingStepData[] array = null;
		IRecipeListCache recipeListCache = GameUtils.RequestManagerInterface<IRecipeListCache>();
		if (recipeListCache != null)
		{
			return recipeListCache.GetCachedCookingSteps();
		}
		KitchenLevelConfigBase kitchenLevelConfigBase = GameUtils.GetLevelConfig() as KitchenLevelConfigBase;
		if (kitchenLevelConfigBase != null && kitchenLevelConfigBase.m_recipeMatchingList != null)
		{
			array = kitchenLevelConfigBase.m_recipeMatchingList.m_cookingSteps;
			for (int i = 0; i < kitchenLevelConfigBase.m_recipeMatchingList.m_includeLists.Length; i++)
			{
				array = array.Union(kitchenLevelConfigBase.m_recipeMatchingList.m_includeLists[i].m_cookingSteps);
			}
		}
		return array;
	}

	// Token: 0x06003C68 RID: 15464 RVA: 0x00120818 File Offset: 0x0011EC18
	public static GameObject GetOrderPlatingPrefab(AssembledDefinitionNode _node, PlatingStepData _platingStep)
	{
		OrderDefinitionNode[] allOrderNodes = GameUtils.GetAllOrderNodes();
		AssembledDefinitionNode[] allOrderNodesSimplified = GameUtils.GetAllOrderNodesSimplified();
		if (_node == null || _platingStep == null || allOrderNodes == null || allOrderNodesSimplified == null)
		{
			return null;
		}
		AssembledDefinitionNode simpleNode = _node.Simpilfy();
		if (GameUtils.s_LastMatchedOrderIndex >= 0 && GameUtils.s_LastMatchedOrderIndex < allOrderNodes.Length && allOrderNodes[GameUtils.s_LastMatchedOrderIndex].m_platingStep == _platingStep && AssembledDefinitionNode.MatchingAlreadySimple(simpleNode, allOrderNodesSimplified[GameUtils.s_LastMatchedOrderIndex]))
		{
			return allOrderNodes[GameUtils.s_LastMatchedOrderIndex].m_platingPrefab;
		}
		for (int i = 0; i < allOrderNodes.Length; i++)
		{
			if (allOrderNodes[i].m_platingPrefab != null && allOrderNodes[i].m_platingStep == _platingStep && AssembledDefinitionNode.MatchingAlreadySimple(simpleNode, allOrderNodesSimplified[i]))
			{
				GameUtils.s_LastMatchedOrderIndex = i;
				return allOrderNodes[i].m_platingPrefab;
			}
		}
		return null;
	}

	// Token: 0x06003C69 RID: 15465 RVA: 0x001208FE File Offset: 0x0011ECFE
	public static GameObject GetOrderPlatingPrefab(OrderDefinitionNode _node, PlatingStepData _platingStep)
	{
		return GameUtils.GetOrderPlatingPrefab(_node.Simpilfy(), _platingStep);
	}

	// Token: 0x06003C6A RID: 15466 RVA: 0x0012090C File Offset: 0x0011ED0C
	public static bool IsValidRecipe(AssembledDefinitionNode _node)
	{
		AssembledDefinitionNode[] allOrderNodesSimplified = GameUtils.GetAllOrderNodesSimplified();
		if (_node == null || allOrderNodesSimplified == null)
		{
			return false;
		}
		AssembledDefinitionNode simpleNode = _node.Simpilfy();
		if (GameUtils.s_LastMatchedOrderIndex >= 0 && GameUtils.s_LastMatchedOrderIndex < allOrderNodesSimplified.Length && AssembledDefinitionNode.MatchingAlreadySimple(simpleNode, allOrderNodesSimplified[GameUtils.s_LastMatchedOrderIndex]))
		{
			return true;
		}
		for (int i = 0; i < allOrderNodesSimplified.Length; i++)
		{
			if (AssembledDefinitionNode.MatchingAlreadySimple(simpleNode, allOrderNodesSimplified[i]))
			{
				GameUtils.s_LastMatchedOrderIndex = i;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003C6B RID: 15467 RVA: 0x0012098C File Offset: 0x0011ED8C
	public static OrderDefinitionNode GetOrderDefinitionNode(int ID)
	{
		OrderDefinitionNode[] allOrderNodes = GameUtils.GetAllOrderNodes();
		for (int i = 0; i < allOrderNodes.Length; i++)
		{
			if (allOrderNodes[i].m_uID == ID)
			{
				return allOrderNodes[i];
			}
		}
		return null;
	}

	// Token: 0x06003C6C RID: 15468 RVA: 0x001209C8 File Offset: 0x0011EDC8
	public static CookingStepData GetCookingStepData(int ID)
	{
		CookingStepData[] allCookingSteps = GameUtils.GetAllCookingSteps();
		for (int i = 0; i < allCookingSteps.Length; i++)
		{
			if (allCookingSteps[i].m_uID == ID)
			{
				return allCookingSteps[i];
			}
		}
		return null;
	}

	// Token: 0x06003C6D RID: 15469 RVA: 0x00120A04 File Offset: 0x0011EE04
	public static GameObject[] GetAllIngredients()
	{
		GameObject[] a = GameObject.FindGameObjectsWithTag("Pre-Ingredient");
		GameObject[] b = GameObject.FindGameObjectsWithTag("Ingredient");
		return a.Union(b);
	}

	// Token: 0x06003C6E RID: 15470 RVA: 0x00120A30 File Offset: 0x0011EE30
	public static GameObject[] GetPreIngredients(OrderDefinitionNode _orderToBe)
	{
		List<GameObject> list = new List<GameObject>();
		GameObject[] array = GameObject.FindGameObjectsWithTag("Pre-Ingredient");
		for (int i = 0; i < array.Length; i++)
		{
			WorkableItem component = array[i].GetComponent<WorkableItem>();
			if (component)
			{
				GameObject nextPrefab = component.GetNextPrefab();
				IngredientPropertiesComponent ingredientPropertiesComponent = nextPrefab.RequireComponent<IngredientPropertiesComponent>();
				AssembledDefinitionNode orderComposition = ingredientPropertiesComponent.GetOrderComposition();
				if (AssembledDefinitionNode.Matching(_orderToBe, orderComposition))
				{
					list.Add(array[i]);
				}
			}
		}
		return list.ToArray();
	}

	// Token: 0x06003C6F RID: 15471 RVA: 0x00120AAC File Offset: 0x0011EEAC
	public static int GetWorkableChopsPerSlice()
	{
		int result = 1;
		int count = ClientUserSystem.m_Users.Count;
		GameSession gameSession = GameUtils.GetGameSession();
		GameSession.GameType type = gameSession.TypeSettings.Type;
		if (type != GameSession.GameType.Cooperative)
		{
			if (type == GameSession.GameType.Competitive)
			{
				result = ((count >= 4) ? 1 : GameUtils.GetGameConfig().SingleplayerChopTimeMultiplier);
			}
		}
		else
		{
			result = ((count != 1) ? 1 : GameUtils.GetGameConfig().SingleplayerChopTimeMultiplier);
		}
		return result;
	}

	// Token: 0x06003C70 RID: 15472 RVA: 0x00120B28 File Offset: 0x0011EF28
	private static EngagementSlot[] GetEngagedAndInGame()
	{
		EngagementSlot[] result = new EngagementSlot[0];
		PlayerManager playerManager = GameUtils.RequireManager<PlayerManager>();
		OvercookedEngagementController overcookedEngagementController = GameUtils.RequireManager<OvercookedEngagementController>();
		bool flag = overcookedEngagementController.GlobalLevelType == OvercookedEngagementController.LevelType.WithChefs;
		for (int i = 0; i < 4; i++)
		{
			EngagementSlot engagementSlot = (EngagementSlot)i;
			GamepadUser user = playerManager.GetUser(engagementSlot);
			if (user != null && (user.StickyEngagement || !flag))
			{
				ArrayUtils.PushBack<EngagementSlot>(ref result, engagementSlot);
			}
		}
		return result;
	}

	// Token: 0x06003C71 RID: 15473 RVA: 0x00120BA0 File Offset: 0x0011EFA0
	public static void FireDeedOnAllChefs<T>(params object[] _params) where T : DeedManagerBase.PadDeed
	{
		DeedManager deedManager = GameUtils.RequireManager<DeedManager>();
		EngagementSlot[] engagedAndInGame = GameUtils.GetEngagedAndInGame();
		for (int i = 0; i < engagedAndInGame.Length; i++)
		{
			GameUtils.FireDeed<T>((ControlPadInput.PadNum)engagedAndInGame[i], _params);
		}
	}

	// Token: 0x06003C72 RID: 15474 RVA: 0x00120BD8 File Offset: 0x0011EFD8
	public static void FireDeed<T>(ControlPadInput.PadNum _player, params object[] _params) where T : DeedManagerBase.Deed
	{
		DeedManager deedManager = GameUtils.RequireManager<DeedManager>();
		Type[] array = new Type[_params.Length + 1];
		array[0] = typeof(ControlPadInput.PadNum);
		for (int i = 0; i < _params.Length; i++)
		{
			array[i + 1] = _params[i].GetType();
		}
		ConstructorInfo constructor = typeof(T).GetConstructor(array);
		object[] array2 = new object[_params.Length + 1];
		array2[0] = _player;
		_params.CopyTo(array2, 1);
		object obj = constructor.Invoke(null, array2);
		deedManager.FireDeed<T>(obj as T);
	}

	// Token: 0x06003C73 RID: 15475 RVA: 0x00120C74 File Offset: 0x0011F074
	public static GameObject[] GetIngredients(OrderDefinitionNode _orderToBe)
	{
		List<GameObject> list = new List<GameObject>();
		GameObject[] array = GameObject.FindGameObjectsWithTag("Ingredient");
		for (int i = 0; i < array.Length; i++)
		{
			IngredientPropertiesComponent ingredientPropertiesComponent = array[i].RequireComponent<IngredientPropertiesComponent>();
			AssembledDefinitionNode orderComposition = ingredientPropertiesComponent.GetOrderComposition();
			if (AssembledDefinitionNode.Matching(_orderToBe, orderComposition))
			{
				list.Add(array[i]);
			}
		}
		return list.ToArray();
	}

	// Token: 0x06003C74 RID: 15476 RVA: 0x00120CD4 File Offset: 0x0011F0D4
	public static GameObject[] GetIngredientCrates(OrderDefinitionNode _node)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Crate");
		Predicate<GameObject> match = delegate(GameObject _crate)
		{
			ClientPickupItemSpawner clientPickupItemSpawner = _crate.RequireComponent<ClientPickupItemSpawner>();
			GameObject itemPrefab = clientPickupItemSpawner.GetItemPrefab();
			WorkableItem component = itemPrefab.GetComponent<WorkableItem>();
			if (component != null)
			{
				GameObject nextPrefab = component.GetNextPrefab();
				IngredientPropertiesComponent component2 = nextPrefab.GetComponent<IngredientPropertiesComponent>();
				if (component2)
				{
					return AssembledDefinitionNode.Matching(_node, component2.GetOrderComposition());
				}
			}
			return false;
		};
		return array.FindAll(match);
	}

	// Token: 0x06003C75 RID: 15477 RVA: 0x00120D10 File Offset: 0x0011F110
	public static GameObject[] FindContainersWithSubset(string _tag, AssembledDefinitionNode[] _contents, GameObject[] _containers = null)
	{
		GameObject[] array = _containers;
		if (array == null)
		{
			array = GameObject.FindGameObjectsWithTag(_tag);
		}
		Predicate<GameObject> match = delegate(GameObject _obj)
		{
			if (_obj == null || !_obj.CompareTag(_tag) || !_obj.gameObject.activeInHierarchy)
			{
				return false;
			}
			IIngredientContents ingredientContents = _obj.RequestInterface<IIngredientContents>();
			if (ingredientContents == null)
			{
				return false;
			}
			AssembledDefinitionNode[] contents = ingredientContents.GetContents();
			bool[] array2 = new bool[contents.Length];
			for (int i = 0; i < _contents.Length; i++)
			{
				bool flag = false;
				for (int j = 0; j < contents.Length; j++)
				{
					if (!array2[j] && AssembledDefinitionNode.Matching(_contents[i], contents[j]))
					{
						flag = true;
						array2[j] = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		};
		return array.FindAll(match);
	}

	// Token: 0x06003C76 RID: 15478 RVA: 0x00120D5C File Offset: 0x0011F15C
	public static GameObject[] FindEmptyContainers(string _tag, GameObject[] _containers = null)
	{
		GameObject[] array = _containers;
		if (array == null)
		{
			array = GameObject.FindGameObjectsWithTag(_tag);
		}
		Predicate<GameObject> match = delegate(GameObject _obj)
		{
			if (_obj == null || !_obj.CompareTag(_tag) || !_obj.gameObject.activeInHierarchy)
			{
				return false;
			}
			IIngredientContents ingredientContents = _obj.RequestInterface<IIngredientContents>();
			return ingredientContents != null && ingredientContents.GetContents().Length == 0;
		};
		return array.FindAll(match);
	}

	// Token: 0x06003C77 RID: 15479 RVA: 0x00120DA0 File Offset: 0x0011F1A0
	public static GameObject[] GetPlayerHeldItems()
	{
		List<GameObject> list = new List<GameObject>();
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < array.Length; i++)
		{
			ICarrier carrier = array[i].RequireInterface<ICarrier>();
			GameObject gameObject = carrier.InspectCarriedItem();
			if (gameObject != null)
			{
				list.Add(gameObject);
			}
		}
		return list.ToArray();
	}

	// Token: 0x06003C78 RID: 15480 RVA: 0x00120E00 File Offset: 0x0011F200
	public static void QuitLevel()
	{
		GameSession gameSession = GameUtils.GetGameSession();
		if (gameSession == null)
		{
			ServerGameSetup.Mode = GameMode.OnlineKitchen;
			ServerMessenger.LoadLevel("StartScreen", GameState.MainMenu, true, GameState.NotSet);
			return;
		}
		GameSession.GameTypeSettings typeSettings = gameSession.TypeSettings;
		if (ConnectionStatus.IsHost())
		{
			if (SceneManager.GetActiveScene().name == typeSettings.WorldMapScene)
			{
				ServerGameSetup.Mode = GameMode.OnlineKitchen;
				ServerOptions serverOptions = (ServerOptions)ConnectionModeSwitcher.GetAgentData();
				serverOptions.gameMode = GameMode.OnlineKitchen;
				serverOptions.visibility = OnlineMultiplayerSessionVisibility.ePrivate;
				ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Server, serverOptions, null);
				ServerMessenger.LoadLevel("StartScreen", GameState.MainMenu, true, GameState.NotSet);
			}
			else
			{
				switch (ClientGameSetup.Mode)
				{
				case GameMode.Campaign:
				{
					GameProgress progress = gameSession.Progress;
					if (progress.LoadFirstScene && !progress.SaveData.IsLevelComplete(gameSession.Progress.FirstSceneIndex) && !DebugManager.Instance.GetOption("Unlock all levels"))
					{
						ServerGameSetup.Mode = GameMode.OnlineKitchen;
						ServerOptions serverOptions2 = (ServerOptions)ConnectionModeSwitcher.GetAgentData();
						serverOptions2.gameMode = GameMode.OnlineKitchen;
						serverOptions2.visibility = OnlineMultiplayerSessionVisibility.ePrivate;
						ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Server, serverOptions2, null);
						ServerMessenger.LoadLevel("StartScreen", GameState.MainMenu, true, GameState.NotSet);
					}
					else
					{
						gameSession.FillShownMetaDialogStatus();
						ServerMessenger.GameProgressData(gameSession.Progress.SaveData, gameSession.m_shownMetaDialogs);
						ServerMessenger.LoadLevel(typeSettings.WorldMapScene, GameState.CampaignMap, true, GameState.RunMapUnfoldRoutine);
					}
					break;
				}
				case GameMode.Party:
				case GameMode.Versus:
				{
					ServerUserSystem.RemoveMatchmadeUsers();
					bool flag = false;
					for (int i = 0; i < ServerUserSystem.m_Users.Count; i++)
					{
						if (!ServerUserSystem.m_Users._items[i].IsLocal)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						ServerGameSetup.Mode = GameMode.OnlineKitchen;
						ServerOptions serverOptions3 = (ServerOptions)ConnectionModeSwitcher.GetAgentData();
						serverOptions3.gameMode = GameMode.OnlineKitchen;
						serverOptions3.visibility = OnlineMultiplayerSessionVisibility.ePrivate;
						ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Server, serverOptions3, null);
					}
					else
					{
						NetConnectionState state = NetConnectionState.Offline;
						object data = null;
						if (GameUtils.<>f__mg$cache0 == null)
						{
							GameUtils.<>f__mg$cache0 = new GenericVoid<IConnectionModeSwitchStatus>(GameUtils.OnQuitLevelOfflineRequestResult);
						}
						ConnectionModeSwitcher.RequestConnectionState(state, data, GameUtils.<>f__mg$cache0);
					}
					ServerMessenger.LoadLevel("StartScreen", GameState.MainMenu, true, GameState.NotSet);
					break;
				}
				}
			}
		}
		else if (ConnectionStatus.IsInSession())
		{
			ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, null, null);
			LoadingScreenFlow.LoadScene("StartScreen", GameState.NotSet);
		}
		else if (SceneManager.GetActiveScene().name == typeSettings.WorldMapScene)
		{
			ServerGameSetup.Mode = GameMode.OnlineKitchen;
			ServerMessenger.LoadLevel("StartScreen", GameState.MainMenu, true, GameState.NotSet);
		}
		else
		{
			switch (ClientGameSetup.Mode)
			{
			case GameMode.Campaign:
			{
				GameProgress progress2 = gameSession.Progress;
				if (progress2.LoadFirstScene && !progress2.SaveData.IsLevelComplete(gameSession.Progress.FirstSceneIndex) && !DebugManager.Instance.GetOption("Unlock all levels"))
				{
					ServerGameSetup.Mode = GameMode.OnlineKitchen;
					ServerMessenger.LoadLevel("StartScreen", GameState.MainMenu, true, GameState.NotSet);
				}
				else
				{
					ServerMessenger.LoadLevel(typeSettings.WorldMapScene, GameState.CampaignMap, true, GameState.RunMapUnfoldRoutine);
				}
				break;
			}
			case GameMode.Party:
				ServerMessenger.LoadLevel(typeSettings.WorldMapScene, GameState.PartyLobby, true, GameState.NotSet);
				break;
			case GameMode.Versus:
				ServerMessenger.LoadLevel(typeSettings.WorldMapScene, GameState.VSLobby, true, GameState.NotSet);
				break;
			}
		}
	}

	// Token: 0x06003C79 RID: 15481 RVA: 0x00121154 File Offset: 0x0011F554
	public static void OnQuitLevelOfflineRequestResult(IConnectionModeSwitchStatus _status)
	{
		if (_status.GetResult() == eConnectionModeSwitchResult.Success)
		{
			ServerGameSetup.Mode = GameMode.OnlineKitchen;
		}
		else if (_status.GetResult() == eConnectionModeSwitchResult.Failure)
		{
			OfflineOptions offlineOptions = default(OfflineOptions);
			offlineOptions.hostUser = GameUtils.RequireManager<PlayerManager>().GetUser(EngagementSlot.One);
			offlineOptions.connectionMode = new OnlineMultiplayerConnectionMode?(OnlineMultiplayerConnectionMode.eNone);
			NetConnectionState state = NetConnectionState.Offline;
			object data = offlineOptions;
			if (GameUtils.<>f__mg$cache1 == null)
			{
				GameUtils.<>f__mg$cache1 = new GenericVoid<IConnectionModeSwitchStatus>(GameUtils.OnQuitLevelFullOfflineRequestResult);
			}
			ConnectionModeSwitcher.RequestConnectionState(state, data, GameUtils.<>f__mg$cache1);
		}
	}

	// Token: 0x06003C7A RID: 15482 RVA: 0x001211D5 File Offset: 0x0011F5D5
	public static void OnQuitLevelFullOfflineRequestResult(IConnectionModeSwitchStatus _status)
	{
		if (_status.GetResult() == eConnectionModeSwitchResult.Success)
		{
			ServerGameSetup.Mode = GameMode.OnlineKitchen;
		}
	}

	// Token: 0x06003C7B RID: 15483 RVA: 0x001211E9 File Offset: 0x0011F5E9
	public static void LoadLevel(string _levelName)
	{
		GameUtils.LoadScene(_levelName, LoadSceneMode.Single);
	}

	// Token: 0x06003C7C RID: 15484 RVA: 0x001211F4 File Offset: 0x0011F5F4
	public static void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
	{
		if (GameUtils.CanLoadScene(sceneName))
		{
			if (mode == LoadSceneMode.Single)
			{
				ScreenTransitionManager screenTransitionManager = GameUtils.RequireManager<ScreenTransitionManager>();
				screenTransitionManager.TransitionLoad(sceneName);
			}
			else
			{
				AssetBundleManager.LoadLevel(sceneName.ToLowerInvariant(), sceneName, mode == LoadSceneMode.Additive);
			}
		}
	}

	// Token: 0x06003C7D RID: 15485 RVA: 0x00121234 File Offset: 0x0011F634
	public static bool CanLoadScene(string sceneName)
	{
		return true;
	}

	// Token: 0x06003C7E RID: 15486 RVA: 0x00121237 File Offset: 0x0011F637
	public static int GetRequiredBitCount(int value)
	{
		return (int)(Math.Log((double)value, 2.0) + 1.0);
	}

	// Token: 0x06003C7F RID: 15487 RVA: 0x00121254 File Offset: 0x0011F654
	public static void InitialiseAnalytics(string serverUrl, string gameKey, string secretKey, string userId, string buildVersion)
	{
		if (GameUtils.s_AnalyticsHelper == null)
		{
			DeviceInfo.PlatformEnum platform = DeviceInfo.PlatformEnum.STEAM;
			GameUtils.s_AnalyticsHelper = new Helper(serverUrl, gameKey, secretKey, platform, userId, buildVersion);
			GameUtils.s_AnalyticsHelper.EnableAnalytics(GameUtils.s_AnalyticsEnabled);
		}
	}

	// Token: 0x06003C80 RID: 15488 RVA: 0x00121290 File Offset: 0x0011F690
	public static void ShutdownAnalytics()
	{
		GameUtils.s_AnalyticsHelper.EnableAnalytics(false);
		GameUtils.s_AnalyticsHelper = null;
	}

	// Token: 0x06003C81 RID: 15489 RVA: 0x001212A4 File Offset: 0x0011F6A4
	public static void SendDiagnosticEvent(string eventName)
	{
		if (GameUtils.GetT17AnalyticsHelper() != null)
		{
			GameUtils.GetT17AnalyticsHelper().AddDesignEvent(eventName, null);
		}
	}

	// Token: 0x06003C82 RID: 15490 RVA: 0x001212CF File Offset: 0x0011F6CF
	public static void SendDiagnosticValueEvent(string eventName, float value)
	{
		if (GameUtils.GetT17AnalyticsHelper() != null)
		{
			GameUtils.GetT17AnalyticsHelper().AddDesignEvent(eventName, new float?(value));
		}
	}

	// Token: 0x040030DF RID: 12511
	private static GameDebugConfig s_config;

	// Token: 0x040030E0 RID: 12512
	private static string s_sceneNameToLoad;

	// Token: 0x040030E1 RID: 12513
	private static Helper s_AnalyticsHelper;

	// Token: 0x040030E2 RID: 12514
	private static bool s_AnalyticsEnabled = true;

	// Token: 0x040030E3 RID: 12515
	private static int s_LastMatchedOrderIndex = -1;

	// Token: 0x040030E4 RID: 12516
	public static bool s_RoomSearch_NoneAvailable;

	// Token: 0x040030E5 RID: 12517
	[CompilerGenerated]
	private static GenericVoid<IConnectionModeSwitchStatus> <>f__mg$cache0;

	// Token: 0x040030E6 RID: 12518
	[CompilerGenerated]
	private static GenericVoid<IConnectionModeSwitchStatus> <>f__mg$cache1;
}
