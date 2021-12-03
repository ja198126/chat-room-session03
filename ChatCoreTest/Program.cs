using System;
using System.Collections.Generic;
using System.Text;

namespace ChatCoreTest
{

    internal class Program
  {
        enum DataType
        {
            intt = 1,
            floatt = 2,
            stringg = 3,
        }
        private static byte[] m_PacketData;
    private static uint m_Pos;
        static int[] mPosPos;
        static int totalLength;
        static List<int> typeList;
    

    public static void Main(string[] args)
    {
      m_PacketData = new byte[1024];
      mPosPos = new int[10];
      typeList = new List<int>();
      totalLength = 0;
      m_Pos = 4;
       
      Write(109);//0 0 0 109
      Write(109.99f);//66 219 250 225
      Write("YEEE!");//0 0 0 12 0 33 0...0 72

      Console.WriteLine($"封包總長:{m_Pos}");
      Console.WriteLine($"Output Byte array(length:{totalLength}): ");
      for (var i = 0; i < m_Pos; i++)
      {
        Console.WriteLine(m_PacketData[i] + ",");
      }
      Console.WriteLine("Read:");
      Read(m_PacketData);
      var a = Console.ReadLine();

    }

    // write an integer into a byte array
    private static bool Write(int i)
    {
      // convert int to byte array
      var bytes = BitConverter.GetBytes(i);
      _Write(bytes);
            typeList.Add(1);
      return true;
    }

    // write a float into a byte array
    private static bool Write(float f)
    {
      // convert int to byte array
      var bytes = BitConverter.GetBytes(f);
      _Write(bytes);
            typeList.Add(2);

            return true;
    }

    // write a string into a byte array
    private static bool Write(string s)
    {
      // convert string to byte array
      var bytes = Encoding.Unicode.GetBytes(s);

      // write byte array length to packet's byte array
      if (Write(bytes.Length) == false)
      {
        return false;
      }
       typeList.RemoveAt(typeList.Count - 1);
      _Write(bytes);      
       typeList.Add(3);
       return true;
    }
    static void GetTotalLength(byte[] byteData)
    {
            totalLength += byteData.Length;
            var bytes = BitConverter.GetBytes(totalLength);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            bytes.CopyTo(m_PacketData, 0);
        }



    // write a byte array into packet's byte array
    private static void _Write(byte[] byteData)
    {
      GetTotalLength(byteData);
      // converter little-endian to network's big-endian
      if (BitConverter.IsLittleEndian)
      {
          Array.Reverse(byteData);
      }

      byteData.CopyTo(m_PacketData, m_Pos);
      m_Pos += (uint)byteData.Length;
            //for (int i = 0; i < mPosPos.Length; i++)
            //{
            //    if (mPosPos[i] == 0)
            //    {
            //        mPosPos[i] = (int)m_Pos - 4;
            //        break;
            //    }
            //}

        }
    static void Read(byte[] byteData)
    {
        byte[] bytes = new byte[4];
        Array.Copy(byteData,4, bytes, 0,4);
        Array.Reverse(bytes);
        var a = BitConverter.ToInt32(bytes, 0);
        Console.WriteLine(a);

        byte[] bytes1 = new byte[4];
        Array.Copy(byteData,8, bytes1, 0,4);
        Array.Reverse(bytes1);
        var b = BitConverter.ToSingle(bytes1, 0);
        Console.WriteLine(b);

            
        byte[] bytes2 = new byte[4];
        Array.Copy(byteData, 12, bytes2, 0, 4);
        Array.Reverse(bytes2);
        var stringLength = BitConverter.ToInt32(bytes2,0);
        byte[] bytes3 = new byte[stringLength];
        Array.Copy(byteData, 16, bytes3, 0, stringLength);
        Array.Reverse(bytes3);
        var c = Encoding.Unicode.GetString(bytes3);
        Console.WriteLine(c);                         
    }

  }
}


