using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BAF RID: 2991
public abstract class MapNode : MonoBehaviour
{
	// Token: 0x17000422 RID: 1058
	// (get) Token: 0x06003D22 RID: 15650 RVA: 0x00123C2F File Offset: 0x0012202F
	public bool Unfolding
	{
		get
		{
			return this.m_unfolding;
		}
	}

	// Token: 0x17000423 RID: 1059
	// (get) Token: 0x06003D23 RID: 15651 RVA: 0x00123C37 File Offset: 0x00122037
	public bool Unfolded
	{
		get
		{
			return this.m_unfolded;
		}
	}

	// Token: 0x06003D24 RID: 15652 RVA: 0x00123C3F File Offset: 0x0012203F
	public void RegisterFlipCallback(VoidGeneric<FlipDirection, FlipType> _callback)
	{
		this.m_flipCallback = (VoidGeneric<FlipDirection, FlipType>)Delegate.Combine(this.m_flipCallback, _callback);
	}

	// Token: 0x06003D25 RID: 15653 RVA: 0x00123C58 File Offset: 0x00122058
	public void UnregisterFlipCallback(VoidGeneric<FlipDirection, FlipType> _callback)
	{
		this.m_flipCallback = (VoidGeneric<FlipDirection, FlipType>)Delegate.Remove(this.m_flipCallback, _callback);
	}

	// Token: 0x06003D26 RID: 15654 RVA: 0x00123C71 File Offset: 0x00122071
	public void RegisterPreFlipCallback(VoidGeneric<FlipDirection, FlipType> _callback)
	{
		this.m_preFlipCallback = (VoidGeneric<FlipDirection, FlipType>)Delegate.Combine(this.m_preFlipCallback, _callback);
	}

	// Token: 0x06003D27 RID: 15655 RVA: 0x00123C8A File Offset: 0x0012208A
	public void UnregisterPreFlipCallback(VoidGeneric<FlipDirection, FlipType> _callback)
	{
		this.m_preFlipCallback = (VoidGeneric<FlipDirection, FlipType>)Delegate.Remove(this.m_preFlipCallback, _callback);
	}

	// Token: 0x06003D28 RID: 15656 RVA: 0x00123CA3 File Offset: 0x001220A3
	public void RegisterPostFlipCallback(VoidGeneric<FlipDirection, FlipType> _callback)
	{
		this.m_postFlipCallback = (VoidGeneric<FlipDirection, FlipType>)Delegate.Combine(this.m_postFlipCallback, _callback);
	}

	// Token: 0x06003D29 RID: 15657 RVA: 0x00123CBC File Offset: 0x001220BC
	public void UnregisterPostFlipCallback(VoidGeneric<FlipDirection, FlipType> _callback)
	{
		this.m_postFlipCallback = (VoidGeneric<FlipDirection, FlipType>)Delegate.Remove(this.m_postFlipCallback, _callback);
	}

	// Token: 0x06003D2A RID: 15658 RVA: 0x00123CD8 File Offset: 0x001220D8
	protected virtual void OnEnable()
	{
		for (int i = 0; i < this.m_visibilityMesh.Length; i++)
		{
			this.m_visibilityMesh[i].gameObject.SetActive(true);
		}
	}

	// Token: 0x06003D2B RID: 15659 RVA: 0x00123D14 File Offset: 0x00122114
	protected virtual void OnDisable()
	{
		for (int i = 0; i < this.m_visibilityMesh.Length; i++)
		{
			this.m_visibilityMesh[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x06003D2C RID: 15660 RVA: 0x00123D4D File Offset: 0x0012214D
	protected virtual void Update()
	{
	}

	// Token: 0x06003D2D RID: 15661 RVA: 0x00123D50 File Offset: 0x00122150
	public void AddToFlipSet(int _flipSet, WorldMapFlipperBase _flipTile)
	{
		if (_flipSet < this.m_orderedUnfoldingTiles.Length)
		{
			ArrayUtils.PushBack<WorldMapFlipperBase>(ref this.m_orderedUnfoldingTiles[_flipSet].Tiles, _flipTile);
		}
		else if (_flipSet >= this.m_orderedUnfoldingTiles.Length)
		{
			ArrayUtils.PushBack<MapNode.FlipSet>(ref this.m_orderedUnfoldingTiles, new MapNode.FlipSet());
			this.AddToFlipSet(_flipSet, _flipTile);
		}
	}

	// Token: 0x06003D2E RID: 15662 RVA: 0x00123DAC File Offset: 0x001221AC
	public IEnumerator UnfoldFlow()
	{
		this.m_unfolding = true;
		this.m_preFlipCallback(FlipDirection.Unfold, FlipType.Normal);
		bool flippedTile = false;
		bool flippedRamp = false;
		for (int i = 0; i < this.m_orderedUnfoldingTiles.Length; i++)
		{
			for (int l = 0; l < this.m_orderedUnfoldingTiles[i].Tiles.Length; l++)
			{
				WorldMapFlipperBase worldMapFlipperBase = this.m_orderedUnfoldingTiles[i].Tiles[l];
				if (!flippedTile && worldMapFlipperBase is WorldMapTileFlip)
				{
					flippedTile = true;
					GameUtils.TriggerAudio(GameOneShotAudioTag.WorldMapTiles, base.gameObject.layer);
				}
				if (!flippedRamp && worldMapFlipperBase is WorldMapRampFlip)
				{
					flippedRamp = true;
					GameUtils.TriggerAudio(GameOneShotAudioTag.WorldMapRamp, base.gameObject.layer);
				}
				worldMapFlipperBase.StartUnfoldFlow();
			}
			IEnumerator wait = CoroutineUtils.TimerRoutine(this.m_timeBetweenFlipLayers, base.gameObject.layer);
			while (wait.MoveNext())
			{
				yield return null;
			}
		}
		this.m_flipCallback(FlipDirection.Unfold, FlipType.Normal);
		for (int j = 0; j < this.m_orderedUnfoldingTiles.Length; j++)
		{
			for (int k = 0; k < this.m_orderedUnfoldingTiles[j].Tiles.Length; k++)
			{
				WorldMapFlipperBase flipperBase = this.m_orderedUnfoldingTiles[j].Tiles[k];
				while (!flipperBase.IsFinishedFlipping())
				{
					yield return null;
				}
			}
		}
		this.m_unfolded = true;
		this.m_unfolding = false;
		this.m_postFlipCallback(FlipDirection.Unfold, FlipType.Normal);
		yield break;
	}

	// Token: 0x06003D2F RID: 15663 RVA: 0x00123DC8 File Offset: 0x001221C8
	public List<ClientWorldMapInfoPopup.InfoPopupShowRequest> GetPopups()
	{
		List<ClientWorldMapInfoPopup.InfoPopupShowRequest> list = new List<ClientWorldMapInfoPopup.InfoPopupShowRequest>();
		for (int i = 0; i < this.m_orderedUnfoldingTiles.Length; i++)
		{
			for (int j = 0; j < this.m_orderedUnfoldingTiles[i].Tiles.Length; j++)
			{
				list.AddRange(this.m_orderedUnfoldingTiles[i].Tiles[j].GetPopups());
			}
		}
		return list;
	}

	// Token: 0x06003D30 RID: 15664 RVA: 0x00123E30 File Offset: 0x00122230
	public void InstantUnfold()
	{
		this.m_unfolded = true;
		this.m_preFlipCallback(FlipDirection.Unfold, FlipType.Instantaneous);
		for (int i = 0; i < this.m_orderedUnfoldingTiles.Length; i++)
		{
			for (int j = 0; j < this.m_orderedUnfoldingTiles[i].Tiles.Length; j++)
			{
				WorldMapFlipperBase worldMapFlipperBase = this.m_orderedUnfoldingTiles[i].Tiles[j];
				worldMapFlipperBase.StartInstantUnfold();
			}
		}
		this.m_flipCallback(FlipDirection.Unfold, FlipType.Instantaneous);
		this.m_postFlipCallback(FlipDirection.Unfold, FlipType.Instantaneous);
	}

	// Token: 0x06003D31 RID: 15665 RVA: 0x00123EBC File Offset: 0x001222BC
	public bool IsIdle()
	{
		if (this.m_unfolding)
		{
			return false;
		}
		for (int i = 0; i < this.m_orderedUnfoldingTiles.Length; i++)
		{
			for (int j = 0; j < this.m_orderedUnfoldingTiles[i].Tiles.Length; j++)
			{
				WorldMapFlipperBase worldMapFlipperBase = this.m_orderedUnfoldingTiles[i].Tiles[j];
				if (!worldMapFlipperBase.IsFinishedFlipping())
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06003D32 RID: 15666 RVA: 0x00123F2D File Offset: 0x0012232D
	public bool IsStatic()
	{
		return this.m_static;
	}

	// Token: 0x06003D33 RID: 15667 RVA: 0x00123F38 File Offset: 0x00122338
	public void SetAsStatic()
	{
		for (int i = 0; i < this.m_orderedUnfoldingTiles.Length; i++)
		{
			MapNode.FlipSet flipSet = this.m_orderedUnfoldingTiles[i];
			for (int j = 0; j < flipSet.Tiles.Length; j++)
			{
				WorldMapFlipperBase worldMapFlipperBase = flipSet.Tiles[j];
				ITileFlipStaticHandler tileFlipStaticHandler = worldMapFlipperBase.gameObject.RequestInterfaceRecursive<ITileFlipStaticHandler>();
				if (tileFlipStaticHandler != null)
				{
					tileFlipStaticHandler.SetAsStatic();
				}
			}
		}
		this.m_static = true;
	}

	// Token: 0x06003D34 RID: 15668 RVA: 0x00123FAC File Offset: 0x001223AC
	public void StartUp()
	{
		for (int i = 0; i < this.m_orderedUnfoldingTiles.Length; i++)
		{
			for (int j = 0; j < this.m_orderedUnfoldingTiles[i].Tiles.Length; j++)
			{
				ITileFlipStartup tileFlipStartup = this.m_orderedUnfoldingTiles[i].Tiles[j].gameObject.RequestInterface<ITileFlipStartup>();
				if (tileFlipStartup != null)
				{
					tileFlipStartup.StartUp();
				}
			}
		}
	}

	// Token: 0x0400312F RID: 12591
	[SerializeField]
	private float m_timeBetweenFlipLayers = 0.5f;

	// Token: 0x04003130 RID: 12592
	[SerializeField]
	private MeshRenderer[] m_visibilityMesh = new MeshRenderer[0];

	// Token: 0x04003131 RID: 12593
	private MapNode.FlipSet[] m_orderedUnfoldingTiles = new MapNode.FlipSet[0];

	// Token: 0x04003132 RID: 12594
	private bool m_unfolding;

	// Token: 0x04003133 RID: 12595
	private bool m_unfolded;

	// Token: 0x04003134 RID: 12596
	private bool m_static;

	// Token: 0x04003135 RID: 12597
	private VoidGeneric<FlipDirection, FlipType> m_flipCallback = delegate(FlipDirection _direction, FlipType _type)
	{
	};

	// Token: 0x04003136 RID: 12598
	private VoidGeneric<FlipDirection, FlipType> m_preFlipCallback = delegate(FlipDirection _direction, FlipType _type)
	{
	};

	// Token: 0x04003137 RID: 12599
	private VoidGeneric<FlipDirection, FlipType> m_postFlipCallback = delegate(FlipDirection _direction, FlipType _type)
	{
	};

	// Token: 0x02000BB0 RID: 2992
	[Serializable]
	public class FlipSet
	{
		// Token: 0x0400313B RID: 12603
		public WorldMapFlipperBase[] Tiles = new WorldMapFlipperBase[0];
	}
}
