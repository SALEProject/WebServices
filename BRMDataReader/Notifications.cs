using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Business.Common;

namespace Business
{
    public class Notifications
    {
        private TBusiness app = null;
        private Session ses = null;

        public Notifications(TBusiness app, Session ses)
        {
            this.app = app;
            this.ses = ses;
        }

        public void SetNotification(int ID_ReceiptUser, int ID_ReceiptAgency, string Subject, string Body, string BodyHTML, bool sysMessage = false)
        {
            int ID_Bursary = ses.ID_Bursary;

            int ID_SenderUser = ses.ID_User;
            int ID_SenderAgency = ses.ID_Agency;
            if (sysMessage)
            {
                ID_SenderUser = 0;
                ID_SenderAgency = 0;
            }

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = ID_Bursary;
            vl_params.Add("@prm_ID_SenderUser").AsInt32 = ID_SenderUser;
            vl_params.Add("@prm_ID_SenderAgency").AsInt32 = ID_SenderAgency;
            vl_params.Add("@prm_ID_ReceiptUser").AsInt32 = ID_ReceiptUser;
            vl_params.Add("@prm_ID_ReceiptAgency").AsInt32 = ID_ReceiptAgency;
            vl_params.Add("@prm_Subject").AsString = Subject;
            vl_params.Add("@prm_Body").AsString = Body;
            vl_params.Add("@prm_BodyHTML").AsString = BodyHTML;
            int int_count_Notification = app.DB.Exec("insert_Notification", "Notifications", vl_params);
        }

    }
}