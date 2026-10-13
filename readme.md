# StoicDesk

Un widget de escritorio para Windows que muestra frases de filósofos estoicos directamente sobre el fondo de pantalla, actualizándose automáticamente cada 5 minutos.

![StoicDesk](https://img.shields.io/badge/Windows-10%20%7C%2011-0078D6?logo=windows&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)
![License](https://img.shields.io/badge/license-MIT-green)

## Características

-  **Integrado en el escritorio**: la ventana se incrusta en la capa `WorkerW` de Windows, quedando detrás de las ventanas y visible al pulsar `Win + D`.
-  **Actualización automática**: consulta la API cada 5 minutos para mostrar una nueva frase aleatoria.
-  **Clic para refrescar**: haz clic sobre la frase para obtener una nueva al instante.
-  **Diseño minimalista**: fondo *bone white* con tipografía Times New Roman en cursiva y 10px de margen alrededor del texto.
-  **Inicio automático**: puede configurarse para arrancar al iniciar sesión en Windows.
-  **Ejecutable único**: se publica como un `.exe` autocontenido, sin necesidad de instalar .NET en el equipo destino.

##  Arquitectura

El proyecto está dividido en tres partes bien separadas:

| Archivo | Responsabilidad |
|---|---|
| `MainWindow.xaml` / `MainWindow.xaml.cs` | Interfaz de usuario, lógica del widget, timers y centrado en pantalla. |
| `QuoteService.cs` | Cliente HTTP que consulta la API y deserializa la respuesta JSON. |
| `DesktopHelper.cs` | P/Invoke con `user32.dll` para incrustar la ventana en el escritorio (`Progman` / `WorkerW`). |

## API

La aplicación consume un endpoint HTTP que devuelve un JSON con la siguiente estructura:

```json
{
  "id": 12,
  "frase": "No cuentes los días, haz que los días cuenten.",
  "autor": "Muhammad Ali",
  "categoria": {
    "id": 3,
    "categoria": "Motivación",
    "descripcion": "Frases para inspirar"
  }
}
```

La URL de la API se configura en QuoteService.cs:
```csharp
private const string ApiUrl = "https://tudominio.com/api/";
```

## Requisitos
Windows 10 o Windows 11

Visual Studio 2022 con la carga de trabajo .NET desktop development

.NET 8.0 SDK o superior

## Compilar y ejecutar
Clona el repositorio:

```bash
git clone https://github.com/tu-usuario/StoicDesk.git
cd StoicDesk
```

Abre la solución en Visual Studio:
```bash
start StoicDesk.sln
```

Configura la URL de tu API en QuoteService.cs.

Ejecuta en modo debug con F5.

## Contribuciones
Las contribuciones son bienvenidas. Si encuentras un bug o tienes una idea para mejorar el widget, abre un issue o envía un pull request.

Haz un fork del proyecto.

Crea una rama para tu feature: git checkout -b feature/nueva-funcionalidad.

Haz commit de tus cambios: git commit -m 'Añade nueva funcionalidad'.

Sube la rama: git push origin feature/nueva-funcionalidad.

Abre un Pull Request.

## Licencia
Este proyecto se distribuye bajo la licencia MIT. Consulta el archivo LICENSE para más detalles.

## Agradecimientos
Inspirado en los principios del estoicismo y en filósofos como Marco Aurelio, Epicteto, Séneca, Zenón de Citio y Crisipo de Solos.

Construido con WPF y .NET.

<p align="center"> Hecho con 🖤 y monster energy. </p> ```