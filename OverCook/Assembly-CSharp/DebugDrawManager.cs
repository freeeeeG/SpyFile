using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001A9 RID: 425
[AddComponentMenu("Scripts/Core/Debug/DebugDrawManager")]
public class DebugDrawManager : MonoBehaviour
{
	// Token: 0x06000728 RID: 1832 RVA: 0x0002E8FA File Offset: 0x0002CCFA
	public void AddLogText(string _text, Color _color)
	{
		this.m_onScreenLog.Add(_text, _color);
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x0002E909 File Offset: 0x0002CD09
	public void AddText(DebugDrawManager.TextRequest2d _textRequest)
	{
		this.m_textRequests2d.Add(_textRequest);
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x0002E917 File Offset: 0x0002CD17
	public void AddText(DebugDrawManager.TextRequest3d _textRequest)
	{
		this.m_textRequests3d.Add(_textRequest);
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x0002E925 File Offset: 0x0002CD25
	private void Awake()
	{
		DebugUtils.debugDrawManager = this;
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x0002E930 File Offset: 0x0002CD30
	private void Update()
	{
		for (int i = 0; i < this.m_textRequests2d.Count; i++)
		{
			DebugDrawManager.TextRequest2d value = this.m_textRequests2d[i];
			value.m_lifeTime -= TimeManager.GetDeltaTime(base.gameObject);
			this.m_textRequests2d[i] = value;
		}
		for (int j = 0; j < this.m_textRequests3d.Count; j++)
		{
			DebugDrawManager.TextRequest3d value2 = this.m_textRequests3d[j];
			value2.m_lifeTime -= TimeManager.GetDeltaTime(base.gameObject);
			this.m_textRequests3d[j] = value2;
		}
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x0002E9DC File Offset: 0x0002CDDC
	private void Draw(DebugDrawManager.TextRequest2d text)
	{
		GUIContent content = new GUIContent(text.m_contents);
		GUIStyle label = GUI.skin.label;
		label.fontSize = text.m_fontSize;
		Vector2 vector = label.CalcSize(content);
		GUI.color = text.m_color;
		if (text.m_centred)
		{
			GUI.Label(new Rect(text.m_position.x - 0.5f * vector.x, text.m_position.y - 0.5f * vector.y, vector.x, vector.y), content, label);
		}
		else
		{
			GUI.Label(new Rect(text.m_position.x, text.m_position.y, vector.x, vector.y), content, label);
		}
	}

	// Token: 0x0600072E RID: 1838 RVA: 0x0002EAB4 File Offset: 0x0002CEB4
	private void Draw(DebugDrawManager.TextRequest3d text)
	{
		GUIContent content = new GUIContent(text.m_contents);
		GUIStyle label = GUI.skin.label;
		label.fontSize = text.m_fontSize;
		Vector2 vector = label.CalcSize(content);
		Vector2 vector2 = Camera.main.WorldToScreenPoint(text.m_position);
		vector2.y = (float)Camera.main.pixelHeight - vector2.y;
		GUI.color = text.m_color;
		if (text.m_centred)
		{
			GUI.Label(new Rect(vector2.x - 0.5f * vector.x, vector2.y - 0.5f * vector.y, vector.x, vector.y), content, label);
		}
		else
		{
			GUI.Label(new Rect(vector2.x, vector2.y, vector.x, vector.y), content, label);
		}
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x0002EBA8 File Offset: 0x0002CFA8
	private void OnGUI()
	{
		foreach (DebugDrawManager.TextRequest2d text in this.m_textRequests2d)
		{
			this.Draw(text);
		}
		foreach (DebugDrawManager.TextRequest3d text2 in this.m_textRequests3d)
		{
			this.Draw(text2);
		}
		this.m_onScreenLog.Draw();
		GUI.color = Color.white;
		Event current = Event.current;
		if (current.GetTypeForControl(0) == EventType.Repaint)
		{
			this.m_textRequests2d.RemoveAll((DebugDrawManager.TextRequest2d x) => x.m_lifeTime <= 0f);
			this.m_textRequests3d.RemoveAll((DebugDrawManager.TextRequest3d x) => x.m_lifeTime <= 0f);
		}
	}

	// Token: 0x040005F0 RID: 1520
	private List<DebugDrawManager.TextRequest2d> m_textRequests2d = new List<DebugDrawManager.TextRequest2d>();

	// Token: 0x040005F1 RID: 1521
	private List<DebugDrawManager.TextRequest3d> m_textRequests3d = new List<DebugDrawManager.TextRequest3d>();

	// Token: 0x040005F2 RID: 1522
	private DebugDrawManager.Log m_onScreenLog = new DebugDrawManager.Log();

	// Token: 0x020001AA RID: 426
	public struct TextRequest2d
	{
		// Token: 0x040005F5 RID: 1525
		public string m_contents;

		// Token: 0x040005F6 RID: 1526
		public Vector2 m_position;

		// Token: 0x040005F7 RID: 1527
		public Color m_color;

		// Token: 0x040005F8 RID: 1528
		public bool m_centred;

		// Token: 0x040005F9 RID: 1529
		public float m_lifeTime;

		// Token: 0x040005FA RID: 1530
		public int m_fontSize;
	}

	// Token: 0x020001AB RID: 427
	public struct TextRequest3d
	{
		// Token: 0x040005FB RID: 1531
		public string m_contents;

		// Token: 0x040005FC RID: 1532
		public Vector3 m_position;

		// Token: 0x040005FD RID: 1533
		public Color m_color;

		// Token: 0x040005FE RID: 1534
		public bool m_centred;

		// Token: 0x040005FF RID: 1535
		public float m_lifeTime;

		// Token: 0x04000600 RID: 1536
		public int m_fontSize;
	}

	// Token: 0x020001AC RID: 428
	private class Log
	{
		// Token: 0x06000733 RID: 1843 RVA: 0x0002ED08 File Offset: 0x0002D108
		public void Add(string _text, Color _color)
		{
			DebugDrawManager.Log.StringEntry item = default(DebugDrawManager.Log.StringEntry);
			item.Text = _text;
			item.Color = _color;
			this.m_text.Add(item);
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0002ED3C File Offset: 0x0002D13C
		public void Draw()
		{
			Camera main = Camera.main;
			if (main == null)
			{
				return;
			}
			Vector2 vector = new Vector2(0f, (float)main.pixelHeight);
			for (int i = this.m_text.Count - 1; i >= 0; i--)
			{
				if (vector.y < 0f)
				{
					this.m_text.RemoveAt(i);
				}
				else
				{
					DebugDrawManager.Log.StringEntry stringEntry = this.m_text[i];
					GUIContent content = new GUIContent(stringEntry.Text);
					GUIStyle label = GUI.skin.label;
					label.fontSize = 14;
					Vector2 vector2 = label.CalcSize(content);
					GUI.color = stringEntry.Color;
					GUI.Label(new Rect(vector.x, vector.y, vector2.x, vector2.y), content, label);
					vector.y -= vector2.y;
				}
			}
		}

		// Token: 0x04000601 RID: 1537
		private List<DebugDrawManager.Log.StringEntry> m_text = new List<DebugDrawManager.Log.StringEntry>();

		// Token: 0x020001AD RID: 429
		private struct StringEntry
		{
			// Token: 0x04000602 RID: 1538
			public string Text;

			// Token: 0x04000603 RID: 1539
			public Color Color;
		}
	}
}
