using System;
using System.Collections;
using PlayFab;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000110 RID: 272
[RequireComponent(typeof(Sound))]
public class Game : Singleton<Game>
{
	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x060006B9 RID: 1721 RVA: 0x0001266E File Offset: 0x0001086E
	public string CurrentState
	{
		get
		{
			return this.m_SceneStateController.m_State.StateName;
		}
	}

	// Token: 0x060006BA RID: 1722 RVA: 0x00012680 File Offset: 0x00010880
	protected override void Awake()
	{
		base.Awake();
		if (!this.alreadyExist)
		{
			Application.runInBackground = true;
			Object.DontDestroyOnLoad(base.gameObject);
			Singleton<StaticData>.Instance.Initialize();
			TurretSkillFactory.Initialize();
			EnemyBuffFactory.Initialize();
			TurretBuffFactory.Initialize();
			TechnologyFactory.Initialize();
			RuleFactory.Initialize();
		}
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x000126D0 File Offset: 0x000108D0
	private void Start()
	{
		int buildIndex = SceneManager.GetActiveScene().buildIndex;
		if (buildIndex != 0)
		{
			if (buildIndex == 1)
			{
				Singleton<LevelManager>.Instance.LoadGame();
				this.m_SceneStateController.SetState(new BattleState(this.m_SceneStateController));
			}
		}
		else
		{
			Singleton<StaticData>.Instance.ContentFactory.SetDefaultRecipes();
			this.m_SceneStateController.SetState(new MenuState(this.m_SceneStateController));
		}
		Singleton<Sound>.Instance.BgVolume = 0.5f;
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x0001274C File Offset: 0x0001094C
	private void Update()
	{
		this.m_SceneStateController.StateUpdate();
		if (this.TestMode)
		{
			if (Input.GetKeyDown(KeyCode.K))
			{
				Singleton<TipsManager>.Instance.ShowMessage(GameMultiLang.GetTraduction("TEST1"));
				Singleton<LevelManager>.Instance.SetGameLevel(0);
				Singleton<LevelManager>.Instance.GameExp = 0;
				Singleton<LevelManager>.Instance.PassDiifcutly = 0;
				PlayerPrefs.DeleteAll();
				base.Invoke("ReloadScene", 1f);
			}
			if (Input.GetKeyDown(KeyCode.J))
			{
				Singleton<TipsManager>.Instance.ShowMessage(GameMultiLang.GetTraduction("TEST2"));
				Singleton<LevelManager>.Instance.SetGameLevel(99);
				Singleton<LevelManager>.Instance.GameExp = 0;
				Singleton<LevelManager>.Instance.PassDiifcutly = 6;
				PlayerPrefs.SetInt("MaxDifficulty", 6);
				base.Invoke("ReloadScene", 1f);
			}
		}
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x0001281C File Offset: 0x00010A1C
	public void LoadScene(int index)
	{
		base.StartCoroutine(this.Transition(index));
	}

	// Token: 0x060006BE RID: 1726 RVA: 0x0001282C File Offset: 0x00010A2C
	public void ReloadScene()
	{
		this.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x0001284C File Offset: 0x00010A4C
	private IEnumerator Transition(int index)
	{
		this.OnTransition = true;
		this.transition.SetTrigger("Start");
		this.m_SceneStateController.EndState();
		yield return new WaitForSeconds(this.transitionTime);
		SceneManager.LoadScene(index, LoadSceneMode.Single);
		this.OnTransition = false;
		yield return SceneManager.LoadSceneAsync(index);
		if (index != 0)
		{
			if (index == 1)
			{
				this.m_SceneStateController.SetState(new BattleState(this.m_SceneStateController));
			}
		}
		else
		{
			this.m_SceneStateController.SetState(new MenuState(this.m_SceneStateController));
		}
		this.transition.SetTrigger("End");
		this.globalCanvas.worldCamera = Camera.main;
		yield break;
	}

	// Token: 0x060006C0 RID: 1728 RVA: 0x00012862 File Offset: 0x00010A62
	public bool InitializeNetworks()
	{
		SteamManager.Instance.Initialize();
		Singleton<PlayfabManager>.Instance.Login();
		return SteamManager.Initialized && PlayFabClientAPI.IsClientLoggedIn();
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x00012886 File Offset: 0x00010A86
	public void QuitGame()
	{
		Application.Quit();
	}

	// Token: 0x04000314 RID: 788
	private SceneStateController m_SceneStateController = new SceneStateController();

	// Token: 0x04000315 RID: 789
	public Animator transition;

	// Token: 0x04000316 RID: 790
	public float transitionTime = 0.8f;

	// Token: 0x04000317 RID: 791
	public bool Tutorial;

	// Token: 0x04000318 RID: 792
	public bool OnTransition;

	// Token: 0x04000319 RID: 793
	public bool TestMode;

	// Token: 0x0400031A RID: 794
	[SerializeField]
	private Canvas globalCanvas;
}
