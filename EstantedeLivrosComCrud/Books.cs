using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace EstantedeLivrosComCrud
{
    internal class Books
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement ("Título")]
        public string Name { get; set; }

        [BsonElement ("Editora")]
        public string Edition { get; set; }

        [BsonElement ("Autor")]
        public string Writer { get; set; }

        [BsonElement ("Autor 2")]
        public string? Writer2 { get; set; }

        [BsonElement ("ISBN")]
        public string ISBN { get; set; }

        [BsonElement ("Ano Publicacao")]
        public string PublicationYear { get; set; }

        public Books(string name, string edition, string writer, string isbn, string publicationYear)
        {

            this.Name = name;
            this.Edition = edition;
            this.Writer = writer;
            this.ISBN = isbn;
            this.PublicationYear = publicationYear;
        }


        public override string ToString()
        {
            return "\nTítulo: " + (Name) + "\nEditora: " + (Edition) + "\nAutor 1: " + (Writer)
                + "\nAutor 2: " + (Writer2) + "\nISBN: " + (ISBN) +
                "\nAno de Publicação: " + (PublicationYear);
        }
    }
}
