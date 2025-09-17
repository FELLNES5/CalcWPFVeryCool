using CalcWPFVeryCool.Base;
using System;
using System.Linq;
using System.Windows.Input;

namespace CalcWPFVeryCool.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        private float _num1;
        private float _num2;
        private char _operation = ' ';
        private float _output;
        private string _outputText;
        private string _outputTextSecond;
        private bool _justCalculated = false;
        private bool _isSecondNumber = false;

        public MainWindowViewModel()
        {
            //numbers
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
            //operations
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
            Output = "0";
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


        //основная логика
        private string GetAction(string Val)
        {
            //ввод цифр
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
            //вычисление
            else if (Val == "=")
            {
                if (!_isSecondNumber && _operation == ' ')
                {
                    return Output;
                }
                else return GetResult();
            }
            //ввод операции
            else if (Val == "+" || Val == "-" || Val == "*" || Val == "/")
            {
                if (!_isSecondNumber)
                {
                    _num1 = Convert.ToSingle(Output);
                    _operation = Val[0];
                    _isSecondNumber = true;
                    _justCalculated = false;
                    return _num1.ToString();
                }
                else
                {
                    _num1 = float.Parse(GetResult());
                    _operation = Val[0];
                    _isSecondNumber = true;
                    _justCalculated = false;
                    return _num1.ToString();
                }
            }
            //очистка
            else if (Val == "C")
            {
                ResetCalculator();
                return "0";
            }
            //очистка нынешнего числа
            else if (Val == "CE")
            {
                return "0";
            }
            //смена знака
            else if (Val == "+/-")
            {

                if (Output == "0")
                    return "0";

                if (Output.StartsWith("-"))
                    return Output.Substring(1);

                return "-" + Output;
            }
            //удаление символа
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
            //уствновка точки
            else if (Val == ".")
            {
                if (_justCalculated) return "0.";
                if (Output.Contains(".")) return Output;
                return Output += ".";
            }
            return "0";
        }
        //завершающие действия с дополнительным полем и возврат вычисления
        private string GetFinalValue(string inputVal)
        {
            var result = GetAction(inputVal.ToString());
            if (_isSecondNumber)
            {
                SecondOutput = $"{_num1} {_operation}";
                if (_justCalculated) SecondOutput = $"{_num1} {_operation} {_num2} =";
            }
            else
            SecondOutput = string.Empty;
            return result.ToString();
        }
        //метод сброса
        private void ResetCalculator()
        {
            _num1 = 0;
            _num2 = 0;
            _operation = ' ';
            _justCalculated = false;
            _isSecondNumber = false;
            SecondOutput = string.Empty;
        }

        public string Output
        {
            get { return _outputText; }
            set
            {
                _outputText = value;
                OnPropertyChanged(nameof(Output));
            }
        }
        public string SecondOutput
        {
            get { return _outputTextSecond; }
            set
            {
                _outputTextSecond = value;
                OnPropertyChanged(nameof(SecondOutput));
            }
        }


        #region MathOperations
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
        private string GetResult()
        {
            if (_isSecondNumber)
            {
                _num2 = Convert.ToSingle(Output);
            }
            switch (_operation)
            {
                case '+':
                    _output = Add();
                    break;
                case '-':
                    _output = Substract();
                    break;
                case '*':
                    _output = Multiply();
                    break;
                case '/':
                    _output = Divide();
                    break;
            }

            _num1 = _output;
            _justCalculated = true;
            _isSecondNumber = false;
            //SecondaryOutput.Text = string.Empty;
            return _output.ToString();
        }
        #endregion

        #region UIEvents
        private void ZeroButton(object obj) => Output = GetFinalValue("0");
        private void OneButton(object obj) => Output = GetFinalValue("1");
        private void TwoButton(object obj) => Output = GetFinalValue("2");
        private void ThreeButton(object obj) => Output = GetFinalValue("3");
        private void FourButton(object obj) => Output = GetFinalValue("4");
        private void FiveButton(object obj) => Output = GetFinalValue("5");
        private void SixButton(object obj) => Output = GetFinalValue("6");
        private void SevenButton(object obj) => Output = GetFinalValue("7");
        private void EightButton(object obj) => Output = GetFinalValue("8");
        private void NineButton(object obj) => Output = GetFinalValue("9");
        private void PlusButton(object obj) => Output = GetFinalValue("+");
        private void MinusButton(object obj) => Output = GetFinalValue("-");
        private void MultiplyButton(object obj) => Output = GetFinalValue("*");
        private void DivideButton(object obj) => Output = GetFinalValue("/");
        private void EqualsButton(object obj) => Output = GetFinalValue("=");
        private void CButton(object obj) => Output = GetFinalValue("C");
        private void CEButton(object obj) => Output = GetFinalValue("CE");
        private void NegativeButton(object obj) => Output = GetFinalValue("+/-");
        private void DelButton(object obj) => Output = GetFinalValue("<");
        private void DotButton(object obj) => Output = GetFinalValue(".");
        #endregion

    }
}
