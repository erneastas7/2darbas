using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2darbas
      
{

    public partial class Form1 : Form

    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
            
        {
            button5.Visible = true;
            string text = textBox1.Text;
            string key = textBox3.Text;

            label7.Visible = true;
            textBox5.Visible = true;

            Encoding encoding = Encoding.GetEncoding("437");
            byte[] IV = encoding.GetBytes(textBox7.Text);

            try
            {

                string encryptedString = Encrypt(text, key,IV);
                textBox5.Text = encryptedString;

            }
            catch (Exception exc)
            {
                MessageBox.Show("Error: " + exc.Message);
            }

        }


        
        private void button2_Click(object sender, EventArgs e)
        {
            label8.Visible = true;
            textBox6.Visible = true;

            textBox6.Clear();
            string text = textBox2.Text;
            string key = textBox4.Text;

            Encoding encoding = Encoding.GetEncoding("437");
            byte[] IV = encoding.GetBytes(textBox7.Text);
            
            try
            {
                string decryptedString = Decrypt(text, key,IV);
                textBox6.Text = decryptedString;
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error: " + exc.Message);
            }

        }
        
        
        public static string Encrypt(string text, string key, byte[] IV)
        {
            if (key.Length == 8)
            { 
                // Encode message and password
                byte[] messageBytes = ASCIIEncoding.ASCII.GetBytes(text);
                byte[] passwordBytes = ASCIIEncoding.ASCII.GetBytes(key);
              
                
                // Set encryption settings -- Use password for both key and init. vector
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                ICryptoTransform transform = provider.CreateEncryptor(passwordBytes, IV);
                CryptoStreamMode mode = CryptoStreamMode.Write;

                // Set up streams and encrypt
                MemoryStream memStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memStream, transform, mode);
                cryptoStream.Write(messageBytes, 0, messageBytes.Length);
                cryptoStream.FlushFinalBlock();

                // Read the encrypted message from the memory stream
                byte[] encryptedMessageBytes = new byte[memStream.Length];
                memStream.Position = 0;
                memStream.Read(encryptedMessageBytes, 0, encryptedMessageBytes.Length);

                // Encode the encrypted message as base64 string
                string encryptedMessage = Convert.ToBase64String(encryptedMessageBytes);

                return encryptedMessage;
            }
            else throw new Exception("Key must be 8 symbols lenght ! (8 Bits)");
        }


        
        
        public static string Decrypt(string encryptedMessage, string key, byte[]IV)
        {
            if (key.Length == 8)
            {
                // Convert encrypted message and password to bytes
                byte[] encryptedMessageBytes = Convert.FromBase64String(encryptedMessage);
                byte[] passwordBytes = ASCIIEncoding.ASCII.GetBytes(key);
                


                // Set encryption settings -- Use password for both key and init. vector
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                ICryptoTransform transform = provider.CreateDecryptor(passwordBytes, IV);
                CryptoStreamMode mode = CryptoStreamMode.Write;

                // Set up streams and decrypt
                MemoryStream memStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memStream, transform, mode);
                cryptoStream.Write(encryptedMessageBytes, 0, encryptedMessageBytes.Length);
                cryptoStream.FlushFinalBlock();

                // Read decrypted message from memory stream
                byte[] decryptedMessageBytes = new byte[memStream.Length];
                memStream.Position = 0;
                memStream.Read(decryptedMessageBytes, 0, decryptedMessageBytes.Length);

                // Encode deencrypted binary data to base64 string
                string decryptedMessage = ASCIIEncoding.ASCII.GetString(decryptedMessageBytes);
                return decryptedMessage;
            }
            else throw new Exception("Key must be 8 symbols lenght ! (8 Bits)");
        }
        
        private void textBox3_TextChanged(object sender, EventArgs e)
      
        {

        }
        
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            var random = new Random();

            byte[] IV = new byte[8];

            random.NextBytes(IV);

            Encoding encoding = Encoding.GetEncoding("437");

            textBox7.Text = encoding.GetString(IV);

           // IV = encoding.GetBytes(textBox7.Text);
         
        }

        private void button5_Click(object sender, EventArgs e)
      
        {

            
            

            File.WriteAllText("C:\\Users\\Huh\\Desktop\\2DarbasEncrypted.txt", textBox5.Text + Environment.NewLine + textBox7.Text);
            MessageBox.Show("Saved to C:\\Users\\Huh\\Desktop\\2DarbasEncrypted.txt", "Saved Log File", MessageBoxButtons.OK, MessageBoxIcon.Information);

            
            /*
                // assigned to Button2.
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Text Files | *.txt";
                saveFileDialog1.Title = " File is saved";
                saveFileDialog1.ShowDialog();
            
                // If the file name is not an empty string open it for saving.       
                // Saves the Image via a FileStream created by the OpenFile method    
                // Saves the Image in the appropriate ImageFormat based upon the
                // File type selected in the dialog box.
                // NOTE that the FilterIndex property is one-based.
                string savePath = Path.GetDirectoryName(saveFileDialog1.FileName);
                File.WriteAllText(savePath, textBox5.Text + Environment.NewLine + textBox7.Text);
               */

        }
        


        private void button4_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "File selection";
            fd.InitialDirectory = @"C:\";
            fd.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
            fd.FilterIndex = 2;
            fd.RestoreDirectory = true;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                string[] lines = System.IO.File.ReadAllLines(fd.FileName);
                textBox2.Text = lines[0];
                textBox7.Text = lines[1];
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
