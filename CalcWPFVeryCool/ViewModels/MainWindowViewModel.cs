using CalcWPFVeryCool.Base;
using System;
using System.Windows.Input;
using CalcWPFVeryCool.Services;
using CalcWPFVeryCool.Models;
using CalcWPFVeryCool.Dto;

namespace CalcWPFVeryCool.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        private decimal _num1;
        private decimal _num2;
        private char _operation;
        private string _outputText = "0";
        private string _outputTextSecond;
        private bool _justCalculated = false;
        private bool _isSecondNumber = false;
        private bool _historyVisible = false;
        private HistoryModel _model;
        public MainWindowViewModel()
        {
            #region ButtonInit
            // Numbers
            ZeroButtonCommand = new RelayCommand(ZeroButton);
            OneButtonCommand = new RelayCommand(OneButton);
            TwoButtonCommand = new RelayCommand(TwoButton);
            ThreeButtonCommand = new RelayCommand(ThreeButton);
            FourButtonCommand = new RelayCommand(FourButton);
            FiveButtonCommand = new RelayCommand(FiveButton);
            SixButtonCommand = new RelayCommand(SixButton);
            SevenButtonCommand = new RelayCommand(SevenButton);
            EightButtonCommand = new RelayCommand(EightButton);
            NineButtonCommand = new RelayCommand(NineButton);
            // Operations
            PlusButtonCommand = new RelayCommand(PlusButton);
            MinusButtonCommand = new RelayCommand(MinusButton);
            MultiplyButtonCommand = new RelayCommand(MultiplyButton);
            DivideButtonCommand = new RelayCommand(DivideButton);
            EqualsButtonCommand = new RelayCommand(EqualsButton);
            CButtonCommand = new RelayCommand(CButton);
            CEButtonCommand = new RelayCommand(CEButton);
            NegativeButtonCommand = new RelayCommand(NegativeButton);
            DelButtonCommand = new RelayCommand(DelButton);
            DotButtonCommand = new RelayCommand(DotButton);

            HistoryToggleCommand = new RelayCommand(HistoryToggle);

            //RestoreFromHistoryCommand = new RelayCommand(RestoreFromHistory);
            #endregion
        }

        #region NumCommands
        public ICommand ZeroButtonCommand { get; }
        public ICommand OneButtonCommand { get; }
        public ICommand TwoButtonCommand { get; }
        public ICommand ThreeButtonCommand { get; }
        public ICommand FourButtonCommand { get; }
        public ICommand FiveButtonCommand { get; }
        public ICommand SixButtonCommand { get; }
        public ICommand SevenButtonCommand { get; }
        public ICommand EightButtonCommand { get; }
        public ICommand NineButtonCommand { get; }
        #endregion

        #region OperatonCommands
        //operations
        public ICommand PlusButtonCommand { get; }
        public ICommand MinusButtonCommand { get; }
        public ICommand MultiplyButtonCommand { get; }
        public ICommand DivideButtonCommand { get; }
        public ICommand EqualsButtonCommand { get; }
        public ICommand CButtonCommand { get; }
        public ICommand CEButtonCommand { get; }
        public ICommand NegativeButtonCommand { get; }
        public ICommand DelButtonCommand { get; }
        public ICommand DotButtonCommand { get; }
        public ICommand RestoreFromHistoryCommand { get; }
        public ICommand HistoryToggleCommand { get; }
        #endregion

        #region Masks
        // Главное окно вывода
        public string Output
        {
            get { return _outputText; }
            set
            {
                _outputText = value;
                OnPropertyChanged(nameof(Output));
            }
        }
        
        // Дополнительное окно вывода
        public string SecondOutput
        {
            get { return _outputTextSecond; }
            set
            {
                _outputTextSecond = value;
                OnPropertyChanged(nameof(SecondOutput));
            }
        }
        
        // Переключение видимости истории
        public bool HistoryVisible
        {
            get { return _historyVisible; }
            set
            {
                if (_historyVisible != value)
                { 
                    _historyVisible = value;
                    OnPropertyChanged(nameof(HistoryVisible));
                }
            }
        }
        
        // Коллекция истории
        public HistoryModel Model
            {
            get { return _model; }
            set
            {
                if (_model != value)
                {
                    _model = value;
                    OnPropertyChanged(nameof(Model));
                }
            }
        }
        #endregion

        #region CalculatorLogic
        // Получение цифр
        private string GetNumber(decimal Val)
        {
            // Ввод цифр
            if (_justCalculated)
            {
                ResetCalculator();
                return $"{Val}";
            }
            if (Output == "0" || (_isSecondNumber && Output == _num1.ToString()))
            {
                return $"{Val}";
            }
            return $"{Output}{Val}";
        }

        // Метод сброса
        private string ResetCalculator()
        {
            _num1 = 0;
            _num2 = 0;
            _operation = ' ';
            _justCalculated = false;
            _isSecondNumber = false;
            SecondOutput = string.Empty;
            return "0";
        }

        // Ввод операции
        public string SetOperation(string operation)
        {
            if (_isSecondNumber) GetResult();

            _num1 = Convert.ToDecimal(Output);
            _operation = operation[0];
            _isSecondNumber = true;
            _justCalculated = false;
            SecondOutput = $"{_num1} {_operation}";
            return _num1.ToString();
        }

        // Получение результата, сохранение его в первое число и возврат
        private string GetResult()
        {
            if (_operation == ' ') return Output;
            if (_isSecondNumber)
            {
                _num2 = Convert.ToDecimal(Output);
            }

            SecondOutput = $"{_num1} {_operation} {_num2} =";

            switch (_operation)
            {
                case '+':
                    _num1 += + _num2;
                    break;
                case '-':
                    _num1 -= _num2;
                    break;
                case '*':
                    _num1 *= _num2;
                    break;
                case '/':
                    if (_num2 == 0)
                    {
                        Output = "На ноль делить нельзя";
                        _justCalculated = true;
                        _isSecondNumber = false;
                        return Output;
                    }
                    _num1 /= _num2;
                    break;
            }

            _justCalculated = true;
            _isSecondNumber = false;

            Output = _num1.ToString();

            HistoryService.Instance.AddHistoryItem(new HistoryItemDto
            {
                SecondOutput = SecondOutput,
                OutputText = Output
            });
            return Output;
        }
        private string GetDot()
        {
            if (_justCalculated || (_isSecondNumber && Output == _num1.ToString())) return "0.";
            if (Output.Contains('.')) return Output;
            return Output + ".";
        }
        private static string ClearElement()
        {
            return "0";
        }
        private string DenialElement()
        {
            if (Output == "0") return "0";
            if (Output.StartsWith('-'))
                return Output.Substring(1);
            return "-" + Output;
        }
        private string DeleteSymbol()
        {
            if (_justCalculated)
            {
                ResetCalculator();
                return "0";
            }
            var s = Output;
            if (string.IsNullOrEmpty(s) || s.Length == 1 || (s.Length == 2 && s.StartsWith('-')))
                return "0";
            return s.Substring(0, s.Length - 1);
        }

        //private void RestoreFromHistory(HistoryItemDto item)
        //{

        //}

        #endregion

        #region UIEvents
        private void ZeroButton(object obj) => Output = GetNumber(0);
        private void OneButton(object obj) => Output = GetNumber(1);
        private void TwoButton(object obj) => Output = GetNumber(2);
        private void ThreeButton(object obj) => Output = GetNumber(3);
        private void FourButton(object obj) => Output = GetNumber(4);
        private void FiveButton(object obj) => Output = GetNumber(5);
        private void SixButton(object obj) => Output = GetNumber(6);
        private void SevenButton(object obj) => Output = GetNumber(7);
        private void EightButton(object obj) => Output = GetNumber(8);
        private void NineButton(object obj) => Output = GetNumber(9);
        private void PlusButton(object obj) => Output = SetOperation("+");
        private void MinusButton(object obj) => Output = SetOperation("-");
        private void MultiplyButton(object obj) => Output = SetOperation("*");
        private void DivideButton(object obj) => Output = SetOperation("/");
        private void EqualsButton(object obj) => GetResult();
        private void CButton(object obj) => Output = ResetCalculator();
        private void CEButton(object obj) => Output = ClearElement();
        private void NegativeButton(object obj) => Output = DenialElement();
        private void DelButton(object obj) => Output = DeleteSymbol();
        private void DotButton(object obj) => Output = GetDot();
        private void HistoryToggle(object obj)
        {
            Model = HistoryService.Instance.GetHistoryItems();
            HistoryVisible = !HistoryVisible;
        }
        #endregion
    }
}
