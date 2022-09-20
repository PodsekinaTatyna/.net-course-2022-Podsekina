create extension if not exists "uuid-ossp";

/*
а) наполнить базу тестовыми данными пользуясь оператором Insert;
*/
insert into client (id,first_name,last_name,passport_id,date_of_birth,phone_number)
values (uuid_generate_v4(),'Jon1','Smit1',123451,'12.12.1994',77588999),
       (uuid_generate_v4(),'Jon2','Smit2',123451,'12.12.1991',77588992),
       (uuid_generate_v4(),'Jon3','Smit3',123451,'12.12.1993',77588993),
       (uuid_generate_v4(),'Jon4','Smit4',123451,'12.12.1994',77588994),
       (uuid_generate_v4(),'Jon5','Smit5',123451,'12.12.1993',77588995);

insert into employee (id,first_name,last_name,passport_id,date_of_birth,contract,salary)
values (uuid_generate_v4(),'Bob1','Lem1',123454,'2.3.1990','Contract', 1235),
       (uuid_generate_v4(),'Bob2','Lem2',123454,'2.3.1991','Contract', 1235),
       (uuid_generate_v4(),'Bob3','Lem3',123454,'2.3.1993','Contract', 1235),
       (uuid_generate_v4(),'Bob4','Lem4',123454,'2.3.1994','Contract', 1235),
       (uuid_generate_v4(),'Bob5','Lem5',123454,'2.3.1992','Contract', 1235);

insert into account (amount, currency, client_id)
values (123,'USD','043c3bed-baab-44f1-a3f7-608d7c283bbb'),
       (433,'EUR','043c3bed-baab-44f1-a3f7-608d7c283bbb'),
       (233,'USD','5e5f9308-1d80-4e8b-a40a-b3040a78e3dd'),
       (323,'EUR','5e5f9308-1d80-4e8b-a40a-b3040a78e3dd'),
       (133,'USD','453e956a-fe55-401f-b61b-438d94d12de0');

/*
б) провести выборки клиентов, у которых сумма на счету ниже
определенного значения, отсортированных в порядке возрастания суммы;
*/
select  client_id, amount
from account a
where a.amount < 150
order by a.amount;

/*
в) провести поиск клиента с минимальной суммой на счете;
*/
select client_id, amount, currency
from account
where amount = (select min(amount) from account);

/*
г) провести подсчет суммы денег у всех клиентов банка;
*/
select sum(amount) as total_amount
from account;

/*
д) с помощью оператора Join, получить выборку банковских счетов и их владельцев;
*/
select a.amount, a.currency, a.client_id, c.first_name, c.last_name
from account a
join client c on a.client_id = c.id;

/*
е) вывести клиентов от самых старших к самым младшим;
*/
select c.id, c.first_name, c.last_name, c.date_of_birth
from client c
order by c.date_of_birth;

/*
ж) подсчитать количество человек, у которых одинаковый возраст;
*/
select c.date_of_birth, count(*)
from client c
group by c.date_of_birth
having count(c.date_of_birth) > 1;

/*
з) сгруппировать клиентов банка по возрасту;
*/
select c.date_of_birth, count(*)
from client c
group by c.date_of_birth;

/*
и) вывести только N человек из таблицы;
*/
select c.id, c.first_name, c.last_name, c.date_of_birth
from client c
limit 2;
