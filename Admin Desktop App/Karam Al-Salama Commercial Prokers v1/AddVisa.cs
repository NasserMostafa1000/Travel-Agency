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
    public partial class AddVisa : Form
    {
        public AddVisa()
        {
            InitializeComponent();
        }
        private async void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.Enabled = false;
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
                        // عرض الصورة في PictureBox
                        pictureBox1.Image = new Bitmap(openFileDialog.FileName);

                        // تحويل الصورة إلى MemoryStream
                        using (var memoryStream = new MemoryStream())
                        {
                            pictureBox1.Image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

                            // استدعاء الدالة مع الصورة
                            var result = await VisasDAL.UploadVisaImageAsync(memoryStream, CurrentUser.Token);

                            // تخزين النتيجة في Tag وعرضها
                            pictureBox1.Tag = result.ToString();
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
        private bool IsInfoCompleteAndConsistent()
        {
            return !(pictureBox1.Image == null || string.IsNullOrEmpty(TxVisaName.Text) ||
                     !decimal.TryParse(TxIssuedPrice.Text, out decimal p) ||
                     !decimal.TryParse(TxRenewalPrice.Text, out decimal P) ||
                     string.IsNullOrEmpty(AboutVisa.Text) || VisaPeriod.Value == 0);
        }
        private VisasDAL.VisaDTO GetVisaInfoFromUi()
        {
            VisasDAL.VisaDTO NewVisa = new VisasDAL.VisaDTO();
            NewVisa.ImagePath = pictureBox1.Tag.ToString();
            NewVisa.Period =byte.Parse(VisaPeriod.Value.ToString());
            NewVisa.MoreDetails = AboutVisa.Text;
            NewVisa.IssuancePrice =decimal.Parse(TxIssuedPrice.Text);
            NewVisa.RenewalPrice = decimal.Parse(TxRenewalPrice.Text);
            NewVisa.Name = TxVisaName.Text;
            return NewVisa;

        }
        private async void BtnAdd_Click(object sender, EventArgs e)
        {
            if(IsInfoCompleteAndConsistent())
            {
                VisasDAL.VisaDTO NewVisa = GetVisaInfoFromUi();
                bool IsAdded = await VisasDAL.AddNewVisaAsync(NewVisa, CurrentUser.Token);
            if(IsAdded)
                {
                    MessageBox.Show("تمت الاضافه بنجاح", "ناجح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("خطا من السيرفر", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("هناك معلومات مفقوده", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

        }

        private void AddVisa_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
    }

