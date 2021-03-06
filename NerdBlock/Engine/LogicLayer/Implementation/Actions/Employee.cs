﻿using NerdBlock.Engine.Backend;
using NerdBlock.Engine.Backend.Models;
using NerdBlock.Engine.Frontend;
using System;

namespace NerdBlock.Engine.LogicLayer.Implementation.Actions
{
    /// <summary>
    /// Stores actions related to employees
    /// </summary>
    [BusinessActionContainer]
    public class EmployeeActions
    {
        /// <summary>
        /// Handles inserting a new employee into the database, assumes that there is valid employee information in the context
        /// </summary>
        [BusinessAction("insert_employee")]
        [AuthAttrib("Human Resources", "General Manager")]
        public void Insert()
        {
            IoMap map = ViewManager.CurrentMap;

            string msg = "";
            if (!Validate("", ref msg) || Validations.ValidateAddressFromMap(map, "Address", ref msg) == null)
            {
                ViewManager.ShowFlash(string.Format("Error Adding Employee:\n{0}", msg), FlashMessageType.Bad);
                ViewManager.Show("AddEmployee", map);
            }
            else
            {
                Employee employee = new Employee();
                employee.FirstName = map.GetInput<string>("FirstName");
                employee.LastName = map.GetInput<string>("LastName");
                employee.SIN = map.GetInput<string>("SIN");
                employee.Email = map.GetInput<string>("Email");
                employee.Phone = long.Parse(map.GetInput<string>("Phone").Replace(" ", "").Replace("-", ""));
                employee.DateJoined = DateTime.Now;
                employee.HashedPassword = PasswordSecurity.PasswordStorage.CreateHash(employee.SIN);

                Address address = new Address();
                address.StreetAddress = map.GetInput<string>("Address.StreetAddress");
                address.Country = map.GetInput<string>("Address.Country");
                address.State = map.GetInput<string>("Address.State");
                address.PostalCode = map.GetInput<string>("Address.PostalCode");
                address.City = map.GetInput<string>("Address.City");

                int aptNum = -1;
                if (int.TryParse(map.GetInput<string>("Address.AptNum"), out aptNum))
                    address.ApartmentNumber = aptNum;
                else
                    address.ApartmentNumber = null;
                employee.HomeAddress = address;
                
                employee.Role = map.GetInput<EmployeeRole>("Role");

                if (DataAccess.Insert(employee))
                {
                    employee = DataAccess.Match(employee)[0];
                    ViewManager.ShowFlash(string.Format("Employee added with ID: {0}\nTheir password will be their SIN until they change it.", employee.EmployeeId), FlashMessageType.Good);
                    Logger.Log(LogLevel.Info, "Added employee with ID {0}", employee.EmployeeId);
                    ViewManager.Show("AddEmployee", map);
                }
                else
                {
                    ViewManager.ShowFlash("Failed to add employee:\n" + DataAccess.Database.LastFailReason.Message, FlashMessageType.Bad);
                    Logger.Log(LogLevel.Error, DataAccess.Database.LastFailReason.Message);
                    ViewManager.Show("AddEmployee", map);
                }
            }
        }

        /// <summary>
        /// Handles updating an employee in the database
        /// </summary>
        [BusinessAction("update_employee")]
        [AuthAttrib("Human Resources", "General Manager")]
        public void Update()
        {
            IoMap map = ViewManager.CurrentMap;

            string msg = "";
            if (!Validate("Employee", ref msg) || Validations.ValidateAddressFromMap(map, "Address", ref msg) == null)
            {
                ViewManager.ShowFlash(string.Format("Error updating Employee:\n{0}", msg), FlashMessageType.Bad);
                ViewManager.Show("ViewEditEmployee", map);
            }
            else
            {
                Employee employee = map.GetInput<Employee>("Employee");

                employee.FirstName = map.GetInput<string>("Employee.FirstName");
                employee.LastName = map.GetInput<string>("Employee.LastName");
                employee.SIN = map.GetInput<string>("Employee.SIN");
                employee.Email = map.GetInput<string>("Employee.Email");
                employee.Phone = long.Parse(map.GetInput<string>("Employee.Phone").Replace(" ", "").Replace("-", ""));
                employee.DateJoined = DateTime.Now;
                employee.HashedPassword = null;
                
                employee.HomeAddress.StreetAddress = map.GetInput<string>("Address.Street");
                employee.HomeAddress.Country = map.GetInput<string>("Address.Country");
                employee.HomeAddress.State = map.GetInput<string>("Address.State");
                employee.HomeAddress.PostalCode = map.GetInput<string>("Address.PostalCode");
                employee.HomeAddress.City = map.GetInput<string>("Address.City");

                int aptNum = -1;
                if (int.TryParse(map.GetInput<string>("Address.AptNum"), out aptNum))
                    employee.HomeAddress.ApartmentNumber = aptNum;
                else
                    employee.HomeAddress.ApartmentNumber = null;
                employee.HomeAddress = employee.HomeAddress;

                employee.Role = map.GetInput<EmployeeRole>("Employee.Role");

                if (DataAccess.Update(employee, false, true) > 0)
                {
                    employee = DataAccess.Match(employee)[0];
                    ViewManager.ShowFlash(string.Format("Employee has sucessfully been updated"), FlashMessageType.Good);
                    Logger.Log(LogLevel.Info, "Updated employee with ID {0}", employee.EmployeeId);
                    map.Reset();
                    ViewManager.Show("EmployeeSearch", map);
                }
                else
                {
                    ViewManager.ShowFlash("Failed to update employee:\n" + DataAccess.Database.LastFailReason.Message, FlashMessageType.Bad);
                    Logger.Log(LogLevel.Error, DataAccess.Database.LastFailReason.Message);
                    ViewManager.Show("AddEmployee", map);
                }
            }
        }

        /// <summary>
        /// Handles showing the employee add view
        /// </summary>
        [BusinessAction("goto_employee_add")]
        [AuthAttrib("Human Resources", "General Manager")]
        public void ShowAdd()
        {
            ViewManager.Show("AddEmployee");
        }

        /// <summary>
        /// Handles showing the employee search view
        /// </summary>
        [BusinessAction("goto_employee_search")]
        [AuthAttrib("Human Resources", "General Manager")]
        public void ShowSearch()
        {
            ViewManager.Show("EmployeeSearch");
        }

        /// <summary>
        /// Handles showing the employee update view
        /// </summary>
        [BusinessAction("goto_employee_update")]
        [AuthAttrib("Human Resources", "General Manager")]
        public void ShowUpdate()
        {
            ViewManager.Show("ViewEditEmployee");
        }




        /// <summary>
        /// Handles checking to see if the employee data entry on the input map is valid
        /// </summary>
        /// <param name="employee">The employee instance to validate</param>
        /// <param name="reason">The reference to the reason for failure</param>
        /// <returns>True if validation was successful, false if otherwise</returns>
        public static bool Validate(string prefix, ref string reason)
        {
            bool result = true;

            if (string.IsNullOrEmpty(prefix))
                prefix = "";
            else
                prefix += ".";

            IoMap map = ViewManager.CurrentMap;

            string fName = map.GetInput<string>(prefix + "FirstName");
            string lName = map.GetInput<string>(prefix + "LastName");
            string sin = map.GetInput<string>(prefix + "SIN");
            string phone = map.GetInput<string>(prefix + "Phone");
            string email = map.GetInput<string>(prefix + "Email");

            long tempVal = -1;

            if (string.IsNullOrWhiteSpace(fName) || fName.Length < 2)
            {
                result = false;
                reason += "First name must be at least 2 characters" + Environment.NewLine;
            }

            if (string.IsNullOrWhiteSpace(lName) || lName.Length < 2)
            {
                result = false;
                reason += "Last name must be at least 2 characters" + Environment.NewLine;
            }

            if (string.IsNullOrWhiteSpace(sin) || sin.Length != 9)
            {
                result = false;
                reason += "SIN must be exactly 9 characters" + Environment.NewLine;
            }
            else if (!long.TryParse(sin, out tempVal))
            {
                result = false;
                reason += "SIN must be numeric" + Environment.NewLine;
            }

            if (string.IsNullOrWhiteSpace(phone) || phone.Replace(" ", "").Length < 10)
            {
                result = false;
                reason += "Phone # must be at least 10 characters" + Environment.NewLine;
            }
            else if (!long.TryParse(phone.Replace(" ", "").Replace("-", ""), out tempVal))
            {
                result = false;
                reason += "Phone # must be numeric" + Environment.NewLine;
            }

            return result;
        }
    }
}
