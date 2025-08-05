using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualBasic;
using SalamaTravelDAL;
using SalamTravelDAL;
using static System.Net.WebRequestMethods;

namespace Karam_Al_Salama_Commercial_Prokers_v1
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        private async Task<List<OrdersDAL.OrderDTO>> RefreshAllOrdersAsync()
        {
            var orders = await OrdersDAL.GetOrdersAsync(CurrentUser.Token, CurrentUser.OwnerId);
            return orders.OrderByDescending(o => o.OrderID)
                         .Skip((Convert.ToInt32(btnnext.Tag) - 1) * 50)
                         .Take(50).ToList();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            ReturnTextBoxAndTheLabelToOrders();
            SHowSearchBoxAndLabel();
            SetUpTheDefaultForTheDataGridView();
            dataGridView1.DataSource = await RefreshAllOrdersAsync();
        }
        private void SetUpTheDefaultForTheDataGridView()
        {
            this.WindowState = FormWindowState.Maximized; // تعيين الفورم كحالة مكبرة
            dataGridView1.ContextMenuStrip = CMSOrders;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RightToLeft = RightToLeft.Yes;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Opensans", 15, FontStyle.Bold);
            dataGridView1.DefaultCellStyle.Font= new System.Drawing.Font("Opensans", 12, FontStyle.Bold);
        }
        private async Task<List<OrdersDAL.OrderDTO>> RefreshUnderProcessingOrdersAsync()
        {
            return (await RefreshAllOrdersAsync())
                .Where(e => e.Order_Status == "تحت المعالجة")
                .ToList();
        }
        private async void اطلباتتحتالمعالمجهToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.ContextMenuStrip = CMSOrders;
            ReturnTextBoxAndTheLabelToOrders();
            ShowPaginateButtons();
            SHowSearchBoxAndLabel();
            try
            {
                List<OrdersDAL.OrderDTO> allOrders = await RefreshUnderProcessingOrdersAsync();

                if (allOrders != null && allOrders.Any()) // تحقق إذا كانت القائمة تحتوي على بيانات
                {
                    dataGridView1.DataSource = allOrders;
                }
                else
                {
                    MessageBox.Show(" !تهانينا لا وجود لطلبات تحت المعالجه حتي الان رائع لقد انجزت العمل✔👍", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // في حالة وجود خطأ
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void عرضالكلToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.ContextMenuStrip = CMSOrders;
            try
            {
                List<OrdersDAL.OrderDTO> allOrders = await RefreshAllOrdersAsync();

                if (allOrders != null && allOrders.Any()) // تحقق إذا كانت القائمة تحتوي على بيانات
                {
                    dataGridView1.DataSource = allOrders;
                }
                else
                {
                    MessageBox.Show(" لا وجود لطلبات الان من الافضل تقديم مزيد من الاعلانات لتطبيقك لينتشر بسرعه كبيره😎", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // في حالة وجود خطأ
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task<List<OrdersDAL.OrderDTO>> RefreshAllSuccessOrders()
        {
            return (await RefreshAllOrdersAsync())
                        .Where(e => e.Order_Status == "مكتمل")
                        .ToList();
        }
        private async void الطلباتالناجحهToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.ContextMenuStrip = CMSOrders;
            ReturnTextBoxAndTheLabelToOrders();
            ShowPaginateButtons();
            SHowSearchBoxAndLabel();
            try
            {
                List<OrdersDAL.OrderDTO> allOrders = await RefreshAllSuccessOrders();

                if (allOrders != null && allOrders.Any()) // تحقق إذا كانت القائمة تحتوي على بيانات
                {
                    dataGridView1.DataSource = allOrders;
                }
                else
                {
                    MessageBox.Show("لا وجود لطلبات ناجحه حتي الان😢", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // في حالة وجود خطأ
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task<List<OrdersDAL.OrderDTO>> RefreshAllFailedOrders()
        {
            return (await RefreshAllOrdersAsync())
                        .Where(e => e.Order_Status == "مرفوض")
                        .ToList();
        }
        private async void الطلباتالفاشلهToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.ContextMenuStrip = CMSOrders;
            ReturnTextBoxAndTheLabelToOrders();
            ShowPaginateButtons();
            SHowSearchBoxAndLabel();
            try
            {
                List<OrdersDAL.OrderDTO> allOrders = await RefreshAllFailedOrders();

                if (allOrders != null && allOrders.Any()) // تحقق إذا كانت القائمة تحتوي على بيانات
                {
                    dataGridView1.DataSource = allOrders;
                }
                else
                {
                    MessageBox.Show("رائع لا وجود لطلبات فاشله حتي الان✔", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // في حالة وجود خطأ
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task<List<ClientsDAL.ClientDTO>> RefreshAllClients()
        {
            return await ClientsDAL.FetchClientsDataAsync(CurrentUser.Token, CurrentUser.OwnerId);
        }
        private void HidePaginateButtons()
        {
            btnnext.Enabled = false;
            BtnPrev.Enabled = false;
        }
        private void ShowPaginateButtons()
        {
            btnnext.Enabled = true;
            BtnPrev.Enabled =true;
        }
        private async void عرضالكلToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            ReturnTextBoxAndTheLabelToClients();
            HidePaginateButtons();
            dataGridView1.ContextMenuStrip = CMSClients;
            try
            {
                List<ClientsDAL.ClientDTO> AllClients = await RefreshAllClients();

                if (AllClients != null && AllClients.Any()) // تحقق إذا كانت القائمة تحتوي على بيانات
                {
                    dataGridView1.DataSource = AllClients;
                }
                else
                {
                    MessageBox.Show(" لا وجود لعملاء حتي الان من الافضل تقديم مزيد من الاعلانات لتطبيقك لينتشر بسرعه كبيره😎", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // في حالة وجود خطأ
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task<List<IssuedVisas.IssuedVisasDTO>> RefreshAllIssuedVisas()
        {
            return await IssuedVisas.GetIssuedVisasAsync(CurrentUser.Token, CurrentUser.OwnerId);
        }
        private async void عرضالكلToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            HideSearchBoxAndLabel();
            try
            {
                List<IssuedVisas.IssuedVisasDTO> allOrders = await RefreshAllIssuedVisas();

                if (allOrders != null && allOrders.Any()) // تحقق إذا كانت القائمة تحتوي على بيانات
                {
                    dataGridView1.ContextMenuStrip = null;
                    dataGridView1.DataSource = allOrders;
                }
                else
                {
                    MessageBox.Show(" لا وجود لاقامات صادره حتي الان,يرجي تقديم المزيد من الاعلانات لينتشر موقك بسرعه✔", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // في حالة وجود خطأ
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void عرضاعداداتكToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserSettings settings = new UserSettings();
            settings.ShowDialog();
        }
        private void عرضمعلوماتالمؤسسهعليالموقعToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FoundationInfo FrmNewFoundationInfo = new FoundationInfo();
            FrmNewFoundationInfo.ShowDialog();
        }
        private async void اضفعنصرجديداليالموقعToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasswordInput passwordForm = new PasswordInput();

            if (passwordForm.ShowDialog() == DialogResult.OK)
            {
                string password = passwordForm.EnteredPassword;
                if (await UsersDAL.VerifyPasswordAsync(CurrentUser.Email, password))
                {
                    AddVisa FrmAddVisa = new AddVisa();
                    FrmAddVisa.ShowDialog();
                }
                else
                {
                    MessageBox.Show("كلمه سر خاطئه", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return;
        }
        private async void عرضالعناصروالتحكمبهاToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasswordInput passwordForm = new PasswordInput();

            if (passwordForm.ShowDialog() == DialogResult.OK)
            {
                string password = passwordForm.EnteredPassword;
                bool IsPasswordValid = await UsersDAL.VerifyPasswordAsync(CurrentUser.Email, password);

                if (IsPasswordValid)
                {
                    VisasCtrls visasCtrl = new VisasCtrls();
                    visasCtrl.ShowDialog();
                }
                else
                {
                    MessageBox.Show("كلمه سر خاطئه", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return;
         

        }
        private OrdersDAL.RejectOrBadOrderRequest FillRejectObject(int OrderId, string Reason)
        {
            //declare rejectionReason Object and fill it with Necessary Data
            OrdersDAL.RejectOrBadOrderRequest ReqObj = new OrdersDAL.RejectOrBadOrderRequest();
            ReqObj.Reason = Reason;
            ReqObj.OrderId = OrderId;
            return ReqObj;

        }
        private async Task<bool> IsRejected(OrdersDAL.RejectOrBadOrderRequest RejectOrderOgject)
        {
            return await OrdersDAL.RejectOrderAsync(RejectOrderOgject);
        }
        private async Task SendEmailWithRejectedOrderMessage(string Email, string OrderType)
        {
            await Settings.SendEmail(
                $"إشعار برفض طلب {OrderType} الخاص بك / Notification of Rejected Order {(OrderType == "اصدار" ? "issuance" : "renewal")}",
                "نشكرك على ثقتك بخدماتنا. نود إعلامك بأنه قد تم رفض طلبك من قبل الجهة المعنية في الدولة. نعلم أن هذا قد يسبب لك بعض الإحباط، ولكن نود التأكيد أنه وفقًا للقوانين والإجراءات المعتمدة، فإن المبلغ المدفوع لا يمكن استرداده في هذه الحالة.\n\n" +
                "على الرغم من ذلك، نحن دائمًا هنا لمساعدتك. إذا كان لديك أي استفسار أو رغبة في مناقشة التفاصيل بشكل أكبر، لا تتردد في التواصل مع فريق الدعم لدينا. نحن ملتزمون بتقديم أفضل خدمة ممكنة لك.\n\n" +
                "Thank you for trusting our services. We would like to inform you that your order has been rejected by the relevant authorities in the country. We understand this may be disappointing, but we want to reassure you that, according to the applicable regulations and procedures, the payment made is non-refundable in this case.\n\n" +
                "However, we are always here to assist you. If you have any questions or would like to discuss the details further, please don't hesitate to reach out to our support team. We are committed to providing you with the best possible service.\n\n",
                Email);
        }
        private async void rejectOrder(int ID)
        {
            PasswordInput passwordForm = new PasswordInput();

            if (passwordForm.ShowDialog() == DialogResult.OK)
            {
                string password = passwordForm.EnteredPassword;
                if (await UsersDAL.VerifyPasswordAsync(CurrentUser.Email, password))
                {
                    //get order id
                    //fill order object with order id and reason for the rejection
                    OrdersDAL.RejectOrBadOrderRequest RejectOrderObject = FillRejectObject(ID, "Authorities Rejection.");
                    if (await IsRejected(RejectOrderObject))
                    {
                        //sending Email sms with rejection details 
                        await SendEmailWithRejectedOrderMessage(dataGridView1.CurrentRow.Cells["Email"].Value.ToString(), dataGridView1.CurrentRow.Cells["OrderType"].Value.ToString());
                        MessageBox.Show("تم الرفض و ارسال بريد الكتروني لحساب المستخدم يشمل تفاصيل الرفض   ", " تم بنجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView1.DataSource = await RefreshAllOrdersAsync();
                    }

                }
                else
                {
                    MessageBox.Show("كلمه سر خاطئه", "حطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private async void رفضالطلبToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int OrderId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["OrderID"].Value);
            string OrderStatus = dataGridView1.CurrentRow.Cells["Order_Status"].Value.ToString();
            if (OrderStatus != "تحت المعالجة")
            {
                DialogResult result = MessageBox.Show($"هذا الطلب تم تعينه من قبل الي {OrderStatus} هل انت متأكد من اعاده التعيين", "تنبيه", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {

                    rejectOrder(OrderId);

                }
                else
                {
                    return;
                }
            }
            else
            {
                rejectOrder(OrderId);
            }


        }
        int selectedRowIndex = -1;
        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                if (selectedRowIndex != -1 && selectedRowIndex < dataGridView1.RowCount)
                {
                    dataGridView1.Rows[selectedRowIndex].Selected = false;
                }

                // تحديد الصف الجديد
                if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.RowCount)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[0]; // اختر الخلية الأولى في الصف
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                    selectedRowIndex = e.RowIndex;
                }
            }
        }
        private async Task<bool> IsAccepted(int OrderId)
        {
            return await OrdersDAL.AcceptOrderAsync(OrderId, CurrentUser.OwnerId);
        }
        private async Task SendEmailWithAcceptOrderMessage(string OrderType, string Email)
        {
            string subject = $"إشعار بإتمام طلب {OrderType} الخاص بك / Order Completion Notification {(OrderType == "اصدار" ? "issuance" : "renewal")}";

            string body =
                "نشكرك على ثقتك بخدماتنا. يسرنا أن نعلمك بأن طلبك قد اكتمل بنجاح وتمت معالجة كافة الإجراءات المتعلقة به. نقدر اختيارك لنا، ونؤكد لك أن جميع خطوات المعالجة تمت وفقاً لأعلى معايير الجودة. " +
                "الطلب جاهز الآن ويمكنك متابعة حالته أو استخدامه حسب الحاجة.\n\n" +
                "Thank you for trusting our services. We are pleased to inform you that your order has been successfully completed and all related procedures have been processed. We appreciate your choice, and we assure you that all steps were carried out according to the highest quality standards. " +
                "Your order is now ready, and you can proceed with its use or track its status as needed.";

            try
            {
                // إرسال البريد الإلكتروني باستخدام الإعدادات المناسبة
                await Settings.SendEmail(subject, body, Email);
            }
            catch (Exception ex)
            {
                // التعامل مع الأخطاء في حالة حدوثها
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
        private async void AcceptOrder(int ID)
        {
            PasswordInput passwordForm = new PasswordInput();

            if (passwordForm.ShowDialog() == DialogResult.OK)
            {
                string password = passwordForm.EnteredPassword;
                bool IsPasswordValid = await UsersDAL.VerifyPasswordAsync(CurrentUser.Email, password);
                if (IsPasswordValid)
                {
                    if (await IsAccepted(ID))
                    {
                        await SendEmailWithAcceptOrderMessage(dataGridView1.CurrentRow.Cells["OrderType"].Value.ToString(), dataGridView1.CurrentRow.Cells["Email"].Value.ToString());
                        MessageBox.Show("تم بنجاح وتم ارسال رساله بريديه الي المستخدم بان الطلب قد اكتمل وتم انتهاء المعالجه", " تم بنجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView1.DataSource = await RefreshAllOrdersAsync();
                    }
                }
                else
                {
                    MessageBox.Show("كلمه سر خاطئه", "حطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private async void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["OrderID"].Value);
            string OrderStatus = dataGridView1.CurrentRow.Cells["Order_Status"].Value.ToString();
            if (OrderStatus != "تحت المعالجة")
            {
                DialogResult result = MessageBox.Show($"هذا الطلب تم تعينه من قبل الي {OrderStatus} هل انت متأكد من اعاده التعيين", "تنبيه", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    AcceptOrder(ID);
                }
                else
                {
                    return;
                }
            }
            else
            {
                AcceptOrder(ID);
            }
        }
        private async Task SendEmailWithBadOrderMessage(string OrderType, string Email)
        {
            await Settings.SendEmail(
                $"إشعار برفض طلب {OrderType} الخاص بك / Notification of Rejected Order {(OrderType == "اصدار" ? "issuance" : "renewal")}",
                "نشكرك على ثقتك بخدماتنا. نود إعلامك بأنه قد تم رفض طلبك، لكن لا داعي للقلق، فقد تمت إضافة قيمة الطلب إلى رصيد حسابك في الموقع. يمكنك استخدام الرصيد لإعادة تقديم الطلب بعد ان تراجع الأسباب ومعالجتها. كما يمكنك التواصل مع فريق الدعم الخاص بنا لاسترداد المبلغ بالكامل أو للحصول على المزيد من التفاصيل حول سبب الرفض وكيفية إصلاح المشكلة. نحن هنا لخدمتك، ونسعد بتواصلك معنا في أي وقت.\n\n" +
                "Thank you for trusting our services. We would like to inform you that your order has been rejected, but there is no need to worry as the value of your order has been added to your account balance. You can use this balance to resubmit your order after reviewing and addressing the reasons for rejection. You may also contact our support team to request a full refund or to get more details about the rejection reason and how to resolve it. We are here to assist you, and we are happy to hear from you anytime.",
                Email
            );
        }
        private async void MakeItBadOrder(int ID)
        {
            PasswordInput passwordForm = new PasswordInput();

            if (passwordForm.ShowDialog() == DialogResult.OK)
            {
                string password = passwordForm.EnteredPassword;
                bool IsPasswordValid = await UsersDAL.VerifyPasswordAsync(CurrentUser.Email, password);

                if (IsPasswordValid)
                {
                    string reason = Interaction.InputBox(" ما النواقص في ذالك الطلب لكي نرسل رساله للمستخدم بها:", "Input Required");

                    if (await OrdersDAL.IsItBeenBadOrderSuccessAsync(new OrdersDAL.RejectOrBadOrderRequest { OrderId = ID, Reason = reason }, CurrentUser.Token))
                    {
                        await SendEmailWithBadOrderMessage((dataGridView1.CurrentRow.Cells["OrderType"].Value.ToString()), dataGridView1.CurrentRow.Cells["Email"].Value.ToString());
                        MessageBox.Show("تم بنجاح وتم ارسال رساله بريديه الي المستخدم بان الطلب قد اكتمل وتم انتهاء المعالجه", " تم بنجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView1.DataSource = await RefreshAllOrdersAsync();
                    }
                }

                else
                {
                    MessageBox.Show("كلمه سر خاطئه", "حطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private async void تعيينكنواقصToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string OrderStatus = dataGridView1.CurrentRow.Cells["Order_Status"].Value.ToString();
            int OrderID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["OrderID"].Value);
            if (OrderStatus != "تحت المعالجة")
            {
                DialogResult result = MessageBox.Show($"هذا الطلب تم تعينه من قبل الي {OrderStatus} هل انت متأكد من اعاده التعيين", "تنبيه", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {

                    MakeItBadOrder(OrderID);
                }
                else
                {
                    return;
                }

            }
            else
            {
                MakeItBadOrder(OrderID);
            }
        }
        private void SaveImagePathAsPdf(string imageUrl, string FullName)
        {
            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                    saveFileDialog.Title = "Save PDF File";
                    saveFileDialog.FileName = $"{FullName}.pdf";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string pdfPath = saveFileDialog.FileName;

                        // إنشاء مستند PDF
                        using (FileStream fs = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            Document doc = new Document(PageSize.A4);
                            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                            doc.Open();

                            // تحميل الصورة من الإنترنت باستخدام WebClient أو HttpClient
                            try
                            {
                                // تحميل الصورة من URL باستخدام WebClient
                                using (System.Net.WebClient client = new System.Net.WebClient())
                                {
                                    byte[] imageBytes = client.DownloadData(imageUrl);
                                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imageBytes);
                                    img.Alignment = Element.ALIGN_CENTER;
                                    img.ScaleToFit(500f, 700f); // تحجيم الصورة لتناسب الصفحة
                                    doc.Add(img);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"خطأ في تحميل الصورة من URL: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            doc.Close();
                        }

                        MessageBox.Show($"تم حفظ PDF بنجاح: {pdfPath}", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string PersonalImagePath = Settings.ServerPath + dataGridView1.CurrentRow.Cells["PersonalImagePath"].Value.ToString();
            string FullName = dataGridView1.CurrentRow.Cells["FullName"].Value.ToString();
            SaveImagePathAsPdf(PersonalImagePath,FullName + " "+"Personal Image");
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            string PersonalImagePath = Settings.ServerPath + dataGridView1.CurrentRow.Cells["PassportImagePath"].Value.ToString();
            string FullName = dataGridView1.CurrentRow.Cells["FullName"].Value.ToString();
            SaveImagePathAsPdf(PersonalImagePath, FullName + " " + "Passport Image");
        }

        private async void دفعالديونToolStripMenuItem_Click(object sender, EventArgs e)
        {
            decimal ClientBalance = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Balance"].Value);
            int ClientID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ClientID"].Value);
            if (ClientBalance > 0)
            {
                PasswordInput passwordForm = new PasswordInput();

                if (passwordForm.ShowDialog() == DialogResult.OK)
                {
                    string password = passwordForm.EnteredPassword;
                    bool IsPasswordValid = await UsersDAL.VerifyPasswordAsync(CurrentUser.Email, password);

                    if (password != null && IsPasswordValid)
                    {

                        bool IsDuesSuccessfullyPayed = await InternalBalance.PayDuesAsync(new InternalBalance.PayDuesRequest { ClientId = ClientID, Token = CurrentUser.Token });
                        if (IsDuesSuccessfullyPayed)
                        {
                            MessageBox.Show("تم دفع المستحقات بنجاح", "عمليه ناجحه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dataGridView1.DataSource = await RefreshAllClients();
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("لا وجود لمستحقات في رصيد هذا العميل", " التباس!!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private async void تحديثالرصيدToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ClientID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ClientId"].Value);
            PasswordInput passwordForm = new PasswordInput();

            if (passwordForm.ShowDialog() == DialogResult.OK)
            {
                string password = passwordForm.EnteredPassword;
                bool IsPasswordValid = await UsersDAL.VerifyPasswordAsync(CurrentUser.Email, password);

                if (IsPasswordValid)
                {
                    string amount = Interaction.InputBox("ادخل الرصيد الجديد", "تحديث الرصيد");
                    if (decimal.TryParse(amount, out decimal newBalance))
                    {
                        bool IsBalanceUpdatedSuccess = await  InternalBalance.UpdateInternalBalanceAsync(new InternalBalance.PayAndUpdateInternalBalanceRequest { Token = CurrentUser.Token, ClientId = ClientID, Amount = decimal.Parse(amount) });
                        if (IsBalanceUpdatedSuccess)
                        {
                            MessageBox.Show("تم التحديث بنجاح", "ناجح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dataGridView1.DataSource = await RefreshAllClients();
                        }
                        else {
                            MessageBox.Show("قيمه ماليه خاطئه لا تحاول ادراج 0 او قيمه سالبه", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }

                    }
                    else
                    {
                        MessageBox.Show("قيمه ماليه خاطئه", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

                }
                else
                {
                    MessageBox.Show("كلمه السر خاطئه", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(LbSearch.Text=="Order Id ?")
            {
                if (string.IsNullOrEmpty(TxOrderId.Text))
                {
                    dataGridView1.DataSource = await RefreshAllOrdersAsync();
                    return;
                }
                try
                {
                    List<OrdersDAL.OrderDTO> OrderToFind = new List<OrdersDAL.OrderDTO>();
                    OrderToFind.Add(await OrdersDAL.GetOrderByIdAsync(CurrentUser.Token, CurrentUser.OwnerId, int.Parse(TxOrderId.Text)));
                    dataGridView1.DataSource = OrderToFind;

                }
                catch (Exception)
                {
                    MessageBox.Show("لا يوجد طلب بهذا المعرف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(TxOrderId.Text))
                {
                    dataGridView1.DataSource = await RefreshAllClients();
                    return;
                }
                try
                {
                    List<ClientsDAL.ClientDTO> ClientsToFind = new List<ClientsDAL.ClientDTO>();
                    ClientsToFind=await ClientsDAL.FindClientByNameAsync(TxOrderId.Text,CurrentUser.Token);
                    dataGridView1.DataSource = ClientsToFind;

                }
                catch (Exception)
                {
                    return;
                }
            }
           
        }
        private void ReturnTextBoxAndTheLabelToClients()
        {
            LbSearch.Text = "Client Name ?";
        }
        private void ReturnTextBoxAndTheLabelToOrders()
        {
            LbSearch.Text = "Order Id ?";
        }
        private void HideSearchBoxAndLabel()
        {
            TxOrderId.Visible = false;
            LbSearch.Visible = false;
        }
        private void SHowSearchBoxAndLabel()
        {
            TxOrderId.Visible = true;
            LbSearch.Visible = true;
        }
        private async void كلالطلباتToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.ContextMenuStrip = CMSOrders;
            ReturnTextBoxAndTheLabelToOrders();
            ShowPaginateButtons();
            SHowSearchBoxAndLabel();
            dataGridView1.DataSource = await RefreshAllOrdersAsync();
        }

        private void العملاءToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private async Task<List<OrdersDAL.OrderDTO>>  GetAllBadOrdersAsync()
        {
            return (await RefreshAllOrdersAsync())
                        .Where(e => e.Order_Status == "نواقص")
                        .ToList();
        }
        private async void نواقصToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReturnTextBoxAndTheLabelToOrders();
            ShowPaginateButtons();
            dataGridView1.ContextMenuStrip = CMSOrders;

            dataGridView1.DataSource =await GetAllBadOrdersAsync();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            btnnext.Tag = (int.Parse(btnnext.Tag.ToString())) + 1;
             dataGridView1.DataSource= await RefreshAllOrdersAsync();

        }

        private async void BtnPrev_Click(object sender, EventArgs e)
        {
            btnnext.Tag = (int.Parse(btnnext.Tag.ToString())) - 1;

            dataGridView1.DataSource = await RefreshAllOrdersAsync();
        }
    }
}


