using App3.Class.Singleton;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3.Class
{
    public static class Geocoder
    {
        #region Геокодинг
       

        public static OSMObjectInfo GetOSMObject(double[] coor, string table, string idfield = "osm_id")
        {
            DataRow row = null;
            string s = String.Format("SELECT {3}, name, dist, way FROM ( SELECT *, ST_Distance(way, ST_Transform(ST_GeomFromText('POINT({0} {1})', 4326), 900913)) as dist FROM {2} ) t ORDER BY dist", C2S(coor[0]), C2S(coor[1]), table, idfield);
            try
            {
                row = DataBase.FirstRow(s);
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                Logger.Instance.WriteToLog(string.Format("Не удалось получить информацию об объекте: {0} - {1} ", s, ex.Message));
            }
            if (row != null && double.Parse(row["dist"].ToString().Replace('.', ',')) < 10 )
            {
                return new OSMObjectInfo(Convert.ToInt64(row[idfield]), row["name"].ToString(), row["way"]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// По названию региона возвращает его OSM_ID
        /// </summary>
        /// <param name="RegionName"></param>
        /// <returns></returns>
        public static Int64 GeocodeRegion(string RegionName)
        {
            Int64 rID = 0;
            object o = DataBase.First(String.Format("select * from kladr_osm_regions where fullname = '{0}'", RegionName), "osm_id");
            if (o != null)
            {
                rID = Convert.ToInt64(o);
            }
            return rID;
        }

        /// <summary>
        /// По названию района или городского округа возвращает его OSM_ID
        /// </summary>
        /// <param name="DistrictName"></param>
        /// <returns></returns>
        public static Int64 GeocodeDistirct(string DistrictName)
        {
            Int64 rID = 0;
            object o = DataBase.First(String.Format("select * from kladr_osm_districts where fullname = '{0}'", DistrictName), "osm_id");
            if (o != null)
            {
                rID = o.ToInt64();
            }
            return rID;
        }

        /// <summary>
        /// По названию района или городского округа возвращает его номер
        /// </summary>
        /// <param name="DistrictName"></param>
        /// <returns></returns>
        public static int GeocodeDistirctNum(string DistrictName)
        {
            int rID = 0;
            object o = DataBase.First(String.Format("select num from regions2map where fullname = '{0}'", DistrictName), "num");
            if (o != null)
            {
                rID = o.ToInt();
            }
            return rID;
        }

        /// <summary>
        /// По названию населенного пункта возвращает его OSM_ID
        /// </summary>
        /// <param name="LocalityName"></param>
        /// <returns></returns>
        public static Int64 GeocodeLocality(string LocalityName)
        {
            Int64 rID = 0;
            object o = DataBase.First(String.Format("select * from kladr_osm_locality where fullname = '{0}'", LocalityName), "osm_id");
            if (o != null)
            {
                rID = Convert.ToInt64(o);
            }
            return rID;
        }

        /// <summary>
        /// Прямой геокодинг: по адресу возвращает координаты объекта
        /// </summary>
        /// <param name="Addr"></param>
        /// <returns></returns>
        public static double[] Geocode(Address Addr)
        {
            double[] res = new double[2];
            // 
            DataRow row = DataBase.FirstRow(
                String.Format("SELECT lat, lon FROM oko.osm_geocode('{0}', '{1}', '{2}', '{3}', '{4}') ORDER BY lat, lon", 
                Addr.Region, 
                Addr.District, 
                Addr.Locality, 
                Addr.Street, 
                Addr.House)
            );
            if (row != null)
            {
                res[0] = double.Parse(row["lat"].ToString().Replace('.',','));
                res[1] = double.Parse(row["lon"].ToString().Replace('.', ','));
            }
            return res;
        }
        #endregion

        #region Реверс-геокодинг

        /// <summary>
        /// Перевод координат в строку
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string C2S(this double x)
        {
            return x.ToString("0.00000000000000", System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Реверс-геокодинг: по паре координат возвращает список ближайших адресов
        /// </summary>
        /// <param name="Coor"></param>
        /// <returns></returns>
        public static Address RGeocode(double[] Coor)
        {
            Address iAddresses = new Address();
            DataRow row = null;
            try
            {
                row = DataBase.FirstRow(
                    string.Format(
                        "select region, district, locality, street, house, min(distance) as dist " +
                        "from oko._osm_rgeocode({0}, {1}) " +
                        "group by region, district, locality, street, house " +
                        "order by region, district, locality, street, house, min(distance)",
                        C2S(Coor[0]), C2S(Coor[1])
                    )
                );
            }
            catch (Exception ex)
            { 
                Logger.Instance.WriteToLog(string.Format("Geocoder.Address: Не удалось декодировать координаты: {0} ", ex.Message));
            }
            if (row != null)
            {
                iAddresses = new Address(
                    row["Region"].ToString(),
                    row["District"].ToString(),
                    row["Locality"].ToString(),
                    row["Street"].ToString(),
                    row["House"].ToString()
                );
            }
            return iAddresses;
        }

        /// <summary>
        /// Возвращает список подходящих адресов
        /// </summary>
        /// <param name="Coor"></param>
        /// <returns></returns>
        public static List<Address> RGeocode2(double[] Coor)
        {
            List<Address> ListAddresses = new List<Address>();
            DataSet ds = new DataSet();
            DataBase.RowSelect(
                string.Format(
                    "select region, district, locality, street, house, min(distance) as dist " +
                    "from oko._osm_rgeocode({0}, {1}) " +
                    "group by region, district, locality, street, house " +
                    "order by region, district, locality, street, house, min(distance)",
                    C2S(Coor[0]), C2S(Coor[1])
                ), ds
            );
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow r in ds.Tables[0].Rows)
                {
                    ListAddresses.Add(
                        new Address(
                            r["region"].ToString(),
                            r["district"].ToString(),
                            r["locality"].ToString(),
                            r["street"].ToString(),
                            r["house"].ToString()
                        )
                    );
                }
            }
            return ListAddresses;
        }
        #endregion

        /// <summary>
        /// Переводит координаты в формате UTM к формату долгота/широта
        /// </summary>
        /// <param name="UTMCoor"></param>
        /// <returns></returns>
        public static double[] UTM2LL(double[] UTMCoor)
        {
            double[] r = new double[2] { UTMCoor[0], UTMCoor[1] };
            DataRow row = DataBase.FirstRow(
                string.Format(
                    "SELECT ST_AsText(ST_Transform(ST_GeomFromText('POINT({0} {1})', 900913), 4326)) as txt",
                    C2S(UTMCoor[0]), C2S(UTMCoor[1])
                )
            );
            if (row != null)
            {
                string s = row["txt"].ToString();
                r = ExtractFromString(s);
            }
            return r;
        }

        /// <summary>
        /// Извлечение координат точки из строкового представления
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static double[] ExtractFromString(string point)
        {
            double[] res = new double[] { 0, 0 };
            try
            {
                res = point.Substring(6).Trim('(', ')', ' ').Split(' ').Select(o => double.Parse(o, CultureInfo.InvariantCulture)).ToArray();
            }
            catch (Exception ex) 
            {
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return res;
        }

        /// <summary>
        /// Переводит координаты в формате долгота/широта к формату UTM
        /// </summary>
        /// <param name="LLCoor"></param>
        /// <returns></returns>
        public static double[] LL2UTM(double[] LLCoor)
        {
            double[] r = new double[2] { LLCoor[0], LLCoor[1] };
            DataRow row = DataBase.FirstRow(
                string.Format(
                    "SELECT ST_AsText(ST_Transform(ST_GeomFromText('POINT({0} {1})', 4326), 900913)) as txt",
                    C2S(LLCoor[0]), C2S(LLCoor[1])
                )
            );
            if (row != null)
            {
                string s = row["txt"].ToString();
                r = s.Substring(6).Trim('(', ')', ' ').Split(' ').Select(o => double.Parse(o, CultureInfo.InvariantCulture)).ToArray();
            }
            return r;
        }

    }
}
