using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x020000A3 RID: 163
[SerializeField]
public class MapNode : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x17000046 RID: 70
	// (get) Token: 0x0600035F RID: 863 RVA: 0x0000D916 File Offset: 0x0000BB16
	// (set) Token: 0x06000360 RID: 864 RVA: 0x0000D91E File Offset: 0x0000BB1E
	public IMapElement MapElement { get; private set; }

	// Token: 0x06000361 RID: 865 RVA: 0x0000D928 File Offset: 0x0000BB28
	public void Initialize(MapNodeData mapNodeData, MapData mapData, Map mapManager)
	{
		this.mapNodeData = mapNodeData;
		this.mapdata = mapData;
		this.mapManager = mapManager;
		if (mapNodeData.HasStageReward())
		{
			StageRewardData stageReward = mapNodeData.stageReward;
			if (stageReward.RewardType == eStageRewardType.TOWER)
			{
				this.image_CompleteRewardIcon_Other.enabled = false;
				this.image_CompleteRewardIcon_Tower.enabled = true;
			}
			else if (stageReward.RewardType == eStageRewardType.TETRIS)
			{
				this.image_CompleteRewardIcon_Other.enabled = true;
				this.image_CompleteRewardIcon_Tower.enabled = false;
				AItemSettingData itemDataByType = Singleton<ResourceManager>.Instance.GetItemDataByType(stageReward.ItemType);
				this.image_CompleteRewardIcon_Other.sprite = itemDataByType.GetCardIcon();
				if (stageReward.ItemType.ToCardType() == eCardType.PANEL_CARD)
				{
					this.image_CompleteRewardIcon_Other.color = ((PanelSettingData)itemDataByType).GetSpriteColor();
				}
			}
		}
		this.MapElement = base.GetComponent<IMapElement>();
		this.UpdateNodeName();
		this.node_CompleteReward.SetActive(mapNodeData.HasStageReward() && (mapNodeData.State == eMapNodeState.AVALIABLE || mapNodeData.State == eMapNodeState.TOO_FAR));
	}

	// Token: 0x06000362 RID: 866 RVA: 0x0000DA23 File Offset: 0x0000BC23
	private void OnEnable()
	{
		EventMgr.Register(eGameEvents.OnLanguageChanged, new Action(this.OnLanguageChanged));
		this.button.onClick.AddListener(new UnityAction(this.OnButtonClick));
	}

	// Token: 0x06000363 RID: 867 RVA: 0x0000DA58 File Offset: 0x0000BC58
	private void OnDisable()
	{
		EventMgr.Remove(eGameEvents.OnLanguageChanged, new Action(this.OnLanguageChanged));
		this.button.onClick.RemoveListener(new UnityAction(this.OnButtonClick));
	}

	// Token: 0x06000364 RID: 868 RVA: 0x0000DA8D File Offset: 0x0000BC8D
	private void OnLanguageChanged()
	{
		this.UpdateNodeName();
	}

	// Token: 0x06000365 RID: 869 RVA: 0x0000DA98 File Offset: 0x0000BC98
	private void OnButtonClick()
	{
		if (this.mapNodeData.State == eMapNodeState.AVALIABLE)
		{
			this.animator.SetTrigger("Click_Valid");
			EventMgr.SendEvent<MapNode>(eMapSceneEvents.OnClickMapNodeButton, this);
			SoundManager.PlaySound("MapScene", "MapNode_Click_Valid", -1f, -1f, -1f);
			return;
		}
		this.animator.SetTrigger("Click_Invalid");
		SoundManager.PlaySound("MapScene", "MapNode_Click_Invalid", -1f, -1f, -1f);
	}

	// Token: 0x06000366 RID: 870 RVA: 0x0000DB20 File Offset: 0x0000BD20
	public void SwitchState(eMapNodeState state)
	{
		bool flag = this.IsState(eMapNodeState.COMPLETED) || this.IsState(eMapNodeState.JUST_FINISHED);
		List<MapNode> list = this.mapManager.GetNextStepMapNodes(this);
		list = (from a in list
		orderby a.mapNodeData.IndexInStep
		select a).ToList<MapNode>();
		List<bool> list2 = new List<bool>();
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(flag && list[i].DoShowLightLine());
		}
		switch (state)
		{
		case eMapNodeState.UNINITIALIZED:
			this.particle_FOWMask.gameObject.SetActive(false);
			this.SetLineRendererStyle(false, list2);
			break;
		case eMapNodeState.TOO_FAR:
			this.particle_FOWMask.gameObject.SetActive(false);
			this.SetLineRendererStyle(false, list2);
			break;
		case eMapNodeState.NOT_SELECTED:
			this.animator.SetBool("isNotSelected", true);
			this.particle_FOWMask.gameObject.SetActive(false);
			this.particle_FOWMask.transform.localScale = Vector3.one * 0.66f;
			this.SetLineRendererStyle(false, list2);
			break;
		case eMapNodeState.AVALIABLE:
			this.particle_FOWMask.gameObject.SetActive(true);
			this.particle_FOWMask.transform.localScale = Vector3.one * 0.75f;
			this.SetLineRendererStyle(false, list2);
			break;
		case eMapNodeState.JUST_FINISHED:
			this.particle_FOWMask.gameObject.SetActive(true);
			this.particle_FOWMask.transform.localScale = Vector3.one * 1f;
			this.SetLineRendererStyle(true, list2);
			break;
		case eMapNodeState.COMPLETED:
			this.particle_FOWMask.gameObject.SetActive(true);
			this.particle_FOWMask.transform.localScale = Vector3.one * 1f;
			this.SetLineRendererStyle(false, list2);
			break;
		}
		this.node_NextStageArrow.SetActive(state == eMapNodeState.AVALIABLE);
		this.text_DebugState.text = string.Format("({0}){1}\n{2}\n{3}", new object[]
		{
			this.mapNodeData.IndexInStep,
			state,
			this.mapNodeData.stageEnvSceneName,
			this.mapNodeData.difficulty
		});
		this.text_DebugState.enabled = Debug.isDebugBuild;
		this.mapNodeData.State = state;
	}

	// Token: 0x06000367 RID: 871 RVA: 0x0000DD87 File Offset: 0x0000BF87
	public void UpdateNodeName()
	{
		this.text_NodeName.text = LocalizationManager.Instance.GetString("UI", string.Format("MAPNODE_{0}", this.mapNodeData.MapNodeType), Array.Empty<object>());
	}

	// Token: 0x06000368 RID: 872 RVA: 0x0000DDC2 File Offset: 0x0000BFC2
	public bool IsState(eMapNodeState state)
	{
		return state == this.mapNodeData.State;
	}

	// Token: 0x06000369 RID: 873 RVA: 0x0000DDD2 File Offset: 0x0000BFD2
	public bool DoShowLightLine()
	{
		return this.IsState(eMapNodeState.AVALIABLE) || this.IsState(eMapNodeState.COMPLETED) || this.IsState(eMapNodeState.JUST_FINISHED);
	}

	// Token: 0x0600036A RID: 874 RVA: 0x0000DDF0 File Offset: 0x0000BFF0
	private void SetLineRendererStyle(bool isLineAvaliable, List<bool> doShowLightLine)
	{
		for (int i = 0; i < this.list_PathLines.Count; i++)
		{
			MapNodePathLine mapNodePathLine = this.list_PathLines[i];
			if (i < doShowLightLine.Count)
			{
				mapNodePathLine.SetLineType(isLineAvaliable ? MapNodePathLine.eLineType.AVALIABLE_PATH : MapNodePathLine.eLineType.DISABLED_PATH);
				mapNodePathLine.SetColorGradient(isLineAvaliable ? this.gradient_AvaliableLine : this.gradient_NormalLine);
				mapNodePathLine.SetMaterial(isLineAvaliable ? this.material_AvaliableLine : this.material_NormalLine);
				mapNodePathLine.SetColor(doShowLightLine[i] ? this.color_PathLineLit : this.color_PathLineUnlit);
			}
		}
	}

	// Token: 0x0600036B RID: 875 RVA: 0x0000DE81 File Offset: 0x0000C081
	public void Toggle(bool isOn, float sndPitch = 1f)
	{
		this.animator.SetBool("isOn", isOn);
		if (isOn)
		{
			SoundManager.PlaySound("MapScene", "MapNode_Show", sndPitch, -1f, -1f);
		}
	}

	// Token: 0x0600036C RID: 876 RVA: 0x0000DEB4 File Offset: 0x0000C0B4
	public void ToggleImmediate(bool isOn)
	{
		this.animator.SetBool("isOn", isOn);
		if (isOn)
		{
			this.animator.CrossFade("On", 0f, 0);
			return;
		}
		this.animator.CrossFade("Off", 0f, 0);
	}

	// Token: 0x0600036D RID: 877 RVA: 0x0000DF04 File Offset: 0x0000C104
	public void CalculateLines(List<MapNode> list_TargetNodes)
	{
		if (list_TargetNodes.Count == 0)
		{
			return;
		}
		foreach (MapNodePathLine mapNodePathLine in this.list_PathLines)
		{
			mapNodePathLine.SetColor(Color.black);
			mapNodePathLine.SetShowPercentage(0f);
		}
		for (int i = 0; i < list_TargetNodes.Count; i++)
		{
			MapNodePathLine mapNodePathLine2 = this.list_PathLines[i];
			List<Vector3> list = new List<Vector3>();
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = base.transform.InverseTransformPoint(list_TargetNodes[i].transform.position);
			Vector3 b = (vector2 - vector).normalized * this.pathToIconOffset;
			Vector3 b2 = (vector - vector2).normalized * this.pathToIconOffset;
			vector += b;
			vector2 += b2;
			float num = Random.Range(0f, 35f);
			if (vector.y < vector2.y)
			{
				num *= -1f;
			}
			list.AddRange(MathUtils.CreateCurveLine(vector, vector2, -1f * num, 15));
			mapNodePathLine2.SetPathPoints(list);
			mapNodePathLine2.SetupCollider(vector, vector2);
		}
	}

	// Token: 0x0600036E RID: 878 RVA: 0x0000E05C File Offset: 0x0000C25C
	public void SetPathShowPercentage(float percentage)
	{
		for (int i = 0; i < this.list_PathLines.Count; i++)
		{
			this.list_PathLines[i].SetShowPercentage(percentage);
		}
	}

	// Token: 0x0600036F RID: 879 RVA: 0x0000E094 File Offset: 0x0000C294
	public void SetCompleted(bool isCompleted, bool isFastForward)
	{
		if (isFastForward)
		{
			this.animator.SetBool("isCompleted", isCompleted);
			this.animator.CrossFade("On", 0f, 1);
			return;
		}
		this.animator.SetBool("isCompleted", isCompleted);
		if (isCompleted)
		{
			if (this.mapNodeData.MapNodeType == eStageType.BATTLE)
			{
				SoundManager.PlaySound("MapScene", "MapNode_Battle_Complete", -1f, -1f, -1f);
			}
			else
			{
				SoundManager.PlaySound("MapScene", "MapNode_Other_Complete", -1f, -1f, -1f);
			}
		}
		this.particle_StageComplete.Play();
	}

	// Token: 0x06000370 RID: 880 RVA: 0x0000E139 File Offset: 0x0000C339
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.animator.SetBool("isMouseOver", true);
		SoundManager.PlaySound("MapScene", "MapNode_MouseOver", -1f, -1f, -1f);
	}

	// Token: 0x06000371 RID: 881 RVA: 0x0000E16B File Offset: 0x0000C36B
	public void OnPointerExit(PointerEventData eventData)
	{
		this.animator.SetBool("isMouseOver", false);
	}

	// Token: 0x04000390 RID: 912
	public MapNodeData mapNodeData;

	// Token: 0x04000392 RID: 914
	[SerializeField]
	private Animator animator;

	// Token: 0x04000393 RID: 915
	[SerializeField]
	private List<MapNodePathLine> list_PathLines;

	// Token: 0x04000394 RID: 916
	[SerializeField]
	private ParticleSystem particle_FOWMask;

	// Token: 0x04000395 RID: 917
	[SerializeField]
	private Button button;

	// Token: 0x04000396 RID: 918
	[SerializeField]
	private Gradient gradient_NormalLine;

	// Token: 0x04000397 RID: 919
	[SerializeField]
	private Gradient gradient_AvaliableLine;

	// Token: 0x04000398 RID: 920
	[SerializeField]
	private Color color_PathLineUnlit;

	// Token: 0x04000399 RID: 921
	[SerializeField]
	private Color color_PathLineLit;

	// Token: 0x0400039A RID: 922
	[SerializeField]
	private Material material_NormalLine;

	// Token: 0x0400039B RID: 923
	[SerializeField]
	private Material material_AvaliableLine;

	// Token: 0x0400039C RID: 924
	[SerializeField]
	private float pathToIconOffset = 50f;

	// Token: 0x0400039D RID: 925
	[SerializeField]
	private TMP_Text text_NodeName;

	// Token: 0x0400039E RID: 926
	[SerializeField]
	private GameObject node_NextStageArrow;

	// Token: 0x0400039F RID: 927
	[SerializeField]
	private GameObject node_CompleteReward;

	// Token: 0x040003A0 RID: 928
	[SerializeField]
	[FormerlySerializedAs("image_CompleteRewardIcon")]
	private Image image_CompleteRewardIcon_Other;

	// Token: 0x040003A1 RID: 929
	[SerializeField]
	private Image image_CompleteRewardIcon_Gem;

	// Token: 0x040003A2 RID: 930
	[SerializeField]
	private Image image_CompleteRewardIcon_Tower;

	// Token: 0x040003A3 RID: 931
	[SerializeField]
	private ParticleSystem particle_StageComplete;

	// Token: 0x040003A4 RID: 932
	[SerializeField]
	[Header("Debug文字")]
	private TMP_Text text_DebugState;

	// Token: 0x040003A5 RID: 933
	private MapData mapdata;

	// Token: 0x040003A6 RID: 934
	private Map mapManager;
}
