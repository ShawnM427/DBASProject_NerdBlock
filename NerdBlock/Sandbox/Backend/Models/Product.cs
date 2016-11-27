﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdBlock.Sandbox.Backend.Models
{
    [DataModel("tblproduct")]
    class Product
    {
        [DataField("productid", QueryParamType.Integer), AutoGenerated, PrimaryKey]
        public int? ProductId { get; set; }

        [DataField("name", QueryParamType.VarChar)]
        public string Name { get; set; }

        [DataField("description", QueryParamType.Text)]
        public string Description { get; set; }

        [DataField("width", QueryParamType.Double)]
        public decimal? Width { get; set; }

        [DataField("height", QueryParamType.Double)]
        public decimal? Height { get; set; }

        [DataField("depth", QueryParamType.Double)]
        public decimal? Depth { get; set; }

        [DataField("numdamaged", QueryParamType.Integer), Nullable]
        public int? NumDamaged { get; set; }
    }
}
