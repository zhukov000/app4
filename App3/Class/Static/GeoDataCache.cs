using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3.Class
{
    static public class GeoDataCache
    {

        #region Справочник типов слоев

        public enum LayerType
        {
            REGION,
            POLYGON,
            ROAD,
            POINT,
            BUILD_BORDER,
            BUILD,
            HIGHWAY,
            PLACES,
            OBJECT,
            BIG_POLYGON,
            MID_POLYGON,
            SML_POLYGON
        }

        static public IDictionary<LayerType, string> tableName = new Dictionary<LayerType, string>()
        {
            {LayerType.REGION, "_regions"},
            {LayerType.POLYGON, "_polygons"},
            {LayerType.ROAD, "_roads"},
            {LayerType.POINT, "_points"},
            {LayerType.BUILD_BORDER, "_buildings_border"},
            {LayerType.BUILD, "_buildings"},
            {LayerType.HIGHWAY, "_highway"},
            {LayerType.PLACES, "_places"},
            {LayerType.OBJECT, "_objects"},
            {LayerType.BIG_POLYGON, "_bpolygons"},
            {LayerType.MID_POLYGON, "_mpolygons"},
            {LayerType.SML_POLYGON, "_spolygons"},
        };

        #endregion

        static public int BuildCacheForListened()
        {
            ClearCache();
            List<object[]> rows = Utils.GetListenIp();
            int id = OpenSession();
            foreach(object[] row in rows)
            {
                int num = row[1].ToInt();
                foreach (var pType in tableName.Keys)
                {
                    UpdateDataTable(id, pType/*, num*/, new object[] { });
                }
            }
            return id;
        }

        static private int OpenSession(string name = "")
        {
            object [] res = null;
            int id_session = 0;
            DataBase.RunCommandInsert("cache", new Dictionary<string, object> { { "name", name.Q() } }, "id_session", out res);
            if (res != null)
            {
                id_session = res[0].ToInt();
            }
            return id_session;
        }


        static public void ClearCache()
        {
            DataBase.RunCommand("DROP SCHEMA cache CASCADE;");
            DataBase.RunCommand("CREATE SCHEMA cache AUTHORIZATION postgres;");
        }

        static public void CloseSession(int pSessionId)
        {
            List<object[]> rows = Utils.GetListenIp();
            int id = OpenSession();
            foreach (object[] row in rows)
            {
                int num = row[1].ToInt();

                foreach (var pType in tableName.Keys)
                {
                    string req = "DROP TABLE IF EXISTS {0}";
                    if (pType == LayerType.OBJECT)
                    {
                        req = "DROP VIEW IF EXISTS {0}";
                    }
                    string sTableName = TableName(pSessionId, pType/*, num*/);
                    DataBase.RunCommand(string.Format(req, sTableName));
                }
            }
        }

        static public string TableName(int pSessionId, LayerType pType/*, int RegionId*/)
        {
            return "cache." + tableName[pType] + pSessionId /*+ "_" + RegionId*/;
        }

        static public string UpdateDataTable(int pSessionId, LayerType pType/*, int RegionId*/, object[] Params)
        {
            string sTableName = TableName(pSessionId, pType/*, RegionId*/);
            string reqCreate = "CREATE TABLE {0} AS ";
            string reqDrop = "DROP TABLE IF EXISTS {0}";

            switch (pType)
            {
                case LayerType.REGION:
                    if (Params[1] != null)
                        reqCreate += string.Format("(SELECT osm_id, way, name, num FROM regions WHERE fullname = '{0}');", Params[1]); // districtname
                    else
                        reqCreate += string.Format("(SELECT osm_id, way, name, num FROM regions);");
                    break;

                case LayerType.POLYGON:
                    reqCreate += string.Format("(SELECT osm_id, way, name, building, highway, place FROM oko.district_polygons({0}))", Params[0]); // osm_id
                    break;
                case LayerType.ROAD:
                    reqCreate += string.Format("(SELECT osm_id, way, name, building, highway, place FROM oko.district_roads({0}) UNION SELECT osm_id, way, name, building, highway, place FROM oko.district_lines({0}))", Params[0]); // osm_id
                    break;
                case LayerType.POINT:
                    reqCreate += string.Format("(SELECT osm_id, way, name, building, highway, place FROM oko.district_points({0}))", Params[0]); // osm_id
                    break;

                case LayerType.BIG_POLYGON:
                    reqCreate += string.Format("(SELECT osm_id, way, name, building, highway, place FROM oko.district_weigth_polygons({0}, 1000000, null) WHERE building is null)", Params[0]); 
                    break;
                case LayerType.MID_POLYGON:
                    reqCreate += string.Format("(SELECT osm_id, way, name, building, highway, place FROM oko.district_weigth_polygons({0}, 100000, null) WHERE building is null)", Params[0]);
                    break;
                case LayerType.SML_POLYGON:
                    reqCreate += string.Format("(SELECT osm_id, way, name, building, highway, place FROM oko.district_weigth_polygons({0}, 200, null) WHERE building is null)", Params[0]);
                    break;

                case LayerType.BUILD_BORDER:
                    // reqCreate += string.Format("(SELECT osm_id, way, name FROM {0} WHERE building is not null)", TableName(pSessionId, LayerType.ROAD));
                    // reqCreate += string.Format("(SELECT osm_id, way, name, building, highway, place FROM oko.district_weigth_polygons({0}, 0, 100000) WHERE building = 'yes')", Params[0]);
                    break;
                case LayerType.BUILD:
                    // reqCreate += string.Format("(SELECT osm_id, way, COALESCE(name, \"addr:housenumber\") as name FROM {0} WHERE building is not null)", TableName(pSessionId, LayerType.POLYGON));
                    reqCreate += string.Format("(SELECT osm_id, way, COALESCE(name, \"addr:housenumber\") as name FROM oko.district_weigth_polygons({0}, 0, 100000) WHERE building is not null)", Params[0]);
                    break;
                case LayerType.HIGHWAY:
                    reqCreate += string.Format("(SELECT osm_id, way, name FROM {0} WHERE highway is not null)", TableName(pSessionId, LayerType.ROAD/*, RegionId*/));
                    break;
                case LayerType.PLACES:
                    reqCreate += string.Format("(SELECT osm_id, way, name FROM {0} WHERE place is not null)", TableName(pSessionId, LayerType.POLYGON/*, RegionId*/));
                    break;

                case LayerType.OBJECT:
                    reqCreate = "CREATE VIEW {0} AS " + string.Format("(SELECT odo.way, odo.name as name, odo.number, os.* FROM oko.district_objects3({0}) as odo " +
                        "INNER JOIN oko.object_status as os on odo.osm_id = os.osm_id)", 
                        Params[0]); // osm_id
                    reqDrop = "DROP VIEW  IF EXISTS {0}";
                    break;
            }
            // удалить таблицу
            DataBase.RunCommand(string.Format(reqDrop, sTableName));
            // создать таблицу
            DataBase.RunCommand(string.Format(reqCreate, sTableName));

            return sTableName;
        }
    }
}
