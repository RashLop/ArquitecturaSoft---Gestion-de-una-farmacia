# 🏥 Gestor de Farmacia - Razor Pages
### 📌 Descripción del Proyecto

Sistema web desarrollado con ASP.NET Core Razor Pages y C# que permite gestionar la información de una farmacia mediante operaciones CRUD para diferentes entidades del sistema.

El sistema permite administrar información de:

- Medicamentos
- Clientes
- Bioquímicos

El proyecto fue diseñado aplicando buenas prácticas de arquitectura de software, incluyendo *principios SOLID, Clean Code, Dependency Injection y el patrón de diseño Factory Method*, con el objetivo de obtener un sistema modular, mantenible y escalable.

--------------------------------------------------------------------
### 🚀 Tecnologías Utilizadas

- ASP.NET Core Razor Pages
- C#
- MySQL
- MySql.Data
- HTML / CSS / Bootstrap
- Git / GitHub
----------------------------------------------------------------------------

### 🗄️ Base de Datos

El sistema utiliza una base de datos MySQL con las siguientes entidades principales:

- Cliente
- Medicamento
- Bioquímico

Cada entidad posee operaciones CRUD completas gestionadas a través de repositorios.

-----------------------------------------------------------------------
### 🧱 Arquitectura del Proyecto

El sistema sigue una arquitectura en capas, separando responsabilidades para mejorar la mantenibilidad del código.

    Presentation Layer (Razor Pages)
            ↓
    Application Layer (Services + Factories)
            ↓
    Domain Layer (Entities + Interfaces)
            ↓
    Infrastructure Layer (Repositories + Base de Datos)

-----------------------------------------------------------------------
### *Capas del sistema*

*Presentation (Pages)*
Contiene las páginas Razor que gestionan la interacción con el usuario.

*Application (Services & Factories)*
Implementa la lógica de negocio y el patrón Factory Method para la creación de objetos del dominio.

*Domain (Entities & Interfaces)*
Define las entidades principales del sistema y las interfaces que abstraen la lógica de acceso a datos.

*Infrastructure (Repositories)*
Implementa el acceso a la base de datos mediante repositorios que cumplen las interfaces del dominio.

-----------------------------------------

## 🧩 Patrón de Diseño Aplicado
### Factory Method

El sistema utiliza el patrón Factory Method para crear diferentes tipos de medicamentos según su clasificación.

Clasificaciones implementadas:

- Antibiótico
- Analgésico
- Antiinflamatorio
- Antialérgico
- Antipirético
- Vitaminas
- Antiséptico

Esto permite extender el sistema agregando nuevos tipos de medicamentos sin modificar el código existente, cumpliendo el principio Open/Closed.

------------------------------------------

## 🧱 Principios SOLID Aplicados
### S — Single Responsibility Principle

Cada clase tiene una única responsabilidad:

    - Razor Pages → Interfaz de usuario
    - Services → Lógica de negocio
    - Repositories → Acceso a datos
    - Factories → Creación de objetos

-----------------------------------------------
### O — Open / Closed Principle

El sistema permite agregar nuevos tipos de medicamentos sin modificar código existente.

-----------------------------------------------
### L — Liskov Substitution Principle

Las subclases de Medicamento pueden reemplazar a la clase base sin afectar el comportamiento del sistema.

-------------------------------------------------

### I — Interface Segregation Principle

Las interfaces del dominio están separadas para evitar dependencias innecesarias.
sistema.

-------------------------------------------------

### D — Dependency Inversion Principle

Las capas superiores dependen de interfaces, no de implementaciones concretas.

Ejemplo:

    MedicamentoService
        ↓
    IMedicamentoRepository
        ↓
    MedicamentoRepository

----------------------------------------------

### 📂 Estructura del Proyecto

    ProyectoArqSoft
    │
    ├── Pages
    │   ├── Cliente
    │   ├── Medicamento
    │   └── Bioquimico
    │
    ├── Domain
    │   ├── Entities
    │   ├── Interfaces
    │   └── Enums
    │
    ├── Application
    │   ├── Services
    │   ├── Factories
    │   └── Validaciones
    │
    ├── Infrastructure
    │   └── Repositories
    │
    ├── Helpers
    ├── Base
    └── Program.cs

-----------------------------

### 📊 Funcionalidades

    ✔ Registro de medicamentos
    ✔ Edición de medicamentos
    ✔ Eliminación lógica de medicamentos
    ✔ Gestión de clientes
    ✔ Gestión de bioquímicos
    ✔ Búsqueda con filtros
    ✔ Validaciones de datos

------------------------------------------------

### ⚙️ Cómo ejecutar el proyecto

1. Clonar el repositorio

    git clone https://github.com/usuario/repositorio.git

2. Abrir el proyecto en Visual Studio

3. Configurar la conexión a MySQL en:

    appsettings.json

4. Ejecutar el proyecto


------------------------------------------------

### 👨‍💻 Autor: Jose Carlos Lopez Soria.

Proyecto académico con guia en el readme para uso en la farmacia VITALCARE.