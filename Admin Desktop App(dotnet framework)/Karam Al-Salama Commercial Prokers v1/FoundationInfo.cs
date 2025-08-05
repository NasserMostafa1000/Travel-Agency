using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SalamTravelDAL;

namespace Karam_Al_Salama_Commercial_Prokers_v1
{
    public partial class FoundationInfo : Form
    {
        public FoundationInfo()
        {
            InitializeComponent();
        }
        private async Task<FoundationInfoDAL.FoundationInfoDTO> GetFoundationInfo()
        {
            return await FoundationInfoDAL.GetFoundationInformationAsync();
        }
        private void putCurrentInfoIntoUi(FoundationInfoDAL.FoundationInfoDTO CurrentInformation)
        {
            TxCallNumber.Text = CurrentInformation.CallNumber;
            TxAboutInfo.Text = CurrentInformation.About;
            TxEmail.Text = CurrentInformation.Email;
            TxWhatsNumber.Text = CurrentInformation.WhastAppNumber;
            TxTikTokUser.Text = CurrentInformation.TikTok;
            TxInstaUser.Text = CurrentInformation.Insta;
            TxFaceBookLink.Text = CurrentInformation.FaceBook;
        }
        private async void FoundationInfo_Load(object sender, EventArgs e)
        {
            //this call data access layer to call the api and return the current info.
            FoundationInfoDAL.FoundationInfoDTO CurrentInformation = await GetFoundationInfo();
            putCurrentInfoIntoUi(CurrentInformation);

        }

        private void BtnUpdate_DragOver(object sender, DragEventArgs e)
        {
            BtnUpdate.BackColor = Color.Green;
        }

        private void BtnUpdate_DragLeave(object sender, EventArgs e)
        {
            BtnUpdate.BackColor = Color.Gray;

        }

        private FoundationInfoDAL.FoundationInfoDTO GetInfoFromUi()
        {
            FoundationInfoDAL.FoundationInfoDTO NewInfo =new FoundationInfoDAL.FoundationInfoDTO();
            NewInfo.WhastAppNumber = TxWhatsNumber.Text;
            NewInfo.CallNumber = TxCallNumber.Text;
            NewInfo.About = TxAboutInfo.Text;
            NewInfo.Email = TxEmail.Text;
            NewInfo.TikTok = TxTikTokUser.Text;
            NewInfo.Insta = TxInstaUser.Text;
            NewInfo.FaceBook = TxFaceBookLink.Text;
            return NewInfo;

        }
        private async Task<bool> UpdateFoundationInformationAsync(FoundationInfoDAL.FoundationInfoDTO NewInfo)
        {
            //this function call data access layer to call the api's
          return  await FoundationInfoDAL.UpdateFoundationInformationAsync(NewInfo);
        }
        private bool IsUiInfoCompleteAndConsistent()
        {
            return !(string.IsNullOrEmpty(TxAboutInfo.Text) || string.IsNullOrEmpty(TxTikTokUser.Text) || string.IsNullOrEmpty(TxFaceBookLink.Text) || string.IsNullOrEmpty(TxInstaUser.Text) || string.IsNullOrEmpty(TxEmail.Text) || string.IsNullOrEmpty(TxCallNumber.Text) || string.IsNullOrEmpty(TxWhatsNumber.Text));
        }
        private async void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                FoundationInfoDAL.FoundationInfoDTO NewInfo = GetInfoFromUi();
                if (IsUiInfoCompleteAndConsistent())
                {
                    bool IsUpdated = await UpdateFoundationInformationAsync(NewInfo);
                    if (IsUpdated)
                    {
                        MessageBox.Show("تم تحديث المعلومات بنجاح", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                        return;

                    }
                    
                }            }
            catch (Exception ex)

            {

                MessageBox.Show("تم العثور علي خطأ  : " + ex.ToString(), "خطأ");
            }

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
