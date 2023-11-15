﻿using System;
using System.Collections.Generic;
using FMOD.Studio;
using KSerialization;
using UnityEngine;

// Token: 0x02000A12 RID: 2578
[AddComponentMenu("KMonoBehaviour/scripts/UserNavigation")]
public class UserNavigation : KMonoBehaviour
{
	// Token: 0x06004D35 RID: 19765 RVA: 0x001B1454 File Offset: 0x001AF654
	public UserNavigation()
	{
		for (global::Action action = global::Action.SetUserNav1; action <= global::Action.SetUserNav10; action++)
		{
			this.hotkeyNavPoints.Add(UserNavigation.NavPoint.Invalid);
		}
	}

	// Token: 0x06004D36 RID: 19766 RVA: 0x001B149B File Offset: 0x001AF69B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.Subscribe(1983128072, delegate(object worlds)
		{
			global::Tuple<int, int> tuple = (global::Tuple<int, int>)worlds;
			int first = tuple.first;
			int second = tuple.second;
			int num = Grid.PosToCell(CameraController.Instance.transform.position);
			if (!Grid.IsValidCell(num) || (int)Grid.WorldIdx[num] != second)
			{
				WorldContainer world = ClusterManager.Instance.GetWorld(second);
				float x = Mathf.Clamp(CameraController.Instance.transform.position.x, world.minimumBounds.x, world.maximumBounds.x);
				float y = Mathf.Clamp(CameraController.Instance.transform.position.y, world.minimumBounds.y, world.maximumBounds.y);
				Vector3 position = new Vector3(x, y, CameraController.Instance.transform.position.z);
				CameraController.Instance.SetPosition(position);
			}
			this.worldCameraPositions[second] = new UserNavigation.NavPoint
			{
				pos = CameraController.Instance.transform.position,
				orthoSize = CameraController.Instance.targetOrthographicSize
			};
			if (!this.worldCameraPositions.ContainsKey(first))
			{
				WorldContainer world2 = ClusterManager.Instance.GetWorld(first);
				Vector2I vector2I = world2.WorldOffset + new Vector2I(world2.Width / 2, world2.Height / 2);
				this.worldCameraPositions.Add(first, new UserNavigation.NavPoint
				{
					pos = new Vector3((float)vector2I.x, (float)vector2I.y),
					orthoSize = CameraController.Instance.targetOrthographicSize
				});
			}
			CameraController.Instance.SetTargetPosForWorldChange(this.worldCameraPositions[first].pos, this.worldCameraPositions[first].orthoSize, false);
		});
	}

	// Token: 0x06004D37 RID: 19767 RVA: 0x001B14C0 File Offset: 0x001AF6C0
	public void SetWorldCameraStartPosition(int world_id, Vector3 start_pos)
	{
		if (!this.worldCameraPositions.ContainsKey(world_id))
		{
			this.worldCameraPositions.Add(world_id, new UserNavigation.NavPoint
			{
				pos = new Vector3(start_pos.x, start_pos.y),
				orthoSize = CameraController.Instance.targetOrthographicSize
			});
			return;
		}
		this.worldCameraPositions[world_id] = new UserNavigation.NavPoint
		{
			pos = new Vector3(start_pos.x, start_pos.y),
			orthoSize = CameraController.Instance.targetOrthographicSize
		};
	}

	// Token: 0x06004D38 RID: 19768 RVA: 0x001B1558 File Offset: 0x001AF758
	private static int GetIndex(global::Action action)
	{
		int result = -1;
		if (global::Action.SetUserNav1 <= action && action <= global::Action.SetUserNav10)
		{
			result = action - global::Action.SetUserNav1;
		}
		else if (global::Action.GotoUserNav1 <= action && action <= global::Action.GotoUserNav10)
		{
			result = action - global::Action.GotoUserNav1;
		}
		return result;
	}

	// Token: 0x06004D39 RID: 19769 RVA: 0x001B1588 File Offset: 0x001AF788
	private void SetHotkeyNavPoint(global::Action action, Vector3 pos, float ortho_size)
	{
		int index = UserNavigation.GetIndex(action);
		if (index < 0)
		{
			return;
		}
		this.hotkeyNavPoints[index] = new UserNavigation.NavPoint
		{
			pos = pos,
			orthoSize = ortho_size
		};
		EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("UserNavPoint_set", false), Vector3.zero, 1f);
		instance.setParameterByName("userNavPoint_ID", (float)index, false);
		KFMOD.EndOneShot(instance);
	}

	// Token: 0x06004D3A RID: 19770 RVA: 0x001B15F8 File Offset: 0x001AF7F8
	private void GoToHotkeyNavPoint(global::Action action)
	{
		int index = UserNavigation.GetIndex(action);
		if (index < 0)
		{
			return;
		}
		UserNavigation.NavPoint navPoint = this.hotkeyNavPoints[index];
		if (navPoint.IsValid())
		{
			CameraController.Instance.SetTargetPos(navPoint.pos, navPoint.orthoSize, true);
			EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("UserNavPoint_recall", false), Vector3.zero, 1f);
			instance.setParameterByName("userNavPoint_ID", (float)index, false);
			KFMOD.EndOneShot(instance);
		}
	}

	// Token: 0x06004D3B RID: 19771 RVA: 0x001B1670 File Offset: 0x001AF870
	public bool Handle(KButtonEvent e)
	{
		bool flag = false;
		for (global::Action action = global::Action.GotoUserNav1; action <= global::Action.GotoUserNav10; action++)
		{
			if (e.TryConsume(action))
			{
				this.GoToHotkeyNavPoint(action);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			for (global::Action action2 = global::Action.SetUserNav1; action2 <= global::Action.SetUserNav10; action2++)
			{
				if (e.TryConsume(action2))
				{
					Camera baseCamera = CameraController.Instance.baseCamera;
					Vector3 position = baseCamera.transform.GetPosition();
					this.SetHotkeyNavPoint(action2, position, baseCamera.orthographicSize);
					flag = true;
					break;
				}
			}
		}
		return flag;
	}

	// Token: 0x0400325C RID: 12892
	[Serialize]
	private List<UserNavigation.NavPoint> hotkeyNavPoints = new List<UserNavigation.NavPoint>();

	// Token: 0x0400325D RID: 12893
	[Serialize]
	private Dictionary<int, UserNavigation.NavPoint> worldCameraPositions = new Dictionary<int, UserNavigation.NavPoint>();

	// Token: 0x020018A1 RID: 6305
	[Serializable]
	private struct NavPoint
	{
		// Token: 0x06009242 RID: 37442 RVA: 0x0032B917 File Offset: 0x00329B17
		public bool IsValid()
		{
			return this.orthoSize != 0f;
		}

		// Token: 0x04007282 RID: 29314
		public Vector3 pos;

		// Token: 0x04007283 RID: 29315
		public float orthoSize;

		// Token: 0x04007284 RID: 29316
		public static readonly UserNavigation.NavPoint Invalid = new UserNavigation.NavPoint
		{
			pos = Vector3.zero,
			orthoSize = 0f
		};
	}
}
