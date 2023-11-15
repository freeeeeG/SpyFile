using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200023D RID: 573
public class EmojiText : Text
{
	// Token: 0x1700052F RID: 1327
	// (get) Token: 0x06000EBB RID: 3771 RVA: 0x00026160 File Offset: 0x00024360
	public override float preferredWidth
	{
		get
		{
			return base.cachedTextGeneratorForLayout.GetPreferredWidth(this.emojiText, base.GetGenerationSettings(base.rectTransform.rect.size)) / base.pixelsPerUnit;
		}
	}

	// Token: 0x17000530 RID: 1328
	// (get) Token: 0x06000EBC RID: 3772 RVA: 0x000261A0 File Offset: 0x000243A0
	public override float preferredHeight
	{
		get
		{
			return base.cachedTextGeneratorForLayout.GetPreferredHeight(this.emojiText, base.GetGenerationSettings(base.rectTransform.rect.size)) / base.pixelsPerUnit;
		}
	}

	// Token: 0x17000531 RID: 1329
	// (get) Token: 0x06000EBD RID: 3773 RVA: 0x000261DE File Offset: 0x000243DE
	private string emojiText
	{
		get
		{
			return Regex.Replace(this.text, "\\[[0-9]+\\]", "XX");
		}
	}

	// Token: 0x06000EBE RID: 3774 RVA: 0x000261F8 File Offset: 0x000243F8
	protected override void OnPopulateMesh(VertexHelper toFill)
	{
		if (base.font == null)
		{
			return;
		}
		if (EmojiText.m_EmojiIndexDict == null)
		{
			EmojiText.m_EmojiIndexDict = new Dictionary<string, EmojiText.EmojiInfo>();
			string[] array = Resources.Load<TextAsset>("emoji").text.Split('\n', StringSplitOptions.None);
			for (int i = 1; i < array.Length; i++)
			{
				if (!string.IsNullOrEmpty(array[i]))
				{
					string[] array2 = array[i].Split('\t', StringSplitOptions.None);
					EmojiText.EmojiInfo value;
					value.x = float.Parse(array2[3]);
					value.y = float.Parse(array2[4]);
					value.size = float.Parse(array2[5]);
					EmojiText.m_EmojiIndexDict.Add(array2[1], value);
				}
			}
		}
		Dictionary<int, EmojiText.EmojiInfo> dictionary = new Dictionary<int, EmojiText.EmojiInfo>();
		if (base.supportRichText)
		{
			int num = 0;
			int num2 = 0;
			MatchCollection matchCollection = Regex.Matches(this.text, "\\[[0-9]+\\]");
			for (int j = 0; j < matchCollection.Count; j++)
			{
				EmojiText.EmojiInfo value2;
				if (EmojiText.m_EmojiIndexDict.TryGetValue(matchCollection[j].Value, out value2))
				{
					dictionary.Add(matchCollection[j].Index - num2 + num, value2);
					num2 += matchCollection[j].Length - 1;
					num++;
				}
			}
		}
		this.m_DisableFontTextureRebuiltCallback = true;
		Vector2 size = base.rectTransform.rect.size;
		TextGenerationSettings generationSettings = base.GetGenerationSettings(size);
		base.cachedTextGenerator.Populate(this.emojiText, generationSettings);
		Rect rect = base.rectTransform.rect;
		Vector2 textAnchorPivot = Text.GetTextAnchorPivot(base.alignment);
		Vector2 zero = Vector2.zero;
		zero.x = Mathf.Lerp(rect.xMin, rect.xMax, textAnchorPivot.x);
		zero.y = Mathf.Lerp(rect.yMin, rect.yMax, textAnchorPivot.y);
		IList<UIVertex> verts = base.cachedTextGenerator.verts;
		float d = 1f / base.pixelsPerUnit;
		int count = verts.Count;
		if (count <= 0)
		{
			toFill.Clear();
			return;
		}
		Vector2 vector = new Vector2(verts[0].position.x, verts[0].position.y) * d;
		vector = base.PixelAdjustPoint(vector) - vector;
		toFill.Clear();
		if (vector != Vector2.zero)
		{
			for (int k = 0; k < count; k++)
			{
				int num3 = k & 3;
				this.m_TempVerts[num3] = verts[k];
				UIVertex[] tempVerts = this.m_TempVerts;
				int num4 = num3;
				tempVerts[num4].position = tempVerts[num4].position * d;
				UIVertex[] tempVerts2 = this.m_TempVerts;
				int num5 = num3;
				tempVerts2[num5].position.x = tempVerts2[num5].position.x + vector.x;
				UIVertex[] tempVerts3 = this.m_TempVerts;
				int num6 = num3;
				tempVerts3[num6].position.y = tempVerts3[num6].position.y + vector.y;
				if (num3 == 3)
				{
					toFill.AddUIVertexQuad(this.m_TempVerts);
				}
			}
		}
		else
		{
			for (int l = 0; l < count; l++)
			{
				int key = l / 4;
				EmojiText.EmojiInfo emojiInfo;
				if (dictionary.TryGetValue(key, out emojiInfo))
				{
					float num7 = 2f * (verts[l + 1].position.x - verts[l].position.x) * 0.7f;
					float num8 = verts[l + 1].position.y - verts[l + 2].position.y;
					float num9 = verts[l + 1].position.x - verts[l].position.x;
					float num10 = (num7 - num8) * 0.5f;
					float num11 = num7 * 0.3f;
					this.m_TempVerts[3] = verts[l];
					this.m_TempVerts[2] = verts[l + 1];
					this.m_TempVerts[1] = verts[l + 2];
					this.m_TempVerts[0] = verts[l + 3];
					UIVertex[] tempVerts4 = this.m_TempVerts;
					int num12 = 0;
					tempVerts4[num12].position = tempVerts4[num12].position + new Vector3(num11, -num10, 0f);
					UIVertex[] tempVerts5 = this.m_TempVerts;
					int num13 = 1;
					tempVerts5[num13].position = tempVerts5[num13].position + new Vector3(num11 - num9 + num7, -num10, 0f);
					UIVertex[] tempVerts6 = this.m_TempVerts;
					int num14 = 2;
					tempVerts6[num14].position = tempVerts6[num14].position + new Vector3(num11 - num9 + num7, num10, 0f);
					UIVertex[] tempVerts7 = this.m_TempVerts;
					int num15 = 3;
					tempVerts7[num15].position = tempVerts7[num15].position + new Vector3(num11, num10, 0f);
					UIVertex[] tempVerts8 = this.m_TempVerts;
					int num16 = 0;
					tempVerts8[num16].position = tempVerts8[num16].position * d;
					UIVertex[] tempVerts9 = this.m_TempVerts;
					int num17 = 1;
					tempVerts9[num17].position = tempVerts9[num17].position * d;
					UIVertex[] tempVerts10 = this.m_TempVerts;
					int num18 = 2;
					tempVerts10[num18].position = tempVerts10[num18].position * d;
					UIVertex[] tempVerts11 = this.m_TempVerts;
					int num19 = 3;
					tempVerts11[num19].position = tempVerts11[num19].position * d;
					float num20 = dictionary[key].size / 32f / 2f;
					this.m_TempVerts[0].uv1 = new Vector2(dictionary[key].x + num20, dictionary[key].y + num20);
					this.m_TempVerts[1].uv1 = new Vector2(dictionary[key].x - num20 + dictionary[key].size, dictionary[key].y + num20);
					this.m_TempVerts[2].uv1 = new Vector2(dictionary[key].x - num20 + dictionary[key].size, dictionary[key].y - num20 + dictionary[key].size);
					this.m_TempVerts[3].uv1 = new Vector2(dictionary[key].x + num20, dictionary[key].y - num20 + dictionary[key].size);
					toFill.AddUIVertexQuad(this.m_TempVerts);
					l += 7;
				}
				else
				{
					int num21 = l & 3;
					this.m_TempVerts[num21] = verts[l];
					UIVertex[] tempVerts12 = this.m_TempVerts;
					int num22 = num21;
					tempVerts12[num22].position = tempVerts12[num22].position * d;
					if (num21 == 3)
					{
						toFill.AddUIVertexQuad(this.m_TempVerts);
					}
				}
			}
		}
		this.m_DisableFontTextureRebuiltCallback = false;
	}

	// Token: 0x04000720 RID: 1824
	private const float ICON_SCALE_OF_DOUBLE_SYMBOLE = 0.7f;

	// Token: 0x04000721 RID: 1825
	private static Dictionary<string, EmojiText.EmojiInfo> m_EmojiIndexDict;

	// Token: 0x04000722 RID: 1826
	private readonly UIVertex[] m_TempVerts = new UIVertex[4];

	// Token: 0x020002FC RID: 764
	private struct EmojiInfo
	{
		// Token: 0x04000A57 RID: 2647
		public float x;

		// Token: 0x04000A58 RID: 2648
		public float y;

		// Token: 0x04000A59 RID: 2649
		public float size;
	}
}
