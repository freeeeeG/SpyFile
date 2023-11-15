using System;
using UnityEngine;

// Token: 0x0200001C RID: 28
public class LeanAudioStream
{
	// Token: 0x06000215 RID: 533 RVA: 0x0000DFA6 File Offset: 0x0000C1A6
	public LeanAudioStream(float[] audioArr)
	{
		this.audioArr = audioArr;
	}

	// Token: 0x06000216 RID: 534 RVA: 0x0000DFB8 File Offset: 0x0000C1B8
	public void OnAudioRead(float[] data)
	{
		for (int i = 0; i < data.Length; i++)
		{
			data[i] = this.audioArr[this.position];
			this.position++;
		}
	}

	// Token: 0x06000217 RID: 535 RVA: 0x0000DFF1 File Offset: 0x0000C1F1
	public void OnAudioSetPosition(int newPosition)
	{
		this.position = newPosition;
	}

	// Token: 0x040000F0 RID: 240
	public int position;

	// Token: 0x040000F1 RID: 241
	public AudioClip audioClip;

	// Token: 0x040000F2 RID: 242
	public float[] audioArr;
}
