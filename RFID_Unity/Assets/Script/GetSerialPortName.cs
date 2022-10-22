using System.Collections.Generic;
using System.IO.Ports;
using Microsoft.Win32;

public class GetSerialPortName
{
    #region Methods 

    public static string AutoDetectArduinoPort()
    {
        List<string> comports = new List<string>();
        RegistryKey rk1 = Registry.LocalMachine;
        RegistryKey rk2 = rk1.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum");
        string temp;
        foreach (string s3 in rk2.GetSubKeyNames())
        {
            RegistryKey rk3 = rk2.OpenSubKey(s3);
            foreach (string s in rk3.GetSubKeyNames())
            {
                if (s.Contains("VID") && s.Contains("PID"))
                {
                    RegistryKey rk4 = rk3.OpenSubKey(s);
                    foreach (string s2 in rk4.GetSubKeyNames())
                    {
                        RegistryKey rk5 = rk4.OpenSubKey(s2);
                        if ((temp = (string)rk5.GetValue("FriendlyName")) != null && temp.Contains("Arduino"))
                        {
                            RegistryKey rk6 = rk5.OpenSubKey("Device Parameters");
                            if (rk6 != null && (temp = (string)rk6.GetValue("PortName")) != null)
                            {
                                comports.Add(temp);
                            }
                        }
                    }
                }
            }
        }

        if (comports.Count > 0)
        {
            foreach (string s in SerialPort.GetPortNames())
            {
                if (comports.Contains(s))
                    return s;
            }
        }

        return "COM3";
    }

    #endregion
}