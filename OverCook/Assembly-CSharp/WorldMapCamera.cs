using System;
using UnityEngine;
using UnityEngine.PostProcessing;

// Token: 0x02000BDD RID: 3037
public class WorldMapCamera : MonoBehaviour
{
	// Token: 0x17000430 RID: 1072
	// (get) Token: 0x06003DF4 RID: 15860 RVA: 0x00127B05 File Offset: 0x00125F05
	public Vector3 AccessIdealOffset
	{
		get
		{
			return this.m_followCamera.IdealOffset;
		}
	}

	// Token: 0x17000431 RID: 1073
	// (get) Token: 0x06003DF5 RID: 15861 RVA: 0x00127B12 File Offset: 0x00125F12
	public GameObject AccessAvatar
	{
		get
		{
			return this.m_avatar;
		}
	}

	// Token: 0x06003DF6 RID: 15862 RVA: 0x00127B1A File Offset: 0x00125F1A
	public Vector3 GetIdealLocation()
	{
		return this.m_followCamera.GetIdealLocation();
	}

	// Token: 0x06003DF7 RID: 15863 RVA: 0x00127B28 File Offset: 0x00125F28
	public void Awake()
	{
		this.m_mapControls = this.m_avatar.RequestComponent<MapAvatarControls>();
		this.m_followCamera = base.gameObject.AddComponent<FollowCamera>();
		this.m_followCamera.Target = this.m_avatar;
		this.m_followCamera.IdealOffset = base.transform.position - this.m_avatar.transform.position;
		this.m_followCamera.GradientLimit = this.m_gradientLimit;
		this.m_followCamera.TimeToMax = this.m_timeToMax;
		this.m_followCamera.enabled = base.enabled;
		if (this.m_postProcessingBehaviour != null)
		{
			this.m_postProcessingBehaviour.profile = UnityEngine.Object.Instantiate<PostProcessingProfile>(this.m_postProcessingBehaviour.profile);
		}
	}

	// Token: 0x06003DF8 RID: 15864 RVA: 0x00127BF2 File Offset: 0x00125FF2
	public void Initialise()
	{
		base.transform.position = this.GetIdealLocation();
	}

	// Token: 0x06003DF9 RID: 15865 RVA: 0x00127C05 File Offset: 0x00126005
	private void OnEnable()
	{
		this.m_followCamera.enabled = true;
	}

	// Token: 0x06003DFA RID: 15866 RVA: 0x00127C13 File Offset: 0x00126013
	private void OnDisable()
	{
		this.m_followCamera.enabled = false;
	}

	// Token: 0x06003DFB RID: 15867 RVA: 0x00127C24 File Offset: 0x00126024
	private void Update()
	{
		Vector3 idealLocation = this.m_followCamera.GetIdealLocation();
		Vector2 a = idealLocation.XZ() - base.transform.position.XZ();
		float num = Mathf.Max(1f, a.DividedBy(this.m_thresholdDistanceFromIdeal).magnitude);
		float num2 = Mathf.Max(1f, this.m_mapControls.GetUnclampedMovementSpeed());
		this.m_followCamera.GradientLimit = Mathf.Max(num2 * this.m_gradientLimit, this.m_mapControls.GetSpeed()) * num;
		this.m_followCamera.TimeToMax = this.m_timeToMax / num2;
		if (this.m_focalPoint != null)
		{
			this.m_focalPoint.position = base.transform.position - this.m_followCamera.IdealOffset;
		}
	}

	// Token: 0x040031BF RID: 12735
	[SerializeField]
	private GameObject m_avatar;

	// Token: 0x040031C0 RID: 12736
	[SerializeField]
	private float m_gradientLimit = 0.5f;

	// Token: 0x040031C1 RID: 12737
	[SerializeField]
	private float m_timeToMax = 0.5f;

	// Token: 0x040031C2 RID: 12738
	[SerializeField]
	[AssignChild("FocalPoint", Editorbility.Editable)]
	private Transform m_focalPoint;

	// Token: 0x040031C3 RID: 12739
	[SerializeField]
	private Vector2 m_thresholdDistanceFromIdeal = new Vector2(7f, 3f);

	// Token: 0x040031C4 RID: 12740
	[SerializeField]
	[AssignComponent(Editorbility.Editable)]
	private PostProcessingBehaviour m_postProcessingBehaviour;

	// Token: 0x040031C5 RID: 12741
	private MapAvatarControls m_mapControls;

	// Token: 0x040031C6 RID: 12742
	private FollowCamera m_followCamera;
}
