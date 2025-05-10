using NursingHome.UI.Models;
using QuestPDF.Fluent;

namespace NursingHome.UI.PdfGenerators
{
    public class FeeBookPdfGenerator
    {
        public static byte[] Generate(FeeBookViewModel model)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);
                    page.Header()
                        .PaddingBottom(20)
                        .Text($"Таксова книга — {model.SelectedMonth}/{model.SelectedYear}")
                        .FontSize(16)
                        .SemiBold()
                        .AlignCenter();

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2);   
                            columns.ConstantColumn(30);  
                            columns.ConstantColumn(50);  
                            columns.ConstantColumn(50);  
                            columns.ConstantColumn(50);  
                            columns.ConstantColumn(50);  
                            columns.ConstantColumn(50);  
                            columns.ConstantColumn(60);  
                            columns.ConstantColumn(30);  
                            columns.ConstantColumn(60);  
                            columns.ConstantColumn(40);  
                        });

                        table.Header(header =>
                        {
                            header.Cell().BorderLeft(1).BorderRight(1).BorderTop(1).Padding(2).Text("Три имена").AlignCenter().Bold();
                            header.Cell().BorderRight(1).BorderTop(1).Padding(2).Text("Дни").AlignCenter().Bold();
                            header.Cell().BorderRight(1).BorderTop(1).Padding(2).Text("РИ").AlignCenter().Bold();
                            header.Cell().BorderRight(1).BorderTop(1).Padding(2).Text("Пенсия").AlignCenter().Bold();
                            header.Cell().BorderRight(1).BorderTop(1).Padding(2).Text("Наем").AlignCenter().Bold();
                            header.Cell().BorderRight(1).BorderTop(1).Padding(2).Text("Заплата").AlignCenter().Bold();
                            header.Cell().BorderRight(1).BorderTop(1).Padding(2).Text("Рента").AlignCenter().Bold();
                            header.Cell().BorderRight(1).BorderTop(1).Padding(2).Text("Доход").AlignCenter().Bold();
                            header.Cell().BorderRight(1).BorderTop(1).Padding(2).Text("%").AlignCenter().Bold();
                            header.Cell().BorderRight(1).BorderTop(1).Padding(2).Text("Такса").AlignCenter().Bold();
                            header.Cell().BorderRight(1).BorderTop(1).Padding(2).Text("Изкл.").AlignCenter().Bold();
                        });

                        foreach (var row in model.Rows)
                        {
                            table.Cell().BorderLeft(1).BorderRight(1).BorderTop(1).BorderBottom(1).Padding(2).AlignCenter().Text(row.FullName).FontSize(10); 
                            table.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).Padding(2).AlignCenter().Text(row.PresentDays.ToString()).FontSize(10);
                            table.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).Padding(2).AlignCenter().Text(row.RealCost.ToString("F2")).FontSize(10); 
                            table.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).Padding(2).AlignCenter().Text(row.Pension.ToString("F2")).FontSize(10); 
                            table.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).Padding(2).AlignCenter().Text(row.Rent.ToString("F2")).FontSize(10); 
                            table.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).Padding(2).AlignCenter().Text(row.Salary.ToString("F2")).FontSize(10); 
                            table.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).Padding(2).AlignCenter().Text(row.OtherIncome.ToString("F2")).FontSize(10); 
                            table.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).Padding(2).AlignCenter().Text(row.TotalIncome.ToString("F2")).FontSize(10);                                 
                            table.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).Padding(2).AlignCenter().Text(row.PercentageType).FontSize(10); 
                            table.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).Padding(2).AlignCenter().Text(row.FeeCalculated.ToString("F2")).FontSize(10); 
                            table.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).Padding(2).AlignCenter().Text(row.HasFeeException).FontSize(10); 
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Генерирано на: ").SemiBold();
                            x.Span(DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                        });
                });
            }).GeneratePdf();
        }
    }
}