using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Highlighters
{
	// Token: 0x02000012 RID: 18
	public class TestSceneManager : MonoBehaviour
	{
		// Token: 0x0600004A RID: 74 RVA: 0x00003138 File Offset: 0x00001338
		private void Start()
		{
			foreach (GameObject gameObject in this.objectsToShow)
			{
				gameObject.SetActive(false);
			}
			if (this.objectsToShow.Count > 0)
			{
				this.objectsToShow[0].SetActive(true);
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000031AC File Offset: 0x000013AC
		private void Update()
		{
			if (this.objectsToShow.Count < 1)
			{
				return;
			}
			if (this.text != null)
			{
				this.text.text = (this.currentObjectActiveID + 1).ToString() + " / " + this.objectsToShow.Count.ToString();
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				this.Previous();
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				this.Next();
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003234 File Offset: 0x00001434
		public void Next()
		{
			foreach (GameObject gameObject in this.objectsToShow)
			{
				gameObject.SetActive(false);
			}
			this.currentObjectActiveID++;
			this.currentObjectActiveID %= this.objectsToShow.Count;
			this.objectsToShow[this.currentObjectActiveID].SetActive(true);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000032C4 File Offset: 0x000014C4
		public void Previous()
		{
			foreach (GameObject gameObject in this.objectsToShow)
			{
				gameObject.SetActive(false);
			}
			this.currentObjectActiveID--;
			if (this.currentObjectActiveID < 0)
			{
				this.currentObjectActiveID = this.objectsToShow.Count - 1;
			}
			this.objectsToShow[this.currentObjectActiveID].SetActive(true);
		}

		// Token: 0x0400003A RID: 58
		public List<GameObject> objectsToShow = new List<GameObject>();

		// Token: 0x0400003B RID: 59
		public Text text;

		// Token: 0x0400003C RID: 60
		private int currentObjectActiveID;
	}
}
