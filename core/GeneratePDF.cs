using pulse.collection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.IO.Image;
using pulse.forms;
using iText.Layout.Borders;
using System.Windows.Forms;

namespace pulse.core
{
    public class GeneratePDF
    {
        string path;
        Signal signal;
        List<Data> data;
        public GeneratePDF(Signal signal, List<Data> data, string path)
        {
            this.signal = signal;
            this.data = data;
            this.path = path;

            Initialize();
        }
        public void Initialize()
        {
            // Main
            var writer = new PdfWriter(path);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);
            document.SetMargins(10, 20, 5, 20);

            // Font set
            string fonts = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
            string arialuniTff = Path.Combine(fonts, "ARIALUNI.TTF");

            PdfFont font = PdfFontFactory.CreateFont(arialuniTff, "Identity-H", true);
            document.SetFont(font);

            setHeader(document);
            patientCard(document);
            statsCard(document);
            graphicCard(document, data);

            document.Close();
        }
        private void setHeader(Document document) 
        {
            document.Add(new Paragraph("Результаты анализа")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(20));
            document.Add(new LineSeparator(new SolidLine()));
        }
        private void patientCard(Document document)
        {
            Table table = new Table(2).UseAllAvailableWidth();
            var patient = signal.record.patient;

            document.Add(new Paragraph("Данные пациента:").SetFontSize(12));

            table.AddCell(new Cell(1, 1).Add(new Paragraph("Фамилия").SetFontSize(10)));
            table.AddCell(new Cell(1, 1).Add(new Paragraph(patient.surname).SetFontSize(10)));

            table.AddCell(new Cell(1, 1).Add(new Paragraph("Имя").SetFontSize(10)));
            table.AddCell(new Cell(1, 1).Add(new Paragraph(patient.name).SetFontSize(10)));

            table.AddCell(new Cell(1, 1).Add(new Paragraph("Отчетсво").SetFontSize(10)));
            table.AddCell(new Cell(1, 1).Add(new Paragraph(patient.middleName).SetFontSize(10)));

            table.AddCell(new Cell(1, 1).Add(new Paragraph("Пол").SetFontSize(10)));
            table.AddCell(new Cell(1, 1).Add(new Paragraph(patient.genderName()).SetFontSize(10)));

            table.AddCell(new Cell(1, 1).Add(new Paragraph("Дата рождения").SetFontSize(10)));
            table.AddCell(new Cell(1, 1).Add(new Paragraph(patient.birthdayDate.ToString("dd.MM.yyyy")).SetFontSize(10)));

            table.AddCell(new Cell(1, 1).Add(new Paragraph("Рост").SetFontSize(10)));
            table.AddCell(new Cell(1, 1).Add(new Paragraph(patient.height.ToString()).SetFontSize(10)));

            table.AddCell(new Cell(1, 1).Add(new Paragraph("Вес").SetFontSize(10)));
            table.AddCell(new Cell(1, 1).Add(new Paragraph(patient.weight.ToString()).SetFontSize(10)));

            document.Add(table);
        }
        private void statsCard(Document document)
        {
            Table table = new Table(2).UseAllAvailableWidth();
            var jToken = signal.ComputeStatistics();
            List<Statistic> list = VSRStatistics.assertMap(jToken);
            foreach(var stat in list)
            {
                table.AddCell(new Cell(1, 1).Add(new Paragraph(stat.key).SetFontSize(10)));
                table.AddCell(new Cell(1, 1).Add(new Paragraph(stat.value).SetFontSize(10)));
            }
            document.Add(new Paragraph("Базовые статистки:").SetFontSize(12));
            document.Add(table);
        }
        private void graphicCard(Document document, List<Data> data)
        {
            Table table = new Table(2);
            table.SetBorder(Border.NO_BORDER);
            Cell cell = new Cell();

            foreach(Data graphic in data)
            {
                cell.Add(new Paragraph("\n" + graphic.info.Key + ":").SetFontSize(12));
                cell.Add(new Image(ImageDataFactory.Create(graphic.path))
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetTextAlignment(TextAlignment.CENTER));
            }
            table.AddCell(cell);
            document.Add(table);
        }
    }

    public class Data
    {
        private string appdata = Path.Combine(Environment.GetFolderPath(
                        Environment.SpecialFolder.ApplicationData), "pulse");
        public KeyValuePair<string,string> info { set; get; }
        public string path { set; get; }
        public string description { set; get; }
        public Data(KeyValuePair<string, string> info, Control control, string description)
        {
            this.info = info;
            this.description = description;

            path = Path.Combine(appdata, string.Format("{0}.png", info.Key));
            var chart = (Chart)control;
            chart.Dock = DockStyle.None;
            chart.SaveImage(path, ChartImageFormat.Png);
        }
    }
}
