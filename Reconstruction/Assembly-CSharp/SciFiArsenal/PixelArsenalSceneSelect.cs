using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SciFiArsenal
{
	// Token: 0x020002AA RID: 682
	public class PixelArsenalSceneSelect : MonoBehaviour
	{
		// Token: 0x060010C9 RID: 4297 RVA: 0x0002EDFD File Offset: 0x0002CFFD
		public void LoadSceneDemo1()
		{
			SceneManager.LoadScene("demo_missiles");
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x0002EE09 File Offset: 0x0002D009
		public void LoadSceneDemo2()
		{
			SceneManager.LoadScene("demo_beams");
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x0002EE15 File Offset: 0x0002D015
		public void LoadSceneDemo3()
		{
			SceneManager.LoadScene("3");
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x0002EE21 File Offset: 0x0002D021
		public void LoadSceneDemo4()
		{
			SceneManager.LoadScene("4");
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x0002EE2D File Offset: 0x0002D02D
		public void LoadSceneDemo5()
		{
			SceneManager.LoadScene("5");
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x0002EE39 File Offset: 0x0002D039
		public void LoadSceneDemo6()
		{
			SceneManager.LoadScene("6");
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x0002EE45 File Offset: 0x0002D045
		public void LoadSceneDemo7()
		{
			SceneManager.LoadScene("7");
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x0002EE51 File Offset: 0x0002D051
		public void LoadSceneDemo8()
		{
			SceneManager.LoadScene("8");
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x0002EE5D File Offset: 0x0002D05D
		public void LoadSceneDemo9()
		{
			SceneManager.LoadScene("9");
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x0002EE69 File Offset: 0x0002D069
		public void LoadSceneDemo10()
		{
			SceneManager.LoadScene("10");
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x0002EE75 File Offset: 0x0002D075
		public void LoadSceneDemo11()
		{
			SceneManager.LoadScene("11");
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x0002EE81 File Offset: 0x0002D081
		public void LoadSceneDemo12()
		{
			SceneManager.LoadScene("12");
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x0002EE8D File Offset: 0x0002D08D
		public void LoadSceneDemo13()
		{
			SceneManager.LoadScene("13");
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x0002EE99 File Offset: 0x0002D099
		public void LoadSceneDemo14()
		{
			SceneManager.LoadScene("14");
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x0002EEA5 File Offset: 0x0002D0A5
		public void LoadSceneDemo15()
		{
			SceneManager.LoadScene("15");
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x0002EEB4 File Offset: 0x0002D0B4
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.J))
			{
				this.GUIHide = !this.GUIHide;
				if (this.GUIHide)
				{
					GameObject.Find("CanvasSceneSelect2").GetComponent<Canvas>().enabled = false;
				}
				else
				{
					GameObject.Find("CanvasSceneSelect2").GetComponent<Canvas>().enabled = true;
				}
			}
			if (Input.GetKeyDown(KeyCode.K))
			{
				this.GUIHide2 = !this.GUIHide2;
				if (this.GUIHide2)
				{
					GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;
				}
				else
				{
					GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
				}
			}
			if (Input.GetKeyDown(KeyCode.L))
			{
				this.GUIHide3 = !this.GUIHide3;
				if (this.GUIHide3)
				{
					GameObject.Find("CanvasTips").GetComponent<Canvas>().enabled = false;
					return;
				}
				GameObject.Find("CanvasTips").GetComponent<Canvas>().enabled = true;
			}
		}

		// Token: 0x040008FA RID: 2298
		public bool GUIHide;

		// Token: 0x040008FB RID: 2299
		public bool GUIHide2;

		// Token: 0x040008FC RID: 2300
		public bool GUIHide3;
	}
}
