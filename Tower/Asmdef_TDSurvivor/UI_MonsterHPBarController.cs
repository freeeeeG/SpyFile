using System;
using Lean.Pool;
using UnityEngine;

// Token: 0x02000172 RID: 370
public class UI_MonsterHPBarController : MonoBehaviour
{
	// Token: 0x060009CB RID: 2507 RVA: 0x00024FAA File Offset: 0x000231AA
	private void OnEnable()
	{
		EventMgr.Register<AMonsterBase>(eGameEvents.MonsterSpawn, new Action<AMonsterBase>(this.OnMonsterSpawn));
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x00024FC4 File Offset: 0x000231C4
	private void OnDisable()
	{
		EventMgr.Remove<AMonsterBase>(eGameEvents.MonsterSpawn, new Action<AMonsterBase>(this.OnMonsterSpawn));
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x00024FE0 File Offset: 0x000231E0
	private void OnMonsterSpawn(AMonsterBase monster)
	{
		Vector3 position = Singleton<CameraManager>.Instance.Calculate2DPosFrom3DPos(monster.HeadWorldPosition);
		GameObject gameObject = LeanPool.Spawn(this.prefab_HPBarUI.gameObject, position, Quaternion.identity, base.transform);
		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}
		gameObject.GetComponent<UI_MonsterHPBar>().AttachUI(monster);
	}

	// Token: 0x040007A5 RID: 1957
	[SerializeField]
	private UI_MonsterHPBar prefab_HPBarUI;
}
