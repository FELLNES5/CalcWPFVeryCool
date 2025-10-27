using System.Linq;
using CalcWPFVeryCool.Models;
using System.Collections.Generic;
using CalcWPFVeryCool.Dto;

namespace CalcWPFVeryCool.Services
{
    // Сервис истории, реализуем через одиночку: ОДИН КЛАСС - ОДИН ЭКЗЕМПЛЯР.
    // Тип - строка
    // Хранение - коллекция строк
    // Изначально берем финальный результат OutputTextSecond + OutputText
    class HistoryService
    {
        // Статическая переменная - ссылка на конкретный экземпляр данного объекта, реализация паттерна Singleton
        public static HistoryService Instance { get; } = new HistoryService();
        private List<HistoryItemDto> _historyItems = new List<HistoryItemDto>();
        
        // Получение элементов истории
        public HistoryModel GetHistoryItems() => new HistoryModel { HistoryItems = _historyItems.OrderByDescending(p => p.Id).ToList() };
        
        // Запись значений
        public void AddHistoryItem(HistoryItemDto item)
        {
            // Сравниваем с нулем, потому что поиск максимума в пустой коллекции = исключение
            item.Id = _historyItems.Count == 0 ? 1 : _historyItems.Max(p => p.Id) + 1;
            _historyItems.Add(item);
        }
    }
}
