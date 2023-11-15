using System;
using System.Collections.Generic;
using Highlighters;
using UnityEngine;

// Token: 0x020000D4 RID: 212
public class OutlineController : MonoBehaviour
{
	// Token: 0x060004FB RID: 1275 RVA: 0x00014000 File Offset: 0x00012200
	private void OnEnable()
	{
		EventMgr.Register<Renderer, OutlineController.eOutlineType>(eGameEvents.RequestAddOutline, new Action<Renderer, OutlineController.eOutlineType>(this.OnRequestAddOutline));
		EventMgr.Register<List<Renderer>, OutlineController.eOutlineType>(eGameEvents.RequestAddOutlineByList, new Action<List<Renderer>, OutlineController.eOutlineType>(this.OnRequestAddOutlineByList));
		EventMgr.Register<Renderer>(eGameEvents.RequestRemoveOutline, new Action<Renderer>(this.OnRequestRemoveOutline));
		EventMgr.Register<List<Renderer>>(eGameEvents.RequestRemoveOutlineByList, new Action<List<Renderer>>(this.OnRequestRemoveOutlineByList));
		EventMgr.Register<List<Renderer>, OutlineController.eOutlineType>(eGameEvents.RequestRemoveOutlineByListAndType, new Action<List<Renderer>, OutlineController.eOutlineType>(this.OnRequestRemoveOutlineByListAndType));
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x00014094 File Offset: 0x00012294
	private void OnDisable()
	{
		EventMgr.Remove<Renderer, OutlineController.eOutlineType>(eGameEvents.RequestAddOutline, new Action<Renderer, OutlineController.eOutlineType>(this.OnRequestAddOutline));
		EventMgr.Remove<List<Renderer>, OutlineController.eOutlineType>(eGameEvents.RequestAddOutlineByList, new Action<List<Renderer>, OutlineController.eOutlineType>(this.OnRequestAddOutlineByList));
		EventMgr.Remove<Renderer>(eGameEvents.RequestRemoveOutline, new Action<Renderer>(this.OnRequestRemoveOutline));
		EventMgr.Remove<List<Renderer>>(eGameEvents.RequestRemoveOutlineByList, new Action<List<Renderer>>(this.OnRequestRemoveOutlineByList));
		EventMgr.Remove<List<Renderer>, OutlineController.eOutlineType>(eGameEvents.RequestRemoveOutlineByListAndType, new Action<List<Renderer>, OutlineController.eOutlineType>(this.OnRequestRemoveOutlineByListAndType));
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x00014128 File Offset: 0x00012328
	private void OnRequestAddOutlineByList(List<Renderer> list_Renderers, OutlineController.eOutlineType type)
	{
		Highlighter highlighterByType = this.GetHighlighterByType(type);
		highlighterByType.enabled = true;
		foreach (Renderer renderer in list_Renderers)
		{
			HighlighterRenderer item = new HighlighterRenderer(renderer, renderer.sharedMaterials.Length);
			highlighterByType.Renderers.Add(item);
			this.dic_RendererToHighlighter.Add(renderer, highlighterByType);
		}
		highlighterByType.HighlighterValidate();
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x000141AC File Offset: 0x000123AC
	private void OnRequestRemoveOutlineByList(List<Renderer> list_Renderers)
	{
		using (List<Renderer>.Enumerator enumerator = list_Renderers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Renderer renderer = enumerator.Current;
				if (this.dic_RendererToHighlighter.ContainsKey(renderer))
				{
					Highlighter highlighter = this.dic_RendererToHighlighter[renderer];
					HighlighterRenderer item = highlighter.Renderers.Find((HighlighterRenderer a) => a.renderer == renderer);
					highlighter.Renderers.Remove(item);
					this.dic_RendererToHighlighter.Remove(renderer);
					if (highlighter.Renderers.Count == 0)
					{
						highlighter.enabled = false;
					}
				}
			}
		}
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x00014274 File Offset: 0x00012474
	private void OnRequestRemoveOutlineByListAndType(List<Renderer> list_Renderers, OutlineController.eOutlineType outlineType)
	{
		using (List<Renderer>.Enumerator enumerator = list_Renderers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Renderer renderer = enumerator.Current;
				if (this.dic_RendererToHighlighter.ContainsKey(renderer))
				{
					Highlighter highlighter = this.list_Highlighter[(int)outlineType];
					HighlighterRenderer item = highlighter.Renderers.Find((HighlighterRenderer a) => a.renderer == renderer);
					highlighter.Renderers.Remove(item);
					this.dic_RendererToHighlighter.Remove(renderer);
					if (highlighter.Renderers.Count == 0)
					{
						highlighter.enabled = false;
					}
				}
			}
		}
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x0001433C File Offset: 0x0001253C
	private void OnRequestAddOutline(Renderer renderer, OutlineController.eOutlineType type)
	{
		Highlighter highlighterByType = this.GetHighlighterByType(type);
		highlighterByType.enabled = true;
		HighlighterRenderer item = new HighlighterRenderer(renderer, renderer.sharedMaterials.Length);
		highlighterByType.Renderers.Add(item);
		this.dic_RendererToHighlighter.Add(renderer, highlighterByType);
		highlighterByType.HighlighterValidate();
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x00014388 File Offset: 0x00012588
	private void OnRequestRemoveOutline(Renderer renderer)
	{
		if (!this.dic_RendererToHighlighter.ContainsKey(renderer))
		{
			return;
		}
		Highlighter highlighter = this.dic_RendererToHighlighter[renderer];
		HighlighterRenderer item = highlighter.Renderers.Find((HighlighterRenderer a) => a.renderer == renderer);
		highlighter.Renderers.Remove(item);
		this.dic_RendererToHighlighter.Remove(renderer);
		if (highlighter.Renderers.Count == 0)
		{
			highlighter.enabled = false;
		}
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x00014413 File Offset: 0x00012613
	public Highlighter GetHighlighterByType(OutlineController.eOutlineType type)
	{
		return this.list_Highlighter[(int)type];
	}

	// Token: 0x040004C3 RID: 1219
	[SerializeField]
	private List<Highlighter> list_Highlighter;

	// Token: 0x040004C4 RID: 1220
	private Dictionary<Renderer, Highlighter> dic_RendererToHighlighter = new Dictionary<Renderer, Highlighter>();

	// Token: 0x0200023F RID: 575
	public enum eOutlineType
	{
		// Token: 0x04000B1F RID: 2847
		BASIC,
		// Token: 0x04000B20 RID: 2848
		BUILD_BLUE,
		// Token: 0x04000B21 RID: 2849
		BUILD_RED,
		// Token: 0x04000B22 RID: 2850
		BUFF_EFFECT
	}
}
