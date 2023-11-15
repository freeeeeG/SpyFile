using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009AD RID: 2477
public class LivesGUI : MonoBehaviour
{
	// Token: 0x0600307C RID: 12412 RVA: 0x000E3FCD File Offset: 0x000E23CD
	public int GetLives()
	{
		return this.m_lifeSprites.Count;
	}

	// Token: 0x0600307D RID: 12413 RVA: 0x000E3FDC File Offset: 0x000E23DC
	public void SetLives(int _numLives)
	{
		this.m_startingLives = _numLives;
		if (this.m_lifeSprites.Count < _numLives)
		{
			while (this.m_lifeSprites.Count < _numLives)
			{
				this.AddLife();
			}
		}
		else
		{
			for (int i = this.m_lifeSprites.Count - 1; i >= _numLives; i--)
			{
				SpriteGUI obj = this.m_lifeSprites[i];
				UnityEngine.Object.Destroy(obj);
				this.m_lifeSprites.RemoveAt(i);
			}
		}
	}

	// Token: 0x0600307E RID: 12414 RVA: 0x000E405F File Offset: 0x000E245F
	private void Start()
	{
		this.SetLives(this.m_startingLives);
	}

	// Token: 0x0600307F RID: 12415 RVA: 0x000E4070 File Offset: 0x000E2470
	private void AddLife()
	{
		GameObject gameObject = GameObjectUtils.CreateOnParent<SpriteGUI>(base.gameObject, "Life");
		SpriteGUI component = gameObject.GetComponent<SpriteGUI>();
		GUIRect guirect = this.m_iconDisplayRect.DeepCopy();
		GUIRect guirect2 = guirect;
		guirect2.m_rect.x = guirect2.m_rect.x + (float)this.m_lifeSprites.Count * guirect.m_rect.width;
		component.SetData(guirect, this.m_texture, true);
		this.m_lifeSprites.Add(component);
	}

	// Token: 0x040026E8 RID: 9960
	[SerializeField]
	private GUIRect m_iconDisplayRect;

	// Token: 0x040026E9 RID: 9961
	[SerializeField]
	private SubTexture2D m_texture;

	// Token: 0x040026EA RID: 9962
	[SerializeField]
	private int m_startingLives;

	// Token: 0x040026EB RID: 9963
	private List<SpriteGUI> m_lifeSprites = new List<SpriteGUI>();
}
