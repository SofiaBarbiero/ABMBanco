create database Banco
use Banco
create table tipos_cuentas
(
	id_tipo_cuenta int identity (1,1),
	nombre_cuenta varchar (50),
	constraint pk_tipo_cuenta primary key (id_tipo_cuenta)
)

create table clientes
(
	id_cliente int identity (1,1),
	apellido varchar (50),
	nombre varchar (50),
	dni int,
	constraint pk_cliente primary key (id_cliente)
)

create table cuentas
(
	id_cuenta int identity (1,1),
	cbu int ,
	saldo decimal (12,2),
	ultimo_movimiento datetime,
	id_tipo_cuenta int,
	id_cliente int,
	constraint pk_cbu primary key (cbu),
	constraint fk_cuenta_tipo_cuenta foreign key (id_tipo_cuenta)
		references tipos_cuentas (id_tipo_cuenta),
	constraint fk_cuenta_cliente foreign key (id_cliente)
		references clientes (id_cliente)
)
set dateformat DMY

--insertando tipos de cuentas
insert into tipos_cuentas (nombre_cuenta) values ('Caja de Ahorros en Pesos')
insert into tipos_cuentas (nombre_cuenta) values ('Cuenta Corriente en Pesos')
insert into tipos_cuentas (nombre_cuenta) values ('Cuenta Corriente en Dolares')
insert into tipos_cuentas (nombre_cuenta) values ('Cuenta Sueldo')
--insertando clientes
insert into clientes (apellido,nombre,dni) values ('Perez','Damian',34567890)
insert into clientes (apellido,nombre,dni) values ('Locasio','Fernando',31567890)
insert into clientes (apellido,nombre,dni) values ('Maidana','Adriana',12456789)
insert into clientes (apellido,nombre,dni) values ('Farfan','Pablo',10234567)
insert into clientes (apellido,nombre,dni) values ('Torres','Paola',9789456)
--insertando una cuenta de prueba
insert into cuentas (cbu,saldo,ultimo_movimiento,id_tipo_cuenta,id_cliente)values(123467999,45123.12,'17/07/2022',1,1)

--sp para cargar combo de cuentas
create proc SP_LISTAR_TIPOS_CUENTAS
as
select *
from tipos_cuentas

exec SP_LISTAR_TIPOS_CUENTAS

--sp proximo cliente
CREATE PROCEDURE SP_PROXIMO_ID
@next int OUTPUT
AS
BEGIN
	SET @next = (SELECT MAX(id_cliente)+1  FROM clientes);
END

--sp insertar clientes
CREATE PROCEDURE SP_INSERTAR_CLIENTE
	@apellido varchar(100), 
	@nombre varchar(100),
	@dni int,
	@cliente_nro int OUTPUT
AS
BEGIN
	INSERT INTO clientes (apellido, nombre, dni)
    VALUES (@apellido, @nombre, @dni);
    --Asignamos el valor del último ID autogenerado (obtenido --  
    --mediante la función SCOPE_IDENTITY() de SQLServer)	
    SET @cliente_nro = SCOPE_IDENTITY();

END

--sp insertar cuentas
CREATE PROCEDURE SP_INSERTAR_CUENTAS
	@cbu int,
	@saldo decimal(12,2), 
	@ultimo_mov datetime, 
	@id_tipo_cuenta int,
	@id_cliente int
AS
BEGIN
	INSERT INTO CUENTAS(cbu, saldo, ultimo_movimiento, id_tipo_cuenta, id_cliente)
    VALUES (@cbu, @saldo, @ultimo_mov, @id_tipo_cuenta, @id_cliente);
  
END

select *
from clientes
select * 
from cuentas

ALTER PROC SP_REPORTE_CLIENTES
AS
BEGIN
	select apellido+SPACE(1)+nombre 'Cliente', dni 'DNI', ct.cbu 'CBU', t.nombre_cuenta 'Tipo',
	ct.saldo 'Saldo', ct.ultimo_movimiento 'Ultimo'
	from clientes c join cuentas ct on c.id_cliente = ct.id_cliente
	join tipos_cuentas t on ct.id_tipo_cuenta = t.id_tipo_cuenta
END

CREATE PROCEDURE SP_ELIMINAR_CUENTA
@cbu_nro int
AS
BEGIN
	UPDATE cuentas SET fecha_baja = GETDATE()
	WHERE cbu = @cbu_nro;
END

ALTER TABLE CUENTAS
ADD fecha_baja datetime


create procedure SP_CONSULTAR_CUENTAS
@apellido varchar(50),
@nombre varchar(50),
@dni int
as
begin
select ct.cbu, t.nombre_cuenta, ct.saldo, ct.ultimo_movimiento
from clientes c join cuentas ct on c.id_cliente = ct.id_cliente
join tipos_cuentas t on ct.id_tipo_cuenta = t.id_tipo_cuenta
where c.apellido = @apellido and c.nombre = @nombre and c.dni = @dni
end



