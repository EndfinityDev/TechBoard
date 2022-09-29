using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TechBoard.Containers;
using TechBoard.Types;

namespace TechBoard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Slider[] _engineSliders;
        readonly Slider[] _carSliders;

        readonly TextBox[] _engineTextBoxes;
        readonly TextBox[] _carTextBoxes;


        public MainWindow()
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture("en-US");
            CultureInfo.DefaultThreadCurrentCulture = ci;
            CultureInfo.DefaultThreadCurrentUICulture = ci;

            ClipboardManager.Init();

            InitializeComponent();

            _engineSliders = new Slider[(byte)EngineTechpoolTypes.Count];
            _engineSliders[(byte)EngineTechpoolTypes.Family] = EngineArchitechtureSlider;
            _engineSliders[(byte)EngineTechpoolTypes.BottomEnd] = BottomEndSlider;
            _engineSliders[(byte)EngineTechpoolTypes.TopEnd] = TopEndSlider;
            _engineSliders[(byte)EngineTechpoolTypes.Aspiration] = AspirationSlider;
            _engineSliders[(byte)EngineTechpoolTypes.FuelSystem] = FuelSystemSlider;
            _engineSliders[(byte)EngineTechpoolTypes.Exhaust] = ExhaustSlider;

            _engineTextBoxes = new TextBox[(byte)EngineTechpoolTypes.Count];
            _engineTextBoxes[(byte)EngineTechpoolTypes.Family] = EngineArchitechtureTextBox;
            _engineTextBoxes[(byte)EngineTechpoolTypes.BottomEnd] = BottomEndTextBox;
            _engineTextBoxes[(byte)EngineTechpoolTypes.TopEnd] = TopEndTextBox;
            _engineTextBoxes[(byte)EngineTechpoolTypes.Aspiration] = AspirationTextBox;
            _engineTextBoxes[(byte)EngineTechpoolTypes.FuelSystem] = FuelSystemTextBox;
            _engineTextBoxes[(byte)EngineTechpoolTypes.Exhaust] = ExhaustTextBox;

            _carSliders = new Slider[(byte)CarTechpoolTypes.Count];
            _carSliders[(byte)CarTechpoolTypes.Body] = BodySlider;
            _carSliders[(byte)CarTechpoolTypes.Chassis] = ChassisSlider;
            _carSliders[(byte)CarTechpoolTypes.Interior] = InteriorSlider;
            _carSliders[(byte)CarTechpoolTypes.Assists] = AssistsSlider;
            _carSliders[(byte)CarTechpoolTypes.Safety] = SafetySlider;
            _carSliders[(byte)CarTechpoolTypes.Drivetrain] = DrivetrainSlider;
            _carSliders[(byte)CarTechpoolTypes.Tyres] = TyresSlider;
            _carSliders[(byte)CarTechpoolTypes.Brakes] = BrakesSlider;
            _carSliders[(byte)CarTechpoolTypes.Aerodynamics] = AeroSlider;
            _carSliders[(byte)CarTechpoolTypes.Suspension] = SuspensionSlider;

            _carTextBoxes = new TextBox[(byte)CarTechpoolTypes.Count];
            _carTextBoxes[(byte)CarTechpoolTypes.Body] = BodyTextBox;
            _carTextBoxes[(byte)CarTechpoolTypes.Chassis] = ChassisTextBox;
            _carTextBoxes[(byte)CarTechpoolTypes.Interior] = InteriorTextBox;
            _carTextBoxes[(byte)CarTechpoolTypes.Assists] = AssistsTextBox;
            _carTextBoxes[(byte)CarTechpoolTypes.Safety] = SafetyTextBox;
            _carTextBoxes[(byte)CarTechpoolTypes.Drivetrain] = DrivetrainTextBox;
            _carTextBoxes[(byte)CarTechpoolTypes.Tyres] = TyresTextBox;
            _carTextBoxes[(byte)CarTechpoolTypes.Brakes] = BrakesTextBox;
            _carTextBoxes[(byte)CarTechpoolTypes.Aerodynamics] = AeroTextBox;
            _carTextBoxes[(byte)CarTechpoolTypes.Suspension] = SuspensionTextBox;
        }

        private void EngineSliderUpdated(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsInitialized)
                return;

            Slider slider = sender as Slider;
            for(byte i = 0; i < (byte)EngineTechpoolTypes.Count; i++)
            {
                if(_engineSliders[i] == slider)
                {
                    _engineTextBoxes[i].Text = "+" + Math.Round(slider.Value, 0).ToString();
                    return;
                }
            }
        }
        private void CarSliderUpdated(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsInitialized)
                return;

            Slider slider = sender as Slider;
            for (byte i = 0; i < (byte)CarTechpoolTypes.Count; i++)
            {
                if (_carSliders[i] == slider)
                {
                    _carTextBoxes[i].Text = "+" + Math.Round(slider.Value, 0).ToString();
                    return;
                }
            }
        }

        private void EngineAllSliderUpdated(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsInitialized)
                return;

            Slider slider = sender as Slider;
            EngineAllTextBox.Text = "+" + Math.Round(slider.Value, 0).ToString();
            
            foreach(Slider s in _engineSliders)
            {
                s.Value = slider.Value;
            }
        }
        private void CarAllSliderUpdated(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsInitialized)
                return;

            Slider slider = sender as Slider;
            CarAllTextBox.Text = "+" + Math.Round(slider.Value, 0).ToString();

            foreach (Slider s in _carSliders)
            {
                s.Value = slider.Value;
            }
        }

        private void LoadTPFromClipboard(object sender, RoutedEventArgs e)
        {
            TechpoolStruct techpool;
            try
            {
                techpool = ClipboardManager.Instance.ReadClipboard();
            }
            catch (TechpoolNotFoundException)
            {
                MessageBox.Show("No techpool was found in clipboard ", "Couldn't process techpool", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "And unknown error appeared!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            for (byte i = 0; i < (byte)EngineTechpoolTypes.Count; i++)
            {
                _engineSliders[i].Value = techpool.EngineTechpool[i];
            }
            for (byte i = 0; i < (byte)CarTechpoolTypes.Count; i++)
            {
                _carSliders[i].Value = techpool.CarTechpool[i];
            }
        }

        private void PushToClipboard(object sender, RoutedEventArgs e)
        {
            double value;

            Dictionary<EngineTechpoolTypes, double> engineTP;
            engineTP = new Dictionary<EngineTechpoolTypes, double>();

            EngineTechpoolTypes engType;
            for(byte i = 0; i < (byte)EngineTechpoolTypes.Count; i++)
            {
                engType = (EngineTechpoolTypes)i;
                value = Math.Round(_engineSliders[i].Value, 1);
                engineTP.Add(engType, value);
            }


            Dictionary<CarTechpoolTypes, double> carTP;
            carTP = new Dictionary<CarTechpoolTypes, double>();

            CarTechpoolTypes carType;
            for (byte i = 0; i < (byte)CarTechpoolTypes.Count; i++)
            {
                carType = (CarTechpoolTypes)i;
                value = Math.Round(_carSliders[i].Value, 1);
                carTP.Add(carType, value);
            }

            ClipboardManager.Instance.PushToClipboard(ref engineTP, ref carTP);
        }
    }
}
