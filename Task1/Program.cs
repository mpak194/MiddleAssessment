using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MiddleAssessment
{
    class Program
    {
        /// <summary>
        /// Метод перевода string в int
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns>Целочисленное значение</returns>
        static int GetIntValue(string str)
        {
            if (int.TryParse(str, out int n))
                return n;
            throw new Exception();
        }

        /// <summary>
        /// Метод перевода string в int c ограничениями
        /// </summary>
        /// <param name="str">Строка</param>
        /// <param name="minBorder">Нижняя граница</param>
        /// <param name="maxBorder">Верхняя граница</param>
        /// <returns>Целочисленное значение</returns>
        static int GetIntValue(string str, int minBorder,
            int maxBorder = int.MaxValue)
        {
            if (int.TryParse(str, out int n) && n >= minBorder &&
                n <= maxBorder) return n;
            throw new Exception();
        }

        /// <returns></returns>
        /// /// <summary>
        /// Метод получения значения среднего балла из файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// /// <param name="error">Индикатор ошибки</param>
        /// <returns>Среднее значение</returns>
        static double GetMiddleAssessment(string path, out bool error)
        {
            StreamReader sr = null;
            string fileResponseString;
            int fileResponseInt;
            double middleAssessment = 0;
            try
            {
                sr = new StreamReader(path);
                try
                {
                    while (!sr.EndOfStream)
                    {
                        // Чтение числа учеников.
                        fileResponseString = sr.ReadLine();
                        // Перевод в int с ограничением.
                        fileResponseInt = GetIntValue(fileResponseString, 1);
                        int quantityPeople = fileResponseInt;
                        // Чтение оценок учеников.
                        fileResponseString = sr.ReadLine();
                        // Запись оценок в массив без пробелов.
                        string[] peopleAssessments = fileResponseString.Split();
                        int OneAssessment = 0;
                        // Проверка на выход за границы массива.
                        if (peopleAssessments.Length > quantityPeople)
                            throw new IndexOutOfRangeException();
                        // Подсчет среднего балла.
                        for (int i = 0; i < peopleAssessments.Length; i++)
                        {
                            int assessment = GetIntValue(peopleAssessments[i], 0, 10);
                            OneAssessment += assessment;
                        }
                        Console.WriteLine("Процесс подсчета среднего балла завершен успешно...");
                        error = false;
                        return middleAssessment = OneAssessment / quantityPeople;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Проверьте введенные данные\n" +
                        "Обнаружен выход за границы массива...");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка преобразования в int или" +
                        " неверный диапазон значений...");
                }

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Данного файла не существует...");
                Console.WriteLine(e.Message);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (sr != null) sr.Close();
            }
            error = true;
            return double.NaN;
        }

        /// <summary>
        /// Метод записи среденего балла
        /// </summary>
        /// <param name="path">Файл для записи</param>
        /// <param name="middleAssessment">Средний балл</param>
        /// <param name="reWrite">Перезаписывать или заменять значение</param>
        static void SetMiddleAssessment(string path, double middleAssessment,
            bool reWrite)
        {
            StreamWriter sw = new StreamWriter(path, reWrite);
            try
            {
                string response = middleAssessment.ToString("F3") + "\r\n";
                sw.Write(response);
                Console.WriteLine("Процесс записи среднего балла завершен успешно...");
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                sw.Close();
            }
        }

        static void Main(string[] args)
        {
            do
            {
                bool error;
                // Определение среднего балла.
                double middleAssessment = GetMiddleAssessment(@"../../../Input.txt",
                    out error);
                if (!error)
                {
                    Console.WriteLine("Желаете добавить ответ к предыдущему? " +
                        "Нажмите ESC для отмены...");
                    bool reWrite = true;
                    if (Console.ReadKey(true).Key == ConsoleKey.Escape) reWrite = false;
                    SetMiddleAssessment(@"../../../Output.txt", middleAssessment,
                        reWrite);
                }
                else Console.WriteLine("Что-то пошло не так...");
                Console.WriteLine("Для выхода нажмите ESC...");
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }
    }
}
