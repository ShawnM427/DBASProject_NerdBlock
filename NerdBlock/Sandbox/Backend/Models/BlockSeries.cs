﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdBlock.Sandbox.Backend.Models
{
    [DataModel("tblblockseries")]
    public class BlockSeries
    {
        [DataField("seriesid", QueryParamType.Integer), AutoGenerated, PrimaryKey]
        public int? SeriesId { get; set; }

        [DataField("genreid", QueryParamType.Integer), ForeignKey("tblgenre", "genreid")]
        public int? GenreId { get; set; }

        [DataField("title", QueryParamType.VarChar)]
        public string Title { get; set; }

        [DataField("subscriptionprice", QueryParamType.Money)]
        public decimal? SubscriptionPrice { get; set; }

        [DataField("blockvolume", QueryParamType.Integer)]
        public int? BlockVolume { get; set; }
    }
}