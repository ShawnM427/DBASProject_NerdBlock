﻿
namespace NerdBlock.Engine.Backend.Models
{
    [DataModel("tblsupplier")]
    public class Supplier
    {
        [DataField("supplierid", QueryParamType.Integer), AutoGenerated, PrimaryKey]
        public int? SupplierId { get; set; }

        [DataField("company", QueryParamType.VarChar)]
        public string Company { get; set; }

        [DataField("address", QueryParamType.Integer), ForeignKey("tbladdress", "addressid")]
        public Address Address { get; set; }

        [DataField("phone", QueryParamType.Long)]
        public long? Phone { get; set; }
        
        public override string ToString()
        {
            return Company;
        }
    }
}
