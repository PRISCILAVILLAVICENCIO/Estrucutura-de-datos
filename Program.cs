using System;
using System.IO;
using System.Diagnostics;

namespace PracticaGrafosVuelos
{
    // Clase que representa el Grafo de Vuelos usando Matriz de Adyacencia
    // Nivel: Estructura de Datos (3er Semestre)
    class GrafoVuelos
    {
        private const int MAX_CIUDADES = 20;
        private const double INFINITO = 9999999.0;

        private string[] nombresCiudades;
        private double[,] matrizCostos;
        private int totalCiudades;

        public GrafoVuelos()
        {
            nombresCiudades = new string[MAX_CIUDADES];
            matrizCostos = new double[MAX_CIUDADES, MAX_CIUDADES];
            totalCiudades = 0;

            // Inicializar la matriz con INFINITO (sin conexion directa)
            // y 0 en la diagonal principal (distancia a si mismo)
            for (int i = 0; i < MAX_CIUDADES; i++)
            {
                for (int j = 0; j < MAX_CIUDADES; j++)
                {
                    if (i == j)
                    {
                        matrizCostos[i, j] = 0;
                    }
                    else
                    {
                        matrizCostos[i, j] = INFINITO;
                    }
                }
            }
        }

        // Metodo para agregar una ciudad (Vertice)
        public int AgregarCiudad(string nombre)
        {
            // Validar si ya existe
            int indiceExistente = ObtenerIndiceCiudad(nombre);
            if (indiceExistente != -1)
            {
                return indiceExistente;
            }

            if (totalCiudades >= MAX_CIUDADES)
            {
                Console.WriteLine("Error: Se ha alcanzado el limite maximo de ciudades.");
                return -1;
            }

            nombresCiudades[totalCiudades] = nombre.Trim();
            totalCiudades++;
            return totalCiudades - 1;
        }

        // Metodo para buscar el indice numerico de una ciudad
        public int ObtenerIndiceCiudad(string nombre)
        {
            for (int i = 0; i < totalCiudades; i++)
            {
                if (nombresCiudades[i].ToLower() == nombre.Trim().ToLower())
                {
                    return i;
                }
            }
            return -1;
        }

        // Metodo para registrar un vuelo directo (Arista dirigida con peso)
        public bool AgregarVuelo(string origen, string destino, double precio)
        {
            int idxOrigen = ObtenerIndiceCiudad(origen);
            int idxDestino = ObtenerIndiceCiudad(destino);

            if (idxOrigen == -1)
            {
                idxOrigen = AgregarCiudad(origen);
            }
            if (idxDestino == -1)
            {
                idxDestino = AgregarCiudad(destino);
            }

            if (idxOrigen != -1 && idxDestino != -1)
            {
                matrizCostos[idxOrigen, idxDestino] = precio;
                return true;
            }

            return false;
        }

        // Cargar datos predeterminados de prueba (Rutas de Ecuador)
        public void CargarDatosPredeterminados()
        {
            AgregarCiudad("Quito");
            AgregarCiudad("Guayaquil");
            AgregarCiudad("Cuenca");
            AgregarCiudad("Manta");
            AgregarCiudad("Loja");
            AgregarCiudad("Coca");
            AgregarCiudad("Galapagos");

            // Registro de vuelos con sus precios en dolares (aristas ponderadas)
            AgregarVuelo("Quito", "Guayaquil", 55.00);
            AgregarVuelo("Quito", "Cuenca", 65.00);
            AgregarVuelo("Quito", "Manta", 45.00);
            AgregarVuelo("Quito", "Coca", 50.00);
            AgregarVuelo("Quito", "Galapagos", 140.00);

            AgregarVuelo("Guayaquil", "Quito", 58.00);
            AgregarVuelo("Guayaquil", "Cuenca", 38.00);
            AgregarVuelo("Guayaquil", "Galapagos", 110.00);
            AgregarVuelo("Guayaquil", "Manta", 35.00);

            AgregarVuelo("Cuenca", "Quito", 62.00);
            AgregarVuelo("Cuenca", "Loja", 30.00);
            AgregarVuelo("Cuenca", "Guayaquil", 40.00);

            AgregarVuelo("Manta", "Quito", 48.00);
            AgregarVuelo("Manta", "Galapagos", 125.00);

            AgregarVuelo("Loja", "Guayaquil", 42.00);
            AgregarVuelo("Coca", "Quito", 52.00);
        }

        // Cargar base de datos ficticia desde archivo de texto plano
        public void CargarDesdeArchivo(string nombreArchivo)
        {
            if (!File.Exists(nombreArchivo))
            {
                Console.WriteLine("El archivo no existe. Creando archivo de ejemplo...");
                CrearArchivoEjemplo(nombreArchivo);
            }

            try
            {
                string[] lineas = File.ReadAllLines(nombreArchivo);
                int contador = 0;

                for (int i = 0; i < lineas.Length; i++)
                {
                    string linea = lineas[i].Trim();
                    // Ignorar lineas vacias o comentarios con #
                    if (string.IsNullOrEmpty(linea) || linea.StartsWith("#"))
                    {
                        continue;
                    }

                    string[] partes = linea.Split(',');
                    if (partes.Length >= 3)
                    {
                        string orig = partes[0].Trim();
                        string dest = partes[1].Trim();
                        double costo;
                        string textoCosto = partes[2].Trim().Replace(',', '.');
                        if (double.TryParse(textoCosto, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out costo))
                        {
                            AgregarVuelo(orig, dest, costo);
                            contador++;
                        }
                    }
                }
                Console.WriteLine("Se cargaron exitosamente {0} vuelos desde '{1}'.", contador, nombreArchivo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al leer el archivo: " + ex.Message);
            }
        }

        // Metodo auxiliar para crear el archivo si no existe
        public void CrearArchivoEjemplo(string nombreArchivo)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(nombreArchivo))
                {
                    sw.WriteLine("# Base de Datos Ficticia de Vuelos Comerciales");
                    sw.WriteLine("# Formato: Origen,Destino,Precio");
                    sw.WriteLine("Quito,Guayaquil,55.00");
                    sw.WriteLine("Quito,Cuenca,65.00");
                    sw.WriteLine("Quito,Manta,45.00");
                    sw.WriteLine("Quito,Coca,50.00");
                    sw.WriteLine("Quito,Galapagos,140.00");
                    sw.WriteLine("Guayaquil,Quito,58.00");
                    sw.WriteLine("Guayaquil,Cuenca,38.00");
                    sw.WriteLine("Guayaquil,Galapagos,110.00");
                    sw.WriteLine("Guayaquil,Manta,35.00");
                    sw.WriteLine("Cuenca,Quito,62.00");
                    sw.WriteLine("Cuenca,Loja,30.00");
                    sw.WriteLine("Cuenca,Guayaquil,40.00");
                    sw.WriteLine("Manta,Quito,48.00");
                    sw.WriteLine("Manta,Galapagos,125.00");
                    sw.WriteLine("Loja,Guayaquil,42.00");
                    sw.WriteLine("Coca,Quito,52.00");
                }
                Console.WriteLine("Archivo de ejemplo '{0}' creado correctamente.", nombreArchivo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al crear el archivo: " + ex.Message);
            }
        }

        // REPORTERIA 1: Listar todas las ciudades registradas
        public void ReporteCiudades()
        {
            Console.WriteLine("=================================================");
            Console.WriteLine("           REPORTE DE CIUDADES (VERTICES)        ");
            Console.WriteLine("=================================================");
            if (totalCiudades == 0)
            {
                Console.WriteLine("No hay ciudades registradas en el sistema.");
                return;
            }

            for (int i = 0; i < totalCiudades; i++)
            {
                Console.WriteLine(" [{0}] {1}", i + 1, nombresCiudades[i]);
            }
            Console.WriteLine("Total de ciudades registradas: {0}", totalCiudades);
        }

        // REPORTERIA 2: Listar todos los vuelos existentes
        public void ReporteVuelos()
        {
            Console.WriteLine("=================================================");
            Console.WriteLine("          REPORTE DE VUELOS DIRECTOS (ARISTAS)   ");
            Console.WriteLine("=================================================");
            int totalVuelos = 0;

            for (int i = 0; i < totalCiudades; i++)
            {
                for (int j = 0; j < totalCiudades; j++)
                {
                    if (i != j && matrizCostos[i, j] < INFINITO)
                    {
                        Console.WriteLine(" - De {0,-12} hacia {1,-12} | Costo: ${2,7:F2}",
                            nombresCiudades[i], nombresCiudades[j], matrizCostos[i, j]);
                        totalVuelos++;
                    }
                }
            }

            if (totalVuelos == 0)
            {
                Console.WriteLine("No existen vuelos registrados actualmente.");
            }
            else
            {
                Console.WriteLine("Total de vuelos directos disponibles: {0}", totalVuelos);
            }
        }

        // REPORTERIA 3: Visualizar la matriz de adyacencia
        public void ReporteMatrizAdyacencia()
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("               REPORTE: MATRIZ DE ADYACENCIA (COSTOS EN $)               ");
            Console.WriteLine("=========================================================================");

            if (totalCiudades == 0)
            {
                Console.WriteLine("El grafo se encuentra vacio.");
                return;
            }

            // Encabezado de columnas
            Console.Write("{0,-12}", "Ciudad");
            for (int i = 0; i < totalCiudades; i++)
            {
                // Mostramos nombres recortados a 8 letras para que encaje bien
                string col = nombresCiudades[i];
                if (col.Length > 8) col = col.Substring(0, 8);
                Console.Write("{0,10}", col);
            }
            Console.WriteLine();
            Console.WriteLine(new string('-', 12 + (totalCiudades * 10)));

            // Filas
            for (int i = 0; i < totalCiudades; i++)
            {
                string fila = nombresCiudades[i];
                if (fila.Length > 11) fila = fila.Substring(0, 11);
                Console.Write("{0,-12}", fila);

                for (int j = 0; j < totalCiudades; j++)
                {
                    if (matrizCostos[i, j] >= INFINITO)
                    {
                        Console.Write("{0,10}", "INF");
                    }
                    else
                    {
                        Console.Write("{0,10:F0}", matrizCostos[i, j]);
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("Nota: 'INF' indica que no hay vuelo directo entre ambas ciudades.");
        }

        // Algoritmo de Dijkstra para encontrar la ruta mas barata
        public void BuscarRutaMasBarata(string origen, string destino)
        {
            int idxOrigen = ObtenerIndiceCiudad(origen);
            int idxDestino = ObtenerIndiceCiudad(destino);

            if (idxOrigen == -1)
            {
                Console.WriteLine("Error: La ciudad de origen '{0}' no existe en el sistema.", origen);
                return;
            }
            if (idxDestino == -1)
            {
                Console.WriteLine("Error: La ciudad de destino '{0}' no existe en el sistema.", destino);
                return;
            }
            if (idxOrigen == idxDestino)
            {
                Console.WriteLine("El origen y el destino son la misma ciudad. Costo = $0.00");
                return;
            }

            // Medir el tiempo de ejecucion con Stopwatch
            Stopwatch cronometro = new Stopwatch();
            cronometro.Start();

            // Estructuras clasicas de Dijkstra
            double[] distancia = new double[totalCiudades];
            bool[] visitado = new bool[totalCiudades];
            int[] previo = new int[totalCiudades];

            // Inicializacion
            for (int i = 0; i < totalCiudades; i++)
            {
                distancia[i] = INFINITO;
                visitado[i] = false;
                previo[i] = -1;
            }

            distancia[idxOrigen] = 0;

            // Bucle principal de Dijkstra
            for (int count = 0; count < totalCiudades - 1; count++)
            {
                // Buscar el vertice no visitado con menor distancia acumulada
                double menor = INFINITO;
                int u = -1;

                for (int v = 0; v < totalCiudades; v++)
                {
                    if (!visitado[v] && distancia[v] <= menor)
                    {
                        menor = distancia[v];
                        u = v;
                    }
                }

                // Si no hay nodo alcanzable o ya no se puede mejorar, salir
                if (u == -1 || menor == INFINITO)
                {
                    break;
                }

                visitado[u] = true;

                // Actualizar los vecinos del nodo 'u'
                for (int v = 0; v < totalCiudades; v++)
                {
                    if (!visitado[v] && matrizCostos[u, v] < INFINITO)
                    {
                        double nuevaDistancia = distancia[u] + matrizCostos[u, v];
                        if (nuevaDistancia < distancia[v])
                        {
                            distancia[v] = nuevaDistancia;
                            previo[v] = u;
                        }
                    }
                }
            }

            cronometro.Stop();

            // Mostrar resultados del calculo
            Console.WriteLine();
            Console.WriteLine("=================================================");
            Console.WriteLine("          RESULTADO DEL VUELO MAS ECONOMICO      ");
            Console.WriteLine("=================================================");
            Console.WriteLine(" Ciudad de Origen : {0}", nombresCiudades[idxOrigen]);
            Console.WriteLine(" Ciudad de Destino: {0}", nombresCiudades[idxDestino]);

            if (distancia[idxDestino] >= INFINITO)
            {
                Console.WriteLine("No existe ninguna ruta de vuelos disponible para conectar ambas ciudades.");
            }
            else
            {
                Console.WriteLine(" Costo Total Minimo: ${0:F2}", distancia[idxDestino]);

                // Reconstruir la ruta optima usando el arreglo 'previo'
                int[] ruta = new int[totalCiudades];
                int pasos = 0;
                int actual = idxDestino;

                while (actual != -1)
                {
                    ruta[pasos] = actual;
                    pasos++;
                    actual = previo[actual];
                }

                Console.Write(" Itinerario de Vuelo: ");
                for (int i = pasos - 1; i >= 0; i--)
                {
                    Console.Write(nombresCiudades[ruta[i]]);
                    if (i > 0)
                    {
                        Console.Write(" -> ");
                    }
                }
                Console.WriteLine();

                if (pasos > 2)
                {
                    Console.WriteLine(" Tipo de Vuelo: Vuelo con escala(s) ({0} escalas intermedias)", pasos - 2);
                }
                else
                {
                    Console.WriteLine(" Tipo de Vuelo: Vuelo directo");
                }
            }

            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine(" Metricas de Rendimiento:");
            Console.WriteLine("  * Tiempo de ejecucion: {0} ms ({1} ticks)", 
                cronometro.ElapsedMilliseconds, cronometro.ElapsedTicks);
            Console.WriteLine("  * Complejidad temporal teorica: O(V^2)");
            Console.WriteLine("=================================================");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            GrafoVuelos sistemaVuelos = new GrafoVuelos();
            string archivoVuelos = "vuelos.txt";

            // Intentar cargar desde archivo si existe, sino cargar predeterminados
            if (File.Exists(archivoVuelos))
            {
                sistemaVuelos.CargarDesdeArchivo(archivoVuelos);
            }
            else
            {
                sistemaVuelos.CargarDatosPredeterminados();
                sistemaVuelos.CrearArchivoEjemplo(archivoVuelos);
            }

            int opcion = -1;

            do
            {
                Console.WriteLine();
                Console.WriteLine("*************************************************");
                Console.WriteLine("  SISTEMA DE GESTION Y BUSQUEDA DE VUELOS ECONOMICOS ");
                Console.WriteLine("      UNIVERSIDAD ESTATAL AMAZONICA (UEA)        ");
                Console.WriteLine("      ESTRUCTURA DE DATOS - UNIDAD IV: GRAFOS    ");
                Console.WriteLine("*************************************************");
                Console.WriteLine(" 1. Ver lista de ciudades registradas (Vertices)");
                Console.WriteLine(" 2. Ver lista de vuelos directos (Aristas)");
                Console.WriteLine(" 3. Ver matriz de adyacencia (Costos de vuelos)");
                Console.WriteLine(" 4. Buscar vuelo mas barato entre dos ciudades (Dijkstra)");
                Console.WriteLine(" 5. Registrar una nueva ciudad");
                Console.WriteLine(" 6. Registrar un nuevo vuelo directo");
                Console.WriteLine(" 7. Recargar datos desde archivo 'vuelos.txt'");
                Console.WriteLine(" 0. Salir");
                Console.WriteLine("*************************************************");
                Console.Write(" Ingrese una opcion: ");

                string entrada = Console.ReadLine();
                if (!int.TryParse(entrada, out opcion))
                {
                    Console.WriteLine("Entrada no valida. Debe ingresar un numero entero.");
                    continue;
                }

                Console.WriteLine();

                switch (opcion)
                {
                    case 1:
                        sistemaVuelos.ReporteCiudades();
                        break;

                    case 2:
                        sistemaVuelos.ReporteVuelos();
                        break;

                    case 3:
                        sistemaVuelos.ReporteMatrizAdyacencia();
                        break;

                    case 4:
                        Console.WriteLine("--- BUSCADOR DE VUELO MAS ECONOMICO ---");
                        Console.Write("Ingrese la ciudad de origen: ");
                        string origen = Console.ReadLine();
                        Console.Write("Ingrese la ciudad de destino: ");
                        string destino = Console.ReadLine();

                        if (!string.IsNullOrEmpty(origen) && !string.IsNullOrEmpty(destino))
                        {
                            sistemaVuelos.BuscarRutaMasBarata(origen, destino);
                        }
                        else
                        {
                            Console.WriteLine("Error: El origen y el destino no pueden estar vacios.");
                        }
                        break;

                    case 5:
                        Console.WriteLine("--- REGISTRO DE NUEVA CIUDAD ---");
                        Console.Write("Ingrese el nombre de la ciudad: ");
                        string nuevaCiudad = Console.ReadLine();
                        if (!string.IsNullOrEmpty(nuevaCiudad))
                        {
                            int idx = sistemaVuelos.AgregarCiudad(nuevaCiudad);
                            if (idx != -1)
                            {
                                Console.WriteLine("Ciudad '{0}' registrada con exito en el indice {1}.", nuevaCiudad, idx + 1);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error: El nombre de la ciudad no es valido.");
                        }
                        break;

                    case 6:
                        Console.WriteLine("--- REGISTRO DE NUEVO VUELO ---");
                        Console.Write("Ciudad de origen: ");
                        string origVuelo = Console.ReadLine();
                        Console.Write("Ciudad de destino: ");
                        string destVuelo = Console.ReadLine();
                        Console.Write("Precio del pasaje ($): ");
                        double costoVuelo;
                        string entradaPrecio = Console.ReadLine().Trim().Replace(',', '.');
                        if (double.TryParse(entradaPrecio, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out costoVuelo) && costoVuelo >= 0)
                        {
                            bool ok = sistemaVuelos.AgregarVuelo(origVuelo, destVuelo, costoVuelo);
                            if (ok)
                            {
                                Console.WriteLine("Vuelo registrado: {0} -> {1} por ${2:F2}", origVuelo, destVuelo, costoVuelo);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error: Precio ingresado no es valido.");
                        }
                        break;

                    case 7:
                        sistemaVuelos = new GrafoVuelos();
                        sistemaVuelos.CargarDesdeArchivo(archivoVuelos);
                        break;

                    case 0:
                        Console.WriteLine("Gracias por utilizar el sistema de vuelos. Saliendo...");
                        break;

                    default:
                        Console.WriteLine("Opcion invalida. Intente de nuevo.");
                        break;
                }

                if (opcion != 0)
                {
                    Console.WriteLine("\nPresione ENTER para continuar...");
                    Console.ReadLine();
                }

            } while (opcion != 0);
        }
    }
}
