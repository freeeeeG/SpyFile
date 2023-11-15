using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005A1 RID: 1441
[AddComponentMenu("KMonoBehaviour/scripts/BubbleManager")]
public class BubbleManager : KMonoBehaviour, ISim33ms, IRenderEveryTick
{
	// Token: 0x06002325 RID: 8997 RVA: 0x000C07B5 File Offset: 0x000BE9B5
	public static void DestroyInstance()
	{
		BubbleManager.instance = null;
	}

	// Token: 0x06002326 RID: 8998 RVA: 0x000C07BD File Offset: 0x000BE9BD
	protected override void OnPrefabInit()
	{
		BubbleManager.instance = this;
	}

	// Token: 0x06002327 RID: 8999 RVA: 0x000C07C8 File Offset: 0x000BE9C8
	public void SpawnBubble(Vector2 position, Vector2 velocity, SimHashes element, float mass, float temperature)
	{
		BubbleManager.Bubble item = new BubbleManager.Bubble
		{
			position = position,
			velocity = velocity,
			element = element,
			temperature = temperature,
			mass = mass
		};
		this.bubbles.Add(item);
	}

	// Token: 0x06002328 RID: 9000 RVA: 0x000C0818 File Offset: 0x000BEA18
	public void Sim33ms(float dt)
	{
		ListPool<BubbleManager.Bubble, BubbleManager>.PooledList pooledList = ListPool<BubbleManager.Bubble, BubbleManager>.Allocate();
		ListPool<BubbleManager.Bubble, BubbleManager>.PooledList pooledList2 = ListPool<BubbleManager.Bubble, BubbleManager>.Allocate();
		foreach (BubbleManager.Bubble bubble in this.bubbles)
		{
			bubble.position += bubble.velocity * dt;
			bubble.elapsedTime += dt;
			int num = Grid.PosToCell(bubble.position);
			if (!Grid.IsVisiblyInLiquid(bubble.position) || Grid.Element[num].id == bubble.element)
			{
				pooledList2.Add(bubble);
			}
			else
			{
				pooledList.Add(bubble);
			}
		}
		foreach (BubbleManager.Bubble bubble2 in pooledList2)
		{
			SimMessages.AddRemoveSubstance(Grid.PosToCell(bubble2.position), bubble2.element, CellEventLogger.Instance.FallingWaterAddToSim, bubble2.mass, bubble2.temperature, byte.MaxValue, 0, true, -1);
		}
		this.bubbles.Clear();
		this.bubbles.AddRange(pooledList);
		pooledList2.Recycle();
		pooledList.Recycle();
	}

	// Token: 0x06002329 RID: 9001 RVA: 0x000C0970 File Offset: 0x000BEB70
	public void RenderEveryTick(float dt)
	{
		ListPool<SpriteSheetAnimator.AnimInfo, BubbleManager>.PooledList pooledList = ListPool<SpriteSheetAnimator.AnimInfo, BubbleManager>.Allocate();
		SpriteSheetAnimator spriteSheetAnimator = SpriteSheetAnimManager.instance.GetSpriteSheetAnimator("liquid_splash1");
		foreach (BubbleManager.Bubble bubble in this.bubbles)
		{
			SpriteSheetAnimator.AnimInfo item = new SpriteSheetAnimator.AnimInfo
			{
				frame = spriteSheetAnimator.GetFrameFromElapsedTimeLooping(bubble.elapsedTime),
				elapsedTime = bubble.elapsedTime,
				pos = new Vector3(bubble.position.x, bubble.position.y, 0f),
				rotation = Quaternion.identity,
				size = Vector2.one,
				colour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
			};
			pooledList.Add(item);
		}
		pooledList.Recycle();
	}

	// Token: 0x04001415 RID: 5141
	public static BubbleManager instance;

	// Token: 0x04001416 RID: 5142
	private List<BubbleManager.Bubble> bubbles = new List<BubbleManager.Bubble>();

	// Token: 0x02001230 RID: 4656
	private struct Bubble
	{
		// Token: 0x04005EB8 RID: 24248
		public Vector2 position;

		// Token: 0x04005EB9 RID: 24249
		public Vector2 velocity;

		// Token: 0x04005EBA RID: 24250
		public float elapsedTime;

		// Token: 0x04005EBB RID: 24251
		public int frame;

		// Token: 0x04005EBC RID: 24252
		public SimHashes element;

		// Token: 0x04005EBD RID: 24253
		public float temperature;

		// Token: 0x04005EBE RID: 24254
		public float mass;
	}
}
