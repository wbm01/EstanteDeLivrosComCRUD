using EstantedeLivrosComCrud;
using static System.Reflection.Metadata.BlobBuilder;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;

internal class Program
{
    private static void Main(string[] args)
    {
        MongoClient client = new MongoClient("mongodb://localhost:27017");
        var db = client.GetDatabase("Biblioteca");
        var collectionLivro = db.GetCollection<BsonDocument>("Livro");
        var collectionEmprestado = db.GetCollection<BsonDocument>("LivroEmprestado");
        var collectionLivroLeitura = db.GetCollection<BsonDocument>("LivroLeitura");

        int escolhamenu;

        do
        {
            escolhamenu = Menu();

            switch (escolhamenu)
            {
                case 0:

                    System.Environment.Exit(0);
                    break;

                case 1:

                    Books books = CadastrarLivro();

                    var cadastrarbook = new BsonDocument();

                    if (books.Writer2 != null)
                    {
                        cadastrarbook = new BsonDocument {
                            {"ISBN", books.ISBN},
                            {"Título", books.Name},
                            {"Editora", books.Edition},
                            {"Ano Publicacao", books.PublicationYear},
                            {"Autor", books.Writer},
                            {"Autor 2", books.Writer2}
                        };
                    }
                    else
                    {
                        cadastrarbook = new BsonDocument
                        {
                            {"ISBN", books.ISBN},
                            {"Título", books.Name},
                            {"Editora", books.Edition},
                            {"Ano Publicacao", books.PublicationYear},
                            {"Autor", books.Writer}
                        };
                    }
                    collectionLivro.InsertOne(cadastrarbook);
                    break;

                case 2:

                    Console.Write("\nInforme o nome do livro que deseja emprestar: ");
                    var emprestar = Console.ReadLine();

                    try
                    {
                        var emprestado = collectionLivro.Find(Builders<BsonDocument>.Filter.Eq("Título", emprestar)).FirstOrDefault();
                        collectionEmprestado.InsertOne(emprestado);

                        collectionLivro.FindOneAndDelete(emprestado);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        Console.WriteLine("\nLivro emprestado com sucesso!");
                        Thread.Sleep(2000);
                    }
                    break;

                case 3:

                    Console.Write("\nInforme o nome do livro que deseja ler: ");
                    var livroLer = Console.ReadLine();
                    try
                    {
                        var trazerLivro = collectionLivro.Find(Builders<BsonDocument>.Filter.Eq("Título", livroLer)).FirstOrDefault();

                        collectionLivroLeitura.InsertOne(trazerLivro);

                        collectionLivro.FindOneAndDelete(trazerLivro);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Thread.Sleep(2000);
                    }
                    finally
                    {
                        Console.WriteLine("\nLivro separado com sucesso!");
                        Thread.Sleep(2000);
                    }
                    break;

                case 4:

                    var livrosdisponiveis = collectionLivro.Find(new BsonDocument()).ToList();

                    if (livrosdisponiveis == null)
                    {
                        Console.WriteLine("Não há livros disponíveis!");
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        var disponivel = livrosdisponiveis.ToList();
                        disponivel.ForEach(item => Console.WriteLine(BsonSerializer.Deserialize<Books>(item.ToString())));

                        //var livro = BsonSerializer.Deserialize<Books>(disponivel);
                        /*foreach (var disponivel in livrosdisponiveis)
                        {
                            var livro = BsonSerializer.Deserialize<Books>(disponivel);
                            Console.WriteLine(livro.ToString());
                            Console.WriteLine();
                        }*/
                    }
                    break;

                case 5:

                    var listaLivrosEmprestados = collectionEmprestado.Find(new BsonDocument()).ToList();

                    var listaEmprestado = listaLivrosEmprestados.ToList();

                    if (listaEmprestado == null)
                    {
                        Console.WriteLine("Não há livros emprestados!");
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        listaEmprestado.ForEach(item => Console.WriteLine(BsonSerializer.Deserialize<Books>(item.ToString())));
                        Console.WriteLine();

                        Console.Write("\nDeseja devolver esse livro? (S ou N): ");
                        var devolucao = Console.ReadLine().ToUpper();

                        if(devolucao == "S")
                        {
                            var devolucaoLivro = collectionEmprestado.Find(Builders<BsonDocument>.Filter.Eq("Título", item)).FirstOrDefault();
                            collectionLivro.InsertOne(devolucaoLivro);
                            //collectionEmprestado.Find(Builders<BsonDocument>.Filter.Eq("Título", item);
                        }
                    }

                    /*foreach (var emprestado in listaLivrosEmprestados)
                    {
                        var livroEmprestado = BsonSerializer.Deserialize<Books>(emprestado);
                        Console.WriteLine(livroEmprestado.ToString());
                        Console.WriteLine();
                    }*/
                    break;

                case 6:

                    var listaLivrosLeitura = collectionLivroLeitura.Find(new BsonDocument()).ToList();
                    
                    var lista = listaLivrosLeitura.ToList();

                    if (lista == null)
                    {
                        Console.WriteLine("Não há livros separados para leitura!");
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        lista.ForEach(item => Console.WriteLine(BsonSerializer.Deserialize<Books>(item.ToString())));
                        Console.WriteLine();
                    }
                    break;

                case 7:
                    try
                    {
                        Console.Write("\nInforme o nome do livro que deseja deletar: ");
                        var livrodeletar = Console.ReadLine();

                        var delete = collectionLivro.Find(Builders<BsonDocument>.Filter.Eq("Título", livrodeletar)).FirstOrDefault();

                        collectionLivro.FindOneAndDelete(delete);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"\nError: {ex}");
                        Thread.Sleep(2000);
                    }
                    finally
                    {
                        Console.WriteLine("\nLivro deletado com sucesso!");
                        Thread.Sleep(2000);
                    }
                    break;
            }

        } while (true);

        int Menu()
        {

            int escolhamenu;

            Console.WriteLine("**BEM-VINDO A ESTANTE VIRTUAL!**");

            Console.WriteLine("\n1 - Cadastrar Livro" + "\n2 - Emprestar Livro" +
                "\n3 - Ler Livro" + "\n4 - Lista de Livros disponíveis" + "\n5 - Lista de Livros Emprestados" +
                "\n6 - Lista de Livros Separados para Leitura" + "\n7 - Deletar Livro" + "\n0 - Sair");

            Console.Write("\nEscolha a opção desejada: ");
            escolhamenu = int.Parse(Console.ReadLine());

            return escolhamenu;

        }

        Books CadastrarLivro()
        {
            try
            {
                Console.Write("\nInforme o nome do livro: ");
                string name = Console.ReadLine();

                Console.Write("\nInforme a edição: ");
                string edition = Console.ReadLine();

                Console.Write("\nInforme o autor: ");
                string writer = Console.ReadLine();

                Console.Write("\nInforme o autor 2: ");
                string writer2 = Console.ReadLine();

                Console.Write("\nInforme o ISBN: ");
                string isbn = Console.ReadLine();

                Console.Write("\nInforme o ano de publicação: ");
                string publicationYear = Console.ReadLine();

                Books books = new Books(name, edition, writer, isbn, publicationYear);

                if (string.IsNullOrEmpty(writer2))
                {

                    return books;
                }
                else
                {
                    books.Writer2 = writer2;
                    return books;
                }
            }
            catch
            {
                Console.WriteLine("\nNão foi possível gravar!");
                Thread.Sleep(2000);
            }
            finally
            {
                Console.WriteLine("\nLivro cadastrado com sucesso!");
                Thread.Sleep(2000);
            }
            return null;
        }
    }
}