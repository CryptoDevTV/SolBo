![SolBo Logo](Docs/images/solbo_logo_small.png)

> Dużo małych pieniędzy tworzy dużo pieniędzy

# SolBo - Edukacyjny krypto bot tradingowy

Program ten udostępniony jest w celach edukacyjnych. Użytkownik pobiera i korzysta z aplikacji na **własną odpowiedzialność**. 
Solbo jest botem, którego należy używać całkowicie samodzielnie a to znaczy, że trzeba go samemu:

- pobrać,
- skonfigurować,
- uruchomić.

Praca z Solbo przynosi najlepsze efekty kiedy działa on w sposób ciągły, oznacza to, że warto uruchamiać go na specjalnie wydzielonym do tego sprzęcie - np. [Raspberry Pi](https://www.raspberrypi.org/) albo uruchomić na [VPS](https://en.wikipedia.org/wiki/Virtual_private_server) (z zainstalowanym systemem operacyjnym Windows lub Linux).

# Konfiguracja bota

W celu skonfigurowania bota należy:

- odpowiednio przygotować *plik konfiguracyjny*,
- wykorzystać przynajmniej jedną *strategię*, którą Solbo będzie automatyzował na wybranej giełdzie kryptowalutowej.

## Podstawowa

Podstawowy plik konfiguracyjny o nazwie `appsettings.solbo-runtime.json` powinien znajdować w głównym katalogu uruchomieniowym, w tym samym miejscu gdzie znajduje się plik `Solbo.Agent`. 
Plik ten w celu dostosowania do własnych potrzeb, można przed uruchomieniem Solbo:

- edytować w dowolnym edytorze tekstu (np. w Notatniku)

lub

- wygenrować i pobrać poprzez interfejs dostępny przez stronę internetową - [https://cryptodev.tv/Solbo-UI/](https://cryptodev.tv/Solbo-UI/) - *praca w toku*.

Przykładowa zawartość tego pliku poniżej:

```
{
  "version": "0.4.0",
  "strategies": [
    {
      "name": "Alfa",
      "pairs": [
        {
          "symbol": "BTCUSDC",
          "intervaltype": 1,
          "interval": 7
        },
        {
          "symbol": "ETHUSDC",
          "intervaltype": 1,
          "interval": 13
        }
      ]
    },
    {
      "name": "Beta",
      "pairs": [
        {
          "symbol": "BTC-USDT",
          "intervaltype": 1,
          "interval": 30
        },
        {
          "symbol": "ETH-USDT",
          "intervaltype": 1,
          "interval": 45
        }
      ]
    }
  ],
  "notifications": {
    "pushover": {
      "token": "",
      "recipients": "",
      "endpoint": "https://api.pushover.net/1/messages.json"
    }
  }
}
```

Plik ten zawiera informacje o:

- wersji - parametr `version` - nie należy go ręcznie edytować,
- używanych strategii w ramach jednej instancji bota - zbiór `strategies`,
- powiadomieniach o dokonywanych operacjach na giełdzie - parametr `notifications`.

Powyższy przykład zawiera definicje dwóch strategii, którymi Solbo będzie się posługiwał po uruchomieniu:

- Alfa
- Beta

Każda z tych strategii określona jest przez nazwą oraz definuje zbiór par, na których będzie realizowana przez Solbo w kontekście giełdy zaimplementowanej w strategii.
Strategia może być realizowana na jednej lub większej liczbie par, każda para w ramach strategii powinna być unikalna.

Podstawowe dwie strategie (Alfa i Beta), udostępnione razem z Solbo realizują taką samą logikę, której istota sprowadza się do:

- okresowego sprawdzania ceny danej pary, 
- kupowania jeśli cena spadnie o określoną wartość (bezwględną lub procentową),
- sprzedawania jeśli cena wzrośnie o określoną wartość (bezwględną lub procentową).

Różnica pomiędzy Alfa i Beta:

- Alfa działa na giełdzie [Binance](https://www.binance.com/en/register?ref=T0ANYAVJ),
- Beta działa na giełdzie [KuCoin](https://www.kucoin.com/ucenter/signup?rcode=2NNePfu).

Parę w obrębie danej strategii definujemy zgodnie z poniższym schematem i nomenklaturą dla giełdy z której dana strategi korzysta:

- [Binance](https://www.binance.com/en/register?ref=T0ANYAVJ) nie rozdziela symboli w parze,
- [KuCoin](https://www.kucoin.com/ucenter/signup?rcode=2NNePfu) rozdziela symbole w parze stosując średnik (`-`).

Pozostałe parametry dla pary to:

- `intervaltype` - możliwe wartości to:
	- `0` - strategia wykona się tylko raz,
	- `1` - strategia będzie wykonywana co liczbę **sekund** określoną w parametrze `interval`,
	- `2` - strategia będzie wykonywana co liczbę **minut** określoną w parametrze `interval`,
	- `3` - strategia będzie wykonywana co liczbę **godzin** określoną w parametrze `interval`
- `interval` - wartość określająca konkretną liczbę sekund, minut lub godzin.

## W ramach danej strategii

W celu poprawnego działania Solbo musi korzystać z przynajmniej jednej strategii zadeklarowanej w podstawowym pliku konfiguracyjnym - `appsettings.solbo-runtime.json`.
Każda strategia to niezależny od innej plugin, który jest automatycznie wykrywany przez Solbo po uruchomieniu.

Strategie, które mają być automatyzowane przez Solbo powinny znajdować się w katalogu `/strategies`, przykładowe położenie folderów na podstawie wyżej opisanego pliku `appsettings.solbo-runtime.json` powinno wyglądać jak poniżej:

Katalog `Solbo`:

- plik uruchomieniowy `Solbo.Agent`,
- plik konfiguracyjny `appsettings.solbo-runtime.json`,
- inne pliki wymgane przez Solbo..
	- Katalog `strategies`:
		- Katalog `Alfa` przechowujący pliki: 
			- `Solbo.Strategy.Alfa.dll` - implementacja strategii Alfa,
			- `strategy.json` - konfiguracja strategii Alfa
		- Katalog `Beta` przechowujący pliki:
			- `Solbo.Strategy.Beta.dll` - implementacja strategii Beta,
			- `strategy.json` - konfiguracja strategii Beta


### Alfa

Przykładowy plik konfiguracyjny dla strategii Alfa, zgodny z podstawowym plikiem konfiguracyjnym:

Nazwa pliku: `strategy.json`

```
{
  "exchange": {
    "activeexchangetype": 0,
    "binance": {
      "exchangetype": 0,
      "apikey": "binance_apikey",
      "apisecret": "binance_apisecret"
    }
  },
  "pairs": [
    {
      "symbol": "BTCUSDC",
      "buydown": 9,
      "sellup": 6.1,
      "average": 3,
      "averagetype": 0,
      "fundpercentage": 90,
      "selltype": 1,
      "commissiontype": 1,
      "stoplossdown": 12,
      "stoplosspausecycles": 3,
      "clearonstartup": true
    },
    {
      "symbol": "ETHUSDC",
      "buydown": 8,
      "sellup": 5.1,
      "average": 2,
      "averagetype": 1,
      "fundpercentage": 80,
      "selltype": 1,
      "commissiontype": 1,
      "stoplossdown": 12,
      "stoplosspausecycles": 3,
      "clearonstartup": true
    }
  ]
}
```

### Beta

Przykładowy plik konfiguracyjny dla strategii Alfa, zgodny z podstawowym plikiem konfiguracyjnym:

Nazwa pliku: `strategy.json`

```
{
  "exchange": {
    "activeexchangetype": 1,
    "kucoin": {
      "exchangetype": 1,
      "apikey": "kucoin_apikey_here",
      "apisecret": "kucoin_apisecret_here",
      "passphrase": "kucoin_passphrase_here"
    }
  },
  "pairs": [
    {
      "symbol": "BTC-USDT",
      "buydown": 9,
      "sellup": 6.1,
      "average": 3,
      "averagetype": 1,
      "fundpercentage": 90,
      "selltype": 1,
      "commissiontype": 1,
      "stoplossdown": 12,
      "stoplosspausecycles": 3,
      "clearonstartup": true
    },
    {
      "symbol": "ETH-USDT",
      "buydown": 8,
      "sellup": 5.1,
      "average": 2,
      "averagetype": 1,
      "fundpercentage": 80,
      "selltype": 1,
      "commissiontype": 1,
      "stoplossdown": 12,
      "stoplosspausecycles": 3,
      "clearonstartup": false
    }
  ]
}
```