using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SalamaTravelDAL;
using SalamTravelDAL;
using static System.Net.WebRequestMethods;

namespace Karam_Al_Salama_Commercial_Prokers_v1
{
    public partial class VisasCtrls : Form
    {
        public VisasCtrls()
        {
            InitializeComponent();
        }
        private void AddSingleVisaCtrl(VisasDAL.VisaDTO VisaInfo)
        {
            CtrlIVisa NewVisaCtrl = new CtrlIVisa();
            NewVisaCtrl.AboutVIsa = VisaInfo.MoreDetails;
            NewVisaCtrl.VisaName = VisaInfo.Name;
            NewVisaCtrl.VisaId = VisaInfo.VisaId;
            NewVisaCtrl.ImagePath = Settings.ServerPath + VisaInfo.ImagePath;
            NewVisaCtrl.VisaIssuedPrice = VisaInfo.IssuancePrice;
            NewVisaCtrl.VisaRenewalPrice = VisaInfo.RenewalPrice;
            NewVisaCtrl.VisaPeriod = VisaInfo.Period;
            NewVisaCtrl.AddVisaImageToPictureBox();
            flowLayoutPanel1.Controls.Add(NewVisaCtrl);
        }
        private async void SetAllVisasControls()
        {
            List<VisasDAL.VisaDTO> All =await VisasDAL.GetVisasAsync();
            foreach(VisasDAL.VisaDTO dto in All)
            {
                AddSingleVisaCtrl(dto);
            }
        }
        private void VisasCtrls_Load(object sender, EventArgs e)
        {
            SetAllVisasControls();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
