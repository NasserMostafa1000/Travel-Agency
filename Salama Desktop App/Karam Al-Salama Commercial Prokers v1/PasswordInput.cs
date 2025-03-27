using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Karam_Al_Salama_Commercial_Prokers_v1
{
    public partial class PasswordInput : Form
    {
        public PasswordInput()
        {
            InitializeComponent();
        }

        private void PasswordInput_Load(object sender, EventArgs e)
        {
            foreach(Control c in this.Controls)
            {
                if(c.Name!="BtnOk"|| c.Name != "BtnCancel"||c.Name != "TxPass"|| c.Name != "label1")
                {
                    c.Visible = true;
                }
            }
        }
        public string EnteredPassword { get; private set; }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            EnteredPassword = TxPass.Text; // تخزين كلمة المرور المدخلة
            this.DialogResult = DialogResult.OK; // تحديد النتيجة إلى OK
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; // تحديد النتيجة إلى Cancel
            this.Close(); // غلق الفورم
        }
    }
}
