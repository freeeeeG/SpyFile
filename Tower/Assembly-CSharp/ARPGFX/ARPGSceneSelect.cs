using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ARPGFX
{
	// Token: 0x0200006D RID: 109
	public class ARPGSceneSelect : MonoBehaviour
	{
		// Token: 0x0600018A RID: 394 RVA: 0x000072DB File Offset: 0x000054DB
		public void LoadSceneDemo1()
		{
			SceneManager.LoadScene("ARPGDemo01");
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000072E7 File Offset: 0x000054E7
		public void LoadSceneDemo2()
		{
			SceneManager.LoadScene("ARPGDemo02");
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000072F3 File Offset: 0x000054F3
		public void LoadSceneDemo3()
		{
			SceneManager.LoadScene("ARPGDemo03");
		}

		// Token: 0x0600018D RID: 397 RVA: 0x000072FF File Offset: 0x000054FF
		public void LoadSceneDemo4()
		{
			SceneManager.LoadScene("ARPGDemo04");
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000730B File Offset: 0x0000550B
		public void LoadSceneDemo5()
		{
			SceneManager.LoadScene("ARPGDemo05");
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00007317 File Offset: 0x00005517
		public void LoadSceneDemo6()
		{
			SceneManager.LoadScene("ARPGDemo06");
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00007323 File Offset: 0x00005523
		public void LoadSceneDemo7()
		{
			SceneManager.LoadScene("ARPGDemo07");
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000732F File Offset: 0x0000552F
		public void LoadSceneDemo8()
		{
			SceneManager.LoadScene("ARPGDemo08");
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000733B File Offset: 0x0000553B
		public void LoadSceneDemo9()
		{
			SceneManager.LoadScene("ARPGDemo09");
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00007347 File Offset: 0x00005547
		public void LoadSceneDemo10()
		{
			SceneManager.LoadScene("ARPGDemo10");
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00007353 File Offset: 0x00005553
		public void LoadSceneDemo11()
		{
			SceneManager.LoadScene("ARPGDemo11");
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000735F File Offset: 0x0000555F
		public void LoadSceneDemo12()
		{
			SceneManager.LoadScene("ARPGDemo12");
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000736B File Offset: 0x0000556B
		public void LoadSceneDemo13()
		{
			SceneManager.LoadScene("ARPGDemo13");
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00007377 File Offset: 0x00005577
		public void LoadSceneDemo14()
		{
			SceneManager.LoadScene("ARPGDemo14");
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00007384 File Offset: 0x00005584
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.J))
			{
				this.GUIHide = !this.GUIHide;
				if (this.GUIHide)
				{
					GameObject.Find("Canvas2").GetComponent<Canvas>().enabled = false;
				}
				else
				{
					GameObject.Find("Canvas2").GetComponent<Canvas>().enabled = true;
				}
			}
			if (Input.GetKeyDown(KeyCode.K))
			{
				this.GUIHide2 = !this.GUIHide2;
				if (this.GUIHide2)
				{
					GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;
					return;
				}
				GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
			}
		}

		// Token: 0x04000181 RID: 385
		public bool GUIHide;

		// Token: 0x04000182 RID: 386
		public bool GUIHide2;
	}
}
