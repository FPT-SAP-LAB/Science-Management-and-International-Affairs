using BLL.ModelDAL;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.Conference;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Hosting;

namespace BLL.ScienceManagement.ConferenceSponsor
{
    public class ConferenceSponsorExportRepo
    {
        //private ScienceAndInternationalAffairsEntities db;
        public byte[] ExportRequest(int request_id, int account_id = 0)
        {
            //db = new ScienceAndInternationalAffairsEntities();
            CostRepo costRepo = new CostRepo();

            List<Cost> Costs = costRepo.GetList(request_id);
            ConferenceSponsorDetailRepo DetailRepos = new ConferenceSponsorDetailRepo();
            JObject @object = JObject.Parse(DetailRepos.GetDetailPageGuest(request_id, 1, account_id));
            //Person person = db.Profiles.Where(x => x.account_id == account_id).Select(x => x.Person).FirstOrDefault();
            ConferenceDetail Conference = @object["Conference"].ToObject<ConferenceDetail>();
            ConferenceParticipantExtend Participants = @object["Participants"].ToObject<List<ConferenceParticipantExtend>>()[0];
            try
            {
                string fileName = HostingEnvironment.MapPath("/Word_Template/ConferenceSponsor/RequestForm.docx");
                byte[] byteArray = System.IO.File.ReadAllBytes(fileName);
                using (var stream = new MemoryStream())
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                    using (var doc = WordprocessingDocument.Open(stream, true))
                    {
                        ////////////////////////////////////replace/////////////////////////////////
                        string docText = null;
                        using (StreamReader sr = new StreamReader(doc.MainDocumentPart.GetStream()))
                        {
                            docText = sr.ReadToEnd();
                        }

                        Regex regexText = new Regex("@Date");
                        DateTime Now = DateTime.Now;
                        docText = regexText.Replace(docText, "ngày " + Now.Day + " tháng " + Now.Month + " năm " + Now.Year);

                        regexText = new Regex("@Name");
                        docText = regexText.Replace(docText, Participants.FullName);

                        regexText = new Regex("@OfficeName");
                        docText = regexText.Replace(docText, Conference.SpecializationName + " " + Participants.OfficeName);

                        regexText = new Regex("@ConferenceName");
                        docText = regexText.Replace(docText, Conference.ConferenceName);

                        regexText = new Regex("@TimeStart");
                        docText = regexText.Replace(docText, Conference.TimeStart.ToString("dd/MM/yyyy"));

                        regexText = new Regex("@TimeEnd");
                        docText = regexText.Replace(docText, Conference.TimeEnd.ToString("dd/MM/yyyy"));

                        regexText = new Regex("@CountryName");
                        docText = regexText.Replace(docText, Conference.CountryName);

                        regexText = new Regex("@Total");
                        docText = regexText.Replace(docText, String.Format("{0:n0}", Costs.Sum(x => x.total)));

                        regexText = new Regex("@QsUniversity");
                        docText = regexText.Replace(docText, Conference.QsUniversity);

                        regexText = new Regex("@CoUnit");
                        docText = regexText.Replace(docText, Conference.Co_organizedUnit ?? "");

                        regexText = new Regex("@Website");
                        docText = regexText.Replace(docText, Conference.Website);

                        regexText = new Regex("@KeynoteSpeaker");
                        docText = regexText.Replace(docText, Conference.KeynoteSpeaker);

                        regexText = new Regex("@Attendance");
                        docText = regexText.Replace(docText, Conference.AttendanceStart.ToString("dd/MM/yyyy") + " - " + Conference.AttendanceEnd.ToString("dd/MM/yyyy"));

                        using (StreamWriter sw = new StreamWriter(doc.MainDocumentPart.GetStream(FileMode.Create)))
                        {
                            sw.Write(docText);
                        }
                        return stream.ToArray();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
        public byte[] ExportAppointment(int request_id, int account_id)
        {
            CostRepo costRepo = new CostRepo();
            try
            {
                string fileName = HostingEnvironment.MapPath("/Word_Template/ConferenceSponsor/AppointmentForm.docx");
                byte[] byteArray = System.IO.File.ReadAllBytes(fileName);
                using (var stream = new MemoryStream())
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                    using (var doc = WordprocessingDocument.Open(stream, false))
                    {
                        ////////////////////////////////////replace/////////////////////////////////
                        string docText = null;
                        using (StreamReader sr = new StreamReader(doc.MainDocumentPart.GetStream()))
                        {
                            docText = sr.ReadToEnd();
                        }

                        Regex regexText = new Regex("@Day");
                        docText = regexText.Replace(docText, DateTime.Now.Day.ToString());

                        regexText = new Regex("@Month");
                        docText = regexText.Replace(docText, DateTime.Now.Month.ToString());

                        regexText = new Regex("@Year");
                        docText = regexText.Replace(docText, DateTime.Now.Year.ToString());

                        using (StreamWriter sw = new StreamWriter(doc.MainDocumentPart.GetStream(FileMode.Create)))
                        {
                            sw.Write(docText);
                        }
                        /////////////////////////////////////////////////////////////////////
                        Table table =
                        doc.MainDocumentPart.Document.Body.Elements<Table>().ElementAt(1);

                        List<Cost> Costs = costRepo.GetList(request_id);

                        for (int i = 0; i < Costs.Count; i++)
                        {
                            Cost cost = Costs[i];
                            TableRow tr = new TableRow();

                            TableCell tc1 = new TableCell();
                            tc1.Append(new Paragraph(new Run(new Text((i + 1) + " - " + cost.content))));
                            tr.Append(tc1);

                            TableCell tc2 = new TableCell();
                            tc2.Append(new Paragraph(new Run(new Text(cost.sponsoring_organization))));
                            tr.Append(tc2);

                            TableCell tc3 = new TableCell();
                            tc3.Append(new Paragraph(new Run(new Text(cost.detail))));
                            tr.Append(tc3);

                            TableCell tc4 = new TableCell();
                            tc4.Append(new Paragraph(new Run(new Text(cost.total.ToString()))));
                            tr.Append(tc4);

                            table.Append(tr);
                        }
                        stream.Position = 0;
                        string handle = Guid.NewGuid().ToString();
                        return stream.ToArray();
                        //    return new File(output, "application/vnd.ms-word", "Đơn-đề-nghị-hỗ-trợ-HNKH-Le-Dinh-Duy.docx");
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
