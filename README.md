![SolBo Logo](Docs/images/solbo_logo_small.png)
# SolBo - Edukacyjny krypto bot tradingowy

Celem tego projektu jest pokazanie procesu tworzenia i działania bota, którego zadaniem będzie handel na wybranej giełdzie kryptowalutowej określoną parą.

Program ten udostępniony jest w celach edukacyjnych. Użytkownik pobiera i korzysta z aplikacji na własną odpowiedzialność. 

## Obsługiwane strategie

### 1. Buy deep sell high

W strategii tej bot śledzi zdefiniowany symbol (np. `BTCUSDT`), poprzez okresowe pobieranie ceny z giełdy za pomocą API. W momencie kiedy cena spadnie o zadeklarowany procent, bot składa na giełdzie zlecenie **BUY** typu **MARKET**. Bot dalej śledzi cenę tego symbolu i czeka na jej wzrost o zadeklarowany procent, kiedy cena ta osiągnie określony pułap bot składa na giełdzie zlecenie **SELL** typu **MARKET**. Bot realizuje również zlecenie **STOP LOSS**, jeśli po zakupie, cena spadnie o zadeklarowany procent.

Funkcje bota:

- Bot śledzi zdefiniowany symbol giełdowy (parę kryptowalutową),
- Bot śledzi zdefiniowaną kryptowalutę poprzez okresowe pobieranie jej ceny,
- Bot przechowuje średnią cenę śledzonej kryptowaluty,
- Bot składa zlecenie kupna w momencie obniżenia średniej ceny o zdefiniowaną wartość określoną w procentach,
- Bot składa zlecenie kupna określoną wartością salda (nie musi zajmować pozycji all in),
- Bot składa zlecenie sprzedaży, po zrealizowaniu zlecenia zakupu z ceną większą o określoną wartość wyrażoną w procentach,
- Bot posiada możliwość ustawienia zlecenia typu STOP LOSS.

## Obsługiwane giełdy

- [Binance](https://www.binance.com/en/register?ref=T0ANYAVJ)

## Opis konfiguracji bota

W celu poprawnego skonfigurowania bota należy odpowiednio i zgodnie z własnym poziomem wiedzy i doświadczenia uzupełnić pliki:
- `appsettings.solbo-runtime.json` - plik konfiguracyjny
- `solbo.json` - plik użytkownika

### Plik konfiguracyjny (początkujący użytkownik nie powinien edytować tego pliku)

Plik konfiguracyjny to miejsce gdzie należy zdefiniować:
- `filename` - nazwę pliku użytkownika (domyślnie **solbo**)
- `intervalinminutes` - okres w minutach co jaki bot ma łączyć się z giełdą (domyślnie **1**, bot łączy się z giełdą co 1 minutę)

Plik ten odczytywany jest tylko podczas uruchamiania bota, nie jest on odczytywany każdorazowo zgodnie z ustawionym okresem pobierania ceny z giełdy.

#### Przykładowa (domyślna) zawartość:

```
{
  "filename": "solbo",
  "intervalinminutes": 1
}
```

### Plik użytkownika

Plik użytkownika to miejsce gdzie należy zdefiniować parametry zgodnie z poniższym opisem. Edycja tego pliku i dopasowanie wartości parametrów dla własnej strategii jest niezbędne do poprawnego funkcjonowania bota.

Parametr 	| Opis 	| Przykładowa wartość 	| Typ
------------|-------|-----------------------|-----
**exchange/name**|nazwa giełdy|binance|opcjonalne
**exchange/apikey**|parametr do pobrania z giełdy|aSqaS0a5qkjy9fe05F....|opcjonalny
**exchange/apisecret**|parametr do pobrania z giełdy|0bb9eM0kB506Crdk5....|opcjonalny
**strategy/activeid**|wartość pola **id** aktywnej strategii bota|stała wartość: **1**|obowiązkowe
**strategy/available/id**|identyfikator strategii|stała wartość: **1**|obowiązkowe
**strategy/available/symbol**|symbol opisujący parę walutową (dostępną na giełdzie) np.`ETHBTC` gdzie `ETH` to base asset, a `BTC` to quote asset|`ETHBTC`|obowiązkowe
**strategy/available/storagepath**|ścieżka gdzie bot będzie zapisywał kroki, które wykonuje|Windows: `C:\\solbo\\`|obowiązkowe
**strategy/available/ticker**|pole określające rodzaj pobieranej ceny symbolu|**1** - średnia z ostatnich 5min, **0** - aktualna cena / kurs|obowiązkowe
**strategy/available/average**|liczba ostatnio pobranych wartości do wyliczania średniej ceny tj. dla wartości `5` bot będzie wyliczał średnią arytmetyczną dla 5 ostatnio pobranych wartości kursu|5|obowiązkowe
**strategy/available/buypercentagedown**|wartość procentowa określająca spadek średniej ceny po której bot składa zlecenie `BUY`|2|obowiązkowe
**strategy/available/sellpercentageup**|wartość procentowa określająca wzrost średniej ceny po której bot składa zlecenie `SELL`|5|obowiązkowe
**strategy/available/stoplosspercentagedown**|wartość procentowa określająca spadek średniej ceny po której bot składa zlecenie `STOP LOSS`|10|obowiązkowe
**strategy/available/stoplosstype**|typ określający rodzaj składanego zlecenie typu STOP LOSS|**1** - zlecenie typu STOPLOSSLIMIT, **0** - zlecenie typu SELL na MARKET. [Więcej info](https://binance-docs.github.io/apidocs/spot/en/#new-order-trade)|obowiązkowe
**strategy/available/fundpercentage**|część kapitału posiadanego na giełdze, którym bot będzie operował wyrażona w procentach|80|obowiązkowe

Użytkownik nie powinien samodzielnie modyfikować parametru **actions/bought**.

#### Przykładowa zawartość pliku

```
{
  "exchange": {
    "name": "Binance",
    "apikey": "",
    "apisecret": ""
  },
  "strategy": {
    "activeid": 1,
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
        "stoplosstype": 0,
        "fundpercentage": 80
      }
    ]
  },
  "actions": {
    "bought": 0
  }
}
```

#### Opis zachowania bota na podstawie przykładowego pliku

Bot zostanie uruchomiony w trybie testowym (puste wartości parametrów **exchange/apikey** i **exchange/apisecret**) na giełdzie Binance (zgodnie z **exchange/name**). Bot aktywuje dostępną strategię numer jeden (zgodnie z **strategy/activeid**). Strategia ta polega na handlu na parze `ETHBTC` (zgodnie z **strategy/available/symbol**) w oparciu o średnią aktualnie pobieranej ceny z ostatnich 5 (zgodnie z **strategy/available/average**) okresów. Bot złoży zlecenie typu `BUY` jeśli cena w stosunku do średniej spadnie o 2% (zgodnie z **strategy/available/buypercentagedown**). Bot złoży zlecenie typu `SELL` jeśli cena w stosunku do średniej wzrośnie o 3% (zgodnie z **strategy/available/sellpercentageup**). Bot złoży zlecenie typu `STOP LOSS` w oparciu o `SELL` (zgodnie z **strategy/available/stoplosstype**) jeśli cena w stosunku do średniej spadnie o 10% (zgodnie z **strategy/available/stoplosspercentagedown**). Bot użyje 80% (zgodnie z **strategy/available/fundpercentage**) kapitału `BTC` zdeponowanego na giełdzie przez użytkownika.

## Tryb testowy

Bot posiada tryb testowy, który "emuluje" składanie zleceń na giełdzie, techniczne zapisy kroków wykonanych przez bota można śledzić w pliku zapisywanym zgodnie z wartością parametru **strategy/available/storagepath** i nazwą zgodną z wartością parametru **strategy/available/symbol**. Tryb ten jest mocno sugerowany do użycia w pierwszym etapie korzystania z bota. Zapisy dokonywane przez bota w pliku mogą posłużyć analizie i weryfikacji poprawności działania bota w połączeniu z wykresem giełdy.

Tryb ten jest automatycznie aktywowany poprzez pozostawienie pustych parametrów **exchange/apikey** i **exchange/apisecret**.

## Tryb produkcyjny

Do użycia jedynie dla świadomych użytkowników sposobu działania bota i jego strategii. Wymaga podania wartości dla **exchanges/apikey** i **exchanges/apisecret** zgodnie z danymi pochodzącymi ze strony giełdy. Dane te dla własnego konta na Binance należy pobrać z sekcji [API Management](https://www.binance.com/en/usercenter/settings/api-management).

Tryb ten jest automatycznie aktywowany poprzez wprowadzenie poprawnych wartości dla parametrów **exchange/apikey** i **exchange/apisecret**.

## Instalacja i uruchomienie bota

Do napisania :)

## Kontakt

✉️ [https://cryptodev.tv/](https://cryptodev.tv/) - konkretna i techniczna wiedza o kryptowalutach

👨‍💻 [https://kownet.info/](https://kownet.info/) - tworzenie oprogramowania
