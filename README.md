# FunEvents

Sistema de venta de entradas para eventos (conciertos, teatro, etc.).

## 1. Arquitectura

FunEvents vende entradas por tres canales: el portal online (canal principal), las
oficinas de atención al cliente (repartidas por varios países) y los portales o
puntos de venta de colaboradores externos, que integran FunEvents en su propia
experiencia de compra.

Para que el inventario de cupos no se desincronice entre canales, ninguno accede
directo a la base de datos: todos, incluido el portal propio de FunEvents, pasan
por la misma API REST central. Esa API es la que concentra las reglas de negocio
(disponibilidad, validaciones, reservas) y habla con una única base de datos
relacional compartida. Es la pieza clave: si cada canal tuviera su propia copia del
inventario, dos canales podrían vender el mismo cupo dos veces.

Los colaboradores consumen esa misma API pero con su propia identidad (API key o
cliente OAuth por colaborador), lo que permite darles de alta, medir su volumen de
ventas y cortarles el acceso sin tocar al resto de canales. Como la integración es
por API, cada colaborador puede envolverla como quiera en su propio portal o
aplicación de punto de venta, sin depender del look and feel de FunEvents.

El punto más sensible del sistema es la concurrencia sobre el cupo de un evento:
varios canales pueden intentar reservar las últimas entradas casi al mismo tiempo,
así que el chequeo de capacidad y la creación de la reserva tienen que resolverse
de forma atómica en la base de datos y no en memoria de cada canal.

Quedan fuera del alcance de este prototipo, pero seguirían el mismo esquema de API
central: cobro y pagos, emisión de la entrada final (PDF/QR) y notificaciones al
comprador.

## 2. Prototipo

El prototipo tiene dos partes. FunEvents.Api es la API REST (ASP.NET Core con
Controllers) que expone venues, eventos, usuarios y reservas. FunEvents.Console es
un cliente de consola que reserva entradas para un evento a partir de un código de
evento y uno de usuario ya conocidos, llamando siempre a la API.

No usé .NET Aspire ni Postgres todavía; hice el prototipo con las tecnologías con
las que me siento más cómoda (EF Core y SQL Server), que además ya tenía corriendo
en mi máquina.

### Stack

.NET 10, ASP.NET Core con Controllers, Entity Framework Core y SQL Server, con la
solución organizada en Clean Architecture (`Domain`, `Application`,
`Infrastructure`, `Api`, `Console`).

### Cómo correr

Requisitos: .NET 10 SDK y una instancia de SQL Server accesible (por defecto se usa
`localhost\SQLEXPRESS` en `appsettings.Development.json`; ajustar el connection
string `FunEventsDatabase` si tu instancia es distinta).

```
dotnet run --project src/FunEvents.Api
```

Al arrancar, la API aplica las migraciones automáticamente y, si la base está
vacía, siembra un venue, una categoría, un usuario y un evento publicado de
prueba. Los códigos generados quedan en el log de arranque (`Datos de prueba
creados. EventId=... UserId=...`).

Si necesitás ver qué usuarios existen (por ejemplo para armar el `userId` de una
reserva), la API expone `GET /api/users`; lo mismo con `GET /api/events` para los
eventos. También se puede dar de alta un usuario nuevo con `POST /api/users`.

Con la API corriendo (por defecto en `http://localhost:5112`), el cliente de
consola se ejecuta así:

```
dotnet run --project src/FunEvents.Console -- <eventId> <userId> [quantity] [apiBaseUrl]
```

Si se ejecuta sin argumentos, los pide de forma interactiva.

El archivo `src/FunEvents.Api/FunEvents.Api.http` tiene requests de ejemplo para
probar la API directamente.
