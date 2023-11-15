using System;
using System.Runtime.CompilerServices;
using FMODUnity;
using Klei.AI;
using ProcGenGame;
using UnityEngine;

// Token: 0x02000A72 RID: 2674
public class NewBaseScreen : KScreen
{
	// Token: 0x0600512E RID: 20782 RVA: 0x001CDC82 File Offset: 0x001CBE82
	public override float GetSortKey()
	{
		return 1f;
	}

	// Token: 0x0600512F RID: 20783 RVA: 0x001CDC89 File Offset: 0x001CBE89
	protected override void OnPrefabInit()
	{
		NewBaseScreen.Instance = this;
		base.OnPrefabInit();
		TimeOfDay.Instance.SetScale(0f);
	}

	// Token: 0x06005130 RID: 20784 RVA: 0x001CDCA6 File Offset: 0x001CBEA6
	protected override void OnForcedCleanUp()
	{
		NewBaseScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06005131 RID: 20785 RVA: 0x001CDCB4 File Offset: 0x001CBEB4
	public static Vector2I SetInitialCamera()
	{
		Vector2I vector2I = SaveLoader.Instance.cachedGSD.baseStartPos;
		vector2I += ClusterManager.Instance.GetStartWorld().WorldOffset;
		Vector3 pos = Grid.CellToPosCCC(Grid.OffsetCell(Grid.OffsetCell(0, vector2I.x, vector2I.y), 0, -2), Grid.SceneLayer.Background);
		CameraController.Instance.SetMaxOrthographicSize(40f);
		CameraController.Instance.SnapTo(pos);
		CameraController.Instance.SetTargetPos(pos, 20f, false);
		CameraController.Instance.OrthographicSize = 40f;
		CameraSaveData.valid = false;
		return vector2I;
	}

	// Token: 0x06005132 RID: 20786 RVA: 0x001CDD4C File Offset: 0x001CBF4C
	protected override void OnActivate()
	{
		if (this.disabledUIElements != null)
		{
			foreach (CanvasGroup canvasGroup in this.disabledUIElements)
			{
				if (canvasGroup != null)
				{
					canvasGroup.interactable = false;
				}
			}
		}
		NewBaseScreen.SetInitialCamera();
		if (SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Unpause(false);
		}
		this.Final();
	}

	// Token: 0x06005133 RID: 20787 RVA: 0x001CDDAD File Offset: 0x001CBFAD
	public void Init(Cluster clusterLayout, ITelepadDeliverable[] startingMinionStats)
	{
		this.m_clusterLayout = clusterLayout;
		this.m_minionStartingStats = startingMinionStats;
	}

	// Token: 0x06005134 RID: 20788 RVA: 0x001CDDC0 File Offset: 0x001CBFC0
	protected override void OnDeactivate()
	{
		Game.Instance.Trigger(-122303817, null);
		if (this.disabledUIElements != null)
		{
			foreach (CanvasGroup canvasGroup in this.disabledUIElements)
			{
				if (canvasGroup != null)
				{
					canvasGroup.interactable = true;
				}
			}
		}
	}

	// Token: 0x06005135 RID: 20789 RVA: 0x001CDE10 File Offset: 0x001CC010
	public override void OnKeyDown(KButtonEvent e)
	{
		global::Action[] array = new global::Action[4];
		RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.46E7A7E6CE942EAE1E13925BEDED6E6321F99918099A108FDB32BB9510B8E88D).FieldHandle);
		global::Action[] array2 = array;
		if (!e.Consumed)
		{
			int num = 0;
			while (num < array2.Length && !e.TryConsume(array2[num]))
			{
				num++;
			}
		}
	}

	// Token: 0x06005136 RID: 20790 RVA: 0x001CDE50 File Offset: 0x001CC050
	private void Final()
	{
		SpeedControlScreen.Instance.Unpause(false);
		GameObject telepad = GameUtil.GetTelepad(ClusterManager.Instance.GetStartWorld().id);
		if (telepad)
		{
			this.SpawnMinions(Grid.PosToCell(telepad));
		}
		Game.Instance.baseAlreadyCreated = true;
		this.Deactivate();
	}

	// Token: 0x06005137 RID: 20791 RVA: 0x001CDEA4 File Offset: 0x001CC0A4
	private void SpawnMinions(int headquartersCell)
	{
		if (headquartersCell == -1)
		{
			global::Debug.LogWarning("No headquarters in saved base template. Cannot place minions. Confirm there is a headquarters saved to the base template, or consider creating a new one.");
			return;
		}
		int num;
		int num2;
		Grid.CellToXY(headquartersCell, out num, out num2);
		if (Grid.WidthInCells < 64)
		{
			return;
		}
		int baseLeft = this.m_clusterLayout.currentWorld.BaseLeft;
		int baseRight = this.m_clusterLayout.currentWorld.BaseRight;
		Effect a_new_hope = Db.Get().effects.Get("AnewHope");
		Action<object> <>9__0;
		for (int i = 0; i < this.m_minionStartingStats.Length; i++)
		{
			int x = num + i % (baseRight - baseLeft) + 1;
			int y = num2;
			int cell = Grid.XYToCell(x, y);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(MinionConfig.ID), null, null);
			Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
			gameObject.transform.SetLocalPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
			gameObject.SetActive(true);
			((MinionStartingStats)this.m_minionStartingStats[i]).Apply(gameObject);
			GameScheduler instance = GameScheduler.Instance;
			string name = "ANewHope";
			float time = 3f + 0.5f * (float)i;
			Action<object> callback;
			if ((callback = <>9__0) == null)
			{
				callback = (<>9__0 = delegate(object m)
				{
					GameObject gameObject2 = m as GameObject;
					if (gameObject2 == null)
					{
						return;
					}
					gameObject2.GetComponent<Effects>().Add(a_new_hope, true);
				});
			}
			instance.Schedule(name, time, callback, gameObject, null);
		}
		ClusterManager.Instance.activeWorld.SetDupeVisited();
	}

	// Token: 0x0400352A RID: 13610
	public static NewBaseScreen Instance;

	// Token: 0x0400352B RID: 13611
	[SerializeField]
	private CanvasGroup[] disabledUIElements;

	// Token: 0x0400352C RID: 13612
	public EventReference ScanSoundMigrated;

	// Token: 0x0400352D RID: 13613
	public EventReference BuildBaseSoundMigrated;

	// Token: 0x0400352E RID: 13614
	private ITelepadDeliverable[] m_minionStartingStats;

	// Token: 0x0400352F RID: 13615
	private Cluster m_clusterLayout;
}
