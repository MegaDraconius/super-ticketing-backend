# Super Ticketing Backend

## Índice 📝

- [Descripción](#descripción-);
- [Instalación](#instalación-)
    - [Requisitos previos](#requisitos-previos)
    - [Instalación del proyecto](#instalación-del-proyecto)
- [Uso](#uso-)
- [Estructura del proyecto](#estructura-del-proyecto)
- [Tecnologías](#tecnologías-)
- [Contribuición](#contribuición-)
    - [Convenciones del proyecto](#Convención-de-codificación-en-Ticketing-Backend)
- [Developers](#developers-)
- [Capturas de pantalla](#capturas-de-pantalla)
- [Despliegue del Proyecto](#Despliegue-del-Proyecto)

## Descripción 💡

Este proyecto tiene como objetivo desarrollar una aplicación web para monitorear y gestionar el estado de sequía en las Cuencas Internas de Cataluña. Basándose en una API pública, la aplicación permitirá obtener datos sobre el estado hidrológico y pluviométrico de las unidades de explotación y municipios, indicando las fechas en las que se han realizado cambios en estos estados.

<b>Funcionalidades Principales</b>:

-<b>Integración de API Pública</b>: Obtención de datos actualizados sobre sequía y almacenamiento en una base de datos interna.

-<b>CRUD Completo</b>: Creación, visualización, edición y eliminación de registros relacionados con los estados de sequía.

-<b>Frontend y Backend Independientes</b>: Desarrollo de una interfaz de usuario intuitiva y un backend robusto que se comuniquen eficazmente.
Tests Unitarios: Implementación de tests para asegurar la calidad y correcto funcionamiento de la aplicación.

<b>Objetivo del Proyecto</b>:
<br>
A través de este proyecto, buscamos contribuir a la conservación del medio ambiente y al bienestar social, proporcionando una herramienta útil y accesible para la gestión de recursos hídricos en Cataluña. La aplicación está diseñada para ser utilizada por autoridades locales, investigadores y ciudadanos interesados en el seguimiento de las condiciones de sequía, ayudando a tomar decisiones informadas y a promover la sostenibilidad.consumo de APIs.

## Enlaces a los repositorios 📦

Enlace al Repositorio del Frontend:
https://github.com/MegaDraconius/super-ticketing-frontend

Enlace al Repositorio del Backend:
https://github.com/MegaDraconius/super-ticketing-backend


## Instalación 💾

### Requisitos previos

Instalar jetbrains:
- [JetBrains](https://www.jetbrains.com/es-es/rider/)

o en su defecto

Instalar Visual Studio:
- [Visual Studio](https://visualstudio.microsoft.com/es/)



### Instalación del proyecto

1. Clonar el repositorio:

```bash
 git clone https://github.com/MegaDraconius/super-ticketing-backend
```

2. Las dependencias se instalan automaticamante al realziar el clone del repositorio.
## Uso ⌨️

Para visualizar el proyecto en Local:

1. Ejecuta el servidor de desarrollo:

   ```bash
   dot net run
   ```
2. Ejemplo de vista una vez desplegado en local:


   ![Vista de los controladores desde el Swagger](https://static1.smartbear.co/swagger/media/images/tools/opensource/swagger_ui.png)

Para visualizar los test:

1. Ejecutar los test:

   ```bash
   dotnet test
   ```
   
   ![Vista ejemplo de un test en Xunit](https://www.c-sharpcorner.com/article/implementing-unit-and-integration-tests-on-net-with-xunit/Images/6.png)
## Estructura del proyecto 📐

```plaintext
/
├── public
├── src
|   ├── assets
│   ├── components/
│   │   ├── common
│   │   └── layout 
│   ├── config
│   ├── pages
│   ├── routes
│   ├── services
│   ├── App.css
│   ├── App.jsx
│   ├── home.jsx
│   ├── index.css
│   └── main.jsx
├── eslintrc.cjs
├── .gitignore
├── index.html
├── package-lock.json
├── package.json
├── pnpm-lock.yaml
├── postcss.config.js
├── README.md
├── jsconfig.json
├── tailwind.config.js
└── vite.config.js

```

- **public/:** Contiene los recursos estáticos del proyecto como imágenes, iconos y fuentes.
- **src/:** Contiene los archivos fuente de la aplicación.
    - **_components/:_** Contiene los componentes reutilizables de React.
    - **_config/:_** Contiene el arcihvo urls.js, que nos ayuda a dinamizar la llamada a la API.
    - **layout:** Carpeta que contiene las rutas de los dos layouts principales (homepage y tracker) de la aplicación.
    - **_pages:_** Carpeta que contiene las rutas a las páginas dinámicas de la aplicación.
    - **_routes:_** Dentro del que se encuentra index.jsx, que contiene la lógica de rutas de la aplicación.
    - **_services:_** Dentro del que se encuentra useApi.jsx con la llamada a la API reutilizada en todos los apartados de la aplicación.

## Tecnologías empleadas en Backend🔬


- IDE para el proyecto:
  - [.JetBrains Rider](https://www.jetbrains.com/es-es/rider/)
  
  
- Tecnologías empleadas para el desarrollo de la base de datos:

    - [.Net](https://dotnet.microsoft.com/es-es/)
    - [C#](https://dotnet.microsoft.com/en-us/languages/csharp)
    - [MongoDB](https://www.mongodb.com/)
  

- Despliegue de la base de Datos: 
  - [MongoDB Atlas](https://www.mongodb.com/es/lp/cloud/atlas/try4?utm_source=google&utm_campaign=search_gs_pl_evergreen_atlas_core-high-int_prosp-brand_gic-null_emea-es_ps-all_desktop_eng_lead&utm_term=mongodb%20atlas&utm_medium=cpc_paid_search&utm_ad=e&utm_ad_campaign_id=19609093152&adgroup=145580477757&cq_cmp=19609093152&gad_source=1&gclid=CjwKCAjwnqK1BhBvEiwAi7o0X-pRbSkU8ExwPX-KbGctiGw6IWpoJSDyidIoop9PNujsW8Q8CAFmIBoC1lIQAvD_BwE?utm_source=google&utm_campaign=search_gs_pl_evergreen_atlas_core-high-int_prosp-brand_gic-null_emea-es_ps-all_desktop_eng_lead&utm_term=mongodb%20atlas&utm_medium=cpc_paid_search&utm_ad=e&utm_ad_campaign_id=19609093152&adgroup=145580477757&cq_cmp=19609093152&gad_source=1&gclid=CjwKCAjwnqK1BhBvEiwAi7o0X-pRbSkU8ExwPX-KbGctiGw6IWpoJSDyidIoop9PNujsW8Q8CAFmIBoC1lIQAvD_BwE)
    

- Tecnologías para realizar testing:

    - [XUnit](https://xunit.net/)


## Contribuición 💻


1. Haz fork al repositorio.
2. Crea una nueva rama: `git checkout -b feature-name`.
3. Haz tus cambios.
4. Haz push de tu rama: `git push origin feature-name`.
5. Haz un pull request.

# Convención de codificación en Ticketing Backend

## Jet Brains: RIDER 🖥️

A la hora de crear código en nuestro editor de código tenemos  en cuenta los siguientes enunciados para un código legible, estructurado y ordenado tanto para los miembros del equipo como ojeadores del proyecto:

- Naming en inglés.

```c#
@page "/example"
@inject IJSRuntime JS

<h1 @ref="myHeading">Original Heading</h1>
```

- Usar Cammel Case. Ejemplo:

```c#
@page "/mycomponent"
@implements IAsyncDisposable

<h1 @ref="myHeading">Hello, world!</h1>
```

- Nombre de variables descriptivas y fáciles de leer.

```c#
public class MenuComponent : IOnInit
{
  private readonly IMenuService _menuService;
  public List<MenuItem> Menu { get; private set; }

  public MenuComponent(IMenuService menuService)
  {
    _menuService = menuService;
  }

  public void OnInit()
  {
    Menu = _menuService.GetMenu();
  }
}
```

- Los métodos o funciones deben tener nombres de verbos o frases verbales como ‘*menuService’, ‘deletePage’* o ‘*save*’.
- Las clases y objetos deben tener un nombre de un sustantivo o una frase nominal como *‘MenuComponent’, ‘IAsyncDisposable’, ‘Account’* o ‘*AddressParser*’.
- Evitar palabras como *‘Manager’, ‘Processor’, ‘Data’* o *‘Info’* en el nombre de la clase. Un nombre de clase no debería ser un verbo.



## Developers 👩‍💻

- [Alex Morell](https://github.com/alexmrtc)
- [Iván Vallejos](https://github.com/MegaDraconius)
- [Laura Benavides](https://github.com/LauraBenavides33)
- [Michely Paredes](https://github.com/Michely05)
- [Roger Esteve](https://github.com/alejandria1899)
- [Sara Jorja](https://github.com/MegaDraconius)

## Distribución y seguimiento de tareas con metodologías ágiles 👩‍💻

Para la gestión de nuestro proyecto, hemos utilizado Trello como nuestra herramienta principal, siguiendo la metodología Kanban para la distribución eficiente de tareas. Este enfoque nos ha permitido visualizar el flujo de trabajo, limitar la cantidad de trabajo en progreso y maximizar la eficiencia. Además, hemos seguido una serie de convenciones y mejores prácticas para garantizar la coherencia y la calidad en todo nuestro trabajo. Creemos que este enfoque estructurado y disciplinado ha sido fundamental para nuestro éxito hasta ahora y continuará guiándonos en nuestras futuras iniciativas

Incluyo un enlace a nuestro tablero de Trello para que puedan ver nuestra organización y gestión de tareas.[Trello Super Ticketing](https://trello.com/b/MYcpBsGx/super-ticketing)

## Despliegue del Proyecto 📽️

Hemos desplegado nuestra base de datos usando MongoDB Atlas, que nos ofrece una solución en la nube con administración completa, alta disponibilidad, escalabilidad y fácil integración con nuestras aplicaciones.

Además de las ventajas técnicas que ofrece MongoDB Atlas, también lo seleccionamos por su compromiso con la sostenibilidad. Al ser una plataforma eficiente en el uso de recursos, nos permite contribuir a la conservación del medio ambiente mientras gestionamos nuestras bases de datos.

- Enlace a la pagina de [Ticketing Backend](https://cloud.mongodb.com/v2/6697b290c7ed8c3f10eb19b9#/clusters)



## Pendientes para futuros Sprints

Reconocemos que nuestro proyecto actual tiene un gran potencial para crecer y evolucionar. Aunque hemos logrado mucho, sabemos que hay características adicionales que podríamos implementar para mejorar aún más nuestro producto. Estas mejoras no se han realizado hasta ahora debido a limitaciones de tiempo y conocimientos técnicos. Sin embargo, estamos comprometidos con la mejora continua y planeamos adquirir las habilidades necesarias para implementar estas características en el futuro.

- [ ] Añadir nuestra propia lógica para el login mediante conexión entre el frontend y backend.
- [ ] Añadir las funcionalidades para una implementación del historial.
- [ ] Depurar código siguiendo principios SOLID y CLEAN CODE.
- [ ] Realizar más pruebas unitarias y poder realizar test de integración.
- [ ] Realizar un despliegue en un servidor propio para la base de datos.

## Documentación del Código

El código se está documentando en un notion en el cual se detalla paso a paso lo siguientes puntos:

- Visión General del Código: Una descripción general de la arquitectura del proyecto y cómo se estructuran los diferentes componentes.
- Ejemplos de Código: Fragmentos de código que ilustran cómo se deben implementar y utilizar las funciones principales.
- Guía de Uso: Instrucciones paso a paso sobre cómo utilizar las principales funcionalidades del código.