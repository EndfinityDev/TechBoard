using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TechBoard.Containers;
using TechBoard.Types;
using System.Globalization;

namespace TechBoard
{
    public class ClipboardManager
    {
        public static ClipboardManager Instance;

        readonly Dictionary<string, EngineTechpoolTypes> _engineBindings;
        readonly Dictionary<string, CarTechpoolTypes> _carBindings;

        public static void Init()
        {
            Instance = new ClipboardManager();
        }

        ClipboardManager()
        {
            _engineBindings = new Dictionary<string, EngineTechpoolTypes>();

            EngineTechpoolTypes engType;
            for (byte i = 0; i < (byte)EngineTechpoolTypes.Count; i++)
            {
                engType = (EngineTechpoolTypes)i;
                _engineBindings.Add(engType.ToString(), engType);
            }

            _carBindings = new Dictionary<string, CarTechpoolTypes>();

            CarTechpoolTypes carType;
            for (byte i = 0; i < (byte)CarTechpoolTypes.Count; i++)
            {
                carType = (CarTechpoolTypes)i;
                _carBindings.Add(carType.ToString(), carType);
            }
        }

        public TechpoolStruct ReadClipboard()
        {
            string clipboardText = Clipboard.GetText();
            string[] clipboardLines = clipboardText.Split('\n');
            if (clipboardLines[0] != "Techpool\r")
                throw new TechpoolNotFoundException(clipboardText);

            double[] engineTechpool = new double[(byte)EngineTechpoolTypes.Count];
            double[] carTechpool = new double[(byte)CarTechpoolTypes.Count];

            string[] stringParams;
            for(int i = 1; i < clipboardLines.Length; i++)
            {
                stringParams = clipboardLines[i].Replace("\r","").Split(',');
                if (stringParams.Length < 2)
                    continue;

                if(_engineBindings.TryGetValue(stringParams[0], out EngineTechpoolTypes engTechpoolType))
                {
                    engineTechpool[(byte)engTechpoolType] = double.Parse(stringParams[1], NumberStyles.Any, CultureInfo.InvariantCulture);
                }
                else if (_carBindings.TryGetValue(stringParams[0], out CarTechpoolTypes carTechpoolType))
                {
                    carTechpool[(byte)carTechpoolType] = double.Parse(stringParams[1], NumberStyles.Any, CultureInfo.InvariantCulture);
                }
            }

            return new TechpoolStruct(engineTechpool, carTechpool);
        }

        public void PushToClipboard(ref Dictionary<EngineTechpoolTypes, double> engineTP, ref Dictionary<CarTechpoolTypes, double> carTP)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Techpool\r\n");

            foreach(KeyValuePair<EngineTechpoolTypes, double> pair in engineTP)
            {
                sb.Append(pair.Key.ToString());
                sb.Append(",");
                sb.Append(pair.Value.ToString("0.0", CultureInfo.InvariantCulture));
                sb.Append("\r\n");
            }
            foreach (KeyValuePair<CarTechpoolTypes, double> pair in carTP)
            {
                sb.Append(pair.Key.ToString());
                sb.Append(",");
                sb.Append(pair.Value.ToString("0.0", CultureInfo.InvariantCulture));
                sb.Append("\r\n");
            }

            Clipboard.SetText(sb.ToString());
        }
    }

    class TechpoolNotFoundException : Exception
    {
        public string Clipboard;

        public TechpoolNotFoundException(string clipboard)
        {
            Clipboard = clipboard;
        }
    }
}
