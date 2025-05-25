using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Numerics;
using Newtonsoft.Json;

namespace CurrencyBot
{
    class Program
    {

        private static readonly Random rng = new Random();
        private DiscordSocketClient _client;
        private const string Prefix = "$";
        private const string CurrencyFile = "1234.json";
        private Dictionary<ulong, BigInteger> userCurrencies = new Dictionary<ulong, BigInteger>();
        private Dictionary<ulong, DateTime> lastHourlyUse = new Dictionary<ulong, DateTime>();


        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            });

            _client.Log += LogAsync;
            _client.MessageReceived += MessageReceivedAsync;

            LoadCurrencies();

            string token = "MTM3NDM5Njg2ODc1NDQwNzUzOA.GILWst.utQQrryieq61_ibCUa5AiCZUlj19y3DPl40JUs"; // Güvenlik için gerçek token buraya
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task LogAsync(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private void LoadCurrencies()
        {
            if (File.Exists(CurrencyFile))
            {
                var json = File.ReadAllText(CurrencyFile);
                var dictString = JsonConvert.DeserializeObject<Dictionary<ulong, string>>(json);
                userCurrencies = new Dictionary<ulong, BigInteger>();

                foreach (var kvp in dictString)
                {
                    if (BigInteger.TryParse(kvp.Value, out BigInteger val))
                    {
                        userCurrencies[kvp.Key] = val;
                    }
                    else
                    {
                        userCurrencies[kvp.Key] = BigInteger.Zero;
                    }
                }
            }
        }


        private void SaveCurrencies()
        {
            var dictString = new Dictionary<ulong, string>();
            foreach (var kvp in userCurrencies)
            {
                dictString[kvp.Key] = kvp.Value.ToString();
            }
            var json = JsonConvert.SerializeObject(dictString, Formatting.Indented);
            File.WriteAllText(CurrencyFile, json);
        }


        private async Task MessageReceivedAsync(SocketMessage message)
        {
            if (!(message is SocketUserMessage msg) || message.Author.IsBot) return;

            int argPos = 0;
            if (!msg.HasStringPrefix(Prefix, ref argPos)) return;

            string[] args = msg.Content.Substring(argPos).Split(' ');
            string command = args[0].ToLower();

            ulong userId = msg.Author.Id;

            if (command == "create")
            {
                if (userCurrencies.ContainsKey(userId))
                {
                    await msg.Channel.SendMessageAsync("❌ Zaten bir hesabınız var.");
                }
                else
                {
                    userCurrencies[userId] = new BigInteger(10000);
                    SaveCurrencies();
                    await msg.Channel.SendMessageAsync("✅ Hesabınız oluşturuldu! Başlangıç bakiyesi: 10,000 coin.");
                }
            }

            else if (command == "money")
            {
                if (userCurrencies.TryGetValue(userId, out BigInteger balance))
                {
                    await msg.Channel.SendMessageAsync($"💰 Mevcut paranız: {balance} coin.");
                }
                else
                {
                    await msg.Channel.SendMessageAsync("❌ Önce `$create` komutu ile hesap oluşturmalısınız.");
                }
            }


            else if (command == "coinflip")
            {
                if (!userCurrencies.TryGetValue(userId, out BigInteger balance))
                {
                    await msg.Channel.SendMessageAsync("❌ Önce `$create` komutu ile hesap oluşturmalısınız.");
                    return;
                }

                if (args.Length < 2 || !BigInteger.TryParse(args[1], out BigInteger amount) || amount <= 0)
                {
                    await msg.Channel.SendMessageAsync("❌ Kullanım: `$coinflip <miktar>`");
                    return;
                }

                if (amount > balance)
                {
                    await msg.Channel.SendMessageAsync("❌ Yeterli paranız yok.");
                    return;
                }

                double chance = 0.4 + rng.NextDouble() * 0.2;
                bool won = rng.NextDouble() <= chance;

                if (won)
                {
                    userCurrencies[userId] += amount;
                    await msg.Channel.SendMessageAsync($"🎉 Kazandınız! Yeni bakiyeniz: {userCurrencies[userId]} coin.");
                }
                else
                {
                    userCurrencies[userId] -= amount;
                    await msg.Channel.SendMessageAsync($"😢 Kaybettiniz. Yeni bakiyeniz: {userCurrencies[userId]} coin.");
                }

                SaveCurrencies();
            }
            else if (command == "hourly")
            {
                if (!userCurrencies.TryGetValue(userId, out BigInteger balance))
                {
                    await msg.Channel.SendMessageAsync("❌ Önce `$create` komutu ile hesap oluşturmalısınız.");
                    return;
                }

                DateTime now = DateTime.UtcNow;

                if (lastHourlyUse.TryGetValue(userId, out DateTime lastUsed))
                {
                    TimeSpan timeSinceLastUse = now - lastUsed;
                    if (timeSinceLastUse < TimeSpan.FromHours(1))
                    {
                        TimeSpan remaining = TimeSpan.FromHours(1) - timeSinceLastUse;
                        await msg.Channel.SendMessageAsync($"⏳ Bu komutu tekrar kullanabilmek için {remaining.Minutes} dakika {remaining.Seconds} saniye beklemelisiniz.");
                        return;
                    }
                }

                // 1000 coin ekle
                userCurrencies[userId] += 1000;
                lastHourlyUse[userId] = now;
                SaveCurrencies();
                await msg.Channel.SendMessageAsync("🕒 1 saatlik ödül alındı! +1000 coin 🎉");
            }
            else if (command == "leaderboard")
            {
                var sorted = new List<KeyValuePair<ulong, BigInteger>>(userCurrencies);
                sorted.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value)); // Azalan sırala

                int count = Math.Min(10, sorted.Count); // İlk 10 kişi
                string leaderboardMsg = "🏆 **Leaderboard** 🏆\n";

                for (int i = 0; i < count; i++)
                {
                    ulong uid = sorted[i].Key;
                    BigInteger balance = sorted[i].Value;
                    string username = $"<@{uid}>"; // Etiketleme (kullanıcı adı alınamazsa)

                    // Eğer kullanıcı hala sunucudaysa ismini al
                    if (_client.GetUser(uid) is SocketUser user)
                    {
                        username = user.Username;
                    }

                    leaderboardMsg += $"{i + 1}. **{username}** - {balance} coin\n";
                }

                await msg.Channel.SendMessageAsync(leaderboardMsg);
            }


        }
    }
}
