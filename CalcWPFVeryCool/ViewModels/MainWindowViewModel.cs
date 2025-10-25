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
        private float _num1;
        private float _num2;
        private char _operation = ' ';
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
        #endregion

        public ICommand HistoryToggleCommand { get; }

        // Основная логика
        private string GetAction(string Val)
        {
            // Ввод цифр
            if (char.IsDigit(Val[0]) && Val.Length == 1)
            {
                if (_justCalculated)
                {
                    ResetCalculator();
                    return Val;
                }
                if (Output == "0" || (_isSecondNumber && Output == _num1.ToString()))
                {
                    return Val;
                }
                return Output + Val;
            }

            // Очистка нынешнего числа (главного вывода)
            else if (Val == "CE")
            {
                return "0";
            }

            // Смена знака
            else if (Val == "+/-")
            {

                if (Output == "0")
                    return "0";

                if (Output.StartsWith('-'))
                    return Output.Substring(1);

                return "-" + Output;
            }

            // Удаление символа
            else if (Val == "<")
            {
                if (_justCalculated)
                {
                    ResetCalculator();
                    return "0";
                }

                var s = Output;
                if (string.IsNullOrEmpty(s) || s.Length == 1 || (s.Length == 2 && s.StartsWith("-")))
                    return "0";

                return s.Substring(0, s.Length - 1);
            }            
            return "0";
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
        
        // Показ ListBox истории
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


        #region MathOperations

        // Ввод операции
        public string SetOperation(string operation)
        {
            // Если это первый ввод операции - сохраняем первое число и операцию
            if (!_isSecondNumber)
            {
                _num1 = Convert.ToSingle(Output);
                _operation = operation[0];
                _isSecondNumber = true;
                _justCalculated = false;
                SecondOutput = $"{_num1} {_operation}";
                return _num1.ToString();
            }
            // Если операция вводится после второго числа - вычисляем, сохраняем результат и новую операцию
            else
            {
                GetResult();
                _operation = operation[0];
                _isSecondNumber = true;
                _justCalculated = false;
                SecondOutput = $"{_num1} {_operation}";
                return _num1.ToString();
            }
        }
        private float Add()
        {
            return _num1 + _num2;
        }
        private float Substract()
        {
            return _num1 - _num2;
        }
        private float Multiply()
        {
            return _num1 * _num2;
        }
        private float Divide()
        {
            if (_num2 == 0)
            {
                return _num1;
            }
            return _num1 / _num2;
        }

        // Получение результата, сохранение его в первое число и возврат
        private string GetResult()
        {
            if (!_isSecondNumber && _operation == ' ') return Output;
            if (_isSecondNumber & _operation != ' ')
            {
                _num2 = Convert.ToSingle(Output);
            }
            switch (_operation)
            {
                case '+':
                    _num1 = Add();
                    break;
                case '-':
                    _num1 = Substract();
                    break;
                case '*':
                    _num1 = Multiply();
                    break;
                case '/':
                    _num1 = Divide();
                    break;
                default: return Output;
            }
            SecondOutput = $"{_num1} {_operation} {_num2} =";
            _justCalculated = true;
            _isSecondNumber = false;
            HistoryService.Instance.AddHistoryItem(new HistoryItemDto
            {
                SecondOutput = SecondOutput,
                OutputText = Output
            });
            return _num1.ToString();
        }
        private string GetDot()
        {
            if (_justCalculated) return "0.";
            if (Output.Contains(".")) return Output;
            return Output += ".";
        }

        #endregion

        #region UIEvents
        private void ZeroButton(object obj) => Output = GetAction("0");
        private void OneButton(object obj) => Output = GetAction("1");
        private void TwoButton(object obj) => Output = GetAction("2");
        private void ThreeButton(object obj) => Output = GetAction("3");
        private void FourButton(object obj) => Output = GetAction("4");
        private void FiveButton(object obj) => Output = GetAction("5");
        private void SixButton(object obj) => Output = GetAction("6");
        private void SevenButton(object obj) => Output = GetAction("7");
        private void EightButton(object obj) => Output = GetAction("8");
        private void NineButton(object obj) => Output = GetAction("9");
        private void PlusButton(object obj) => Output = SetOperation("+");
        private void MinusButton(object obj) => Output = SetOperation("-");
        private void MultiplyButton(object obj) => Output = SetOperation("*");
        private void DivideButton(object obj) => Output = SetOperation("/");
        private void EqualsButton(object obj) => Output = GetResult();
        private void CButton(object obj) => Output = ResetCalculator();
        private void CEButton(object obj) => Output = GetAction("CE");
        private void NegativeButton(object obj) => Output = GetAction("+/-");
        private void DelButton(object obj) => Output = GetAction("<");
        private void DotButton(object obj) => Output = GetDot();
        private void HistoryToggle(object obj)
        {
            Model = HistoryService.Instance.GetHistoryItems();
            HistoryVisible = !HistoryVisible;
        }
        #endregion
    }
}
