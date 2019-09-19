using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class SimpleSerial : MonoBehaviour
{

    private SerialPort serialPort = null;
    private String portName = "COM4";  // use the port name for your Arduino, such as /dev/tty.usbmodem1411 for Mac or COM3 for PC 
    private int baudRate = 115200;  // match your rate from your serial in Arduino
    private int readTimeOut = 100;

    private string serialInput;

    bool programActive = true;
    Thread thread;

    void Start()
    {
        try
        {
            serialPort = new SerialPort();
            serialPort.PortName = portName;
            serialPort.BaudRate = baudRate;
            serialPort.ReadTimeout = readTimeOut;
            serialPort.Open();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        thread = new Thread(new ThreadStart(ProcessData));  // serial events are now handled in a separate thread
        thread.Start();
    }

    void ProcessData()
    {
        Debug.Log("Thread: Start");
        while (programActive)
        {
            try
            {
                serialInput = serialPort.ReadLine();
            }
            catch (TimeoutException)
            {

            }
        }
        Debug.Log("Thread: Stop");
    }

    void Update()
    {
        if (serialInput != null)
        {
            string[] strEul = serialInput.Split(';');  // parses using semicolon ; into a string array called strEul. I originally was sending Euler angles for gyroscopes
            if (strEul.Length > 2) // only uses the parsed data if every input expected has been received. In this case, three inputs consisting of a button (0 or 1) and two analog values between 0 and 1023
            {
                if (int.Parse(strEul[0]) != 0) // if button not pressed
                {
                    this.GetComponent<MeshRenderer>().enabled = true; //show the gameobject 

                }
                else
                {
                    this.GetComponent<MeshRenderer>().enabled = false; // hide the gameobject

                }
                this.transform.position = new Vector3(float.Parse(strEul[1]) / 100.0f, float.Parse(strEul[2]) / 100.0f, this.transform.position.z); // reposition gameobject based on the two analog values for X and Y of position
                
            }
        }
    }

    public void OnDisable()  // attempts to closes serial port when the gameobject script is on goes away
    {
        programActive = false;
        if (serialPort != null && serialPort.IsOpen)
            serialPort.Close();
    }
}