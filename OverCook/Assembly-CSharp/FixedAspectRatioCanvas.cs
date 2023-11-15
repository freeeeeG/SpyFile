using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001C1 RID: 449
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Canvas))]
public class FixedAspectRatioCanvas : MonoBehaviour
{
	// Token: 0x060007BE RID: 1982 RVA: 0x00030558 File Offset: 0x0002E958
	private void Awake()
	{
		this.m_transform = (base.transform as RectTransform);
		this.m_canvas = base.gameObject.RequireComponent<Canvas>();
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x0003057C File Offset: 0x0002E97C
	private void Start()
	{
		if (this.m_canvas == null || this.m_canvas.renderMode != RenderMode.ScreenSpaceOverlay)
		{
			return;
		}
		this.EnsureRootCanvas();
		this.FixUpCanvas();
		this.m_ratioManager = GameUtils.RequestManager<FixedAspectRatioManager>();
		if (this.m_ratioManager != null)
		{
			this.m_ratioManager.RegisterOnResolutionChanged(new GenericVoid<Rect, float, float>(this.OnResolutionChanged));
			this.m_ratioManager.InitialiseComponent(new GenericVoid<Rect, float, float>(this.OnResolutionChanged));
		}
		else
		{
			Vector2 defaultAspect = FixedAspectRatio.DefaultAspect;
			Rect rect = FixedAspectRatio.ComputeAspectRatioRect(defaultAspect.x, defaultAspect.y, (float)Screen.width, (float)Screen.height);
			this.m_transform.anchorMin = new Vector2(rect.xMin, rect.yMin);
			this.m_transform.anchorMax = new Vector2(rect.xMax, rect.yMax);
		}
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x00030668 File Offset: 0x0002EA68
	private void OnDestroy()
	{
		if (this.m_ratioManager != null)
		{
			this.m_ratioManager.UnregisterOnResolutionChanged(new GenericVoid<Rect, float, float>(this.OnResolutionChanged));
		}
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x00030694 File Offset: 0x0002EA94
	private void EnsureRootCanvas()
	{
		Transform transform = this.m_transform.FindParentRecursive("FixedAspectRootCanvas");
		if (transform == null)
		{
			if (this.m_transform.parent != null)
			{
				transform = this.m_transform.parent.Find("FixedAspectRootCanvas");
			}
			if (transform == null)
			{
				GameObject gameObject = this.m_rootCanvasPrefab.InstantiateOnParent(this.m_transform.parent, true);
				transform = gameObject.transform;
			}
		}
		this.m_rootCanvasTransform = (transform as RectTransform);
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x00030728 File Offset: 0x0002EB28
	private void FixUpCanvas()
	{
		CanvasScaler canvasScaler = base.gameObject.RequestComponent<CanvasScaler>();
		if (canvasScaler != null)
		{
			UnityEngine.Object.Destroy(canvasScaler);
		}
		this.m_transform.SetParent(this.m_rootCanvasTransform, false);
		this.m_transform.anchorMin = new Vector2(0f, 0f);
		this.m_transform.anchorMax = new Vector2(1f, 1f);
		this.m_transform.pivot = new Vector2(0.5f, 0.5f);
		this.m_transform.offsetMin = new Vector2(0f, 0f);
		this.m_transform.offsetMax = new Vector2(0f, 0f);
		this.m_transform.localScale = Vector3.one;
		this.m_canvas.overrideSorting = true;
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x00030803 File Offset: 0x0002EC03
	private void OnResolutionChanged(Rect _rect, float _screenWidth, float _screenHeight)
	{
		this.m_transform.anchorMin = new Vector2(_rect.xMin, _rect.yMin);
		this.m_transform.anchorMax = new Vector2(_rect.xMax, _rect.yMax);
	}

	// Token: 0x04000624 RID: 1572
	[SerializeField]
	[AssignResource("FixedAspectRootCanvas", Editorbility.NonEditable)]
	private GameObject m_rootCanvasPrefab;

	// Token: 0x04000625 RID: 1573
	private RectTransform m_transform;

	// Token: 0x04000626 RID: 1574
	private Canvas m_canvas;

	// Token: 0x04000627 RID: 1575
	private RectTransform m_rootCanvasTransform;

	// Token: 0x04000628 RID: 1576
	private FixedAspectRatioManager m_ratioManager;
}
