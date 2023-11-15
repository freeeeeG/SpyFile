using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020008BA RID: 2234
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/NoisePolluter")]
public class NoisePolluter : KMonoBehaviour, IPolluter
{
	// Token: 0x060040A0 RID: 16544 RVA: 0x001695D7 File Offset: 0x001677D7
	public static bool IsNoiseableCell(int cell)
	{
		return Grid.IsValidCell(cell) && (Grid.IsGas(cell) || !Grid.IsSubstantialLiquid(cell, 0.35f));
	}

	// Token: 0x060040A1 RID: 16545 RVA: 0x001695FB File Offset: 0x001677FB
	public void ResetCells()
	{
		if (this.radius == 0)
		{
			global::Debug.LogFormat("[{0}] has a 0 radius noise, this will disable it", new object[]
			{
				this.GetName()
			});
			return;
		}
	}

	// Token: 0x060040A2 RID: 16546 RVA: 0x0016961F File Offset: 0x0016781F
	public void SetAttributes(Vector2 pos, int dB, GameObject go, string name)
	{
		this.sourceName = name;
		this.noise = dB;
	}

	// Token: 0x060040A3 RID: 16547 RVA: 0x00169630 File Offset: 0x00167830
	public int GetRadius()
	{
		return this.radius;
	}

	// Token: 0x060040A4 RID: 16548 RVA: 0x00169638 File Offset: 0x00167838
	public int GetNoise()
	{
		return this.noise;
	}

	// Token: 0x060040A5 RID: 16549 RVA: 0x00169640 File Offset: 0x00167840
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060040A6 RID: 16550 RVA: 0x00169648 File Offset: 0x00167848
	public void SetSplat(NoiseSplat new_splat)
	{
		this.splat = new_splat;
	}

	// Token: 0x060040A7 RID: 16551 RVA: 0x00169651 File Offset: 0x00167851
	public void Clear()
	{
		if (this.splat != null)
		{
			this.splat.Clear();
			this.splat = null;
		}
	}

	// Token: 0x060040A8 RID: 16552 RVA: 0x0016966D File Offset: 0x0016786D
	public Vector2 GetPosition()
	{
		return base.transform.GetPosition();
	}

	// Token: 0x1700047A RID: 1146
	// (get) Token: 0x060040A9 RID: 16553 RVA: 0x0016967F File Offset: 0x0016787F
	// (set) Token: 0x060040AA RID: 16554 RVA: 0x00169687 File Offset: 0x00167887
	public string sourceName { get; private set; }

	// Token: 0x1700047B RID: 1147
	// (get) Token: 0x060040AB RID: 16555 RVA: 0x00169690 File Offset: 0x00167890
	// (set) Token: 0x060040AC RID: 16556 RVA: 0x00169698 File Offset: 0x00167898
	public bool active { get; private set; }

	// Token: 0x060040AD RID: 16557 RVA: 0x001696A1 File Offset: 0x001678A1
	public void SetActive(bool active = true)
	{
		if (!active && this.splat != null)
		{
			AudioEventManager.Get().ClearNoiseSplat(this.splat);
			this.splat.Clear();
		}
		this.active = active;
	}

	// Token: 0x060040AE RID: 16558 RVA: 0x001696D0 File Offset: 0x001678D0
	public void Refresh()
	{
		if (this.active)
		{
			if (this.splat != null)
			{
				AudioEventManager.Get().ClearNoiseSplat(this.splat);
				this.splat.Clear();
			}
			KSelectable component = base.GetComponent<KSelectable>();
			string name = (component != null) ? component.GetName() : base.name;
			GameObject gameObject = base.GetComponent<KMonoBehaviour>().gameObject;
			this.splat = AudioEventManager.Get().CreateNoiseSplat(this.GetPosition(), this.noise, this.radius, name, gameObject);
		}
	}

	// Token: 0x060040AF RID: 16559 RVA: 0x00169758 File Offset: 0x00167958
	private void OnActiveChanged(object data)
	{
		bool isActive = ((Operational)data).IsActive;
		this.SetActive(isActive);
		this.Refresh();
	}

	// Token: 0x060040B0 RID: 16560 RVA: 0x0016977E File Offset: 0x0016797E
	public void SetValues(EffectorValues values)
	{
		this.noise = values.amount;
		this.radius = values.radius;
	}

	// Token: 0x060040B1 RID: 16561 RVA: 0x00169798 File Offset: 0x00167998
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.radius == 0 || this.noise == 0)
		{
			global::Debug.LogWarning(string.Concat(new string[]
			{
				"Noisepollutor::OnSpawn [",
				this.GetName(),
				"] noise: [",
				this.noise.ToString(),
				"] radius: [",
				this.radius.ToString(),
				"]"
			}));
			UnityEngine.Object.Destroy(this);
			return;
		}
		this.ResetCells();
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			base.Subscribe<NoisePolluter>(824508782, NoisePolluter.OnActiveChangedDelegate);
		}
		this.refreshCallback = new System.Action(this.Refresh);
		this.refreshPartionerCallback = delegate(object data)
		{
			this.Refresh();
		};
		this.onCollectNoisePollutersCallback = new Action<object>(this.OnCollectNoisePolluters);
		Attributes attributes = this.GetAttributes();
		Db db = Db.Get();
		this.dB = attributes.Add(db.BuildingAttributes.NoisePollution);
		this.dBRadius = attributes.Add(db.BuildingAttributes.NoisePollutionRadius);
		if (this.noise != 0 && this.radius != 0)
		{
			AttributeModifier modifier = new AttributeModifier(db.BuildingAttributes.NoisePollution.Id, (float)this.noise, UI.TOOLTIPS.BASE_VALUE, false, false, true);
			AttributeModifier modifier2 = new AttributeModifier(db.BuildingAttributes.NoisePollutionRadius.Id, (float)this.radius, UI.TOOLTIPS.BASE_VALUE, false, false, true);
			attributes.Add(modifier);
			attributes.Add(modifier2);
		}
		else
		{
			global::Debug.LogWarning(string.Concat(new string[]
			{
				"Noisepollutor::OnSpawn [",
				this.GetName(),
				"] radius: [",
				this.radius.ToString(),
				"] noise: [",
				this.noise.ToString(),
				"]"
			}));
		}
		KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
		this.isMovable = (component2 != null && component2.isMovable);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "NoisePolluter.OnSpawn");
		AttributeInstance attributeInstance = this.dB;
		attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, this.refreshCallback);
		AttributeInstance attributeInstance2 = this.dBRadius;
		attributeInstance2.OnDirty = (System.Action)Delegate.Combine(attributeInstance2.OnDirty, this.refreshCallback);
		if (component != null)
		{
			this.OnActiveChanged(component.IsActive);
		}
	}

	// Token: 0x060040B2 RID: 16562 RVA: 0x00169A19 File Offset: 0x00167C19
	private void OnCellChange()
	{
		this.Refresh();
	}

	// Token: 0x060040B3 RID: 16563 RVA: 0x00169A21 File Offset: 0x00167C21
	private void OnCollectNoisePolluters(object data)
	{
		((List<NoisePolluter>)data).Add(this);
	}

	// Token: 0x060040B4 RID: 16564 RVA: 0x00169A2F File Offset: 0x00167C2F
	public string GetName()
	{
		if (string.IsNullOrEmpty(this.sourceName))
		{
			this.sourceName = base.GetComponent<KSelectable>().GetName();
		}
		return this.sourceName;
	}

	// Token: 0x060040B5 RID: 16565 RVA: 0x00169A58 File Offset: 0x00167C58
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (base.isSpawned)
		{
			if (this.dB != null)
			{
				AttributeInstance attributeInstance = this.dB;
				attributeInstance.OnDirty = (System.Action)Delegate.Remove(attributeInstance.OnDirty, this.refreshCallback);
				AttributeInstance attributeInstance2 = this.dBRadius;
				attributeInstance2.OnDirty = (System.Action)Delegate.Remove(attributeInstance2.OnDirty, this.refreshCallback);
			}
			if (this.isMovable)
			{
				Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
			}
		}
		if (this.splat != null)
		{
			AudioEventManager.Get().ClearNoiseSplat(this.splat);
			this.splat.Clear();
		}
	}

	// Token: 0x060040B6 RID: 16566 RVA: 0x00169B04 File Offset: 0x00167D04
	public float GetNoiseForCell(int cell)
	{
		return this.splat.GetDBForCell(cell);
	}

	// Token: 0x060040B7 RID: 16567 RVA: 0x00169B14 File Offset: 0x00167D14
	public List<Descriptor> GetEffectDescriptions()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.dB != null && this.dBRadius != null)
		{
			float totalValue = this.dB.GetTotalValue();
			float totalValue2 = this.dBRadius.GetTotalValue();
			string text = (this.noise > 0) ? UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_INCREASE : UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_DECREASE;
			text = text + "\n\n" + this.dB.GetAttributeValueTooltip();
			string arg = GameUtil.AddPositiveSign(totalValue.ToString(), totalValue > 0f);
			Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.NOISE_CREATED, arg, totalValue2), string.Format(text, arg, totalValue2), Descriptor.DescriptorType.Effect, false);
			list.Add(item);
		}
		else if (this.noise != 0)
		{
			string format = (this.noise >= 0) ? UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_INCREASE : UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_DECREASE;
			string arg2 = GameUtil.AddPositiveSign(this.noise.ToString(), this.noise > 0);
			Descriptor item2 = new Descriptor(string.Format(UI.BUILDINGEFFECTS.NOISE_CREATED, arg2, this.radius), string.Format(format, arg2, this.radius), Descriptor.DescriptorType.Effect, false);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x060040B8 RID: 16568 RVA: 0x00169C59 File Offset: 0x00167E59
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return this.GetEffectDescriptions();
	}

	// Token: 0x04002A18 RID: 10776
	public const string ID = "NoisePolluter";

	// Token: 0x04002A19 RID: 10777
	public int radius;

	// Token: 0x04002A1A RID: 10778
	public int noise;

	// Token: 0x04002A1B RID: 10779
	public AttributeInstance dB;

	// Token: 0x04002A1C RID: 10780
	public AttributeInstance dBRadius;

	// Token: 0x04002A1D RID: 10781
	private NoiseSplat splat;

	// Token: 0x04002A1F RID: 10783
	public System.Action refreshCallback;

	// Token: 0x04002A20 RID: 10784
	public Action<object> refreshPartionerCallback;

	// Token: 0x04002A21 RID: 10785
	public Action<object> onCollectNoisePollutersCallback;

	// Token: 0x04002A22 RID: 10786
	public bool isMovable;

	// Token: 0x04002A23 RID: 10787
	[MyCmpReq]
	public OccupyArea occupyArea;

	// Token: 0x04002A25 RID: 10789
	private static readonly EventSystem.IntraObjectHandler<NoisePolluter> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<NoisePolluter>(delegate(NoisePolluter component, object data)
	{
		component.OnActiveChanged(data);
	});
}
