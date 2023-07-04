# InfiQ_API

## Instalacja środowiska
Zakładam, że znajdujemy się pod następującą ścieżką (Dysk nie ma znaczenia):
```shell
> cd C:\_PRJ
```
W niej klonujemy sobie nasze repo. Nie wchodzimy do środka na razie.
Tworzymy virtualenv i aktywujemy go. Powinien przed znakiem zachęty pojawić się (InfiQVenv)
```shell
> python -m venv InfiQVenv
> InfiQVenv\Scripts\activate
```

Teraz możemy wejść do projektu.
I instalujemy zależności w pliku requirements.txt
```shell
(InfiQ_Venv) cd InfiQ_API
(InfiQ_Venv) pip install -r requirements.txt
```

## Instalacja Postgres
Pobieramy wersję 15.3:
> https://www.enterprisedb.com/downloads/postgres-postgresql-downloads

Przy instalacji wszystko raczej defaultowo zostawiamy. Hasło postgres ustawiamy na "admin".

Po instalacji uruchamiamy skrypt, WAŻNE BY BYĆ W AKTYWNYM VENVIE.
```shell
(InfiQ_Venv) dbsetup.bat
```

Jeśli coś się popsuło z bazą lokalną. Możemy ją zresetować za pomocą. Należy potem tez uruchomić ponownie setup.
```shell
(InfiQ_Venv) dbreset.bat
(InfiQ_Venv) dbsetup.bat
```