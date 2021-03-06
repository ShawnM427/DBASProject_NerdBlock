﻿using System;
using System.Windows.Forms;

namespace NerdBlock.Engine.Frontend.Winforms.Implementation
{
    /// <summary>
    /// An IInput connector for a combo box control's selected item
    /// </summary>
    public class ComboBoxValueInput : IInput
    {
        private ComboBox myControl;

        /// <summary>
        /// Gets or sets the name of the input in the input map
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// Gets the value of the input
        /// </summary>
        public object Value
        {
            get { return myControl.SelectedItem; }
            set
            {
                if (value != null && myControl.Items.Contains(value))
                    myControl.SelectedItem = value;
            }
        }
        /// <summary>
        /// Creates a new combo box value input
        /// </summary>
        /// <param name="valueName">The name of the input in the input map</param>
        /// <param name="input">The control to use as an input</param>

        public ComboBoxValueInput(string valueName, ComboBox input)
        {
            Name = valueName;
            myControl = input;
        }

        /// <summary>
        /// Fills this IInput from a given IO map
        /// </summary>
        /// <param name="map">The map to fill from</param>
        public void Fill(IoMap map)
        {
            if (map.HasInput(Name))
                Value = map.GetInput<object>(Name);
            else if (myControl.Items.Count > 0)
                myControl.SelectedIndex = 0;
        }

        /// <summary>
        /// Populates an IOMap from this input
        /// </summary>
        /// <param name="map">The map to populate</param>
        public void PopulateMap(IoMap currentMap)
        {
            currentMap.SetInput(Name, myControl.SelectedItem);
        }
    }
}
