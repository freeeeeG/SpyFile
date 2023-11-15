using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020008CC RID: 2252
[AddComponentMenu("KMonoBehaviour/scripts/OrbitalObject")]
[SerializationConfig(MemberSerialization.OptIn)]
public class OrbitalObject : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x06004130 RID: 16688 RVA: 0x0016CFC0 File Offset: 0x0016B1C0
	public void Init(string orbit_data_name, WorldContainer orbiting_world, List<Ref<OrbitalObject>> orbiting_obj)
	{
		OrbitalData orbitalData = Db.Get().OrbitalTypeCategories.Get(orbit_data_name);
		if (orbiting_world != null)
		{
			this.orbitingWorldId = orbiting_world.id;
			this.world = orbiting_world;
			this.worldOrbitingOrigin = this.GetWorldOrigin(this.world, orbitalData);
		}
		else
		{
			this.worldOrbitingOrigin = new Vector3((float)Grid.WidthInCells * 0.5f, (float)Grid.HeightInCells * orbitalData.yGridPercent, 0f);
		}
		this.animFilename = orbitalData.animFile;
		this.initialAnim = this.GetInitialAnim(orbitalData);
		this.angle = this.GetAngle(orbitalData);
		this.timeoffset = this.GetTimeOffset(orbiting_obj);
		this.orbitalDBId = orbitalData.Id;
	}

	// Token: 0x06004131 RID: 16689 RVA: 0x0016D078 File Offset: 0x0016B278
	protected override void OnSpawn()
	{
		this.world = ClusterManager.Instance.GetWorld(this.orbitingWorldId);
		this.orbitData = Db.Get().OrbitalTypeCategories.Get(this.orbitalDBId);
		base.gameObject.SetActive(false);
		KBatchedAnimController kbatchedAnimController = base.gameObject.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = this.initialAnim;
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim(this.animFilename)
		};
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.Always;
		this.animController = kbatchedAnimController;
	}

	// Token: 0x06004132 RID: 16690 RVA: 0x0016D118 File Offset: 0x0016B318
	public void RenderEveryTick(float dt)
	{
		float time = GameClock.Instance.GetTime();
		bool flag;
		Vector3 vector = this.CalculateWorldPos(time, out flag);
		Vector3 vector2 = vector;
		if (this.orbitData.periodInCycles > 0f)
		{
			vector2.x = vector.x / (float)Grid.WidthInCells;
			vector2.y = vector.y / (float)Grid.HeightInCells;
			vector2.x = Camera.main.ViewportToWorldPoint(vector2).x;
			vector2.y = Camera.main.ViewportToWorldPoint(vector2).y;
		}
		bool flag2 = (!this.orbitData.rotatesBehind || !flag) && (this.world == null || ClusterManager.Instance.activeWorldId == this.world.id);
		Vector3 offset = vector2 - base.gameObject.transform.position;
		offset.z = 0f;
		this.animController.Offset = offset;
		Vector3 position = vector2;
		position.x = this.worldOrbitingOrigin.x;
		position.y = this.worldOrbitingOrigin.y;
		base.gameObject.transform.SetPosition(position);
		if (this.orbitData.periodInCycles > 0f)
		{
			base.gameObject.transform.localScale = Vector3.one * (CameraController.Instance.baseCamera.orthographicSize / this.orbitData.distance);
		}
		else
		{
			base.gameObject.transform.localScale = Vector3.one * this.orbitData.distance;
		}
		if (base.gameObject.activeSelf != flag2)
		{
			base.gameObject.SetActive(flag2);
		}
	}

	// Token: 0x06004133 RID: 16691 RVA: 0x0016D2D8 File Offset: 0x0016B4D8
	private Vector3 CalculateWorldPos(float time, out bool behind)
	{
		Vector3 result;
		if (this.orbitData.periodInCycles > 0f)
		{
			float num = this.orbitData.periodInCycles * 600f;
			float f = ((time + (float)this.timeoffset) / num - (float)((int)((time + (float)this.timeoffset) / num))) * 2f * 3.1415927f;
			float d = 0.5f * this.orbitData.radiusScale * (float)this.world.WorldSize.x;
			Vector3 vector = new Vector3(Mathf.Cos(f), 0f, Mathf.Sin(f));
			behind = (vector.z > this.orbitData.behindZ);
			Vector3 b = Quaternion.Euler(this.angle, 0f, 0f) * (vector * d);
			result = this.worldOrbitingOrigin + b;
			result.z = this.orbitData.renderZ;
		}
		else
		{
			behind = false;
			result = this.worldOrbitingOrigin;
			result.z = this.orbitData.renderZ;
		}
		return result;
	}

	// Token: 0x06004134 RID: 16692 RVA: 0x0016D3E8 File Offset: 0x0016B5E8
	private string GetInitialAnim(OrbitalData data)
	{
		if (data.initialAnim.IsNullOrWhiteSpace())
		{
			KAnimFileData data2 = Assets.GetAnim(data.animFile).GetData();
			int index = new KRandom().Next(0, data2.animCount - 1);
			return data2.GetAnim(index).name;
		}
		return data.initialAnim;
	}

	// Token: 0x06004135 RID: 16693 RVA: 0x0016D440 File Offset: 0x0016B640
	private Vector3 GetWorldOrigin(WorldContainer wc, OrbitalData data)
	{
		if (wc != null)
		{
			float x = (float)wc.WorldOffset.x + (float)wc.WorldSize.x * data.xGridPercent;
			float y = (float)wc.WorldOffset.y + (float)wc.WorldSize.y * data.yGridPercent;
			return new Vector3(x, y, 0f);
		}
		return new Vector3((float)Grid.WidthInCells * data.xGridPercent, (float)Grid.HeightInCells * data.yGridPercent, 0f);
	}

	// Token: 0x06004136 RID: 16694 RVA: 0x0016D4C7 File Offset: 0x0016B6C7
	private float GetAngle(OrbitalData data)
	{
		return UnityEngine.Random.Range(data.minAngle, data.maxAngle);
	}

	// Token: 0x06004137 RID: 16695 RVA: 0x0016D4DC File Offset: 0x0016B6DC
	private int GetTimeOffset(List<Ref<OrbitalObject>> orbiting_obj)
	{
		List<int> list = new List<int>();
		foreach (Ref<OrbitalObject> @ref in orbiting_obj)
		{
			if (@ref.Get().world == this.world)
			{
				list.Add(@ref.Get().timeoffset);
			}
		}
		int num = UnityEngine.Random.Range(0, 600);
		while (list.Contains(num))
		{
			num = UnityEngine.Random.Range(0, 600);
		}
		return num;
	}

	// Token: 0x04002A6D RID: 10861
	private WorldContainer world;

	// Token: 0x04002A6E RID: 10862
	private OrbitalData orbitData;

	// Token: 0x04002A6F RID: 10863
	private KBatchedAnimController animController;

	// Token: 0x04002A70 RID: 10864
	[Serialize]
	private string animFilename;

	// Token: 0x04002A71 RID: 10865
	[Serialize]
	private string initialAnim;

	// Token: 0x04002A72 RID: 10866
	[Serialize]
	private Vector3 worldOrbitingOrigin;

	// Token: 0x04002A73 RID: 10867
	[Serialize]
	private int orbitingWorldId;

	// Token: 0x04002A74 RID: 10868
	[Serialize]
	private float angle;

	// Token: 0x04002A75 RID: 10869
	[Serialize]
	public int timeoffset;

	// Token: 0x04002A76 RID: 10870
	[Serialize]
	public string orbitalDBId;
}
