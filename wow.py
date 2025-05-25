import discord
from discord.ext import commands, tasks
import json
import asyncio
from datetime import datetime, timedelta
import random
from decimal import Decimal

intents = discord.Intents.default()
intents.message_content = True

PREFIX = "$"
CURRENCY_FILE = "1234.json"

bot = commands.Bot(command_prefix=PREFIX, intents=intents)

user_currencies = {}
last_hourly_use = {}

# Decimal kullanabiliriz, Python'da BigInteger gibi sınırsız büyüklükte sayı otomatik var.
# Decimal kullanmak, hassasiyet ve büyük sayılar için iyidir.

def load_currencies():
    global user_currencies
    try:
        with open(CURRENCY_FILE, "r") as f:
            data = json.load(f)
            user_currencies = {int(k): Decimal(v) for k, v in data.items()}
    except FileNotFoundError:
        user_currencies = {}

def save_currencies():
    with open(CURRENCY_FILE, "w") as f:
        json.dump({str(k): str(v) for k, v in user_currencies.items()}, f, indent=4)

@bot.event
async def on_ready():
    print(f"Bot hazır! Giriş yapan bot: {bot.user}")
    load_currencies()

@bot.command()
async def create(ctx):
    user_id = ctx.author.id
    if user_id in user_currencies:
        await ctx.send("❌ Zaten bir hesabınız var.")
    else:
        user_currencies[user_id] = Decimal(10000)
        save_currencies()
        await ctx.send("✅ Hesabınız oluşturuldu! Başlangıç bakiyesi: 10,000 coin.")

@bot.command()
async def money(ctx):
    user_id = ctx.author.id
    balance = user_currencies.get(user_id)
    if balance is None:
        await ctx.send("❌ Önce `$create` komutu ile hesap oluşturmalısınız.")
    else:
        await ctx.send(f"💰 Mevcut paranız: {balance} coin.")

@bot.command()
async def coinflip(ctx, amount: str = None):
    user_id = ctx.author.id
    if user_id not in user_currencies:
        await ctx.send("❌ Önce `$create` komutu ile hesap oluşturmalısınız.")
        return

    try:
        amount_val = Decimal(amount)
        if amount_val <= 0:
            raise ValueError
    except:
        await ctx.send("❌ Kullanım: `$coinflip <miktar>`")
        return

    balance = user_currencies[user_id]
    if amount_val > balance:
        await ctx.send("❌ Yeterli paranız yok.")
        return

    chance = 0.4 + random.random() * 0.2
    won = random.random() <= chance

    if won:
        user_currencies[user_id] += amount_val
        await ctx.send(f"🎉 Kazandınız! Yeni bakiyeniz: {user_currencies[user_id]} coin.")
    else:
        user_currencies[user_id] -= amount_val
        await ctx.send(f"😢 Kaybettiniz. Yeni bakiyeniz: {user_currencies[user_id]} coin.")

    save_currencies()

@bot.command()
async def hourly(ctx):
    user_id = ctx.author.id
    if user_id not in user_currencies:
        await ctx.send("❌ Önce `$create` komutu ile hesap oluşturmalısınız.")
        return

    now = datetime.utcnow()

    if user_id in last_hourly_use:
        last_used = last_hourly_use[user_id]
        if now - last_used < timedelta(hours=1):
            remaining = timedelta(hours=1) - (now - last_used)
            minutes, seconds = divmod(int(remaining.total_seconds()), 60)
            await ctx.send(f"⏳ Bu komutu tekrar kullanabilmek için {minutes} dakika {seconds} saniye beklemelisiniz.")
            return

    user_currencies[user_id] += Decimal(1000)
    last_hourly_use[user_id] = now
    save_currencies()
    await ctx.send("🕒 1 saatlik ödül alındı! +1000 coin 🎉")

@bot.command()
async def leaderboard(ctx):
    sorted_users = sorted(user_currencies.items(), key=lambda x: x[1], reverse=True)
    count = min(10, len(sorted_users))

    leaderboard_msg = "🏆 **Leaderboard** 🏆\n"
    for i in range(count):
        user_id, balance = sorted_users[i]
        member = ctx.guild.get_member(user_id)
        if member:
            username = member.name
        else:
            username = f"<@{user_id}>"
        leaderboard_msg += f"{i + 1}. **{username}** - {balance} coin\n"

    await ctx.send(leaderboard_msg)


# Tokenınızı buraya koyun (güvenlik için, bunu doğrudan paylaşmamanız gerekir)
TOKEN = "MTM3NDM5Njg2ODc1NDQwNzUzOA.GNoy4p.lxrH1sQnMjoTgkDRhEK8fIKAWY6_4Tthy6zqr0"

bot.run(TOKEN)
