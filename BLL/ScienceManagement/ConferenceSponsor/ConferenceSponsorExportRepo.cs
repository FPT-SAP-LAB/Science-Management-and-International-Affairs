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
        public byte[] ExportRequest(int request_id, int account_id = 0)
        {
            CostRepo costRepo = new CostRepo();

            List<Cost> Costs = costRepo.GetList(request_id);
            ConferenceSponsorDetailRepo DetailRepos = new ConferenceSponsorDetailRepo();
            JObject @object = JObject.Parse(DetailRepos.GetDetailPageGuest(request_id, 1, account_id));
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
                        docText = regexText.Replace(docText, string.Format("{0:n0}", Costs.Sum(x => x.total)));

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
        public byte[] ExportAppointment(int request_id, int account_id = 0)
        {
            CostRepo costRepo = new CostRepo();

            List<Cost> Costs = costRepo.GetList(request_id);
            ConferenceSponsorDetailRepo DetailRepos = new ConferenceSponsorDetailRepo();
            JObject @object = JObject.Parse(DetailRepos.GetDetailPageGuest(request_id, 1, account_id));
            ConferenceDetail Conference = @object["Conference"].ToObject<ConferenceDetail>();
            ConferenceParticipantExtend Participants = @object["Participants"].ToObject<List<ConferenceParticipantExtend>>()[0];
            try
            {
                string fileName = HostingEnvironment.MapPath("/Word_Template/ConferenceSponsor/AppointmentForm.docx");
                byte[] byteArray = System.IO.File.ReadAllBytes(fileName);
                using (var stream = new MemoryStream())
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                    using (var doc = WordprocessingDocument.Open(stream, true))
                    {
                        ////////////////////////////////////replace/////////////////////////////////
                        Table table =
                        doc.MainDocumentPart.Document.Body.Elements<Table>().ElementAt(1);
                        TableRow template = table.Elements<TableRow>().ElementAt(1);

                        for (int i = 0; i < Costs.Count; i++)
                        {
                            Cost cost = Costs[i];
                            TableRow tr = template.Clone() as TableRow;

                            tr.ChildElements[0].InnerXml = tr.ChildElements[0].InnerXml.Replace("@Content", (i + 1) + "- " + cost.content);
                            tr.ChildElements[1].InnerXml = tr.ChildElements[1].InnerXml.Replace("@Sponsore", cost.sponsoring_organization);
                            tr.ChildElements[2].InnerXml = tr.ChildElements[2].InnerXml.Replace("@Detail", cost.detail);
                            tr.ChildElements[3].InnerXml = tr.ChildElements[3].InnerXml.Replace("@SubTotal", string.Format("{0:n0}", cost.total));

                            table.InsertAfter(tr, table.Elements<TableRow>().ElementAt(i));
                        }
                        table.RemoveChild(template);
                        doc.MainDocumentPart.Document.Save();
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

                        string PositionName = PositionRepo.GetPositionNameByProfileCode(Participants.ID, 1);
                        if (PositionName == null)
                        {
                            if (Participants.TitleID == 1 || Participants.TitleID == 2)
                                docText = docText.Replace("@PositionName", "Chức vụ: " + Participants.TitleName);
                            else
                                docText = docText.Replace("@PositionName", "");
                        }
                        else
                            docText = docText.Replace("@PositionName", "Chức vụ: " + PositionName);

                        regexText = new Regex("@OfficeName");
                        docText = regexText.Replace(docText, Conference.SpecializationName + " " + Participants.OfficeName);

                        regexText = new Regex("@ConferenceName");
                        docText = regexText.Replace(docText, Conference.ConferenceName);

                        regexText = new Regex("@CountryName");
                        docText = regexText.Replace(docText, Conference.CountryName);

                        regexText = new Regex("@AttendanceStart");
                        docText = regexText.Replace(docText, Conference.AttendanceStart.ToString("dd/MM/yyyy"));

                        regexText = new Regex("@AttendanceEnd");
                        docText = regexText.Replace(docText, Conference.AttendanceEnd.ToString("dd/MM/yyyy"));

                        regexText = new Regex("@Total");
                        docText = regexText.Replace(docText, string.Format("{0:n0}", Costs.Sum(x => x.total)));

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
    }
}
