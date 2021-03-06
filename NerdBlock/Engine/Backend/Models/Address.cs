﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdBlock.Engine.Backend.Models
{
    [DataModel("tbladdress")]
    public class Address
    {
        [DataField("addressid", QueryParamType.Integer), PrimaryKey, AutoGenerated]
        public int? AddressId { get; set; }

        [DataField("streetaddress", QueryParamType.VarChar)]
        public string StreetAddress { get; set; }

        [DataField("country", QueryParamType.VarChar)]
        public string Country { get; set; }

        [DataField("state", QueryParamType.VarChar)]
        public string State { get; set; }

        [DataField("city", QueryParamType.VarChar)]
        public string City { get; set; }

        [DataField("apartmentnumber", QueryParamType.Integer), Nullable]
        public int? ApartmentNumber { get; set; }

        [DataField("specialrequests", QueryParamType.Text), Nullable]
        public string SpecialRequests { get; set; }

        [DataField("postalcode", QueryParamType.VarChar)]
        public string PostalCode { get; set; } = null;
    }
}
