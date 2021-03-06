﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdBlock.Engine.Backend.Models
{
    [DataModel("tblemployees")]
    public class Employee
    {
        [DataField("employeeid", QueryParamType.Integer), AutoGenerated, PrimaryKey]
        public int? EmployeeId { get; set; }

        [DataField("datejoined", QueryParamType.Date)]
        public DateTime? DateJoined { get; set; }

        [DataField("terminationdate", QueryParamType.Date), Nullable]
        public DateTime? TerminatedDate { get; set; }

        [DataField("sin", QueryParamType.VarChar)]
        public string SIN { get; set; }

        [DataField("firstname", QueryParamType.VarChar)]
        public string FirstName { get; set; }

        [DataField("lastname", QueryParamType.VarChar)]
        public string LastName { get; set; }

        public string FullName { get { return FirstName + " " + LastName; } }

        [DataField("email", QueryParamType.VarChar), Nullable]
        public string Email { get; set; }

        [DataField("password", QueryParamType.VarChar)]
        public string HashedPassword { get; set; }

        [DataField("phone", QueryParamType.Long), Nullable]
        public long? Phone { get; set; }

        [DataField("homeaddress", QueryParamType.Integer), ForeignKey("tbladdress", "id")]
        public Address HomeAddress { get; set; }

        [DataField("roleid", QueryParamType.Integer), ForeignKey("tblemployeerole", "roleid")]
        public EmployeeRole Role { get; set; }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}
