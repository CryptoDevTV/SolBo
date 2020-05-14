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

## Obsługiwane giełdy

- [Binance](https://www.binance.com/en/register?ref=T0ANYAVJ)

## Opis pliku konfiguracyjnego

W celu poprawnego skonfigurowania bota należy odpowiednio i zgodnie z własnym poziomem wiedzy i doświadczenia uzupełnić plik: `appsettings.solbo.json`. Plik należy uzupełnić po dokładnym zrozumieniu tego w jaki sposób działa strategia wykorzystywana przez bota.

### Parametry konfiguracyjne

Parametr 	| Opis 	| Przykładowa wartość 	| Typ
------------|-------|-----------------------|-----
**name**	|nazwa bota|solbot|opcjonalne
**exchanges/name**|nazwa giełdy|binance|opcjonalne
**exchanges/apikey**|parametr do pobrania z giełdy|aSqaS0a5qkjy9fe05F....|opcjonalny gdy **strategy/testmode** ma wartość 1
**exchanges/apisecret**|parametr do pobrania z giełdy|0bb9eM0kB506Crdk5....|opcjonalny gdy **strategy/testmode** ma wartość 1
**strategy/activeid**|wartość pola **id** aktywnej strategii bota|stała wartość: **1**|obowiązkowe
**strategy/intervalinminutes**|czas w minutach, co jaki bot będzie odpytywał giełde o cenę|5|obowiązkowe
**strategy/testmode**|aktywacja trybu testowego lub działanie produkcyjne|**1** - tryb testowy, **0** - tryb produkcyjny|obowiązkowe
**strategy/available/id**|identyfikator strategii|stała wartość: **1**|obowiązkowe
**strategy/available/symbol**|symbol opisujący parę walutową tj.`ETHBTC` gdzie `ETH` to base asset, a `BTC` to quote asset|`ETHBTC`|obowiązkowe
**strategy/available/storagepath**|ścieżka gdzie bot będzie zapisywał kroki, które wykonuje|Windows: `C:\\solbo\\`|obowiązkowe
**strategy/available/ticker**|pole określające rodzaj pobieranej ceny symbolu|**1** - średnia z ostatnich 5min, **0** - aktualna cena / kurs|obowiązkowe
**strategy/available/average**|liczba ostatnio pobranych wartości do wyliczania średniej ceny tj. dla wartości `5` bot będzie wyliczał średnią arytmetyczną dla 5 ostatnio pobranych wartości kursu|5|obowiązkowe
**strategy/available/buypercentagedown**|wartość procentowa określająca spadek średniej ceny po której bot składa zlecenie `BUY`|2|obowiązkowe
**strategy/available/sellpercentageup**|wartość procentowa określająca wzrost średniej ceny po której bot składa zlecenie `SELL`|5|obowiązkowe
**strategy/available/stoplosspercentagedown**|wartość procentowa określająca spadek średniej ceny po której bot składa zlecenie `STOP LOSS`|10|obowiązkowe
**strategy/available/fundpercentage**|część kapitału posiadanego na giełdze, którym bot będzie operował wyrażona w procentach|80|obowiązkowe

#### Tryb testowy

Bot posiada tryb testowy, który "emuluje" składanie zleceń na giełdzie, techniczne zapisy kroków wykonanych przez bota można śledzić w pliku zapisywanym zgodnie z wartością parametru **strategy/available/storagepath**. Tryb ten jest mocno sugerowany do użycia w pierwszym etapie korzystania z bota i nie wymaga nawet rejestracji na giełdzie. Zapisy dokonywane przez bota w pliku mogą posłużyć analizie i weryfikacji poprawności działania bota w połączeniu z wykresem giełdy.

#### Tryb produkcyjny

Do użycia jedynie dla świadomych użytkowników sposobu działania bota i jego strategii. Wymaga podania wartości dla **exchanges/apikey** i **exchanges/apisecret** zgodnie z danymi pochodzącymi ze strony giełdy. Dane te dla własnego konta na Binance należy pobrać z sekcji [API Management](https://www.binance.com/en/usercenter/settings/api-management).

#### Przykładowa zawartość pliku

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
        "stoplosspercentagedown": 10,
        "fundpercentage": 80 
      }
    ]
  }
}
```

## Instalacja i uruchomienie bota

Do napisania :)

## Kontakt

✉️ [https://cryptodev.tv/](https://cryptodev.tv/) - konkretna i techniczna wiedza o kryptowalutach

👨‍💻 [https://kownet.info/](https://kownet.info/) - tworzenie oprogramowania