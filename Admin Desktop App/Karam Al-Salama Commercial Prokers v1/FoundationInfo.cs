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
            return NewInfo;

        }
        private async Task<bool> UpdateFoundationInformationAsync(FoundationInfoDAL.FoundationInfoDTO NewInfo)
        {
            //this function call data access layer to call the api's
          return  await FoundationInfoDAL.UpdateFoundationInformationAsync(NewInfo);
        }
        private bool IsUiInfoCompleteAndConsistent()
        {
            return !(string.IsNullOrEmpty(TxAboutInfo.Text) || string.IsNullOrEmpty(TxEmail.Text) || string.IsNullOrEmpty(TxCallNumber.Text) || string.IsNullOrEmpty(TxWhatsNumber.Text));
        }
        private async void BtnUpdate_Click(object sender, EventArgs e)
        {
           FoundationInfoDAL.FoundationInfoDTO NewInfo = GetInfoFromUi();
            if(IsUiInfoCompleteAndConsistent())
            {
                bool IsUpdated = await UpdateFoundationInformationAsync(NewInfo);
                if (IsUpdated)
                {
                    MessageBox.Show("تم تحديث المعلومات بنجاح", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    return;

                }
                MessageBox.Show("تم العثور علي خطأ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            MessageBox.Show("تم العثور علي معلومات فارغه", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);


        }
    }
}
