# Console Screen Shot and Mail Sender

Bu basit Console uygulaması, çalıştığında arka planda alt sekmeye geçen ve belirli aralıklarla ekran görüntüsü alıp masaustune kaydeden ve e-posta atan bir uygulamadır.
Açıkçası, başlangıçta bu uygulamayı bir Windows servisi olarak tasarlamayı düşünmüştüm ancak etik dışı olabileceği endişesiyle bu fikirden vazgeçip basit bir console uygulamasına dönüştürdüm.

## Nasıl Çalıştırılır?

1. `Program.cs` dosyasındaki `SmtpClient` ve `MailMessage` ayarlarını, e-posta gönderme servisinize göre güncelleyin.

3. Uygulamayı çalıştırın.

4. Uygulama, alt sekmeye geçecek ve her 60 saniyede bir ekran görüntüsü alıp belirtilen e-posta adresine gönderilecektir.
