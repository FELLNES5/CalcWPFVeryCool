using CalcWPFVeryCool.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CalcWPFVeryCool.Services
{
    // Сервис истории, реализуем через одиночку: ОДИН КЛАСС - ОДИН ЭКЗЕМПЛЯР.
    // Тип - строка
    // Хранение - коллекция строк
    // Изначально берем финальный результат SecondOutput
    class HistoryService
    {

        // Статическая переменная - ссылка на конкретный экземпляр данного объекта
        private static HistoryService _instance;

        // Статическое свойство - точка доступа к экземпляру класса, как с Output
        public static HistoryService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new HistoryService();
                }
                return _instance;
            }
        }

        // Приватный конструктор, чтобы нельзя было создать объект извне
        private HistoryService()
        { }

    }
}
