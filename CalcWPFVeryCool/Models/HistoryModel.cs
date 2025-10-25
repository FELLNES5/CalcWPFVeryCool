using System.Collections.Generic;
using CalcWPFVeryCool.Base;
using CalcWPFVeryCool.Dto;

namespace CalcWPFVeryCool.Models
{
    public class HistoryModel : BaseModel
    {
        private List<HistoryItemDto> _historyItems;
        public List<HistoryItemDto> HistoryItems
        {
            get { return _historyItems; }
            set
            {
                if (_historyItems != value)
                {
                    _historyItems = value;
                    OnPropertyChanged(nameof(HistoryItems));
                }
            }
        }
    }
}
