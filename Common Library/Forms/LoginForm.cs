using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Odbc;

namespace DamirM.CommonLibrary
{
    public partial class LoginForm : Form
    {
        OdbcCommand odbcCommand;
        OdbcConnection odbcConn = new System.Data.Odbc.OdbcConnection();
        int userID = 0;
        int pristup;
        int poslovnica;

        string connectionString;

        public LoginForm(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
            odbcConn = new System.Data.Odbc.OdbcConnection(connectionString);
        }

        private void btnPonisti_Click(object sender, EventArgs e)
        {
            this.Close();

        }
        private void btnPrijava_Click(object sender, EventArgs e)
        {
            if (MyValidate())
            {
                Log.Write("Rucna prijava...", this.Name, "btnPrijava_Click", Log.LogType.DEBUG);
                if (PrijaviSe(tbUsername.Text, tbPassword.Text))
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Prijava nije uspjela", "Prijava", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            else
            {
                errorProvider1.SetError(tbUsername, "Unesi korisnièko ime");
                errorProvider1.SetError(tbPassword, "Unesi lozinku");
            }
        }
        private bool PrijaviSe(string username, string password)
        {
            userID = 0;
            OdbcDataReader dr;
            odbcCommand = new OdbcCommand("SELECT user_id FROM users WHERE username = ? AND password = ? AND status = 'active'", odbcConn);
            odbcCommand.Parameters.Add("@username", OdbcType.VarChar, 32);
            odbcCommand.Parameters.Add("@password", OdbcType.VarChar, 32);
            try
            {
                odbcCommand.Parameters["@username"].Value = username;
                odbcCommand.Parameters["@password"].Value = password;
                
                if (odbcConn.State != ConnectionState.Open)
                    odbcConn.Open();
                dr = odbcCommand.ExecuteReader();
                while (dr.Read())
                {
                    int.TryParse(dr["user_id"].ToString(), out userID);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Write(ex, this.Name, "PrijaviSe", Log.LogType.ERROR);

            }
            finally
            {
                if (odbcConn.State == ConnectionState.Open)
                    odbcConn.Close();
            }
            if (userID > 0)
            {
                Log.Write("Prijavljen pod " + tbUsername.Text, this.Name, "PrijaviSe", Log.LogType.DEBUG);
                UpdateLastLogin();
                return true;
            }
            else
            {
                Log.Write(odbcCommand.CommandText, this.Name, "PrijaviSe", Log.LogType.WARNING);
                return false;
            }
        }
        public bool AutoLogin(string username, string password)
        {
            // omogucujue public prijavu sa sacuvanim podacima 
            Log.Write("Auto login", this.Name, "PrijaviSe", Log.LogType.DEBUG);
            if (PrijaviSe(username, password))
            {
                this.DialogResult = DialogResult.OK;
                return true;
            }
            else
            {
                return false;
            }
        }
        private void UpdateLastLogin()
        {
            odbcCommand = new OdbcCommand("UPDATE users SET last_login = NOW(), counter = counter + 1 WHERE user_id = " + this.userID, odbcConn);
            try
            {
                if (odbcConn.State != ConnectionState.Open)
                    odbcConn.Open();
                odbcCommand.ExecuteScalar();
            }
            catch (Exception exc)
            {
                Log.Write(exc, this, "UpdateLastLogin", Log.LogType.ERROR);
            }
            finally
            {
                if (odbcConn.State == ConnectionState.Open)
                    odbcConn.Close();
            }
        }
        private bool MyValidate()
        {
            if (tbUsername.Text.Trim() == "" && tbPassword.Text.Trim() == "")
                return false;
            else
                return true;
        }


        public int UserID
        {
            get { return userID; }
        }
        public string Ime
        {
            get { return tbUsername.Text.Trim(); }
        }
        public string Lozinka
        {
            get { return tbPassword.Text; }
        }
        public int Pristup
        {
            get { return pristup; }
        }
    }
}