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
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private bool IsInfoCompleteAndConsistent()
        {
            return !(string.IsNullOrEmpty(TxEmail.Text) || string.IsNullOrEmpty(TxPassword.Text));
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            if(!IsInfoCompleteAndConsistent())
            {
                MessageBox.Show("معلومات مفقوده,يبدو ان هناك شئ خاطئ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(!TxEmail.Text.EndsWith("@gmail.com",StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("معلومات مفقوده,يجب ان تنتهي كلمه المرور @gmail.comب ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bool isLoginSuccess = await UsersDAL.LoginAsync(TxEmail.Text, TxPassword.Text);
            if ((isLoginSuccess))
            {
                this.Hide();
                Main frm = new Main();
                frm.Show();
                frm.FormClosed += (s, args) => this.Close();
            }
            else
            {
                MessageBox.Show("معلومات تسجيل خاطئه", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
