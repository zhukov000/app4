using SharpMap.Layers;
using System;
using System.Collections.Generic;

namespace App3.Class.Map
{
	internal class LayerCache
	{
		private static IDictionary<string, IDictionary<int, Layer>> mapLayers = new Dictionary<string, IDictionary<int, Layer>>();

		public static void UpdateLayer(string Key, int pRegionId = 0)
		{
			if (pRegionId >= 0)
			{
				LayerCache.Get(Key, pRegionId).UpdateTable();
				return;
			}
            if (LayerCache.mapLayers.ContainsKey(Key))
            {
                foreach (KeyValuePair<int, Layer> current in LayerCache.mapLayers[Key])
                {
                    current.Value.UpdateTable();
                }
            }
		}

		public static void UpdateLayer(Layer pLayer)
		{
			if (pLayer != null)
			{
				pLayer.UpdateTable();
			}
		}

		public static void UpdateLayers(int pRegionId)
		{
			foreach (KeyValuePair<string, IDictionary<int, Layer>> current in LayerCache.mapLayers)
			{
				if (current.Value.ContainsKey(pRegionId))
				{
					LayerCache.UpdateLayer(current.Value[pRegionId]);
				}
				else if (current.Value.ContainsKey(0))
				{
					LayerCache.UpdateLayer(current.Value[0]);
				}
			}
		}

		public static void CreateAllLayers()
		{
			foreach (KeyValuePair<string, IDictionary<int, Layer>> current in LayerCache.mapLayers)
			{
				foreach (KeyValuePair<int, Layer> current2 in current.Value)
				{
					current2.Value.CreateTable();
				}
			}
		}

		public static Layer Get(string Key, int pRegionId)
		{
			if (!LayerCache.mapLayers.ContainsKey(Key))
			{
				LayerCache.mapLayers.Add(Key, new Dictionary<int, Layer>());
			}
			if (!LayerCache.mapLayers[Key].ContainsKey(pRegionId))
			{
				Layer layer = LayerCache.Create(Key, pRegionId, null);
				LayerCache.AddLayer(Key, pRegionId, layer);
				LayerCache.UpdateLayer(layer);
			}
			return LayerCache.mapLayers[Key][pRegionId];
		}

		public static string GetName(string Key, int RegionId)
		{
			return LayerCache.Get(Key, RegionId).Name;
		}

		public static VectorLayer GetVLayer(string Key, int RegionId)
		{
			return LayerCache.Get(Key, RegionId).VLayer(Key);
		}

		public static LabelLayer GetLLayer(string Key, int RegionId, string Column = "name")
		{
			return LayerCache.Get(Key, RegionId).LLayer(null, Column);
		}

		public static void CreateLayer(Layer pLayer, int pRegionId)
		{
			if (pLayer.RegionId != pRegionId)
			{
				pLayer.RegionId = pRegionId;
			}
			pLayer.CreateTable();
		}

		public static void CreateLayers(int pRegionId)
		{
			foreach (KeyValuePair<string, IDictionary<int, Layer>> current in LayerCache.mapLayers)
			{
				LayerCache.CreateLayer(current.Value[pRegionId], pRegionId);
			}
		}

		private static void AddLayer(string Name, int RegionId, Layer Lay)
		{
			if (!LayerCache.mapLayers.ContainsKey(Name))
			{
				LayerCache.mapLayers.Add(Name, new Dictionary<int, Layer>());
			}
			if (!LayerCache.mapLayers[Name].ContainsKey(RegionId))
			{
				LayerCache.mapLayers[Name].Add(RegionId, Lay);
			}
		}

		public static Layer Create(string Type, int RegionId, Layer SubLayer = null)
		{
			Layer layer = (Type == LayerType.AllRegion) ? new Region() : ((Type == LayerType.Region) ? new Region(RegionId) : null);
			layer = ((Type == LayerType.Polygon) ? new Polygon(((Region)SubLayer) ?? new Region(RegionId)) : layer);
			layer = ((Type == LayerType.Road) ? new Road(((Region)SubLayer) ?? new Region(RegionId)) : layer);
			layer = ((Type == LayerType.BigPoly) ? new BigPoly(((Region)SubLayer) ?? new Region(RegionId)) : layer);
			layer = ((Type == LayerType.MidPoly) ? new MidPoly(((Region)SubLayer) ?? new Region(RegionId)) : layer);
			layer = ((Type == LayerType.SmlPoly) ? new SmlPoly(((Region)SubLayer) ?? new Region(RegionId)) : layer);
			Layer arg_145_0;
			if (!(Type == LayerType.HighWay))
			{
				arg_145_0 = layer;
			}
			else
			{
				Road arg_140_0;
				if ((arg_140_0 = (Road)SubLayer) == null)
				{
					arg_140_0 = new Road(((Region)SubLayer) ?? new Region(RegionId));
				}
				arg_145_0 = new HighWay(arg_140_0);
			}
			layer = arg_145_0;
			layer = ((Type == LayerType.Build) ? new Build(((Region)SubLayer) ?? new Region(RegionId)) : layer);
			layer = ((Type == LayerType.Object) ? new Object(((Region)SubLayer) ?? new Region(RegionId)) : layer);
			layer = ((Type == LayerType.Point) ? new Point(((Region)SubLayer) ?? new Region(RegionId)) : layer);
			layer = ((Type == LayerType.BigPoly) ? new BigPoly(((Region)SubLayer) ?? new Region(RegionId)) : layer);
			layer = ((Type == LayerType.MidPoly) ? new MidPoly(((Region)SubLayer) ?? new Region(RegionId)) : layer);
			layer = ((Type == LayerType.SmlPoly) ? new SmlPoly(((Region)SubLayer) ?? new Region(RegionId)) : layer);
			layer = ((Type == LayerType.Build) ? new Build(((Region)SubLayer) ?? new Region(RegionId)) : layer);
			return (Type == LayerType.Place) ? new Place(((Polygon)SubLayer) ?? new Polygon(RegionId)) : layer;
		}

		public static void Init(int pRegionId = 0)
		{
			Layer lay = LayerCache.Create(LayerType.AllRegion, 0, null);
			LayerCache.AddLayer(LayerType.AllRegion, 0, lay);
			if (pRegionId > 0)
			{
				Layer layer = LayerCache.Create(LayerType.Region, pRegionId, null);
				LayerCache.AddLayer(layer.Name, pRegionId, layer);
				Layer layer2 = LayerCache.Create(LayerType.Polygon, pRegionId, layer);
				LayerCache.AddLayer(layer2.Name, pRegionId, layer2);
				Layer layer3 = LayerCache.Create(LayerType.Road, pRegionId, layer);
				LayerCache.AddLayer(layer3.Name, pRegionId, layer3);
				Layer layer4 = LayerCache.Create(LayerType.Point, pRegionId, layer);
				LayerCache.AddLayer(layer4.Name, pRegionId, layer4);
				layer4 = LayerCache.Create(LayerType.BigPoly, pRegionId, layer);
				LayerCache.AddLayer(layer4.Name, pRegionId, layer4);
				layer4 = LayerCache.Create(LayerType.MidPoly, pRegionId, layer);
				LayerCache.AddLayer(layer4.Name, pRegionId, layer4);
				layer4 = LayerCache.Create(LayerType.SmlPoly, pRegionId, layer);
				LayerCache.AddLayer(layer4.Name, pRegionId, layer4);
				layer4 = LayerCache.Create(LayerType.Build, pRegionId, layer);
				LayerCache.AddLayer(layer4.Name, pRegionId, layer4);
				layer4 = LayerCache.Create(LayerType.HighWay, pRegionId, layer3);
				LayerCache.AddLayer(layer4.Name, pRegionId, layer4);
				layer4 = LayerCache.Create(LayerType.Object, pRegionId, layer);
				LayerCache.AddLayer(layer4.Name, pRegionId, layer4);
				layer4 = LayerCache.Create(LayerType.Place, pRegionId, layer2);
				LayerCache.AddLayer(layer4.Name, pRegionId, layer4);
			}
		}
	}
}
