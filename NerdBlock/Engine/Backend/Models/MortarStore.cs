﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdBlock.Engine.Backend.Models
{
    [DataModel("tblmortarstore")]
    public class MortarStore
    {
        [DataField("storeid", QueryParamType.Integer), AutoGenerated, PrimaryKey]
        public int? StoreId { get; set; }

        [DataField("address", QueryParamType.Integer), ForeignKey("tbladdress", "addressid")]
        public Address Address { get; set; }

        [DataField("phonenumber", QueryParamType.Long)]
        public long? PhoneNumber { get; set; }

        [DataField("generalmanager", QueryParamType.Integer), ForeignKey("tblemployees", "emploeeid")]
        public Employee GeneralManager { get; set; }
    }
}
