using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLoader
{
    class OSMObject: IGeoObject
    {
        public string place_id;
        public string osm_type;
        public string lat;
        public string lon;
        public string display_name;

        public string @class;
        public string @type;

        public OSMAddressDetail address;

        // TODO

        public string region()
        {
            throw new NotImplementedException();
        }

        public string district()
        {
            throw new NotImplementedException();
        }

        public string locality()
        {
            throw new NotImplementedException();
        }

        public string street()
        {
            throw new NotImplementedException();
        }

        public string house()
        {
            throw new NotImplementedException();
        }

        public string addressString()
        {
            return display_name;
        }

        public string latitude()
        {
            throw new NotImplementedException();
        }

        public string longitude()
        {
            throw new NotImplementedException();
        }
    }

    class OSMAddressDetail
    {
        public string road;
        public string suburb;
        public string city_district;
        public string city;
        public string county;
        public string state;
        public string postcode;
        public string country;
        public string country_code;
        public string house_number;
        public string village;
    }
}
