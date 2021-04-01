using BLL.ModelDAL;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ENTITIES;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Hosting;

namespace BLL.ScienceManagement.ConferenceSponsor
{
    public class ConferenceSponsorExportRepo
    {
        public byte[] ExportRequest(int request_id, int account_id)
        {
            CostRepo costRepo = new CostRepo();
            try
            {
                string fileName = HostingEnvironment.MapPath("/Word_Template/ConferenceSponsor/RequestForm.docx");
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
