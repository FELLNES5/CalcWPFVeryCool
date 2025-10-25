// Просто описание модельки, шаблон для записи
namespace CalcWPFVeryCool.Dto
{
    public class HistoryItemDto
    {
        public int Id { get; set; }
        public string Body => $"{SecondOutput} {OutputText}";
        public string OutputText { get; set; }
        public string SecondOutput { get; set; }
    }

    //public HistoryItemDto SelectedHistoryItem
    //{
    //    get;
    //    set;
    //}
}
