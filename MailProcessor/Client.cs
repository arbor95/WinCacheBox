using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Sockets;

namespace WinCachebox.MailProcessor
{
  public class Client
  {
    public String Host = "";

    public int Port = 110;

    public String User = "";

    public String Password = "";

    private TcpClient clientSocket = null;
    private StreamReader reader;
    private StreamWriter writer;

    private const int receiveTimeout = 20000;

    public bool Connected = false;

    private String lastCommandResponse = String.Empty;

    public Client()
    {

    }

    public Client(String host, String user, String password)
    {
      Connected = Connect(host, 110) && Login(user, password);
    }

    public bool Connect(string host, int port)
    {
      try
      {
        Host = host;
        Port = port;

        clientSocket = new TcpClient();
        clientSocket.Connect(host, port);

        reader = new StreamReader(clientSocket.GetStream(), Encoding.Default, true);
                writer = new StreamWriter(clientSocket.GetStream())
                {
                    AutoFlush = true
                };

                WaitForResponse(ref reader);

        return IsOkResponse(reader.ReadLine());
      }
      catch (Exception exc)
      {
#if DEBUG
        Global.AddLog("Checking for mails failed: " + exc.ToString());
#endif
        return false;
      }
    }

    public void Disconnect()
    {
      try
      {
        SendCommand("QUIT");
        reader.Close();
        writer.Close();
        if (clientSocket != null)
          clientSocket.Close();
      }
      catch
      {
      }
      finally
      {
        reader = null;
        writer = null;
        clientSocket = null;
      }
    }

    public void Dispose()
    {
      Disconnect();
    }

    public List<Mail> GetHeaders(bool acceptOnlyLatestMailWithIdenticalTitle, out List<Mail> rawList)
    {
      List<Mail> result = new List<Mail>();
      rawList = new List<Mail>();

      Dictionary<String, bool> titles = new Dictionary<string, bool>();

      int num = GetMessageCount();

      for (int i = num; i >= 1; i--)
        if (SendCommand("TOP " + i.ToString() + " 0"))
        {
          Mail mail = new Mail(i, reader);
          rawList.Add(mail);

          if (acceptOnlyLatestMailWithIdenticalTitle)
          {
            if (!titles.ContainsKey(mail.Subject))
            {
              titles.Add(mail.Subject, true);
              result.Add(mail);
            }
          }
          else
            result.Add(mail);
        }

      result.Reverse();
      return result;
    }


    public bool Login(String user, String password)
    {
      User = user;
      Password = password;

      if (!LoginMethodUser(user, password))
        if (!LoginMethodApop(user, password))
          return false;

      return true;
    }

    /// verify user and password
    /// </summary>
    /// <param name="strlogin">user name</param>
    /// <param name="strPassword">password</param>
    private bool LoginMethodUser(string strlogin, string strPassword)
    {
      if (!SendCommand("USER " + strlogin))
        return false;

      WaitForResponse(ref reader);

      if (!SendCommand("PASS " + strPassword))
        return false;

      return true;
    }

    /// <summary>
    /// verify user and password using APOP
    /// </summary>
    /// <param name="strlogin">user name</param>
    /// <param name="strPassword">password</param>
    private bool LoginMethodApop(string strlogin, string strPassword)
    {
      return SendCommand("APOP " + strlogin + " " + GetMD5HashHex(strPassword));
    }

    static string GetMD5HashHex(String input)
    {
      System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
      System.Security.Cryptography.DES des = new System.Security.Cryptography.DESCryptoServiceProvider();
      //the GetBytes method returns byte array equavalent of a string
      byte[] res = md5.ComputeHash(Encoding.Default.GetBytes(input), 0, input.Length);

      String returnThis = "";

      for (int i = 0; i < res.Length; i++)
      {
        returnThis += System.Uri.HexEscape((char)res[i]);
      }
      returnThis = returnThis.Replace("%", "");
      returnThis = returnThis.ToLower();

      return returnThis;
    }

    private void WaitForResponse(ref StreamReader rdReader)
    {
      DateTime dtStart = DateTime.Now;
      TimeSpan tsSpan;
      while (!rdReader.BaseStream.CanRead)
      {
        tsSpan = DateTime.Now.Subtract(dtStart);
        if (tsSpan.Milliseconds > receiveTimeout)
          throw new TimeoutException();

        System.Threading.Thread.Sleep(250);
      }
    }

    private bool SendCommand(string strCommand)
    {
      lastCommandResponse = "";
      try
      {
        if (writer.BaseStream.CanWrite)
        {
          writer.WriteLine(strCommand);
          writer.Flush();
          WaitForResponse(ref reader);
          lastCommandResponse = reader.ReadLine();
          return IsOkResponse(lastCommandResponse);
        }
        else
          return false;
      }
      catch (Exception exc)
      {
#if DEBUG
        Global.AddLog("Mailprocessor.Client.SendCommand(" + strCommand + ") " + exc.ToString());
#endif
        return false;
      }
    }

    private bool IsOkResponse(string strResponse)
    {
      try
      {
        if (strResponse == null)
          return false;

        return (strResponse.Substring(0, 3) == "+OK");
      }
      catch (Exception exc)
      {
#if DEBUG
        Global.AddLog("Mailprocessor.Client.IsOkResponse: " + exc.ToString());
#endif
        return false;
      }
    }

    /// <summary>
    /// get message count
    /// </summary>
    /// <returns>message count</returns>
    public int GetMessageCount()
    {
      return SendCommandIntResponse("STAT");
    }

    /// <summary>
    /// Löscht die Mail mit dem übergebenen Index beim Schließen der
    /// Verbindung. Index 1 entspricht der ältesten Mail
    /// </summary>
    /// <param name="index">Index der zu löschenden Mail. Index von 1 entspricht der ältesten Mail</param>
    public bool DeleteMessage(int index)
    {
      return SendCommand("DELE " + index.ToString());
    }

    /// <summary>
    /// get the size of a message
    /// </summary>
    /// <param name="intMessageNumber">message number</param>
    /// <returns>Size of message</returns>
    public int GetMessageSize(int index)
    {
      return SendCommandIntResponse("LIST " + index.ToString(), 2);
    }


    private int SendCommandIntResponse(String command)
    {
      return SendCommandIntResponse(command, 1);
    }

    /// <summary>
    /// Sends a command to the POP server, expects an integer reply in the response
    /// </summary>
    /// <param name="strCommand">command to send to server</param>
    /// <returns>integer value in the reply</returns>
    private int SendCommandIntResponse(string strCommand, int index)
    {
      int retVal = 0;
      if (SendCommand(strCommand))
      {
        try
        {
          retVal = int.Parse(lastCommandResponse.Split(' ')[index]);
        }
        catch (Exception)
        {
          return 0;
        }
      }
      return retVal;
    }

    /// <summary>
    /// Speichert alle Attachments auf den Datenträger
    /// </summary>
    /// <param name="mail">Mail, deren Anhänge gespeichert werden sollen</param>
    /// <param name="directory">Zielverzeichnis</param>
    /// <param name="contentType">Content-Type der zu speichernden Parts. 
    /// Wenn alle gespeichert werden sollen lassen Sie dieses Feld frei!</param>
    public void FetchAttachments(Mail mail, string directory, string contentType, bool overwriteExisting)
    {
      if (!Directory.Exists(directory))
        Directory.CreateDirectory(directory);

      // Mail anfordern
      if (!SendCommand("RETR " + mail.Index.ToString()))
        return;

      // Header überspringen und bis zur ersten Leerzeile lesen
      while (reader.ReadLine().Length > 0) ;

      bool endOfMailReached = false;

      // Ab nun nach MimeBoundary suchen
      while (!endOfMailReached)
      {
        // Part oder Ende suchen
        String line = reader.ReadLine();

        if (line == ".")
          break;

        // Beginn eines Parts?
        if (line.IndexOf(mail.MimeBoundary) != -1)
        {
          // Content-Type suchen
          String partHeader = String.Empty;

          while ((line = reader.ReadLine()).Length != 0 && line != ".")
            partHeader += line + " ";

          // Mail-Ende erreicht?
          if (line == ".")
            break;

          // Content Type extrahieren
          int startIndex = partHeader.ToLower().IndexOf("content-type:") + 13;
          int endIndex = partHeader.IndexOf(";", startIndex + 1);
          String contentTypeMail = partHeader.Substring(startIndex, endIndex - startIndex).Trim();

          // Dateinamen extrahieren
          String fullFilename = String.Empty;
          String extension = String.Empty;
          String filename = String.Empty;

          // Bei den Headern ist es etwas krude. Wenn es ein 
          // "filename"-Feld gibt nehmen wir dies. Ansonsten
          // nehmen wir das "file"-Feld und sollten ungültige Zeichen
          // entfernen.

          String[] scanWords = { "filename=", "name=" };

          for (int i = 0; i < scanWords.Length; i++)
          {
            String fieldname = scanWords[i];
            bool hasNext = i < (scanWords.Length - 1);

            if (partHeader.ToLower().IndexOf(fieldname) != -1)
            {
              int idx = partHeader.ToLower().LastIndexOf(fieldname) + fieldname.Length;
              int endIdxA = partHeader.IndexOf('"', idx + 1) - idx;
              int endIdxB = partHeader.IndexOf(' ', idx + 1) - idx;
              int endIdx = (endIdxA < 0) ? endIdxB : endIdxA;

              fullFilename = partHeader.Substring(idx, endIdx);
              fullFilename = fullFilename.Replace('"', ' ').Trim();

              idx = fullFilename.LastIndexOf('.');
              if (idx == -1)
              {
                if (hasNext)
                  continue;
                else
                {
                  filename = fullFilename;
                  extension = String.Empty;
                  break;
                }
              }
              else
              {
                filename = fullFilename.Substring(0, idx);
                extension = fullFilename.Substring(idx + 1);
                break;
              }
            }
          }

          // Nur zips akzeptieren
          bool downloadFile = filename.Length > 0 && extension.ToLower().EndsWith("zip");

          // Stimmt der Content-Type mit dem Gesuchten überein?
          if (contentTypeMail.ToLower().IndexOf(contentType.ToLower()) != -1)
          {
            // Mail verarbeiten
            Stream stream = null;

            String localFile = String.Empty;
            int cnt = 0;
            if (!overwriteExisting)
              do
              {
                String suffix = (cnt == 0) ? "" : "_" + cnt.ToString();
                cnt++;
                localFile = directory + "\\" + filename + suffix + "." + extension;
              } while (File.Exists(localFile));

            try
            {
              if (File.Exists(localFile))
                stream = new FileStream(localFile, FileMode.Truncate);
              else
                stream = new FileStream(localFile, FileMode.CreateNew);
            }
            catch (Exception)
            {

            }

            while ((line = reader.ReadLine()).IndexOf(mail.MimeBoundary) == -1 && line != ".")
            {
              if (line == null || line == "")
                continue;

              if (stream != null)
              {
                line = line.Substring(0, line.Length - line.Length % 4);
                try
                {
                  byte[] data = Convert.FromBase64String(line);
                  stream.Write(data, 0, data.Length);
                }
                catch (FormatException exc)
                {
#if DEBUG
                  Global.AddLog("Client.FetchAttachment.Base64Conversion: " + exc.ToString());
#endif
                }
              }
            }

            // Ende der Mail erreicht?
            if (line == ".")
              endOfMailReached = true;

            // Wenn ein Stream geöffnet diesen wieder schließen
            if (stream != null)
            {
              stream.Close();
              stream = null;
            }
          }
        }
      }
    }
  }
}
