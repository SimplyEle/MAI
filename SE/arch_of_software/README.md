# 3. Сайт конференции

Студент: Каратаева Е.С. \
Группа: М8О-114М-22

Для запуска потребуется перейти в папку Docker и запустить из неё команду ```docker-compose up --build```.
При выполнении запроса к /read_all_reports нужно после создания таблиц и заполнения таблицы User выполнить запрос /read_all_reports (получить пустой массив), и после заполнить остальные таблицы, тогда работает корректно.

Результаты wrk в [perfomance.md](./perfomance.md)