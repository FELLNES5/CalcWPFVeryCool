using CalcWPFVeryCool.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CalcWPFVeryCool.Services
{
    // Сервис смены окон, реализуем через одиночку: ОДИН КЛАСС - ОДИН ЭКЗЕМПЛЯР.
    // Тип - строка
    // Хранение - коллекция строк
    class WindowService
    {

        // Статическая переменная - ссылка на конкретный экземпляр данного объекта
        private static WindowService _instance;

        // Статическое свойство - точка доступа к экземпляру класса, как с Output
        public static WindowService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WindowService();
                }
                return _instance;
            }
        }

        // Приватный конструктор, чтобы нельзя было создать объект извне
        private WindowService()
        { }

    }
}
