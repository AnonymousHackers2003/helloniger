using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
//using System.Windows.Forms;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace Sudo
{
    class Program
    {
        static string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+[]{}|;:,.<>?/~`>£#$½";
            StringBuilder result = new StringBuilder(length);
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }

        private DiscordSocketClient _client;

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        /*
        public class FileLimitManager
        {
            private const string FilePath = "C:\\Users\\mseve\\source\\repos\\Sudo\\Sudo\\bin\\Debug\\controls\\userFileCounts.json"; // JSON dosyasının yolu
            private Dictionary<string, int> userFileCounts;

            public FileLimitManager()
            {
                // JSON dosyasını yükle
                if (File.Exists(FilePath))
                {
                    var json = File.ReadAllText(FilePath);
                    userFileCounts = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
                }
                else
                {
                    userFileCounts = new Dictionary<string, int>();
                }
            }

            public bool CanCreateFile(string userId)
            {
                // Kullanıcı daha önce dosya oluşturmuşsa kontrol et
                if (userFileCounts.ContainsKey(userId) && userFileCounts[userId] >= 2)
                {
                    return false; // Dosya limiti aşıldı
                }
                return true; // Dosya oluşturulabilir
            }

            public void IncrementFileCount(string userId)
            {
                if (userFileCounts.ContainsKey(userId))
                {
                    userFileCounts[userId]++;
                }
                else
                {
                    userFileCounts[userId] = 1;
                }

                // Değişiklikleri JSON dosyasına kaydet
                SaveToFile();
            }

            private void SaveToFile()
            {
                var json = JsonConvert.SerializeObject(userFileCounts, Formatting.Indented);
                File.WriteAllText(FilePath, json);
            }
        }
*/
        public async Task MainAsync()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            });

            _client.Log += LogAsync;
            _client.MessageReceived += MessageReceivedAsync;

            string token = "MTMyNTE0Njk0OTUxNDU2MzYyNg.GKHzQV.NqcSdxUEL_5y02-j09yL0LiRkyXTA3O7PCKL4I"; // Yeni tokeni buraya ekleyin

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private const string DeletePassword = "ifd285dFmZ";
        private bool _isSafeMode = false;

        private string count_1 = @"

   __   
 _/  |  
/ $$ |  
$$$$ |  
  $$ |  
  $$ |  
 _$$ |_ 
/ $$   |
$$$$$$/ 
        
        
        

";
        private string count_2 = @"

  ______  
 /      \ 
/$$$$$$  |
$$____$$ |
 /    $$/ 
/$$$$$$/  
$$ |_____ 
$$       |
$$$$$$$$/ 
          
          
          

";
        private string count_3 = @"

  ______  
 /      \ 
/$$$$$$  |
$$ ___$$ |
  /   $$< 
 _$$$$$  |
/  \__$$ |
$$    $$/ 
 $$$$$$/  
          
          
          

";
        private string count_4 = @"

 __    __ 
/  |  /  |
$$ |  $$ |
$$ |__$$ |
$$    $$ |
$$$$$$$$ |
      $$ |
      $$ |
      $$/ 
          
          
          

";

        private string count_5 = @"

 _______  
/       | 
$$$$$$$/  
$$ |____  
$$      \ 
$$$$$$$  |
/  \__$$ |
$$    $$/ 
 $$$$$$/  
          
          
          

";
        private string count_6 = @"

  ______  
 /      \ 
/$$$$$$  |
$$ \__$$/ 
$$      \ 
$$$$$$$  |
$$ \__$$ |
$$    $$/ 
 $$$$$$/  
          
          
          

";
        private string count_7 = @"

 ________ 
/        |
$$$$$$$$/ 
    /$$/  
   /$$/   
  /$$/    
 /$$/     
/$$/      
$$/       
          
          
          

";
        private string count_8 = @"

  ______  
 /      \ 
/$$$$$$  |
$$ \__$$ |
$$    $$< 
 $$$$$$  |
$$ \__$$ |
$$    $$/ 
 $$$$$$/  
          
          
          

";
        private string count_9 = @"

  ______  
 /      \ 
/$$$$$$  |
$$ \__$$ |
$$    $$ |
 $$$$$$$ |
/  \__$$ |
$$    $$/ 
 $$$$$$/  
          
          
          

";
        private string count_10 = @"

   __     ______  
 _/  |   /      \ 
/ $$ |  /$$$$$$  |
$$$$ |  $$$  \$$ |
  $$ |  $$$$  $$ |
  $$ |  $$ $$ $$ |
 _$$ |_ $$ \$$$$ |
/ $$   |$$   $$$/ 
$$$$$$/  $$$$$$/  
                  
                  
                  

";

        private async Task MessageReceivedAsync(SocketMessage socketMessage)
        {
            // Botun kendi mesajını işlememek için kontrol
            if (socketMessage.Author.IsBot) return;

            if (socketMessage.Content.StartsWith("$sudo "))
            {
                string command = socketMessage.Content.Substring(6).Trim(); // Başındaki $sudo'yu çıkarıyoruz

                if (_isSafeMode)
                {
                    // Güvenlik modu açıkken sadece "sifredegistiaktifol" komutunu dinle
                    if (command == "sifredegistiaktifol")
                    {
                        _isSafeMode = false;
                        if (socketMessage.Channel is ITextChannel textChannel)
                        {
                            var messages = (await textChannel.GetMessagesAsync(1 + 1).FlattenAsync()).ToList(); // +1 kendi komut mesajını da silmek için
                            await textChannel.DeleteMessagesAsync(messages);
                            await socketMessage.Channel.SendMessageAsync("Güvenlik modu kapatıldı. Bot tekrar aktif.");
                        }
                    }
                    else
                    {
                        if (socketMessage.Channel is ITextChannel textChannel)
                        {
                            var messages = (await textChannel.GetMessagesAsync(1 + 1).FlattenAsync()).ToList(); // +1 kendi komut mesajını da silmek için
                            await textChannel.DeleteMessagesAsync(messages);
                            await socketMessage.Channel.SendMessageAsync("Bot güvenlik modunda. Sadece `izinsiz` komutu çalışır.");
                        }
                    }
                    return;
                }

                // Normal çalışma (güvenlik modu kapalıysa)
                else if (command == "sifrebulundudeaktifol")
                {
                    _isSafeMode = true;
                    if (socketMessage.Channel is ITextChannel textChannel)
                    {
                        var messages = (await textChannel.GetMessagesAsync(1 + 1).FlattenAsync()).ToList(); // +1 kendi komut mesajını da silmek için
                        await textChannel.DeleteMessagesAsync(messages);
                        await socketMessage.Channel.SendMessageAsync("Güvenlik modu aktif edildi. Bot komutlara kapandı.");
                    }
                    return;
                }


                else if (command.StartsWith("echo ") && _isSafeMode == false)
                {
                    string output = command.Substring(5).Trim(); // "echo " kısmını çıkarıyoruz
                    await socketMessage.Channel.SendMessageAsync(output);
                }

                else if (command.StartsWith("viewuseragents") && _isSafeMode == false)
                {
                    // useragent.txt dosyasının yolu
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "useragent.txt");

                    // Dosya mevcut mu kontrol et
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            // Dosya içeriğini oku
                            await socketMessage.Channel.SendFileAsync(filePath);
                        }
                        catch (Exception ex)
                        {
                            await socketMessage.Channel.SendMessageAsync($"Hata: {ex.Message}");
                        }
                    }
                    else
                    {
                        await socketMessage.Channel.SendMessageAsync("useragent.txt dosyası bulunamadı.");
                    }
                    return;
                }

                else if (command.StartsWith("view more") && _isSafeMode == false)
                {
                    await socketMessage.Channel.SendMessageAsync("Daha fazla useragent buradan görebilirsiniz: \nhttps://gist.githubusercontent.com/pzb/b4b6f57144aea7827ae4/raw/cf847b76a142955b1410c8bcef3aabe221a63db1/user-agents.txt");
                }

                else if (command.StartsWith("asciiquarium") && _isSafeMode == false)
                {
                    await socketMessage.Channel.SendMessageAsync("https://opensource.com/sites/default/files/uploads/linux-toy-asciiquarium-animated.gif");
                }

                else if (command.StartsWith("cmatrix") && _isSafeMode == false)
                {
                    await socketMessage.Channel.SendMessageAsync("https://opensource.com/sites/default/files/uploads/linux-toy-cmatrix-animated.gif");
                }

                else if (command.StartsWith("rev") && _isSafeMode == false)
                {
                    // Kullanıcıdan gelen komutun geri kalan kısmı
                    string inputText = command.Substring(3).Trim();

                    if (string.IsNullOrWhiteSpace(inputText))
                    {
                        await socketMessage.Channel.SendMessageAsync("Tersine çevrilecek bir metin belirtmelisin.");
                        return;
                    }

                    // Metni tersine çevir
                    string reversedText = new string(inputText.Reverse().ToArray());

                    // Ters çevrilmiş metni kullanıcıya gönder
                    await socketMessage.Channel.SendMessageAsync($"{reversedText}");
                }

                else if (command.StartsWith("ls") && _isSafeMode == false)
                {
                    string RootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "root");
                    string userPath = command.Substring(2).Trim();

                    if (string.IsNullOrWhiteSpace(userPath) || userPath == "/")
                        userPath = "C:\\Users\\mseve\\source\\repos\\Sudo\\Sudo\\bin\\Debug\\root\\"; // boşsa root klasörü listele

                    if (userPath == "/")
                    {
                        userPath = "C:\\Users\\mseve\\source\\repos\\Sudo\\Sudo\\bin\\Debug\\root\\"; // kök dizin demek
                    }

                    if (userPath.StartsWith("/"))
                        userPath = userPath.Substring(1); // baştaki "/" işaretini sil

                    string fullPath = Path.Combine(RootPath, userPath.Replace("/", Path.DirectorySeparatorChar.ToString()));
                    fullPath = Path.GetFullPath(fullPath);

                    if (userPath.Contains("..") || userPath.StartsWith(@"\\") || userPath.Contains(@"\\.\") || userPath.Contains(@"\??\") || userPath.Contains(@"\\?\") || userPath.Contains(":") || userPath.Contains("..") || userPath.Contains("GLOBALROOT") || userPath.Contains("REGEDIT") || userPath.Contains("REGISTRY"))
                    {
                        await socketMessage.Channel.SendMessageAsync("Üst dizine çıkış yapamazsın!");
                        return;
                    }

                    // Root dışına çıkış kontrolü
                    if (!fullPath.StartsWith(@"C:\Users\mseve\source\repos\Sudo\Sudo\bin\Debug\root\", StringComparison.OrdinalIgnoreCase))
                    {
                        await socketMessage.Channel.SendMessageAsync("Root dizinin dışına çıkamazsın!");
                        return;
                    }

                    // Klasör var mı kontrolü
                    if (Directory.Exists(fullPath))
                    {
                        var entries = Directory.GetFileSystemEntries(fullPath);
                        if (entries.Length == 0)
                        {
                            await socketMessage.Channel.SendMessageAsync("Klasör boş.");
                        }
                        else
                        {
                            string list = string.Join("\n", entries.Select(Path.GetFileName));
                            await socketMessage.Channel.SendMessageAsync($"```\n{list}\n```");
                        }
                    }
                    else
                    {
                        await socketMessage.Channel.SendMessageAsync("Böyle bir klasör yok.");
                    }
                    return;
                }

                else if (command.StartsWith("cat") && _isSafeMode == false)
                {
                    string RootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "root");
                    string userPath = command.Substring(3).Trim();

                    if (string.IsNullOrWhiteSpace(userPath) || userPath == "/")
                    {
                        await socketMessage.Channel.SendMessageAsync("Bir dosya belirtmelisin.");
                        return;
                    }

                    if (userPath.StartsWith("/"))
                        userPath = userPath.Substring(1);

                    string fullPath = Path.Combine(RootPath, userPath.Replace("/", Path.DirectorySeparatorChar.ToString()));
                    fullPath = Path.GetFullPath(fullPath);

                    if (userPath.Contains("..") || userPath.StartsWith(@"\\") || userPath.Contains(@"\\.\") || userPath.Contains(@"\??\") || userPath.Contains(@"\\?\") || userPath.Contains(":") || userPath.Contains("..") || userPath.Contains("GLOBALROOT") || userPath.Contains("REGEDIT") || userPath.Contains("REGISTRY"))
                    {
                        await socketMessage.Channel.SendMessageAsync("Geçersiz veya erişim reddedildi.");
                        return;
                    }

                    // Root dışına çıkış kontrolü
                    if (!fullPath.StartsWith(@"C:\Users\mseve\source\repos\Sudo\Sudo\bin\Debug\root\", StringComparison.OrdinalIgnoreCase))
                    {
                        await socketMessage.Channel.SendMessageAsync("Root dizinin dışına çıkamazsın!");
                        return;
                    }

                    if (File.Exists(fullPath))
                    {
                        string content = File.ReadAllText(fullPath);

                        if (content.Length > 1900)
                            content = content.Substring(0, 1900) + "\n... (devamı var)";

                        await socketMessage.Channel.SendMessageAsync($"```\n{content}\n```");
                    }

                    if (command.Contains("/dev/urandom") || command.Contains("dev/urandom"))
                    {
                        string readyrandomtext = GenerateRandomString(1000);
                        await socketMessage.Channel.SendMessageAsync($"`{readyrandomtext}`");
                        return;
                    }

                    else
                    {
                        await socketMessage.Channel.SendMessageAsync("Dosya bulunamadı.");
                    }
                }

                else if (command.StartsWith("touch") && _isSafeMode == false)
                {
                    string userId = socketMessage.Author.Id.ToString(); // Kullanıcının ID'sini al

                    var fileManager = new FileLimitManager();

                    if (!fileManager.CanCreateFile(userId))
                    {
                        await socketMessage.Channel.SendMessageAsync("Her kullanıcı yalnızca 2 dosya oluşturabilir.");
                        return;
                    }

                    string RootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "root");
                    string userPath = command.Substring(5).Trim();

                    if (string.IsNullOrWhiteSpace(userPath) || userPath == "/")
                    {
                        await socketMessage.Channel.SendMessageAsync("Bir dosya belirtmelisin.");
                        return;
                    }

                    if (userPath.StartsWith("/"))
                        userPath = userPath.Substring(1); // "/" başını sil

                    string fullPath = Path.Combine(RootPath, userPath.Replace("/", Path.DirectorySeparatorChar.ToString()));
                    fullPath = Path.GetFullPath(fullPath);

                    // Root dışına çıkış kontrolü
                    if (userPath.Contains("..") || userPath.StartsWith(@"\\") || userPath.Contains(@"\\.\") || userPath.Contains(@"\??\") || userPath.Contains(@"\\?\") || userPath.Contains(":") || userPath.Contains("..") || userPath.Contains("GLOBALROOT") || userPath.Contains("REGEDIT") || userPath.Contains("REGISTRY"))
                    {
                        await socketMessage.Channel.SendMessageAsync("Üst dizine çıkış yapamazsın!");
                        return;
                    }

                    // Root dışına çıkış kontrolü
                    if (!fullPath.StartsWith(@"C:\Users\mseve\source\repos\Sudo\Sudo\bin\Debug\root\", StringComparison.OrdinalIgnoreCase))
                    {
                        await socketMessage.Channel.SendMessageAsync("Root dizinin dışına çıkamazsın!");
                        return;
                    }

                    try
                    {
                        if (!File.Exists(fullPath))
                        {
                            // Eğer yoksa boş dosya oluştur
                            File.Create(fullPath).Close();
                            await socketMessage.Channel.SendMessageAsync("Dosya oluşturuldu.");
                            fileManager.IncrementFileCount(userId);
                        }
                        else
                        {
                            await socketMessage.Channel.SendMessageAsync("Dosya zaten var.");
                        }
                    }
                    catch (Exception ex)
                    {
                        await socketMessage.Channel.SendMessageAsync($"Hata: {ex.Message}");
                    }
                    return;
                }

                else if (command.StartsWith("rm") && _isSafeMode == false)
                {
                    string RootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "root");
                    string userPath = command.Substring(3).Trim();

                    if (string.IsNullOrWhiteSpace(userPath) || userPath == "/")
                    {
                        await socketMessage.Channel.SendMessageAsync("Silmek için bir dosya belirtmelisin.");
                        return;
                    }

                    if (userPath.StartsWith("/"))
                        userPath = userPath.Substring(1); // "/" başını sil

                    string fullPath = Path.Combine(RootPath, userPath.Replace("/", Path.DirectorySeparatorChar.ToString()));
                    fullPath = Path.GetFullPath(fullPath);

                    if (userPath.StartsWith(@"\\") || userPath.Contains(@"\\.\") || userPath.Contains(@"\??\") || userPath.Contains(@"\\?\") || userPath.Contains(":") || userPath.Contains("..") || userPath.Contains("GLOBALROOT") || userPath.Contains("REGEDIT") || userPath.Contains("REGISTRY"))
                    {
                        await socketMessage.Channel.SendMessageAsync("Geçersiz yol veya erişim reddedildi.");
                        return;
                    }

                    // Root dışına çıkış kontrolü
                    if (!fullPath.StartsWith(@"C:\Users\mseve\source\repos\Sudo\Sudo\bin\Debug\root\", StringComparison.OrdinalIgnoreCase))
                    {
                        await socketMessage.Channel.SendMessageAsync("Root dizinin dışına çıkamazsın!");
                        return;
                    }

                    try
                    {
                        if (File.Exists(fullPath))
                        {
                            // Dosyayı sil
                            File.Delete(fullPath);
                            await socketMessage.Channel.SendMessageAsync("Dosya silindi.");
                        }
                        else
                        {
                            await socketMessage.Channel.SendMessageAsync("Dosya bulunamadı.");
                        }
                    }
                    catch (Exception ex)
                    {
                        await socketMessage.Channel.SendMessageAsync($"Hata: {ex.Message}");
                    }
                    return;
                }

                else if (command.StartsWith("count") && _isSafeMode == false)
                {
                    var message = await socketMessage.Channel.SendMessageAsync("`" + count_1 + "`");

                    await Task.Delay(1000);
                    await message.ModifyAsync(properties => properties.Content = "`" + count_2 + "`");
                    await Task.Delay(1000);
                    await message.ModifyAsync(properties => properties.Content = "`" + count_3 + "`");
                    await Task.Delay(1000);
                    await message.ModifyAsync(properties => properties.Content = "`" + count_4 + "`");
                    await Task.Delay(1000);
                    await message.ModifyAsync(properties => properties.Content = "`" + count_5 + "`");
                    await Task.Delay(1000);
                    await message.ModifyAsync(properties => properties.Content = "`" + count_6 + "`");
                    await Task.Delay(1000);
                    await message.ModifyAsync(properties => properties.Content = "`" + count_7 + "`");
                    await Task.Delay(1000);
                    await message.ModifyAsync(properties => properties.Content = "`" + count_8 + "`");
                    await Task.Delay(1000);
                    await message.ModifyAsync(properties => properties.Content = "`" + count_9 + "`");
                    await Task.Delay(1000);
                    await message.ModifyAsync(properties => properties.Content = "`" + count_10 + "`");
                }

                else if (command.StartsWith("seq "))
                {
                    string numberStr = command.Substring(4).Trim();
                    if (int.TryParse(numberStr, out int max))
                    {
                        if (max > 0 && max <= 525)
                        {
                            string output = "";
                            for (int i = 1; i <= max; i++)
                            {
                                output += i + "\n";
                            }
                            await socketMessage.Channel.SendMessageAsync($"```\n{output}```");
                        }
                        else
                        {
                            await socketMessage.Channel.SendMessageAsync("1 ile 525 arasında bir sayı girin.");
                        }
                    }
                    else
                    {
                        await socketMessage.Channel.SendMessageAsync("Geçerli bir sayı girin.");
                    }
                    return;
                }

                else if (command.StartsWith("cowsay ") && _isSafeMode == false)
                {
                    string cowMessage = command.Substring(8).Trim(); // "cowsay " kısmını çıkarıyoruz

                    // Eğer cowsay komutundan sonra boş bir mesaj gelirse, uyarı veriyoruz
                    if (string.IsNullOrWhiteSpace(cowMessage))
                    {
                        await socketMessage.Channel.SendMessageAsync("Bir mesaj gir. Mesela: `$sudo cowsay Merhaba!`");
                        return;
                    }

                    // Cowsay ASCII sanatı (Daha düzgün hizalanmış)
                    string cow = $@"
        _______________
       < {cowMessage} >
        ---------------
               \   ^__^
                \  (oo)\_______
                   (__)\       )\/\
                       ||----w |
                       ||     ||
    ";

                    // ASCII sanatı "preformatted block" içine alıyoruz
                    await socketMessage.Channel.SendMessageAsync($"```{cow}```");
                }

                else if (command.StartsWith("curl") && _isSafeMode == false)
                {
                    string userAgent = null;
                    string url = null;

                    // Command'ı parçala
                    var parts = Regex.Matches(command, @"(?<=^|\s)(?:""([^""]*)""|(\S+))")
                                     .Cast<Match>()
                                     .Select(m => m.Groups[1].Success ? m.Groups[1].Value : m.Groups[2].Value)
                                     .ToList();

                    // parts ≅ ["curl", "-A", "myagent", "https://youtube.com"]
                    for (int i = 1; i < parts.Count; i++)
                    {
                        if ((parts[i] == "-A" || parts[i] == "--user-agent") && i + 1 < parts.Count)
                        {
                            userAgent = parts[++i]; // -A parametresini al
                        }
                        else if (parts[i].StartsWith("http", StringComparison.OrdinalIgnoreCase))
                        {
                            url = parts[i]; // URL'i al
                        }
                    }

                    if (url == null)
                    {
                        await socketMessage.Channel.SendMessageAsync("Lütfen geçerli bir URL girin.");
                        return;
                    }

                    // HttpClient kullanarak URL'ye istek atma
                    using (var client = new HttpClient())
                    {
                        if (!string.IsNullOrEmpty(userAgent))
                            client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent); // User-Agent header'ını ayarla

                        HttpResponseMessage response;
                        try
                        {
                            response = await client.GetAsync(url);
                        }
                        catch (Exception ex)
                        {
                            await socketMessage.Channel.SendMessageAsync($"İstek sırasında hata: {ex.Message}");
                            return;
                        }

                        if (!response.IsSuccessStatusCode)
                        {
                            await socketMessage.Channel.SendMessageAsync($"URL erişilemedi: {response.StatusCode}");
                            return;
                        }

                        var content = await response.Content.ReadAsStringAsync();
                        if (content.Length > 1900)
                            content = content.Substring(0, 1900) + "\n...(devamı var, ilk 1900 karakter gösterildi.)"; // Çok uzun içerik varsa kısalt

                        await socketMessage.Channel.SendMessageAsync($"```{content}```");
                    }

                    return;
                }


                else if (command.StartsWith("delete ") && _isSafeMode == false)
                {
                    string args = command.Substring(7).Trim();
                    string[] parts = args.Split(' ');

                    if (parts.Length < 2)
                    {
                        await socketMessage.Channel.SendMessageAsync("Geçersiz giriş. Kullanım: `$sudo delete <sayı>`");
                        return;
                    }

                    if (!int.TryParse(parts[0], out int number))
                    {
                        await socketMessage.Channel.SendMessageAsync("Geçerli bir sayı girin.");
                        return;
                    }

                    string password = parts[1];

                    if (password != DeletePassword)
                    {
                        await socketMessage.Channel.SendMessageAsync("Silmek için izniniz yok.");
                        return;
                    }

                    if (number <= 0 || number > 100)
                    {
                        await socketMessage.Channel.SendMessageAsync("Lütfen 1 ile 100 arasında bir sayı girin.");
                        return;
                    }

                    if (socketMessage.Channel is ITextChannel textChannel)
                    {
                        var messages = (await textChannel.GetMessagesAsync(number + 1).FlattenAsync()).ToList(); // +1 kendi komut mesajını da silmek için
                        await textChannel.DeleteMessagesAsync(messages);
                        await socketMessage.Channel.SendMessageAsync($"{number} mesaj silindi.");
                    }
                    else
                    {
                        await socketMessage.Channel.SendMessageAsync("Burda silemem.");
                        return;
                    }
                }
                else if (command == "yardım" && _isSafeMode == false) // $sudo yardım komutu
                {
                    string helpMessage = @"
**Mevcut Komutlar:**
- `$sudo cowsay <mesaj>`: Mesajınızı Cowsay ASCII sanatıyla gösterir.
- `$sudo yardım`: Mevcut komutları gösterir.
- `$sudo echo <mesaj>`: Mesajınızı sohbete yazdırır.
- `$sudo expr <sayı1> <opeatör> <sayı2>`: Sonucu hesaplayarak sohbete yazdırır.
- `$sudo seq <sayı1> <sayı2>`: sayı1 den başlayarak sayı2 ye kadar sayar.
- `$sudo ls <klasör>`: Klasörün içeriğini listeler.
- `$sudo rm <dosya>`: Belirtilen dosyayı siler.
- `$sudo cat <dosya>`: Belirtilen dosyanın içeriğini sohbete yazdırır.
- `$sudo touch <dosya>`: Belirtilen dosyayı oluşturur. DIKKAT: EN FAZLA 2 DOSYA.
- `$sudo rev <mesaj>`: Mesajınızı ters olarak sohbete yazdırır.
- `$sudo curl <https://example.com>`: Web sitesinin kaynak kodunu yazdırır. (UserAgent değiştirmek için -A)
- `$sudo viewuseragents`: $sudo curl -A useragent https://example.com burdaki useragent yerine kullanabileceğiniz user agentleri listeler.
- `$sudo count`: 1 den 10 a kadar sayar.
- `$sudo asciiquarium`: ASCII akvaryum
- `$sudo cmatrix`: Matrix";

                    await socketMessage.Channel.SendMessageAsync(helpMessage); // Yardım mesajını gönderiyoruz
                }

                else if (command.StartsWith("expr ") && _isSafeMode == false)
                {
                    string[] parts = command.Substring(5).Split(' '); // Komutu ve sayıları ayırıyoruz
                    if (parts.Length != 3)
                    {
                        await socketMessage.Channel.SendMessageAsync("Doğru formatta girin: `$sudo expr <sayı1> <operatör> <sayı2>`");
                        return;
                    }

                    double num1, num2;
                    string op = parts[1];

                    if (!double.TryParse(parts[0], out num1) || !double.TryParse(parts[2], out num2))
                    {
                        await socketMessage.Channel.SendMessageAsync("Geçerli sayılar gir.");
                        return;
                    }

                    double result = 0;
                    switch (op)
                    {
                        case "+":
                            result = num1 + num2;
                            break;
                        case "-":
                            result = num1 - num2;
                            break;
                        case "*":
                            result = num1 * num2;
                            break;
                        case "/":
                            if (num2 == 0)
                            {
                                await socketMessage.Channel.SendMessageAsync("Sıfıra bölme hatası.");
                                return;
                            }
                            result = num1 / num2;
                            break;
                        default:
                            await socketMessage.Channel.SendMessageAsync("Geçersiz operatör. +, -, * veya / kullanın.");
                            return;
                    }

                    await socketMessage.Channel.SendMessageAsync($"{result}");
                }
                else
                {
                    await socketMessage.Channel.SendMessageAsync("Böyle bir komut yok veya geçerli değil. Komutlar:\nyardım\ncowsay (mesaj)\necho (mesaj)\nexpr <sayı1> <operatör> <sayı2>\nDaha fazla komut görüntüleme için $sudo yardım");
                }
            
            }
        }
    }
}
