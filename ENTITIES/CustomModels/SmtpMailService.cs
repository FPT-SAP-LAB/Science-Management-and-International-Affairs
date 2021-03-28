using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace ENTITIES.CustomModels
{
    public static class SmtpMailService
    {
        public static readonly string From = "fpt.saplab@gmail.com";
        public static readonly string Name = "SAP-LAB";
        private static readonly string Password = "fptsaplab123";
        public static AlertModal<string> Send(string To, string Subject, string BodyText, List<string> CC = null)
        {
            return Send(new List<string> { To }, Subject, BodyText, CC);
        }
        public static AlertModal<string> Send(List<string> To, string Subject, string BodyText, List<string> CC = null)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(From, Password)
                };
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(From, Name),
                    Body = BodyText,
                    Subject = Subject
                };

                To.ForEach(x => mail.To.Add(new MailAddress(x)));

                if (CC != null)
                    CC.ForEach(x => mail.CC.Add(x));

                smtpClient.Send(mail);
                return new AlertModal<string>(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<string>(false);
            }
        }
        public static AlertModal<string> SendUpdateRequest(string To, string SubSubject, string BodyText, int RequestType, List<string> CC = null)
        {
            return Send(new List<string> { To }, SubSubject, BodyText, CC);
        }
        public static AlertModal<string> SendUpdateRequest(List<string> To, string BodyText, int RequestType, List<string> CC = null)
        {
            string Subject = "Đề nghị ";
            switch (RequestType)
            {
                case 1:
                    Subject += "tham dự hội nghị";
                    break;
                case 2:
                    Subject += "khen thưởng khoa học";
                    break;
                case 3:
                    Subject += "khen thưởng bằng sáng chế";
                    break;
                default:
                    return new AlertModal<string>(false, "Loại đề nghị không tồn tại");
            }
            Subject += " của bạn có cập nhật mới";
            return Send(To, Subject, BodyText, CC);
        }
    }
}
