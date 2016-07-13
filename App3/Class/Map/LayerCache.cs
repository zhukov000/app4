using App3.Class.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App3.Class.Map
{
    class LayerCache
    {
        // список слоев, данные по которым кэшируются
        // список слоев - только расширяется
        static private IDictionary<string, IDictionary<int, Layer>> mapLayers = new Dictionary<string, IDictionary<int, Layer>>();

        static public void UpdateLayer(String Key, int pRegionId = 0)
        {
            Layer pLayer = Get(Key, pRegionId);
            pLayer.UpdateTable();
        }

        static public void UpdateLayer(Layer pLayer)
        {
            pLayer.UpdateTable();
        }

        static public void UpdateLayers(int pRegionId)
        {
            foreach (KeyValuePair<string, IDictionary<int, Layer>> l in mapLayers)
            {
                if (l.Value.ContainsKey(pRegionId))
                {
                    UpdateLayer(l.Value[pRegionId]);
                } else if (l.Value.ContainsKey(0))
                {
                    UpdateLayer(l.Value[0]);
                }
            }
        }

        public static Layer Get(string Key, int pRegionId)
        {
            if (mapLayers.ContainsKey(Key))
                if (mapLayers[Key].ContainsKey(pRegionId))
                {
                    return mapLayers[Key][pRegionId];
                }
            throw new Exception("Ошибка обращения к справочнику слоев по ключу " + Key + " для региона " + pRegionId);
        }

        public static String GetName(string Key, int RegionId)
        {
            return Get(Key, RegionId).Name;
        }

        public static SharpMap.Layers.VectorLayer GetVLayer(string Key, int RegionId = 0)
        {
            Layer l = Get(Key, RegionId);
            return l.VLayer(Key);
        }

        public static SharpMap.Layers.LabelLayer GetLLayer(string Key, int RegionId = 0, string Column = "name")
        {
            return Get(Key, RegionId).LLayer(null, Column);
        }

        static public void CreateLayer(Layer pLayer, int pRegionId)
        {
            if (pLayer.RegionId != pRegionId)
            {
                pLayer.RegionId = pRegionId;
            }
            pLayer.CreateTable();
        }

        static public void CreateLayers(int pRegionId)
        {
            foreach (KeyValuePair<string, IDictionary<int, Layer>> l in mapLayers)
            {
                CreateLayer(l.Value[pRegionId], pRegionId);
            }
        }

        static private void AddLayer(string Name, int RegionId, Layer Lay)
        {
            if (!mapLayers.ContainsKey(Name))
            {
                mapLayers.Add(Name, new Dictionary<int, Layer>());
            }
            if (!mapLayers[Name].ContainsKey(RegionId))
            {
                mapLayers[Name].Add(RegionId, Lay);
            }
        }

        static public void Init(int pRegionId = 0)
        {
            Region rall = new Region();
            AddLayer("regions", 0, rall);
            if (pRegionId > 0)
            {
                Region r = new Region(pRegionId);
                AddLayer(r.Name, pRegionId, r); // mapLayers.Add(r.Name, r);
                /*Polygon pl = new Polygon(r);
                mapLayers.Add(pl.Name, pl);
                Road rd = new Road(r);
                mapLayers.Add(rd.Name, rd);
                Layer tmp = new Point(r);
                mapLayers.Add(tmp.Name, tmp);
                tmp = new BigPoly(r);
                mapLayers.Add(tmp.Name, tmp);
                tmp = new MidPoly(r);
                mapLayers.Add(tmp.Name, tmp);
                tmp = new SmlPoly(r);
                mapLayers.Add(tmp.Name, tmp);
                tmp = new Build(r);
                mapLayers.Add(tmp.Name, tmp);
                tmp = new HighWay(rd);
                mapLayers.Add(tmp.Name, tmp);
                tmp = new Place(pl);
                mapLayers.Add(tmp.Name, tmp);
                tmp = new Object(r);
                mapLayers.Add(tmp.Name, tmp);*/
            }
        }

    }
}
