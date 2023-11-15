using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000042 RID: 66
public class TowerBuffModule : MonoBehaviour
{
	// Token: 0x06000142 RID: 322 RVA: 0x00005BE0 File Offset: 0x00003DE0
	private void Awake()
	{
		this.tower = base.GetComponent<ABaseTower>();
		ABaseTower abaseTower = this.tower;
		abaseTower.OnTowerShoot = (Action<ABaseTower, AMonsterBase>)Delegate.Combine(abaseTower.OnTowerShoot, new Action<ABaseTower, AMonsterBase>(this.OnTowerShoot));
		ABaseTower abaseTower2 = this.tower;
		abaseTower2.OnTowerDespawn = (Action<ABaseTower>)Delegate.Combine(abaseTower2.OnTowerDespawn, new Action<ABaseTower>(this.OnTowerDespawn));
		ABaseTower abaseTower3 = this.tower;
		abaseTower3.OnTowerHit = (Action<ABaseTower, AMonsterBase, int, int>)Delegate.Combine(abaseTower3.OnTowerHit, new Action<ABaseTower, AMonsterBase, int, int>(this.OnTowerHit));
	}

	// Token: 0x06000143 RID: 323 RVA: 0x00005C70 File Offset: 0x00003E70
	private void OnDestroy()
	{
		ABaseTower abaseTower = this.tower;
		abaseTower.OnTowerShoot = (Action<ABaseTower, AMonsterBase>)Delegate.Remove(abaseTower.OnTowerShoot, new Action<ABaseTower, AMonsterBase>(this.OnTowerShoot));
		ABaseTower abaseTower2 = this.tower;
		abaseTower2.OnTowerDespawn = (Action<ABaseTower>)Delegate.Remove(abaseTower2.OnTowerDespawn, new Action<ABaseTower>(this.OnTowerDespawn));
		ABaseTower abaseTower3 = this.tower;
		abaseTower3.OnTowerHit = (Action<ABaseTower, AMonsterBase, int, int>)Delegate.Remove(abaseTower3.OnTowerHit, new Action<ABaseTower, AMonsterBase, int, int>(this.OnTowerHit));
	}

	// Token: 0x06000144 RID: 324 RVA: 0x00005CF4 File Offset: 0x00003EF4
	private void Update()
	{
		if (Singleton<GameStateController>.Instance.IsCurrentState(eGameState.PAUSE_GAME))
		{
			return;
		}
		foreach (ABaseBuffSettingData abaseBuffSettingData in this.dic_Buffs.Values.ToList<ABaseBuffSettingData>())
		{
			abaseBuffSettingData.Tick(Time.deltaTime);
			if (abaseBuffSettingData.IsFinished)
			{
				this.dic_Buffs.Remove(abaseBuffSettingData.GetItemType());
			}
		}
	}

	// Token: 0x06000145 RID: 325 RVA: 0x00005D80 File Offset: 0x00003F80
	public void ApplyBuff(ABaseBuffSettingData buff)
	{
		if (this.HasAnyBuff())
		{
			this.RemoveAllBuffs();
		}
		buff.Initialize(this.tower);
		if (this.dic_Buffs.ContainsKey(buff.GetItemType()))
		{
			this.dic_Buffs[buff.GetItemType()].Activate();
		}
		else
		{
			this.dic_Buffs.Add(buff.GetItemType(), buff);
			buff.Activate();
		}
		base.StartCoroutine(this.CR_BuffAnim(this.tower, buff));
		DebugManager.Log(eDebugKey.BUFF_SYSTEM, string.Format("{0}套用Buff: {1}", base.gameObject.name, buff.GetItemType()), base.gameObject);
	}

	// Token: 0x06000146 RID: 326 RVA: 0x00005E2B File Offset: 0x0000402B
	private IEnumerator CR_BuffAnim(ABaseTower tower, ABaseBuffSettingData buff)
	{
		tower.PlayAnim_ApplyBuff();
		yield return new WaitForSeconds(1.33f);
		UI_Obj_TowerBuffIcon component = Singleton<PrefabManager>.Instance.InstantiatePrefab("Obj_UI_TowerBuffIcon", Vector3.zero, Quaternion.identity, Singleton<CameraManager>.Instance.Canvas.transform).GetComponent<UI_Obj_TowerBuffIcon>();
		component.transform.SetParent(Singleton<UIManager>.Instance.GetDynamicUIAnchor());
		component.Setup(tower, buff);
		yield break;
	}

	// Token: 0x06000147 RID: 327 RVA: 0x00005E44 File Offset: 0x00004044
	private void OnTowerShoot(ABaseTower tower, AMonsterBase target)
	{
		foreach (ABaseBuffSettingData abaseBuffSettingData in this.dic_Buffs.Values.ToList<ABaseBuffSettingData>())
		{
			if (abaseBuffSettingData.IsShootingEffect)
			{
				abaseBuffSettingData.OnTowerShoot(tower, target);
			}
		}
	}

	// Token: 0x06000148 RID: 328 RVA: 0x00005EAC File Offset: 0x000040AC
	private void OnTowerHit(ABaseTower tower, AMonsterBase target, int shootIndex, int bulletIndex)
	{
		foreach (ABaseBuffSettingData abaseBuffSettingData in this.dic_Buffs.Values.ToList<ABaseBuffSettingData>())
		{
			if (abaseBuffSettingData.IsHitTargetEffect)
			{
				abaseBuffSettingData.OnTowerBulletHit(tower, target, shootIndex, bulletIndex);
			}
		}
	}

	// Token: 0x06000149 RID: 329 RVA: 0x00005F18 File Offset: 0x00004118
	private void OnTowerDespawn(ABaseTower tower)
	{
		this.RemoveAllBuffs();
	}

	// Token: 0x0600014A RID: 330 RVA: 0x00005F20 File Offset: 0x00004120
	private void RemoveAllBuffs()
	{
		foreach (ABaseBuffSettingData abaseBuffSettingData in this.dic_Buffs.Values.ToList<ABaseBuffSettingData>())
		{
			abaseBuffSettingData.ForceRemove();
		}
		this.dic_Buffs.Clear();
	}

	// Token: 0x0600014B RID: 331 RVA: 0x00005F88 File Offset: 0x00004188
	public bool HasAnyBuff()
	{
		return this.dic_Buffs.Count > 0;
	}

	// Token: 0x040000DD RID: 221
	[SerializeField]
	private ABaseTower tower;

	// Token: 0x040000DE RID: 222
	private Dictionary<eItemType, ABaseBuffSettingData> dic_Buffs = new Dictionary<eItemType, ABaseBuffSettingData>();

	// Token: 0x040000DF RID: 223
	private bool isOutlineOn;

	// Token: 0x040000E0 RID: 224
	private List<Renderer> buffOutlineRenderers;
}
