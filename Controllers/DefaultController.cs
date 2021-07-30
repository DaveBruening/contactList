using ASPNetFrmwrkRapid.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
namespace ASPNetFrmwrkRapid.Controllers {
    public class DefaultController : Controller    {
        // GET: Default
        List<contact> lesContact = new List<contact>();
        public ActionResult displayEm() {
            return View(lesContact);
        }
        //public string Index() { 
        public ActionResult Index(string sortOrder) {
            /*switch (sortOrder) {
                case "": ViewBag.sortEm = "compAsc"; break;
                case null: ViewBag.sortEm = "compAsc"; break;
                case 
            } */
            //ViewBag.sortEm = (String.IsNullOrEmpty(sortOrder) || sortOrder=="desc") ? "asc" : "desc";
            //ViewBag.sortEm = String.IsNullOrEmpty(sortOrder) ? "companyDesc" : sortOrder;
            //2021-7-29 19:54 commented above, added below:
            ViewBag.sortEm = String.IsNullOrEmpty(sortOrder) ? "" : sortOrder;
            addData(sortOrder);
            //sbd.Append(System.DateTime.Now.Year.ToString());
            /*foreach (contact cntc in lesContact)
                sbd.Append($"{cntc.contactName} - {cntc.yearsInBusiness}<br/>"); */
            
            return View(lesContact);
        }
        public void addData(string sortOrder) {
            //TextReader trd = new StreamReader(@"C:\Users\Owner\Downloads\code_exercise_inputs\code_exercise_inputs\comma.txt");
            //TextReader trd = new StreamReader("~/Model/comma.txt");
            TextReader trd = new StreamReader(@"C:\Users\Owner\Documents\Visual Studio 2019\ASPNetFrmwrkRapid\Models\comma.txt");
            StringBuilder sbd = new StringBuilder();
            contact cntct;
            string[] fieldAr = new string[10];
            int yrsNBsns = 0;
            while (trd.Peek() > 0) {
                fieldAr = trd.ReadLine().Split(',');
                cntct = new contact {
                    companyName = fieldAr[0], yearsInBusiness = Int32.Parse(fieldAr[3]),
                    contactName = fieldAr[1].Trim(), phone = fieldAr[2], email = fieldAr[4]
                };
                lesContact.Add(cntct);
            }
            //return trd.ReadLine();
            //return View();
            //return fieldAr[0];
            //return lesContact.Count.ToString();
            //foreach (contact cntc in lesContact)
            //    sbd.Append(cntc.contactName + ", ");
            //return sbd.ToString();
            //import other 2 .txt
            trd.Close();
            trd = new StreamReader(@"C:\Users\Owner\Documents\Visual Studio 2019\ASPNetFrmwrkRapid\Models\hash.txt");
            while (trd.Peek() > 0) {
                fieldAr = trd.ReadLine().Split('#');
                //sbd.Append(fieldAr[1] + ", ");
                yrsNBsns = System.DateTime.Now.Year - Int32.Parse(fieldAr[1]);
                cntct = new contact {
                    companyName = fieldAr[0], yearsInBusiness = yrsNBsns,
                    contactName = fieldAr[2].Trim(), phone = fieldAr[3]
                };
                lesContact.Add(cntct);
            }
            trd.Close();
            trd = new StreamReader(@"C:\Users\Owner\Documents\Visual Studio 2019\ASPNetFrmwrkRapid\Models\hyphen.txt");
            while (trd.Peek() > 0) {
                fieldAr = trd.ReadLine().Split('-');
                yrsNBsns = System.DateTime.Now.Year - Int32.Parse(fieldAr[1]);
                cntct = new contact { companyName = fieldAr[0].Trim(), contactName = fieldAr[4] + " " + fieldAr[5]
                    , yearsInBusiness = yrsNBsns, phone = fieldAr[2], email = fieldAr[3]
                };
                lesContact.Add(cntct);
            }
            if (sortOrder == "companyAsc")
                lesContact = lesContact.OrderBy(c => c.companyName).ToList<contact>();
            else if (sortOrder == "companyDesc")
                lesContact = lesContact.OrderByDescending(c => c.companyName).ToList<contact>();
            else if (sortOrder == "contactAsc")
                lesContact = lesContact.OrderBy(c => c.contactName).ToList<contact>();
            else if (sortOrder == "contactDesc")
                lesContact = lesContact.OrderByDescending(c => c.contactName).ToList<contact>();
            else if (sortOrder == "yearAsc")
                lesContact = lesContact.OrderBy(c => c.yearsInBusiness).ThenBy(d=>d.companyName).ToList<contact>();
            else if (sortOrder == "yearDesc")
                lesContact = lesContact.OrderByDescending(c => c.yearsInBusiness).ThenByDescending(d=>d.companyName).ToList<contact>();
        }
        public FileContentResult exportToCSV(string sortOrder) {
            addData(sortOrder);
            StringBuilder sbd = new StringBuilder();
            //string csv = "Charlie, Chaplin, Chuckles";
            //string csv = lesContact.ToString();       System.Collections.Generic.List`1[ASPNetFrmwrkRapid.Models.contact]
            //string csv = String.Join(",", lesContact);      blank
            sbd.Append("Company Name, Years in Business, Contact Name, Contact Phone Number, Contact Email \n");
            //string csv = lesContact.Count.ToString();      // 0 before addData(), 6 after
            foreach (contact cntc in lesContact) {
                //sbd.Append($@"""{cntc.companyName}"", {cntc.yearsInBusiness}, {cntc.contactName}, {cntc.phone}, {cntc.email} \n");
                //sbd.Append($"{cntc.companyName}, {cntc.yearsInBusiness}, {cntc.contactName}, {cntc.phone}, {cntc.email} \n");
                sbd.Append(@"""" + cntc.companyName + @""", " + cntc.yearsInBusiness.ToString() + ", " + cntc.contactName + ", " +
                    cntc.phone + ", " + cntc.email + "\n");
            }
            return File(new System.Text.UTF8Encoding().GetBytes(sbd.ToString()), "text/csv", "companiesSortedBy" + 
                sortOrder + "_" + System.DateTime.Now.ToShortDateString() + "_" + System.DateTime.Now.ToShortTimeString() + ".csv");
        }
        /*public ActionResult exportToCsv() {
            FileInfo fin = new FileInfo()
        }   */
    }
}