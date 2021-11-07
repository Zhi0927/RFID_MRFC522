using UnityEngine;
using System.IO.Ports;
using System;
using System.Threading;

public class RFIDThread : MonoBehaviour
{
	private SerialPort	m_arduinSerial;
	private Thread		m_arduinoThread;
	private string		m_uid = string.Empty;
	public string UID { get { return m_uid; } }

    private void Start()
	{
        //var portname = GetSerialPortName.AutoDetectArduinoPort();
        //m_arduinSerial = new SerialPort(portname, 9600);

        m_arduinSerial = new SerialPort("COM4", 9600);
        m_arduinSerial.ReadTimeout = 500;
		try
		{
			m_arduinSerial.Open();
			m_arduinoThread = new Thread(new ThreadStart(ArduinoRead));
			m_arduinoThread.Start();
			Debug.Log("Connection success!");
		}
		catch(Exception e)
		{
			Debug.LogWarning(e.Message);
		}
	}

    private void Update()
    {
        Debug.Log(UID);
        if (UID.Contains("13812013137"))
        {
            Debug.Log("Access Granted! UID: 13812013137");
        }


        if (UID.Contains("169181236151"))
        {
            Debug.Log("Access Granted! UID: 169181236151");
        }
    }

    private void ArduinoRead()
    {
        while (m_arduinSerial.IsOpen)
        {
            try
            {
                var newRead = m_arduinSerial.ReadLine().Trim();
                m_uid = newRead;
            }
            catch (Exception e)
            {
                m_uid = string.Empty;
                Debug.LogWarning(e.Message);               
            }
        }
    }

    public void ArduinoWrite(string message)
    {
        try
        {
            m_arduinSerial.Write(message);
            Debug.Log(message);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }

    void OnApplicationQuit()
    {
        if (m_arduinSerial != null)
        {
            if (m_arduinSerial.IsOpen)
            {
                m_arduinSerial.Close();
                m_arduinoThread.Abort();
            }
        }
    }
}
