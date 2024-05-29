using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace repetition2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //сериализация, десериализация, дериктории, работа с файлами
            // - процесс преобразования объектов в последовательность байтов: бинарные, джисоны, xml, 

            XmlDocument xmlDocument = new XmlDocument(); //создаем документ

            XmlElement root = xmlDocument.CreateElement("Bookstore"); //соз элемент, который мы присоединяем к хмл документу корневую папку буук стор
            xmlDocument.AppendChild(root); //корневая папка

            XmlElement book = xmlDocument.CreateElement("Book"); //у рута дочерний элемент будет книга 
            book.SetAttribute("ISBN", "123456"); // сама книга, атрибут
            root.AppendChild(book); //Закрепили

            XmlElement title = xmlDocument.CreateElement("Title"); //название книги, создали титл внутри папки бук
            title.InnerText = "C# Programming"; //текст между открывающими и закрывающими тегами <>...<>
            book.AppendChild(title); //добавить дочерний элемент, титл к буку

            xmlDocument.Save("books.xml"); //сохранили




            //создание документа, вначале загружаем
            XmlDocument xmlDocument = new XmlDocument();

            xmlDocument.Load("books.xml"); //загружаем хмл файл

            XmlNode rood = xmlDocument.DocumentElement; //разбираем документ, вытаскиваем

            foreach (XmlNode bookNode in rood.ChildNodes)
            {
                Console.WriteLine($"Book ISBN: {bookNode.Attributes["ISBN"].Value}");
                Console.WriteLine($"Title: {bookNode["Title"].InnerText}");
            }

            //делаем изменения
            XmlNode bookNode = xmlDocument.SelectSingleNode("/Bookstore/Book[@ISBN='123456']/Title");

            if (bookNode != null)
            {
                bookNode.InnerText = "Update";
            }
            xmlDocument.Save("books.xml");

            //-----------------------------------------------------------------------------

            //джейсон, текстовые файлы

            //запись в файл для двоичных
            string filePath = "output.txt"; //путь
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))  //использовать файловый поток (путь, создать, пишем)
            {
                string text = "Hello";
                //создаем буфер байтов //декодируем аски код в виде бвйтов
                byte[] buffer = System.Text.Encoding.ASCII.GetBytes(text);

                fileStream.Write(buffer, 0, buffer.Length); //массив байтов, значение offset, длина массивов байтов
            }

            //читать файл
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))  //использовать файловый поток (путь, открытьб прочитать )
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0) //пока байтов для чтения > 0
                {
                    string text1 = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead); //наш текст переводится из байтового вида в текст
                    Console.WriteLine(text1);
                }
            }

            //писатель в файл StreamWriter 
            string filePath2 = "output2.txt";

            using (StreamWriter writer = new StreamWriter(filePath2, true))
            {
                writer.WriteLine("пишем че хотим");
            }

            //StreamReader
            using (StreamReader writer2 = new StreamReader(filePath2, true))
            {
                string line;
                while ((line = writer2.ReadLine()) != null) //пока у нас есть еще стрроки
                {
                    Console.WriteLine(line);
                }
            }
            Console.WriteLine(File.ReadAllText(filePath2)); //перевести в строку весь текст файла


            //менеджеры пакетов (библиотек) NuGet джейсон
            var person = new { Name = "Alice", Age = 20, IsMarried = false }; //создаем объект
            string Json = JsonConvert.SerializeObject(person); //создаем сериализуемый объект в виде строки json
            Console.WriteLine(Json);
            var deserialized = JsonConvert.DeserializeObject<Person>(Json); // json переводим в объект класса персон
            Console.WriteLine($"Name: {deserialized.Name}, Age: {deserialized.Age}, Is married: {deserialized.IsMarried}");

            //Task1 читаем из документов -------------------------------------------------------------------------
            List<Book> books = new List<Book>();
            XDocument xmlDoc = XDocument.Load("books2.xml"); // xдокумент позволяет использовать линку
            foreach (var element in xmlDoc.Element("books").Elements("book")) //корневой элемент, у которого внутри элементы (буквы) - иначе говоря список
            {
                Book book = new Book
                {
                    Title = element.Element("title").Value,
                    Author = element.Element("author").Value,
                    Year = int.Parse(element.Element("year").Value)
                };
                books.Add(book);
            }

            using (StreamReader r = new StreamReader("books.json"))
            {
                string json = r.ReadToEnd();//читаем
                dynamic array = JsonConvert.DeserializeObject(json); //массив 
                foreach (var item in array.books)
                {
                    Book book = new Book
                    {
                        Title = item.title,
                        Author = item.author,
                        Year = item.year
                    };
                    books.Add(book);
                }
            }
            using (StreamReader r = new StreamReader("books.txt"))
            {
                string line;
                while ((line = r.ReadLine()) != null) //пока наши строки не закончились
                {
                    string[] parts = line.Split(',');//сплитим до запятых
                    if (parts.Length == 3)
                    {
                        Book book = new Book
                        {
                            Title = parts[0].Trim(),
                            Author = parts[1].Trim(),
                            Year = int.Parse(parts[2].Trim()) //трим убирает пробелы в начале и в конце
                        };
                        books.Add(book);
                    }

                }
            }

            foreach (var book in books)
            {
                Console.WriteLine($"{book.Title}, {book.Author}, {book.Year}");
            }





            Console.ReadKey();
        }


    }


    //Task1
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
    }



    public class Person
    {
        public string Name { get; set; }
        public string Age { get; set; }
        public bool IsMarried { get; set; }
    }
}
