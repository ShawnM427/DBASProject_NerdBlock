﻿using System;
using System.Windows.Forms;

namespace NerdBlock.Engine.Frontend.Winforms.Implementation
{
    /// <summary>
    /// An IInput connector for a combobox control's text value
    /// </summary>
    public class ComboBoxTextInput : IInput
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
            get { return myControl.Text; }
            set { myControl.Text = value == null ? "" : value.ToString(); }
        }

        /// <summary>
        /// Creates a new combo box text input
        /// </summary>
        /// <param name="valueName">The name of the input in the input map</param>
        /// <param name="input">The control to use as an input</param>
        public ComboBoxTextInput(string valueName, ComboBox input)
        {
            Name = valueName;
            myControl = input;
        }

        public void Fill(IoMap map)
        {
            if (map.HasOutput(Name))
                Value = map.GetOutput<object>(Name);
            else
                myControl.Text = "";
        }

        public void PopulateMap(IoMap currentMap)
        {
            currentMap.SetInput(Name, myControl.Text);
        }
    }
}
