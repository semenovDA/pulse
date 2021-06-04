using pulse.collection;
using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Events;
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
using iText.Kernel.Colors;
using pulse.graphics;

namespace pulse.core
{
    public class GeneratePDF
    {
        string path;
        Signal signal;
        List<Data> data;

        public static readonly PdfNumber PORTRAIT = new PdfNumber(0);
        public static readonly PdfNumber LANDSCAPE = new PdfNumber(90);
        public static readonly PdfNumber INVERTEDPORTRAIT = new PdfNumber(180);
        public static readonly PdfNumber SEASCAPE = new PdfNumber(270);

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
            PageOrientationsEventHandler eventHandler = new PageOrientationsEventHandler();
            pdf.AddEventHandler(PdfDocumentEvent.START_PAGE, eventHandler);

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
            signalCard(eventHandler, document);
            graphicCard(document, data);
            parsCard(document);

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
        private void signalCard(PageOrientationsEventHandler eventHandler, Document document)
        {
            eventHandler.SetOrientation(LANDSCAPE);
            document.Add(new AreaBreak());

            var sInfo = new KeyValuePair<string, string>("Сигнал", "SIGNAL");
            var signalChart = new SignalChart(signal).chart;

            signalChart.Size = new System.Drawing.Size(1000, 300);
            signalChart.ChartAreas[0].AxisX.ScaleView
                .Zoom(0, signalChart.Series[0].Points.Count);
            signalChart.ChartAreas[0].AxisX.LabelStyle.Interval = 1000;

            var signalPath = new Data(sInfo, signalChart, "").path;
            var signalImg = new Image(ImageDataFactory.Create(signalPath))
                                    .SetTextAlignment(TextAlignment.CENTER)
                                    .SetTextAlignment(TextAlignment.CENTER);
            signalImg.SetProperty(Property.ROTATION_ANGLE, 90 * 0.0174533);
            signalImg.ScaleToFit(800, 300);

            var hInfo = new KeyValuePair<string, string>("DISTRIBUTION_HISTOGRAM_SIGNAL", "Гисторграмма распределение сигнала");
            var histChart = new Histogram(signal, false).chart;

            histChart.Size = new System.Drawing.Size(1000, 400);
            histChart.ChartAreas[0].AxisX.LabelStyle.Interval = 5;
            histChart.ChartAreas[0].AxisX.ScaleView.ZoomReset();

            var histPath = new Data(hInfo, histChart, "").path;
            var histImg = new Image(ImageDataFactory.Create(histPath))
                                    .SetTextAlignment(TextAlignment.CENTER)
                                    .SetTextAlignment(TextAlignment.CENTER);
            histImg.SetProperty(Property.ROTATION_ANGLE, 90 * 0.0174533);
            histImg.ScaleToFit(800, 400);

            var table = new Table(2);
            table.AddCell(new Cell(1, 1).Add(signalImg));
            table.AddCell(new Cell(1, 1).Add(histImg));

            document.Add(table);

            eventHandler.SetOrientation(PORTRAIT);
            document.Add(new AreaBreak());
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
        private void parsCard(Document document)
        {
            document.Add(new Paragraph("Функциональная оценка состояние: ").SetFontSize(12));

            Table resultTable = new Table(UnitValue.CreatePercentArray(14))
                                                        .UseAllAvailableWidth();
            
            var pars = signal.ComputePars().ToObject<int>();
            var dataTable = generateDataTable(pars);
            Cell cell = new Cell(10, 10).Add(dataTable);
            resultTable.AddCell(cell);

            cell = new Cell(3, 4).Add(new Paragraph("Физиологическая норма"));
            resultTable.AddCell(cell.SetHeight(75).SetTextAlignment(TextAlignment.CENTER));
            
            cell = new Cell(4, 4).Add(new Paragraph("Донозологическое состояния\nПреморбидные состояния"));
            resultTable.AddCell(cell.SetHeight(100).SetTextAlignment(TextAlignment.CENTER));
            
            cell = new Cell(3, 4).Add(new Paragraph("Срыв адаптации"));
            resultTable.AddCell(cell.SetHeight(75).SetTextAlignment(TextAlignment.CENTER));

            document.Add(resultTable);
        }

        private Table generateDataTable(int pars)
        {
            Table dataTable = new Table(10);

            var number = 1;
            for (var i = 0; i < 100; i++)
            {
                Paragraph text;
                Cell cell = new Cell(1, 1);

                cell.SetBackgroundColor(ColorConstants.GREEN);
                if (i > 29) cell.SetBackgroundColor(ColorConstants.YELLOW);
                if (i > 69) cell.SetBackgroundColor(ColorConstants.RED);

                if (i % 11 == 0)
                {
                    text = new Paragraph(number.ToString());
                    if (pars == number) cell.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                    //else cell.SetBorder(Border.NO_BORDER);
                    number++;
                }
                else
                {
                    //cell.SetBorder(Border.NO_BORDER);
                    text = new Paragraph("\n");
                }

                cell.Add(text.SetTextAlignment(TextAlignment.CENTER));
                dataTable.AddCell(cell);
            }

            dataTable.UseAllAvailableWidth();
            return dataTable;
        }
        private class PageOrientationsEventHandler : IEventHandler
        {
            private PdfNumber orientation = PORTRAIT;

            public void SetOrientation(PdfNumber orientation)
            {
                this.orientation = orientation;
            }

            public void HandleEvent(Event currentEvent)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)currentEvent;
                docEvent.GetPage().Put(PdfName.Rotate, orientation);
            }
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
