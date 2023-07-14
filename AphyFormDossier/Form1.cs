using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AphyFormDossier
{
    public partial class Form1 : Form
    {

        public PictureBox[] pictureBoxes;
        public Label[] labelTextPicture;
        FileInfo[] myfiles;
        public int startPicture = 0;
        public int dayLimits = 5;
        public int j = 0;
        public FlowLayoutPanel flowLayoutPanel1 = new FlowLayoutPanel();
        public FlowLayoutPanel flowLayoutPanel2 = new FlowLayoutPanel();


        public Form1()
        {   
            flowLayoutPanel1.Location = new Point(183, 12);
            flowLayoutPanel1.Size = new Size(307, 426);
            flowLayoutPanel1.AutoScroll = true;
            this.Controls.Add(flowLayoutPanel1);            

            flowLayoutPanel2.Location = new Point(496, 12);
            flowLayoutPanel2.Size = new Size(292, 426);
            flowLayoutPanel2.AutoScroll = true;
            this.Controls.Add(flowLayoutPanel2);
            InitializeComponent();
        }

        private void btn_open_Click(object sender, EventArgs e)
        {            
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel2.Controls.Clear();
            
            FDB = new FolderBrowserDialog();
            
            if (FDB.ShowDialog() == DialogResult.OK && (!(string.IsNullOrEmpty(txtDays.Text))))
            {
                
                DirectoryInfo path = new DirectoryInfo(FDB.SelectedPath);
                string[] extensionFiles = { ".jpg", ".png", ".webp"};
                myfiles = path.GetFiles("*", SearchOption.AllDirectories)
                    .Where(file => extensionFiles.Contains(file.Extension, StringComparer.OrdinalIgnoreCase))
                    .ToArray();

                pictureBoxes = new PictureBox[myfiles.Length];
                labelTextPicture = new Label[myfiles.Length];

                for (int i = 0 ; i < pictureBoxes.Length; i++)
                {
                    try
                    {
                        DateTime test = myfiles[i].CreationTime;
                        long elapsedTime = (DateTime.Now).Ticks - test.Ticks;
                        TimeSpan testSpan = new TimeSpan(elapsedTime);

                        if(testSpan.TotalDays >= Convert.ToInt16(txtDays.Text)) {
                            Console.WriteLine(j);
                            
                            pictureBoxes[j] = new PictureBox();
                            labelTextPicture[j] = new Label();

                            //picture                        
                            pictureBoxes[j].Location = new Point(225, 50);
                            pictureBoxes[j].Size = new Size(50, 50);
                            pictureBoxes[j].SizeMode = PictureBoxSizeMode.StretchImage;
                            pictureBoxes[j].Image = Image.FromFile(myfiles[j].FullName);

                            //label
                            labelTextPicture[j].Text = myfiles[j].Name;

                            flowLayoutPanel2.Controls.Add(labelTextPicture[j]);
                            flowLayoutPanel1.Controls.Add(pictureBoxes[j]);
                            j++;

                        }
                    }
                    catch (OutOfMemoryException ex)
                    {
                        MessageBox.Show($"Erreur lors du chargement de l'image {myfiles[i].Name}: {ex.Message}");
                    }                    
                }                
            }
            else
            {
                MessageBox.Show("Erreur vous avez oublie de remplir les champs !");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < j; i++)
            {
                try
                {
                    pictureBoxes[i].Image.Dispose();
                    pictureBoxes[i].Image = null;
                    labelTextPicture[i].Text = null;
                }
                catch (NullReferenceException err)
                {
                    MessageBox.Show($"Erreur de suppression : Auncune image a été chargé");
                    Console.WriteLine(err.Message);
                    break;
                }
            }         
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Vous etes sur de supprimer les images ?", "Suppresion Image", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                for (int i = 0; i < j; i++)
                {
                    try
                    {
                        pictureBoxes[i].Image.Dispose();
                        File.Delete(myfiles[i].FullName);
                        pictureBoxes[i].Image = null;
                        labelTextPicture[i].Text = null;
                    }
                    catch(NullReferenceException err)
                    {
                        MessageBox.Show($"Erreur de suppression : Auncune image a été chargé");
                        Console.WriteLine(err.Message);
                        break;
                    }
                }          
            }            
        }

        private void txtDays_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }    
}
