﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace hauntedBuildinggrp3
{
    public partial class fmLogin : Form
    {
        public fmLogin()
        {
            InitializeComponent();
        }
        

        private void fmLogin_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.CenterToScreen();
            txtPass.MaxLength = 20;
            txtUser.TabIndex = 0;
            txtPass.TabIndex = 1;
            btnLogin.TabIndex = 2;
            lklbNewAcc.TabIndex = 3;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUser.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter your username ");
                txtUser.Focus();

            }
            else if (txtPass.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter your password");
                txtPass.Focus();
            }

            string Player;
            Player = txtUser.Text;
            int UStatus;

            SqlConnection Cnn = new SqlConnection();
            Cnn.ConnectionString = "Data Source=WIN-PC;Initial Catalog=HauntedBuilding;Integrated Security=True";
            SqlCommand oCmd = new SqlCommand();
            //SqlDataReader DR= oCmd.ExecuteReader();
            //oCmd.Connection.ConnectionString="Data Source=WIN-PC; Initial Ctalog=HauntedBuilding; Integrated Security=True"; //oConn is a Module which create a connection to sql
            oCmd.Connection = Cnn;
            oCmd.CommandTimeout = 0;
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "spLogin";
            oCmd.Parameters.Add("@UserName", SqlDbType.NVarChar);
            oCmd.Parameters.Add("@Password", SqlDbType.NVarChar);
            oCmd.Parameters["@UserName"].Value = txtUser.Text;
            oCmd.Parameters["@Password"].Value = txtPass.Text;
            SqlDataReader DR;


            try
            {
                Cnn.Open();
                DR = oCmd.ExecuteReader();
                if (DR.Read())
                {
                    DR.Close();
                    this.Close();
                    UStatus = 1;
                    fmSelectGame fmSelGame = new fmSelectGame(Player, UStatus);
                    fmSelGame.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Username or password not valid");
                    DR.Close();
                    txtUser.Clear();
                    txtPass.Clear();
                    txtUser.Focus();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                txtUser.Clear();
                txtPass.Clear();
                txtUser.Focus();
            }
            //UStatus = 1;
            //fmSelectGame fmSelGame = new fmSelectGame(Player, UStatus);
            //fmSelGame.ShowDialog();
        }

        private void lklbNewAcc_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
            fmNewAcc showfmCreateAcc = new fmNewAcc();
            showfmCreateAcc.ShowDialog();
           

           
        }
    }
}