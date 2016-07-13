alter table syn_nodes alter column synin DROP NOT NULL;
alter table syn_nodes alter column synout DROP NOT NULL;
alter table oko.event add column retrnumber integer;
alter table oko.event add column isrepeat bool DEFAULT false;
alter table oko.event add column siglevel integer DEFAULT 0;
insert into public.settings(name, value, "desc") values('TABLE_CACHE_INTERVAL', '43200', 'Время в минутах между обновлением таблиц кэша карт');