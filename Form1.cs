using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Security;


namespace My_Xml_CryptorDecryptor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCreatKeys_Click(object sender, EventArgs e)
        {
            try
            {
                RSACryptoServiceProvider RsaKey = new RSACryptoServiceProvider();
                string publickey = RsaKey.ToXmlString(false);
                string privatekey = RsaKey.ToXmlString(true);
                File.WriteAllText("private.xml", privatekey, Encoding.UTF8);
                File.WriteAllText("public.xml", publickey, Encoding.UTF8);
                btnRsaToXML.BackColor = Color.Green;
            }
            catch (Exception ex)
            {
                btnRsaToXML.BackColor = Color.Red;
                MessageBox.Show(ex.Message.ToString());
            }
        }


        bool _isupdate= true;
        bool _isdata=true;
        bool _ispriv_xml = true;
        bool _ispub_xml = true;
        byte[] EncryptedData;
        byte[] DecryptedData;
        string publicxml = "";
        string privatexml = "";

        private void btnLoadKeys_Click(object sender, EventArgs e)
        {
            try
            {
                privatexml = File.ReadAllText("private.xml", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem with private key... \n" + ex.Message.ToString());
                return;
            }
            try
            {
                publicxml = File.ReadAllText("public.xml", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                btnLoadRsaXml.BackColor = Color.Red;
                MessageBox.Show("Problem with public key... \n" + ex.Message.ToString());
                return;
            }
            btnLoadRsaXml.BackColor = Color.Green;
            
            
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            byte [] data=new byte[1024];
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            try
            {
                if (privatexml.Length == 0)
                {
                    btnDecryptData.BackColor = Color.Red;
                    _ispriv_xml = false;
                    MessageBox.Show("Bad private key...");
                    return;
                }
                else
                {
                    rsa.FromXmlString(privatexml);
                }
            }
            catch (Exception ex)
            {
                btnDecryptData.BackColor = Color.Red;
                MessageBox.Show("Problem with RSA... \n" + ex.Message.ToString());
            }
            try
            {
                data = Convert.FromBase64String(txtBase64.Text);
            }
            catch (FileNotFoundException)
            {
                btnDecryptData.BackColor = Color.Red;
                MessageBox.Show("Data file does not exist...");
                return;
            }
            try
            {
                DecryptedData = rsa.Decrypt(data, false);
            }
            catch (CryptographicException ex)
            {
                btnDecryptData.BackColor = Color.Red;
                MessageBox.Show("Crypto error... \n" + ex.Message.ToString());
                return;
            }
            string text= Encoding.UTF8.GetString(DecryptedData);
            btnDecryptData.BackColor = Color.Green;
            MessageBox.Show(text);
            
            for (int i = 0; i < data.Length - 1; i++)
            {
                data.SetValue((byte)0, i);
            }
            for (int i = 0; i < DecryptedData.Length - 1; i++)
            {
                DecryptedData.SetValue((byte)0, i);
            }

        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[1024];
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            try
            {
                if (publicxml.Length == 0)
                {
                    btnEncrypt.BackColor = Color.Red;
                    _ispub_xml = false;
                    MessageBox.Show("Bad public key...");
                    return;
                }
                else
                {
                    rsa.FromXmlString(publicxml);
                }
            }
            catch (Exception ex)
            {
                btnEncrypt.BackColor = Color.Red;
                MessageBox.Show("Problem with RSA... \n" + ex.Message.ToString());
            }
            try
            {
                data = Encoding.UTF8.GetBytes(txtYourText.Text);
            }
            catch (Exception ss)
            {
                btnEncrypt.BackColor = Color.Red;
                MessageBox.Show(ss.ToString());
                return;
            }
            try
            {
                EncryptedData = rsa.Encrypt(data, false);
            }
            catch (CryptographicException ex)
            {
                btnEncrypt.BackColor = Color.Red;
                MessageBox.Show("Crypto error... \n" + ex.Message.ToString());
            }
            txtEncryptedText.Text = Encoding.UTF8.GetString(EncryptedData);
            txtBase64.Text = Convert.ToBase64String(EncryptedData);
            btnEncrypt.BackColor = Color.Green;
            for (int i = 0; i < data.Length - 1; i++)
            {
                data.SetValue((byte)0, i);
            }
            for (int i = 0; i < EncryptedData.Length - 1; i++)
            {
                EncryptedData.SetValue((byte)0, i);
            }
        }
    }
}