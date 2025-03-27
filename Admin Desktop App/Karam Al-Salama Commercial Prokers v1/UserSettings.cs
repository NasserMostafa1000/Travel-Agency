using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using SalamaTravelDAL;
using SalamTravelDAL;

namespace Karam_Al_Salama_Commercial_Prokers_v1
{
    public partial class UserSettings : Form
    {
        public UserSettings()
        {
            InitializeComponent();
        }

        private void button1_DragOver(object sender, DragEventArgs e)
        {
            btnUpdate.BackColor = Color.Green;
        }

        private void btnUpdate_DragLeave(object sender, EventArgs e)
        {
            btnUpdate.BackColor = Color.Gray;
        }
        private void SetCurrentEmailIntoUI()
        {
            TxEmail.Text = CurrentUser.Email;
            //this for delete any char cause using password char Property
            TxPassowrd.Text = "";
            TxReWritePassword.Text = "";
        }
        private bool IsUiInfoIsCompleteAndConsistent()
        {
            bool Result = (string.IsNullOrEmpty(TxEmail.Text) || string.IsNullOrEmpty(TxPassowrd.Text) || string.IsNullOrEmpty(TxReWritePassword.Text));
              return !Result;
       }
        private void UserSettings_Load(object sender, EventArgs e)
        {
            TxEmail.Enabled = false;
            SetCurrentEmailIntoUI();
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (IsUiInfoIsCompleteAndConsistent())
            {
                string input = Interaction.InputBox("ادخل كلمه السر الحاليه :", "Input Required");

                if (input != null)
                {
                    bool IsPasswordValid = await UsersDAL.VerifyPasswordAsync(CurrentUser.Email, input);

                    if (!IsPasswordValid)
                    {
                        MessageBox.Show("كلمة المرور الحالية غير صحيحة.");
                    }

                    // التحقق من أن إعادة كتابة كلمة المرور تتطابق مع الكلمة الأساسية
                    if (TxPassowrd.Text != TxReWritePassword.Text)
                    {
                        MessageBox.Show("كلمة المرور الجديدة وإعادة كتابتها غير متطابقتين.");
                    }

                    // التحقق من أن البريد الإلكتروني ينتهي بـ @gmail.com
                    if (!TxEmail.Text.EndsWith("@gmail.com"))
                    {
                        MessageBox.Show("البريد الإلكتروني يجب أن ينتهي بـ @gmail.com.");
                    }

                    // إذا لم يكن هناك أخطاء، يتم تحديث المعلومات
                    
                        bool isUpdated = await UsersDAL.UpdateUserInfoAsync(CurrentUser.OwnerId,CurrentUser.Token, TxEmail.Text ,TxPassowrd.Text);

                        if (isUpdated)
                        {
                            CurrentUser.Email = TxEmail.Text;
                            MessageBox.Show("تم تحديث المعلومات بنجاح", "إشعار", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                        }
                        else
                        {
                            MessageBox.Show("حدث خطأ أثناء تحديث المعلومات. حاول مرة أخرى.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                else
                {
                    MessageBox.Show("معلومات مفقوده", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }

        }
    }
}
