# Práctica de Estructura de Datos - Unidad IV: Grafos
## Universidad Estatal Amazónica (UEA)

**Tema seleccionado:** Opción 2 - Encuentro de vuelos baratos a partir de una base de datos utilizando Grafos y Algoritmo de Dijkstra.

### 📌 Descripción del Proyecto
Este proyecto implementa una solución en lenguaje **C# (.NET)** sin librerías externas para modelar una red de transporte aéreo mediante un **grafo dirigido y ponderado** utilizando una **matriz de adyacencia**. 

Permite:
1. Consultar ciudades (Vértices).
2. Consultar vuelos comerciales directos y sus tarifas (Aristas con peso).
3. Visualizar la Matriz de Adyacencia en consola.
4. Encontrar la ruta más económica entre cualquier origen y destino aplicando el **Algoritmo de Dijkstra**, mostrando escalas, costo mínimo y tiempo de ejecución.
5. Registrar dinámicamente nuevas ciudades y rutas.
6. Cargar y persistir datos desde un archivo de texto plano (`vuelos.txt`).

---

### 💻 Requisitos y Compilación

#### Compilación directa en Windows (PowerShell / CMD):
No requiere Visual Studio ni descargas pesadas. Se compila con el compilador nativo de C# incluido en Windows:
```powershell
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe /out:SistemaVuelos.exe Program.cs
```

#### Ejecución:
```powershell
.\SistemaVuelos.exe
```

---

### 📁 Estructura del Repositorio
* `Program.cs`: Código fuente completo en C#.
* `vuelos.txt`: Base de datos de texto plano con los vuelos precargados.
* `INFORME_PRACTICA_UEA.md`: Informe técnico completo con formato institucional UEA y normas APA 7ma edición.

---

### 🤖 Declaración de Uso de Inteligencia Artificial
* **Herramienta / Agente utilizado:** Asistente Antigravity / Gemini.
* **Porcentaje aproximado de apoyo:** 35% (apoyo en la estructuración de la matriz de adyacencia, formato de reportería en consola y redacción del marco teórico con formato APA 7).
* **Autoría del estudiante:** 65% (lógica del algoritmo de Dijkstra para nivel de 3er semestre, definición del menú interactivo, validaciones de entrada y pruebas con rutas aéreas de Ecuador).
