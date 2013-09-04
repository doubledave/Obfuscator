using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace obfuscator
{
    public partial class Form1 : Form
    {
        public System.Collections.ArrayList listBox1 = new System.Collections.ArrayList();
        public int[] statistics = new int[32];
        Random rnd = new Random();
        int MaxMsg = 60000;

        public Form1()
        {
            InitializeComponent();
            buttonObfuscate.Enabled = true;
            buttonDecrypt.Enabled = true;
            progressBar1.Visible = false;
            label1.Text = "Input text, "+MaxMsg.ToString()+" characters max";
            label2.Text = "Characters remaining: " + MaxMsg.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label2.Text = "Characters remaining: " + (MaxMsg - textBox1.Text.Length).ToString();
        }

        private void buttonObfuscate_Click(object sender, EventArgs e)
        {
            if (textBoxSender.Text.Length < 1)
            { MessageBox.Show("Please use an encryption key of at least one character."); goto end; }
            
            if (textBox1.Text.Length < 1 || textBox1.Text.Length > MaxMsg)
            { MessageBox.Show("Message length must be between 1 and " + MaxMsg.ToString() + " characters."); goto end; }
            // Ready to encrypt
            textBox4.Text = encrypt(textBox1.Text, textBoxSender.Text +"obfuscator");
            label6.Text = "Characters: " + textBox4.Text.Length.ToString();
            
        end: ;
        }

        private void buttonDecrypt_Click(object sender, EventArgs e)
        {
            if (textBoxSender.Text.Length < 1)
            { MessageBox.Show("Please use an encryption key of at least one character."); goto end; }

            if (textBox1.Text.Length < 1 || textBox1.Text.Length > MaxMsg)
            { MessageBox.Show("Message length must be between 1 and " + MaxMsg.ToString() + " characters."); goto end; }
            // Ready to decrypt
            textBox4.Text = decrypt(textBox1.Text, textBoxSender.Text + "obfuscator");
            label6.Text = "Characters: " + textBox4.Text.Length.ToString();

        end: ;
        }

  

        private string encrypt(string input, string hash)
        {
            hash = GetMD5Hash(hash);
            string output = "";
            int index = 0;
            foreach (char c in input)
            {   if (index >= hash.Length) { index = 0; }
                byte b = Convert.ToByte(Convert.ToChar(hash.Substring(index, 1)));
                index++;
                if (b < 58) { b -= 47; } // convert ascii numbers into numbers 1-10
                if (b > 64) { b -= 86; } // converv ascii hex A-F into numbers 11-16
                string upperCase = "";
                byte tempc = Convert.ToByte(c);
                if (tempc > 47 && tempc < 58) { upperCase = "N"; } // number
                if (tempc > 64 && tempc < 91) { upperCase = "U"; } // upper
                if (tempc > 96 && tempc < 123) { upperCase = "L"; } // lower
                if (upperCase.Length > 0 ) { tempc += b; } 
                if (upperCase == "N" && tempc > 57) { tempc -= 10; }
                if (upperCase == "N" && tempc > 57) { tempc -= 10; } // repeat once
                if (upperCase == "U" && tempc > 90) { tempc -= 26; }
                if (upperCase == "L" && tempc > 122) { tempc -= 26; }
                output += Convert.ToChar(tempc);
           }
            return output;

        }
        private string decrypt(string input, string hash)
        {
            hash = GetMD5Hash(hash);
            string output = "";
            int index = 0;
            foreach (char c in input)
            {   if (index >= hash.Length) { index = 0; }
                byte b = Convert.ToByte(Convert.ToChar(hash.Substring(index, 1)));
                index++;
                if (b < 58) { b -= 47; } // convert ascii numbers into numbers 1-10
                if (b > 64) { b -= 86; } // converv ascii hex A-F into numbers 11-16
                
                string upperCase = "";
                byte tempc = Convert.ToByte(c);
                if (tempc > 47 && tempc < 58) { upperCase = "N"; }
                if (tempc > 64 && tempc < 91) { upperCase = "U"; }
                if (tempc > 96 && tempc < 123) { upperCase = "L"; }
                if (upperCase.Length > 0 ) { tempc -= b; }
                if (upperCase == "N" && tempc < 48) { tempc += 10; }
                if (upperCase == "N" && tempc < 48) { tempc += 10; } // repeat once
                if (upperCase == "U" && tempc < 65) { tempc += 26; }
                if (upperCase == "L" && tempc < 97) { tempc += 26; }
                output += Convert.ToChar(tempc);
            }

            return output;
        }



        public string GetMD5Hash(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }

        private void textBoxSender_TextChanged(object sender, EventArgs e)
        {
            textBoxMd5.Text = GetMD5Hash(textBoxSender.Text);
        }

    }
}
