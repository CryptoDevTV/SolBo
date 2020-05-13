![SolBo Logo](Docs/images/solbo_logo_small.png)
# SolBo - Edukacyjny krypto bot tradingowy

Celem tego projektu jest pokazanie w jaki sposób można podejść do tworzenia bota, którego zadaniem będzie trading na wybranej giełdzie kryptowalutowej.

Program ten udostępniony jest w celach edukacyjnych i pokazuje kroki jakie należy obrać w celu przygotowania tego typu programu. Użytkownik pobiera i korzysta z aplikacji na własną odpowiedzialność. 

## Obsługiwane strategie

### 1. Buy deep sell high

W strategii tej bot śledzi cenę zdefiniowanego symbolu (np. `BTCUSDT`), poprzez okresowe jej pobieranie z giełdy. W momencie kiedy cena spadnie o zadeklarowany procent, bot składa na giełdzie zlecenie **BUY** typu **MARKET**. Bot dalej śledzi cenę tego symbolu i czeka na jej wzrost o zadeklarowany procent, kiedy cena ta osiągnie określony pułap bot składa na giełdzie zlecenie **SELL** typu **MARKET**. Bot realizuje również zlecenie **STOP LOSS**, jeśli po zakupie, cena spadnie o zadeklarowany procent.

- Bot powinien śledzić cenę określonej kryptowaluty w stosunku do zdefiniowanej waluty,
- Bot śledzi zdefiniowaną kryptowalutę poprzez okresowe pobieranie jej ceny,
- Bot przechowuje średnią cenę śledzonej kryptowaluty,
- Bot składa zlecenie kupna w momencie obniżenia średniej ceny o zdefiniowaną wartość określoną w procentach,
- Bot składa zlecenie sprzedaży, po zrealizowaniu zlecenia zakupu z ceną większą o określoną wartość wyrażoną w procentach,
- Bot posiada możliwość ustawienia zlecenia typu STOP LOSS.

## Opis pliku konfiguracyjnego

W celu poprawnego skonfigurowania bota należy odpowiednio i zgodnie z własnym poziomem wiedzy i doświadczenia uzupełnić plik: `appsettings.solbo.json`. Plik należy uzupełnić po dokładnym zrozumieniu tego w jaki sposób działa strategia wykorzystywana przez bota.

### Parametry konfiguracyjne

Parametr 	| Opis 	| Przykładowa wartość 	| Typ
------------|-------|-----------------------|-----
**name**	|nazwa bota|solbot|opcjonalne
**exchanges/name**|nazwa giełdy|binance|opcjonalne
**exchanges/apikey**|parametr do pobrania z giełdy|aSqaS0a5qkjy9fe05Fu40yeM0kB506xXWAZ70bb9Crdk55fEzBN99hA7uLUqs01J|opcjonalny gdy **strategy/testmode** ma wartość 1
**exchanges/apisecret**|parametr do pobrania z giełdy|0bb9eM0kB506Crdk55aS0a5qkjy9fe05fEzBN99hA7uLUqs01JaSqFu40yxXWAZ7\opcjonalny gdy **strategy/testmode** ma wartość 1

```
{
  "name": "solbot",
  "exchanges": [
    {
      "name": "Binance",
      "apikey": "",
      "apisecret": ""
    }
  ],
  "strategy": {
    "activeid": 1,
    "intervalinminutes": 1,
    "testmode": 0,
    "available": [
      {
        "id": 1,
        "symbol": "ETHBTC",
        "storagepath": "C:\\solbo\\",
        "ticker": 0,
        "average": 5,
        "buypercentagedown": 2,
        "sellpercentageup": 3,
        "stoplosspercentagedown": 10
      }
    ]
  }
}
```

## Kontakt

✉️ [https://cryptodev.tv/](https://cryptodev.tv/) - konkretna i techniczna wiedza o kryptowalutach

👨‍💻 [https://kownet.info/](https://kownet.info/) - tworzenie oprogramowania