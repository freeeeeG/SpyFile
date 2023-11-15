using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000005 RID: 5
public class fxController : MonoBehaviour
{
	// Token: 0x0600000B RID: 11 RVA: 0x00002194 File Offset: 0x00000394
	private void Start()
	{
		for (int i = 0; i < 4; i++)
		{
			this.fxObject[i].SetActive(false);
		}
		this.SetlevelFXTxt(this.levelFX, this.FX);
	}

	// Token: 0x0600000C RID: 12 RVA: 0x000021D0 File Offset: 0x000003D0
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			this.levelFX++;
			this.levelFX = Mathf.Clamp(this.levelFX, 0, 3);
			this.DisabledFX();
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			this.levelFX--;
			this.levelFX = Mathf.Clamp(this.levelFX, 0, 3);
			this.DisabledFX();
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			this.FX--;
			this.FX = Mathf.Clamp(this.FX, 0, 7);
			this.DisabledFX();
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			this.FX++;
			this.FX = Mathf.Clamp(this.FX, 0, 7);
			this.DisabledFX();
		}
		if (Input.GetKeyDown(KeyCode.KeypadMinus))
		{
			this.cameraZoom = Mathf.Clamp(this.cameraZoom + 1, 0, 2);
		}
		if (Input.GetKeyDown(KeyCode.KeypadPlus))
		{
			this.cameraZoom = Mathf.Clamp(this.cameraZoom - 1, 0, 2);
		}
		this.myCamera.fieldOfView = Mathf.Lerp(this.myCamera.fieldOfView, this.cameraFOV[this.cameraZoom], 0.2f);
		switch (this.FX)
		{
		case 0:
			if (this.levelFX == 0)
			{
				this.fxObject[0].SetActive(true);
				this.SetNameFXTxt(0);
			}
			if (this.levelFX == 1)
			{
				this.fxObject[1].SetActive(true);
				this.SetNameFXTxt(1);
			}
			if (this.levelFX == 2)
			{
				this.fxObject[2].SetActive(true);
				this.SetNameFXTxt(2);
			}
			if (this.levelFX == 3)
			{
				this.fxObject[3].SetActive(true);
				this.SetNameFXTxt(3);
				return;
			}
			break;
		case 1:
			if (this.levelFX == 0)
			{
				this.fxObject[4].SetActive(true);
				this.SetNameFXTxt(4);
			}
			if (this.levelFX == 1)
			{
				this.fxObject[5].SetActive(true);
				this.SetNameFXTxt(5);
			}
			if (this.levelFX == 2)
			{
				this.fxObject[6].SetActive(true);
				this.SetNameFXTxt(6);
			}
			if (this.levelFX == 3)
			{
				this.fxObject[7].SetActive(true);
				this.SetNameFXTxt(7);
				return;
			}
			break;
		case 2:
			if (this.levelFX == 0)
			{
				this.fxObject[8].SetActive(true);
				this.SetNameFXTxt(8);
			}
			if (this.levelFX == 1)
			{
				this.fxObject[9].SetActive(true);
				this.SetNameFXTxt(9);
			}
			if (this.levelFX == 2)
			{
				this.fxObject[10].SetActive(true);
				this.SetNameFXTxt(10);
			}
			if (this.levelFX == 3)
			{
				this.fxObject[11].SetActive(true);
				this.SetNameFXTxt(11);
				return;
			}
			break;
		case 3:
			if (this.levelFX == 0)
			{
				this.fxObject[12].SetActive(true);
				this.SetNameFXTxt(12);
			}
			if (this.levelFX == 1)
			{
				this.fxObject[13].SetActive(true);
				this.SetNameFXTxt(13);
			}
			if (this.levelFX == 2)
			{
				this.fxObject[14].SetActive(true);
				this.SetNameFXTxt(14);
			}
			if (this.levelFX == 3)
			{
				this.fxObject[15].SetActive(true);
				this.SetNameFXTxt(15);
				return;
			}
			break;
		case 4:
			if (this.levelFX == 0)
			{
				this.fxObject[16].SetActive(true);
				this.SetNameFXTxt(16);
			}
			if (this.levelFX == 1)
			{
				this.fxObject[17].SetActive(true);
				this.SetNameFXTxt(17);
			}
			if (this.levelFX == 2)
			{
				this.fxObject[18].SetActive(true);
				this.SetNameFXTxt(18);
			}
			if (this.levelFX == 3)
			{
				this.fxObject[19].SetActive(true);
				this.SetNameFXTxt(19);
				return;
			}
			break;
		case 5:
			if (this.levelFX == 0)
			{
				this.fxObject[20].SetActive(true);
				this.SetNameFXTxt(20);
			}
			if (this.levelFX == 1)
			{
				this.fxObject[21].SetActive(true);
				this.SetNameFXTxt(21);
			}
			if (this.levelFX == 2)
			{
				this.fxObject[22].SetActive(true);
				this.SetNameFXTxt(22);
			}
			if (this.levelFX == 3)
			{
				this.fxObject[23].SetActive(true);
				this.SetNameFXTxt(23);
				return;
			}
			break;
		case 6:
			if (this.levelFX == 0)
			{
				this.fxObject[24].SetActive(true);
				this.SetNameFXTxt(24);
			}
			if (this.levelFX == 1)
			{
				this.fxObject[25].SetActive(true);
				this.SetNameFXTxt(25);
			}
			if (this.levelFX == 2)
			{
				this.fxObject[26].SetActive(true);
				this.SetNameFXTxt(26);
			}
			if (this.levelFX == 3)
			{
				this.fxObject[27].SetActive(true);
				this.SetNameFXTxt(27);
				return;
			}
			break;
		case 7:
			if (this.levelFX == 0)
			{
				this.fxObject[28].SetActive(true);
				this.SetNameFXTxt(28);
			}
			if (this.levelFX == 1)
			{
				this.fxObject[29].SetActive(true);
				this.SetNameFXTxt(29);
			}
			if (this.levelFX == 2)
			{
				this.fxObject[30].SetActive(true);
				this.SetNameFXTxt(30);
			}
			if (this.levelFX == 3)
			{
				this.fxObject[31].SetActive(true);
				this.SetNameFXTxt(31);
				return;
			}
			break;
		case 8:
			if (this.levelFX == 0)
			{
				this.fxObject[32].SetActive(true);
				this.SetNameFXTxt(32);
			}
			if (this.levelFX == 1)
			{
				this.fxObject[33].SetActive(true);
				this.SetNameFXTxt(33);
			}
			if (this.levelFX == 2)
			{
				this.fxObject[34].SetActive(true);
				this.SetNameFXTxt(34);
			}
			if (this.levelFX == 3)
			{
				this.fxObject[35].SetActive(true);
				this.SetNameFXTxt(35);
				return;
			}
			break;
		case 9:
			if (this.levelFX == 0)
			{
				this.fxObject[36].SetActive(true);
				this.SetNameFXTxt(36);
			}
			if (this.levelFX == 1)
			{
				this.fxObject[37].SetActive(true);
				this.SetNameFXTxt(37);
			}
			if (this.levelFX == 2)
			{
				this.fxObject[38].SetActive(true);
				this.SetNameFXTxt(38);
			}
			if (this.levelFX == 3)
			{
				this.fxObject[39].SetActive(true);
				this.SetNameFXTxt(39);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00002858 File Offset: 0x00000A58
	private void DisabledFX()
	{
		for (int i = 0; i < 32; i++)
		{
			this.fxObject[i].SetActive(false);
		}
		this.SetlevelFXTxt(this.levelFX, this.FX);
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002894 File Offset: 0x00000A94
	private void SetlevelFXTxt(int i, int j)
	{
		switch (i)
		{
		case 0:
			this.levelFXTxt.text = "Level : Light";
			break;
		case 1:
			this.levelFXTxt.text = "Level : Medium";
			break;
		case 2:
			this.levelFXTxt.text = "Level : Heavy";
			break;
		case 3:
			this.levelFXTxt.text = "Level : VeryHeavy";
			break;
		}
		i++;
		j++;
		this.numberFXTxt.text = "No.  : " + j.ToString() + " / 8";
	}

	// Token: 0x0600000F RID: 15 RVA: 0x0000292A File Offset: 0x00000B2A
	private void SetNameFXTxt(int i)
	{
		this.nameFXTxt.text = "Name  : " + this.fxObject[i].gameObject.name;
	}

	// Token: 0x04000007 RID: 7
	public int levelFX;

	// Token: 0x04000008 RID: 8
	public int FX;

	// Token: 0x04000009 RID: 9
	public GameObject[] fxObject;

	// Token: 0x0400000A RID: 10
	public Text levelFXTxt;

	// Token: 0x0400000B RID: 11
	public Text numberFXTxt;

	// Token: 0x0400000C RID: 12
	public Text nameFXTxt;

	// Token: 0x0400000D RID: 13
	public Camera myCamera;

	// Token: 0x0400000E RID: 14
	private int cameraZoom = 2;

	// Token: 0x0400000F RID: 15
	private float[] cameraFOV = new float[]
	{
		40f,
		50f,
		60f
	};
}
