using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DamirM.CommonLibrary
{
    public partial class WindowsToolsTop : UserControl
    {
        Color cActiveBackColor = System.Drawing.SystemColors.ActiveCaption;
        Color cInactiveBackColor = System.Drawing.SystemColors.InactiveCaption;

        public WindowsToolsTop()
        {
            InitializeComponent();
            SetColorToControl(cInactiveBackColor);
        }

        /// <summary>
        /// Set caption name of control
        /// </summary>
        public string Caption
        {
            get
            {
                return lName.Text;
            }
            set
            {
                lName.Text = value;
            }
        }

        /// <summary>
        /// Active color of control
        /// </summary>
        public Color ActiveColor
        {
            get
            {
                return cActiveBackColor;
            }
            set
            {
                this.cActiveBackColor = value;
            }
        }

        /// <summary>
        /// Inactive color of control
        /// </summary>
        public Color InactiveColor
        {
            get
            {
                return cInactiveBackColor;
            }
            set
            {
                this.cInactiveBackColor = value;
            }
        }

        /// <summary>
        /// Set colors to all controls
        /// </summary>
        /// <param name="color"></param>
        private void SetColorToControl(Color color)
        {
            this.BackColor = color;
            this.lClose.BackColor = color;
            this.lName.BackColor = color;
        }

        private void lClose_MouseLeave(object sender, EventArgs e)
        {
            lClose.BackColor = cInactiveBackColor;
        }

        private void lClose_MouseEnter(object sender, EventArgs e)
        {
            lClose.BackColor = cActiveBackColor;
        }
    }
}
