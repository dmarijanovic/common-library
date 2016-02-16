using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using DamirM.CommonLibrary;

namespace DamirM.CommonLibrary
{
    public partial class InputBox : Form
    {
        ArrayList list = null;
        private InputType inputType;
        string[] confirmationStringList;
        string informationText;
        private enum InputType
        {
            Confirmation,
            SingleText,
            MultiText
        }

        public InputBox()
        {
            InitializeComponent();
            list = new ArrayList();
            list.Add(tbInputText);
        }
        public InputBox(string title, string text): this()
        {
            SetPropertys(title, text);
            inputType = InputType.SingleText;
        }
        public InputBox(string title, string text, string defText): this(title, text)
        {
            tbInputText.Text = defText;
            tbInputText.SelectAll();
        }

        /// <summary>
        /// zastarjela metoda, korisitit ovu sa ArrayList parametro
        /// </summary>
        /// <param name="title"></param>
        /// <param name="textList"></param>
        /// <param name="defText"></param>
        public InputBox(string title, Array textList, string defText)
        {
            InitializeComponent();
            inputType = InputType.MultiText;
            int lastControlID = 0;
            list = new ArrayList();
            list.Add(tbInputText);
            tbInputText.Text = defText;
            int leftTextUnos = tbInputText.Left;

            SetPropertys(title, textList.GetValue(0).ToString());
            for (int i = 1; i < textList.Length; i++)
            {
                // 
                // txtUnos
                // 
                tbInputText = new TextBox();
                this.tbInputText.Location = new System.Drawing.Point(leftTextUnos, GetControl(lastControlID).Top + lDisplayText.Height + 30);
                this.tbInputText.Name = "txtUnos_" + list.Count;
                this.tbInputText.Text = defText;
                this.tbInputText.Size = new System.Drawing.Size(211, 20);
                this.tbInputText.TabIndex = 0;

                // 
                // label1
                // 

                this.lDisplayText = new Label();
                this.lDisplayText.AutoSize = true;
                this.lDisplayText.Location = new System.Drawing.Point(leftTextUnos, GetControl(lastControlID).Top + lDisplayText.Height + 3);
                this.lDisplayText.Name = "label1_" + list.Count;
                this.lDisplayText.Size = new System.Drawing.Size(213, 13);
                this.lDisplayText.TabIndex = 4;
                this.lDisplayText.Text = textList.GetValue(i).ToString();

                this.panel1.Controls.Add(this.tbInputText);
                this.panel1.Controls.Add(this.lDisplayText);

                list.Add(tbInputText);
                if (i <= 10)
                {
                    lastControlID = i;
                }
            }
            if (list.Count > 10)
            {
                this.Width = this.Width + 10;
            }
            this.panel1.Height = GetControl(lastControlID).Top + GetControl(lastControlID).Height + 10;
            this.Height = this.panel1.Top + this.panel1.Height + btnUredu.Height + 30;
        }

        public InputBox(string title, ArrayList inputData, string defText)
        {
            InitializeComponent();
            ComboBox cbInputCombo = null;
            NameValueDataStruct nameValueDataStruct;

            int lastControlID = 0;
            int leftTextUnos = tbInputText.Left;

            this.Text = Application.ProductName + " - " + title;
            inputType = InputType.MultiText;
            list = new ArrayList();
            tbInputText.Text = defText;

            //SetPropertys(title, inputData.GetValue(0).ToString());
            for (int i = 0; i < inputData.Count; i++)
            {
                nameValueDataStruct = (NameValueDataStruct)inputData[i];
                if (nameValueDataStruct.Data == null)
                {
                    // 
                    // txtUnos
                    // 
                    if (i == 0)
                    {
                        //list.Add(tbInputText);
                        lDisplayText.Text = nameValueDataStruct.Name;
                    }
                    else
                    {
                        tbInputText = new TextBox();
                        this.tbInputText.Location = new System.Drawing.Point(leftTextUnos, GetControl(lastControlID).Top + lDisplayText.Height + 30);
                        this.tbInputText.Name = "txtUnos_" + list.Count;
                        this.tbInputText.Text = defText;
                        this.tbInputText.Size = new System.Drawing.Size(240, 20);
                        this.tbInputText.TabIndex = 0;
                    }
                }
                else
                {
                    // 
                    // comboBox
                    // 

                    cbInputCombo = new ComboBox();
                    cbInputCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                    cbInputCombo.FormattingEnabled = true;

                    ArrayList ar = (ArrayList)nameValueDataStruct.Data;

                    foreach (NameValueDataStruct item in ar)
                    {
                        cbInputCombo.Items.Add(item);
                    }


                    cbInputCombo.Location = new System.Drawing.Point(leftTextUnos, GetControl(lastControlID).Top + lDisplayText.Height + 30);
                    cbInputCombo.Name = "cbInputCombo" + list.Count;
                    cbInputCombo.Size = new System.Drawing.Size(240, 20);
                    cbInputCombo.TabIndex = 0;
                }

                // 
                // label1
                // 
                if (i != 0)
                {
                    this.lDisplayText = new Label();
                    this.lDisplayText.AutoSize = true;
                    this.lDisplayText.Location = new System.Drawing.Point(leftTextUnos, GetControl(lastControlID).Top + lDisplayText.Height + 3);
                    this.lDisplayText.Name = "label1_" + list.Count;
                    this.lDisplayText.Size = new System.Drawing.Size(213, 13);
                    this.lDisplayText.TabIndex = 4;
                    this.lDisplayText.Text = nameValueDataStruct.Name;

                    this.panel1.Controls.Add(this.lDisplayText);
                }

                if (cbInputCombo != null)
                {
                    this.panel1.Controls.Add(cbInputCombo);
                    list.Add(cbInputCombo);
                }
                else
                {
                    this.panel1.Controls.Add(tbInputText);
                    list.Add(tbInputText);
                }

                if (i <= 10)
                {
                    lastControlID = i;
                }
            }

            if (list.Count > 10)
            {
                this.Width = this.Width + 10;
            }
            this.panel1.Height = GetControl(lastControlID).Top + GetControl(lastControlID).Height + 10;
            this.Height = this.panel1.Top + this.panel1.Height + btnUredu.Height + 30;
        }


        public InputBox(string title, string text, string defText, string[] confirmationStringList): this()
        {
            SetPropertys(title, text);
            inputType = InputType.Confirmation;
            this.confirmationStringList = confirmationStringList;
        }
        private void SetPropertys(string title, string text)
        {
            lDisplayText.Text = text;
            this.Text = Application.ProductName + " - " + title;
        }

        private void btnUredu_Click(object sender, EventArgs e)
        {
            if (inputType == InputType.SingleText)
            {
                DialogResult = DialogResult.OK;
                this.Close();
            }
            else if (inputType == InputType.MultiText)
            {
                DialogResult = DialogResult.OK;
                this.Close();
            }
            else if (inputType == InputType.Confirmation)
            {
                try
                {
                    foreach (string confirmationText in confirmationStringList)
                    {
                        if (tbInputText.Text.Trim() == confirmationText)
                        {
                            DialogResult = DialogResult.OK;
                            this.Close();
                            break;
                        }
                    }
                    if (DialogResult != DialogResult.OK)
                    {
                        MessageBox.Show(informationText, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(ex, this, "btnUredu_Click", Log.LogType.ERROR, true);
                }
            }
            else
            {
                Log.Write("InputType nije definiran", this, "btnUredu_Click", Log.LogType.ERROR, true);
            }
        }

        private Control GetControl(int id)
        {
            Control control;
            if (list.Count == 0)
            {
                control = tbInputText;
                control.Top = control.Top - control.Height - 25;
            }
            else
            {
                control = (Control)list[id];
            }

            return control;
        }


        private void brnPonisti_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
        public string InputTekst
        {
            get { return tbInputText.Text.Trim(); }
        }

        public string this[int id]
        {
            get
            {
                string result = null;
                if (list[id].GetType() == typeof(TextBox))
                {
                    result = ((TextBoxBase)list[id]).Text;
                }
                else if (list[id].GetType() == typeof(ComboBox))
                {
                    ComboBox comboBox = (ComboBox)list[id];
                    if (comboBox.SelectedIndex != -1)
                    {
                        if (comboBox.Items[comboBox.SelectedIndex].GetType() == typeof(NameValueDataStruct))
                        {
                            result = ((NameValueDataStruct)comboBox.Items[comboBox.SelectedIndex]).Value.ToString();
                        }
                        else
                        {
                            result = comboBox.Text;
                        }
                    }
                    else
                    {
                        result = "";
                    }
                }

                return result;
            }
        }
        public string InformationText
        {
            set
            {
                informationText = value;
            }
        }
    }
}