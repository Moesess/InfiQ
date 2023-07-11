# InfiQ_API
## Instalacja środowiska
Pobierz dockera. https://www.docker.com/ nic specjalnego raczej nie trzeba dodatkowo konfigurować.

Uruchom skrypt docker_setup.bat, powinien przelecieć i w docker desktopie powinien pojawić się kontener. 
Jeśli z jakiegoś powodu nie odpalił się automatycznie, można go tam uruchomić.

## Wypełnienie danymi
W celu uruchomienia scrappera wydajemy polecenie:
```shell
python manage.py fill_db
```