using Manager;
using System;
using System.Collections;
using System.Text;
using System.Windows.Input;



Console.OutputEncoding = UTF8Encoding.UTF8; //Для української мови


Menu();


void Menu()
{
    AccountsManager MainObject = new AccountsManager();
    string? Comand;
    while (true)
    {
        Console.WriteLine("Акаунти:");
        if (MainObject.Accounts is not null)
        {
            for (int i = 0; i < MainObject.Accounts.Count; i++)
            {
                Console.WriteLine(ConvertNumber(MainObject.Accounts[i].AccountNumber));
            }
        }
        Console.WriteLine("                 Введіть номер бажаної операції:");
        Console.WriteLine("Інформація про рахунок: 1");
        Console.WriteLine("Поповнення рахунку: 2");
        Console.WriteLine("Зняти кошти: 3");
        Console.WriteLine("Визначення кількості грошей на депозитах рахунку: 4");
        Console.WriteLine("Всі депозити : 5");
        Console.WriteLine("Відкриття депозиту: 6");
        Console.WriteLine("Закриття депозиту: 7");
        Comand = Console.ReadLine();
        switch (Comand)
        {
            case "1": 
                {
                    int code;
                    BankAccount? bankAccount = null; 
                    Console.Clear();
                    while (true)
                    {
                        code = GetAccountNumber(ref bankAccount, MainObject);
                        if (code == 1) break;
                        else if (code == 2) continue;

                        Ending(bankAccount.ToString());
                        break;
                    }
                    break;
                }
            case "2": 
                {
                    int code = 0;
                    string? input = null;
                    double amount = 0.0;
                    BankAccount? bankAccount = null;
                    code = GetAccountNumber(ref bankAccount, MainObject); 
                    if (code == 1) break;
                    else if (code == 2) continue;

                    while (true)
                    {
                        Console.WriteLine("Сума");
                        code = CheckNumber(ref input, ref amount); 
                        if (code == 1) break;
                        else if (code == 2) continue;

                        bankAccount.Amount += amount;
                        Ending($"{amount} грн були успішно нараховані\nБаланс: {bankAccount.Amount} грн.");

                        break;
                    }

                    break;
                }
            case "3":
                {
                    int code;
                    string? input = null;
                    double amount = 0.0;
                    BankAccount? bankAccount = null;
                    code = GetAccountNumber(ref bankAccount, MainObject); 
                    if (code == 1) break;
                    else if (code == 2) continue;

                    if (bankAccount.Amount == 0)
                    {
                        Ending("Недостатньо коштів");
                        continue;
                    }

                    while (true)
                    {
                        Console.WriteLine("Сума зняття:");
                        code = CheckNumber(ref input, ref amount); 
                        if (code == 1) break;
                        else if (code == 2) continue;

                        if (amount > bankAccount.Amount) 
                        {
                            Ending("Недостатньо коштів");
                            continue;
                        }
                        bankAccount.Amount -= amount;
                        Ending($"{amount} грн були успішно зняті\nБаланс: {bankAccount.Amount} грн.");
                        break;
                    }
                    break;
                }
            case "4":
                {
                    int code;
                    BankAccount? bankAccount = null; 
                    Console.Clear();
                    while (true)
                    {
                        code = GetAccountNumber(ref bankAccount, MainObject); 
                        if (code == 1) break;
                        else if (code == 2) continue;

                        if (bankAccount.Deposits.Count == 0)
                        {
                            Ending("Немає депозитів");
                            continue;
                        }
                        // Виводимо інформацію
                        Ending($"Відкрито {bankAccount.Deposits.Count} депозити\\ів.\n Баланс депозитів: {bankAccount.AmountOnDeposits()} грн.");
                        break;
                    }
                    break;
                }
            case "5": 
                {
                    DateTime dt = MainObject.Accounts[0].Deposits[0].LastPayedDate;
                    TimeSpan ts = new TimeSpan(365, 0, 0, 0, 0);
                    dt -= ts;
                    MainObject.Accounts[0].Deposits[0].LastPayedDate = dt;
                    MainObject.InterestAccrual();
                    break;
                }
            case "6":  
                {
                    int rate, code, temp;
                    double val = 0.0;
                    string? input = null;
                    BankAccount? bankAccount = null;
                    Console.Clear();
                    while (true)
                    {
                        code = GetAccountNumber(ref bankAccount, MainObject); 
                        if (code == 1) break;
                        else if (code == 2) continue;

                        if (bankAccount.Amount < 100) 
                        {
                            Ending("Мінімальна потрібна сума 100");
                            continue;
                        }

                        while (true)
                        {
                            Console.WriteLine("Кількість грошей (мінімальна сума - 100):");
                            code = CheckNumber(ref input, ref val); 
                            if (code == 1) break;
                            else if (code == 2) continue;

                            if (val < 100 || val > bankAccount.Amount) 
                            {
                                Ending("Мінімальна сума - 100");
                                continue;
                            }
                            Console.WriteLine("Відсоткова ставка(6, 8, 10, 14):");
                            rate = Convert.ToInt32(Console.ReadLine());
                            if (rate != 6 && rate != 8 && rate != 10 && rate != 14) 
                            {
                                Ending("Відсоткова ставка(6, 8, 10, 14):");
                                continue;
                            }

                            bankAccount.Amount -= val;
                            temp = bankAccount.GetDepositID(MainObject); 
                            bankAccount.CreateDeposit(temp, val, rate); 
                            Ending($"Новий депозит відкрито");
                            break;
                        }
                        break;
                    }
                    break;
                }
            case "7": 
                {
                    int code;
                    int id = 0;
                    double val = 0.0;
                    string? input = null;
                    BankAccount? bankAccount = null; 
                    Deposits? deposits = null; 
                    Console.Clear();
                    while (true)
                    {                
                        code = GetAccountNumber(ref bankAccount, MainObject);
                        if (code == 1) break;
                        else if (code == 2) continue;
                        if (bankAccount.Deposits is null) 
                        {
                            Ending("Депозити відсутні");
                            continue;
                        }

                        while (true)
                        {
                            Console.WriteLine("Номер депозиту:");
                            code = CheckNumber(ref input, ref val); 
                            if (code == 1) break;
                            else if (code == 2) continue;
                            id = Convert.ToInt32(val);
                            deposits = bankAccount.FindDeposits(id); 
                            val = deposits.DepAmount;
                            bankAccount.DeleteDeposit(MainObject, deposits);
                            Ending($"Депозит був видалений");
                            break;
                        }
                        break;
                    }
                    break;
                }
            default:
                Ending("Неправильне введення");
                break;
        }

    }
}

void Ending(string text)
{
    Console.Clear();
    Console.WriteLine(text);
}

int GetAccountNumber(ref BankAccount? bankAccount, AccountsManager MainObject)
{
    int temp;
    Console.WriteLine("Введіть такі дані:\nНомер рахунку:");
    string? input = Console.ReadLine();
    if (input is null || !int.TryParse(input, out temp) || input.Length != 8) 
    {
        Ending("Неправильний формат даних");
        return 2;
    }
    bankAccount = MainObject.FindAccount(Convert.ToInt32(input));
    if (bankAccount == null)
    {
        Ending("Аккаунт не знайдено");
        return 2;
    }
    return 0;
}

int CheckNumber(ref string? input, ref double amount)
{
    int temp;
    input = Console.ReadLine();
    if (input is null || !int.TryParse(input, out temp) )
    {
        Ending("Неправильний формат даних");
        return 2;
    }
    amount = Convert.ToDouble(input);
    if (amount == 0) // Проверка на ввод данных
    {
        Ending("Неправильний формат даних");
        return 2;
    }
    return 0;
}

string ConvertNumber(in double number)
{
    StringBuilder sb = new StringBuilder();
    string num = number.ToString();
    for (int i = 0; i < 8 - num.Length; i++)
    {
        sb.Append('0');
    }
    sb.Append(num);
    return sb.ToString();
}