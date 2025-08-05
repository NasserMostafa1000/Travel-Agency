using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SalamaTravelDAL;
using SalamTravelDAL;

namespace Karam_Al_Salama_Commercial_Prokers_v1
{
    public partial class CtrlIVisa : UserControl
    {
        public CtrlIVisa()
        {
            InitializeComponent();
        }
        public string VisaName
        {
            get { return lblName.Text; }
            set { lblName.Text = value.ToString(); }
        }
        public decimal VisaIssuedPrice
        {
            get { return decimal.Parse(lblIssuedPrice.Text); }
            set { lblIssuedPrice.Text = value.ToString(); }
        }
        public decimal VisaRenewalPrice
        {
            get { return decimal.Parse(lblRenewalPrice.Text); }
            set { lblRenewalPrice.Text = value.ToString(); }
        }
        public string AboutVIsa
        {
            get { return textBoxAbout.Text; }
            set { textBoxAbout.Text = value.ToString(); }
        }
        public byte VisaPeriod
        {
            get { return byte.Parse(lblPeriod.Text); }
            set { lblPeriod.Text = value.ToString(); }

        }
        public string ImagePath = "";
        public int VisaId;
        public void AddVisaImageToPictureBox()
        {
            try
            {
                VisaImage.ImageLocation = (ImagePath);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message.ToString());
            }
        }
        private void HideDetailsBelongsToUpdateMode()
        {
            buttonSave.Visible = false;
            LkChangeVisaImage.Visible = false;
            textBoxAbout.Enabled = false;
            lblIssuedPrice.Enabled = false;
            lblName.Enabled = false;
            lblRenewalPrice.Enabled = false;
            lblPeriod.Enabled = false;
            this.Height = 400;


        }
        private void ShowDetailsBelongsToUpdateMode()
        {
            buttonSave.Visible = true;
            LkChangeVisaImage.Visible = true;
            textBoxAbout.Enabled = true;
            lblIssuedPrice.Enabled = true;
            lblName.Enabled = true;
            lblRenewalPrice.Enabled = true;
            lblPeriod.Enabled = true;
            this.Height =500;
        }
        private void CtrlIVisa_Load(object sender, EventArgs e)
        {
             HideDetailsBelongsToUpdateMode();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ShowDetailsBelongsToUpdateMode();
        } 
        private bool IsInfoCompleteAndConsistent()
        {
            return !(string.IsNullOrEmpty(lblName.Text) ||
                     !decimal.TryParse(lblIssuedPrice.Text, out decimal p) ||
                     !decimal.TryParse(lblRenewalPrice.Text, out decimal P) ||
                     string.IsNullOrEmpty(textBoxAbout.Text) || string.IsNullOrEmpty(lblPeriod.Text));
        }
        private VisasDAL.VisaDTO GetVisaInfoFromUI()
        {
            VisasDAL.VisaDTO VisaToUpdate = new VisasDAL.VisaDTO();
            VisaToUpdate.VisaId = this.VisaId;
            VisaToUpdate.MoreDetails = this.AboutVIsa;
            VisaToUpdate.RenewalPrice = this.VisaRenewalPrice;
            VisaToUpdate.Period = this.VisaPeriod;
            VisaToUpdate.Name = this.VisaName;
            VisaToUpdate.IssuancePrice = this.VisaIssuedPrice;
            VisaToUpdate.ImagePath = "";
            return VisaToUpdate;
        }
        private async void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {

            if (IsInfoCompleteAndConsistent())
            {
                if (await VisasDAL.UpdateVisaAsync(GetVisaInfoFromUI(), CurrentUser.Token))
                {
                    MessageBox.Show("تم تحديث معلومات الفيزه بنجاح", "طلب ناجح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HideDetailsBelongsToUpdateMode();

                }
            }
            else
            {
                MessageBox.Show("هناك معلومات مفقوده او خاطئه", "طلب فاشل'", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }
        private async void LkChangeVisaImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // تحديد أنواع الملفات المسموح بها
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                openFileDialog.Title = "Select an Image";

                // فتح نافذة اختيار الملفات
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        VisaImage.Image = new Bitmap(openFileDialog.FileName);
                        using (var memoryStream = new MemoryStream())
                        {
                            VisaImage.Image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                            var result = await VisasDAL.UpdateVisaImageAsync(memoryStream, CurrentUser.Token, this.VisaId);
                            VisaImage.Tag = result.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("لم يتم اختيار أي صورة!");
                }
            }
        }
        private void label2_Click(object sender, EventArgs e)
        {
            ShowDetailsBelongsToUpdateMode();
        }
    }
}
