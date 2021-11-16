using DocumentTextReader.Model;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Spire.Pdf;
using Spire.Pdf.Attachments;
using iTextSharp.text.pdf.parser;
using Spire.Pdf.Fields;
using Spire.Pdf.Texts;
using Spire.Pdf.Widget;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;
using IronOcr;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;
using Emgu.CV;
using Emgu.CV.Structure;

namespace DocumentTextReader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfConverterController : ControllerBase
    {
        private readonly ILogger<PdfConverterController> _logger;
        private Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
        private List<string> attachmentFileNames = new List<string>();
        private List<string> fileNames = new List<string>();
        private static IEnumerable xfaFields;

        public PdfConverterController(ILogger<PdfConverterController> logger)
        {
            _logger = logger;
        }

        [Route("pdfConvert")]
        [HttpGet]
        public string PdfToJson()
        {
            try
            {
                //Check if folder exist or not if exist proceed else create new folder inside Documents folder
                if (!Directory.Exists(Model.Constants.DirectoryPath + Model.Constants.DirectoryFolderName))
                {
                    Directory.CreateDirectory(Model.Constants.DirectoryPath + Model.Constants.DirectoryFolderName);
                    PdfToJson();
                }
                else
                {
                    keyValuePairs = GetTextFromDoc();
                    string pdfJsonData = JsonConvert.SerializeObject(keyValuePairs, Formatting.Indented);

                    //Save Keyvalue pair into jsonfile
                    System.IO.File.WriteAllText(Model.Constants.InitialPdfPath + Model.Constants.FileName.Replace("pdf", "json"), pdfJsonData);


                    //var pdfFileldsMGT7 = getAllPdfFiledsMGT7();

                    //var pdfFileldsDIR12 = getAllPdfFiledsDIR12();

                    //var pdfFileldsDIR12 = getAllPdfFiledsPAS();
                    var pdfFileldsAOC4 = getAllPdfFiledsAOC4();


                    //string jsonData = JsonConvert.SerializeObject(pdfFileldsMGT7, Formatting.Indented);
                    //string jsonData = JsonConvert.SerializeObject(pdfFileldsDIR12, Formatting.Indented);
                    //string jsonData = JsonConvert.SerializeObject(pdfFileldsDIR12, Formatting.Indented);
                    string jsonData = JsonConvert.SerializeObject(pdfFileldsAOC4, Formatting.Indented);

                    //Save Keyvalue pair into jsonfile
                    System.IO.File.WriteAllText(Model.Constants.InitialPdfPath + Model.Constants.FileName.Replace("pdf", "json"), jsonData);

                    attachmentFileNames = GetAllAttachments();

                    //Scan pdf attachment files saved in current directory
                    //tesseract to string .txt file save in same folder   
                    foreach (var attachmentName in attachmentFileNames)
                    {
                        IronTesseract ironTesseract = new IronTesseract();
                        using (OcrInput Input = new OcrInput(Model.Constants.DirectoryPath + Model.Constants.DirectoryFolderName + "/" + Model.Constants.AttachmentFolder + "/" + attachmentName))
                        {
                            Input.Deskew(); // removes rotation and perspective
                            OcrResult readResult = ironTesseract.Read(Input);
                            string text = readResult.Text;
                            System.IO.File.WriteAllText(Model.Constants.DirectoryPath + Model.Constants.DirectoryFolderName + "/" + Model.Constants.AttachmentFolder + "/" + attachmentName.Replace("pdf", "txt"), text);
                        }
                    }



                    //For Certificate of Incorporation and Fresh Certificate of Incorporation and COI StrikeThrough


                    IronTesseract tesseract = new IronTesseract();
                    using (OcrInput Input = new OcrInput(Model.Constants.DirectoryPath + Model.Constants.DirectoryFolderName + "/" + Model.Constants.FileName))
                    {
                        Input.Deskew(); // removes rotation and perspective
                        OcrResult readResult = tesseract.Read(Input);
                        string text = readResult.Text;
                        System.IO.File.WriteAllText(Model.Constants.DirectoryPath + Model.Constants.DirectoryFolderName + "/" + Model.Constants.DirectoryTextFile + "/" + Model.Constants.FileName.Replace("PDF", "txt"), text);
                    }



                    // 
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
            //var result = JsonConvert.SerializeObject(keyValuePairs);
            //return result;
        }


        [Route("pdfConvertNew")]
        [HttpGet]
        public string PdfToJsonConvert()
        {
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            //ImgShow();
            try
            {
                string jsonData = string.Empty;
                Dictionary<string, object> pdfFilelds = new Dictionary<string, object>();
                //Check if folder exist or not if exist proceed else create new folder inside Documents folder
                if (!Directory.Exists(Model.Constants.DirectoryPath + Model.Constants.DirectoryFolderName))
                {
                    Directory.CreateDirectory(Model.Constants.DirectoryPath + Model.Constants.DirectoryFolderName);
                    PdfToJsonConvert();
                }
                else
                {
                    //Spire.Pdf.PdfDocument doc1 = new Spire.Pdf.PdfDocument();
                    //doc1.LoadFromFile(Model.Constants.InitialPdfPath + "/" + Model.Constants.FileName);
                    //PdfFormWidget formWidget1 = doc1.Form as PdfFormWidget;
                    //List<string> fieldNameList = new List<string>();
                    //List<string> radioBtnValue = new List<string>();
                    //for (int i = 0; i < formWidget1.FieldsWidget.List.Count; i++)
                    //{
                    //    PdfField field = formWidget1.FieldsWidget.List[i] as PdfField;
                    //    if (field is PdfRadioButtonListFieldWidget)
                    //    {
                    //        PdfRadioButtonListFieldWidget radioBtnField = field as PdfRadioButtonListFieldWidget;
                    //        //get the selected value
                    //        string value = radioBtnField.SelectedValue;
                    //        radioBtnValue.Add(value);
                    //    }
                    //    if (field is PdfCheckBoxWidgetFieldWidget)
                    //    {
                    //        PdfCheckBoxWidgetFieldWidget checkBoxField = field as PdfCheckBoxWidgetFieldWidget;
                    //        //if the checkBox was checked and then get its field name
                    //        if (checkBoxField.Checked == true)
                    //        {
                    //            string fieldName = checkBoxField.Name;
                    //            fieldNameList.Add(fieldName);
                    //        }
                    //    }
                    //}


                    //System.IO.File.WriteAllLines(@"D:\FinalOutput\Checkbox1.json", fieldNameList.ToArray());
                    //System.IO.File.WriteAllLines(@"D:\FinalOutput\Radio2.json", radioBtnValue.ToArray());


                    Spire.Pdf.PdfDocument document = new Spire.Pdf.PdfDocument();
                    document.LoadFromFile(Model.Constants.InitialPdfPath + "/" + Model.Constants.FileName);

                    Spire.Pdf.Widget.PdfFormWidget formWidget = document.Form as PdfFormWidget;
                    if (formWidget != null)
                    {
                        if (formWidget.XFAForm != null)
                        {
                            //Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();
                            //doc.LoadFromFile(Model.Constants.InitialPdfPath + "/" + Model.Constants.FileName);

                            //PdfFormWidget formWidget1 = document.Form as PdfFormWidget;
                            //List<XfaField> xfafields = formWidget1.XFAForm.XfaFields;

                            //foreach (XfaField xfaField in xfafields)
                            //{
                            //    if (xfaField is XfaTextField)
                            //    {
                            //        XfaTextField xf = xfaField as XfaTextField;
                            //        keyValuePairs.Add(xf.Name, xf.Value);
                            //    }
                            //    else if (xfaField is XfaCheckButtonField)
                            //    {
                            //        XfaCheckButtonField xf = xfaField as XfaCheckButtonField;
                            //        keyValuePairs.Add(xf.Name, xf.Items);
                            //    }
                            //    else if (xfaField is XfaChoiceListField)
                            //    {
                            //        XfaChoiceListField xf = xfaField as XfaChoiceListField;
                            //        keyValuePairs.Add(xf.Name, xf.SelectedItem);
                            //    }
                            //    else if (xfaField is XfaDateTimeField)
                            //    {
                            //        XfaDateTimeField xf = xfaField as XfaDateTimeField;
                            //        keyValuePairs.Add(xf.Name, xf.Value);
                            //    }
                            //    else if (xfaField is XfaDoubleField)
                            //    {
                            //        XfaDoubleField xf = xfaField as XfaDoubleField;
                            //        keyValuePairs.Add(xf.Name, xf.Value);
                            //    }
                            //    else if (xfaField is XfaFloatField)
                            //    {
                            //        XfaFloatField xf = xfaField as XfaFloatField;
                            //        keyValuePairs.Add(xf.Name, xf.Value);
                            //    }
                            //    else if (xfaField is XfaIntField)
                            //    {
                            //        XfaIntField xf = xfaField as XfaIntField;
                            //        keyValuePairs.Add(xf.Name, xf.Value);
                            //    }

                            //}

                            pdfFilelds = getAllPdfFileds(document, formWidget);

                            jsonData = JsonConvert.SerializeObject(pdfFilelds, Formatting.Indented);

                            //Save Keyvalue pair into jsonfile
                            System.IO.File.WriteAllText(Model.Constants.DirectoryPath + Model.Constants.DirectoryFolderName + "/" + Model.Constants.FileName.Replace("pdf", "json"), jsonData);

                            attachmentFileNames = GetAllAttachments();

                            //Scan pdf attachment files saved in current directory
                            //tesseract to string .txt file save in same folder   
                            foreach (var attachmentName in attachmentFileNames)
                            {
                                string text = string.Empty;

                                Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();
                                document.LoadFromFile(Model.Constants.DirectoryPath + Model.Constants.DirectoryFolderName + "/" + Model.Constants.AttachmentFolder + "/" + attachmentName);

                                Spire.Pdf.Widget.PdfFormWidget pdfFormWidget = doc.Form as PdfFormWidget;
                                if (pdfFormWidget != null)
                                {
                                    if (pdfFormWidget.XFAForm != null)
                                    {
                                        //pdfFilelds = getAllPdfFileds(doc, pdfFormWidget);

                                        //jsonData = JsonConvert.SerializeObject(pdfFilelds, Formatting.Indented);

                                        ////Save Keyvalue pair into jsonfile
                                        //System.IO.File.WriteAllText(Model.Constants.DirectoryPath + Model.Constants.DirectoryFolderName + "/" + Model.Constants.FileName.Replace("pdf", "json"), jsonData);
                                    }

                                }
                                else
                                {
                                    IronTesseract ironTesseract = new IronTesseract();
                                    using (OcrInput Input = new OcrInput(Model.Constants.DirectoryPath + Model.Constants.DirectoryFolderName + "/" + Model.Constants.AttachmentFolder + "/" + attachmentName))
                                    {
                                        Input.Deskew(); // removes rotation and perspective
                                        OcrResult readResult = ironTesseract.Read(Input);
                                        text = readResult.Text;
                                        System.IO.File.WriteAllText(Model.Constants.DirectoryPath + Model.Constants.DirectoryFolderName + "/" + Model.Constants.AttachmentFolder + "/" + attachmentName.Replace("pdf", "txt"), text);
                                    }
                                }
                            }

                        }
                    }
                    //else if (formWidget.XFAForm == null)
                    //{
                    //    StringBuilder content = new StringBuilder();
                    //    string text = string.Empty;
                    //    foreach (PdfPageBase page in document.Pages)
                    //    {
                    //        content.Append(page.ExtractText());
                    //    }
                    //    System.IO.File.WriteAllText(Model.Constants.DirectoryPath + Model.Constants.DirectoryFolderName + "/" + Model.Constants.FileName.Replace("pdf", "txt"), content.ToString());
                    //}
                    //For Certificate of Incorporation and Fresh Certificate of Incorporation and COI StrikeThrough
                    else
                    {
                        IronTesseract tesseract = new IronTesseract();
                        using (OcrInput Input = new OcrInput(Model.Constants.InitialPdfPath + "/" + Model.Constants.FileName))
                        {
                            Input.Deskew(); // removes rotation and perspective
                            OcrResult readResult = tesseract.Read(Input);
                            string text = readResult.Text;
                            System.IO.File.WriteAllText(Model.Constants.DirectoryPath + Model.Constants.DirectoryFolderName + "/" + Model.Constants.FileName.Replace("PDF", "txt"), text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
            //var result = JsonConvert.SerializeObject(keyValuePairs);
            //return result;
        }

        [Route("pdfConvertCOI")]
        [HttpGet]
        public string PdfToJsonConvertCOI()
        {
            try
            {
                //Check if folder exist or not if exist proceed else create new folder inside Documents folder
                if (!Directory.Exists(Model.Constants.DirectoryPath + Model.Constants.DirectoryFolderName))
                {
                    Directory.CreateDirectory(Model.Constants.DirectoryPath + Model.Constants.DirectoryFolderName);
                    PdfToJsonConvertCOI();
                }
                else
                {
                    //For Certificate of Incorporation and Fresh Certificate of Incorporation and COI StrikeThrough

                    IronTesseract tesseract = new IronTesseract();
                    using (OcrInput Input = new OcrInput(Model.Constants.InitialPdfPath + "/" + Model.Constants.FileName))
                    {
                        Input.Deskew(); // removes rotation and perspective
                        OcrResult readResult = tesseract.Read(Input);
                        string text = readResult.Text;
                        System.IO.File.WriteAllText(Model.Constants.DirectoryPath + Model.Constants.DirectoryFolderName + "/" + Model.Constants.FileName.Replace("pdf", "txt"), text);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
            //var result = JsonConvert.SerializeObject(keyValuePairs);
            //return result;
        }

        private void getAllText()
        {
            Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();

            doc.LoadFromFile(@"C:\Users\Rahul\Downloads\DIR 12.pdf");

            //   PdfPageBase page = doc.Pages[0];
            string extractedText = "";
            for (int i = 0; i < doc.Pages.Count; i++)
            {

                StringBuilder sb = new StringBuilder();
                sb.Append(doc.Pages[i].ExtractText());

                extractedText = extractedText + sb.ToString();
            }

            iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(@"C:\Users\Rahul\Downloads\MGT Scanned.pdf");

            StringWriter output = new StringWriter();
            string text = "";
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                output.WriteLine(iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, i, new SimpleTextExtractionStrategy()));
                text = output.ToString();
            }

        }

        private Dictionary<string, object> getAllPdfFileds(Spire.Pdf.PdfDocument doc, Spire.Pdf.Widget.PdfFormWidget formWidget)
        {
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            List<string> fieldNameList = new List<string>();
            List<PdfField> fieldList = new List<PdfField>();
            string fieldName = string.Empty;
            string fieldType = string.Empty;
            //List<string> fieldNameList = new List<string>();
            List<string> radioBtnValue = new List<string>();
            try
            {
                for (int i = 0; i < formWidget.FieldsWidget.List.Count; i++)
                {

                    PdfField field = formWidget.FieldsWidget.List[i] as PdfField;
                    fieldName = field.Name;

                    fieldNameList.Add(field.Name);
                    fieldList.Add(field);

                    if (field is PdfTextBoxFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfTextBoxFieldWidget)field).Text);
                        fieldType = "TextBoxType";
                    }
                    else if (field is PdfRadioButtonListFieldWidget)
                    {
                        PdfRadioButtonListFieldWidget radioBtnField = field as PdfRadioButtonListFieldWidget;
                        //get the selected value
                        string value = radioBtnField.SelectedValue;
                        radioBtnValue.Add(value);
                        keyValuePairs.Add(fieldName, value);
                    }
                    else if (field is PdfCheckBoxWidgetFieldWidget)
                    {
                        PdfCheckBoxWidgetFieldWidget checkBoxField = field as PdfCheckBoxWidgetFieldWidget;
                        //if the checkBox was checked and then get its field name
                        if (checkBoxField.Checked == true)
                        {
                            fieldName = checkBoxField.Name;
                            fieldNameList.Add(fieldName);
                            keyValuePairs.Add(fieldName, fieldName);
                        }
                    }
                    //else if (field is PdfComboBoxWidgetFieldWidget)
                    //{
                    //    keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfComboBoxWidgetFieldWidget)field).SelectedValue);
                    //    fieldType = "ComboBoxType";
                    //}
                    //else if (field is PdfCheckBoxWidgetFieldWidget)
                    //{
                    //    keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfCheckBoxWidgetFieldWidget)field).Checked.ToString());
                    //    fieldType = "CheckBoxType";
                    //}
                    //else if (field is PdfSignatureFieldWidget)
                    //{
                    //    keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfSignatureFieldWidget)field).Signature.ToString());
                    //    fieldType = "SignatureType";
                    //}
                    else
                    {
                        fieldType = "InvalidType";
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return keyValuePairs;
        }

        private PdfFileldsMGT7 getAllPdfFiledsMGT7()
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            PdfFileldsMGT7 pdfFieds = new PdfFileldsMGT7();
            List<String> fieldNameList = new List<String>();
            List<PdfField> fieldList = new List<PdfField>();
            string fieldName = string.Empty;
            string fieldType = string.Empty;
            PdfFileldsMGT7 pdfFilelds = new PdfFileldsMGT7();
            try
            {
                Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();
                doc.LoadFromFile(Model.Constants.InitialPdfPath + Model.Constants.FileName);

                Spire.Pdf.Widget.PdfFormWidget formWidget = doc.Form as PdfFormWidget;

                for (int i = 0; i < formWidget.FieldsWidget.List.Count; i++)
                {
                    PdfField field = formWidget.FieldsWidget.List[i] as PdfField;
                    fieldName = field.Name;

                    fieldNameList.Add(field.Name);
                    fieldList.Add(field);

                    if (field is PdfTextBoxFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfTextBoxFieldWidget)field).Text);
                        fieldType = "TextBoxType";
                    }
                    else if (field is PdfButtonWidgetFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfButtonWidgetFieldWidget)field).Text);
                        fieldType = "ButtonType";
                    }
                    else if (field is PdfRadioButtonListFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfRadioButtonListFieldWidget)field).Value);
                        fieldType = "RadioButtonType";
                    }
                    //else if (field is PdfComboBoxWidgetFieldWidget)
                    //{
                    //    keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfComboBoxWidgetFieldWidget)field).SelectedValue);
                    //    fieldType = "ComboBoxType";
                    //}
                    else if (field is PdfCheckBoxWidgetFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfCheckBoxWidgetFieldWidget)field).Checked.ToString());
                        fieldType = "CheckBoxType";
                    }
                    else if (field is PdfSignatureFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfSignatureFieldWidget)field).Signature.ToString());
                        fieldType = "SignatureType";
                    }
                    else
                    {
                        fieldType = "InvalidType";
                    }
                }

                //For MGT - 7

                //pdfFilelds.CorporateIdentificationNumber = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].Page1[0].CIN[0]"];
                //pdfFilelds.FormLanguage = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].Page1[0].FormLanguage[0]"];
                //pdfFilelds.GlobalLocationNumber = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].Page1[0].GLN[0]"];
                //pdfFilelds.NameOfTheCompany = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].Page1[0].Name[0]"];
                //pdfFilelds.RegisteredOfficeAddress = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].Page1[0].Address[0]"];
                //pdfFilelds.EmailIdOfTheCompany = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].Page1[0].Email[0]"];
                //pdfFilelds.TelephoneNumberWithSTDCode = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].Page1[0].Telephone[0]"];
                //pdfFilelds.Website = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].Page1[0].Website[0]"];
                //pdfFilelds.DateofIncorporation = DateTime.Parse(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].Page1[0].DateOfIncorporation[0]"]);
                //pdfFilelds.HavingShareCapital = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].Page1[0].SharedCapital_R[0]"];
                //pdfFilelds.SharesListedOnRecognizedStockExchange = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].Page1[0].StockExchange[0]"];

                pdfFilelds.FinancialYearFromDate = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].AGMHeldDtls[0].FinancialYearFrom[0]"];
                pdfFilelds.AGMHeld = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].AGMHeldDtls[0].AGMHeld[0]"];
                pdfFilelds.DateOfAGM = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].AGMHeldDtls[0].DateOfAgm[0]"];
                pdfFilelds.DueDateOfAGM = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].AGMHeldDtls[0].DateOfAgm[0]"];
                pdfFilelds.NumberOfBusinessActivities = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionII[0].NoBusinessActivity[0]"];
                //pdfFilelds.MainActivityGroupCode = keyValuePairs[""];    //Unsure about its value

                List<string> list = new List<string>();
                list.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionIITable[0].PrincipalBusinessActivity[0].Row1[0].Cell3[0]"]);
                list.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionIITable[0].PrincipalBusinessActivity[0].Row1[1].Cell3[0]"]);
                pdfFilelds.DescriptionOfMainActivityGroup = list;

                pdfFilelds.BusinessActivityCode = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].AGMHeldDtls[0].DateOfAgm[0]"];

                List<string> data = new List<string>();
                data.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionIITable[0].PrincipalBusinessActivity[0].Row1[0].Cell5[0]"]);
                data.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionIITable[0].PrincipalBusinessActivity[0].Row1[1].Cell5[0]"]);

                pdfFilelds.DescriptionOfBusinessActivity = data;


                List<string> data1 = new List<string>();
                data1.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionIITable[0].PrincipalBusinessActivity[0].Row1[0].Cell6[0]"]);
                data1.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionIITable[0].PrincipalBusinessActivity[0].Row1[1].Cell6[0]"]);

                pdfFilelds.PercentOfTurnoverOfCompany = data1;




                pdfFilelds.TotalNumberOfEquitySharesAuthorised = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].EquityDynamicTable[0].EquityDynamic[0].Row1[0].NumAuthCapitalShares[0]"];
                pdfFilelds.TotalAmountOfEquitySharesInRupeesAuthorised = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].EquityDynamicTable[0].EquityDynamic[0].Row3[0].TotAuthCapitalShares[0]"];


                //Start from here
                pdfFilelds.TotalNumberOfEquitySharesIssued = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].EquityDynamicTable[0].EquityDynamic[0].Row1[0].NumIssueCapitalShares[0]"];
                pdfFilelds.TotalAmountOfEquitySharesInRupeesIssued = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].EquityDynamicTable[0].EquityDynamic[0].Row3[0].TotIssueCapitalShares[0]"];
                pdfFilelds.TotalNumberOfEquitySharesSubscribed = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].EquityDynamicTable[0].EquityDynamic[0].Row1[0].NumSubCapitalShares[0]"];
                pdfFilelds.TotalAmountOfEquitySharesRupeesSubscribed = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].EquityDynamicTable[0].EquityDynamic[0].Row3[0].TotSubCapitalShares[0]"];
                pdfFilelds.TotalNumberOfEquitySharesPaidUp = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].EquityDynamicTable[0].EquityDynamic[0].Row1[0].NumPaidCapitalShares[0]"];
                pdfFilelds.TotalAmountOfEquitySharesRupeesPaidUp = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].EquityDynamicTable[0].EquityDynamic[0].Row3[0].TotPaidCapitalShares[0]"];
                pdfFilelds.TotalNumberOfPreferenceSharesAuthorised = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionIV1b[0].EquityStatic[0].Row1[0].NumericField23[0]"];
                pdfFilelds.TotalAmountOfPreferenceSharesRupeesAuthorised = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionIV1b[0].EquityStatic[0].Row2[0].NumericField19[0]"];
                pdfFilelds.TotalAmountOfPreferenceSharesRupeesIssued = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionIV1b[0].EquityStatic[0].Row1[0].NumericField22[0]"];
                pdfFilelds.TotalNumberOfPreferenceSharesSubscribed = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionIV1b[0].EquityStatic[0].Row1[0].NumericField21[0]"];
                pdfFilelds.TotalAmountOfPreferenceSharesRupeesSubscribed = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionIV1b[0].EquityStatic[0].Row2[0].NumericField20[0]"];
                pdfFilelds.TotalAmountOfPreferenceSharesRupeesPaidUp = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionIV1b[0].EquityStatic[0].Row2[0].NumericField17[0]"];

                //List<string> directors = new List<string>();
                //directors.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionVIIIADynamic[0].Table13[0].Row1[0].Cell1[0]"]);
                //directors.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionVIIIADynamic[0].Table13[0].Row1[1].Cell1[0]"]);
                //directors.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionVIIIADynamic[0].Table13[0].Row1[2].Cell1[0]"]);


                pdfFilelds.NameOfDirector = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionVIIIADynamic[0].Table13[0].Row1[2].Cell1[0]"];

                //List<string> dinPan = new List<string>();
                //dinPan.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionVIIIADynamic[0].Table13[0].Row1[0].Cell2[0]"]);
                //dinPan.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionVIIIADynamic[0].Table13[0].Row1[1].Cell2[0]"]);
                //dinPan.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionVIIIADynamic[0].Table13[0].Row1[2].Cell2[0]"]);

                pdfFilelds.DINPANOfDirector = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionVIIIADynamic[0].Table13[0].Row1[2].Cell2[0]"];
                //pdfFilelds.DesignationOfDirectorAtTheBeginningDuringTheFinancialYear = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].AGMHeldDtls[0].DateOfAgm[0]"];  //Value not there in json

                //List<string> equitySharesHeld = new List<string>();
                //equitySharesHeld.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionVIIIADynamic[0].Table13[0].Row1[0].Cell4[0]"]);
                //equitySharesHeld.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionVIIIADynamic[0].Table13[0].Row1[1].Cell4[0]"]);
                //equitySharesHeld.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionVIIIADynamic[0].Table13[0].Row1[2].Cell4[0]"]);

                pdfFilelds.NumberOfEquityShareHeldByDirector = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionVIIIADynamic[0].Table13[0].Row1[2].Cell4[0]"];
                pdfFilelds.DateOfAppointmentChangeInDesignationCessation = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionVIIIBDynamic[0].Table13[0].Row1[0].Cell4[0]"];
                pdfFilelds.NatureOfChangeAppointmentChangeInDesignationCessation = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionVIIIBDynamic[0].Table13[0].Row1[0].Cell5[0]"];

                List<string> grossSalary = new List<string>();
                grossSalary.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionXA[0].Table20[0].Row1[0].Cell4[0]"]);
                grossSalary.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionXA[0].Table20[0].Row1[1].Cell4[0]"]);
                grossSalary.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionXA[0].Table20[0].FooterRow[0].Cell4[0]"]);

                pdfFilelds.GrossSalaryofDirector = grossSalary;  //Unsure which one to take

                List<string> commisions = new List<string>();
                commisions.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionXA[0].Table20[0].Row1[0].Cell5[0]"]);
                commisions.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionXA[0].Table20[0].Row1[1].Cell5[0]"]);

                pdfFilelds.CommissionofDirector = commisions;  //Unsure which one to take

                List<string> stockOptions = new List<string>();
                stockOptions.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionXA[0].Table20[0].Row1[0].Cell6[0]"]);
                stockOptions.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionXA[0].Table20[0].Row1[1].Cell6[0]"]);

                pdfFilelds.StockOptionSweatEquityOfDirector = stockOptions;

                List<string> otherRenumeration = new List<string>();
                otherRenumeration.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionXC[0].Table20[0].Row1[0].Cell2[0]"]);
                otherRenumeration.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionXC[0].Table20[0].Row1[0].Cell3[0]"]);
                otherRenumeration.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionXC[0].Table20[0].Row1[0].Cell4[0]"]);
                otherRenumeration.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionXC[0].Table20[0].Row1[0].Cell5[0]"]);
                otherRenumeration.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionXC[0].Table20[0].Row1[0].Cell6[0]"]);
                otherRenumeration.Add(keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionXC[0].Table20[0].Row1[0].Cell7[0]"]);

                pdfFilelds.OtherRemmunerationOfDirector = otherRenumeration;

                pdfFilelds.NameOfShareholder = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionIIITable[0].ParticularOfHolding[0].Row1[0].Cell4[0]"];  //Confirm
                pdfFilelds.CategoryOfShareholder = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].AGMHeldDtls[0].DateOfAgm[0]"];  //Doubt
                pdfFilelds.NumberOfShares = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].AGMHeldDtls[0].DateOfAgm[0]"]; //Doubt
                pdfFilelds.PercentShareholding = keyValuePairs["data[0].FormMGT7_Dtls[0].MainPage[0].SectionIIITable[0].ParticularOfHolding[0].Row1[0].Cell6[0]"]; //Confirm
            }
            catch (Exception ex)
            {
                throw;
            }
            return pdfFilelds;
        }

        private PdfFieldsDIR12Json getAllPdfFiledsDIR12()
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            PdfFieldsDIR12Json pdfFieds = new PdfFieldsDIR12Json();
            List<String> fieldNameList = new List<String>();
            List<PdfField> fieldList = new List<PdfField>();
            string fieldName = string.Empty;
            string fieldType = string.Empty;
            PdfFieldsDIR12Json pdfFilelds = new PdfFieldsDIR12Json();
            try
            {
                Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();
                doc.LoadFromFile(Model.Constants.InitialPdfPath + Model.Constants.FileName);

                Spire.Pdf.Widget.PdfFormWidget formWidget = doc.Form as PdfFormWidget;

                for (int i = 0; i < formWidget.FieldsWidget.List.Count; i++)
                {
                    PdfField field = formWidget.FieldsWidget.List[i] as PdfField;
                    fieldName = field.Name;

                    fieldNameList.Add(field.Name);
                    fieldList.Add(field);

                    if (field is PdfTextBoxFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfTextBoxFieldWidget)field).Text);
                        fieldType = "TextBoxType";
                    }
                    else if (field is PdfButtonWidgetFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfButtonWidgetFieldWidget)field).Text);
                        fieldType = "ButtonType";
                    }
                    else if (field is PdfRadioButtonListFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfRadioButtonListFieldWidget)field).Value);
                        fieldType = "RadioButtonType";
                    }
                    //else if (field is PdfComboBoxWidgetFieldWidget)
                    //{
                    //    keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfComboBoxWidgetFieldWidget)field).SelectedValue);
                    //    fieldType = "ComboBoxType";
                    //}
                    else if (field is PdfCheckBoxWidgetFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfCheckBoxWidgetFieldWidget)field).Checked.ToString());
                        fieldType = "CheckBoxType";
                    }
                    else if (field is PdfSignatureFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfSignatureFieldWidget)field).Signature.ToString());
                        fieldType = "SignatureType";
                    }
                    else
                    {
                        fieldType = "InvalidType";
                    }
                }

                //For DIR - 12


                pdfFieds.DirectorName = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Page1[0].hiName[0]"];
                pdfFieds.DirectorIdentificationNumber = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Director1[0].DIN_C[0]"];
                pdfFieds.DirectorFatherName = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Page1[0].hiFathName[0]"];
                pdfFieds.DirectorResidenceAddress = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Page1[0].hiAddress[0]"];
                pdfFieds.DirectorNationality = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Page1[0].hiNationality[0]"];
                pdfFieds.DirectorDOB = DateTime.Parse(keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Page1[0].hiDob[0]"]);
                pdfFieds.DirectorGender = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Page1[0].hiGender[0]"];
                //pdfFieds.DirectorDesignation = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Director1[0].Desig_C[0]"];    //Not Present in json
                //pdfFieds.DirectorCategory = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Director1[0].Categ_C[0]"];    //Not Present in json
                pdfFieds.DirectorEmail = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Director1[0].EmailId_C[0]"];
                pdfFieds.DateOfResignation = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Director1[0].DateEffect_D[0]"];

                //pdfFieds.ServiceRequestNumber = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Page1[0].CIN_C[0]"];
                //pdfFieds.CompanyName = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Page1[0].CompanyName_C[0]"];
                //pdfFieds.GlobalLocationNumber = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Page1[0].GLN_C[0]"];
                //pdfFieds.CompanyEmail = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Page1[0].CIN_C[0]"];
                //pdfFieds.NumberOfDirectors = int.Parse(keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Page1[0].NumDir_N[0]"]);
                //pdfFieds.CompanyAddress = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Page1[0].CompanyAdd_C[0]"];
                //pdfFieds.EffectFromDate = DateTime.Parse(keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Director1[0].DateEffect_D[0]"]);

                //pdfFieds.CompanyNameOfAppointee = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Director1[0].CompNameAppointee_C[0]"];
                //pdfFieds.AppointeeAlternate = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Director1[0].AppAltDIN_C[0]"];
                //pdfFieds.AlternateDIrectorName = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Director1[0].NameAppAlt1_C[0]"];
                //pdfFieds.NumOfEntity = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Director1[0].NumOfEntity[0]"];
                //pdfFieds.CINNumber = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Director1[0].CIN_C[0]"];
                //pdfFieds.EntityName = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Director1[0].EntName[0]"];
                //pdfFieds.EntityAddress = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Director1[0].EntAdd[0]"];
                //pdfFieds.EntityDesignation = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Director1[0].EntDes[0]"];
                //pdfFieds.PercentageOfShareholding = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Director1[0].EntPer[0]"];
                //pdfFieds.EntityAmount = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Director1[0].EntAmt[0]"];
                //pdfFieds.EntityOthers = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Director1[0].EntOth[0]"];
                //pdfFieds.ManagerFirstName = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].NumOfMan[0].HI_FNAME[0]"];
                //pdfFieds.ManagerMiddleName = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].NumOfMan[0].HI_MNAME[0]"];
                //pdfFieds.ManagerLastName = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].NumOfMan[0].HI_LNAME[0]"];
                //pdfFieds.ManagerFathersFirstName = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].NumOfMan[0].HI_FATHFNAME[0]"];
                //pdfFieds.ManagerFathersMiddleName = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].NumOfMan[0].HI_FATHMNAME[0]"];
                //pdfFieds.ManagerFathersLastName = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].NumOfMan[0].HI_FATHLNAME[0]"];
                //pdfFieds.DIN = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Secretary1[0].DIN_C[0]"];
                //pdfFieds.PANNumber = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Secretary1[0].PAN_C[0]"];
                //pdfFieds.ResidentialAddressLine1 = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Secretary1[0].permaddress21_C[0]"];
                //pdfFieds.ResidentialAddressLine2 = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Secretary1[0].permaddress22_C[0]"];
                //pdfFieds.City = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Secretary1[0].City_C[0]"];
                //pdfFieds.State = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Secretary1[0].State_P[0]"];
                //pdfFieds.PinCode = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Secretary1[0].Pin1_C[0]"];
                //pdfFieds.CountryCode = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Secretary1[0].Country_C[0]"];
                //pdfFieds.Country = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Secretary1[0].countryname1[0]"];
                //pdfFieds.ManagerDOB = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Secretary1[0].Birthdate1_D[0]"];
                //pdfFieds.PhoneNumber = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Secretary1[0].Phone_C[0]"];
                //pdfFieds.Fax = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Secretary1[0].Fax_C[0]"];
                //pdfFieds.EmailID = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Secretary1[0].EmailID_C[0]"];
                //pdfFieds.Designation = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Secretary1[0].Desig_C[0]"];
                //pdfFieds.DateOfAppointment = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Secretary1[0].DateOfAppSect_D[0]"];
                //pdfFieds.ListOfAttachment = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Page9[0].Hidden_L[0]"];
                //pdfFieds.NewDesignation = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Page9[0].DesigD_C[0]"];
                //pdfFieds.ResoNum = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Page9[0].ResoNum[0]"];
                //pdfFieds.Date = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Page9[0].DateOfAppSect_D[0]"];
                //pdfFieds.CompanyDesignation = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Page9[0].Desig_C[0]"];
                //pdfFieds.DINOfDirector = keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Page9[0].signatory_id[0]"];
                //pdfFieds.MembershipNumber = int.Parse(keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Page9[0].Mem_C[0]"]);
                //pdfFieds.PracticeNumber = int.Parse(keyValuePairs["data[0].Form32_Dtls[0].MainPage[0].Page9[0].COPNo[0]"]);
            }
            catch (Exception ex)
            {
                throw;
            }
            return pdfFieds;
        }

        private PdfFieldsPASJson getAllPdfFiledsPAS()
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            PdfFieldsPASJson pdfFieds = new PdfFieldsPASJson();
            List<String> fieldNameList = new List<String>();
            List<PdfField> fieldList = new List<PdfField>();
            string fieldName = string.Empty;
            string fieldType = string.Empty;
            PdfFieldsPASJson pdfFilelds = new PdfFieldsPASJson();
            try
            {
                Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();
                doc.LoadFromFile(Model.Constants.InitialPdfPath + Model.Constants.FileName);

                Spire.Pdf.Widget.PdfFormWidget formWidget = doc.Form as PdfFormWidget;

                for (int i = 0; i < formWidget.FieldsWidget.List.Count; i++)
                {
                    PdfField field = formWidget.FieldsWidget.List[i] as PdfField;
                    fieldName = field.Name;

                    fieldNameList.Add(field.Name);
                    fieldList.Add(field);

                    if (field is PdfTextBoxFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfTextBoxFieldWidget)field).Text);
                        fieldType = "TextBoxType";
                    }
                    else if (field is PdfButtonWidgetFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfButtonWidgetFieldWidget)field).Text);
                        fieldType = "ButtonType";
                    }
                    else if (field is PdfRadioButtonListFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfRadioButtonListFieldWidget)field).Value);
                        fieldType = "RadioButtonType";
                    }
                    //else if (field is PdfComboBoxWidgetFieldWidget)
                    //{
                    //    keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfComboBoxWidgetFieldWidget)field).SelectedValue);
                    //    fieldType = "ComboBoxType";
                    //}
                    else if (field is PdfCheckBoxWidgetFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfCheckBoxWidgetFieldWidget)field).Checked.ToString());
                        fieldType = "CheckBoxType";
                    }
                    else if (field is PdfSignatureFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfSignatureFieldWidget)field).Signature.ToString());
                        fieldType = "SignatureType";
                    }
                    else
                    {
                        fieldType = "InvalidType";
                    }
                }

                pdfFieds.AddressOfShareHolder = keyValuePairs["data[0].Form2_Dtls[0].MainPage[0].Page1[0].CompanyAdd_C[0]"];              //All Doubts
                pdfFieds.NationalityOfShareHolder = keyValuePairs["data[0].Form2_Dtls[0].MainPage[0].Page1[0].CompanyAdd_C[0]"];
                pdfFieds.NumberOfShares = keyValuePairs["data[0].Form2_Dtls[0].MainPage[0].Page1[0].CompanyAdd_C[0]"];
                pdfFieds.PercentShareHolding = keyValuePairs["data[0].Form2_Dtls[0].MainPage[0].Page1[0].CompanyAdd_C[0]"];
                pdfFieds.TotalAmountPaidIncludingPremium = keyValuePairs["data[0].Form2_Dtls[0].MainPage[0].Page1[0].CompanyAdd_C[0]"];
            }
            catch (Exception ex)
            {
                throw;
            }
            return pdfFieds;
        }

        private PdfFieldsAOC4Json getAllPdfFiledsAOC4()
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            PdfFieldsAOC4Json pdfFieds = new PdfFieldsAOC4Json();
            List<String> fieldNameList = new List<String>();
            List<PdfField> fieldList = new List<PdfField>();
            string fieldName = string.Empty;
            string fieldType = string.Empty;
            PdfFieldsAOC4Json pdfFilelds = new PdfFieldsAOC4Json();
            try
            {
                Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();
                doc.LoadFromFile(Model.Constants.InitialPdfPath + Model.Constants.FileName);

                Spire.Pdf.Widget.PdfFormWidget formWidget = doc.Form as PdfFormWidget;

                for (int i = 0; i < formWidget.FieldsWidget.List.Count; i++)
                {
                    PdfField field = formWidget.FieldsWidget.List[i] as PdfField;
                    fieldName = field.Name;

                    fieldNameList.Add(field.Name);
                    fieldList.Add(field);

                    if (field is PdfTextBoxFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfTextBoxFieldWidget)field).Text);
                        fieldType = "TextBoxType";
                    }
                    else if (field is PdfButtonWidgetFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfButtonWidgetFieldWidget)field).Text);
                        fieldType = "ButtonType";
                    }
                    else if (field is PdfRadioButtonListFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfRadioButtonListFieldWidget)field).Value);
                        fieldType = "RadioButtonType";
                    }
                    //else if (field is PdfComboBoxWidgetFieldWidget)
                    //{
                    //    keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfComboBoxWidgetFieldWidget)field).SelectedValue);
                    //    fieldType = "ComboBoxType";
                    //}
                    else if (field is PdfCheckBoxWidgetFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfCheckBoxWidgetFieldWidget)field).Checked.ToString());
                        fieldType = "CheckBoxType";
                    }
                    else if (field is PdfSignatureFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfSignatureFieldWidget)field).Signature.ToString());
                        fieldType = "SignatureType";
                    }
                    else
                    {
                        fieldType = "InvalidType";
                    }
                }

                pdfFieds.AddressOfRegisteredOffice = keyValuePairs["data[0].FormAOC4_Dtls[0].Segment1_PartA[0].CompanyAddress_C[0]"];
                pdfFieds.AuthorizedCapitalOfTheCompany = keyValuePairs["data[0].FormAOC4_Dtls[0].Language[0].AuthCap[0]"];
                pdfFieds.NumberOfMembersOfTheCompany = keyValuePairs["data[0].FormAOC4_Dtls[0].Language[0].NumMem[0]"];
                pdfFieds.CIN = keyValuePairs["data[0].FormAOC4_Dtls[0].Segment1_PartA[0].CIN_C[0]"];
                pdfFieds.GLN = keyValuePairs["data[0].FormAOC4_Dtls[0].Segment1_PartA[0].GLN_C[0]"];
                pdfFieds.NameOfTheCompany = keyValuePairs["data[0].FormAOC4_Dtls[0].Segment1_PartA[0].CompName_C[0]"];
                pdfFieds.EmailOfTheCompany = keyValuePairs["data[0].FormAOC4_Dtls[0].Segment1_PartA[0].CompEmail_C[0]"];

                List<DateTime> dateTimes = new List<DateTime>();
                dateTimes.Add(DateTime.Parse(keyValuePairs["data[0].FormAOC4_Dtls[0].Segment1_PartA[0].FromDate[0]"]));
                dateTimes.Add(DateTime.Parse(keyValuePairs["data[0].FormAOC4_Dtls[0].Segment1_PartA[0].ToDate[0]"]));

                pdfFieds.FinancialYear = dateTimes;
                //pdfFieds.NatureOfFinancialStatements = keyValuePairs["data[0].FormAOC4_Dtls[0].Segment1_PartA[0].CompanyAddress_C[0]"];    //Not Found in Json 
                //pdfFieds.ProvisionalFinancialStatementsFiledEarlier = keyValuePairs["data[0].FormAOC4_Dtls[0].Segment1_PartA[0].CompanyAddress_C[0]"];     //Doubt
                //pdfFieds.AdoptedInAdjournedAGM = keyValuePairs["data[0].FormAOC4_Dtls[0].Segment1_PartA[0].CompanyAddress_C[0]"];   //Doubt

                List<string> dinNo = new List<string>();
                dinNo.Add(keyValuePairs["data[0].FormAOC4_Dtls[0].Segment1_4C[0].Table1[0].Row1[0].DINPAN1[0]"]);
                dinNo.Add(keyValuePairs["data[0].FormAOC4_Dtls[0].Segment1_4C[0].Table1[0].Row2[0].DINPAN2[0]"]);

                pdfFieds.DINOrIncometaxPAN = dinNo;

                List<string> names = new List<string>();
                names.Add(keyValuePairs["data[0].FormAOC4_Dtls[0].Segment1_4C[0].Table1[0].Row1[0].Name1[0]"]);
                names.Add(keyValuePairs["data[0].FormAOC4_Dtls[0].Segment1_4C[0].Table1[0].Row2[0].Name2[0]"]);

                pdfFieds.Names = names;
                //pdfFieds.Designation = keyValuePairs["data[0].FormAOC4_Dtls[0].Segment1_PartA[0].CompanyAddress_C[0]"];     //Doubt

                List<DateTime> dateTime = new List<DateTime>();
                dateTime.Add(DateTime.Parse(keyValuePairs["data[0].FormAOC4_Dtls[0].Segment1_4C[0].Table1[0].Row1[0].Date1[0]"]));
                dateTime.Add(DateTime.Parse(keyValuePairs["data[0].FormAOC4_Dtls[0].Segment1_4C[0].Table1[0].Row2[0].Date2[0]"]));

                //Done till here
                pdfFieds.DateOfSigningOfFinancialStatements = dateTime;
                pdfFieds.DateOfSigningOfBoardsReport = keyValuePairs["data[0].FormAOC4_Dtls[0].Segment1_PartA[0].CompanyAddress_C[0]"];
                pdfFieds.DateOfSigningOfReportsOnTheFinancialStatementsByTheAuditors = keyValuePairs["data[0].FormAOC4_Dtls[0].Segment1_PartA[0].CompanyAddress_C[0]"];
                pdfFieds.AnnualGeneralMeetingAGMHeld = keyValuePairs["data[0].FormAOC4_Dtls[0].Segment1_PartA[0].CompanyAddress_C[0]"];
                pdfFieds.DateOfAGM = keyValuePairs["data[0].FormAOC4_Dtls[0].Segment1_PartA[0].CompanyAddress_C[0]"];
                pdfFieds.DueDateOfAGM = keyValuePairs["data[0].FormAOC4_Dtls[0].Segment1_PartA[0].CompanyAddress_C[0]"];
                pdfFieds.extensionForFinancialYearOrAGMGranted = keyValuePairs["data[0].FormAOC4_Dtls[0].Segment1_PartA[0].CompanyAddress_C[0]"];

            }
            catch (Exception ex)
            {
                throw;
            }
            return pdfFieds;
        }

        //private Dictionary<string, string> getTextFromMGTDoc()
        //{
        //    try
        //    {
        //        PdfFieldsJSON pdfFieds = new PdfFieldsJSON();

        //        Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();

        //        doc.LoadFromFile(@"C:\Users\Rahul\Downloads\Form MGT-7-07102017_signed.pdf");

        //        Spire.Pdf.Widget.PdfFormWidget formWidget = doc.Form as PdfFormWidget;

        //        string fieldName = "";
        //        List<String> fieldNameList = new List<String>();
        //        List<PdfField> fieldList = new List<PdfField>();
        //        string fieldType = "";
        //        string fullstring = "";
        //        Hashtable hashtable = new Hashtable();

        //        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
        //        for (int i = 0; i < formWidget.FieldsWidget.List.Count; i++)
        //        {
        //            PdfField field = formWidget.FieldsWidget.List[i] as PdfField;

        //            fieldName = field.Name;

        //            fieldNameList.Add(field.Name);
        //            fieldList.Add(field);

        //            if (field is PdfTextBoxFieldWidget)
        //            {
        //                keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfTextBoxFieldWidget)field).Text);
        //                fieldType = "textBoxType";
        //            }
        //            else if (field is PdfButtonWidgetFieldWidget)
        //            {
        //                keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfButtonWidgetFieldWidget)field).Text);
        //                fieldType = "buttonType";
        //            }
        //            else if (field is PdfRadioButtonListFieldWidget)
        //            {
        //                keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfRadioButtonListFieldWidget)field).Value);
        //                fieldType = "radioButtonType";
        //            }
        //            //else if (field is PdfComboBoxWidgetFieldWidget)
        //            //{
        //            //    if (!string.IsNullOrWhiteSpace(((Spire.Pdf.Widget.PdfComboBoxWidgetFieldWidget)field).SelectedValue))
        //            //    {
        //            //        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfComboBoxWidgetFieldWidget)field).SelectedValue);
        //            //    }
        //            //           hashtable.Add(fieldName, ((Spire.Pdf.Widget.PdfComboBoxWidgetFieldWidget)field).SelectedValue);
        //            //       if (!((Spire.Pdf.Widget.PdfComboBoxWidgetFieldWidget)field).SelectedIndex.Equals(-1))
        //            //       {
        //            //           if (!hashtable.ContainsKey(fieldName))
        //            //           {
        //            //               hashtable.Add(fieldName, ((Spire.Pdf.Widget.PdfComboBoxWidgetFieldWidget)field).SelectedValue);
        //            //           }
        //            //       }else{

        //            //           if (!hashtable.ContainsKey(fieldName))
        //            //           {
        //            //               hashtable.Add(fieldName,"");
        //            //           }
        //            //       }

        //            //    fieldType = "comboBoxType";
        //            //}
        //            else if (field is PdfCheckBoxWidgetFieldWidget)
        //            {
        //                keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfCheckBoxWidgetFieldWidget)field).Checked.ToString());
        //                fieldType = "checkBoxType";
        //            }
        //            else if (field is PdfSignatureFieldWidget)
        //            {
        //                keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfSignatureFieldWidget)field).Signature.ToString());
        //                fieldType = "signatureType";
        //            }
        //            else
        //            {
        //                fieldType = "invalidType";
        //            }
        //        }
        //        Console.WriteLine(fullstring);

        //        Console.WriteLine(keyValuePairs.Count);

        //        return keyValuePairs;
        //    }
        //    catch (Exception e)
        //    {

        //        throw;
        //    }

        //}

        private Dictionary<string, string> GetTextFromDoc()
        {
            try
            {
                //PdfFieldsJson pdfFieds = new PdfFieldsJson();

                Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();

                doc.LoadFromFile(Model.Constants.InitialPdfPath + Model.Constants.FileName);

                Spire.Pdf.Widget.PdfFormWidget formWidget = doc.Form as PdfFormWidget;

                string fieldName = string.Empty;
                List<String> fieldNameList = new List<String>();
                List<PdfField> fieldList = new List<PdfField>();
                string fieldType = string.Empty;
                string fullstring = string.Empty;
                Hashtable hashtable = new Hashtable();

                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                for (int i = 0; i < formWidget.FieldsWidget.List.Count; i++)
                {
                    PdfField field = formWidget.FieldsWidget.List[i] as PdfField;

                    fieldName = field.Name;

                    fieldNameList.Add(field.Name);
                    fieldList.Add(field);

                    if (field is PdfTextBoxFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfTextBoxFieldWidget)field).Text);
                        fieldType = "textBoxType";
                    }
                    else if (field is PdfButtonWidgetFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfButtonWidgetFieldWidget)field).Text);
                        fieldType = "buttonType";
                    }
                    else if (field is PdfRadioButtonListFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfRadioButtonListFieldWidget)field).Value);
                        fieldType = "radioButtonType";
                    }
                    else if (field is PdfCheckBoxWidgetFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfCheckBoxWidgetFieldWidget)field).Checked.ToString());
                        fieldType = "checkBoxType";
                    }
                    else if (field is PdfSignatureFieldWidget)
                    {
                        keyValuePairs.Add(fieldName, ((Spire.Pdf.Widget.PdfSignatureFieldWidget)field).Signature.ToString());
                        fieldType = "signatureType";
                    }
                    else
                    {
                        fieldType = "invalidType";
                    }
                }
                Console.WriteLine(fullstring);

                Console.WriteLine(keyValuePairs.Count);

                return keyValuePairs;
            }
            catch (Exception e)
            {

                throw;
            }

        }

        private List<string> GetAllAttachments()
        {
            //List<string> fileNames = new List<string>();
            Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();

            //doc.LoadFromFile(Model.Constants.InitialPdfPath + Model.Constants.FileName);
            doc.LoadFromFile(Model.Constants.InitialPdfPath + "/" + Model.Constants.FileName);

            PdfAttachment attachment = null;
            try
            {
                if (!Directory.Exists(Model.Constants.DirectoryPath + Model.Constants.DirectoryFolderName + "/" + Model.Constants.AttachmentFolder))
                {
                    Directory.CreateDirectory(Model.Constants.DirectoryPath + Model.Constants.DirectoryFolderName + "/" + Model.Constants.AttachmentFolder);
                    GetAllAttachments();
                }
                else
                {
                    if (doc.Attachments.Count > 0)
                    {
                        foreach (PdfAttachment oneAttachment in doc.Attachments)
                        {
                            string filename = oneAttachment.FileName;
                            fileNames.Add(filename);
                            attachment = oneAttachment;
                            System.IO.File.WriteAllBytes(Model.Constants.DirectoryPath + Model.Constants.DirectoryFolderName + "/" + Model.Constants.AttachmentFolder + "/" + filename, attachment.Data);

                        }
                    }
                }
                return fileNames;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private void GetFormattedData()
        {
            Dictionary<int, string[]> words = new Dictionary<int, string[]>();
            Dictionary<string, string[]> fileData = new Dictionary<string, string[]>();
            int counter = 0;
            string shareCapital = string.Empty;
            string textData = string.Empty;

            try
            {
                // Read the file and display it line by line.  
                foreach (string line in System.IO.File.ReadLines(@"D:\Durgesh\Rubix\Project\Documents\Data.txt"))
                {
                    if (line != "")
                    {
                        //shareCapital = line.Substring(4,13);
                        if (line.Length > 20)
                        {
                            textData = line.Substring(0, 15);
                            if (textData.Contains("TOTAL"))
                            {

                            }
                            else
                            {
                                var dateTime = DateTimeRecognizer.RecognizeDateTime(line, Culture.English);
                                var numbers = NumberRecognizer.RecognizeNumber(line, Culture.English);
                                var arrayValue = numbers.ToArray();
                                List<string> amounts = new List<string>();
                                foreach (var val in arrayValue)
                                {
                                    if (val.Text.Length > 5)
                                    {
                                        amounts.Add(val.Text);
                                    }
                                }
                                fileData.Add(textData.Trim(), amounts.ToArray());
                            }
                        }
                    }
                    words.Add(counter, line.Split());
                    System.Console.WriteLine(line);
                    counter++;
                }


                foreach (KeyValuePair<string, string[]> keyValuePair in fileData)
                {
                    foreach (var value in keyValuePair.Value)
                    {
                        if (value == "")
                        {
                            //value.Value.SetValue(null,0);
                        }
                    }
                    System.IO.File.AppendAllText(@"D:\Durgesh\Rubix\Project\Documents\Data1.txt", keyValuePair.Key.Trim() + "            ");
                    System.IO.File.AppendAllLines(@"D:\Durgesh\Rubix\Project\Documents\Data1.txt", keyValuePair.Value);
                    System.IO.File.AppendAllText(@"D:\Durgesh\Rubix\Project\Documents\Data1.txt", "\n ");
                }


                string json = JsonConvert.SerializeObject(fileData, Formatting.Indented);
                System.IO.File.WriteAllText(Model.Constants.jsonFileName.Replace("txt", "json"), json);
            }
            catch(Exception ex)
            {
                throw;
            }
        }


        private void ImgShow()
        {
            Image<Bgr, byte> img = new Image<Bgr, byte>(Model.Constants.filePath);
            CvInvoke.Imshow("Image", img);
            CvInvoke.WaitKey(0);
        }
    }
}
