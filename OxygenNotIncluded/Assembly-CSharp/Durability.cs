using System;
using Klei.CustomSettings;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000778 RID: 1912
[AddComponentMenu("KMonoBehaviour/scripts/Durability")]
public class Durability : KMonoBehaviour
{
	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x060034E5 RID: 13541 RVA: 0x0011C1F2 File Offset: 0x0011A3F2
	// (set) Token: 0x060034E6 RID: 13542 RVA: 0x0011C1FA File Offset: 0x0011A3FA
	public float TimeEquipped
	{
		get
		{
			return this.timeEquipped;
		}
		set
		{
			this.timeEquipped = value;
		}
	}

	// Token: 0x060034E7 RID: 13543 RVA: 0x0011C203 File Offset: 0x0011A403
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Durability>(-1617557748, Durability.OnEquippedDelegate);
		base.Subscribe<Durability>(-170173755, Durability.OnUnequippedDelegate);
	}

	// Token: 0x060034E8 RID: 13544 RVA: 0x0011C230 File Offset: 0x0011A430
	protected override void OnSpawn()
	{
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.Durability, base.gameObject);
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.Durability);
		if (currentQualitySetting != null)
		{
			string id = currentQualitySetting.id;
			if (id != null)
			{
				if (id == "Indestructible")
				{
					this.difficultySettingMod = EQUIPMENT.SUITS.INDESTRUCTIBLE_DURABILITY_MOD;
					return;
				}
				if (id == "Reinforced")
				{
					this.difficultySettingMod = EQUIPMENT.SUITS.REINFORCED_DURABILITY_MOD;
					return;
				}
				if (id == "Flimsy")
				{
					this.difficultySettingMod = EQUIPMENT.SUITS.FLIMSY_DURABILITY_MOD;
					return;
				}
				if (!(id == "Threadbare"))
				{
					return;
				}
				this.difficultySettingMod = EQUIPMENT.SUITS.THREADBARE_DURABILITY_MOD;
			}
		}
	}

	// Token: 0x060034E9 RID: 13545 RVA: 0x0011C2DF File Offset: 0x0011A4DF
	private void OnEquipped()
	{
		if (!this.isEquipped)
		{
			this.isEquipped = true;
			this.timeEquipped = GameClock.Instance.GetTimeInCycles();
		}
	}

	// Token: 0x060034EA RID: 13546 RVA: 0x0011C300 File Offset: 0x0011A500
	private void OnUnequipped()
	{
		if (this.isEquipped)
		{
			this.isEquipped = false;
			float num = GameClock.Instance.GetTimeInCycles() - this.timeEquipped;
			this.DeltaDurability(num * this.durabilityLossPerCycle);
		}
	}

	// Token: 0x060034EB RID: 13547 RVA: 0x0011C33C File Offset: 0x0011A53C
	private void DeltaDurability(float delta)
	{
		delta *= this.difficultySettingMod;
		this.durability = Mathf.Clamp01(this.durability + delta);
	}

	// Token: 0x060034EC RID: 13548 RVA: 0x0011C35C File Offset: 0x0011A55C
	public void ConvertToWornObject()
	{
		GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.wornEquipmentPrefabID), Grid.SceneLayer.Ore, null, 0);
		gameObject.transform.SetPosition(base.transform.GetPosition());
		gameObject.GetComponent<PrimaryElement>().SetElement(base.GetComponent<PrimaryElement>().ElementID, false);
		gameObject.SetActive(true);
		EquippableFacade component = base.GetComponent<EquippableFacade>();
		if (component != null)
		{
			gameObject.GetComponent<RepairableEquipment>().facadeID = component.FacadeID;
		}
		Storage component2 = base.gameObject.GetComponent<Storage>();
		if (component2)
		{
			JetSuitTank component3 = base.gameObject.GetComponent<JetSuitTank>();
			if (component3)
			{
				component2.AddLiquid(SimHashes.Petroleum, component3.amount, base.GetComponent<PrimaryElement>().Temperature, byte.MaxValue, 0, false, true);
			}
			component2.DropAll(false, false, default(Vector3), true, null);
		}
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x060034ED RID: 13549 RVA: 0x0011C448 File Offset: 0x0011A648
	public float GetDurability()
	{
		if (this.isEquipped)
		{
			float num = GameClock.Instance.GetTimeInCycles() - this.timeEquipped;
			return this.durability - num * this.durabilityLossPerCycle;
		}
		return this.durability;
	}

	// Token: 0x060034EE RID: 13550 RVA: 0x0011C485 File Offset: 0x0011A685
	public bool IsWornOut()
	{
		return this.GetDurability() <= 0f;
	}

	// Token: 0x04001FF6 RID: 8182
	private static readonly EventSystem.IntraObjectHandler<Durability> OnEquippedDelegate = new EventSystem.IntraObjectHandler<Durability>(delegate(Durability component, object data)
	{
		component.OnEquipped();
	});

	// Token: 0x04001FF7 RID: 8183
	private static readonly EventSystem.IntraObjectHandler<Durability> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<Durability>(delegate(Durability component, object data)
	{
		component.OnUnequipped();
	});

	// Token: 0x04001FF8 RID: 8184
	[Serialize]
	private bool isEquipped;

	// Token: 0x04001FF9 RID: 8185
	[Serialize]
	private float timeEquipped;

	// Token: 0x04001FFA RID: 8186
	[Serialize]
	private float durability = 1f;

	// Token: 0x04001FFB RID: 8187
	public float durabilityLossPerCycle = -0.1f;

	// Token: 0x04001FFC RID: 8188
	public string wornEquipmentPrefabID;

	// Token: 0x04001FFD RID: 8189
	private float difficultySettingMod = 1f;
}
