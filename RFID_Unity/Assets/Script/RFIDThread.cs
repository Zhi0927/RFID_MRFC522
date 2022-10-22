using UnityEngine;
using System.IO.Ports;
using System;
using System.Threading;

public class RFIDThread : MonoBehaviour
{
    #region Fields

    private static  RFIDThread  m_Instance          = null;
    private SerialPort	        m_arduinSerial      = null;
	private Thread		        m_arduinoThread     = null;
	private string		        m_uid               = string.Empty;

    [SerializeField]
    private string              PortName            = "COM3";
    [SerializeField]
    private int                 BaudRate            = 9600;

    #endregion

    #region Properties

    public static RFIDThread Instance   => m_Instance;
    public string UID                   => m_uid; 

    #endregion

    #region Unity Methods

    private void Awake()
	{
        if (m_Instance)
            Destroy(gameObject);
        else
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        m_arduinSerial = new SerialPort(PortName, BaudRate);

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

    private void OnApplicationQuit()
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

    #endregion

    #region Methods

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

    #endregion

    #region Test via Unity Methods

    private void Update()
    {
        //if (UID.Contains("1971014946"))
        //{
        //    Debug.Log($"Access Granted! UID: {UID}");
        //}


        //if (UID.Contains("24124714657"))
        //{
        //    Debug.Log($"Access Granted! UID: {UID}");
        //}
    }

    #endregion
}
