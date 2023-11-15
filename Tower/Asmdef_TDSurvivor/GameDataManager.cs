using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000080 RID: 128
public class GameDataManager : MonoBehaviour
{
	// Token: 0x17000032 RID: 50
	// (get) Token: 0x060002A3 RID: 675 RVA: 0x0000B202 File Offset: 0x00009402
	public PlayerLifetimeData Playerdata
	{
		get
		{
			return this.playerdata;
		}
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x060002A4 RID: 676 RVA: 0x0000B20A File Offset: 0x0000940A
	public GameplayData GameplayData
	{
		get
		{
			return this.gameplayData;
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000B212 File Offset: 0x00009412
	public IntermediateData IntermediateData
	{
		get
		{
			return this.intermediateData;
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000B21A File Offset: 0x0000941A
	public bool HaveSaveData
	{
		get
		{
			return this.haveSaveData;
		}
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x0000B222 File Offset: 0x00009422
	private void Awake()
	{
		if (GameDataManager.instance == null)
		{
			GameDataManager.instance = this;
			base.transform.SetParent(null);
			Object.DontDestroyOnLoad(base.gameObject);
			this.LoadData();
			return;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x0000B260 File Offset: 0x00009460
	private void OnDestroy()
	{
		if (!this.isInitialized)
		{
			return;
		}
		this.SaveData();
		if (this.playerdata != null)
		{
			this.playerdata.ClearEvents();
		}
		if (this.gameplayData != null)
		{
			this.gameplayData.ClearEvents();
		}
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x0000B298 File Offset: 0x00009498
	public void StartNewGame(int seed)
	{
		if (this.gameplayData != null)
		{
			this.gameplayData.ClearEvents();
			this.gameplayData = null;
		}
		seed = ((seed == -1) ? Random.Range(0, 99999999) : seed);
		this.gameplayData = new GameplayData(seed);
		this.gameplayData.SetGameStarted();
		Resources.Load<EnvSceneCollectionData>("EnvSceneCollectionData").ResetStageWeights();
	}

	// Token: 0x060002AA RID: 682 RVA: 0x0000B2F9 File Offset: 0x000094F9
	public void SetIntermediateData(IntermediateData data)
	{
		this.intermediateData = data;
	}

	// Token: 0x060002AB RID: 683 RVA: 0x0000B304 File Offset: 0x00009504
	public void SaveData()
	{
		ES3.Save<string>("gameVersion", Application.version);
		ES3.Save<PlayerLifetimeData>("playerdata", this.playerdata);
		ES3.Save<GameplayData>("gameplayData", this.gameplayData);
		ES3.Save<IntermediateData>("intermediateData", this.intermediateData);
	}

	// Token: 0x060002AC RID: 684 RVA: 0x0000B350 File Offset: 0x00009550
	public void LoadData()
	{
		Debug.Log("LOAD DATA...");
		if (!ES3.FileExists("SaveFile.es3"))
		{
			Debug.Log("找不到ES3存檔, 建立新的");
			this.playerdata = new PlayerLifetimeData();
			this.intermediateData = new IntermediateData();
		}
		else
		{
			this.playerdata = (ES3.Load("playerdata") as PlayerLifetimeData);
			this.gameplayData = (ES3.Load("gameplayData") as GameplayData);
			this.intermediateData = (ES3.Load("intermediateData") as IntermediateData);
			this.haveSaveData = true;
			this.gameplayData.RegisterEvents();
			this.gameplayData.LoadDataProcess();
		}
		this.playerdata.RegisterEvents();
	}

	// Token: 0x060002AD RID: 685 RVA: 0x0000B3FC File Offset: 0x000095FC
	public void ForceResetData()
	{
		if (ES3.FileExists("SaveFile.es3"))
		{
			ES3.DeleteFile("SaveFile.es3");
			this.LoadData();
		}
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	// Token: 0x04000323 RID: 803
	public static GameDataManager instance;

	// Token: 0x04000324 RID: 804
	[SerializeField]
	private PlayerLifetimeData playerdata;

	// Token: 0x04000325 RID: 805
	[SerializeField]
	private GameplayData gameplayData;

	// Token: 0x04000326 RID: 806
	[SerializeField]
	private IntermediateData intermediateData;

	// Token: 0x04000327 RID: 807
	private bool haveSaveData;

	// Token: 0x04000328 RID: 808
	private bool isInitialized;
}
