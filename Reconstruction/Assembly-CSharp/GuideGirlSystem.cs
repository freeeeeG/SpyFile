using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x0200025D RID: 605
public class GuideGirlSystem : Singleton<GuideGirlSystem>
{
	// Token: 0x17000537 RID: 1335
	// (get) Token: 0x06000F21 RID: 3873 RVA: 0x00027EA0 File Offset: 0x000260A0
	// (set) Token: 0x06000F22 RID: 3874 RVA: 0x00027EA8 File Offset: 0x000260A8
	public int CurrentGuideIndex { get; set; }

	// Token: 0x06000F23 RID: 3875 RVA: 0x00027EB4 File Offset: 0x000260B4
	private void Start()
	{
		this.m_RootUI = base.transform.Find("Root");
		this.backBtn = this.m_RootUI.Find("BackBtn").gameObject;
		this.dialogTxt = this.m_RootUI.GetComponentInChildren<TextMeshProUGUI>();
		this.anim = base.GetComponent<Animator>();
		this.wordQueue = new Queue<string>();
		this.m_GuideBook.Initialize();
		Singleton<GameEvents>.Instance.onTempWord += this.DisplayTempDialogue;
	}

	// Token: 0x06000F24 RID: 3876 RVA: 0x00027F3C File Offset: 0x0002613C
	public void Initialize()
	{
		this.m_Dialogues = Singleton<LevelManager>.Instance.CurrentLevel.GuideDialogues;
		Singleton<Game>.Instance.Tutorial = (this.m_Dialogues.Length > 1);
		this.GuideObjList.Clear();
		this.GuideDIC.Clear();
		Singleton<GameEvents>.Instance.GuideObjCollect();
	}

	// Token: 0x06000F25 RID: 3877 RVA: 0x00027F93 File Offset: 0x00026193
	public void ShowGuideGirl(bool value, int posID)
	{
		if (value)
		{
			this.SetGirlPos(posID);
			this.Show();
			return;
		}
		this.Hide();
	}

	// Token: 0x06000F26 RID: 3878 RVA: 0x00027FAC File Offset: 0x000261AC
	public void AddGuideObj(GameObject obj)
	{
		using (List<GameObject>.Enumerator enumerator = this.GuideObjList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.name == obj.name)
				{
					return;
				}
			}
		}
		this.GuideObjList.Add(obj);
	}

	// Token: 0x06000F27 RID: 3879 RVA: 0x00028018 File Offset: 0x00026218
	private void InitializeGuideDIC()
	{
		foreach (GameObject gameObject in this.GuideObjList)
		{
			this.GuideDIC.Add(gameObject.name, gameObject);
		}
	}

	// Token: 0x06000F28 RID: 3880 RVA: 0x00028078 File Offset: 0x00026278
	public GameObject GetGuideObj(string name)
	{
		if (this.GuideDIC.ContainsKey(name))
		{
			return this.GuideDIC[name];
		}
		Debug.LogWarning("没有可以该教学物体:" + name);
		return null;
	}

	// Token: 0x06000F29 RID: 3881 RVA: 0x000280A6 File Offset: 0x000262A6
	public void PrepareTutorial()
	{
		this.InitializeGuideDIC();
		if (this.m_Dialogues != null && this.m_Dialogues.Length != 0)
		{
			this.StarTutorial();
		}
	}

	// Token: 0x06000F2A RID: 3882 RVA: 0x000280C5 File Offset: 0x000262C5
	private void StarTutorial()
	{
		Singleton<GameEvents>.Instance.onTutorialTrigger += this.GuideTrigger;
		this.CurrentGuideIndex = this.startIndex;
		this.currentDialogue = this.m_Dialogues[this.CurrentGuideIndex];
		this.GuideTrigger(TutorialType.None);
	}

	// Token: 0x06000F2B RID: 3883 RVA: 0x00028103 File Offset: 0x00026303
	public void Release()
	{
		this.m_GuideBook.Hide();
		Singleton<GameEvents>.Instance.onTutorialTrigger -= this.GuideTrigger;
		Singleton<GameEvents>.Instance.onTempWord -= this.DisplayTempDialogue;
	}

	// Token: 0x06000F2C RID: 3884 RVA: 0x0002813C File Offset: 0x0002633C
	private void DisplayTempDialogue(TempWord wordType)
	{
		if (this.typingSentence || Singleton<Game>.Instance.Tutorial)
		{
			return;
		}
		switch (wordType.WordType)
		{
		case TempWordType.StandardLose:
			base.StartCoroutine(this.TempWordCor(Singleton<LevelManager>.Instance.CurrentLevel.LostDialogue, 0));
			return;
		case TempWordType.StandardWin:
			base.StartCoroutine(this.TempWordCor(Singleton<LevelManager>.Instance.CurrentLevel.WinDialogue, 0));
			return;
		case TempWordType.EndlessEnd:
			base.StartCoroutine(this.TempWordCor(Singleton<LevelManager>.Instance.CurrentLevel.WinDialogue, wordType.ID));
			return;
		case TempWordType.RefreshShop:
			break;
		case TempWordType.Refactor:
			if (Random.value > 0.95f)
			{
				base.StartCoroutine(this.TempWordCor(this.RefactorDialogue, Random.Range(0, this.RefactorDialogue.Words.Length - 1)));
				return;
			}
			break;
		case TempWordType.Demo:
			base.StartCoroutine(this.TempWordCor(this.DemoDialogue, wordType.ID));
			return;
		case TempWordType.DieProtect:
			base.StartCoroutine(this.TempWordCor(this.DieProtectDialogue, wordType.ID));
			break;
		case TempWordType.WaveEnd:
		{
			int num = 99;
			int id = wordType.ID;
			if (id != 31)
			{
				if (id != 61)
				{
					if (id == 100)
					{
						num = 2;
					}
				}
				else
				{
					num = 1;
				}
			}
			else
			{
				num = 0;
			}
			if (num != 99)
			{
				base.StartCoroutine(this.TempWordCor(Singleton<LevelManager>.Instance.CurrentLevel.WaveDialogue[num], Random.Range(0, Singleton<LevelManager>.Instance.CurrentLevel.WaveDialogue[num].Words.Length - 1)));
				return;
			}
			break;
		}
		default:
			return;
		}
	}

	// Token: 0x06000F2D RID: 3885 RVA: 0x000282BF File Offset: 0x000264BF
	private IEnumerator TempWordCor(DialogueData dialogue, int id = 0)
	{
		this.typingSentence = true;
		this.SetGirlPos(1);
		dialogue.TriggerGuideStartEvents();
		this.Show();
		this.backBtn.SetActive(false);
		this.dialogTxt.text = "";
		yield return new WaitForSeconds(0.5f);
		string traduction = GameMultiLang.GetTraduction(dialogue.Words[id]);
		this.dialogTxt.text = traduction;
		this.dialogTxt.maxVisibleCharacters = 0;
		this.dialogTxt.ForceMeshUpdate(false, false);
		TMP_TextInfo textInfo = this.dialogTxt.textInfo;
		for (int i = 0; i < textInfo.characterCount; i++)
		{
			this.SetCharacterAlpha(i, 0);
		}
		float timer = 0f;
		float interval = 0.03f;
		while (this.dialogTxt.maxVisibleCharacters < textInfo.characterCount)
		{
			timer += Time.deltaTime;
			if (timer >= interval)
			{
				timer = 0f;
				TextMeshProUGUI textMeshProUGUI = this.dialogTxt;
				int maxVisibleCharacters = textMeshProUGUI.maxVisibleCharacters;
				textMeshProUGUI.maxVisibleCharacters = maxVisibleCharacters + 1;
			}
			yield return null;
		}
		yield return new WaitForSeconds(8f);
		this.Hide();
		this.typingSentence = false;
		yield break;
	}

	// Token: 0x06000F2E RID: 3886 RVA: 0x000282DC File Offset: 0x000264DC
	private void StartDialogue()
	{
		this.backBtn.SetActive(true);
		this.wordQueue.Clear();
		string[] words = this.currentDialogue.Words;
		for (int i = 0; i < words.Length; i++)
		{
			string traduction = GameMultiLang.GetTraduction(words[i]);
			this.wordQueue.Enqueue(traduction);
		}
		this.DisplayNextSentence();
	}

	// Token: 0x06000F2F RID: 3887 RVA: 0x00028338 File Offset: 0x00026538
	private void EndDialogue()
	{
		this.backBtn.SetActive(false);
		this.currentDialogue.TriggerGuideEndEvents();
		if (this.CurrentGuideIndex < this.m_Dialogues.Length - 1)
		{
			int currentGuideIndex = this.CurrentGuideIndex;
			this.CurrentGuideIndex = currentGuideIndex + 1;
			this.currentDialogue = this.m_Dialogues[this.CurrentGuideIndex];
			this.GuideTrigger(TutorialType.None);
			return;
		}
		this.Hide();
	}

	// Token: 0x06000F30 RID: 3888 RVA: 0x000283A0 File Offset: 0x000265A0
	private void DisplayNextSentence()
	{
		if (this.wordQueue.Count == 0)
		{
			this.EndDialogue();
			return;
		}
		string word = this.wordQueue.Dequeue();
		base.StopAllCoroutines();
		base.StartCoroutine(this.TypeSentence(word));
	}

	// Token: 0x06000F31 RID: 3889 RVA: 0x000283E1 File Offset: 0x000265E1
	private IEnumerator TypeSentence(string word)
	{
		this.ClickTip.SetActive(true);
		this.typingSentence = true;
		this.dialogTxt.text = word;
		this.dialogTxt.maxVisibleCharacters = 0;
		this.dialogTxt.ForceMeshUpdate(false, false);
		TMP_TextInfo textInfo = this.dialogTxt.textInfo;
		for (int i = 0; i < textInfo.characterCount; i++)
		{
			this.SetCharacterAlpha(i, 0);
		}
		float timer = 0f;
		float interval = 0.03f;
		while (this.dialogTxt.maxVisibleCharacters < textInfo.characterCount)
		{
			timer += Time.deltaTime;
			if (timer >= interval)
			{
				timer = 0f;
				TextMeshProUGUI textMeshProUGUI = this.dialogTxt;
				int maxVisibleCharacters = textMeshProUGUI.maxVisibleCharacters;
				textMeshProUGUI.maxVisibleCharacters = maxVisibleCharacters + 1;
			}
			yield return null;
		}
		this.typingSentence = false;
		if (this.wordQueue.Count == 0 && this.currentDialogue.DontNeedClickEnd)
		{
			this.EndDialogue();
		}
		yield break;
	}

	// Token: 0x06000F32 RID: 3890 RVA: 0x000283F8 File Offset: 0x000265F8
	private void SetCharacterAlpha(int index, byte alpha)
	{
		int materialReferenceIndex = this.dialogTxt.textInfo.characterInfo[index].materialReferenceIndex;
		Color32[] colors = this.dialogTxt.textInfo.meshInfo[materialReferenceIndex].colors32;
		int vertexIndex = this.dialogTxt.textInfo.characterInfo[index].vertexIndex;
		colors[vertexIndex].a = alpha;
		colors[vertexIndex + 1].a = alpha;
		colors[vertexIndex + 2].a = alpha;
		colors[vertexIndex + 3].a = alpha;
	}

	// Token: 0x06000F33 RID: 3891 RVA: 0x00028491 File Offset: 0x00026691
	public void GuideTrigger(TutorialType triggetType = TutorialType.None)
	{
		if (!Singleton<Game>.Instance.Tutorial)
		{
			return;
		}
		if (this.currentDialogue.JudgeConditions(triggetType))
		{
			this.currentDialogue.TriggerGuideStartEvents();
			base.Invoke("StartDialogue", this.currentDialogue.WaitingTime);
		}
	}

	// Token: 0x06000F34 RID: 3892 RVA: 0x000284CF File Offset: 0x000266CF
	public void NextBtnClick()
	{
		if (!this.typingSentence)
		{
			this.DisplayNextSentence();
		}
	}

	// Token: 0x06000F35 RID: 3893 RVA: 0x000284DF File Offset: 0x000266DF
	public void Show()
	{
		this.m_RootUI.gameObject.SetActive(true);
		Singleton<Sound>.Instance.PlayUISound("Sound_Guide");
		this.anim.SetBool("Show", true);
	}

	// Token: 0x06000F36 RID: 3894 RVA: 0x00028512 File Offset: 0x00026712
	public void Hide()
	{
		this.anim.SetBool("Show", false);
	}

	// Token: 0x06000F37 RID: 3895 RVA: 0x00028525 File Offset: 0x00026725
	public void HideRoot()
	{
		this.m_RootUI.gameObject.SetActive(false);
	}

	// Token: 0x06000F38 RID: 3896 RVA: 0x00028538 File Offset: 0x00026738
	public void SetGirlPos(int posID)
	{
		if (posID == 0)
		{
			this.m_GirlTr.anchorMin = new Vector2(0.5f, 0f);
			this.m_GirlTr.anchorMax = new Vector2(0.5f, 0f);
			this.m_GirlTr.anchoredPosition = new Vector2(0f, 250f);
			return;
		}
		if (posID != 1)
		{
			return;
		}
		this.m_GirlTr.anchorMin = new Vector2(0f, 0f);
		this.m_GirlTr.anchorMax = new Vector2(0f, 0f);
		this.m_GirlTr.anchoredPosition = new Vector2(380f, 100f);
	}

	// Token: 0x06000F39 RID: 3897 RVA: 0x000285EA File Offset: 0x000267EA
	public void ShowGuideBook(int index)
	{
		this.m_GuideBook.Show();
		this.m_GuideBook.ShowPage(index);
	}

	// Token: 0x06000F3A RID: 3898 RVA: 0x00028603 File Offset: 0x00026803
	public void SetRectMaskObj(GameObject obj, float delayTime)
	{
		this.m_RectMaskController.SetTarget(obj, delayTime);
	}

	// Token: 0x06000F3B RID: 3899 RVA: 0x00028612 File Offset: 0x00026812
	public void SetEventPermeaterTarget(GameObject obj)
	{
		this.m_EventPermeater.SetTarget(obj);
	}

	// Token: 0x04000788 RID: 1928
	[SerializeField]
	private int startIndex;

	// Token: 0x0400078A RID: 1930
	private const float DialogueTime = 8f;

	// Token: 0x0400078B RID: 1931
	private Transform m_RootUI;

	// Token: 0x0400078C RID: 1932
	[SerializeField]
	private RectTransform m_GirlTr;

	// Token: 0x0400078D RID: 1933
	private Animator anim;

	// Token: 0x0400078E RID: 1934
	private GameObject backBtn;

	// Token: 0x0400078F RID: 1935
	private DialogueData currentDialogue;

	// Token: 0x04000790 RID: 1936
	private bool typingSentence;

	// Token: 0x04000791 RID: 1937
	private TextMeshProUGUI dialogTxt;

	// Token: 0x04000792 RID: 1938
	private Queue<string> wordQueue;

	// Token: 0x04000793 RID: 1939
	private DialogueData[] m_Dialogues;

	// Token: 0x04000794 RID: 1940
	[Header("小姐姐临时对话")]
	[SerializeField]
	private DialogueData RefactorDialogue;

	// Token: 0x04000795 RID: 1941
	[SerializeField]
	private DialogueData DemoDialogue;

	// Token: 0x04000796 RID: 1942
	[SerializeField]
	private DialogueData DieProtectDialogue;

	// Token: 0x04000797 RID: 1943
	[Header("教学物体")]
	[SerializeField]
	private List<GameObject> GuideObjList = new List<GameObject>();

	// Token: 0x04000798 RID: 1944
	private Dictionary<string, GameObject> GuideDIC = new Dictionary<string, GameObject>();

	// Token: 0x04000799 RID: 1945
	[SerializeField]
	private GameObject ClickTip;

	// Token: 0x0400079A RID: 1946
	[Header("其他")]
	[SerializeField]
	private RectMaskController m_RectMaskController;

	// Token: 0x0400079B RID: 1947
	[SerializeField]
	private EventPermeater m_EventPermeater;

	// Token: 0x0400079C RID: 1948
	[SerializeField]
	private GuideBookUI m_GuideBook;
}
