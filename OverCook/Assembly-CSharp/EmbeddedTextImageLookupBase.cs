using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200036F RID: 879
public abstract class EmbeddedTextImageLookupBase : MonoBehaviour, ITextProcessor
{
	// Token: 0x1700023A RID: 570
	// (get) Token: 0x060010C8 RID: 4296 RVA: 0x000605D5 File Offset: 0x0005E9D5
	// (set) Token: 0x060010C9 RID: 4297 RVA: 0x000605DD File Offset: 0x0005E9DD
	public Text Text
	{
		get
		{
			return this.m_text;
		}
		set
		{
			this.m_text = value;
		}
	}

	// Token: 0x060010CA RID: 4298
	protected abstract Sprite GetIcon(int _iconID);

	// Token: 0x060010CB RID: 4299 RVA: 0x000605E6 File Offset: 0x0005E9E6
	public bool HasEmbeddedImages(string markupString)
	{
		return Regex.IsMatch(markupString, EmbeddedTextImageLookupBase.m_lookupPattern);
	}

	// Token: 0x060010CC RID: 4300 RVA: 0x000605F4 File Offset: 0x0005E9F4
	public void RefreshImage()
	{
		foreach (KeyValuePair<int, EmbeddedTextImageLookupBase.Element> keyValuePair in this.m_elements)
		{
			EmbeddedTextImageLookupBase.Element value = keyValuePair.Value;
			if (value.Overlay != null)
			{
				value.Overlay.Icon = this.GetIcon(value.ImageMaterialNum);
			}
			if (value.Image != null)
			{
				value.Image.sprite = this.GetIcon(value.ImageMaterialNum);
			}
		}
	}

	// Token: 0x060010CD RID: 4301 RVA: 0x0006069C File Offset: 0x0005EA9C
	public bool OnPopulateMesh(VertexHelper _helper)
	{
		if (Application.isPlaying)
		{
			this.UpdateMesh(_helper);
		}
		return true;
	}

	// Token: 0x060010CE RID: 4302 RVA: 0x000606B0 File Offset: 0x0005EAB0
	private void LateUpdate()
	{
		foreach (KeyValuePair<int, EmbeddedTextImageLookupBase.Element> keyValuePair in this.m_elements)
		{
			EmbeddedTextImageLookupBase.Element value = keyValuePair.Value;
			if (value.Overlay != null)
			{
				if (value.Image == null)
				{
					GameObject gameObject = GameObjectUtils.CreateOnParent(base.gameObject, "ChildImage");
					value.Image = gameObject.AddComponent<Image>();
				}
				RectTransform rectTransform = base.gameObject.RequireComponent<RectTransform>();
				float num = value.Overlay.X + this.m_offset.x;
				float num2 = value.Overlay.Y + this.m_offset.y;
				value.Image.rectTransform.anchorMin = rectTransform.pivot;
				value.Image.rectTransform.anchorMax = rectTransform.pivot;
				value.Image.rectTransform.localScale = Vector3.one;
				value.Image.rectTransform.offsetMax = new Vector3(num + value.Overlay.Width, num2 + value.Overlay.Height, 0f);
				value.Image.rectTransform.offsetMin = new Vector3(num, num2, 0f);
				value.Image.sprite = value.Overlay.Icon;
				value.Overlay = null;
			}
		}
		for (int i = 0; i < this.m_buttonToDestroy.Count; i++)
		{
			EmbeddedTextImageLookupBase.Element element = this.m_buttonToDestroy[i];
			if (element.Image != null)
			{
				UnityEngine.Object.Destroy(element.Image.gameObject);
			}
		}
		this.m_buttonToDestroy.Clear();
	}

	// Token: 0x060010CF RID: 4303 RVA: 0x000608AC File Offset: 0x0005ECAC
	private void UpdateMesh(VertexHelper _helper)
	{
		Match match = Regex.Match(this.m_text.text, EmbeddedTextImageLookupBase.m_lookupPattern);
		int count = 0;
		while (match.Success)
		{
			int num = match.Index * 4;
			if (_helper.currentVertCount <= num + 4)
			{
				break;
			}
			UIVertex[] array = new UIVertex[]
			{
				default(UIVertex),
				default(UIVertex),
				default(UIVertex),
				default(UIVertex)
			};
			for (int i = 0; i < 4; i++)
			{
				_helper.PopulateUIVertex(ref array[i], num + i);
				_helper.SetUIVertex(new UIVertex
				{
					position = array[i].position,
					normal = array[i].normal,
					tangent = array[i].tangent,
					uv0 = Vector2.zero,
					uv1 = Vector2.zero
				}, num + i);
			}
			float num2 = Mathf.Min(new float[]
			{
				array[0].position.x,
				array[1].position.x,
				array[2].position.x,
				array[3].position.x
			});
			float num3 = Mathf.Min(new float[]
			{
				array[0].position.y,
				array[1].position.y,
				array[2].position.y,
				array[3].position.y
			});
			float width = Mathf.Max(new float[]
			{
				array[0].position.x,
				array[1].position.x,
				array[2].position.x,
				array[3].position.x
			}) - num2;
			float height = Mathf.Max(new float[]
			{
				array[0].position.y,
				array[1].position.y,
				array[2].position.y,
				array[3].position.y
			}) - num3;
			int num4 = int.Parse(match.Groups[1].Value);
			EmbeddedTextImageLookupBase.Element element = this.m_elements.CreationGet(count);
			element.Overlay = new EmbeddedTextImageLookupBase.ImageOverlay(num2, num3, width, height, this.GetIcon(num4));
			element.ImageMaterialNum = num4;
			count++;
			match = match.NextMatch();
		}
		foreach (KeyValuePair<int, EmbeddedTextImageLookupBase.Element> keyValuePair in this.m_elements)
		{
			if (keyValuePair.Key >= count)
			{
				List<EmbeddedTextImageLookupBase.Element> buttonToDestroy = this.m_buttonToDestroy;
				Dictionary<int, EmbeddedTextImageLookupBase.Element>.Enumerator enumerator;
				KeyValuePair<int, EmbeddedTextImageLookupBase.Element> keyValuePair2 = enumerator.Current;
				buttonToDestroy.Add(keyValuePair2.Value);
			}
		}
		this.m_elements.RemoveAll((KeyValuePair<int, EmbeddedTextImageLookupBase.Element> x) => x.Key >= count);
	}

	// Token: 0x060010D0 RID: 4304 RVA: 0x00060C46 File Offset: 0x0005F046
	public bool ProcessText(ref string inputString)
	{
		return false;
	}

	// Token: 0x04000CF1 RID: 3313
	[SerializeField]
	[AssignComponent(Editorbility.NonEditable)]
	private Text m_text;

	// Token: 0x04000CF2 RID: 3314
	[SerializeField]
	private Vector2 m_offset;

	// Token: 0x04000CF3 RID: 3315
	private Dictionary<int, EmbeddedTextImageLookupBase.Element> m_elements = new Dictionary<int, EmbeddedTextImageLookupBase.Element>();

	// Token: 0x04000CF4 RID: 3316
	private List<EmbeddedTextImageLookupBase.Element> m_buttonToDestroy = new List<EmbeddedTextImageLookupBase.Element>();

	// Token: 0x04000CF5 RID: 3317
	private static string m_lookupPattern = "<quad\\s*material\\s*=\\s*(\\d+)";

	// Token: 0x02000370 RID: 880
	private class Element
	{
		// Token: 0x04000CF6 RID: 3318
		public EmbeddedTextImageLookupBase.ImageOverlay Overlay;

		// Token: 0x04000CF7 RID: 3319
		public Image Image;

		// Token: 0x04000CF8 RID: 3320
		public int ImageMaterialNum;
	}

	// Token: 0x02000371 RID: 881
	private class ImageOverlay
	{
		// Token: 0x060010D3 RID: 4307 RVA: 0x00060C5D File Offset: 0x0005F05D
		public ImageOverlay(float _x, float _y, float _width, float _height, Sprite _icon)
		{
			this.X = _x;
			this.Y = _y;
			this.Width = _width;
			this.Height = _height;
			this.Icon = _icon;
		}

		// Token: 0x04000CF9 RID: 3321
		public float X;

		// Token: 0x04000CFA RID: 3322
		public float Y;

		// Token: 0x04000CFB RID: 3323
		public float Width;

		// Token: 0x04000CFC RID: 3324
		public float Height;

		// Token: 0x04000CFD RID: 3325
		public Sprite Icon;
	}
}
