![SolBo Logo](Docs/images/solbo_logo_small.png)

> Dużo małych pieniędzy tworzy dużo pieniędzy

# SolBo - Edukacyjny krypto bot tradingowy

Celem tego projektu jest pokazanie procesu tworzenia i działania bota, którego zadaniem będzie handel na wybranej giełdzie kryptowalutowej określoną parą.

Program ten udostępniony jest w celach edukacyjnych. Użytkownik pobiera i korzysta z aplikacji na własną odpowiedzialność. 

## Obsługiwane strategie

### 1. Buy deep sell high

W strategii tej bot śledzi zdefiniowany symbol (np. `BTCUSDT`), poprzez okresowe pobieranie ceny z giełdy za pomocą API. W momencie kiedy cena spadnie o zadeklarowaną wartość (bezwzględną lub procentową), bot składa na giełdzie zlecenie **BUY** typu **MARKET**. Bot dalej śledzi cenę tego symbolu i czeka na jej wzrost o zadeklarowaną wartość (bezwzględną lub procentową), kiedy cena ta osiągnie określony pułap bot składa na giełdzie zlecenie **SELL** typu **MARKET**. Bot realizuje również zlecenie **STOP LOSS**, jeśli po zakupie, cena spadnie o zadeklarowaną wartość (bezwzględną lub procentową).

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
- [KuCoin](https://www.kucoin.com/ucenter/signup?rcode=2NNePfu)

## Opis konfiguracji bota

W celu poprawnego skonfigurowania bota należy odpowiednio i zgodnie z własnym poziomem wiedzy i doświadczenia uzupełnić pliki:
- `appsettings.solbo-runtime.json` - plik konfiguracyjny
- `solbo.json` - plik użytkownika

### Plik konfiguracyjny (początkujący użytkownik nie powinien edytować tego pliku)

Plik konfiguracyjny to miejsce gdzie należy zdefiniować:
- `filename` - nazwę pliku użytkownika (domyślnie **solbo**)
- `intervalinminutes` - okres w minutach co jaki bot ma łączyć się z giełdą (domyślnie **1**, bot łączy się z giełdą co 1 minutę)

oraz giełdę na jakiej ma grać bot:
- `exchange/type` - **0** - Binance, **1** - Kucoin
- `apikey` - klucz API dla danej giełdy
- `apisecret` - klucz SECRET danej giełdy
- `passphrase` - fraza wymagana jedynie dla giełdy Kucoin, dla Binance pole to należy zostawić puste

oraz opcjonalnie sekcje **notifications/pushover** (w celu otrzymywania powiadomień [Pushover](https://pushover.net/))
- `token` - API Token/Key z utworzonej aplikacji w serwisie Pushover,
- `recipients` - User Key otrzymany z serwisu Pushover.

Nie należy zmieniać wartości:
- `version` - parametr ten przechowuje aktualną wersję bota (zgodnie z zakładką [Releases](https://github.com/CryptoDevTV/SolBo/releases))

Plik ten odczytywany jest tylko podczas uruchamiania bota, nie jest on odczytywany każdorazowo zgodnie z ustawionym okresem pobierania ceny z giełdy.

#### Przykładowa (domyślna) zawartość:

```
{
  "version": "0.3.3",
  "filename": "solbo",
  "intervalinminutes": 1,
  "notifications": {
    "pushover": {
      "token": "",
      "recipients": "",
      "endpoint": "https://api.pushover.net/1/messages.json"
    }
  },
  "exchange": {
    "type": "1",
    "apikey": "",
    "apisecret": "",
    "passphrase": ""
  }
}
```

### Plik użytkownika

Plik użytkownika to miejsce gdzie należy zdefiniować parametry zgodnie z poniższym opisem. Edycja tego pliku i dopasowanie wartości parametrów dla własnej strategii jest niezbędne do poprawnego funkcjonowania bota.

Parametr 	| Opis 	| Przykładowa wartość 	| Typ
------------|-------|-----------------------|-----
**strategy/activeid**|wartość pola **id** aktywnej strategii bota|stała wartość: **1**|obowiązkowe
**strategy/modetype**|parametr określający czy bot pracuje (`0`) czy jest w stanie przerwy (`1`)|**0** - bot pracuje i realizuje strategie, **1** - bot śledzi cenę i ją zapisuje, nie analizuje warunków do zleceń, nie składa zleceń|obowiązkowe
**strategy/available/id**|identyfikator strategii|stała wartość: **1**|obowiązkowe
**strategy/available/symbol**|symbol opisujący parę walutową (dostępną na giełdzie) np.`ETHBTC` gdzie `ETH` to base asset, a `BTC` to quote asset|`ETHBTC`|obowiązkowe
**strategy/available/average**|liczba ostatnio pobranych wartości do wyliczania średniej ceny tj. dla wartości `5` bot będzie wyliczał średnią arytmetyczną dla 5 ostatnio pobranych wartości kursu|5|obowiązkowe
**strategy/available/averagetype**|wartość określająca czy ostatnio pobrana cena ma być uwzględniana w wyliczaniu średniej|**0** - jest uwzględniana, **1** - nie jest uwzględniana|obowiązkowe
**strategy/available/selltype**|wartość określająca czy *sellup* w przypadku **SELL** ma być wyliczana od ceny zakupu (**0**) czy od wartości wyliczonej średniej (**1**), analogicznie dla **STOPLOSS**|**0** - od ceny zakupu, **1** - od wyliczonej średniej|obowiązkowe
**strategy/available/commissiontype**|wartość określająca czy bot ma śledzić zmianę ceny w ujęciu procentowym czy wartościowym|**0** - wartości bezwzględne ceny, **1** - wartości procentowe|obowiązkowe
**strategy/available/buydown**|wartość procentowa (liczba całkowita - np. `4` lub wymierna dodatnia - np. `3.2`) określająca spadek średniej ceny po której bot składa zlecenie `BUY`|2|obowiązkowe
**strategy/available/sellup**|wartość procentowa (liczba całkowita - np. `4` lub wymierna dodatnia - np. `3.2`) określająca wzrost średniej ceny po której bot składa zlecenie `SELL`|5|obowiązkowe
**strategy/available/stoplossdown**|wartość procentowa (liczba całkowita - np. `4` lub wymierna dodatnia - np. `3.2`) określająca spadek średniej ceny po której bot składa zlecenie `STOP LOSS`, gdy wartość ustawiona na `0`, `STOP LOSS` jest **wyłączony**|10|obowiązkowe
**strategy/available/stoplosspausecycles**|wartość określające ile cykli bot czeka przed jakimkolwiek działaniem po zrealizowaniu zlecenia typu STOP LOSS|5|obowiązkowe
**strategy/available/fundpercentage**|część kapitału posiadanego na giełdze, którym bot będzie operował wyrażona w procentach|80|obowiązkowe
**strategy/available/clearonstartup**|czyszczenie pliku przechowującego ceny dla danego symbolu, **true** - czyści plik i robi kopię zapasową poprzedniego przy uruchamianiu bota, **0** - nie czyści istniejącego pliku przy uruchomieniu, pobierane ceny są zapisywane do istniejącego pliku|true|obowiązkowe

Użytkownik nie powinien samodzielnie modyfikować parametrów:
- **actions/boughtprice**,
- **actions/stoplossreached**,
- **actions/stoplosscurrentcycle**.

#### Przykładowa zawartość pliku

```
{
  "strategy": {
    "activeid": 1,
    "modetype": 0,
    "available": [
      {
        "id": 1,
        "symbol": "ALGOBTC",
        "average": 1,
        "averagetype": 0,
        "selltype": 0,
        "commissiontype": 0,
        "buydown": 0,
        "sellup": 0,
        "stoplossdown": 0,
        "stoplosspausecycles": 0,
        "fundpercentage": 100,
        "clearonstartup": false
      }
    ]
  },
  "actions": {
    "boughtprice": 0,
    "stoplossreached": false,
    "stoplosscurrentcycle": 0
  }
}
```

## Tryb testowy

Bot posiada tryb testowy, który "emuluje" składanie zleceń na giełdzie, techniczne zapisy kroków wykonanych przez bota można śledzić w pliku zapisywanym w katalogu instalacyjnym / uruchomieniowym i nazwą zgodną z wartością parametru **strategy/available/symbol**. Tryb ten jest mocno sugerowany do użycia w pierwszym etapie korzystania z bota. Zapisy dokonywane przez bota w pliku mogą posłużyć analizie i weryfikacji poprawności działania bota w połączeniu z wykresem giełdy.

Tryb ten jest automatycznie aktywowany poprzez pozostawienie pustych parametrów **apikey**, **apisecret**, **passphrase** w pliku konfiguracyjnym - `appsettings.solbo-runtime.json`.

## Tryb produkcyjny

Do użycia jedynie dla świadomych użytkowników sposobu działania bota i jego strategii. Wymaga podania wartości dla **apikey**, **apisecret**, **passphrase** w pliku konfiguracyjnym - `appsettings.solbo-runtime.json` zgodnie z danymi pochodzącymi ze strony giełdy. Dane te dla własnego konta na Binance należy pobrać z sekcji [API Management](https://www.binance.com/en/usercenter/settings/api-management).

Tryb ten jest automatycznie aktywowany poprzez wprowadzenie poprawnych wartości dla parametrów **apikey**, **apisecret**, **passphrase** w pliku konfiguracyjnym - `appsettings.solbo-runtime.json`.

### Oddzielne konto dla bota

Istotnym ze względów bezpieczeństwa jest by Solbo, działał na innym koncie niż Twoje główne konto na danej giełdzie. Będę niezwykle wdzięczny jeśli na jego potrzeby założysz konto z tego linku polecającego [Binance](https://www.binance.com/en/register?ref=T0ANYAVJ) lub [KuCoin](https://www.kucoin.com/ucenter/signup?rcode=2NNePfu), za każdą transakcję złożoną przez bota ja dostanę małą gratyfikację, Ciebie nic to nie będzie kosztować.

## Instalacja i uruchomienie bota

1. [Windows](https://youtu.be/_mPunoV0FzI)
2. [Raspberry PI](https://github.com/CryptoDevTV/SolBo/wiki/Raspberry-PI) - analogicznie na linuksach

## Kontakt

✉️ [https://cryptodev.tv/](https://cryptodev.tv/) - konkretna i techniczna wiedza o kryptowalutach

👨‍💻 [https://kownet.info/](https://kownet.info/) - tworzenie oprogramowania
