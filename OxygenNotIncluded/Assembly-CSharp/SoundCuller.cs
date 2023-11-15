using System;
using UnityEngine;

// Token: 0x02000477 RID: 1143
public struct SoundCuller
{
	// Token: 0x060018FD RID: 6397 RVA: 0x0008287C File Offset: 0x00080A7C
	public static bool IsAudibleWorld(Vector2 pos)
	{
		bool result = false;
		int num = Grid.PosToCell(pos);
		if (Grid.IsValidCell(num) && (int)Grid.WorldIdx[num] == ClusterManager.Instance.activeWorldId)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x060018FE RID: 6398 RVA: 0x000828B0 File Offset: 0x00080AB0
	public bool IsAudible(Vector2 pos)
	{
		return SoundCuller.IsAudibleWorld(pos) && this.min.LessEqual(pos) && pos.LessEqual(this.max);
	}

	// Token: 0x060018FF RID: 6399 RVA: 0x000828D8 File Offset: 0x00080AD8
	public bool IsAudibleNoCameraScaling(Vector2 pos, float falloff_distance_sq)
	{
		return (pos.x - this.cameraPos.x) * (pos.x - this.cameraPos.x) + (pos.y - this.cameraPos.y) * (pos.y - this.cameraPos.y) < falloff_distance_sq;
	}

	// Token: 0x06001900 RID: 6400 RVA: 0x00082933 File Offset: 0x00080B33
	public bool IsAudible(Vector2 pos, float falloff_distance_sq)
	{
		if (!SoundCuller.IsAudibleWorld(pos))
		{
			return false;
		}
		pos = this.GetVerticallyScaledPosition(pos, false);
		return this.IsAudibleNoCameraScaling(pos, falloff_distance_sq);
	}

	// Token: 0x06001901 RID: 6401 RVA: 0x0008295B File Offset: 0x00080B5B
	public bool IsAudible(Vector2 pos, HashedString sound_path)
	{
		return sound_path.IsValid && this.IsAudible(pos, KFMOD.GetSoundEventDescription(sound_path).falloffDistanceSq);
	}

	// Token: 0x06001902 RID: 6402 RVA: 0x0008297C File Offset: 0x00080B7C
	public Vector3 GetVerticallyScaledPosition(Vector3 pos, bool objectIsSelectedAndVisible = false)
	{
		float num = 1f;
		float num2;
		if (pos.y > this.max.y)
		{
			num2 = Mathf.Abs(pos.y - this.max.y);
		}
		else if (pos.y < this.min.y)
		{
			num2 = Mathf.Abs(pos.y - this.min.y);
			num = -1f;
		}
		else
		{
			num2 = 0f;
		}
		float extraYRange = TuningData<SoundCuller.Tuning>.Get().extraYRange;
		num2 = ((num2 < extraYRange) ? num2 : extraYRange);
		float num3 = num2 * num2 / (4f * this.zoomScaler);
		num3 *= num;
		Vector3 result = new Vector3(pos.x, pos.y + num3, 0f);
		if (objectIsSelectedAndVisible)
		{
			result.z = pos.z;
		}
		return result;
	}

	// Token: 0x06001903 RID: 6403 RVA: 0x00082A50 File Offset: 0x00080C50
	public static SoundCuller CreateCuller()
	{
		SoundCuller result = default(SoundCuller);
		Camera main = Camera.main;
		Vector3 vector = main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 vector2 = main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		result.min = new Vector3(vector2.x, vector2.y, 0f);
		result.max = new Vector3(vector.x, vector.y, 0f);
		result.cameraPos = main.transform.GetPosition();
		Audio audio = Audio.Get();
		float num = CameraController.Instance.OrthographicSize / (audio.listenerReferenceZ - audio.listenerMinZ);
		if (num <= 0f)
		{
			num = 2f;
		}
		else
		{
			num = 1f;
		}
		result.zoomScaler = num;
		return result;
	}

	// Token: 0x04000DCE RID: 3534
	private Vector2 min;

	// Token: 0x04000DCF RID: 3535
	private Vector2 max;

	// Token: 0x04000DD0 RID: 3536
	private Vector2 cameraPos;

	// Token: 0x04000DD1 RID: 3537
	private float zoomScaler;

	// Token: 0x020010F0 RID: 4336
	public class Tuning : TuningData<SoundCuller.Tuning>
	{
		// Token: 0x04005AC1 RID: 23233
		public float extraYRange;
	}
}
