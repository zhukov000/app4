using App3.Class.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App3.Class.Map
{
    public class LayerType
    {
        private LayerType(string value) { Value = value; }

        public string Value { get; set; }

        public static LayerType BigPoly { get { return new LayerType("bigpoly"); } }
        public static LayerType Build { get { return new LayerType("build"); } }
        public static LayerType BuildBorder { get { return new LayerType("buildborder"); } }
        public static LayerType HighWay { get { return new LayerType("highway"); } }
        public static LayerType MidPoly { get { return new LayerType("midpoly"); } }
        public static LayerType Object { get { return new LayerType("object"); } }
        public static LayerType Place { get { return new LayerType("place"); } }
        public static LayerType Point { get { return new LayerType("point"); } }
        public static LayerType Polygon { get { return new LayerType("polygon"); } }
        public static LayerType Region { get { return new LayerType("region"); } }
        public static LayerType Road { get { return new LayerType("road"); } }
        public static LayerType SmlPoly { get { return new LayerType("smlpoly"); } }
        public static LayerType AllRegion { get { return new LayerType("regions"); } }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string (LayerType t)
        {
            return t.ToString();
        }
    }

    class LayerCache
    {
        
        // список слоев, данные по которым кэшируются
        // список слоев - только расширяется
        static private IDictionary<string, IDictionary<int, Layer>> mapLayers = new Dictionary<string, IDictionary<int, Layer>>();

        static public void UpdateLayer(String Key, int pRegionId = 0)
        {
            if (pRegionId >= 0)
            {
                Layer pLayer = Get(Key, pRegionId);
                pLayer.UpdateTable();
            } else
            {
                foreach (KeyValuePair<int, Layer> p in mapLayers[Key])
                {
                    p.Value.UpdateTable(); 
                }
            }
        }

        static public void UpdateLayer(Layer pLayer)
        {
            if (pLayer != null) pLayer.UpdateTable();
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

        static public void CreateAllLayers()
        {
            foreach (KeyValuePair<string, IDictionary<int, Layer>> l in mapLayers)
            {
                foreach (KeyValuePair<int, Layer> p in l.Value)
                {
                    p.Value.CreateTable();
                }
            }
        }

        public static Layer Get(string Key, int pRegionId)
        {
            if (!mapLayers.ContainsKey(Key))
            {
                mapLayers.Add(Key, new Dictionary<int, Layer>());
            }
            if (!mapLayers[Key].ContainsKey(pRegionId))
            {
                Layer l = Create(Key, pRegionId);
                AddLayer(Key, pRegionId, l);
                UpdateLayer(l);
            }
            // throw new Exception("Ошибка обращения к справочнику слоев по ключу " + Key + " для региона " + pRegionId);
            return mapLayers[Key][pRegionId];
        }

        public static String GetName(string Key, int RegionId)
        {
            return Get(Key, RegionId).Name;
        }

        public static SharpMap.Layers.VectorLayer GetVLayer(string Key, int RegionId)
        {
            Layer l = Get(Key, RegionId);
            return l.VLayer(Key);
        }

        public static SharpMap.Layers.LabelLayer GetLLayer(string Key, int RegionId, string Column = "name")
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

        /// <summary>
        /// Фабрика объектов
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="RegionId"></param>
        /// <returns></returns>
        static public Layer Create(string Type, int RegionId, Layer SubLayer = null)
        {
            Layer l = Type == LayerType.AllRegion ? new Region() :
                Type == LayerType.Region ? new Region(RegionId) :
                null;
            l = Type == LayerType.Polygon ? new Polygon((Region)SubLayer ?? new Region(RegionId)) : l;
            l = Type == LayerType.Road ? new Road((Region)SubLayer ?? new Region(RegionId)) : l;
            l = Type == LayerType.BigPoly ? new BigPoly((Region)SubLayer ?? new Region(RegionId)) : l;
            l = Type == LayerType.MidPoly ? new MidPoly((Region)SubLayer ?? new Region(RegionId)) : l;
            l = Type == LayerType.SmlPoly ? new SmlPoly((Region)SubLayer ?? new Region(RegionId)) : l;
            l = Type == LayerType.HighWay ? new HighWay((Road)SubLayer ?? new Road((Region)SubLayer ?? new Region(RegionId))) : l;
            l = Type == LayerType.Build ? new Build((Region)SubLayer ?? new Region(RegionId)) : l;
            l = Type == LayerType.Object ? new Object((Region)SubLayer ?? new Region(RegionId)) : l;
            l = Type == LayerType.Point ? new Point((Region)SubLayer ?? new Region(RegionId)) : l;
            l = Type == LayerType.BigPoly ? new BigPoly((Region)SubLayer ?? new Region(RegionId)) : l;
            l = Type == LayerType.MidPoly ? new MidPoly((Region)SubLayer ?? new Region(RegionId)) : l;
            l = Type == LayerType.SmlPoly ? new SmlPoly((Region)SubLayer ?? new Region(RegionId)) : l;
            l = Type == LayerType.Build ? new Build((Region)SubLayer ?? new Region(RegionId)) : l;
            l = Type == LayerType.Place ? new Place((Polygon)SubLayer ?? new Polygon(RegionId)) : l;
            return l;
        }

        static public void Init(int pRegionId = 0)
        {
            Layer rall = Create(LayerType.AllRegion, 0);
            AddLayer(LayerType.AllRegion, 0, rall);
            if (pRegionId > 0)
            {
                Layer r = Create(LayerType.Region, pRegionId); 
                AddLayer(r.Name, pRegionId, r);
                Layer pl = Create(LayerType.Polygon, pRegionId, r);
                AddLayer(pl.Name, pRegionId, pl);
                Layer rd = Create(LayerType.Road, pRegionId, r);
                AddLayer(rd.Name, pRegionId, rd);
                Layer tmp = Create(LayerType.Point, pRegionId, r);
                AddLayer(tmp.Name, pRegionId, tmp);
                tmp = Create(LayerType.BigPoly, pRegionId, r);
                AddLayer(tmp.Name, pRegionId, tmp);
                tmp = Create(LayerType.MidPoly, pRegionId, r);
                AddLayer(tmp.Name, pRegionId, tmp);
                tmp = Create(LayerType.SmlPoly, pRegionId, r);
                AddLayer(tmp.Name, pRegionId, tmp);
                tmp = Create(LayerType.Build, pRegionId, r);
                AddLayer(tmp.Name, pRegionId, tmp);
                tmp = Create(LayerType.HighWay, pRegionId, rd);
                AddLayer(tmp.Name, pRegionId, tmp);
                tmp = Create(LayerType.Object, pRegionId, r);
                AddLayer(tmp.Name, pRegionId, tmp);
                tmp = Create(LayerType.Place, pRegionId, pl);
                AddLayer(tmp.Name, pRegionId, tmp);
            }
        }

    }
}
