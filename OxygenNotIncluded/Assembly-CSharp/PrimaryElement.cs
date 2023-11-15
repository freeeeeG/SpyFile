using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Klei;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020008F1 RID: 2289
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/PrimaryElement")]
public class PrimaryElement : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06004236 RID: 16950 RVA: 0x001722D8 File Offset: 0x001704D8
	public void SetUseSimDiseaseInfo(bool use)
	{
		this.useSimDiseaseInfo = use;
	}

	// Token: 0x17000492 RID: 1170
	// (get) Token: 0x06004237 RID: 16951 RVA: 0x001722E1 File Offset: 0x001704E1
	// (set) Token: 0x06004238 RID: 16952 RVA: 0x001722EC File Offset: 0x001704EC
	[Serialize]
	public float Units
	{
		get
		{
			return this._units;
		}
		set
		{
			if (float.IsInfinity(value) || float.IsNaN(value))
			{
				DebugUtil.DevLogError("Invalid units value for element, setting Units to 0");
				this._units = 0f;
			}
			else
			{
				this._units = value;
			}
			if (this.onDataChanged != null)
			{
				this.onDataChanged(this);
			}
		}
	}

	// Token: 0x17000493 RID: 1171
	// (get) Token: 0x06004239 RID: 16953 RVA: 0x0017233B File Offset: 0x0017053B
	// (set) Token: 0x0600423A RID: 16954 RVA: 0x00172349 File Offset: 0x00170549
	public float Temperature
	{
		get
		{
			return this.getTemperatureCallback(this);
		}
		set
		{
			this.SetTemperature(value);
		}
	}

	// Token: 0x17000494 RID: 1172
	// (get) Token: 0x0600423B RID: 16955 RVA: 0x00172352 File Offset: 0x00170552
	// (set) Token: 0x0600423C RID: 16956 RVA: 0x0017235A File Offset: 0x0017055A
	public float InternalTemperature
	{
		get
		{
			return this._Temperature;
		}
		set
		{
			this._Temperature = value;
		}
	}

	// Token: 0x0600423D RID: 16957 RVA: 0x00172364 File Offset: 0x00170564
	[OnSerializing]
	private void OnSerializing()
	{
		this._Temperature = this.Temperature;
		this.SanitizeMassAndTemperature();
		this.diseaseID.HashValue = 0;
		this.diseaseCount = 0;
		if (this.useSimDiseaseInfo)
		{
			int i = Grid.PosToCell(base.transform.GetPosition());
			if (Grid.DiseaseIdx[i] != 255)
			{
				this.diseaseID = Db.Get().Diseases[(int)Grid.DiseaseIdx[i]].id;
				this.diseaseCount = Grid.DiseaseCount[i];
				return;
			}
		}
		else if (this.diseaseHandle.IsValid())
		{
			DiseaseHeader header = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle);
			if (header.diseaseIdx != 255)
			{
				this.diseaseID = Db.Get().Diseases[(int)header.diseaseIdx].id;
				this.diseaseCount = header.diseaseCount;
			}
		}
	}

	// Token: 0x0600423E RID: 16958 RVA: 0x00172454 File Offset: 0x00170654
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.ElementID == (SimHashes)351109216)
		{
			this.ElementID = SimHashes.Creature;
		}
		this.SanitizeMassAndTemperature();
		float temperature = this._Temperature;
		if (float.IsNaN(temperature) || float.IsInfinity(temperature) || temperature < 0f || 10000f < temperature)
		{
			DeserializeWarnings.Instance.PrimaryElementTemperatureIsNan.Warn(string.Format("{0} has invalid temperature of {1}. Resetting temperature.", base.name, this.Temperature), null);
			temperature = this.Element.defaultValues.temperature;
		}
		this._Temperature = temperature;
		this.Temperature = temperature;
		if (this.Element == null)
		{
			DeserializeWarnings.Instance.PrimaryElementHasNoElement.Warn(base.name + "Primary element has no element.", null);
		}
		if (this.Mass < 0f)
		{
			DebugUtil.DevLogError(base.gameObject, "deserialized ore with less than 0 mass. Error! Destroying");
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		if (this.Mass == 0f && !this.KeepZeroMassObject)
		{
			DebugUtil.DevLogError(base.gameObject, "deserialized element with 0 mass. Destroying");
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		if (this.onDataChanged != null)
		{
			this.onDataChanged(this);
		}
		byte index = Db.Get().Diseases.GetIndex(this.diseaseID);
		if (index == 255 || this.diseaseCount <= 0)
		{
			if (this.diseaseHandle.IsValid())
			{
				GameComps.DiseaseContainers.Remove(base.gameObject);
				this.diseaseHandle.Clear();
				return;
			}
		}
		else
		{
			if (this.diseaseHandle.IsValid())
			{
				DiseaseHeader header = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle);
				header.diseaseIdx = index;
				header.diseaseCount = this.diseaseCount;
				GameComps.DiseaseContainers.SetHeader(this.diseaseHandle, header);
				return;
			}
			this.diseaseHandle = GameComps.DiseaseContainers.Add(base.gameObject, index, this.diseaseCount);
		}
	}

	// Token: 0x0600423F RID: 16959 RVA: 0x00172638 File Offset: 0x00170838
	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
	}

	// Token: 0x06004240 RID: 16960 RVA: 0x00172640 File Offset: 0x00170840
	private void SanitizeMassAndTemperature()
	{
		if (this._Temperature <= 0f)
		{
			DebugUtil.DevLogError(base.gameObject.name + " is attempting to serialize a temperature of <= 0K. Resetting to default. world=" + base.gameObject.DebugGetMyWorldName());
			this._Temperature = this.Element.defaultValues.temperature;
		}
		if (this.Mass > PrimaryElement.MAX_MASS)
		{
			DebugUtil.DevLogError(string.Format("{0} is attempting to serialize very large mass {1}. Resetting to default. world={2}", base.gameObject.name, this.Mass, base.gameObject.DebugGetMyWorldName()));
			this.Mass = this.Element.defaultValues.mass;
		}
	}

	// Token: 0x17000495 RID: 1173
	// (get) Token: 0x06004241 RID: 16961 RVA: 0x001726E8 File Offset: 0x001708E8
	// (set) Token: 0x06004242 RID: 16962 RVA: 0x001726F7 File Offset: 0x001708F7
	public float Mass
	{
		get
		{
			return this.Units * this.MassPerUnit;
		}
		set
		{
			this.SetMass(value);
			if (this.onDataChanged != null)
			{
				this.onDataChanged(this);
			}
		}
	}

	// Token: 0x06004243 RID: 16963 RVA: 0x00172714 File Offset: 0x00170914
	private void SetMass(float mass)
	{
		if ((mass > PrimaryElement.MAX_MASS || mass < 0f) && this.ElementID != SimHashes.Regolith)
		{
			DebugUtil.DevLogErrorFormat(base.gameObject, "{0} is getting an abnormal mass set {1}.", new object[]
			{
				base.gameObject.name,
				mass
			});
		}
		mass = Mathf.Clamp(mass, 0f, PrimaryElement.MAX_MASS);
		this.Units = mass / this.MassPerUnit;
		if (this.Units <= 0f && !this.KeepZeroMassObject)
		{
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x06004244 RID: 16964 RVA: 0x001727AC File Offset: 0x001709AC
	private void SetTemperature(float temperature)
	{
		if (float.IsNaN(temperature) || float.IsInfinity(temperature))
		{
			DebugUtil.LogErrorArgs(base.gameObject, new object[]
			{
				"Invalid temperature [" + temperature.ToString() + "]"
			});
			return;
		}
		if (temperature <= 0f)
		{
			KCrashReporter.Assert(false, "Tried to set PrimaryElement.Temperature to a value <= 0");
		}
		this.setTemperatureCallback(this, temperature);
	}

	// Token: 0x06004245 RID: 16965 RVA: 0x00172814 File Offset: 0x00170A14
	public void SetMassTemperature(float mass, float temperature)
	{
		this.SetMass(mass);
		this.SetTemperature(temperature);
	}

	// Token: 0x17000496 RID: 1174
	// (get) Token: 0x06004246 RID: 16966 RVA: 0x00172824 File Offset: 0x00170A24
	public Element Element
	{
		get
		{
			if (this._Element == null)
			{
				this._Element = ElementLoader.FindElementByHash(this.ElementID);
			}
			return this._Element;
		}
	}

	// Token: 0x17000497 RID: 1175
	// (get) Token: 0x06004247 RID: 16967 RVA: 0x00172848 File Offset: 0x00170A48
	public byte DiseaseIdx
	{
		get
		{
			if (this.diseaseRedirectTarget)
			{
				return this.diseaseRedirectTarget.DiseaseIdx;
			}
			byte result = byte.MaxValue;
			if (this.useSimDiseaseInfo)
			{
				int i = Grid.PosToCell(base.transform.GetPosition());
				result = Grid.DiseaseIdx[i];
			}
			else if (this.diseaseHandle.IsValid())
			{
				result = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle).diseaseIdx;
			}
			return result;
		}
	}

	// Token: 0x17000498 RID: 1176
	// (get) Token: 0x06004248 RID: 16968 RVA: 0x001728C0 File Offset: 0x00170AC0
	public int DiseaseCount
	{
		get
		{
			if (this.diseaseRedirectTarget)
			{
				return this.diseaseRedirectTarget.DiseaseCount;
			}
			int result = 0;
			if (this.useSimDiseaseInfo)
			{
				int i = Grid.PosToCell(base.transform.GetPosition());
				result = Grid.DiseaseCount[i];
			}
			else if (this.diseaseHandle.IsValid())
			{
				result = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle).diseaseCount;
			}
			return result;
		}
	}

	// Token: 0x06004249 RID: 16969 RVA: 0x00172933 File Offset: 0x00170B33
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GameComps.InfraredVisualizers.Add(base.gameObject);
		base.Subscribe<PrimaryElement>(1335436905, PrimaryElement.OnSplitFromChunkDelegate);
		base.Subscribe<PrimaryElement>(-2064133523, PrimaryElement.OnAbsorbDelegate);
	}

	// Token: 0x0600424A RID: 16970 RVA: 0x00172970 File Offset: 0x00170B70
	protected override void OnSpawn()
	{
		Attributes attributes = this.GetAttributes();
		if (attributes != null)
		{
			foreach (AttributeModifier modifier in this.Element.attributeModifiers)
			{
				attributes.Add(modifier);
			}
		}
	}

	// Token: 0x0600424B RID: 16971 RVA: 0x001729D4 File Offset: 0x00170BD4
	public void ForcePermanentDiseaseContainer(bool force_on)
	{
		if (force_on)
		{
			if (!this.diseaseHandle.IsValid())
			{
				this.diseaseHandle = GameComps.DiseaseContainers.Add(base.gameObject, byte.MaxValue, 0);
			}
		}
		else if (this.diseaseHandle.IsValid() && this.DiseaseIdx == 255)
		{
			GameComps.DiseaseContainers.Remove(base.gameObject);
			this.diseaseHandle.Clear();
		}
		this.forcePermanentDiseaseContainer = force_on;
	}

	// Token: 0x0600424C RID: 16972 RVA: 0x00172A4B File Offset: 0x00170C4B
	protected override void OnCleanUp()
	{
		GameComps.InfraredVisualizers.Remove(base.gameObject);
		if (this.diseaseHandle.IsValid())
		{
			GameComps.DiseaseContainers.Remove(base.gameObject);
			this.diseaseHandle.Clear();
		}
		base.OnCleanUp();
	}

	// Token: 0x0600424D RID: 16973 RVA: 0x00172A8B File Offset: 0x00170C8B
	public void SetElement(SimHashes element_id, bool addTags = true)
	{
		this.ElementID = element_id;
		if (addTags)
		{
			this.UpdateTags();
		}
	}

	// Token: 0x0600424E RID: 16974 RVA: 0x00172AA0 File Offset: 0x00170CA0
	public void UpdateTags()
	{
		if (this.ElementID == (SimHashes)0)
		{
			global::Debug.Log("UpdateTags() Primary element 0", base.gameObject);
			return;
		}
		KPrefabID component = base.GetComponent<KPrefabID>();
		if (component != null)
		{
			List<Tag> list = new List<Tag>();
			foreach (Tag item in this.Element.oreTags)
			{
				list.Add(item);
			}
			if (component.HasAnyTags(PrimaryElement.metalTags))
			{
				list.Add(GameTags.StoredMetal);
			}
			foreach (Tag tag in list)
			{
				component.AddTag(tag, false);
			}
		}
	}

	// Token: 0x0600424F RID: 16975 RVA: 0x00172B64 File Offset: 0x00170D64
	public void ModifyDiseaseCount(int delta, string reason)
	{
		if (this.diseaseRedirectTarget)
		{
			this.diseaseRedirectTarget.ModifyDiseaseCount(delta, reason);
			return;
		}
		if (this.useSimDiseaseInfo)
		{
			SimMessages.ModifyDiseaseOnCell(Grid.PosToCell(this), byte.MaxValue, delta);
			return;
		}
		if (delta != 0 && this.diseaseHandle.IsValid() && GameComps.DiseaseContainers.ModifyDiseaseCount(this.diseaseHandle, delta) <= 0 && !this.forcePermanentDiseaseContainer)
		{
			base.Trigger(-1689370368, false);
			GameComps.DiseaseContainers.Remove(base.gameObject);
			this.diseaseHandle.Clear();
		}
	}

	// Token: 0x06004250 RID: 16976 RVA: 0x00172C00 File Offset: 0x00170E00
	public void AddDisease(byte disease_idx, int delta, string reason)
	{
		if (delta == 0)
		{
			return;
		}
		if (this.diseaseRedirectTarget)
		{
			this.diseaseRedirectTarget.AddDisease(disease_idx, delta, reason);
			return;
		}
		if (this.useSimDiseaseInfo)
		{
			SimMessages.ModifyDiseaseOnCell(Grid.PosToCell(this), disease_idx, delta);
			return;
		}
		if (this.diseaseHandle.IsValid())
		{
			if (GameComps.DiseaseContainers.AddDisease(this.diseaseHandle, disease_idx, delta) <= 0)
			{
				GameComps.DiseaseContainers.Remove(base.gameObject);
				this.diseaseHandle.Clear();
				return;
			}
		}
		else if (delta > 0)
		{
			this.diseaseHandle = GameComps.DiseaseContainers.Add(base.gameObject, disease_idx, delta);
			base.Trigger(-1689370368, true);
			base.Trigger(-283306403, null);
		}
	}

	// Token: 0x06004251 RID: 16977 RVA: 0x00172CBA File Offset: 0x00170EBA
	private static float OnGetTemperature(PrimaryElement primary_element)
	{
		return primary_element._Temperature;
	}

	// Token: 0x06004252 RID: 16978 RVA: 0x00172CC4 File Offset: 0x00170EC4
	private static void OnSetTemperature(PrimaryElement primary_element, float temperature)
	{
		global::Debug.Assert(!float.IsNaN(temperature));
		if (temperature <= 0f)
		{
			DebugUtil.LogErrorArgs(primary_element.gameObject, new object[]
			{
				primary_element.gameObject.name + " has a temperature of zero which has always been an error in my experience."
			});
		}
		primary_element._Temperature = temperature;
	}

	// Token: 0x06004253 RID: 16979 RVA: 0x00172D18 File Offset: 0x00170F18
	private void OnSplitFromChunk(object data)
	{
		Pickupable pickupable = (Pickupable)data;
		if (pickupable == null)
		{
			return;
		}
		float percent = this.Units / (this.Units + pickupable.PrimaryElement.Units);
		SimUtil.DiseaseInfo percentOfDisease = SimUtil.GetPercentOfDisease(pickupable.PrimaryElement, percent);
		this.AddDisease(percentOfDisease.idx, percentOfDisease.count, "PrimaryElement.SplitFromChunk");
		pickupable.PrimaryElement.ModifyDiseaseCount(-percentOfDisease.count, "PrimaryElement.SplitFromChunk");
	}

	// Token: 0x06004254 RID: 16980 RVA: 0x00172D8C File Offset: 0x00170F8C
	private void OnAbsorb(object data)
	{
		Pickupable pickupable = (Pickupable)data;
		if (pickupable == null)
		{
			return;
		}
		this.AddDisease(pickupable.PrimaryElement.DiseaseIdx, pickupable.PrimaryElement.DiseaseCount, "PrimaryElement.OnAbsorb");
	}

	// Token: 0x06004255 RID: 16981 RVA: 0x00172DCC File Offset: 0x00170FCC
	private void SetDiseaseVisualProvider(GameObject visualizer)
	{
		HandleVector<int>.Handle handle = GameComps.DiseaseContainers.GetHandle(base.gameObject);
		if (handle != HandleVector<int>.InvalidHandle)
		{
			DiseaseContainer payload = GameComps.DiseaseContainers.GetPayload(handle);
			payload.visualDiseaseProvider = visualizer;
			GameComps.DiseaseContainers.SetPayload(handle, ref payload);
		}
	}

	// Token: 0x06004256 RID: 16982 RVA: 0x00172E18 File Offset: 0x00171018
	public void RedirectDisease(GameObject target)
	{
		this.SetDiseaseVisualProvider(target);
		this.diseaseRedirectTarget = (target ? target.GetComponent<PrimaryElement>() : null);
		global::Debug.Assert(this.diseaseRedirectTarget != this, "Disease redirect target set to myself");
	}

	// Token: 0x04002B3D RID: 11069
	public static float MAX_MASS = 100000f;

	// Token: 0x04002B3E RID: 11070
	public PrimaryElement.GetTemperatureCallback getTemperatureCallback = new PrimaryElement.GetTemperatureCallback(PrimaryElement.OnGetTemperature);

	// Token: 0x04002B3F RID: 11071
	public PrimaryElement.SetTemperatureCallback setTemperatureCallback = new PrimaryElement.SetTemperatureCallback(PrimaryElement.OnSetTemperature);

	// Token: 0x04002B40 RID: 11072
	private PrimaryElement diseaseRedirectTarget;

	// Token: 0x04002B41 RID: 11073
	private bool useSimDiseaseInfo;

	// Token: 0x04002B42 RID: 11074
	public const float DefaultChunkMass = 400f;

	// Token: 0x04002B43 RID: 11075
	private static readonly Tag[] metalTags = new Tag[]
	{
		GameTags.Metal,
		GameTags.RefinedMetal
	};

	// Token: 0x04002B44 RID: 11076
	[Serialize]
	[HashedEnum]
	public SimHashes ElementID;

	// Token: 0x04002B45 RID: 11077
	private float _units = 1f;

	// Token: 0x04002B46 RID: 11078
	[Serialize]
	[SerializeField]
	private float _Temperature;

	// Token: 0x04002B47 RID: 11079
	[Serialize]
	[NonSerialized]
	public bool KeepZeroMassObject;

	// Token: 0x04002B48 RID: 11080
	[Serialize]
	private HashedString diseaseID;

	// Token: 0x04002B49 RID: 11081
	[Serialize]
	private int diseaseCount;

	// Token: 0x04002B4A RID: 11082
	private HandleVector<int>.Handle diseaseHandle = HandleVector<int>.InvalidHandle;

	// Token: 0x04002B4B RID: 11083
	public float MassPerUnit = 1f;

	// Token: 0x04002B4C RID: 11084
	[NonSerialized]
	private Element _Element;

	// Token: 0x04002B4D RID: 11085
	[NonSerialized]
	public Action<PrimaryElement> onDataChanged;

	// Token: 0x04002B4E RID: 11086
	[NonSerialized]
	private bool forcePermanentDiseaseContainer;

	// Token: 0x04002B4F RID: 11087
	private static readonly EventSystem.IntraObjectHandler<PrimaryElement> OnSplitFromChunkDelegate = new EventSystem.IntraObjectHandler<PrimaryElement>(delegate(PrimaryElement component, object data)
	{
		component.OnSplitFromChunk(data);
	});

	// Token: 0x04002B50 RID: 11088
	private static readonly EventSystem.IntraObjectHandler<PrimaryElement> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<PrimaryElement>(delegate(PrimaryElement component, object data)
	{
		component.OnAbsorb(data);
	});

	// Token: 0x02001743 RID: 5955
	// (Invoke) Token: 0x06008E02 RID: 36354
	public delegate float GetTemperatureCallback(PrimaryElement primary_element);

	// Token: 0x02001744 RID: 5956
	// (Invoke) Token: 0x06008E06 RID: 36358
	public delegate void SetTemperatureCallback(PrimaryElement primary_element, float temperature);
}
