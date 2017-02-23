using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLoader
{
    class YandexObject : IGeoObject
    {
        private string cacheArea = "";
        private string cacheHouse = "";
        private string cacheLocality = "";
        private string cacheStreet = "";


        public YandexResponse response;

        public string addressString()
        {
            if (response.geoObjectCollection.featureMember.Count() > 0)
            {
                return response.geoObjectCollection.featureMember[0].geoObject.metaDataProperty.geocoderMetaData.Address.formatted;
            }
            else
            {
                return "";
            }
        }

        public string district()
        {
            if (cacheArea != "") return cacheArea; 
            if (response.geoObjectCollection.featureMember.Count() > 0)
            {
                foreach(var component in response.geoObjectCollection.featureMember[0].geoObject.metaDataProperty.geocoderMetaData.Address.Components)
                {
                    if (component.kind == "area")
                    {
                        cacheArea = component.name.Replace("городской округ ", "город");
                        break;
                    }
                }
            }
            return cacheArea;
        }

        public string house()
        {
            if (cacheHouse != "") return cacheHouse;
            if (response.geoObjectCollection.featureMember.Count() > 0)
            {
                foreach (var component in response.geoObjectCollection.featureMember[0].geoObject.metaDataProperty.geocoderMetaData.Address.Components)
                {
                    if (component.kind == "house")
                    {
                        cacheHouse = component.name;
                        break;
                    }
                }
            }
            return cacheHouse;
        }

        public string locality()
        {
            if (cacheLocality != "") return cacheLocality;
            if (response.geoObjectCollection.featureMember.Count() > 0)
            {
                foreach (var component in response.geoObjectCollection.featureMember[0].geoObject.metaDataProperty.geocoderMetaData.Address.Components)
                {
                    if (component.kind == "locality")
                    {
                        cacheLocality = component.name;
                        break;
                    }
                }
            }
            return cacheLocality;
        }

        public string latitude()
        {
            if (founded())
            {
                return response.geoObjectCollection.featureMember[0].geoObject.point.lat;
            }
            return "";
        }

        public string longitude()
        {
            if (founded())
            {
                return response.geoObjectCollection.featureMember[0].geoObject.point.lon;
            }
            return "";
        }

        public string region()
        {
            return "Ростовская область";
        }

        public string street()
        {
            if (cacheStreet != "") return cacheStreet;
            if (response.geoObjectCollection.featureMember.Count() > 0)
            {
                foreach (var component in response.geoObjectCollection.featureMember[0].geoObject.metaDataProperty.geocoderMetaData.Address.Components)
                {
                    if (component.kind == "street")
                    {
                        cacheStreet = component.name;
                        break;
                    }
                }
            }
            return cacheStreet;
        }

        public bool founded()
        {
            if (response.geoObjectCollection.metaDataProperty.geocoderResponseMetaData.found > 0)
                return true;
            return false;
        }
    }

    struct YandexResponse
    {
        public GeoObjectCollection geoObjectCollection;
    }

    struct GeoObjectCollection
    {
        public FeatureMember[] featureMember;
        public ResponseData metaDataProperty;
    }

    struct ResponseData
    {
        public GeocoderResponseMetaData geocoderResponseMetaData;
    }

    struct GeocoderResponseMetaData
    {
        public string request;
        public int found;
        public int results;
    }

    struct FeatureMember
    {
        public GeoObject geoObject;
    }

    struct MetaDataProperty
    {
        public GeocoderMetaData geocoderMetaData;
    }

    struct GeoObject
    {
        public MetaDataProperty metaDataProperty;
        public GeoPoint point;
    }

    struct GeoPoint
    {
        public string pos;

        // lon lat
        public string lat 
        {
            get
            {
                return pos.Split(' ')[1];
            }
        }
        public string lon
        {
            get
            {
                return pos.Split(' ')[0];
            }
        }
    }

    struct GeocoderMetaData
    {
        public string kind;
        public string text;
        public string precision;
        public YandexAddress Address;
    }

    struct YandexAddress
    {
        public string country_code;
        public string formatted;
        public YandexAddressProperty[] Components;
    }

    struct YandexAddressProperty
    {
        public string kind;
        public string name;
    }
}
