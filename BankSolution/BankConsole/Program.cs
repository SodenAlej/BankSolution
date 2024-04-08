using BankConsole;
using System.Text.RegularExpressions;


if (args.Length == 0)
    EmailService.SendMail();
else
    ShowMenu();

void ShowMenu()
{
    Console.Clear();
    Console.WriteLine("Selecciona una opción: ");
    Console.WriteLine("1.- Crear un usuario nuevo.");
    Console.WriteLine("2.- Eliminar un usuario existente.");
    Console.WriteLine("3.- Salir");

    int option = 0;
    do
    {
        string input = Console.ReadLine();

        if (!int.TryParse(input, out option))
            Console.WriteLine("Debes ingresar un número (1, 2 o 3).");
        else if (option > 3)
            Console.WriteLine("Debes ingresar un número válido (1, 2 o 3).");
    } while (option == 0 || option > 3);

    switch (option)
    {
        case 1:
            CreateUser();
            break;
        case 2:
            DeleteUser();
            break;
        case 3:
            Environment.Exit(0);
            break;
    }
}

void CreateUser()
{
    Console.Clear();
    Console.WriteLine("Ingresa la información del usuario: ");
    int ID = 0;

    ID = validarID();

    Console.Write("Nombre: ");
    string name = Console.ReadLine();

    Console.Write("Email: ");
    string email;
    email = validarCorreo();
    bool valido = false;
    decimal balance = 0;
    while (!valido)
    {
        do
        {
            try
            {
                Console.Write("Saldo: ");
                balance = decimal.Parse(Console.ReadLine());
                valido = true;
                if (balance < 0)
                    Console.WriteLine("Debe colocar un numero mayor a cero");
            }
            catch(FormatException)
            {
                Console.WriteLine("Error de formato. Tiene que ser un numero valido");
            }
        }
        while (balance < 0);
    }

    Console.Write("Escribe 'c' si el usuario es Cliente o 'e' si es Empleado: ");
    char userType = char.Parse("x");

    valido = false;
    while (!valido)
    {
        do
        {
            try
            {
                userType = char.Parse(Console.ReadLine());
                valido = true;
                if (userType != char.Parse("c") && userType != char.Parse("e"))
                    Console.WriteLine("Ingrese un caracter valido ('e' o 'c')");
            }
            catch (FormatException)
            {
                Console.WriteLine("Error de formato. Se debe de incluir un caracter valido.");
            }

        } while (userType != char.Parse("c") && userType != char.Parse("e"));
    }

    User newUser;

    if (userType.Equals('c'))
    {
        Console.Write("Regimen Fiscal: ");
        char taxRegime = char.Parse(Console.ReadLine());

        newUser = new Client(ID, name, email, balance, taxRegime);
    }
    else
    {
        Console.Write("Departamento: ");
        string department = Console.ReadLine();

        newUser = new Employee(ID, name, email, balance, department);
    }

    Storage.AddUser(newUser);

    Console.WriteLine("Usuario creado.");
    Thread.Sleep(2000);
    ShowMenu();
}

void DeleteUser()
{
    Console.Clear();

    Console.WriteLine("Ingresa el ID del usuario a eliminar: ");

    int ID = validarIDtoDelete();

    string result = Storage.DeleteUser(ID);

    if (result.Equals("Success"))
    {
        Console.Write("Usuario eliminado.");
        Thread.Sleep(2000);
        ShowMenu();
    }
}

string validarCorreo()
{
    string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    bool valido = true;
    string email;
    do
    {
        email = Console.ReadLine();
        valido = Regex.IsMatch(email, pattern);
        if (valido == false)
            Console.WriteLine("Ingrese un correo electronico valido.");
    }
    while (!valido);

    return email;
}

int validarID()
{
    int ID = 0;
    bool valido = false;
    string existID = "";
    while (!valido)
    {
        do
        {
            try
            {
                Console.Write("ID: ");
                ID = int.Parse(Console.ReadLine());
                valido = true;
                if (ID < 0)
                    Console.WriteLine("Debe colocar un numero mayor a cero");
                existID = Storage.searchByID(ID);
                if (existID != null && ID >= 0 )
                {
                    Console.WriteLine("El ID ya existe");
                }
            }
            catch
            {
                Console.WriteLine("Error de formato. Tiene que ser un numero entero");
            }
        }
        while (ID < 0 || existID != null);
    }
    return ID;
}

int validarIDtoDelete()
{
    int ID = 0;
    bool valido = false;
    string existID = "";
    while (!valido)
    {
        do
        {
            try
            {
                Console.Write("ID: ");
                ID = int.Parse(Console.ReadLine());
                valido = true;
                if (ID < 0)
                    Console.WriteLine("Debe colocar un numero mayor a cero");
                existID = Storage.searchByID(ID);
                if (existID == null && ID > 0 )
                {
                    Console.WriteLine("El ID no existe");
                }
            }
            catch
            {
                Console.WriteLine("Error de formato. Tiene que ser un numero entero");
            }
        }
        while (ID < 0 || existID == null);
    }
    return ID;
}
